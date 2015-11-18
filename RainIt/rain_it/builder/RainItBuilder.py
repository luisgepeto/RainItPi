from abc import ABCMeta, abstractmethod

class RainItBuilder(metaclass = ABCMeta):
    
    @abstractmethod
    def read_data_source(self, source_subject):
        pass
    
    @abstractmethod
    def build_pattern(self, pattern_id = 0, conversion_parameter = None, matrix = None):
        pass
    
    @abstractmethod
    def build_routine(self, pattern_list):
        pass
    
    @abstractmethod
    def build_procedure(self, routine_list):
        pass