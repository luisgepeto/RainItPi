from abc import ABCMeta, abstractmethod

import datetime

from interfaces.IExpirable import IExpirable
from interfaces.IPickleable import IPickleable
from writer.FileWriter import FileWriter


class RainItComponent(IExpirable, IPickleable):
    def __init__(self):
        IExpirable.__init__(self)
        self.writers = []
        self.component_type = None

    def set_component_type(self, component_type):
        self.component_type = component_type

    def add_writer(self, writer):
        found_writer = False
        for index, old_writer in enumerate(self.writers):
            if isinstance(old_writer, type(writer)):
                self.writers[index] = writer
                found_writer = True
        if not found_writer:
            self.writers.append(writer)

    def add_rain_it_component(self, rain_it_component):
        pass

    def file_write(self):
        file_writer = self.get_writer_of_type(FileWriter)
        if file_writer is not None:
            file_writer.write(self)

    @abstractmethod
    def gpio_write(self, device_settings):
        pass

    @abstractmethod
    def gpio_force_write(self, device_settings):
        pass

    def get_writer_of_type(self, writer_type):
        for writer in self.writers:
            if type(writer) is writer_type:
                return writer

    @abstractmethod
    def __eq__(self, other):
        pass