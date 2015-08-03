'''
This is the routine response object
'''
from domain.routine_pattern import RoutinePattern

class Routine(object):    
    def __init__(self, routine_id, routine_pattern_list):
        self.routine_id = routine_id
        self.routine_pattern_list = []
        for routine_pattern in routine_pattern_list:
            routine_pattern = RoutinePattern(routine_pattern["RoutinePatternId"], routine_pattern["PatternDTO"], routine_pattern["Repetitions"])            
            self.routine_pattern_list.append(routine_pattern)
        
        