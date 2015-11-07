from writer.WriterState import WriterState

class FileWriterState(WriterState):   
             
    write_future = None
        
    def force_write(self, writer_manager):
        self.write(writer_manager)
        
    def get_write_future(self):
        return FileWriterState.write_future 
    
    def set_write_future(self, write_future):
        FileWriterState.write_future = write_future