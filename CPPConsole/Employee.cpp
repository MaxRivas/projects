#include <string>
#include <iostream>
#include <memory>
#include "Employee.h"
#include "SharedEnums.h"
#include "Person.h"

Employee::Employee(string first, string last, Gender gender, int id) : Person(first, last, gender) {
	mId = id;
	mType = EmployeeType::Unknown;
	mRole = Role::Unknown;
	mPersonPtr = nullptr;
}

Employee::Employee(shared_ptr<Person>& person, int id){
	mPersonPtr = person;
	mId = id;
	mType = EmployeeType::Unknown;
	mRole = Role::Unknown;
}

void Employee::EstablishType(EmployeeType type)
{
	mType = type;
}

void Employee::EstablishRole(Role role)
{
	mRole = role;
}

void Employee::PrintInformation()
{
	if (mPersonPtr == nullptr)
	{
		Person::PrintInformation();
	}
	else
	{
		mPersonPtr->PrintInformation();
	}

	cout << "Employee ID: " << mId << endl;
	cout << "Employee  Type: " << static_cast<int>(mType) << endl;
	cout << "employee role: " << static_cast<int>(mRole) << endl;
}

Employee::~Employee()
{
	cout << "~Employee destructor" << endl;
}
