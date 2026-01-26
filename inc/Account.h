///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Account.h
 * @brief specifies a bank account .
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#ifndef ACCOUNT_H
#define ACCOUNT_H

#include <AccountTypes.h>

class Account{
    private:
        AccountTypes _accountType;

    public:
        Account(AccountTypes type): _accountType(type) {};
        AccountTypes getAccountType() const { return _accountType; }
};

#endif  // ACCOUNT_H