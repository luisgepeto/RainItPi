from writer.WriterState import WriterState

class GPIOWriterState(WriterState):
    
    write_future = None
        
    def get_write_future(self):
        return GPIOWriterState.write_future 
    
    def set_write_future(self, write_future):
        GPIOWriterState.write_future =  write_future