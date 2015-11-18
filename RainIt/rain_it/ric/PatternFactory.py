from ric.Pattern import Pattern

class PatternFactory(object):
    
    def __init__(self):
        self.pattern_id_dict = {}
        
    def get_pattern(self, pattern_id):
        if pattern_id not in self.pattern_id_dict:
            self.create_pattern(pattern_id)
        return self.pattern_id_dict[pattern_id]
        
    def create_pattern(self, pattern_id):
        self.pattern_id_dict[pattern_id] = Pattern(pattern_id, None, None)
        