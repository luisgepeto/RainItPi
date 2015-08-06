'''
This is the pattern object 
'''
import ast
class Pattern(object):      
    def __init__(self, pattern_id, pattern_as_matrix):
        self.pattern_id = int(pattern_id)
        self.pattern_as_matrix = ast.literal_eval(pattern_as_matrix)       
        