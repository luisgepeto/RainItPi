from abc import ABCMeta, abstractmethod

import datetime

from writer.FileWriter import FileWriter


class RainItComponent(metaclass=ABCMeta):
    def __init__(self):
        self.writers = []
        self.component_type = None
        self.time_stamp = None

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
    def gpio_write(self):
        pass

    @abstractmethod
    def gpio_force_write(self):
        pass

    def get_writer_of_type(self, writer_type):
        for writer in self.writers:
            if type(writer) is writer_type:
                return writer

    @abstractmethod
    def get_pickle_name(self):
        pass

    @abstractmethod
    def get_pickle_form(self):
        pass

    def set_time_stamp(self, time_stamp):
        self.time_stamp = datetime.datetime.strptime(time_stamp, '%Y-%m-%dT%H:%M:%S.%f')

    def is_expired(self, expire_minutes):
        utc_now = datetime.datetime.utcnow()
        return self.time_stamp is not None and self.time_stamp + datetime.timedelta(minutes=expire_minutes) < utc_now
