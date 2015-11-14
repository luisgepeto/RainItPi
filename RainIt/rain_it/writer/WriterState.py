from abc import ABCMeta, abstractmethod

class WriterState(metaclass = ABCMeta):

    @abstractmethod
    def write(self, writer, rain_it_component):
        pass
    
    @abstractmethod
    def force_write(self, writer, rain_it_component):
        pass
    
    def change_state(self, writer, writer_state):
        writer.change_state(writer_state)
        
    @abstractmethod
    def get_write_event(self):
        pass
    
    @abstractmethod
    def set_write_event(self, new_write_event):
        pass
    
    def terminate_event(self):
        write_event = self.get_write_event()
        if write_event:
            write_event.set()            
            self.set_write_event(None) 