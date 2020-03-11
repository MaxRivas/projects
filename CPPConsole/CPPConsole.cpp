#include <iostream>
#include "Person.h"
#include "Employee.h"
#include "ComplexEmployee.h"
#include "../Shapes/Triangle.h"
#include "../Shapes/Triangle.cpp"

int main()
{
	using namespace std;

	// instantiation and working with a base class
	auto maleFirst = "Max";
	auto maleLast = "Rivas";

	shared_ptr<Person> personPtr(nullptr);
	personPtr = make_shared<Person>(maleFirst, maleLast, Gender::Male);
	personPtr->PrintInformation();

	// instantiation and working with a derived class
	auto femaleFirst = "Brandi";
	auto FemaleLast = "Pitta";
	unique_ptr<Employee> employeePtr(new Employee(femaleFirst, FemaleLast, Gender::Female, 14));
	employeePtr->PrintInformation();

	auto personCopy = personPtr;
	if (personCopy == personPtr)
	{
		cout << "Objects are equal after assigned after using the shared pointer equal operator" << endl;
	}
	personCopy->PrintInformation();
	
	auto complexPtr = Employee(personPtr, 5);
	complexPtr.PrintInformation();

	Person t1(maleFirst, maleLast, Gender::Male);
	Person t2( maleFirst, maleLast, Gender::Female);
	if (t1 == t2)
	{
		cout << "The two objects are the same." << endl;
	}

	Person copyOp;
	copyOp = t2;
	if (copyOp == t2)
	{
		cout << "objects are equal after assigned using equal operator" << endl;
	}

	auto constCopy(copyOp);
	constCopy.PrintInformation();

	Triangle t(1.0, 1.0, 1.0);
	t.DisplayDimensions();

	return 0;
}