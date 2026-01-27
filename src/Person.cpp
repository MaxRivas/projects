///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Person.cpp
 * @brief implements a person
 *
 */
///////////////////////////////////////////////////////////////////////////////

#include <Person.h>
#include <Utilities.h>
#include <Constants.h>

namespace bank {
    Person::Person(const std::string &name, int age)
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
            _age = Constants::INVALID_AGE;
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

    bool Person::addAccount(std::unique_ptr<IAccount> account)
    {
        bool result = false;
        if (account)
        {
            _accounts.push_back(std::move(account));
            result = true;
        }
        return result;
    }

    bool Person::removeAccount(int accountId)
    {
        bool result = false;
        auto iterator = std::remove_if(_accounts.begin(), _accounts.end(),
                                 [accountId](const std::shared_ptr<IAccount>& account) {
                                     return account->getId() == accountId;
                                 });
        if (iterator != _accounts.end())
        {
            _accounts.erase(iterator, _accounts.end());
            result = true;
        }
        return result;
    }

    const std::vector<std::shared_ptr<const IAccount>> Person::getAccounts() const
    {
        std::vector<std::shared_ptr<const IAccount>> result;
        for (const auto& account : _accounts)
        {
            result.push_back(account);
        }
        return result;
    }   
} // namespace bank
