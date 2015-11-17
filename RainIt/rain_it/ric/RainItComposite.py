from ric.RainItComponent import RainItComponent

class RainItComposite(RainItComponent):
    
    def __init__(self):
        self.components = []    
    
    def add_writer(self, writer):
        super().add_writer(writer)
        for component in self.components:
            component.add_writer(writer)
        
    def add_rain_it_component(self, rain_it_component):
        for writer in self.writers:
            rain_it_component.add_writer(writer)
        self.components.append(rain_it_component)
    
    def file_write(self):
        for component in self.components:
            component.file_write()
    
    def gpio_write(self):
        for component in self.components:
            component.gpio_write()
    
    def gpio_force_write(self):
        component_iterator = iter(self.components)
        first_component = next(component_iterator)
        first_component.gpio_force_write()
        while True:
            try:
                component = next(component_iterator)
                component.gpio_write()
            except StopIteration:
                break