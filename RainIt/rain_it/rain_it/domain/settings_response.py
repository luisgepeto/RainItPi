'''
Created on Sep 14, 2015

@author: luis
'''

class Settings(object):    
    def __init__(self, minutes_refresh_rate, millisecond_latch_delay, millisecond_clock_delay, test_mode):
        self.minutes_refresh_rate = minutes_refresh_rate
        self.millisecond_latch_delay = millisecond_latch_delay
        self.millisecond_clock_delay = millisecond_clock_delay
        self.test_mode = test_mode