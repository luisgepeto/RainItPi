'''
This will include the common files
'''
def get_authorization_header(token):
    header_value = "Bearer " + token
    authorization_header = {"Authorization": header_value}
    return authorization_header
