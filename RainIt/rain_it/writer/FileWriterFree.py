from writer.FileWriterState import FileWriterState
import asyncio
import time
import threading

class FileWriterFree(FileWriterState):
    
    def write(self, writer_manager):
        self.write_async()         
        busy_state = writer_manager.state_factory.create_busy_state()
        self.change_state(writer_manager, busy_state)
        
    def write_async(self):
        event = threading.Event()
        self.set_write_event(event)
        asyncio.get_event_loop().run_in_executor(None, self.async_file_write, 10, event)        
    
    def async_file_write(self, seconds_to_block, event):
        for i in range(seconds_to_block):
            if event.is_set():
                return
            print('writing file {}/{}'.format(i, seconds_to_block))
            time.sleep(1)
        print('done writing file {}'.format(seconds_to_block))
        
    def async_file_write_callback(self, writer_manager):
        self.terminate_event()
        free_state = writer_manager.state_factory.create_free_state()
        self.change_state(writer_manager, free_state) 
