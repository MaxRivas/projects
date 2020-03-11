#include "pch.h"
#include "Circle.h"
#include "ShapeConstants.h"
#include <iostream>

using namespace std;

Circle::Circle(double radius) {
	m_radius = radius;
}

double Circle::CalculatePerimeter()
{
	return Constants::TWO * Constants::PI * m_radius;
}

double Circle::CalculateArea() {
	return Constants::PI * m_radius * m_radius;
}
void Circle::DisplayDimensions() {
	cout << "Circle radius is: " << m_radius << endl;
}

Circle::~Circle() {}