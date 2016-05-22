from abc import ABCMeta, abstractmethod
from builder.SourceSubject import SourceSubject
from ric.Routine import Routine
from ric.Procedure import Procedure
from ric.Pattern import Pattern
from ric.ConversionParameter import ConversionParameter

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
    def get_matrix(self, pattern_id, conversion_parameter):
        pass
    
    @abstractmethod
    def get_test_routine(self):         
        pass
    
    @abstractmethod
    def get_active_procedure(self):
        pass    
    
    def build_pattern(self, pattern_id = 0, conversion_parameter = None, matrix = None, path = None, pattern_factory = None):        
        pattern = pattern_factory.get_pattern(pattern_id)
        if pattern is None:
            if not matrix and pattern_id is not 0:
                matrix = self.get_matrix(pattern_id, conversion_parameter)
            if conversion_parameter is None:
                conversion_parameter = self.build_conversion_parameter(0, 0, 0, False, 0)
            pattern = Pattern(pattern_id, conversion_parameter, matrix)
            pattern_factory.add_pattern(pattern)    
        return pattern
    
    def build_conversion_parameter(self, r_weight, g_weight, b_weight, is_inverted, threshold_percentage):
        return ConversionParameter(r_weight, g_weight, b_weight, is_inverted, threshold_percentage)
    
    def build_routine(self, routine_id, pattern_list):
        routine = Routine(routine_id)
        for pattern in pattern_list:
            routine.add_rain_it_component(pattern)
        return routine
    
    def build_procedure(self, routine_list):
        procedure = Procedure()
        for routine in routine_list:
            procedure.add_rain_it_component(routine)
        return procedure