from ric.RainItComponent import RainItComponent


class RainItComposite(RainItComponent):
    def __init__(self):
        super().__init__()
        self.components = []

    def add_writer(self, writer):
        super().add_writer(writer)
        for component in self.components:
            component.add_writer(writer)

    def add_rain_it_component(self, rain_it_component):
        for writer in self.writers:
            rain_it_component.add_writer(writer)
        self.components.append(rain_it_component)

    def gpio_write(self, device_settings, hardware_wrapper):
        for component in self.components:
            component.gpio_write(device_settings, hardware_wrapper)

    def gpio_force_write(self, device_settings, hardware_wrapper):
        component_iterator = iter(self.components)
        first_component = next(component_iterator)
        first_component.gpio_force_write(device_settings, hardware_wrapper)
        while True:
            try:
                component = next(component_iterator)
                component.gpio_write(device_settings, hardware_wrapper)
            except StopIteration:
                break

    def __eq__(self, other):
        return other is not None and self.components == other.components
