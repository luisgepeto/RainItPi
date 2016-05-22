from abc import ABCMeta, abstractmethod


class Writer(metaclass=ABCMeta):
    def __init__(self, writer_state_factory):
        self.state_factory = writer_state_factory
        self.state = None

    @abstractmethod
    def write(self, rain_it_component):
        pass

    @abstractmethod
    def force_write(self, rain_it_component):
        pass

    def change_state(self, writer_state):
        self.state = writer_state
