'''
This adapter will contain the functions needed for the service authentication
'''
from rain_it import requests


def authenticate(serial):
    r = requests.get("http://www.google.com")
    return r.text
    