from ric.RainItComponent import RainItComponent
from writer.FileWriter import FileWriter
from writer.GPIOWriter import GPIOWriter

class Pattern(RainItComponent):
    
    def __init__(self, name):
        super().__init__(name)
        
    def file_write(self):        
        file_writer = self.get_writer_of_type(FileWriter)
        file_writer.write(self)
    
    def gpio_write(self):
        gpio_writer = self.get_writer_of_type(GPIOWriter)
        gpio_writer.write(self)
    
    def gpio_force_write(self):
        gpio_writer = self.get_writer_of_type(GPIOWriter)
        gpio_writer.force_write(self)