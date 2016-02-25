from writer.WriterFactory import WriterFactory
from builder.RainItDirector import RainItDirector
from builder.DemoBuilder import DemoBuilder
from builder.ServiceBuilder import ServiceBuilder
from builder.FileBuilder import FileBuilder


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

    demo_pattern = demo_director.get_test_pattern()
    demo_routine = demo_director.get_test_routine()
    demo_procedure = demo_director.get_active_procedure()
    
    demo_pattern.gpio_write()
    demo_pattern.file_write()
    demo_routine.gpio_write()
    demo_routine.file_write()
    demo_procedure.gpio_write()
    demo_procedure.file_write()   
