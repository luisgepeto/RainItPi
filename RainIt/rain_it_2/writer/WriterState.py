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
    def get_write_pool(self):
        pass
    
    @abstractmethod
    def set_write_pool(self, new_write_pool):
        pass
    
    def terminate_pool(self):
        instance_write_pool = self.get_write_pool()
        if instance_write_pool:
            instance_write_pool.terminate()
            self.set_write_pool(None) 