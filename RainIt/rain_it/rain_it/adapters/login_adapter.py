'''
This adapter will contain the functions needed for the service authentication
'''
import requests
from domain.authentication_response import Authentication_Response

def authenticate(serial):
    r = requests.post("http://localhost:8723/api/account/login?serial="+serial)
    json_object = r.json()
    authentication_response = Authentication_Response(json_object["LoginStatus"], json_object["TokenExpirationUtcTime"], json_object["SecurityToken"])
    return authentication_response
    