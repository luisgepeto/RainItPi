'''
This is an example of a project source file.
All other source files must be in this directory
'''

from hardware import hardware_manager
from adapters import login_adapter, routine_adapter, pattern_adapter, device_adapter
from common import file_util
from domain.conversion_parameter import ConversionParameter
from domain.pattern import Pattern
from domain.routine_pattern import RoutinePattern
from domain.routine import Routine
from domain.settings import Settings
from multiprocessing import pool    
from domain.exceptions import RequestException
from datetime import datetime, timedelta
import ast

def get_routine_list_from_file(routine_root_dir):
    routine_list = []     
    try:
        if file_util.is_dir_existent(routine_root_dir):
            all_routines = file_util.get_all_dir_under(routine_root_dir)
            for routine_dir in all_routines:
                split_character = "/"
                if "\\" in routine_dir:
                    split_character="\\"
                routine_id =  routine_dir.split(split_character)[len(routine_dir.split(split_character))-1]                     
                all_routine_patterns = file_util.get_all_files_under(routine_dir)
                routine_pattern_list = []
                for routine_pattern in all_routine_patterns:
                    routine_pattern_id = routine_pattern.split("_")[0]
                    pattern_id = routine_pattern.split("_")[1]
                    repetitions = routine_pattern.split("_")[2]
                    r_weight = routine_pattern.split("_")[3]
                    g_weight = routine_pattern.split("_")[4]
                    b_weight = routine_pattern.split("_")[5]
                    is_inverted = routine_pattern.split("_")[6]
                    threshold_percentage = routine_pattern.split("_")[7]
                    conversion_parameter = ConversionParameter(r_weight, g_weight, b_weight, is_inverted, threshold_percentage)
                    current_pattern = Pattern(pattern_id, "", conversion_parameter.get_as_dictionary(), ast.literal_eval(file_util.read_file(routine_dir, routine_pattern)))                    
                    current_routine_pattern = RoutinePattern(routine_pattern_id, current_pattern.get_as_dictionary(), repetitions)
                    routine_pattern_list.append(current_routine_pattern.get_as_dictionary())
                current_routine = Routine(routine_id, routine_pattern_list, "")
                routine_list.append(current_routine)
    except:
        pass 
    return routine_list

def get_pattern_from_file(pattern_root_dir):
    try:
        all_patterns = file_util.get_all_files_under(pattern_root_dir)
        all_patterns.remove("timestamp")    
        all_patterns.remove("sample_timestamp")    
        if all_patterns:
            return Pattern(1, "", None, ast.literal_eval(file_util.read_file(pattern_root_dir, all_patterns[0])))
    except:
        pass
    return None

dir_key = "dir"
value_key = "value"
async_flag_key = "async_flag"
refresh_key = "refresh_key"
last_access_key = "last_access_key"
pool_key = "pool_key"
is_new_key = "is_new_key"

ar_dict = { dir_key: file_util.get_routine_root_path(),
                              value_key: get_routine_list_from_file(file_util.get_routine_root_path()),
                              async_flag_key: False}
tr_dict = { dir_key: file_util.get_test_routine_root_path(),
                              value_key: get_routine_list_from_file(file_util.get_test_routine_root_path()),
                              async_flag_key: False}
tp_dict = { dir_key: file_util.get_test_pattern_root_path(),
                              value_key: get_pattern_from_file(file_util.get_test_pattern_root_path()),
                              async_flag_key: False}
s_dict = {value_key: Settings(30, 500, 0), async_flag_key: False, last_access_key : None }
auth_dict = {value_key: None, async_flag_key: False, refresh_key:False}
out_dict = {async_flag_key: False, refresh_key:False, pool_key:None}

''' 
This is the callback section
'''
def get_authentication_callback(authentication_response):
    auth_dict[refresh_key] = False
    auth_dict[async_flag_key] = False
    auth_dict[value_key] = authentication_response
    
def get_settings_callback(result):
    s_dict[async_flag_key] = False
    s_dict[last_access_key] = datetime.utcnow()
    if result is None:
        auth_dict[refresh_key] = True
    else:
        if result[value_key] is not None and result[is_new_key]:
            s_dict[value_key] = result[value_key]
        out_dict[refresh_key] = result[is_new_key]
    
def get_active_routines_callback(result):
    ar_dict[async_flag_key] = False
    if result is None:
        auth_dict[refresh_key] = True
    else:
        if result[value_key] is not None and result[is_new_key]:
            ar_dict[value_key] = result[value_key]
        out_dict[refresh_key] = result[is_new_key]
        
def get_test_routine_callback(result):
    tr_dict[async_flag_key] = False
    if result is None:
        auth_dict[refresh_key] = True
    else:
        if result[value_key] is not None and result[is_new_key]:
            tr_dict[value_key] = result[value_key]
        out_dict[refresh_key] = result[is_new_key]

