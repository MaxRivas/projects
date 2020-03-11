#pragma once
#include "Person.h"
#include <memory>

using namespace std;

class Employee: public Person {
private:
	int mId;
	EmployeeType mType;
	Role mRole;
	shared_ptr<Person> mPersonPtr;

public:
	using Person::Person;
	Employee(string fist, string last, Gender gender, int id);
	Employee(shared_ptr<Person>& person, int id);
	void EstablishType(EmployeeType employeeType);
	void EstablishRole(Role role);
	void PrintInformation();
	~Employee();
};