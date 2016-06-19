from builder.RainItBuilder import RainItBuilder
from adapter.ServiceAdapter import ServiceAdapter
import json


class ServiceBuilder(RainItBuilder):

    base_url = "http://localhost:100/api/"

    def __init__(self):
        self.service_adapter = ServiceAdapter(self.base_url)

    def get_device_settings(self):
        return self.service_adapter.try_get("device/settings")

    def get_test_pattern(self):
        return self.service_adapter.try_get("pattern/test")

    def get_test_routine(self):
        return self.service_adapter.try_get("routine/test")

    def get_active_procedure(self):
        return self.service_adapter.try_get("routine/active")

    def get_matrix(self, pattern_id, conversion_parameter):
        return self.service_adapter.try_post("pattern/" + str(pattern_id) + "/transform",
                                         json.dumps(conversion_parameter.get_as_dictionary()))
