from ric.RainItComponent import RainItComponent
from writer.FileWriter import FileWriter
from writer.GPIOWriter import GPIOWriter

class Pattern(RainItComponent):
    
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