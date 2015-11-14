from writer.FileWriterState import FileWriterState
import asyncio
import time
import threading

class FileWriterFree(FileWriterState):
    
    def write(self, writer, rain_it_component):
        self.write_async(writer, rain_it_component)         
        busy_state = writer.state_factory.create_busy_state()
        self.change_state(writer, busy_state) 
        
    def write_async(self, writer, rain_it_component):
        event = threading.Event()
        self.set_write_event(event)
        asyncio.get_event_loop().run_in_executor(None, self.async_file_write, writer, rain_it_component, event)        
    
    def async_file_write(self,writer, rain_it_component, event):
        self.blocking_function(rain_it_component, event)
        if not event.is_set():
            self.async_file_write_callback(writer)
        
    def async_file_write_callback(self, writer):
        self.terminate_event()
        free_state = writer.state_factory.create_free_state()        
        self.change_state(writer, free_state)
        
    def blocking_function(self, rain_it_component, event):
        for i in range(10):
            if event.is_set():
                return
            print('writing file {} {}/{}'.format(rain_it_component.name, i, 10))
            time.sleep(1)
        print('done writing file {} {}'.format(rain_it_component.name, 10))