def get_test_pattern_callback(result):
    tp_dict[async_flag_key] = False
    if result is None:
        auth_dict[refresh_key] = True
    else:
        if result[is_new_key]:
            tp_dict[value_key] = result[value_key]
        out_dict[refresh_key] = result[is_new_key]        

'''
These functions will be called by their async partner
'''
def get_authentication():
    cpu_serial = hardware_manager.get_serial_number()
    authentication_result = login_adapter.authenticate(cpu_serial)
    return authentication_result

def get_settings(old_value, token):
    try:
        is_new_value = False
        settings = device_adapter.get_settings(token)
        if settings is not None:
            if not settings == old_value:
                is_new_value = True
    except RequestException:
        return None
    return {value_key: settings, is_new_key: is_new_value }   

def get_active_routines(old_value, token):
    try:
        is_new_value = False
        active_routines = routine_adapter.get_active_routines(token)
        resulting_dir = file_util.make_new_dir(ar_dict[dir_key])
        file_util.add_timestamp_file(resulting_dir)
        if active_routines is not None:            
            if not active_routines == old_value:
                is_new_value = True
                for routine in active_routines:
                    routine_path = file_util.make_new_dir_under(resulting_dir, str(routine.routine_id), True)
                    for routine_pattern in routine.routine_pattern_list:
                        pattern_as_matrix = pattern_adapter.get_pattern_as_matrix(routine_pattern.pattern.pattern_id,routine_pattern.pattern.conversion_parameter.get_as_dictionary(), token)
                        routine_pattern.pattern.set_matrix(pattern_as_matrix)
                        file_util.write_new_file(routine_path, str(routine_pattern.routine_pattern_id)+"_"+str(routine_pattern.pattern.pattern_id)+"_"+str(routine_pattern.repetitions)+"_"+str(routine_pattern.pattern.conversion_parameter.r_weight)+"_"+str(routine_pattern.pattern.conversion_parameter.g_weight)+"_"+str(routine_pattern.pattern.conversion_parameter.b_weight)+"_"+str(int(routine_pattern.pattern.conversion_parameter.is_inverted))+"_"+str(routine_pattern.pattern.conversion_parameter.threshold_percentage), str(pattern_as_matrix))        
    except RequestException:
        return False
    return {value_key: active_routines, is_new_key:is_new_value }

def get_test_routine(old_value, token):
    try:
        is_new_value = False
        test_routine_list = routine_adapter.get_test_routine_as_list(token)
        resulting_dir = file_util.make_new_dir(tr_dict[dir_key])
        file_util.add_timestamp_file(resulting_dir)
        if test_routine_list is not None:            
            if not test_routine_list == old_value:
                is_new_value = True
                for routine in test_routine_list:
                    routine_path = file_util.make_new_dir_under(resulting_dir, str(routine.routine_id), True)
                    for routine_pattern in routine.routine_pattern_list:
                        pattern_as_matrix = pattern_adapter.get_pattern_as_matrix(routine_pattern.pattern.pattern_id,routine_pattern.pattern.conversion_parameter.get_as_dictionary(), token)
                        routine_pattern.pattern.set_matrix(pattern_as_matrix)
                        file_util.write_new_file(routine_path, str(routine_pattern.routine_pattern_id)+"_"+str(routine_pattern.pattern.pattern_id)+"_"+str(routine_pattern.repetitions)+"_"+str(routine_pattern.pattern.conversion_parameter.r_weight)+"_"+str(routine_pattern.pattern.conversion_parameter.g_weight)+"_"+str(routine_pattern.pattern.conversion_parameter.b_weight)+"_"+str(int(routine_pattern.pattern.conversion_parameter.is_inverted))+"_"+str(routine_pattern.pattern.conversion_parameter.threshold_percentage), str(pattern_as_matrix))
                sample_time_stamp = None
                if test_routine_list:
                    sample_time_stamp = test_routine_list[0].time_stamp
                file_util.add_sample_timestamp_file(resulting_dir, sample_time_stamp)
    except RequestException:
        return False
    return {value_key: test_routine_list, is_new_key:is_new_value }
    
def get_test_pattern(old_value, token):
    try:
        is_new_value = False
        json_result = pattern_adapter.get_test_pattern_as_matrix(token)
        pattern_as_matrix = json_result["patternAsMatrix"]
        resulting_dir = file_util.make_new_dir(tp_dict[dir_key])
        file_util.add_timestamp_file(resulting_dir)
        if pattern_as_matrix is None or (old_value is not None and not pattern_as_matrix == old_value.matrix):
            is_new_value = True
            file_util.write_new_file(resulting_dir, "1_1_1_1_1_1_1_1", str(pattern_as_matrix))
            file_util.add_sample_timestamp_file(resulting_dir, json_result["SampleTimeStamp"])
    except RequestException:
        return False
    return {value_key: pattern_as_matrix, is_new_key:is_new_value }

