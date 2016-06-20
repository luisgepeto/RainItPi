import os.path
import sys
from rain_it.RainItCommand import RainItCommand
sys.path.append(os.path.join(os.path.dirname(__file__), '..'))

if __name__ == '__main__':
    command = RainItCommand()
    while True:
        command.execute()
