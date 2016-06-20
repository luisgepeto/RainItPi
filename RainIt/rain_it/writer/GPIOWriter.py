from writer.Writer import Writer


class GPIOWriter(Writer):
    def __init__(self, state_factory):
        super().__init__(state_factory)
        self.state = self.state_factory.create_free_state()

    def write(self, rain_it_component, device_settings):
        self.state.write(self, rain_it_component, device_settings)

    def force_write(self, rain_it_component, device_settings):
        self.state.force_write(self, rain_it_component, device_settings)
