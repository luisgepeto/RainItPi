
import os
from pip._vendor.distlib._backport import shutil

def get_root_path():
    return os.path.abspath(os.sep)

def make_new_dir(root_path, dir_name):
    directory_path = root_path +"\\"+ dir_name
    if os.path.exists(directory_path):
        shutil.rmtree(directory_path)
    os.makedirs(directory_path)
    return directory_path

def write_new_file(dir_path, file_name, file_contents):
    file_path = dir_path +"\\" +file_name
    if os.path.isfile(file_path):
        os.remove(file_path)
    new_file = open(file_path, "w")
    new_file.write(file_contents)
    new_file.close()
    return file_path

def read_file(dir_path, file_name):
    file_path = dir_path +"\\"+ file_name
    new_file = open(file_path, "r+")
    file_content = new_file.read()
    new_file.close()
    return file_content