from abc import ABCMeta, abstractmethod


class IWriterStateFactory(metaclass=ABCMeta):
    @abstractmethod
    def create_busy_state(self):
        pass

    @abstractmethod
    def create_free_state(self):
        pass
