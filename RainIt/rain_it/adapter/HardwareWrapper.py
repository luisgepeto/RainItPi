import os
from configparser import ConfigParser

try:
    import RPi.GPIO as GPIO
except ImportError:
    print("An error occurred importing GPIO")


class HardwareWrapper(object):
    def __init__(self):
        config = ConfigParser()
        config_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', "rainit.config")
        config.read(config_path)
        self.gpio_mode = config.getint("Hardware", "GPIOMode")
        self.gpio_out = config.getint("Hardware", "GPIOOutput")
        self.clock_channel = config.getint("Hardware", "ClockChannel")
        self.latch_channel = config.getint("Hardware", "LatchChannel")
        self.serial_channel = config.getint("Hardware", "SerialChannel")
        self.clock_transition = config.getint("Hardware", "ClockTransition")
        self.latch_transition = config.getint("Hardware", "LatchTransition")
        self.default_serial = config.get("Hardware", "DefaultSerial")

    def gpio_initialize(self):
        try:
            GPIO.setmode(self.gpio_mode)
            GPIO.setup(self.clock_channel, self.gpio_out)
            GPIO.setup(self.latch_channel, self.gpio_out)
            GPIO.setup(self.serial_channel, self.gpio_out)
        except:
            pass

    def write_serial(self, state):
        try:
            GPIO.output(self.serial_channel, state)
            GPIO.output(self.clock_channel, abs(self.clock_transition - 1))
            GPIO.output(self.clock_channel, self.clock_transition)
        except:
            pass
        print(state, end="", flush=True)

    def trigger_latch(self):
        try:
            GPIO.output(self.latch_channel, abs(self.latch_transition - 1))
            GPIO.output(self.latch_channel, self.latch_transition)
        except:
            pass
        print()

    def get_serial_number(self):
        cpu_serial = "0000000000000000"
        try:
            f = open('/proc/cpuinfo', 'r')
            for line in f:
                if line[0:6] == 'Serial':
                    cpu_serial = line[10:26]
            f.close()
        except:
            cpu_serial = self.default_serial
        return cpu_serial

    def gpio_cleanup(self):
        try:
            GPIO.cleanup()
        except:
            pass

