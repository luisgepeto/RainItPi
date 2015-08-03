'''
This is an example of a project source file.
All other source files must be in this directory

'''
from rain_it.hardware import hardware_manager
from rain_it.adapters import login_adapter, routine_adapter, pattern_adapter
from datetime import datetime
from rain_it.domain.pattern import Pattern
from rain_it.domain.routine_pattern import RoutinePattern
from rain_it.common import file_util


    
def authenticate_to_service():
    cpu_serial = hardware_manager.get_serial_number()
    authentication_response = login_adapter.authenticate(cpu_serial)
    return authentication_response

def write_to_file_from_service():
    authentication_response = authenticate_to_service()
    if authentication_response.login_status == 1:
        active_routines = routine_adapter.get_active(authentication_response.token)        
        for routine in active_routines:            
            routine_path = file_util.make_new_dir(file_util.get_root_path(), "Routines\\"+str(routine.routine_id))
            for routine_pattern in routine.routine_pattern_list:
                pattern_as_matrix = pattern_adapter.get_pattern_as_matrix(routine_pattern.pattern.pattern_id, authentication_response.token)
                file_util.write_new_file(routine_path, str(routine_pattern.routine_pattern_id)+"_"+str(routine_pattern.repetitions), str(pattern_as_matrix))
                    
 
def initialize():
   write_to_file_from_service()
              
    

if __name__ == '__main__':
    initialize()
    
    
    
