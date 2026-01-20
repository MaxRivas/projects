import re

def greeting(name: str) -> str:
    if not isinstance(name, str) or not name or not __is_ascii_alpha(name):
        return "Please provide a string name without special characters or numbers."
    
    return f"Hello, {name}!"


def __is_ascii_alpha(input_string):
    # Checks if the string contains only ASCII letters.
    # The 'r' prefix creates a raw string, which is recommended for regex in Python.
    pattern = r'^[a-zA-Z]+$'
    # re.fullmatch checks if the ENTIRE string matches the pattern.
    if re.fullmatch(pattern, input_string):
        return True
    else:
        return False