import time

from adapter.HardwareWrapper import HardwareWrapper
from adapter.MessagePipe import MessagePipe
from builder.ComponentType import ComponentType
from rain_it.RainItComponentManager import RainItComponentManager


class RainItCommand(object):
    def __init__(self):
        self.manager = RainItComponentManager()
        self.all_components = []
        self.previous_time = time.time()
        self.new_dict = {}
        self.hardware_wrapper = HardwareWrapper()
        self.message_pipe = MessagePipe()

    def initialize(self):
        self.hardware_wrapper.gpio_initialize()
        self.message_pipe.pipe_initialize()

    def can_continue(self):
        return not self.message_pipe.read()

    def update_components(self):
        current_time = time.time()
        if self.is_expired(current_time):
            for component in ComponentType:
                self.update_component(component)
            self.previous_time = current_time

    def is_expired(self, current_time):
        device_settings = self.retrieve_component(ComponentType.device_settings)
        if device_settings is None:
            return True
        return (current_time - self.previous_time) / 60 > device_settings.minutes_refresh_rate

    def update_component(self, component_type):
        current_component = self.retrieve_component(component_type)
        new_component = self.manager.get_component(component_type)
        if not new_component == current_component:
            if new_component is not None:
                new_component.file_write()
                self.append_component(new_component)
            self.set_new(component_type, True)

    def append_component(self, new_component):
        for index, component in enumerate(self.all_components):
            if component.component_type == new_component.component_type:
                self.all_components[index] = new_component
                return
        self.all_components.append(new_component)

    def print_components(self):
        test_pattern = self.retrieve_component(ComponentType.test_pattern)
        test_routine = self.retrieve_component(ComponentType.test_routine)
        if test_pattern is not None or test_routine is not None:
            if test_pattern is not None and test_pattern.is_most_recent(test_routine):
                self.gpio_write(ComponentType.test_pattern)
            elif test_routine is not None:
                self.gpio_write(ComponentType.test_routine)
        else:
            self.gpio_write(ComponentType.active_procedure)

    def gpio_write(self, component_type):
        device_settings = self.retrieve_component(ComponentType.device_settings)
        current_component = self.retrieve_component(component_type)
        if current_component is not None:
            if self.is_new(component_type):
                current_component.gpio_force_write(device_settings, self.hardware_wrapper)
                self.set_new(component_type, False)
            else:
                current_component.gpio_write(device_settings, self.hardware_wrapper)

    def exit(self):
        device_settings = self.retrieve_component(ComponentType.device_settings)
        null_component = self.manager.get_component(None)
        null_component.gpio_force_write(device_settings, self.hardware_wrapper)
        self.hardware_wrapper.gpio_cleanup()

    def retrieve_component(self, component_type):
        for component in self.all_components:
            if component.component_type is component_type:
                return component

    def set_new(self, component_type, is_new):
        self.new_dict[component_type.get_name()] = is_new

    def is_new(self, component_type):
        if component_type.get_name() in self.new_dict:
            return self.new_dict[component_type.get_name()]
        return False


