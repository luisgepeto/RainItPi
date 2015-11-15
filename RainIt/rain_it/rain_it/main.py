from writer.WriterFactory import WriterFactory
from ric.Pattern import Pattern
from ric.Routine import Routine

if __name__ == '__main__':    
    writer_factory = WriterFactory()
    file_writer = writer_factory.create_file_writer()
    gpio_writer = writer_factory.create_gpio_writer()
    
    patternA = Pattern("A")
    patternB = Pattern("B")
    patternC = Pattern("C")
    patternC.add_writer(gpio_writer)
    patternC.add_writer(file_writer)
    patternB.add_writer(gpio_writer)
    routineX = Routine("X")
    routineX.add_writer(file_writer)
    routineX.add_rain_it_component(patternA)
    routineX.add_writer(gpio_writer)
    routineX.add_rain_it_component(patternB)
    
    routineX.gpio_write()
    patternC.gpio_write()
    routineX.gpio_force_write()
    
    
    routineX.file_write()
    patternC.file_write()
    
    
    


