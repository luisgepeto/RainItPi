from ric.RainItComponent import RainItComponent
from writer.FileWriter import FileWriter
from writer.GPIOWriter import GPIOWriter
from interfaces.IExpirable import IExpirable


class Pattern(RainItComponent):
    def __init__(self, pattern_id, conversion_parameter, matrix):
        super().__init__()
        self.pattern_id = pattern_id
        self.conversion_parameter = conversion_parameter
        self.matrix = matrix

    def file_write(self):
        file_writer = self.get_writer_of_type(FileWriter)
        if file_writer is not None:
            file_writer.write(self)

    def gpio_write(self, device_settings, hardware_wrapper):
        gpio_writer = self.get_writer_of_type(GPIOWriter)
        if gpio_writer is not None:
            gpio_writer.write(self, device_settings, hardware_wrapper)

    def gpio_force_write(self, device_settings, hardware_wrapper):
        gpio_writer = self.get_writer_of_type(GPIOWriter)
        if gpio_writer is not None:
            gpio_writer.force_write(self, device_settings, hardware_wrapper)

    def get_pickle_form(self):
        return self

    def __eq__(self, other):
        pattern_equal = other is not None \
            and self.pattern_id == other.pattern_id \
            and self.conversion_parameter == other.conversion_parameter
        if pattern_equal:
            if isinstance(self, IExpirable) and isinstance(other, IExpirable):
                return other is not None and self.time_stamp == other.time_stamp
        return pattern_equal
