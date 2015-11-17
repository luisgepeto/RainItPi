class ConversionParameter(object):
    
    def __init__(self, r_weight, g_weight, b_weight, is_inverted, threshold_percentage):
        self.r_weight = r_weight
        self.g_weight = g_weight
        self.b_weight = b_weight
        self.is_inverted = is_inverted
        self.threshold_percentage = threshold_percentage