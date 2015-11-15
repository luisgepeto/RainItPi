from writer.GPIOWriterState import GPIOWriterState

class GPIOWriterBusy(GPIOWriterState):
    
    def write(self, writer, rain_it_component):
        pass           
    
    def force_write(self, writer, rain_it_component):
        self.terminate_event()      
        free_state = writer.state_factory.create_free_state()
        self.change_state(writer, free_state)
        writer.write(rain_it_component)