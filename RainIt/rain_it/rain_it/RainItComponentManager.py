import time

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

    def gpio_continous_write_component(self, rain_it_component, device_settings):
        start_time = time.time()
        current_time = start_time
        while rain_it_component is not None \
                and not rain_it_component.is_expired(device_settings.minutes_refresh_rate) \
                and not current_time - start_time > device_settings.minutes_refresh_rate:
            current_time = time.time()
            rain_it_component.gpio_write()

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
