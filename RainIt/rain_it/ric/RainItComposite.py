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

    def gpio_write(self, device_settings):
        for component in self.components:
            component.gpio_write(device_settings)

    def gpio_force_write(self, device_settings):
        component_iterator = iter(self.components)
        first_component = next(component_iterator)
        first_component.gpio_force_write(device_settings)
        while True:
            try:
                component = next(component_iterator)
                component.gpio_write(device_settings)
            except StopIteration:
                break