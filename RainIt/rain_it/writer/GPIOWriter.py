from writer.Writer import Writer


class GPIOWriter(Writer):
    def __init__(self, state_factory):
        super().__init__(state_factory)
        self.state = self.state_factory.create_free_state()

    def write(self, rain_it_component, device_settings, hardware_wrapper):
        self.state.write(self, rain_it_component, device_settings, hardware_wrapper)

    def force_write(self, rain_it_component, device_settings, hardware_wrapper):
        self.state.force_write(self, rain_it_component, device_settings, hardware_wrapper)
