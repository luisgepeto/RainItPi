'''
This is an example of a project source file.
All other source files must be in this directory
'''

from hardware import hardware_manager
from adapters import login_adapter, routine_adapter, pattern_adapter, device_adapter
from common import file_util
from domain.pattern import Pattern
from domain.routine_pattern import RoutinePattern
from domain.routine import Routine
from domain.settings_response import Settings
from domain import routine_pattern
from multiprocessing import pool    
from domain.exceptions import RequestException
from datetime import datetime, timedelta


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
                    current_pattern = Pattern(pattern_id, file_util.read_file(routine_dir, routine_pattern))
                    current_routine_pattern = RoutinePattern(routine_pattern_id, repetitions, current_pattern)
                    routine_pattern_list.append(current_routine_pattern)
                current_routine = Routine(routine_id, routine_pattern_list)
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
            return Pattern(1, file_util.read_file(pattern_root_dir, all_patterns[0]))
    except:
        pass
    return None

dir_key = "dir"
value_key = "value"
async_flag_key = "async_flag"
refresh_key = "refresh_key"
last_access_key = "last_access_key"
test_pool_key = "test pool key"
routine_pool_key = "routine pool key"

ar_dict = { dir_key: file_util.get_routine_root_path(),
                              value_key: get_routine_list_from_file(file_util.get_routine_root_path()),
                              async_flag_key: False}
tr_dict = { dir_key: file_util.get_test_routine_root_path(),
                              value_key: get_routine_list_from_file(file_util.get_test_routine_root_path()),
                              async_flag_key: False}
tp_dict = { dir_key: file_util.get_test_pattern_root_path(),
                              value_key: get_pattern_from_file(file_util.get_test_pattern_root_path()),
                              async_flag_key: False}
s_dict = {value_key: Settings(30, 0, 0, False), async_flag_key: False, last_access_key : None }
auth_dict = {value_key: None, async_flag_key: False, refresh_key:False}
out_dict = {async_flag_key: False, refresh_key:False, test_pool_key:None, routine_pool_key: None}

''' 
This is the callback section
'''
def authenticate_to_service_callback(authentication_response):
    auth_dict[refresh_key] = False
    auth_dict[async_flag_key] = False
    auth_dict[value_key] = authentication_response
    
def get_settings_callback(result):
    auth_dict[refresh_key] = not result
    s_dict[async_flag_key] = False
    s_dict[last_access_key] = datetime.utcnow()
    if result is not None:
        s_dict[value_key] = result
        if s_dict[value_key].test_mode:
            out_dict[refresh_key] = True
    
def write_active_routine_to_file_callback(result):
    auth_dict[refresh_key] = not result    
    ar_dict[async_flag_key] = False
    ar_dict[value_key] = get_routine_list_from_file(ar_dict[dir_key])
    
def write_test_routine_to_file_callback(result):
    auth_dict[refresh_key] = not result
    tr_dict[async_flag_key] = False
    tr_dict[value_key] = get_routine_list_from_file(tr_dict[dir_key])

def write_test_pattern_to_file_callback(result):
    auth_dict[refresh_key] = not result
    tp_dict[async_flag_key] = False
    tp_dict[value_key] = get_pattern_from_file(tp_dict[dir_key])
        
def output_test_callback(result):
    out_dict[async_flag_key] = False
    
def output_routine_list_callback(result):
    out_dict[async_flag_key] = False

    
'''
These functions will be called by their async partner
'''
def authenticate_to_service():
    cpu_serial = hardware_manager.get_serial_number()
    authentication_result = login_adapter.authenticate(cpu_serial)
    return authentication_result

def get_settings(token):
    settings = device_adapter.get_settings(token)
    return settings

def write_active_routine_to_file(token):
    try:
        active_routines = routine_adapter.get_active_routines(token)
        resulting_dir = file_util.make_new_dir(ar_dict[dir_key])
        for routine in active_routines:
            routine_path = file_util.make_new_dir_under(resulting_dir, str(routine.routine_id))
            for routine_pattern in routine.routine_pattern_list:
                pattern_as_matrix = pattern_adapter.get_pattern_as_matrix(routine_pattern.pattern.pattern_id,routine_pattern.pattern.conversion_parameter.get_as_dictionary(), token)
                file_util.write_new_file(routine_path, str(routine_pattern.routine_pattern_id)+"_"+str(routine_pattern.pattern.pattern_id)+"_"+str(routine_pattern.repetitions), str(pattern_as_matrix))
        file_util.add_timestamp_file(resulting_dir)
    except RequestException:
        return False
    return True

def write_test_routine_to_file(token):
    try:
        active_routines = routine_adapter.get_test_routine_as_list(token)
        resulting_dir = file_util.make_new_dir(tr_dict[dir_key])
        for routine in active_routines:
            routine_path = file_util.make_new_dir_under(resulting_dir, str(routine.routine_id))
            for routine_pattern in routine.routine_pattern_list:
                pattern_as_matrix = pattern_adapter.get_pattern_as_matrix(routine_pattern.pattern.pattern_id,routine_pattern.pattern.conversion_parameter.get_as_dictionary(), token)
                file_util.write_new_file(routine_path, str(routine_pattern.routine_pattern_id)+"_"+str(routine_pattern.pattern.pattern_id)+"_"+str(routine_pattern.repetitions), str(pattern_as_matrix))
        sample_time_stamp = None
        if active_routines:
            sample_time_stamp = active_routines[0].time_stamp        
        file_util.add_timestamp_file(resulting_dir)
        file_util.add_sample_timestamp_file(resulting_dir, sample_time_stamp)
    except RequestException:
        return False
    return True
    
