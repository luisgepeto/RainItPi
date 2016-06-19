from abc import ABCMeta, abstractmethod


class IPickleable(metaclass=ABCMeta):
    def __init__(self):
        pass

    @abstractmethod
    def get_pickle_name(self):
        pass

    @abstractmethod
    def get_pickle_form(self):
        pass