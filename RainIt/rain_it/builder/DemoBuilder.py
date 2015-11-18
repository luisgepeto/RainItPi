from builder.RainItBuilder import RainItBuilder
from builder.SourceSubject import SourceSubject
from ric.Pattern import Pattern 
from datetime import datetime

class DemoBuilder(RainItBuilder):
    
    initial_pattern_id = 0
    initial_routine_id = 0
    
    def read_data_source(self, source_subject):
        if source_subject is SourceSubject.test_pattern:
            return self.get_test_pattern()
        elif source_subject is SourceSubject.test_routine:
            return self.get_test_routine()
        elif source_subject is SourceSubject.active_procedure:
            pass
        else:
            pass    
    
    def get_test_pattern(self):
        DemoBuilder.initial_pattern_id+=1
        sample_matrix = self.create_new_matrix(DemoBuilder.initial_pattern_id)
        return { 'patternAsMatrix' : sample_matrix, 'SampleTimeStamp': datetime.now()}
    
    def get_test_routine(self):         
        routine_list = []
        DemoBuilder.initial_pattern_id+=1
        sample_routine = self.create_new_routine(DemoBuilder.initial_pattern_id)
        DemoBuilder.initial_pattern_id+=2        
        routine_list.append(sample_routine)
        return routine_list
        
    def build_pattern(self, pattern_id = 0, conversion_parameter = None, matrix = None):        
        return Pattern(pattern_id, conversion_parameter, matrix)
    
    def build_routine(self, pattern_list):
        pass
    
    def build_procedure(self, routine_list):
        pass
    
    def create_new_matrix(self, new_matrix_elements):        
        sample_matrix = []
        for i in range(10):
            current_line = []
            for j in range(10):
                current_line.append(DemoBuilder.initial_pattern_id)
            sample_matrix.append(current_line)
        return sample_matrix  
    
    def create_new_routine(self, new_routine_base):
        routine = {
                   'RoutineId': 0,
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