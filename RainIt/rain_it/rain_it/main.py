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
    procedure = director.get_active_procedure()
    
    procedure.add_writer(file_writer)
    procedure.add_writer(gpio_writer)
    
    procedure.gpio_write()
    
    
    
    


