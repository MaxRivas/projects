#pragma once
#include "IShapeInterface.h"

class Triangle :
	public IShapeInterface
{
private:
	double m_horizontal;
	double m_vertical;
	double m_hypo;

public:
	Triangle(double horizontal, double vertical, double hypotenuse);
	double CalculatePerimeter();
	double CalculateArea();
	void DisplayDimensions();
	~Triangle();
};

