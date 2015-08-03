'''
This is an example of a project source file.
All other source files must be in this directory

'''
from rain_it.hardware import hardware_manager
from rain_it.adapters import login_adapter
    
def authenticate_to_service():
    cpu_serial = hardware_manager.get_serial_number()
    authentication_response = login_adapter.authenticate(cpu_serial)
    return authentication_response
    
def initialize():
    authentication_response = authenticate_to_service()
    print(authentication_response.login_status)
    print(authentication_response.token_expiration_time)
    print(authentication_response.token)       
    
    
if __name__ == '__main__':
    initialize()
    
    
    
