from ric.RainItComponent import RainItComponent
from writer.FileWriter import FileWriter

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
    
    def file_write(self, source_subject):
        file_writer = self.get_writer_of_type(FileWriter)
        if file_writer is not None:
            file_writer.write(self, source_subject)
        for component in self.components:
            component.file_write(source_subject)
    
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