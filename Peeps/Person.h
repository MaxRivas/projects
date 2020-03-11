#pragma once
#include <string>
#include <memory>
#include "SharedEnums.h"

#ifdef PEEPS_EXPORTS
#define PERSON_API __declspec(dllexport)
#else
#define PERSON_API __declspec(dllimport)
#endif

using namespace std;

class PERSON_API Person {
private:
	string mFirst;
	string mLast;
	Gender mGender;
	shared_ptr<Person> mPersonPtr;

public:
	Person(string first, string last, Gender gender);
	Person();
	Person(const Person& other);
	void PrintInformation();
	bool operator ==(const Person& compare) const;
	void operator =(const Person& assign);
	~Person();
};