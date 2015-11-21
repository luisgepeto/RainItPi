from builder.RainItBuilder import RainItBuilder
from builder.SourceSubject import SourceSubject

class ServiceBuilder(RainItBuilder):

    base_url = "http://localhost:8723/api/"
        
    def read_data_source(self, source_subject):
        if source_subject is SourceSubject.test_pattern:
            pass            
        elif source_subject is SourceSubject.test_routine:
            pass
        elif source_subject is SourceSubject.active_procedure:
            pass
        else:
            pass
    
    def build_pattern(self, pattern_id = 0, conversion_parameter = None, matrix = None, path = None, pattern_factory = None):
        pass
    
    def build_routine(self, pattern_list):
        pass
    
    def build_procedure(self, routine_list):
        pass