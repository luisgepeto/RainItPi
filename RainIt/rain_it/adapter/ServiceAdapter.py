import requests
import json

from adapter.AuthenticationResult import AuthenticationResult
from adapter.Exceptions import GetRequestException
from adapter.Exceptions import PostRequestException
from adapter.HardwareAdapter import HardwareAdapter


class ServiceAdapter(object):

    def __init__(self, base_uri):
        self.base_uri = base_uri
        self.authentication_result = None

    def authenticate(self):
        serial = HardwareAdapter().get_serial()
        json_result = self.post("account/login/" + serial, needs_authentication=False);
        self.authentication_result = AuthenticationResult(json_result["LoginStatus"], json_result["SecurityToken"],
                                                          json_result["TokenExpirationUtcTime"])

    def authenticate_if(self, needs_authentication):
        if needs_authentication:
            if self.authentication_result is None or not self.authentication_result.is_valid:
                self.authenticate()

    def get_authorization_header(self):
        header_value = "Bearer " + self.authentication_result.security_token
        authorization_header = {"Authorization": header_value}
        return authorization_header

    def get(self, api_url, needs_authentication = True):
        self.authenticate_if(needs_authentication)
        authorization_header = None
        if needs_authentication:
            authorization_header = self.get_authorization_header()
        r = requests.get(self.base_uri + api_url, headers=authorization_header)
        if r.status_code == 200:
            json_result = r.json()
            return json_result
        else:
            raise GetRequestException("An exception occurred when making the get request for " + api_url)

    def post(self, api_url, data = "", needs_authentication = True):
        self.authenticate_if(needs_authentication)
        authorization_header = None
        if needs_authentication:
            authorization_header = self.get_authorization_header()
        if not data == "" and self.is_json(data):
            if authorization_header is None:
                authorization_header = {}
            authorization_header["content-type"] = "application/json"
        r = requests.post(self.base_uri + api_url, headers=authorization_header, data=data)
        if r.status_code == 200:
            json_result = r.json()
            return json_result
        else:
            raise PostRequestException("An exception occurred when making the post request for " + api_url)

    def is_json(self, json_data):
        try:
            json.loads(json_data)
        except ValueError:
            return False
        return True
