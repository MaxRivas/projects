///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Account.cpp
 * @brief implements a bank account
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <Account.h>

namespace bank {

    
    Account::Account(AccountType type, double balance)
    {
        if (balance > MIN_INITIAL_BALANCE) {
            _accountType = type;
            _balance = balance;
        } else
        {   
            _accountType = type;
            _balance = 0.0;
        }
    }

    AccountType Account::getAccountType() const
    {
        return _accountType;
    }

    double Account::getBalance() const
    {
        return _balance;
    }
}