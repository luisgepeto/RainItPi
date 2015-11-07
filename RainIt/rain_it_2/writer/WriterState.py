from abc import ABCMeta, abstractmethod

class WriterState(metaclass = ABCMeta):

    @abstractmethod
    def write(self, writer_manager):
        pass
    
    @abstractmethod
    def force_write(self, writer_manager):
        pass
    
    def change_state(self, writer_manager, writer_state):
        writer_manager.change_state(writer_state)
        
    @abstractmethod
    def get_write_future(self):
        pass
    
    @abstractmethod
    def set_write_future(self, new_write_pool):
        pass
    
    def terminate_future(self):
        instance_write_future = self.get_write_future()
        if instance_write_future:
            instance_write_future.set()
            self.set_write_future(None) 