'''
This adapter will contain the functions needed for the service authentication
'''
import requests
from domain.authentication import Authentication

base_url = "http://localhost:8723/api/"
def authenticate(serial):
    r = requests.post(base_url+"account/login?serial="+serial)
    json_object = r.json()
    authentication_response = Authentication(json_object["LoginStatus"], json_object["TokenExpirationUtcTime"], json_object["SecurityToken"])
    return authentication_response
    