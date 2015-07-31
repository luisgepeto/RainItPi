'''
This will contain functions that will interact with the raspberry hardware
'''

def get_serial_number():
    cpu_serial = "0000000000000000"
    try:
        cpu_info_file = open('proc/cpuinfo', 'r')
        for line in cpu_info_file:
            if line[0:6] == "Serial":
                cpu_serial = line[10:26]
        cpu_info_file.close()
    except:
        cpu_serial = "ERROR00000000000"
    return cpu_serial