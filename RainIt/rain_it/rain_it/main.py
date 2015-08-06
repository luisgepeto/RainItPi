'''
This is an example of a project source file.
All other source files must be in this directory
'''

from hardware import hardware_manager
from adapters import login_adapter, routine_adapter, pattern_adapter
from datetime import datetime, timedelta
from common import file_util
from domain.pattern import Pattern
from domain.routine_pattern import RoutinePattern
from domain.routine import Routine
from domain import routine_pattern
from common.file_util import get_sampletimestamp_from


    
def authenticate_to_service():
    cpu_serial = hardware_manager.get_serial_number()
    authentication_response = login_adapter.authenticate(cpu_serial)
    return authentication_response

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

def get_pattern_from_file(pattern_root_dir):
    all_patterns = file_util.get_all_files_under(pattern_root_dir)
    all_patterns.remove("timestamp")    
    all_patterns.remove("sample_timestamp")    
    if all_patterns:
        return Pattern(1, file_util.read_file(pattern_root_dir, all_patterns[0]))
    return None
        

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

def get_routine_list_from_file(routine_root_dir):
    routine_list = []     
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
    return routine_list
 
def output_routine_list(routine_list):
    for routine in routine_list:
        for routine_pattern in routine.routine_pattern_list:
            for i in range(0, routine_pattern.repetitions):                
                hardware_manager.print_matrix(routine_pattern.pattern.pattern_as_matrix)
    return True 

def is_dir_valid(dir_path):    
    routine_timestamp = file_util.get_timestamp_from(dir_path)
    if routine_timestamp + timedelta(minutes = 5) < datetime.utcnow():
        return False    
    return True

def update_routine_dir(active_routines_dir, previous_active_routines):
    if not is_dir_valid(active_routines_dir):            
            try_write_routine_to_file_from_service(routine_adapter.get_active_routines, active_routines_dir)
            active_routines = get_routine_list_from_file(active_routines_dir)
            return active_routines
    return previous_active_routines

def update_test_routine_dir(test_routines_dir, previous_test_routines):
    if not is_dir_valid(test_routines_dir):
            test_routines = []
            if try_write_routine_to_file_from_service(routine_adapter.get_test_routine_as_list, test_routines_dir):                
                test_routines = get_routine_list_from_file(test_routines_dir)
            return test_routines
    return previous_test_routines

def update_test_pattern_dir(test_patterns_dir, previous_test_pattern):
    if not is_dir_valid(test_patterns_dir):
            test_pattern = None             
            if try_write_pattern_to_file_from_service(pattern_adapter.get_test_pattern_as_matrix, test_patterns_dir):                
                test_pattern = get_pattern_from_file(test_patterns_dir)
            return test_pattern
    return previous_test_pattern

def print_tests(test_routines, test_routines_dir, test_pattern, test_patterns_dir):
    test_pattern_timestamp = get_sampletimestamp_from(test_patterns_dir)
    test_routine_timestamp = get_sampletimestamp_from(test_routines_dir)
    if test_routine_timestamp > test_pattern_timestamp and test_routines:
        output_routine_list(test_routines)
    elif test_pattern is not None:
        hardware_manager.print_matrix(test_pattern.pattern_as_matrix)
    
def initialize():
    active_routines_dir = file_util.get_routine_root_path()
    test_routines_dir = file_util.get_test_routine_root_path()
    test_patterns_dir = file_util.get_test_pattern_root_path()
    active_routines = []
    test_routines = []
    test_pattern = None
    while True:
        if test_pattern is not None or test_routines:
            print_tests(test_routines, test_routines_dir, test_pattern, test_patterns_dir)        
        elif active_routines:            
            output_routine_list(active_routines)
        active_routines = update_routine_dir(active_routines_dir, active_routines)
        test_routines = update_test_routine_dir(test_routines_dir, test_routines)
        test_pattern = update_test_pattern_dir(test_patterns_dir, test_pattern)
        

if __name__ == '__main__':
    initialize()
    
    
    
