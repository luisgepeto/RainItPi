import os.path
import sys
sys.path.append(os.path.join(os.path.dirname(__file__), '..'))
from rain_it.RainItCommand import RainItCommand


if __name__ == '__main__':
    command = RainItCommand()
    command.init()
    try:
        while True:
            command.update_components()
            command.print_components()
    except KeyboardInterrupt:
        pass
    finally:
        command.exit()

