'''
Created on Aug 3, 2015

@author: luis
'''
from common import adapter_util
import requests
import json

base_url = "http://devrainit.azurewebsites.net/api/"

def get_pattern_as_matrix(pattern_id, conversion_parameter, token):    
    return adapter_util.make_post_service_call(base_url+"pattern/transform?patternId="+str(pattern_id), token, json.dumps(conversion_parameter))  

def get_test_pattern_as_matrix(token):        
    return  adapter_util.make_get_service_call(base_url+"pattern/test", token)   
        