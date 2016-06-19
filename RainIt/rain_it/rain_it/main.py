import sys
import os.path
sys.path.append(os.path.join(os.path.dirname(__file__), '..'))
from builder.DemoBuilder import DemoBuilder
from builder.FileBuilder import FileBuilder
from builder.ServiceBuilder import ServiceBuilder
from builder.RainItDirector import RainItDirector
from writer.WriterFactory import WriterFactory

if __name__ == '__main__':

    writer_factory = WriterFactory()
    all_writers = writer_factory.create_all_writers()

    #demo_director = RainItDirector(DemoBuilder(), all_writers)
    service_director = RainItDirector(ServiceBuilder(), all_writers)
    file_director = RainItDirector(FileBuilder(), all_writers)
    director = service_director

    settings = director.get_device_settings()
    pattern = director.get_test_pattern()
    routine = director.get_test_routine()
    procedure = director.get_active_procedure()

    settings.file_write()
    #pattern.gpio_write()
    pattern.file_write()
    #routine.gpio_write()
    routine.file_write()
    #procedure.gpio_write()
    procedure.file_write()

    #print(pattern.is_expired(1))
    #print(routine.is_expired(1))
    #print(procedure.is_expired(1))
