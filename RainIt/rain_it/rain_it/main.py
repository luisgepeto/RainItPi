'''
This is an example of a project source file.
All other source files must be in this directory
'''

from hardware import hardware_manager
from adapters import login_adapter, routine_adapter, pattern_adapter
from common import file_util
from domain.pattern import Pattern
from domain.routine_pattern import RoutinePattern
from domain.routine import Routine
from domain import routine_pattern
from multiprocessing import pool     

dir_key = "dir"
value_key = "value"
async_flag_key = "async_flag"
ar_dict = { dir_key: file_util.get_routine_root_path(),
                              value_key: [],
                              async_flag_key: False}
tr_dict = { dir_key: file_util.get_test_routine_root_path(),
                              value_key: [],
                              async_flag_key: False}
tp_dict = { dir_key: file_util.get_test_pattern_root_path(),
                              value_key: None,
                              async_flag_key: False}

def initialize():    
    while True:
        if tp_dict[value_key] is not None or tr_dict[value_key]:
            print_tests()        
        elif ar_dict[value_key]:            
            output_routine_list(ar_dict[value_key])        
        update_active_routine_dict()
        update_test_routine_dict()
        update_test_pattern_dict()        

if __name__ == '__main__':
    initialize()
    
def print_tests():
    test_pattern_timestamp = file_util.get_sampletimestamp_from(tp_dict[dir_key])
    test_routine_timestamp = file_util.get_sampletimestamp_from(tr_dict[dir_key])
    if test_routine_timestamp > test_pattern_timestamp and tr_dict[value_key]:
        output_routine_list(tr_dict[value_key])
    elif test_routine_timestamp < test_pattern_timestamp and tp_dict[value_key] is not None:
        hardware_manager.print_matrix(tp_dict[value_key].pattern_as_matrix)
        
def output_routine_list(routine_list):
    routine_list.sort(key = lambda x: x.routine_id)
    for routine in routine_list:        
        routine.routine_pattern_list.sort(key = lambda x: x.routine_pattern_id)
        for routine_pattern in routine.routine_pattern_list:
            for i in range(0, routine_pattern.repetitions):                
                hardware_manager.print_matrix(routine_pattern.pattern.pattern_as_matrix)
    return True 

def update_active_routine_dict():
    if not is_dir_valid(ar_dict[dir_key]) and not ar_dict[async_flag_key]:
        ar_dict[async_flag_key] = True
        try_write_routine_to_file_from_service_async(routine_adapter.get_active_routines, ar_dict[dir_key], update_active_routine_callback)      
    
def update_test_routine_dict():
    if not is_dir_valid(tr_dict[dir_key]) and not tr_dict[async_flag_key]:
        tr_dict[async_flag_key] = True
        try_write_routine_to_file_from_service_async(routine_adapter.get_test_routine_as_list, tr_dict[dir_key], update_test_routine_callback)    

def update_test_pattern_dict():
    if not is_dir_valid(tp_dict[dir_key]) and not tp_dict[async_flag_key]:                        
        try_write_pattern_to_file_from_service_async(pattern_adapter.get_test_pattern_as_matrix, tp_dict[dir_key], update_test_pattern_callback)
        
def update_active_routine_callback():
    ar_dict[async_flag_key] = False
    ar_dict[value_key] = get_routine_list_from_file(ar_dict[dir_key])
    
def update_test_routine_callback():
    tr_dict[async_flag_key] = False
    tr_dict[value_key] = get_routine_list_from_file(tr_dict[dir_key])
    
def update_test_pattern_callback():
    tp_dict[async_flag_key] = False
    tp_dict[value_key] = get_pattern_from_file(tp_dict[dir_key])
        
def try_write_routine_to_file_from_service_async(service_function, dir_path, callback_function):
    p = pool.Pool(1)
    p.apply_async(try_write_routine_to_file_from_service, [service_function, dir_path], callback=callback_function)
    
def try_write_routine_to_file_from_service(service_function, dir_path):    
    authentication_response = authenticate_to_service()
    if authentication_response.login_status == 1:
        active_routines = service_function(authentication_response.token)
        resulting_dir = file_util.make_new_dir(dir_path)
        sample_time_stamp = None
        if active_routines:
            sample_time_stamp = active_routines[0].time_stamp        
        file_util.add_timestamp_file(resulting_dir)
        file_util.add_sample_timestamp_file(resulting_dir, sample_time_stamp)
        for routine in active_routines:
            routine_path = file_util.make_new_dir_under(resulting_dir, str(routine.routine_id))
            for routine_pattern in routine.routine_pattern_list:
                pattern_as_matrix = pattern_adapter.get_pattern_as_matrix(routine_pattern.pattern.pattern_id,routine_pattern.pattern.conversion_parameter.get_as_dictionary(), authentication_response.token)
                file_util.write_new_file(routine_path, str(routine_pattern.routine_pattern_id)+"_"+str(routine_pattern.pattern.pattern_id)+"_"+str(routine_pattern.repetitions), str(pattern_as_matrix))
    else:
        return False               
    return True

def try_write_pattern_to_file_from_service_async(service_function, dir_path, callback_function):
    p = pool.Pool(1)
    p.apply_async(try_write_pattern_to_file_from_service, [service_function, dir_path], callback=callback_function)
    
def try_write_pattern_to_file_from_service(service_function, dir_path):
    authentication_response = authenticate_to_service()
    if authentication_response.login_status == 1:
        json_result = service_function(authentication_response.token)
        pattern_as_matrix = json_result["patternAsMatrix"]
        resulting_dir = file_util.make_new_dir(dir_path)        
        file_util.add_timestamp_file(resulting_dir)
        file_util.add_sample_timestamp_file(resulting_dir, json_result["SampleTimeStamp"])
        if pattern_as_matrix is not None:
            file_util.write_new_file(resulting_dir, "1_1_1", str(pattern_as_matrix))                        
    else:
        return False            
    return True

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

def authenticate_to_service():
    cpu_serial = hardware_manager.get_serial_number()
    authentication_response = login_adapter.authenticate(cpu_serial)
    return authentication_response
        
        
        