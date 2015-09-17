'''
Created on Sep 14, 2015

@author: luis
'''

class Settings(object):    
    def __init__(self, minutes_refresh_rate, millisecond_latch_delay, millisecond_clock_delay):
        self.minutes_refresh_rate = minutes_refresh_rate
        self.millisecond_latch_delay = millisecond_latch_delay
        self.millisecond_clock_delay = millisecond_clock_delay        
        
    def __eq__(self, other): 
        return self.minutes_refresh_rate == other.minutes_refresh_rate and self.millisecond_latch_delay == other.millisecond_latch_delay and self.millisecond_clock_delay == other.millisecond_clock_delay
    
    