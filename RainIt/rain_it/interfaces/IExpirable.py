import datetime
from abc import ABCMeta


class IExpirable(metaclass=ABCMeta):
    def __init__(self):
        self.time_stamp = None

    def set_time_stamp(self, time_stamp):
        self.time_stamp = datetime.datetime.strptime(time_stamp, '%Y-%m-%dT%H:%M:%S.%f')

    def is_expired(self, expire_minutes):
        utc_now = datetime.datetime.utcnow()
        if self.time_stamp is None:
            return True
        return self.time_stamp + datetime.timedelta(minutes=expire_minutes) < utc_now

    def is_most_recent(self, other_expirable):
        if other_expirable is None or other_expirable.time_stamp is None:
            return True
        return self.time_stamp > other_expirable.time_stamp
