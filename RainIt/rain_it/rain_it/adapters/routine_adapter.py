'''
This file will have the methods defined for calling the routine methods
'''
import requests
from common import adapter_util
from domain.routine_response import Routine

def get_active_routines(token):
    url = "http://localhost:8723/api/routine/allActive"
    authorization_header = adapter_util.get_authorization_header(token)
    r = requests.get(url, headers= authorization_header)
    json_result = r.json()
    routine_list = []
    if json_result is None:
        return routine_list
    for result in json_result:
        new_routine = Routine(result["RoutineId"], result["RoutinePatternDTOs"])
        routine_list.append(new_routine)
    return routine_list

def get_test_routine_as_list(token):
    url = "http://localhost:8723/api/routine/test"
    authorization_header = adapter_util.get_authorization_header(token)
    r = requests.get(url, headers= authorization_header)
    json_result = r.json()
    routine_list = []
    if json_result is None:
        return routine_list
    new_routine = Routine(json_result["RoutineId"], json_result["RoutinePatternDTOs"])
    routine_list.append(new_routine)
    return routine_list

