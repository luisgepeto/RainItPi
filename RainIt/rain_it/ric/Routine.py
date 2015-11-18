from ric.RainItComposite import RainItComposite

class Routine(RainItComposite):
    def __init__(self, routine_id):   
        super().__init__()
        self.routine_id = routine_id