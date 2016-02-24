from builder.RainItBuilder import RainItBuilder
from adapter.ServiceAdapter import ServiceAdapter
import json

class ServiceBuilder(RainItBuilder):
    
<<<<<<< HEAD
    base_url = "http://localhost:8723/api/"
    temporary_token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6IjI2MDcxOTkxMDUwNjE5OTAiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9oYXNoIjoiZjc5ZmEwZjEtMzExZS00MDRkLTkxZTktMzQ4MjI5OGMwNWFjIiwiaXNzIjoiUmFpbkl0VG9rZW5TZXJ2aWNlIiwiYXVkIjoiaHR0cDovL1JhaW5JdFdlYlNlcnZpY2VBcHBsaWNhdGlvbiIsImV4cCI6MTQ0ODc1OTE2NiwibmJmIjoxNDQ4MTU0MzY2fQ.ljPDrc3i_fEYiz8wrUeKTcmZdtRH5Nqc5pJIpAgnezA"
=======
    base_url = "http://devrainitservices.azurewebsites.net/api/"
    temporary_token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6ImFiY2RlZjEyMzQ1Njc4OTAiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9oYXNoIjoiNDZmM2IwNGEtMjc1Mi00ZTI1LWJlODYtMTMzYjI2Y2Y3YjA4IiwiaXNzIjoiUmFpbkl0VG9rZW5TZXJ2aWNlIiwiYXVkIjoiaHR0cDovL1JhaW5JdFdlYlNlcnZpY2VBcHBsaWNhdGlvbiIsImV4cCI6MTQ0OTE2ODIxNCwibmJmIjoxNDQ4NTYzNDE0fQ.h9r-9TyDzlWKImhftnMhiZGzX3Vc0hS3PuM-aCFI9bE"
>>>>>>> branch 'dev' of https://luisgepeto@bitbucket.org/luisgepeto/rainitpi.git
    
    def __init__(self):
        self.service_adapter = ServiceAdapter(self.base_url)           
    
    def get_test_pattern(self):
        return self.service_adapter.get("pattern/test", self.temporary_token)
    
    def get_test_routine(self):         
        return self.service_adapter.get("routine/test", self.temporary_token)
    
    def get_active_procedure(self):
        return self.service_adapter.get("routine/active", self.temporary_token)
    
    def get_matrix(self, pattern_id, conversion_parameter):
        return self.service_adapter.post("pattern/transform?patternId="+str(pattern_id), self.temporary_token, json.dumps(conversion_parameter.get_as_dictionary()))
    
    