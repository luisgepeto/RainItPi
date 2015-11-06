from writer.WriterState import WriterState

class GPIOWriterState(WriterState):
    
    write_pool = None
        
    def get_write_pool(self):
        return GPIOWriterState.write_pool 
    
    def set_write_pool(self, write_pool):
        GPIOWriterState.write_pool =  write_pool