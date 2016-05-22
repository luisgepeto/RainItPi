from writer.FileWriter import FileWriter
from writer.FileWriterStateFactory import FileWriterStateFactory
from writer.GPIOWriter import GPIOWriter
from writer.GPIOWriterStateFactory import GPIOWriterStateFactory


class WriterFactory(object):
    def create_file_writer(self):
        return FileWriter(FileWriterStateFactory())

    def create_gpio_writer(self):
        return GPIOWriter(GPIOWriterStateFactory())

    def create_all_writers(self):
        all_writers = []
        all_writers.append(self.create_file_writer())
        all_writers.append(self.create_gpio_writer())
        return all_writers
