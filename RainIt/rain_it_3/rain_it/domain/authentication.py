'''
This class is the response of the authentication
'''
import time
from datetime import datetime

class Authentication(object):
    def __init__(self, login_status, token_expiration_time, token):
        self.login_status = login_status
        self.token_expiration_time = "1970-01-01T00:00:00.0"
        if not token_expiration_time == "" and token_expiration_time is not None:
            self.token_expiration_time = token_expiration_time[:len(token_expiration_time)-2]
        self.token = token
        
    def has_expired(self):        
        expiration_time = datetime.fromtimestamp(time.mktime(time.strptime(self.token_expiration_time , "%Y-%m-%dT%H:%M:%S.%f")))        
        return expiration_time < datetime.utcnow() 