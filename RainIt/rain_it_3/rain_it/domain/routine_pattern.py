'''
This is the routine pattern response object 
'''
from domain.pattern import Pattern


class RoutinePattern(object):    
    def __init__(self, routine_pattern_id, pattern, repetitions):
        self.routine_pattern_id = int(routine_pattern_id)
        self.repetitions = int(repetitions)
        if not "Matrix" in pattern:
            pattern["Matrix"] = []
        self.pattern = Pattern(pattern["PatternId"], pattern["Path"], pattern["ConversionParameterDTO"], pattern["Matrix"])
    
    def get_as_dictionary(self):
        return {"RoutinePatternId":self.routine_pattern_id, "PatternDTO":self.pattern.get_as_dictionary(), "Repetitions":self.repetitions}
    
    def __eq__(self, other): 
        return self.routine_pattern_id == other.routine_pattern_id and self.pattern == other.pattern and self.repetitions == other.repetitions