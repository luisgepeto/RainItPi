'''
This is the routine response object
'''
from domain.routine_pattern import RoutinePattern

class Routine(object):    
    def __init__(self, routine_id, routine_pattern_list, time_stamp):
        self.routine_id = int(routine_id)
        self.routine_pattern_list = []
        for routine_pattern in routine_pattern_list:
            routine_pattern = RoutinePattern(routine_pattern["RoutinePatternId"], routine_pattern["PatternDTO"], routine_pattern["Repetitions"])            
            self.routine_pattern_list.append(routine_pattern)
        self.time_stamp = time_stamp
        
    def __eq__(self, other):        
        if self.routine_id == other.routine_id and len(self.routine_pattern_list) == len(other.routine_pattern_list):
            self_set = set((x.routine_pattern_id, x.repetitions, x.pattern.pattern_id, x.pattern.conversion_parameter.r_weight, x.pattern.conversion_parameter.g_weight, x.pattern.conversion_parameter.b_weight, x.pattern.conversion_parameter.is_inverted, x.pattern.conversion_parameter.threshold_percentage) for x in self.routine_pattern_list)
            difference_set = [ x for x in other.routine_pattern_list if (x.routine_pattern_id, x.repetitions, x.pattern.pattern_id, x.pattern.conversion_parameter.r_weight, x.pattern.conversion_parameter.g_weight, x.pattern.conversion_parameter.b_weight, x.pattern.conversion_parameter.is_inverted, x.pattern.conversion_parameter.threshold_percentage) not in self_set ]
            if not difference_set:
                return True
        return False