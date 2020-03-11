#include "pch.h"
#include "Square.h"
#include <iostream>

using namespace std;

Square::Square(double side) {
	m_side = side;
}

double Square::CalculatePerimeter()
{
	return 4 * m_side;
}

double Square::CalculateArea() {
	return m_side * m_side;
}
void Square::DisplayDimensions() {
	cout << "Square side is: " << m_side << endl;
}

Square::~Square() {}