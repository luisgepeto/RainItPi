'''
This file will have the methods defined for calling the routine methods
'''
from rain_it import requests
from rain_it.common import adapter_utils
from rain_it.domain.routine import Routine

def get_active(token):
    url = "http://devrainitservices.azurewebsites.net/api/Routine/AllActive"
    authorization_header = adapter_utils.get_authorization_header(token)
    r = requests.get(url, headers= authorization_header)
    json_result = r.json()
    routine_list = []
    for result in json_result:
        new_routine = Routine(result["RoutineId"], result["RoutinePatternDTOs"])
        routine_list.append(new_routine)
    return routine_list