from abc import ABCMeta, abstractmethod

class WriterManager(metaclass = ABCMeta):
    
    def __init__(self, writer_state_factory):
        self.state_factory = writer_state_factory
        self.state = None
        
    @abstractmethod
    def write(self):
        pass
    
    @abstractmethod
    def update(self):
        pass
    
    def change_state(self, writer_state):
        self.state = writer_state