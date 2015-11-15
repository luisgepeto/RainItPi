from writer.WriterState import WriterState
 
class GPIOWriterState(WriterState):
    
    write_event = None
    write_queue = None
    
    def get_write_event(self):
        return GPIOWriterState.write_event 
    
    def set_write_event(self, write_event):
        GPIOWriterState.write_event =  write_event
        
    def terminate_event(self):
        write_event = self.get_write_event()
        if write_event:
            write_event.set()            
            self.set_write_event(None)
        
    def get_write_queue(self):
        return GPIOWriterState.write_queue
    
    def set_write_queue(self, write_queue):
        GPIOWriterState.write_queue = write_queue   
        
    def terminate_queue(self):
        self.set_write_queue(None)
