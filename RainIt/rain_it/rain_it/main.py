'''
This is an example of a project source file.
All other source files must be in this directory

'''
from rain_it.hardware import hardware_manager
from rain_it.adapters import login_adapter, routine_adapter, pattern_adapter
from datetime import datetime
from rain_it.domain.pattern import Pattern
from rain_it.domain.routine_pattern import RoutinePattern


    
def authenticate_to_service():
    cpu_serial = hardware_manager.get_serial_number()
    authentication_response = login_adapter.authenticate(cpu_serial)
    return authentication_response
    
def initialize():
    authentication_response = authenticate_to_service()
    if authentication_response.login_status == 1:
        active_routines = routine_adapter.get_active(authentication_response.token)
        for routine in active_routines:
            for routine_pattern in routine.routine_pattern_list:
                for i in range(0, routine_pattern.repetitions):
                    pattern_as_matrix = pattern_adapter.get_pattern_as_matrix(routine_pattern.pattern.pattern_id, authentication_response.token)
                    print(pattern_as_matrix)            
    else:
        print ("Could not authenticate device information. Please try again")   
    print("Finished reading information for this device")                     
              
    
if __name__ == '__main__':
    initialize()
    
    
    
