from builder.ComponentType import ComponentType
from builder.DemoBuilder import DemoBuilder
from builder.FileBuilder import FileBuilder
from builder.RainItDirector import RainItDirector
from builder.ServiceBuilder import ServiceBuilder
from writer.WriterFactory import WriterFactory
from requests.exceptions import RequestException


class RainItComponentManager(object):
    def __init__(self):
        all_writers = WriterFactory().create_all_writers()
        self.service_director = RainItDirector(ServiceBuilder(), all_writers)
        self.file_director = RainItDirector(FileBuilder(), all_writers)
        self.demo_director = RainItDirector(DemoBuilder(), all_writers)

    def get_device_settings(self):
        try:
            return self.service_director.get_device_settings()
        except RequestException:
            try:
                return self.file_director.get_device_settings()
            except IOError:
                return self.demo_director.get_device_settings()

    def get_test_pattern(self):
        try:
            return self.service_director.get_test_pattern()
        except RequestException:
            try:
                return self.file_director.get_test_pattern()
            except IOError:
                return None

    def get_test_routine(self):
        try:
            return self.service_director.get_test_routine()
        except RequestException:
            try:
                return self.file_director.get_test_routine()
            except IOError:
                return None

    def get_active_procedure(self):
        try:
            return self.service_director.get_active_procedure()
        except RequestException:
            try:
                return self.file_director.get_active_procedure()
            except IOError:
                return None

    def get_null_component(self):
        return self.demo_director.get_null_component()

    def get_component(self, component_type):
        if component_type is ComponentType.test_pattern:
            return self.get_test_pattern()
        elif component_type is ComponentType.test_routine:
            return self.get_test_routine()
        elif component_type is ComponentType.active_procedure:
            return self.get_active_procedure()
        elif component_type is ComponentType.device_settings:
            return self.get_device_settings()
        else:
            return self.get_null_component()
