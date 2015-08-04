
import os
from pip._vendor.distlib._backport import shutil

def get_routine_root_path():
    return os.path.join(os.path.abspath(os.sep), "Routines")

def make_new_dir(new_dir_path):    
    if is_dir_existent(new_dir_path):
        shutil.rmtree(new_dir_path, ignore_errors=True)
    os.makedirs(new_dir_path)
    return new_dir_path

def make_new_dir_under(dir_path, new_dir_path):
    concat_new_dir_path = os.path.join(dir_path, new_dir_path)
    if is_dir_existent(concat_new_dir_path):
        shutil.rmtree(concat_new_dir_path, ignore_errors=True)
    os.makedirs(concat_new_dir_path)
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
    new_file = open(file_path, "r+")
    file_content = new_file.read()
    new_file.close()
    return file_content

def is_dir_existent(dir_path):
    return os.path.exists(dir_path)

def get_all_dir_under(dir_path):
    return [os.path.join(dir_path, dir_name) for dir_name in os.listdir(dir_path) if os.path.isdir(os.path.join(dir_path, dir_name))]
    
def get_all_files_under(dir_path):
    return [ file_name for file_name in os.listdir(dir_path) if os.path.isfile(os.path.join(dir_path,file_name))]