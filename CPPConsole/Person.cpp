#include <string>
#include "Person.h"
#include <iostream>

using namespace std;

Person::Person(string first, string last, Gender gender)
{
	mFirst = first;
	mLast = last;
	mGender = gender;
}

Person::Person() {
	mFirst, mLast = "";
}

Person::Person(const Person& other) : Person(other.mFirst, other.mLast, other.mGender){
}

void Person::PrintInformation() {
	cout << "First Name: " << mFirst << endl;
	cout << "Last Name: " << mLast << endl;
	cout << "Gender: " << static_cast<int>(mGender) << endl;
}

bool Person::operator ==(const Person& compare) const {
	auto result = this->mFirst == compare.mFirst;
	result = result && this->mLast == compare.mLast;
	result = result && this->mGender == compare.mGender;
	
	return result;
}

void Person::operator =(const Person& assign){
	this->mFirst = assign.mFirst;
	this->mLast = assign.mLast;
	this->mGender = assign.mGender;
	this->mPersonPtr = assign.mPersonPtr;
}

Person::~Person() {
	cout << "~Person destructor" << endl;
}