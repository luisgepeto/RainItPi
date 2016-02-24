from writer.WriterFactory import WriterFactory
from builder.RainItDirector import RainItDirector
from builder.ServiceBuilder import ServiceBuilder
from rain_it.SourceSubject import SourceSubject

if __name__ == '__main__':    
    writer_factory = WriterFactory()
    file_writer = writer_factory.create_file_writer()
    gpio_writer = writer_factory.create_gpio_writer()
    
<<<<<<< HEAD
    director = RainItDirector(DemoBuilder())
    pattern = director.get_test_pattern()
    routine = director.get_test_routine()
    procedure = director.get_active_procedure()
    
=======
    director = RainItDirector(ServiceBuilder())
    pattern = director.get_test_pattern()
    routine = director.get_test_routine()
    procedure = director.get_active_procedure()
     
>>>>>>> branch 'dev' of https://luisgepeto@bitbucket.org/luisgepeto/rainitpi.git
    pattern.add_writer(file_writer)
<<<<<<< HEAD
    pattern.add_writer(gpio_writer)
    routine.add_writer(file_writer)
    routine.add_writer(gpio_writer)
    procedure.add_writer(file_writer)
    procedure.add_writer(gpio_writer)
=======
    routine.add_writer(file_writer)
    procedure.add_writer(file_writer)
>>>>>>> branch 'dev' of https://luisgepeto@bitbucket.org/luisgepeto/rainitpi.git
    
<<<<<<< HEAD
    pattern.gpio_write()
    pattern.file_write()
    routine.gpio_write()
    procedure.gpio_write()
    
    
    
    


=======
    pattern.file_write(SourceSubject.test_pattern)
    routine.file_write(SourceSubject.test_routine)
    procedure.file_write(SourceSubject.active_procedure)
>>>>>>> branch 'dev' of https://luisgepeto@bitbucket.org/luisgepeto/rainitpi.git
