from abc import ABCMeta, abstractmethod

class RainItComponent(metaclass = ABCMeta):
    
    def __init__(self):
        self.writers = []
        
    def add_writer(self, writer):
        self.writers.append(writer)
    
    def add_rain_it_component(self, rain_it_component):        
        pass
    
    @abstractmethod
    def file_write(self):        
        pass
    
    @abstractmethod
    def gpio_write(self):
        pass
    
    @abstractmethod
    def gpio_force_write(self):
        pass
                
    def get_writer_of_type(self, writer_type):
        for writer in self.writers:
            if type(writer) is writer_type:
                return writer