import datetime


class AuthenticationResult(object):

    def __init__(self, login_status, security_token, token_expiration):
        self.login_status = login_status
        self.security_token = security_token
        raw_date = token_expiration.partition(".")[0]
        self.token_expiration = datetime.datetime.strptime(raw_date, '%Y-%m-%dT%H:%M:%S')

    def is_valid(self):
        utc_now = datetime.datetime.utcnow()
        return not(self.login_status != 1 or not self.security_token or utc_now > self.token_expiration)

