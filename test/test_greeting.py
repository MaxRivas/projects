
error_message = "Please provide a string name without special characters or numbers."

from src.greeting import greeting

def test_greeting():
    assert greeting("Alice") == "Hello, Alice!"

def test_greeting_empty_name():
        assert greeting("") == error_message

def test_greeting_numbers_with_casting():
    assert greeting("123") == error_message

def test_greeting_float_with_casting():
    assert greeting("123.45") == error_message

def test_greeting_special_characters():
    assert greeting("John_Doe!") == error_message

def test_greeting_whitespace_only_name():
    assert greeting("   ") == error_message

def test_greeting_mixed_characters():
    assert greeting("Alice123!") == error_message

def test_greeting_none_input():
    assert greeting(None) == error_message

def test_greeting_non_string_input():
    assert greeting(123) == error_message

def test_greeting_valid_name_with_spaces():
    assert greeting("Alice Smith") == error_message

def test_greeting_valid_name_with_hyphen():
    assert greeting("Mary-Jane") == error_message

def test_greeting_valid_name_with_apostrophe():
    assert greeting("O'Connor") == error_message

def test_greeting_long_name():
    long_name = "A" * 1000
    assert greeting(long_name) == f"Hello, {long_name}!"

def test_greeting_unicode_characters():
    assert greeting("José") == error_message

def test_greeting_non_ascii_characters():
    assert greeting("李华") == error_message

def test_greeting_leading_trailing_spaces():
    assert greeting("  Alice  ") == error_message

def test_greeting_tab_characters():
    assert greeting("\tAlice\t") == error_message

def test_greeting_newline_characters():
    assert greeting("\nAlice\n") == error_message

def test_greeting_mixed_whitespace_characters():
    assert greeting(" \t\nAlice \n\t ") == error_message

def test_greeting_single_character_name():
    assert greeting("A") == "Hello, A!" 

def test_greeting_two_character_name():
    assert greeting("Al") == "Hello, Al!"

def test_greeting_name_within_quotes():
    assert greeting('"Alice"') == error_message

def test_greeting_name_with_backslash():
    assert greeting("Alice\\") == error_message

def test_greeting_name_with_slash():
    assert greeting("Alice/") == error_message

def test_greeting_name_with_mixed_case():
    assert greeting("aLiCe") == "Hello, aLiCe!"

def test_greeting_name_with_repeated_characters():
    assert greeting("AAAA") == "Hello, AAAA!"

def test_greeting_name_with_numbers_embedded():
    assert greeting("Al1ce") == error_message

def test_greeting_name_with_special_characters_embedded():
    assert greeting("A!ice") == error_message

def test_greeting_name_with_only_special_characters():
    assert greeting("!@#$%") == error_message

def test_greeting_name_with_leading_number():
    assert greeting("1Alice") == error_message

def test_greeting_name_with_trailing_number():
    assert greeting("Alice1") == error_message

def test_greeting_name_with_all_uppercase():
    assert greeting("ALICE") == "Hello, ALICE!"

def test_greeting_name_with_all_lowercase():
    assert greeting("alice") == "Hello, alice!"