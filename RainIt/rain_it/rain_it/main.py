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


    
def authenticate_to_service():
    cpu_serial = hardware_manager.get_serial_number()
    authentication_response = login_adapter.authenticate(cpu_serial)
    return authentication_response

def try_write_to_file_from_service(service_function, dir_path):    
    authentication_response = authenticate_to_service()
    if authentication_response.login_status == 1:
        active_routines = service_function(authentication_response.token)
        resulting_dir = file_util.make_new_dir(dir_path)        
        file_util.add_timestamp_file(resulting_dir)
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
    if routine_timestamp + timedelta(seconds = 10) < datetime.utcnow():
        return False    
    return True


def initialize():
    active_routines_dir = file_util.get_routine_root_path()
    active_routines = []    
    test_routine_dir = file_util.get_test_routine_root_path()
    test_routines = []
    while True:
        if test_routines:            
            output_routine_list(test_routines)
        elif active_routines:            
            output_routine_list(active_routines)
        if not is_dir_valid(active_routines_dir):            
            try_write_to_file_from_service(routine_adapter.get_active_routines, active_routines_dir)
            active_routines = get_routine_list_from_file(active_routines_dir)
        if not is_dir_valid(test_routine_dir):
            test_routines = []
            if try_write_to_file_from_service(routine_adapter.get_test_routine_as_list, test_routine_dir):                
                test_routines = get_routine_list_from_file(test_routine_dir)

if __name__ == '__main__':
    initialize()
    
    
    
