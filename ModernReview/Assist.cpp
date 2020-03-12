#include "Assist.h"

decltype(auto) Assist::ReturnNormalType()
{
	int x = 0;

	// returns an int due to type deduction
	return x;
}

decltype(auto) Assist::ReturnReferenceType()
{
	int x = 0;

	// retuns a reference to a local int variable
	// which can result in undefined behavior
	return (x);
}