def write_test_pattern_to_file(token):
    try:
        json_result = pattern_adapter.get_test_pattern_as_matrix(token)
        pattern_as_matrix = json_result["patternAsMatrix"]
        resulting_dir = file_util.make_new_dir(tp_dict[dir_key])
        if pattern_as_matrix is not None:
            file_util.write_new_file(resulting_dir, "1_1_1", str(pattern_as_matrix))
        file_util.add_timestamp_file(resulting_dir)
        file_util.add_sample_timestamp_file(resulting_dir, json_result["SampleTimeStamp"])
    except RequestException:
        return False
    return True

def output_test(clock_delay, latch_delay):
    test_pattern_timestamp = file_util.get_sampletimestamp_from(tp_dict[dir_key])
    test_routine_timestamp = file_util.get_sampletimestamp_from(tr_dict[dir_key])
    if test_routine_timestamp > test_pattern_timestamp and tr_dict[value_key]:
        output_routine_list(tr_dict[value_key])
    elif test_routine_timestamp < test_pattern_timestamp and tp_dict[value_key] is not None:
        hardware_manager.print_matrix(tp_dict[value_key].pattern_as_matrix, clock_delay, latch_delay)
        
def output_routine_list(routine_list,clock_delay, latch_delay):
    routine_list.sort(key = lambda x: x.routine_id)
    for routine in routine_list:        
        routine.routine_pattern_list.sort(key = lambda x: x.routine_pattern_id)
        for routine_pattern in routine.routine_pattern_list:
            for i in range(0, routine_pattern.repetitions):                
                hardware_manager.print_matrix(routine_pattern.pattern.pattern_as_matrix, clock_delay, latch_delay)
                
'''
This is the async function section
'''
def authenticate_to_service_async():
    p = pool.Pool(1)
    p.apply_async(authenticate_to_service, callback=authenticate_to_service_callback)
    p.close()
    return p

def get_settings_async():
    p = pool.Pool(1)
    p.apply_async(get_settings,[auth_dict[value_key].token], callback=get_settings_callback)
    p.close()
    return p
    
def write_active_routine_to_file_async():
    p = pool.Pool(1)
    p.apply_async(write_active_routine_to_file,[auth_dict[value_key].token], callback=write_active_routine_to_file_callback)
    p.close()
    return p
    
def write_test_routine_to_file_async():
    p = pool.Pool(1)
    p.apply_async(write_test_routine_to_file,[auth_dict[value_key].token], callback=write_test_routine_to_file_callback)
    p.close()
    return p

def write_test_pattern_to_file_async():
    p = pool.Pool(1)
    p.apply_async(write_test_pattern_to_file,[auth_dict[value_key].token], callback=write_test_pattern_to_file_callback)
    p.close()
    return p
    
def output_test_async(clock_delay, latch_delay):
    p = pool.Pool(1)
    p.apply_async(output_test, [clock_delay, latch_delay],callback=output_test_callback)
    p.close()
    return p
    
def output_routine_list_async(routine_list,clock_delay, latch_delay):
    p = pool.Pool(1)
    p.apply_async(output_routine_list, [routine_list,clock_delay, latch_delay], callback=output_routine_list_callback)
    p.close()
    return p
    
'''
This is the update function section
'''
    
def display_test():
    if out_dict[refresh_key] and out_dict[test_pool_key] is not None:
        out_dict[refresh_key] = False
        out_dict[async_flag_key] = False
        out_dict[test_pool_key].terminate()
    if (tp_dict[value_key] is not None or tr_dict[value_key]) and not out_dict[async_flag_key]:
        out_dict[async_flag_key] = True
        out_dict[test_pool_key] = output_test_async(s_dict[value_key].millisecond_clock_delay,s_dict[value_key].millisecond_latch_delay)
    
def display_routines():
    if out_dict[refresh_key] and out_dict[routine_pool_key] is not None:
        out_dict[refresh_key] = False
        out_dict[async_flag_key] = False
        out_dict[routine_pool_key].terminate()
    if tp_dict[value_key] is None and not tr_dict[value_key] and ar_dict[value_key] and not out_dict[async_flag_key]:
        out_dict[async_flag_key] = True
        out_dict[routine_pool_key] = output_routine_list_async(ar_dict[value_key],s_dict[value_key].millisecond_clock_delay,s_dict[value_key].millisecond_latch_delay)
            
def update_auth():
    if (not is_auth_valid() or auth_dict[refresh_key]) and not auth_dict[async_flag_key]:
        auth_dict[async_flag_key] = True
        authenticate_to_service_async()    

def update_settings():
    if not are_settings_valid() and not s_dict[async_flag_key] and is_auth_valid():
        s_dict[async_flag_key] = True
        get_settings_async()    
 
def update_active_routine_dict():    
    if not file_util.is_dir_valid(ar_dict[dir_key], s_dict[value_key].minutes_refresh_rate) and not ar_dict[async_flag_key] and is_auth_valid():
        ar_dict[async_flag_key] = True
        write_active_routine_to_file_async()              
    
def update_test_routine_dict():
    if not file_util.is_dir_valid(tr_dict[dir_key], s_dict[value_key].minutes_refresh_rate) and not tr_dict[async_flag_key] and is_auth_valid():
        tr_dict[async_flag_key] = True
        write_test_routine_to_file_async()

def update_test_pattern_dict():
    if not file_util.is_dir_valid(tp_dict[dir_key], s_dict[value_key].minutes_refresh_rate) and not tp_dict[async_flag_key] and is_auth_valid():  
        tp_dict[async_flag_key] = True                      
        write_test_pattern_to_file_async()
  
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
        display_test()
        display_routines()
        update_auth()
        update_settings()
        update_active_routine_dict()
        update_test_routine_dict()
        update_test_pattern_dict()        

if __name__ == '__main__':
    initialize()