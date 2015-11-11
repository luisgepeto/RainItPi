from writer.Writer import Writer

class FileWriter(Writer):
    
    def __init__(self, state_factory):
        super().__init__(state_factory)
        self.state = self.state_factory.create_free_state()
    
    def write(self):
        self.state.write(self)
        
    def update(self):
        self.state.force_write(self)