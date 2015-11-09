from writer.WriterState import WriterState

class GPIOWriterState(WriterState):
    
    write_event = None
        
    def get_write_event(self):
        return GPIOWriterState.write_event 
    
    def set_write_event(self, write_event):
        GPIOWriterState.write_event =  write_event
