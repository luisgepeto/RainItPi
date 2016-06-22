from writer.GPIOWriterState import GPIOWriterState
from queue import Queue
import time
import asyncio
import threading


class GPIOWriterFree(GPIOWriterState):
    def write(self, writer, rain_it_component, device_settings):
        self.write_async(writer, rain_it_component, device_settings)
        busy_state = writer.state_factory.create_busy_state()
        self.change_state(writer, busy_state)

    def force_write(self, writer, rain_it_component, device_settings):
        self.write(writer, rain_it_component, device_settings)

    def write_async(self, writer, rain_it_component, device_settings):
        event = threading.Event()
        self.set_write_event(event)
        queue = Queue(maxsize=10)
        queue.put({"rain_it_component": rain_it_component, "device_settings": device_settings})
        self.set_write_queue(queue)
        asyncio.get_event_loop().run_in_executor(None, self.async_gpio_write, writer, queue, event)

    def async_gpio_write(self, writer, queue, event):
        while not queue.empty():
            queue_element = queue.get()
            pattern = queue_element["rain_it_component"]
            device_settings = queue_element["device_settings"]
            self.blocking_function(pattern, device_settings, event)
        if not event.is_set():
            self.async_gpio_write_callback(writer)

    def async_gpio_write_callback(self, writer):
        self.terminate_event()
        self.terminate_queue()
        free_state = writer.state_factory.create_free_state()
        self.change_state(writer, free_state)

    def blocking_function(self, pattern, device_settings, event):
        clock_delay = device_settings.millisecond_clock_delay / 1000
        latch_delay = device_settings.millisecond_latch_delay / 1000
        if pattern.matrix is not None:
            for matrix_line in pattern.matrix:
                for element in matrix_line:
                    if event.is_set():
                        return
                    if element == True:
                        element = 1
                    elif element == False:
                        element = 0
                    print(element, end="", flush=True)
                    time.sleep(clock_delay)
                print()
                time.sleep(latch_delay)
