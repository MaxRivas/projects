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
    class Account
    {
        private:
            constexpr static double MIN_INITIAL_BALANCE = 0.0;
            double _balance = MIN_INITIAL_BALANCE;
            AccountType _accountType;

        public:
            Account(AccountType type, double balance);
            AccountType getAccountType() const;
            double getBalance() const;
            bool deposit(double amount);
            bool withdraw(double amount);
    };
}

#endif  // ACCOUNT_H