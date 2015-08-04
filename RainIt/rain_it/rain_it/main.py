'''
This is an example of a project source file.
All other source files must be in this directory

'''
from rain_it.hardware import hardware_manager
from rain_it.adapters import login_adapter, routine_adapter, pattern_adapter
from datetime import datetime
from rain_it.common import file_util
from rain_it.domain.pattern import Pattern
from rain_it.domain.routine_pattern import RoutinePattern
from rain_it.domain.routine import Routine

    
def authenticate_to_service():
    cpu_serial = hardware_manager.get_serial_number()
    authentication_response = login_adapter.authenticate(cpu_serial)
    return authentication_response

def write_to_file_from_service():
    authentication_response = authenticate_to_service()
    if authentication_response.login_status == 1:
        active_routines = routine_adapter.get_active(authentication_response.token)
        routines_dir = file_util.make_new_dir(file_util.get_routine_root_path())
        for routine in active_routines:
            routine_path = file_util.make_new_dir_under(routines_dir, str(routine.routine_id))
            for routine_pattern in routine.routine_pattern_list:
                pattern_as_matrix = pattern_adapter.get_pattern_as_matrix(routine_pattern.pattern.pattern_id,routine_pattern.pattern.conversion_parameter.get_as_dictionary(), authentication_response.token)
                file_util.write_new_file(routine_path, str(routine_pattern.routine_pattern_id)+"_"+str(routine_pattern.pattern.pattern_id)+"_"+str(routine_pattern.repetitions), str(pattern_as_matrix))
                

def get_routine_list_from_file():
    routine_list = []
    routine_root_dir = file_util.get_routine_root_path()
    if file_util.is_dir_existent(routine_root_dir):
        all_routines = file_util.get_all_dir_under(routine_root_dir)
        for routine_dir in all_routines:
            routine_id =  routine_dir.split("\\")[len(routine_dir.split("\\"))-1]                     
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
 
 

def initialize():        
    #write_to_file_from_service()
    get_routine_list_from_file()
              
    

if __name__ == '__main__':
    initialize()
    
    
    
