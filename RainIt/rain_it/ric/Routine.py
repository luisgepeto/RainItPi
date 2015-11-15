from ric.RainItComponent import RainItComponent

class Routine(RainItComponent):
    
    def __init__(self, name):
        super().__init__(name)
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
        for component in self.components:
            component.gpio_force_write()
                
    def get_writer_of_type(self, writer_type):
        for writer in self.writers:
            if type(writer) is writer_type:
                return writer