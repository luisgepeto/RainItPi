'''
Created on Aug 3, 2015

@author: luis
'''
from common import adapter_util
import requests
import json

def get_pattern_as_matrix(pattern_id, conversion_parameter, token):    
    return adapter_util.make_post_service_call("http://localhost:8723/api/pattern/transform?patternId="+str(pattern_id), token, json.dumps(conversion_parameter))  

def get_test_pattern(token):    
    json_result = adapter_util.make_get_service_call("http://localhost:8723/api/pattern/test", token)    
    routine_list = []
    if json_result is None:
        return routine_list
    new_routine = Routine(json_result["RoutineId"], json_result["RoutinePatternDTOs"])
    routine_list.append(new_routine)
    return routine_list