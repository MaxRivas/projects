///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Customer.cpp
 * @brief implements a customer
 *
 */
///////////////////////////////////////////////////////////////////////////////

#include <Customer.h>
#include <interfaces/IAccountManager.h>

namespace bank{

    Customer::Customer(const bank::RelationshipTier &tier, const std::string &name, const int &age)
     : Person(name, age), _accountManager(nullptr)
    {
        _tier = tier;
    }
    const bank::IAccountManager* Customer::getAccountManager() const
    {
        return _accountManager.get();
    }

    const bank::RelationshipTier Customer::getTier() const
    {
        return _tier;
    }
} // namespace bank
