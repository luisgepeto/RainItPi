from abc import ABCMeta, abstractmethod
from queue import Queue


class WriterState(metaclass=ABCMeta):
    @abstractmethod
    def write(self, writer, rain_it_component):
        pass

    @abstractmethod
    def force_write(self, writer, rain_it_component):
        pass

    def change_state(self, writer, writer_state):
        writer.change_state(writer_state)
