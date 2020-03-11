#include "pch.h"
#include "InputValidator.h"
#include "ShapeConstants.h"
#include <stdexcept>

using namespace std;

void InputValidator::IsDoubleValueWithinRange(double val) {
	if (val <= Constants::ZERO)
	{
		throw out_of_range(Constants::OUT_OF_RANGE_MSG);
	}
}