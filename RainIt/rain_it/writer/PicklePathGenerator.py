from configparser import ConfigParser
from builder.ComponentType import ComponentType
import os


class PicklePathGenerator(object):
    def __init__(self):
        self.config = ConfigParser()
        self.config.read("..\\rainit.config")
        self.base_directory = self.config.get("Pickle", "BaseDirectory")

    def get_full_pickle_path(self, component_type):
        pickle_path = self.create_path(component_type.get_pickle_path())
        return os.path.join(pickle_path, component_type.get_pickle_name() + '.pickle')

    def create_path(self, specific_path):
        return os.path.join(os.path.abspath(os.sep), self.base_directory + specific_path)
