from writer.WriterState import WriterState

class FileWriterState(WriterState):   
             
    write_pool = None
        
    def force_write(self, writer_manager):
        self.write(writer_manager)
        
    def get_write_pool(self):
        return FileWriterState.write_pool 
    
    def set_write_pool(self, write_pool):
        FileWriterState.write_pool =  write_pool