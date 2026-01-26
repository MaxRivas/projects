#ifndef ACCOUNT_H
#define ACCOUNT_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Account.h
 * @brief specifies a bank account .
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <interfaces/IAccount.h>

namespace bank {
    class Account : public bank::IAccount
    {
        private:
            int id;
            constexpr static double MIN_INITIAL_BALANCE = 0.0;
            double _balance = MIN_INITIAL_BALANCE;
            AccountType _accountType;

        public:
            Account(AccountType type, double balance);
            AccountType getAccountType() const override;
            double getBalance() const override;
            bool deposit(double amount) override;
            bool withdraw(double amount) override;
            int getId() const override;
    };
}

#endif  // ACCOUNT_H