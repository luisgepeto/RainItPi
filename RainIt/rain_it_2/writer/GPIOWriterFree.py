from writer.GPIOWriterState import GPIOWriterState
import time
from multiprocessing import pool
               
class GPIOWriterFree(GPIOWriterState):
    
    def write(self, writer_manager):
        write_pool = self.write_async()
        self.set_write_pool(write_pool)             
        busy_state = writer_manager.state_factory.create_busy_state()
        self.change_state(writer_manager, busy_state)    
    
    def force_write(self, writer_manager):
        self.terminate_pool()
        self.write(writer_manager)
        
    def write_async(self):
        p = pool.Pool(1)
        p.apply_async(self.gpio_write, [self], callback=self.write_async_callback)
        return p       
        
    def gpio_write(self):
        time.sleep(10)
        print("GPIO WRITE ASYNC")
        return self
        
    def write_async_callback(self):
        self.terminate_pool()
        print("GPIO WRITE CALLBACK")         