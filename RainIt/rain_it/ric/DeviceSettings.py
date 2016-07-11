from ric.RainItComponent import RainItComponent


class DeviceSettings(RainItComponent):

    def __init__(self, minutes_refresh_rate, millisecond_latch_delay, millisecond_clock_delay):
        super().__init__()
        self.minutes_refresh_rate = int(minutes_refresh_rate)
        self.millisecond_latch_delay = int(millisecond_latch_delay)
        self.millisecond_clock_delay = int(millisecond_clock_delay)

    def get_pickle_form(self):
        return self

    def gpio_force_write(self, device_settings, hardware_wrapper):
        pass

    def gpio_write(self, device_settings, hardware_wrapper):
        pass

    def __eq__(self, other):
        return other is not None and self.minutes_refresh_rate == other.minutes_refresh_rate \
               and self.millisecond_clock_delay == other.millisecond_clock_delay \
               and self.millisecond_latch_delay == other.millisecond_latch_delay