#pragma once
class IShapeInterface
{
public:
	virtual double CalculatePerimeter() = 0;
	virtual double CalculateArea() = 0;
	virtual void DisplayDimensions() = 0;
	virtual ~IShapeInterface() {};
};
