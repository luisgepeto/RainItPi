from builder.RainItBuilder import RainItBuilder
from adapter.ServiceAdapter import ServiceAdapter
from ric.Pattern import Pattern 

class ServiceBuilder(RainItBuilder):
    
    base_url = "http://localhost:8723/api/"
    temporary_token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6IjI2MDcxOTkxMDUwNjE5OTAiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9oYXNoIjoiZjc5ZmEwZjEtMzExZS00MDRkLTkxZTktMzQ4MjI5OGMwNWFjIiwiaXNzIjoiUmFpbkl0VG9rZW5TZXJ2aWNlIiwiYXVkIjoiaHR0cDovL1JhaW5JdFdlYlNlcnZpY2VBcHBsaWNhdGlvbiIsImV4cCI6MTQ0ODczMDI3NCwibmJmIjoxNDQ4MTI1NDc0fQ.5JcJPpeJC6rSyEjcD58PIS0lV8f9ynkCtA3vZ618qnY"
    
    def __init__(self):
        self.service_adapter = ServiceAdapter(self.base_url)           
    
    def get_test_pattern(self):
        return self.service_adapter.get("pattern/test", self.temporary_token)
    
    def get_test_routine(self):         
        return self.service_adapter.get("routine/test", self.temporary_token)
    
    def get_active_procedure(self):
        return self.service_adapter.get("routine/active", self.temporary_token)
    
    def build_pattern(self, pattern_id = 0, conversion_parameter = None, matrix = None, path = None, pattern_factory = None):
        if not matrix:
            matrix = self.get_matrix(pattern_id, conversion_parameter)
        pattern = pattern_factory.get_pattern(pattern_id)
        if pattern is None:
            pattern = Pattern(pattern_id, conversion_parameter, matrix)
            pattern_factory.add_pattern(pattern)    
        return pattern 
    
    def get_matrix(self, pattern_id, conversion_parameter):
        pass
    
    def build_routine(self, pattern_list):
        pass
    
    def build_procedure(self, routine_list):
        pass