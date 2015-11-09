from writer.IWriterStateFactory import IWriterStateFactory
from writer.GPIOWriterBusy import GPIOWriterBusy
from writer.GPIOWriterFree import GPIOWriterFree

class GPIOWriterStateFactory(IWriterStateFactory):
    
    def create_busy_state(self):
        return GPIOWriterBusy()
    
    def create_free_state(self):
        return GPIOWriterFree()        