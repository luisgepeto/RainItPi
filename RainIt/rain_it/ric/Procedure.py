from ric.RainItComposite import RainItComposite


class Procedure(RainItComposite):
    def __init__(self):
        super().__init__()

    def get_pickle_form(self):
        return self
