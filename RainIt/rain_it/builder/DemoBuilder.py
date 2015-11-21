from builder.RainItBuilder import RainItBuilder
from ric.Pattern import Pattern 
from ric.Routine import Routine
from datetime import datetime
from ric.Procedure import Procedure

class DemoBuilder(RainItBuilder):
    
    initial_pattern_id = 0
    initial_routine_id = 0
    
    def get_test_pattern(self):
        DemoBuilder.initial_pattern_id+=1
        sample_matrix = self.create_new_matrix(DemoBuilder.initial_pattern_id)
        return { 'patternAsMatrix' : sample_matrix, 'SampleTimeStamp': datetime.now()}
    
    def get_test_routine(self):         
        routine_list = []
        DemoBuilder.initial_routine_id+=1
        sample_routine = self.create_new_routine(DemoBuilder.initial_routine_id)
        DemoBuilder.initial_routine_id+=2        
        routine_list.append(sample_routine)
        return routine_list
    
    def get_active_procedure(self):
        first_routine_list = self.get_test_routine()
        second_routine_list = self.get_test_routine()        
        return first_routine_list + second_routine_list
        
    def build_pattern(self, pattern_id = 0, conversion_parameter = None, matrix = None, path = None, pattern_factory = None):
        if not matrix:
            matrix = self.create_new_matrix(pattern_id)
        pattern = pattern_factory.get_pattern(pattern_id)
        if pattern is None:
            pattern = Pattern(pattern_id, conversion_parameter, matrix)
            pattern_factory.add_pattern(pattern)    
        return pattern 
    
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
    
    def create_new_matrix(self, new_matrix_elements):        
        sample_matrix = []
        for i in range(10):
            current_line = []
            for j in range(10):
                current_line.append(new_matrix_elements)
            sample_matrix.append(current_line)
        return sample_matrix  
    
    def create_new_routine(self, new_routine_base):
        routine = {
                   'RoutineId': new_routine_base,
                   'SampleTimeStamp': datetime.now(),
                   'RoutinePatternDTOs': [
                                          {
                                           'Repetitions': new_routine_base,
                                           'RoutinePatternId': new_routine_base,
                                           'PatternDTO':{
                                                         'Path': 'somepath',
                                                         'PatternId': new_routine_base,
                                                         'ConversionParameterDTO':{
                                                                                   'RWeight': 0,
                                                                                   'GWeight': 0,
                                                                                   'BWeight':0,
                                                                                   'ThresholdPercentage': 0,
                                                                                   'IsInverted': True
                                                                                   }
                                                         }
                                           },
                                          {
                                            'Repetitions': new_routine_base+1,
                                            'RoutinePatternId': new_routine_base+1,
                                            'PatternDTO':{
                                                          'Path': 'somepath',
                                                          'PatternId': new_routine_base+1,
                                                          'ConversionParameterDTO':{
                                                                                    'RWeight': 50,
                                                                                    'GWeight': 50,
                                                                                    'BWeight':50,
                                                                                    'ThresholdPercentage': 50,
                                                                                    'IsInverted': False
                                                                                    }
                                                        }
                                           },
                                          {
                                            'Repetitions': new_routine_base+2,
                                            'RoutinePatternId': new_routine_base+2,
                                            'PatternDTO':{
                                                          'Path': 'somepath',
                                                          'PatternId': new_routine_base+2,
                                                          'ConversionParameterDTO':{
                                                                                    'RWeight': 100,
                                                                                    'GWeight': 100,
                                                                                    'BWeight':100,
                                                                                    'ThresholdPercentage': 100,
                                                                                    'IsInverted': True
                                                                                    }
                                                        }
                                           }
                                          ]
                   }
        return routine