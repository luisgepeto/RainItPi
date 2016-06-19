from builder.ComponentType import ComponentType
import os


class PicklePathGenerator(object):
    def __init__(self):
        pass

    def get_full_pickle_path(self, component_type):
        pickle_path = self.create_path(component_type.get_pickle_path())
        return os.path.join(pickle_path, component_type.get_pickle_name() + '.pickle')

    def create_path(self, specific_path):
        return os.path.join(os.path.abspath(os.sep), "home\\pi\\" + specific_path)
