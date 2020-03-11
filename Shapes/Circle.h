#pragma once
#include "IShapeInterface.h"
class Circle :
	public IShapeInterface
{
private:
	double m_radius;

public:
	Circle(double radius);
	double CalculatePerimeter();
	double CalculateArea();
	void DisplayDimensions();
	~Circle();
};

