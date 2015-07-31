'''
This is an example of a project source file.
All other source files must be in this directory

'''
from rain_it.hardware import hardware_manager

def main():
    print("Hello World!")
    cpu_serial = hardware_manager.get_serial_number()
    print(cpu_serial) 
