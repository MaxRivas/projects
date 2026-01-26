#ifndef ACCOUNT_H
#define ACCOUNT_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Account.h
 * @brief specifies a bank account .
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <AccountType.h>

namespace bank {
    class Account{
        private:
            AccountType _accountType;

        public:
            Account(AccountType type);
            AccountType getAccountType() const;
    };
}

#endif  // ACCOUNT_H