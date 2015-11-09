

class RequestException(Exception):
    def __init__(self, value):
        self.value = value
    def __str__(self):
        return repr(self.value)
    
class GetRequestException(RequestException):
    def __init__(self, value):
        self.value = value
    def __str__(self):
        return repr(self.value)
    
    
class PostRequestException(RequestException):
    def __init__(self, value):
        self.value = value
    def __str__(self):
        return repr(self.value)