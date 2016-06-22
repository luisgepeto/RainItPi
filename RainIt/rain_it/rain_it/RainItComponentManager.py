import time

from builder.ComponentType import ComponentType
from builder.DemoBuilder import DemoBuilder
from builder.FileBuilder import FileBuilder
from builder.RainItDirector import RainItDirector
from builder.ServiceBuilder import ServiceBuilder
from writer.WriterFactory import WriterFactory


class RainItComponentManager(object):
    def __init__(self):
        all_writers = WriterFactory().create_all_writers()
        self.service_director = RainItDirector(ServiceBuilder(), all_writers)
        self.file_director = RainItDirector(FileBuilder(), all_writers)
        self.demo_director = RainItDirector(DemoBuilder(), all_writers)

    def is_valid(self, rain_it_component, expire_minutes):
        return rain_it_component is not None and not rain_it_component.is_expired(expire_minutes)

    def file_write_component(self, rain_it_component):
        if rain_it_component is not None:
            rain_it_component.file_write()

    def get_device_settings(self):
        device_settings = self.service_director.get_device_settings()
        if device_settings is None:
            device_settings = self.file_director.get_device_settings()
        if device_settings is None:
            device_settings = self.demo_director.get_device_settings()
        return device_settings

    def get_test_pattern(self):
        test_pattern = self.service_director.get_test_pattern()
        if test_pattern is None:
            test_pattern = self.file_director.get_test_pattern()
        return test_pattern

    def get_test_routine(self):
        test_routine = self.service_director.get_test_routine()
        if test_routine is None:
            test_routine = self.file_director.get_test_routine()
        return test_routine

    def get_active_procedure(self):
        active_procedure = self.service_director.get_active_procedure()
        if active_procedure is None:
            active_procedure = self.file_director.get_active_procedure()
        return active_procedure

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
            pass