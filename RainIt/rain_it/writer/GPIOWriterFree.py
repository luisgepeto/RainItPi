from writer.GPIOWriterState import GPIOWriterState
from queue import Queue
import time
import asyncio
import threading
    
class GPIOWriterFree(GPIOWriterState):
    
    def write(self, writer, rain_it_component):   
        self.write_async(writer, rain_it_component)         
        busy_state = writer.state_factory.create_busy_state()
        self.change_state(writer, busy_state)                 
    
    def force_write(self, writer, rain_it_component):        
        self.write(writer, rain_it_component)
    
    def write_async(self, writer, rain_it_component):        
        event = threading.Event()
        self.set_write_event(event)
        queue = Queue(maxsize=0)
        queue.put(rain_it_component)
        self.set_write_queue(queue)
        asyncio.get_event_loop().run_in_executor(None, self.async_gpio_write, writer, queue, event)          
           
    def async_gpio_write(self, writer, queue, event):
        while not queue.empty():
            rain_it_component = queue.get()
            self.blocking_function(rain_it_component, event)
        if not event.is_set():
            self.async_gpio_write_callback(writer)
         
    def async_gpio_write_callback(self, writer):
        self.terminate_event()
        self.terminate_queue()
        free_state = writer.state_factory.create_free_state()        
        self.change_state(writer, free_state)
        
    def blocking_function(self, rain_it_component, event):
        print('writing gpio')
        for matrix_line in rain_it_component.matrix:
            for element in matrix_line:
                if event.is_set():
                    return
                if element == True:
                    element = 1
                elif element == False:
                    element = 0
                print(element,end="",flush=True)                
            print()
            time.sleep(0.001)
        print('done writing gpio')