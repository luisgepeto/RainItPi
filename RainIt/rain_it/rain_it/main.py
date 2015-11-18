from writer.WriterFactory import WriterFactory
from ric.Pattern import Pattern
from ric.Routine import Routine
from builder.DemoBuilder import DemoBuilder
from builder.RainItDirector import RainItDirector

if __name__ == '__main__':    
    writer_factory = WriterFactory()
    file_writer = writer_factory.create_file_writer()
    gpio_writer = writer_factory.create_gpio_writer()
    
    director = RainItDirector(DemoBuilder())
    pattern1 = director.get_test_pattern() 
    pattern2 = director.get_test_pattern()
    pattern3 = director.get_test_pattern()
    
    pattern1.add_writer(file_writer)
    pattern1.add_writer(gpio_writer)
    pattern2.add_writer(file_writer)
    pattern2.add_writer(gpio_writer)
    pattern3.add_writer(file_writer)
    pattern3.add_writer(gpio_writer)
    
    pattern1.gpio_write()
    pattern2.gpio_write()
    pattern3.gpio_write()
    
    
    
    


