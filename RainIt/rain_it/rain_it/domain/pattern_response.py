'''
This is the pattern response object 
'''
from domain.conversion_parameter_response import ConversionParameter

class Pattern(object):   
    
    def __init__(self, pattern_id, path, conversion_parameter):
        self.pattern_id = pattern_id
        self.path = path        
        self.conversion_parameter = ConversionParameter(conversion_parameter["RWeight"],conversion_parameter["GWeight"], conversion_parameter["BWeight"], conversion_parameter["IsInverted"], conversion_parameter["ThresholdPercentage"])