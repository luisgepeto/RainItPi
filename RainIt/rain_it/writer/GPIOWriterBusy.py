from writer.GPIOWriterState import GPIOWriterState


class GPIOWriterBusy(GPIOWriterState):
    def write(self, writer, rain_it_component, device_settings):
        queue = self.get_write_queue()
        if queue:
            queue.put({"rain_it_component": rain_it_component, "device_settings": device_settings})

    def force_write(self, writer, rain_it_component, device_settings):
        self.terminate_event()
        self.terminate_queue()
        free_state = writer.state_factory.create_free_state()
        self.change_state(writer, free_state)
        writer.write(rain_it_component, device_settings)
