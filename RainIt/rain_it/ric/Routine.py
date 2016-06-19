from ric.RainItComposite import RainItComposite
from itertools import groupby


class Routine(RainItComposite):
    def __init__(self, routine_id):
        super().__init__()
        self.routine_id = routine_id

    def get_pickle_form(self):
        return self
