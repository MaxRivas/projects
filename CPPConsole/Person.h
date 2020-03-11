#pragma once
#include <string>
#include <memory>
#include "SharedEnums.h"

using namespace std;

class Person {
private:
	string mFirst;
	string mLast;
	Gender mGender;
	shared_ptr<Person> mPersonPtr;

public:
	Person(string first, string last, Gender gender);
	Person();
	Person(const Person& other);
	Person(Person&& other) noexcept;
	void PrintInformation();
	bool operator ==(const Person& compare) const;
	void operator =(const Person& assign);
	~Person();
};