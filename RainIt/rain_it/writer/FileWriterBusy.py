from writer.FileWriterState import FileWriterState

class FileWriterBusy(FileWriterState):

    def write(self, writer, rain_it_component):
        #ignores call. may be necessary to create a queue for objects?
        pass