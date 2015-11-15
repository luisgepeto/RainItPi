from writer.FileWriterState import FileWriterState

class FileWriterBusy(FileWriterState):

    def write(self, writer, rain_it_component):
        queue = self.get_write_queue()
        if queue:
            queue.put(rain_it_component)