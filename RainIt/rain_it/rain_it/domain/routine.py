'''
This is the routine object
'''

class Routine(object):    
    def __init__(self, routine_id, routine_pattern_list):
        self.routine_id = int(routine_id)
        self.routine_pattern_list = routine_pattern_list