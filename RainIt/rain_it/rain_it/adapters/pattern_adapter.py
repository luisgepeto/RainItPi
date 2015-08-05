'''
Created on Aug 3, 2015

@author: luis
'''
from common import adapter_util
import requests
import json

def get_pattern_as_matrix(pattern_id, conversion_parameter, token):
    url = "http://localhost:8723/api/pattern/transform?patternId="+str(pattern_id)
    authorization_header = adapter_util.get_authorization_header(token)
    authorization_header["content-type"]="application/json"
    r = requests.post(url, headers= authorization_header, data=json.dumps(conversion_parameter))
    return r.json()