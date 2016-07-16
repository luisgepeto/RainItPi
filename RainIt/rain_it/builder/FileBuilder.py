from builder.RainItBuilder import RainItBuilder
from writer.PicklePathGenerator import PicklePathGenerator
from builder.ComponentType import ComponentType
import pickle


class FileBuilder(RainItBuilder):
    def get_device_settings(self):
        return self.get_unpickled_object(ComponentType.device_settings)

    def get_test_pattern(self):
        return self.get_unpickled_object(ComponentType.test_pattern)

    def get_test_routine(self):
        return self.get_unpickled_object(ComponentType.test_routine)

    def get_active_procedure(self):
        return self.get_unpickled_object(ComponentType.active_procedure)

    def get_unpickled_object(self, component_type):
        pickle_file = self.get_pickle_file(component_type)
        unpickled_object = None
        if not pickle_file == "":
            unpickled_object = pickle.load(pickle_file)
            pickle_file.close()
        return unpickled_object

    def get_pickle_file(self, component_type):
        pickle_path = PicklePathGenerator().get_full_pickle_path(component_type)
        return open(pickle_path, 'rb')

    def build_pattern(self, pattern_id=0, conversion_parameter=None, matrix=None, path=None, pattern_factory=None,
                      component_type=None):
        pass

    def build_routine(self, pattern_list):
        pass

    def build_procedure(self, routine_list):
        pass

    def get_matrix(self, pattern_id, conversion_parameter):
        pass
