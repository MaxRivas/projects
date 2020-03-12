// ModernReview.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <vector>
#include "Assist.h"

using namespace std;

int main()
{
    vector<int> integers{ 1,2,3,4,5 };
    vector<bool> booleans{ true, false, true, true, true };

    // returns an &[int]
    bool i = integers[1];

    // returns a copy not a &[bool]
    bool b = booleans[1];

    // variable declaration
    Assist assist;

    // const reference to lvalue
    const Assist& constA = assist;

    // type deduced is Assist - for auto it ignores the const reference for lvalue
    auto autoA = constA;

    // type deduced is const Assist& - due to the decltype qualifier since it is asking
    // for the delcare type of the rvalue
    decltype(auto) declare = constA;

    // if uncommenting below lines, compiler complies as expected
    //Assist deduction;
    //auto normal = deduction.ReturnNormalType();
    //auto reference = deduction.ReturnReferenceType();

    std::cout << "Hello World!\n";
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
