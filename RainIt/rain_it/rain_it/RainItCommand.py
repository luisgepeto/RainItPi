from builder import ComponentType
from rain_it.RainItComponentManager import RainItComponentManager


class RainItCommand(object):
    def __init__(self):
        self.manager = RainItComponentManager()

    def execute(self):
        all_components = self.get_all_components(self.manager)
        device_settings = self.get_component_by_type(all_components, ComponentType.device_settings)
        for component in all_components:
            self.manager.file_write_component(component)
        for component in all_components:
            if component is not ComponentType.device_settings:
                self.manager.gpio_continous_write_component(component, device_settings)

    def get_all_components(self, manager):
        all_components = [manager.get_device_settings(), manager.get_test_pattern(), manager.get_test_routine(),
                          manager.get_active_procedure()]
        return all_components

    def get_component_by_type(self, all_components, component_type):
        for component in all_components:
            if component.component_type is component_type:
                return component
