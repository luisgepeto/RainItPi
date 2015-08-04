'''
This will contain functions that will interact with the raspberry hardware
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


def write_byte(byte):
    pass

def flush_line():
    pass

def print_matrix(matrix):
    matrix_width = len(matrix[0])
    total_byte_groups = math.ceil(matrix_width/8.0)
    for row in matrix:        
        for i in range(total_byte_groups):
            current_byte = row[8*i:8*i+8]
            write_byte(current_byte)
        flush_line()    