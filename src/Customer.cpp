///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Customer.cpp
 * @brief implements a customer
 *
 */
///////////////////////////////////////////////////////////////////////////////

#include <Customer.h>

namespace bank
{
    const bank::IPerson* Customer::getPerson() const
    {
        return _person.get();
    }

    const bank::IAccountManager* Customer::getAccountManager() const
    {
        return _accountManager.get();
    }
    
} // namespace bank
