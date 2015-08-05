'''
This file will have the methods defined for calling the routine methods
'''
import requests
from common import adapter_util
from domain.routine_response import Routine

def get_active_routines(token):
    url = "http://devrainitservices.azurewebsites.net/api/Routine/AllActive"
    authorization_header = adapter_util.get_authorization_header(token)
    r = requests.get(url, headers= authorization_header)
    json_result = r.json()
    routine_list = []
    for result in json_result:
        new_routine = Routine(result["RoutineId"], result["RoutinePatternDTOs"])
        routine_list.append(new_routine)
    return routine_list

def get_test_routines(token):
    url = "http://devrainitservices.azurewebsites.net/api/Routine/AllActive"
    authorization_header = adapter_util.get_authorization_header(token)
    r = requests.get(url, headers= authorization_header)
    json_result = r.json()
    routine_list = []
    for result in json_result:
        new_routine = Routine(result["RoutineId"], result["RoutinePatternDTOs"])
        routine_list.append(new_routine)
    return routine_list

