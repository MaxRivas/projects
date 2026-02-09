///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Person.cpp
 * @brief implements a person
 *
 */
///////////////////////////////////////////////////////////////////////////////

#include <AccountManager.h>
#include <Constants.h>
#include <Person.h>
#include <Utilities.h>

namespace bank {
    Person::Person(const std::string &name, const int& age)
    {
        if ( age > 0 && bank::Utilities::isValidName(name))
        {
            id = bank::Utilities::generateUniqueId();
            _name = name;
            _age = age;
        }
        else
        {
            id = Constants::INVALID_ID;
            _name = Constants::EMPTY_STRING;
            _age = Constants::INVALID_ZERO;
        }
    }

    std::string Person::getName() const
    {
        return _name;
    }

    int Person::getAge() const
    {
        return _age;
    }

    int Person::getId() const
    {
        return id;
    }
} // namespace bank
