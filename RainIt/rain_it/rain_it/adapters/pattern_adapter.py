'''
Created on Aug 3, 2015

@author: luis
'''
from common import adapter_util
import requests
import json

def get_pattern_as_matrix(pattern_id, conversion_parameter, token):    
    return adapter_util.make_post_service_call("http://devrainitservices.azurewebsites.net/api/pattern/transform?patternId="+str(pattern_id), token, json.dumps(conversion_parameter))  

def get_test_pattern_as_matrix(token):        
    return  adapter_util.make_get_service_call("http://devrainitservices.azurewebsites.net/api/pattern/test", token)   
        