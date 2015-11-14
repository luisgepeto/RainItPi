from writer.WriterState import WriterState

class FileWriterState(WriterState):   
             
    write_event = None
        
    def force_write(self, writer, rain_it_component):
        self.write(writer, rain_it_component)
        
    def get_write_event(self):
        return FileWriterState.write_event 
    
    def set_write_event(self, write_event):
        FileWriterState.write_event = write_event