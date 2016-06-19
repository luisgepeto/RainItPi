from enum import Enum


class ComponentType(Enum):
    test_pattern = 1
    test_routine = 2
    active_procedure = 3
    device_settings = 4

    def get_pickle_name(self):
        return self.name

    def get_pickle_path(self):
        return self.name