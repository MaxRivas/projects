#include "pch.h"
#include <gtest/gtest.h>
#include "../Shapes/Triangle.h"

//TEST(TestCaseName, TestName) {
TEST(Triangle_Constructor, InvalidArguments) {
    try
	{
		Triangle test(-1.0, 1.0, 1.0);
	}
	catch (const std::exception&)
	{

	}
}

int main(int ac, char* av[])
{
    testing::InitGoogleTest(&ac, av);
    return RUN_ALL_TESTS();
}