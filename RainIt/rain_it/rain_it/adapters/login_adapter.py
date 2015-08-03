'''
This adapter will contain the functions needed for the service authentication
'''
from rain_it import requests
from domain.authentication_response import Authentication_Response

def authenticate(serial):
    r = requests.post("http://devrainitservices.azurewebsites.net/api/Account?serial="+serial)
    json_object = r.json()
    authentication_response = Authentication_Response(json_object["LoginStatus"], json_object["TokenExpirationUtcTime"], json_object["SecurityToken"])
    return authentication_response
    