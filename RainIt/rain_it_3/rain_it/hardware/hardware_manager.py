'''
This will contain functions that will interact with the raspberry hardware
CLK refers to the clock that shifts the previous memory input to the next
LATCH refers to the clock that makes the internal state to be displayed
'''

from time import sleep

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
    print(int(bit), end="")
    pass

def clk_positive_transition():
    pass

def latch_positive_transition():
    pass

def push_bit(bit):
    set_output_pin_state(bit)
    clk_positive_transition()
    
def write_line(line, clock_delay):
    for bit in line:
        push_bit(bit)
        sleep(float(clock_delay) / 1000.0)
    return True

def flush_line():
    latch_positive_transition()
    print("")
    return True

def print_matrix(matrix, clock_delay, latch_delay):        
    for row in matrix:        
        write_line(row, clock_delay)
        flush_line()
        sleep(float(latch_delay) / 1000.0)