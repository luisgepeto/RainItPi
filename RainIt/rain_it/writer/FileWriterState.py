from writer.WriterState import WriterState

class FileWriterState(WriterState):   
             
    write_queue = None
        
    def force_write(self, writer, rain_it_component):
        self.write(writer, rain_it_component)
        
    def get_write_queue(self):
        return FileWriterState.write_queue
    
    def set_write_queue(self, write_queue):
        FileWriterState.write_queue = write_queue   
        
    def terminate_queue(self):
        self.set_write_queue(None)
    