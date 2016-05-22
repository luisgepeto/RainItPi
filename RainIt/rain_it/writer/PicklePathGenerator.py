from builder.ComponentType import ComponentType
import os

class PicklePathGenerator(object):
    
    def __init__(self):
        pass
    
    def get_full_pickle_path(self, component_type):
        return os.path.join(self.get_pickle_path(component_type), self.get_pickle_name(component_type)+'.pickle')
    
    def get_pickle_name(self, component_type):
        name = ""
        if component_type is ComponentType.test_pattern:
            name = "test_pattern"
        elif component_type is ComponentType.test_routine:
            name = "test_routine"
        elif component_type is ComponentType.active_procedure:
            name = "active_procedure"        
        return name
    
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