
import os, time
import shutil
from datetime import datetime, timedelta

def get_routine_root_path():
    return os.path.join(os.path.abspath(os.sep), "home/pi/Routines")

def get_test_routine_root_path():
    return os.path.join(os.path.abspath(os.sep), "home/pi/Test/Routines")

def get_test_pattern_root_path():
    return os.path.join(os.path.abspath(os.sep), "home/pi/Test/Pattern")

def make_new_dir(new_dir_path):    
    if is_dir_existent(new_dir_path):
        shutil.rmtree(new_dir_path, ignore_errors=True)
    try:
        os.makedirs(new_dir_path)
    except OSError:
        pass
    return new_dir_path

def make_new_dir_under(dir_path, new_dir_path):
    concat_new_dir_path = os.path.join(dir_path, new_dir_path)
    if is_dir_existent(concat_new_dir_path):
        shutil.rmtree(concat_new_dir_path, ignore_errors=True)
    try:
        os.makedirs(concat_new_dir_path)
    except OSError:
        pass
    return concat_new_dir_path

def write_new_file(dir_path, file_name, file_contents):
    file_path = os.path.join(dir_path, file_name)
    if os.path.isfile(file_path):
        os.remove(file_path)
    new_file = open(file_path, "w")
    new_file.write(file_contents)
    new_file.close()    
    return file_path

def read_file(dir_path, file_name):
    file_path = os.path.join(dir_path, file_name)
    file_content = ""
    try:
        new_file = open(file_path, "r+")
        file_content = new_file.read()
        new_file.close()
    except (OSError, IOError):
        pass
    return file_content

def is_dir_existent(dir_path):
    return os.path.exists(dir_path)

def get_all_dir_under(dir_path):
    return [os.path.join(dir_path, dir_name) for dir_name in os.listdir(dir_path) if os.path.isdir(os.path.join(dir_path, dir_name))]
    
def get_all_files_under(dir_path):
    return [ file_name for file_name in os.listdir(dir_path) if os.path.isfile(os.path.join(dir_path,file_name))]

def add_sample_timestamp_file(dir_path, timestamp):
    current_utc_date = "1970-01-01T00:00:00.0"
    if not(timestamp == "" or timestamp == None):
        current_utc_date = timestamp    
    write_new_file(dir_path, "sample_timestamp", current_utc_date)
    
def add_timestamp_file(dir_path):
    current_utc_date = datetime.utcnow().strftime("%Y-%m-%dT%H:%M:%S.%f")
    write_new_file(dir_path, "timestamp", current_utc_date)

def get_timestamp_from(dir_path):
    timestamp_from_file = read_file(dir_path, "timestamp")
    if timestamp_from_file == "" :   
        timestamp_from_file = "1970-01-01T00:00:00.0"       
    return datetime.fromtimestamp(time.mktime(time.strptime(timestamp_from_file, "%Y-%m-%dT%H:%M:%S.%f")))

def is_dir_valid(dir_path, expiration_minutes):
    routine_timestamp = get_timestamp_from(dir_path)
    return  routine_timestamp + timedelta(minutes = expiration_minutes) > datetime.utcnow()

def get_sampletimestamp_from(dir_path):
    timestamp_from_file = read_file(dir_path, "sample_timestamp")
    if timestamp_from_file == "" :   
        timestamp_from_file = "1970-01-01T00:00:00.0"       
    return datetime.fromtimestamp(time.mktime(time.strptime(timestamp_from_file, "%Y-%m-%dT%H:%M:%S.%f")))
        