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
            double _balance;
            AccountType _accountType;
            void invalidateMembers();

        public:
            Account(AccountType type, double balance);
            AccountType getAccountType() const override;
            double getBalance() const override;
            bool deposit(double amount) override;
            bool withdraw(double amount) override;
            int getId() const override;
            bool isValid() const override;
            bool closeAccount(int& accountId) override;
            ~Account() = default;
    };
}

#endif  // ACCOUNT_H