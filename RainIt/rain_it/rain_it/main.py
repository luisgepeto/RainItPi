from writer.WriterFactory import WriterFactory
from builder.RainItDirector import RainItDirector
from builder.ServiceBuilder import ServiceBuilder
from builder.DemoBuilder import DemoBuilder

if __name__ == '__main__':    
    writer_factory = WriterFactory()
    file_writer = writer_factory.create_file_writer()
    gpio_writer = writer_factory.create_gpio_writer()
    
    director = RainItDirector(ServiceBuilder())
    pattern = director.get_test_routine()
    
    pattern.add_writer(file_writer)
    pattern.add_writer(gpio_writer)
    
    pattern.gpio_write()
    
    
    
    


