from abc import ABCMeta, abstractmethod
from writer.FileWriter import FileWriter

class RainItComponent(metaclass = ABCMeta):
    
    def __init__(self):
        self.writers = []
        self.source_subject = None
        
    def set_source_subject(self, source_subject):
        self.source_subject = source_subject
        
    def add_writer(self, writer):
        self.writers.append(writer)
    
    def add_rain_it_component(self, rain_it_component):        
        pass
    
    def file_write(self):        
        file_writer = self.get_writer_of_type(FileWriter)
        if file_writer is not None:
            file_writer.write(self)
    
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
         
    @abstractmethod
    def should_pickle(self):
        pass
   
    @abstractmethod
    def get_pickle_dir(self):
        pass
    
    @abstractmethod
    def get_pickle_name(self):
        pass
    
    @abstractmethod
    def get_pickle_form(self):
        pass
    