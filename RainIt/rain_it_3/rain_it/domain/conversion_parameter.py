
class ConversionParameter(object):
    def __init__(self, r_weight, g_weight, b_weight, is_inverted, threshold_percentage):
        self.r_weight = int(r_weight)
        self.g_weight = int(g_weight)
        self.b_weight = int(b_weight)
        if int(is_inverted) == True:
            self.is_inverted = True
        else:
            self.is_inverted = False
        self.threshold_percentage = int(threshold_percentage)
        

    def get_as_dictionary(self):
        return {"RWeight":self.r_weight, "GWeight":self.g_weight, "BWeight":self.b_weight, "IsInverted":self.is_inverted, "ThresholdPercentage":self.threshold_percentage}
    
    def __eq__(self, other): 
        return  self.r_weight == other.r_weight and self.g_weight == other.g_weight and self.b_weight == other.b_weight and self.is_inverted == other.is_inverted and self.threshold_percentage == other.threshold_percentage