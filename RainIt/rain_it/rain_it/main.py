from writer.WriterFactory import WriterFactory
from builder.RainItDirector import RainItDirector
from builder.ServiceBuilder import ServiceBuilder
from builder.DemoBuilder import DemoBuilder

if __name__ == '__main__':    
    writer_factory = WriterFactory()
    file_writer = writer_factory.create_file_writer()
    gpio_writer = writer_factory.create_gpio_writer()
    
    director = RainItDirector(ServiceBuilder())
    pattern = director.get_test_pattern()
    routine = director.get_test_routine()
    procedure = director.get_active_procedure()
     
    pattern.add_writer(file_writer)
    routine.add_writer(file_writer)
    procedure.add_writer(file_writer)
    
    pattern.file_write()
    routine.file_write()
    procedure.file_write() 
    
    


