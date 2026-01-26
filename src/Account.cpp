///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Account.cpp
 * @brief implements a bank account
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <Account.h>

namespace bank {

    Account::Account(AccountType type)
    {
        _accountType = type;
    }

    AccountType Account::getAccountType() const
    {
        return _accountType;
    }
}