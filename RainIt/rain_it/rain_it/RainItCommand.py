import time

from builder.ComponentType import ComponentType
from rain_it.RainItComponentManager import RainItComponentManager


class RainItCommand(object):
    def __init__(self):
        self.manager = RainItComponentManager()
        self.all_components = []
        self.start_time = time.time()
        self.current_time = None
        self.is_retrieved = False

    def execute(self):
        self.append_component(self.get_if_expired(ComponentType.test_pattern))
        self.append_component(self.get_if_expired(ComponentType.test_routine))
        self.append_component(self.get_if_expired(ComponentType.active_procedure))
        self.append_component(self.get_if_expired(ComponentType.device_settings))
        # if self.manager.is_valid(self.test_pattern, self.device_settings.minutes_refresh_rate) or \
        #        self.manager.is_valid(self.test_routine, self.device_settings.minutes_refresh_rate):
        #    self.manager.gpio_write_test(self.test_pattern, self.test_routine, self.device_settings, self.is_new)
        #    self.is_new[ComponentType.test_pattern.get_name()] = False
        #    self.is_new[ComponentType.test_routine.get_name()] = False
        # elif self.active_procedure is not None:
        #    if self.is_new[ComponentType.active_procedure.get_name()]:
        #        self.is_new[ComponentType.active_procedure.get_name()] = False
        #        self.active_procedure.gpio_force_write(self.device_settings)
        #    else:
        #        self.active_procedure.gpio_write(self.device_settings)
        self.current_time = time.time()
        if self.is_retrieved:
            self.is_retrieved = False
            self.start_time = self.current_time

    def get_if_expired(self, component_type):
        current_component = self.get_component_by_type(component_type)
        if self.is_expired(self.current_time):
            new_component = self.manager.get_component(component_type)
            if not new_component == current_component:
                current_component = new_component
            self.manager.file_write_component(current_component)
            self.is_retrieved = True
        return current_component

    def is_expired(self, current_time):
        device_settings = self.get_component_by_type(ComponentType.device_settings)
        if device_settings is None or current_time is None:
            return True
        return (current_time - self.start_time) / 60 > device_settings.minutes_refresh_rate

    def get_component_by_type(self, component_type):
        for component in self.all_components:
            if component.component_type is component_type:
                return component

    def append_component(self, new_component):
        if new_component is not None:
            is_inserted = False
            for index, component in enumerate(self.all_components):
                if component.component_type == new_component.component_type:
                    is_inserted = True
                    self.all_components[index] = new_component
            if not is_inserted:
                self.all_components.append(new_component)
