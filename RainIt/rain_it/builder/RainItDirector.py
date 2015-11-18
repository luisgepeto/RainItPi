from builder.SourceSubject import SourceSubject

class RainItDirector(object):
    
    def __init__(self, rain_it_builder):
        self.rain_it_builder = rain_it_builder
    
    def get_test_pattern(self): 
        result = self.rain_it_builder.read_data_source(SourceSubject.test_pattern)        
        pattern_as_matrix = result["patternAsMatrix"]
        test_pattern = self.rain_it_builder.build_pattern(matrix = pattern_as_matrix)
        return test_pattern
    
    def get_test_routine(self):
        result = self.rain_it_builder.read_data_source(SourceSubject.test_routine)
        
    
    def get_active_procedure(self):
        self.rain_it_builder.read_data_source(SourceSubject.active_procedure)