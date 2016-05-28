from writer.FileWriterState import FileWriterState
from queue import Queue
import asyncio
import time
import pickle
from writer.PicklePathGenerator import PicklePathGenerator
import os

class FileWriterFree(FileWriterState):
    def write(self, writer, rain_it_component):
        self.write_async(writer, rain_it_component)
        busy_state = writer.state_factory.create_busy_state()
        self.change_state(writer, busy_state)

    def write_async(self, writer, rain_it_component):
        queue = Queue(maxsize=0)
        queue.put(rain_it_component)
        self.set_write_queue(queue)
        asyncio.get_event_loop().run_in_executor(None, self.async_file_write, writer, queue)

    def async_file_write(self, writer, queue):
        while not queue.empty():
            rain_it_component = queue.get()
            self.pickle_component(rain_it_component)
        self.async_file_write_callback(writer)

    def async_file_write_callback(self, writer):
        free_state = writer.state_factory.create_free_state()
        self.change_state(writer, free_state)
        self.terminate_queue()

    def pickle_component(self, rain_it_component):
        time.sleep(0.001)
        pickle_path = PicklePathGenerator().get_full_pickle_path(rain_it_component.component_type)
        os.makedirs(os.path.dirname(pickle_path), exist_ok=True)
        with open(pickle_path, 'wb') as f:
            pickle.dump(rain_it_component.get_pickle_form(), f, pickle.HIGHEST_PROTOCOL)
