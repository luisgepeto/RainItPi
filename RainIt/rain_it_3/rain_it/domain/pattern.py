'''
This is the pattern response object 
'''
from domain.conversion_parameter import ConversionParameter

class Pattern(object):   
    
    def __init__(self, pattern_id, path, conversion_parameter, matrix):
        self.pattern_id = int(pattern_id)
        self.path = path
        if conversion_parameter is not None:
            self.conversion_parameter = ConversionParameter(conversion_parameter["RWeight"],conversion_parameter["GWeight"], conversion_parameter["BWeight"], conversion_parameter["IsInverted"], conversion_parameter["ThresholdPercentage"])
        self.matrix = matrix        
        
    def get_as_dictionary(self):
        return {"PatternId":self.pattern_id, "Path":self.path, "ConversionParameterDTO":self.conversion_parameter.get_as_dictionary(), "Matrix":self.matrix}
    
    def set_matrix(self, matrix):
        self.matrix = matrix
        
    def __eq__(self, other): 
        return self.pattern_id == other.pattern_id and self.conversion_parameter == other.conversion_parameter