def output_test(clock_delay, latch_delay):
    test_pattern_timestamp = file_util.get_sampletimestamp_from(tp_dict[dir_key])
    test_routine_timestamp = file_util.get_sampletimestamp_from(tr_dict[dir_key])
    if test_routine_timestamp > test_pattern_timestamp and tr_dict[value_key]:
        output_routine_list(tr_dict[value_key], clock_delay, latch_delay)
    elif test_routine_timestamp < test_pattern_timestamp and tp_dict[value_key] is not None:
        while True:
            hardware_manager.print_matrix(tp_dict[value_key].matrix, clock_delay, latch_delay)        
        
def output_routine_list(routine_list,clock_delay, latch_delay):
    routine_list.sort(key = lambda x: x.routine_id)
    while True:
        for routine in routine_list:        
            routine.routine_pattern_list.sort(key = lambda x: x.routine_pattern_id)
            for routine_pattern in routine.routine_pattern_list:
                for i in range(0, routine_pattern.repetitions):                
                    hardware_manager.print_matrix(routine_pattern.pattern.matrix, clock_delay, latch_delay)
                
'''
This is the async function section
'''
def get_authentication_async():
    p = pool.Pool(1)
    p.apply_async(get_authentication, callback=get_authentication_callback)
    p.close()
    return p

def get_settings_async():
    p = pool.Pool(1)
    p.apply_async(get_settings,[s_dict[value_key], auth_dict[value_key].token], callback=get_settings_callback)
    p.close()
    return p
    
def get_active_routines_async():
    p = pool.Pool(1)
    p.apply_async(get_active_routines,[ar_dict[value_key], auth_dict[value_key].token], callback=get_active_routines_callback)
    p.close()
    return p

def get_test_routine_async():
    p = pool.Pool(1)
    p.apply_async(get_test_routine,[tr_dict[value_key], auth_dict[value_key].token], callback=get_test_routine_callback)
    p.close()
    return p

def get_test_pattern_async():
    p = pool.Pool(1)
    p.apply_async(get_test_pattern,[tp_dict[value_key], auth_dict[value_key].token], callback=get_test_pattern_callback)
    p.close()
    return p
    
def output_test_async(clock_delay, latch_delay):
    p = pool.Pool(1)
    p.apply_async(output_test, [clock_delay, latch_delay])    
    return p
    
def output_routine_list_async(routine_list,clock_delay, latch_delay):
    p = pool.Pool(1)
    p.apply_async(output_routine_list, [routine_list,clock_delay, latch_delay])    
    return p
    
'''
This is the update function section
'''
def display():
    if out_dict[refresh_key] and out_dict[pool_key] is not None:
        out_dict[refresh_key] = False
        out_dict[async_flag_key] = False
        out_dict[pool_key].terminate()        
    if not out_dict[async_flag_key]:
        if tp_dict[value_key] is not None or tr_dict[value_key]:
            out_dict[async_flag_key] = True
            out_dict[pool_key] = output_test_async(s_dict[value_key].millisecond_clock_delay,s_dict[value_key].millisecond_latch_delay)
        elif ar_dict[value_key]:
            out_dict[async_flag_key] = True
            out_dict[pool_key] = output_routine_list_async(ar_dict[value_key],s_dict[value_key].millisecond_clock_delay,s_dict[value_key].millisecond_latch_delay)
            
def update_authentication():
    if (not is_auth_valid() or auth_dict[refresh_key]) and not auth_dict[async_flag_key]:
        auth_dict[async_flag_key] = True
        get_authentication_async()    

def update_settings():
    if not are_settings_valid() and not s_dict[async_flag_key] and is_auth_valid():
        s_dict[async_flag_key] = True
        get_settings_async()    
 
def update_active_routines():    
    if not file_util.is_dir_valid(ar_dict[dir_key], s_dict[value_key].minutes_refresh_rate) and not ar_dict[async_flag_key] and is_auth_valid():
        ar_dict[async_flag_key] = True
        get_active_routines_async()
    
def update_test_routine():
    if not file_util.is_dir_valid(tr_dict[dir_key], s_dict[value_key].minutes_refresh_rate) and not tr_dict[async_flag_key] and is_auth_valid():
        tr_dict[async_flag_key] = True
        get_test_routine_async()        

def update_test_pattern():
    if not file_util.is_dir_valid(tp_dict[dir_key], s_dict[value_key].minutes_refresh_rate) and not tp_dict[async_flag_key] and is_auth_valid():  
        tp_dict[async_flag_key] = True
        get_test_pattern_async()
  
'''
This is the output section
'''
def is_auth_valid():
    return (auth_dict[value_key] is not None 
            and auth_dict[value_key].token is not None 
            and not auth_dict[value_key].token == ""
            and not auth_dict[value_key].has_expired())

def are_settings_valid():
    return (s_dict[value_key] is not None
                and s_dict[last_access_key] is not None
                and s_dict[last_access_key]  + timedelta(minutes = s_dict[value_key].minutes_refresh_rate) > datetime.utcnow())
                                
def initialize():    
    while True:
        display()
        update_authentication()
        update_settings()
        update_active_routines()
        update_test_routine()
        update_test_pattern()        

if __name__ == '__main__':
    initialize()