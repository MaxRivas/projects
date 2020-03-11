#pragma once
#include "IShapeInterface.h"
class Square :
	public IShapeInterface
{
private:
	double m_side;

public:
	Square(double side);
	double CalculatePerimeter();
	double CalculateArea();
	void DisplayDimensions();
	~Square();
};
