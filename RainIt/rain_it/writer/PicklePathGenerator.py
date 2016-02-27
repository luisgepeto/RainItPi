from builder.ComponentType import ComponentType
import os

class PicklePathGenerator(object):
    
    def __init__(self):
        pass
    
    def get_full_pickle_path(self, element_pickle_name, component_type):
        return os.path.join(self.get_pickle_path(component_type), element_pickle_name+'.pickle')
    
    def get_pickle_path(self, component_type):
        specific_path = ""
        if component_type is ComponentType.test_pattern:
            specific_path = "test_pattern"
        elif component_type is ComponentType.test_routine:
            specific_path = "test_routine"
        elif component_type is ComponentType.active_procedure:
            specific_path = "active_procedure"        
        return self.create_path(specific_path)
            
    def create_path(self, specific_path):
        return os.path.join(os.path.abspath(os.sep), "home\\pi\\"+specific_path)