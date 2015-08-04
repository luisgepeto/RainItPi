'''
This will contain functions that will interact with the raspberry hardware
CLK refers to the clock that shifts the previous memory input to the next
LATCH refers to the clock that makes the internal state to be displayed
'''

import math

def get_serial_number():
    cpu_serial = "0000000000000000"
    try:
        cpu_info_file = open('proc/cpuinfo', 'r')
        for line in cpu_info_file:
            if line[0:6] == "Serial":
                cpu_serial = line[10:26]
        cpu_info_file.close()
    except:
        cpu_serial = "2607199105061990"
    return cpu_serial

def set_output_pin_state(bit):
    print(bit,)
    pass

def clk_positive_transition():
    pass

def latch_positive_transition():
    pass

def push_bit(bit):
    set_output_pin_state(bit)
    clk_positive_transition()
    
def write_line(line):
    for bit in line:
        push_bit(bit)
        #verify if after pushing bit we need to wait a while
    return True

def flush_line():
    latch_positive_transition()
    print("")
    return True

def print_matrix(matrix):        
    for row in matrix:        
        write_line(row)
        #missing setting of wait time between each line being displayed 
        flush_line()