from writer.FileWriterState import FileWriterState
import asyncio
import time
import threading


class FileWriterFree(FileWriterState):
    
    def write(self, writer_manager):
        write_pool = self.write_async()
        self.set_write_pool(write_pool) 
        busy_state = writer_manager.state_factory.create_busy_state()
        self.change_state(writer_manager, busy_state)
        
    def write_async(self):
        event = threading.Event()
        file_write_future = asyncio.get_event_loop().run_in_executor(None, self.async_file_write, 10, event)
        file_write_future.add_done_callback(self.async_file_write_callback)
        return file_write_future
    
    
    def async_file_write(self, seconds_to_block, event):
        for i in range(seconds_to_block):
            if event.is_set():
                return
            print('writing {}/{}'.format(i, seconds_to_block))
            time.sleep(1)
        print('done writing {}'.format(seconds_to_block))
        
    def async_file_write_callback(self):
        #self.terminate_pool()
        print("FILE WRITE CALLBACK")