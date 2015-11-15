from writer.FileWriterState import FileWriterState
from queue import Queue
import asyncio
import time

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
    
    def async_file_write(self,writer, queue):
        while not queue.empty():
            rain_it_component = queue.get()
            self.blocking_function(rain_it_component)
        self.async_file_write_callback(writer)
        
    def async_file_write_callback(self, writer):        
        self.terminate_queue()
        free_state = writer.state_factory.create_free_state()        
        self.change_state(writer, free_state)
        
    def blocking_function(self, rain_it_component):
        for i in range(10):
            print('writing file {} {}/{}'.format(rain_it_component.name, i, 10))
            time.sleep(1)
        print('done writing file {} {}'.format(rain_it_component.name, 10))