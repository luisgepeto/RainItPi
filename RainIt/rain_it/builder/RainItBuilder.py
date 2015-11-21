from abc import ABCMeta, abstractmethod
from builder.SourceSubject import SourceSubject

class RainItBuilder(metaclass = ABCMeta):
    
    def read_data_source(self, source_subject):
        if source_subject is SourceSubject.test_pattern:
            return self.get_test_pattern()
        elif source_subject is SourceSubject.test_routine:
            return self.get_test_routine()
        elif source_subject is SourceSubject.active_procedure:
            return self.get_active_procedure()
        else:
            pass
    
    @abstractmethod
    def get_test_pattern(self):
        pass
    
    @abstractmethod
    def get_test_routine(self):         
        pass
    
    @abstractmethod
    def get_active_procedure(self):
        pass    
    
    @abstractmethod
    def build_pattern(self, pattern_id = 0, conversion_parameter = None, matrix = None, path = None, pattern_factory = None):
        pass
    
    @abstractmethod
    def build_routine(self, pattern_list):
        pass
    
    @abstractmethod
    def build_procedure(self, routine_list):
        pass