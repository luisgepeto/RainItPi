from ric.PatternFactory import PatternFactory
from builder.ComponentType import ComponentType

class RainItDirector(object):
    
    def __init__(self, rain_it_builder, writers):
        self.rain_it_builder = rain_it_builder
        self.pattern_factory = PatternFactory()
        self.writers = writers
    
    def get_test_pattern(self): 
        result = self.rain_it_builder.read_data_source(ComponentType.test_pattern)        
        pattern_as_matrix = result["patternAsMatrix"]        
        pattern_result = self.rain_it_builder.build_pattern(matrix = pattern_as_matrix, pattern_factory = self.pattern_factory, component_type = ComponentType.test_pattern)        
        return self.add_writers(pattern_result)
                
    
    def get_test_routine(self):
        result = self.rain_it_builder.read_data_source(ComponentType.test_routine)
        routine_dict = None 
        if result:
            routine_dict = result[0]
        routine_result = self.get_routine_from_dict(routine_dict, ComponentType.test_routine)
        return self.add_writers(routine_result)
                
    
    def get_active_procedure(self):
        result = self.rain_it_builder.read_data_source(ComponentType.active_procedure)        
        routines = []
        for routine_dict in result:
            routine = self.get_routine_from_dict(routine_dict, ComponentType.active_procedure)
            routines.append(routine)
        procedure_result = self.rain_it_builder.build_procedure(routines, ComponentType.active_procedure)
        return self.add_writers(procedure_result)     
        
    def get_routine_from_dict(self, routine_dict, component_type):
        routine_id = 0
        patterns = []        
        if routine_dict is not None:
            routine_id = routine_dict["RoutineId"]        
            routine_patterns = routine_dict["RoutinePatternDTOs"]            
            for routine_pattern in routine_patterns:
                repetitions = routine_pattern["Repetitions"]
                current_pattern = routine_pattern["PatternDTO"]
                current_pattern_id = current_pattern["PatternId"]
                current_pattern_path = current_pattern["Path"]
                current_conversion_dict = current_pattern["ConversionParameterDTO"]
                current_conversion  = self.get_conversion_parameter_from_dict(current_conversion_dict)
                pattern = self.rain_it_builder.build_pattern(pattern_id = current_pattern_id, conversion_parameter = current_conversion, path = current_pattern_path, pattern_factory = self.pattern_factory, component_type = component_type)
                for i in range(repetitions):
                    patterns.append(pattern)                
        routine = self.rain_it_builder.build_routine(routine_id, patterns, component_type)
        return routine
    
    def get_conversion_parameter_from_dict(self, conversion_parameter_dict):
        r_weight = conversion_parameter_dict["RWeight"]
        g_weight = conversion_parameter_dict["GWeight"]
        b_weight = conversion_parameter_dict["BWeight"]
        is_inverted = conversion_parameter_dict["IsInverted"]
        threshold_percentage = conversion_parameter_dict["ThresholdPercentage"]
        conversion_parameter = self.rain_it_builder.build_conversion_parameter(r_weight, g_weight, b_weight, is_inverted, threshold_percentage)
        return conversion_parameter
    
    def add_writers(self, rain_it_component):
        for writer in self.writers:
            rain_it_component.add_writer(writer)
        return rain_it_component
        
    
    