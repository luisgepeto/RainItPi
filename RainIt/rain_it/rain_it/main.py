from writer.WriterFactory import WriterFactory
from ric.Pattern import Pattern
from ric.Routine import Routine

writer_factory = WriterFactory()
file_writer = writer_factory.create_file_writer()
gpio_writer = writer_factory.create_gpio_writer()

patternA = Pattern("A")
patternB = Pattern("B")
patternB.add_writer(gpio_writer)
routineX = Routine("X")
routineX.add_writer(file_writer)
routineX.add_rain_it_component(patternA)
routineX.add_writer(gpio_writer)
routineX.add_rain_it_component(patternB)

patternA.file_write()
patternB.file_write()
patternA.gpio_write()
patternB.gpio_write()
