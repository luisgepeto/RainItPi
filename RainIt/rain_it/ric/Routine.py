from ric.RainItComposite import RainItComposite
from itertools import groupby

class Routine(RainItComposite):
    def __init__(self, routine_id):   
        super().__init__()
        self.routine_id = routine_id
        
    def get_pickle_name(self):
        return '{}'.format(self.routine_id)
    
    def get_pickle_form(self):        
        grouped_list = [(component.get_pickle_name(), sum(1 for i in grouped_patterns)) for component, grouped_patterns in groupby(self.components)]
        return grouped_list
        
            
            
    
    
    