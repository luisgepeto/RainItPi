from ric.Pattern import Pattern

class PatternFactory(object):
    
    def __init__(self):
        self.pattern_id_dict = {}
        
    def get_pattern(self, pattern_id):
        if pattern_id not in self.pattern_id_dict:
            return None
        return self.pattern_id_dict[pattern_id]
        
    def add_pattern(self, pattern):
        '''avoiding the add of patterns with id 0 
        helps us to avoid adding the test pattern to the dictionary'''
        if not pattern.pattern_id == 0:
            self.pattern_id_dict[pattern.pattern_id] = pattern        