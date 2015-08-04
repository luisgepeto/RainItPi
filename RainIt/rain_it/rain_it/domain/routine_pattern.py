'''
This is the routine pattern object 
'''
class RoutinePattern(object):    
    def __init__(self, routine_pattern_id, repetitions, pattern):
        self.routine_pattern_id = int(routine_pattern_id)
        self.repetitions = int(repetitions)
        self.pattern = pattern
        