from writer.GPIOWriterState import GPIOWriterState

class GPIOWriterBusy(GPIOWriterState):
    
    def write(self, writer_manager):
        pass           
    
    def force_write(self, writer_manager):
        self.terminate_event()      
        free_state = writer_manager.state_factory.create_free_state()
        self.change_state(writer_manager, free_state)
        writer_manager.write()