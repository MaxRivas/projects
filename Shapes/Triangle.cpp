#include "pch.h"
#include "Triangle.h"
#include "ShapeConstants.h"
#include "Validate.h"
#include <iostream>

using namespace std;

Triangle::Triangle(double horizontal, double vertical, double hypotenuse){

	// Ensure the input is valid
	Validate::IsDoubleValueWithinRange(horizontal);
	Validate::IsDoubleValueWithinRange(vertical);
	Validate::IsDoubleValueWithinRange(hypotenuse);

	// assign verified values
	m_horizontal = horizontal;
	m_vertical = vertical;
	m_hypo = hypotenuse;
}

double Triangle::CalculatePerimeter()
{
	return m_vertical + m_horizontal + m_vertical;
}

double Triangle::CalculateArea() {
	return (m_horizontal * m_vertical) / Constants::TWO;
}
void Triangle::DisplayDimensions() {
	cout << "Triangle Base is: " << m_horizontal << endl;
	cout << "Triangle Height is: " << m_vertical << endl;
	cout << "Triangle Hypotenuse is: " << m_hypo << endl;
}

Triangle::~Triangle(){}