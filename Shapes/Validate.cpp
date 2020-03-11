#include "pch.h"
#include "Validate.h"
#include "ShapeConstants.h"
#include <stdexcept>

using namespace std;

void Validate::IsDoubleValueWithinRange(double val) {
	if (val <= Constants::ZERO)
	{
		throw out_of_range(Constants::OUT_OF_RANGE_MSG);
	}
}