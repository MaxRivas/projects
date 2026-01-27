#ifndef ACCOUNT_MANAGER_H
#define ACCOUNT_MANAGER_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  AccountManager.h
 * @brief specifies a bank account manager.
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <memory>
#include <vector>

#include <interfaces/IAccount.h>
#include <AccountType.h>

namespace bank {
    class AccountManager
    {
        private:
            std::vector<std::shared_ptr<IAccount>> _accounts;
            bool validateInputs(const AccountType &sourceAccount, const AccountType &targetAccount, const double &amount) const;

        public:
            virtual bool addAccount(std::unique_ptr<IAccount> account) = 0;
            virtual bool removeAccount(int accountId) = 0;
            virtual const std::vector<std::shared_ptr<const IAccount>> getAccounts() const = 0;
            virtual bool transferFunds(const AccountType &sourceAccount, const AccountType &targetAccount, const double &amount) = 0;
            //virtual bool billPayment(const int& accountId, const double& amount) = 0;
            //virtual bool cardPurchase(const int &accountId, const double &amount) = 0;
            virtual ~AccountManager() = default;
    };
}
#endif  // ACCOUNT_MANAGER_H