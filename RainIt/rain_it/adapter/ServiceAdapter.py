import json
import os
from configparser import ConfigParser

import requests
from requests.exceptions import RequestException
from adapter.AuthenticationResult import AuthenticationResult
from adapter.HardwareWrapper import HardwareWrapper


class ServiceAdapter(object):

    def __init__(self, base_uri):
        self.base_uri = base_uri
        self.authentication_result = None
        config = ConfigParser()
        config_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', "rainit.config")
        config.read(config_path)
        self.seconds_timeout = config.getint("Services", "SecondsTimeout")

    def authenticate(self):
        serial = HardwareWrapper().get_serial()
        try:
            json_result = self.post("account/login/" + serial, needs_authentication=False);
        except RequestException:
            return
        self.authentication_result = AuthenticationResult(json_result["LoginStatus"], json_result["SecurityToken"],
                                                          json_result["TokenExpirationUtcTime"])

    def authenticate_if(self, needs_authentication):
        if needs_authentication:
            if self.authentication_result is None or not self.authentication_result.is_valid:
                self.authenticate()

    def get_authorization_header(self):
        header_value = "Bearer "
        if self.authentication_result is not None:
            header_value = header_value + self.authentication_result.security_token
        authorization_header = {"Authorization": header_value}
        return authorization_header

    def try_get(self, api_url, needs_authentication = True):
        result = None
        try:
            result = self.get(api_url, needs_authentication)
        except RequestException:
            pass
        return result

    def get(self, api_url, needs_authentication = True):
        self.authenticate_if(needs_authentication)
        authorization_header = None
        if needs_authentication:
            authorization_header = self.get_authorization_header()
        r = requests.get(self.base_uri + api_url, headers=authorization_header, timeout=self.seconds_timeout)
        if r.status_code == 200:
            json_result = r.json()
            return json_result
        else:
            raise RequestException("An exception occurred when making the get request for " + api_url)

    def try_post(self, api_url, data="", needs_authentication = True):
        result = None
        try:
            result = self.post(api_url, data, needs_authentication)
        except RequestException:
            pass
        return result

    def post(self, api_url, data = "", needs_authentication = True):
        self.authenticate_if(needs_authentication)
        authorization_header = None
        if needs_authentication:
            authorization_header = self.get_authorization_header()
        if not data == "" and self.is_json(data):
            if authorization_header is None:
                authorization_header = {}
            authorization_header["content-type"] = "application/json"
        r = requests.post(self.base_uri + api_url, headers=authorization_header, data=data, timeout=self.seconds_timeout)
        if r.status_code == 200:
            json_result = r.json()
            return json_result
        else:
            raise RequestException("An exception occurred when making the post request for " + api_url)

    def is_json(self, json_data):
        try:
            json.loads(json_data)
        except ValueError:
            return False
        return True
