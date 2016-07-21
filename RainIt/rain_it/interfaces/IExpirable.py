import datetime
from abc import ABCMeta


class IExpirable(metaclass=ABCMeta):
    def __init__(self):
        self.time_stamp = None
        self.retrieved_time_stamp = None

    def set_retrieved_time_stamp(self, time_stamp):
        self.retrieved_time_stamp = time_stamp

    def set_time_stamp(self, time_stamp):
        self.time_stamp = datetime.datetime.strptime(time_stamp, '%Y-%m-%dT%H:%M:%S.%f')

    def is_expired(self, expire_minutes, use_retrieved_time_stamp=False):
        utc_now = datetime.datetime.utcnow()
        time_stamp_to_compare = self.retrieved_time_stamp if use_retrieved_time_stamp else self.time_stamp
        if time_stamp_to_compare is None:
            return True
        return time_stamp_to_compare + datetime.timedelta(minutes=expire_minutes) < utc_now

    def is_most_recent(self, other_expirable, use_retrieved_time_stamp=False):
        if other_expirable is None:
            return True
        other_time_stamp_to_compare = other_expirable.retrieved_time_stamp if use_retrieved_time_stamp else other_expirable.time_stamp
        if other_time_stamp_to_compare is None:
            return True
        time_stamp_to_compare = self.retrieved_time_stamp if use_retrieved_time_stamp else self.time_stamp
        return time_stamp_to_compare > other_time_stamp_to_compare
