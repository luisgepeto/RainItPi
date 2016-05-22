from builder.DemoBuilder import DemoBuilder
from builder.FileBuilder import FileBuilder
from builder.RainItDirector import RainItDirector
from writer.WriterFactory import WriterFactory

if __name__ == '__main__':
    '''
        With the WriterFactory we can create several types of writers
        depending on the output type. These writers have specific states
        which control their behavior on the different operations they expose.
        The FileWriter serializes information into a file, while the GPIOWriter
        displays the information on the pins of the Raspberry Pi.
    '''
    writer_factory = WriterFactory()
    all_writers = writer_factory.create_all_writers()
        
    '''
        The director exposes an interface to retrieve the different products
        that can be displayed. It needs a builder which indicates the source 
        from which the different products will be composed. The three possible
        sources are: DemoBuilder, ServiceBuilder and FileBuilder. It also needs
        a list of writers which will be used when writing the resulting objects. 
    '''
    demo_director = RainItDirector(DemoBuilder(), all_writers)
    #service_director = RainItDirector(ServiceBuilder(), all_writers)
    #file_director = RainItDirector(FileBuilder(), all_writers)
    director = demo_director
    
    pattern = director.get_test_pattern()
    routine = director.get_test_routine()
    procedure = director.get_active_procedure()
    
    pattern.gpio_write()
    pattern.file_write()
    routine.gpio_write()
    routine.file_write()
    procedure.gpio_write()
    procedure.file_write()   
