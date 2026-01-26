#ifndef IACCOUNT_H
#define IACCOUNT_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  IAccount.h
 * @brief specifies a bank account interface.
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <AccountType.h>

namespace bank {
    class IAccount
    {
        public:
            virtual AccountType getAccountType() const = 0;
            virtual double getBalance() const = 0;
            virtual bool deposit(double amount) = 0;
            virtual bool withdraw(double amount) = 0;
            virtual int getId() const = 0;
    };
}

#endif  // IACCOUNT_H