import datetime
from abc import ABCMeta


class IExpirable(metaclass=ABCMeta):
    def __init__(self):
        self.time_stamp = None

    def set_time_stamp(self, time_stamp):
        self.time_stamp = datetime.datetime.strptime(time_stamp, '%Y-%m-%dT%H:%M:%S.%f')

    def is_expired(self, expire_minutes):
        utc_now = datetime.datetime.utcnow()
        return self.time_stamp is not None and self.time_stamp + datetime.timedelta(minutes=expire_minutes) < utc_now
