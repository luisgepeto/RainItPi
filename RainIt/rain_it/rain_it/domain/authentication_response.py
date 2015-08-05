'''
This class is the response of the authentication
'''
class Authentication_Response(object):
    def __init__(self, login_status, token_expiration_time, token):
        self.login_status = login_status
        self.token_expiration_time = token_expiration_time
        self.token = token
        