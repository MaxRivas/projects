import pytest

from src.addition import addition

# Test cases for integer numbers
def test_positive_integernumbers():
    assert addition(2, 3) == 5

def test_negative_integernumbers():
    assert addition(-2, -3) == -5

def test_mixed_integernumbers():
    assert addition(-2, 3) == 1

def test_zero_as_integers():
    assert addition(0, 5) == 5
    assert addition(5, 0) == 5 
    assert addition(0, 0) == 0

def test_identical_integers():
    assert addition(7, 7) == 14

def test_negative_and_zero():
    assert addition(-5, 0) == -5
    assert addition(0, -5) == -5

def test_large_integernumbers():
    assert addition(1_000_000, 2_000_000) == pytest.approx(3_000_000)

# Test cases for floating-point numbers
def test_positivefloats():
    assert addition(2.5, 3.5) == pytest.approx(6.0)

def test_negative_floats():
    assert addition(-2.5, -3.5) == pytest.approx(-6.0)

def test_small_floats():
    assert addition(0.0001, 0.0002) == pytest.approx(0.0003)

def test_large_and_small_floats():
    assert addition(1_000_000, 0.0001) == pytest.approx(1_000_000.0001)

def test_mixed_floats():
    assert addition(-2.5, 3.5) == 1.0

def test_large_floats():
    assert addition(1_000_000.5, 2_000_000.5) == pytest.approx(3_000_001.0)

def test_identical_floats():
    assert addition(7.7, 7.7) == pytest.approx(15.4)

def test_negative_and_small_float():
    assert addition(-5.5, 0.0001) == pytest.approx(-5.4999)

def test_large_and_small_float():
    assert addition(1_000_000.5, 0.0001) == pytest.approx(1_000_000.5001)

# Additional edge cases
def test_addition_commutative():
    assert addition(3, 5) == addition(5, 3)

def test_addition_associative():
    assert addition(addition(2, 3), 4) == addition(2, addition(3, 4))

def test_addition_with_itself():
    assert addition(5, 5) == 10