from writer.WriterManagerFactory import WriterManagerFactory

writer_manager = WriterManagerFactory()
file_writer = writer_manager.create_file_writer()
gpio_writer = writer_manager.create_gpio_writer()

gpio_writer.write()
gpio_writer.update()
gpio_writer.write()

file_writer.write()
file_writer.update()
file_writer.write()