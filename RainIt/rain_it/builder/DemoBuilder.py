from builder.RainItBuilder import RainItBuilder
from datetime import datetime


class DemoBuilder(RainItBuilder):
    initial_pattern_id = 0
    initial_routine_id = 0

    def get_test_pattern(self):
        DemoBuilder.initial_pattern_id += 1
        sample_matrix = self.get_matrix(DemoBuilder.initial_pattern_id, None)
        return {'patternAsMatrix': sample_matrix, 'SampleTimeStamp': datetime.now().strftime('%Y-%m-%dT%H:%M:%S.%f')}

    def get_test_routine(self):
        routine_list = []
        DemoBuilder.initial_routine_id += 1
        sample_routine = self.create_new_routine(DemoBuilder.initial_routine_id)
        DemoBuilder.initial_routine_id += 2
        routine_list.append(sample_routine)
        return routine_list

    def get_active_procedure(self):
        first_routine_list = self.get_test_routine()
        second_routine_list = self.get_test_routine()
        return first_routine_list + second_routine_list

    def get_matrix(self, pattern_id, conversion_parameter):
        sample_matrix = []
        for i in range(10):
            current_line = []
            for j in range(10):
                current_line.append(pattern_id)
            sample_matrix.append(current_line)
        return sample_matrix

    def create_new_routine(self, new_routine_base):
        routine = {
            'RoutineId': new_routine_base,
            'SampleTimeStamp': datetime.now().strftime('%Y-%m-%dT%H:%M:%S.%f'),
            'RoutinePatternDTOs': [
                {
                    'Repetitions': new_routine_base,
                    'RoutinePatternId': new_routine_base,
                    'PatternDTO': {
                        'Path': 'somepath',
                        'PatternId': new_routine_base,
                        'ConversionParameterDTO': {
                            'RWeight': 0,
                            'GWeight': 0,
                            'BWeight': 0,
                            'ThresholdPercentage': 0,
                            'IsInverted': True
                        }
                    }
                },
                {
                    'Repetitions': new_routine_base + 1,
                    'RoutinePatternId': new_routine_base + 1,
                    'PatternDTO': {
                        'Path': 'somepath',
                        'PatternId': new_routine_base + 1,
                        'ConversionParameterDTO': {
                            'RWeight': 50,
                            'GWeight': 50,
                            'BWeight': 50,
                            'ThresholdPercentage': 50,
                            'IsInverted': False
                        }
                    }
                },
                {
                    'Repetitions': new_routine_base + 2,
                    'RoutinePatternId': new_routine_base + 2,
                    'PatternDTO': {
                        'Path': 'somepath',
                        'PatternId': new_routine_base + 2,
                        'ConversionParameterDTO': {
                            'RWeight': 100,
                            'GWeight': 100,
                            'BWeight': 100,
                            'ThresholdPercentage': 100,
                            'IsInverted': True
                        }
                    }
                }
            ]
        }
        return routine
