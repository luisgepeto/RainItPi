'''
This will include the common files
'''
import requests
import json
from domain.exceptions import GetRequestException, PostRequestException

def get_authorization_header(token):
    header_value = "Bearer " + token
    authorization_header = {"Authorization": header_value}
    return authorization_header

def is_json(json_data):
    try:
        json_object = json.loads(json_data)
    except ValueError as  e:
        return False
    return True

def make_get_service_call(url, token):
    authorization_header = get_authorization_header(token)    
    r = requests.get(url, headers= authorization_header)
    if r.status_code == 200:
        json_result = r.json()
        return json_result
    else:
        raise GetRequestException("An exception occurred when making the get request for "+url)

def make_post_service_call(url, token, data):
    authorization_header = get_authorization_header(token)
    if not data == "" and is_json(data):
        authorization_header["content-type"]="application/json"
    r = requests.post(url, headers= authorization_header, data = data)
    if r.status_code == 200:
        json_result = r.json()
        return json_result
    else:
        raise PostRequestException("An exception occurred when making the post request for "+url)