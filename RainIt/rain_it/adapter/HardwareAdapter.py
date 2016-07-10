class HardwareAdapter(object):

    def __init__(self):
        pass

    def get_serial(self):
        cpu_serial = "0000000000000000"
        try:
            f = open('/proc/cpuinfo', 'r')
            for line in f:
                if line[0:6] == 'Serial':
                    cpu_serial = line[10:26]
            f.close()
        except:
            cpu_serial = "SERIALERROR00000"
        return cpu_serial