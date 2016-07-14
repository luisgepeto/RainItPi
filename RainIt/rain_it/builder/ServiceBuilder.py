import os
from configparser import ConfigParser
from builder.RainItBuilder import RainItBuilder
from adapter.ServiceAdapter import ServiceAdapter
import json


class ServiceBuilder(RainItBuilder):
    def __init__(self):
        config = ConfigParser()
        config_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', "rainit.config")
        config.read(config_path)
        base_url = config.get("Services", "BaseUrl")
        self.service_adapter = ServiceAdapter(base_url)

    def get_device_settings(self):
        return self.service_adapter.get("device/settings")

    def get_test_pattern(self):
        return self.service_adapter.get("pattern/test")

    def get_test_routine(self):
        return self.service_adapter.get("routine/test")

    def get_active_procedure(self):
        return self.service_adapter.get("routine/active")

    def get_matrix(self, pattern_id, conversion_parameter):
        return self.service_adapter.post("pattern/" + str(pattern_id) + "/transform",
                                         json.dumps(conversion_parameter.get_as_dictionary()))
