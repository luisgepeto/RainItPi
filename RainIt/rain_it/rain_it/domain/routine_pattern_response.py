'''
This is the routine pattern response object 
'''
from domain.pattern_response import Pattern


class RoutinePattern(object):    
    def __init__(self, routine_pattern_id, pattern, repetitions):
        self.routine_pattern_id = routine_pattern_id
        self.repetitions = repetitions
        self.pattern = Pattern(pattern["PatternId"], pattern["Path"], pattern["ConversionParameterDTO"])
        