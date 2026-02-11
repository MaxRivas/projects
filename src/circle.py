import math

class Circle:
    def __init__(self, radius):
        if radius < 0:
            raise ValueError("Radius cannot be negative")
        self._radius = radius

    @property
    def radius(self):
        return self._radius
    
    @property
    def diameter(self):
        return 2 * self._radius

    def area(self):
        return math.pi * (self._radius ** 2)
    
    def circumference(self):
        return math.pi * self.diameter
    
    def __str__(self):
        return f"Circle with radius: {self._radius}"