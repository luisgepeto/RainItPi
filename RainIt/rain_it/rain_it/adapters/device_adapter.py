'''
Created on Sep 14, 2015

@author: luis
'''
from common import adapter_util
from domain.settings import Settings

base_url = "http://localhost:8723/api/"

def get_settings(token):
    json_result = adapter_util.make_get_service_call(base_url+"device/settings", token)
    if json_result is None:
        return None
    settings = Settings(json_result["MinutesRefreshRate"], json_result["MillisecondLatchDelay"],json_result["MillisecondClockDelay"])
    return settings
   

    