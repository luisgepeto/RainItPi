from writer.GPIOWriterState import GPIOWriterState
import time
import threading
import asyncio
               
class GPIOWriterFree(GPIOWriterState):
    
    def write(self, writer_manager):                 
        self.write_async()        
        busy_state = writer_manager.state_factory.create_busy_state()
        self.change_state(writer_manager, busy_state)        
    
    def force_write(self, writer_manager):        
        self.write(writer_manager)
    
    def write_async(self):        
        event = threading.Event()
        self.set_write_future(event)
        asyncio.get_event_loop().run_in_executor(None, self.async_gpio_write, 10, event)
        
    def async_gpio_write(self, seconds_to_block, event):
        for i in range(seconds_to_block):
            if event.is_set():
                return
            print('writing gpio {}/{}'.format(i, seconds_to_block))
            time.sleep(1)
        print('done writing gpio {}'.format(seconds_to_block))        
        
    def async_gpio_write_callback(self, writer_manager):        
        self.terminate_future()
        free_state = writer_manager.state_factory.create_busy_state()
        self.change_state(writer_manager, free_state)  
        