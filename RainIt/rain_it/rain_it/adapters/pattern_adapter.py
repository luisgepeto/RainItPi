'''
Created on Aug 3, 2015

@author: luis
'''
from rain_it.common import adapter_utils
from rain_it import requests

def get_pattern_as_matrix(pattern_id, token):
    url = "http://devrainitservices.azurewebsites.net/api/Pattern/Transform?patternId="+str(pattern_id)
    authorization_header = adapter_utils.get_authorization_header(token)
    r = requests.get(url, headers= authorization_header)
    return r.json()