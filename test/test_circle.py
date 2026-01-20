import pytest
import math

from src.circle import Circle

def test_constructor():
    circle = Circle(5)
    assert circle.radius == 5

def test_negative_radius():
    with pytest.raises(ValueError):
        Circle(-3)

def test_set_radius_property_fails():
    circle = Circle(4)
    with pytest.raises(AttributeError):
        circle.radius = 10

def test_area():
    circle = Circle(3)
    expected_area = math.pi * (3 ** 2)
    assert circle.area() == expected_area

def test_circumference():
    circle = Circle(2)
    expected_circumference = 2 * math.pi * 2
    assert circle.circumference() == expected_circumference

def test_diameter():
    circle = Circle(7)
    assert circle.diameter == 14

def test_area_zero_radius():
    circle = Circle(0)
    assert circle.area() == 0

def test_circumference_zero_radius():
    circle = Circle(0)
    assert circle.circumference() == 0

def test_diameter_zero_radius():
    circle = Circle(0)
    assert circle.diameter == 0

def test_large_radius():
    large_radius = 1e6
    circle = Circle(large_radius)
    expected_area = math.pi * (large_radius ** 2)
    expected_circumference = 2 * math.pi * large_radius
    expected_diameter = 2 * large_radius
    assert circle.area() == expected_area
    assert circle.circumference() == expected_circumference
    assert circle.diameter == expected_diameter

def test_float_radius():
    float_radius = 2.5
    circle = Circle(float_radius)
    expected_area = math.pi * (float_radius ** 2)
    expected_circumference = 2 * math.pi * float_radius
    expected_diameter = 2 * float_radius
    assert circle.area() == expected_area
    assert circle.circumference() == expected_circumference
    assert circle.diameter == expected_diameter

def test_radius_immutable():
    circle = Circle(10)
    with pytest.raises(AttributeError):
        circle.radius = 15

def test_diameter_immutable():
    circle = Circle(10)
    with pytest.raises(AttributeError):
        circle.diameter = 25

def test_area_precision():
    circle = Circle(1)
    expected_area = math.pi
    assert math.isclose(circle.area(), expected_area, rel_tol=1e-9)

def test_circumference_precision():
    circle = Circle(1)
    expected_circumference = 2 * math.pi
    assert math.isclose(circle.circumference(), expected_circumference, rel_tol=1e-9)

def test_diameter_precision():
    circle = Circle(1)
    expected_diameter = 2
    assert math.isclose(circle.diameter, expected_diameter, rel_tol=1e-9)

def test_multiple_circles():
    radii = [1, 2, 3, 4, 5]
    circles = [Circle(r) for r in radii]
    for circle, radius in zip(circles, radii):
        assert circle.radius == radius
        assert circle.diameter == 2 * radius
        assert math.isclose(circle.area(), math.pi * (radius ** 2), rel_tol=1e-9)
        assert math.isclose(circle.circumference(), 2 * math.pi * radius, rel_tol=1e-9)

def test_one_radius():
    circle = Circle(1)
    assert circle.radius == 1
    assert circle.diameter == 2
    assert math.isclose(circle.area(), math.pi, rel_tol=1e-9)
    assert math.isclose(circle.circumference(), 2 * math.pi, rel_tol=1e-9)

def test_fractional_radius():
    circle = Circle(0.75)
    expected_area = math.pi * (0.75 ** 2)
    expected_circumference = 2 * math.pi * 0.75
    expected_diameter = 2 * 0.75
    assert math.isclose(circle.area(), expected_area, rel_tol=1e-9)
    assert math.isclose(circle.circumference(), expected_circumference, rel_tol=1e-9)
    assert math.isclose(circle.diameter, expected_diameter, rel_tol=1e-9)

def test_radius_as_string():
    with pytest.raises(TypeError):
        Circle("five")

def test_str_representation():
    circle = Circle(3)
    assert str(circle) == "Circle with radius: 3"