'''
This file will have the methods defined for calling the routine methods
'''

from common import adapter_util
from domain.routine import Routine

base_url = "http://localhost:8723/api/"

def get_active_routines(token):
    json_result = adapter_util.make_get_service_call(base_url+"routine/allActive", token)
    routine_list = []
    if json_result is None:
        return routine_list
    for result in json_result:
        new_routine = Routine(result["RoutineId"], result["RoutinePatternDTOs"], result["SampleTimeStamp"])
        routine_list.append(new_routine)
    return routine_list

def get_test_routine_as_list(token):    
    json_result = adapter_util.make_get_service_call(base_url+"routine/test", token)
    routine_list = []
    if json_result is None:
        return routine_list
    new_routine = Routine(json_result["RoutineId"], json_result["RoutinePatternDTOs"], json_result["SampleTimeStamp"])
    routine_list.append(new_routine)
    return routine_list    