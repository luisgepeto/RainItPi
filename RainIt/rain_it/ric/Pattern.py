from ric.RainItComponent import RainItComponent
from writer.FileWriter import FileWriter
from writer.GPIOWriter import GPIOWriter

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
    
    def gpio_write(self):
        gpio_writer = self.get_writer_of_type(GPIOWriter)
        if gpio_writer is not None:            
            gpio_writer.write(self)
    
    def gpio_force_write(self):
        gpio_writer = self.get_writer_of_type(GPIOWriter)
        if gpio_writer is not None:
            gpio_writer.force_write(self)
            
    def get_pickle_name(self):
        return '{}_{}_{}_{}_{}_{}'.format(self.pattern_id, self.conversion_parameter.r_weight, self.conversion_parameter.g_weight, self.conversion_parameter.b_weight , self.conversion_parameter.is_inverted, self.conversion_parameter.threshold_percentage)
    
    def get_pickle_form(self):
        return self
    