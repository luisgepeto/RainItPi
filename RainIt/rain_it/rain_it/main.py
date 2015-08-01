'''
This is an example of a project source file.
All other source files must be in this directory

'''
from hardware import hardware_manager
from service_adapters import login_adapter

def initialize():
    cpu_serial = hardware_manager.get_serial_number()
    print(cpu_serial) 
    print(login_adapter.authenticate(cpu_serial))
    
    
    
