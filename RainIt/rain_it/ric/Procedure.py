from ric.RainItComposite import RainItComposite

class Procedure(RainItComposite):
    def __init__(self):
        super().__init__()
     
    def should_pickle(self):
        return self.components
   
    def get_pickle_dir(self):
        return "procedure"
    
    def get_pickle_name(self):
        return 'pickled_procedure'
    
    def get_pickle_form(self):
        routine_list = [component.routine_id for component in self.components]        
        return routine_list