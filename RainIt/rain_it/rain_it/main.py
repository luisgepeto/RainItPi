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

def write_to_file_from_service():    
    authentication_response = authenticate_to_service()
    if authentication_response.login_status == 1:
        active_routines = routine_adapter.get_active(authentication_response.token)
        routines_dir = file_util.make_new_dir(file_util.get_routine_root_path())
        file_util.add_timestamp_file(routines_dir)
        for routine in active_routines:
            routine_path = file_util.make_new_dir_under(routines_dir, str(routine.routine_id))
            for routine_pattern in routine.routine_pattern_list:
                pattern_as_matrix = pattern_adapter.get_pattern_as_matrix(routine_pattern.pattern.pattern_id,routine_pattern.pattern.conversion_parameter.get_as_dictionary(), authentication_response.token)
                file_util.write_new_file(routine_path, str(routine_pattern.routine_pattern_id)+"_"+str(routine_pattern.pattern.pattern_id)+"_"+str(routine_pattern.repetitions), str(pattern_as_matrix))
    return True

def get_routine_list_from_file():
    routine_root_dir = file_util.get_routine_root_path()
    routine_list = []     
    if file_util.is_dir_existent(routine_root_dir):
        all_routines = file_util.get_all_dir_under(routine_root_dir)
        for routine_dir in all_routines:
            routine_id =  routine_dir.split("/")[len(routine_dir.split("/"))-1]                     
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

def is_routine_dir_valid():
    routine_root_dir = file_util.get_routine_root_path()
    routine_timestamp = file_util.get_timestamp_from(routine_root_dir)
    if routine_timestamp + timedelta(minutes = 5) < datetime.utcnow():
        print("Replacing file at", str(datetime.utcnow()))        
        return False    
    return True

def initialize():
    while True:
        if not is_routine_dir_valid():            
            write_to_file_from_service()
        output_routine_list(get_routine_list_from_file())    

if __name__ == '__main__':
    initialize()
    
    
    
