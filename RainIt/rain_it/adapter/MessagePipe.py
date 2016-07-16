import os
from configparser import ConfigParser


class MessagePipe(object):
    def __init__(self):
        config = ConfigParser()
        config_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', "rainit.config")
        config.read(config_path)
        self.pipe_path = config.get("Pipe", "PipePath")
        self.pipe = None

    def pipe_initialize(self):
        try:
            if not os.path.exists(self.pipe_path):
                os.mkfifo(self.pipe_path)
            pipe_fd = os.open(self.pipe_path, os.O_RDONLY | os.O_NONBLOCK)
            self.pipe = os.fdopen(pipe_fd)
        except:
            print("An error occurred creating PIPE")

    def read(self):
        if self.pipe is not None:
            return self.pipe.read()
        return ""
