from writer.IWriterStateFactory import IWriterStateFactory
from writer.FileWriterBusy import FileWriterBusy
from writer.FileWriterFree import FileWriterFree


class FileWriterStateFactory(IWriterStateFactory):
    
    def create_busy_state(self):
        return FileWriterBusy()
    
    def create_free_state(self):
        return FileWriterFree()        