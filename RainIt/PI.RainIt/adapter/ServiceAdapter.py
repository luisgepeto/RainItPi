import requests
import json
from adapter.ServiceAdapter import PostRequestException
from adapter.ServiceAdapter import GetRequestException

class ServiceAdapter(object):
    
    def __init__(self, base_uri):
        self.base_uri = base_uri
        
    def get_authorization_header(self, token):
        header_value = "Bearer " + token
        authorization_header = {"Authorization": header_value}
        return authorization_header
    
    def get(self, api_url, token):
        authorization_header = self.get_authorization_header(token)    
        r = requests.get(self.base_uri+api_url, headers= authorization_header)
        if r.status_code == 200:
            json_result = r.json()
            return json_result
        else:
            raise GetRequestException("An exception occurred when making the get request for "+api_url)

    def post(self, api_url, token, data):
        authorization_header = self.get_authorization_header(token)
        if not data == "" and self.is_json(data):
            authorization_header["content-type"]="application/json"
        r = requests.post(self.base_uri+api_url, headers= authorization_header, data = data)
        if r.status_code == 200:
            json_result = r.json()
            return json_result
        else:
            raise PostRequestException("An exception occurred when making the post request for "+api_url)
        
    def is_json(self, json_data):
        try:
            json.loads(json_data)
        except ValueError:
            return False
        return True