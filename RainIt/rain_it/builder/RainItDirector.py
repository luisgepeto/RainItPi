from builder.SourceSubject import SourceSubject
from ric.PatternFactory import PatternFactory

class RainItDirector(object):
    
    def __init__(self, rain_it_builder):
        self.rain_it_builder = rain_it_builder
        self.pattern_factory = PatternFactory()
    
    def get_test_pattern(self): 
        result = self.rain_it_builder.read_data_source(SourceSubject.test_pattern)        
        pattern_as_matrix = result["patternAsMatrix"]
        test_pattern = self.rain_it_builder.build_pattern(matrix = pattern_as_matrix, pattern_factory = self.pattern_factory)
        return test_pattern
    
    def get_test_routine(self):
        result = self.rain_it_builder.read_data_source(SourceSubject.test_routine)
        routine_dict = result[0]
        test_routine = self.get_routine_from_dict(routine_dict)
        return test_routine
    
    def get_active_procedure(self):
        result = self.rain_it_builder.read_data_source(SourceSubject.active_procedure)
        routines = []
        for routine_dict in result:
            routine = self.get_routine_from_dict(routine_dict)
            routines.append(routine)
        active_procedure = self.rain_it_builder.build_procedure(routines)
        return active_procedure
        
    def get_routine_from_dict(self, routine_dict):
        routine_id = routine_dict["RoutineId"]        
        routine_patterns = routine_dict["RoutinePatternDTOs"]
        patterns = []
        for routine_pattern in routine_patterns:
            repetitions = routine_pattern["Repetitions"]
            current_pattern = routine_pattern["PatternDTO"]
            current_pattern_id = current_pattern["PatternId"]
            current_pattern_path = current_pattern["Path"]
            current_pattern_conversion = current_pattern["ConversionParameterDTO"]
            pattern = self.rain_it_builder.build_pattern(pattern_id = current_pattern_id, conversion_parameter = current_pattern_conversion, path = current_pattern_path, pattern_factory = self.pattern_factory)
            for i in range(repetitions):
                patterns.append(pattern)                
        routine = self.rain_it_builder.build_routine(routine_id, patterns)
        return routine