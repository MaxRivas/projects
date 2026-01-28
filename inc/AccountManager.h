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

#include <AccountType.h>
#include <interfaces/IAccount.h>
#include <interfaces/IAccountManager.h>

namespace bank {
    class AccountManager : public IAccountManager
    {
        private:
            std::vector<std::shared_ptr<IAccount>> _accounts;
            bool validateInputs(const AccountType &sourceAccount, const AccountType &targetAccount, const double &amount) const;

        public:
            virtual bool addAccount(std::unique_ptr<IAccount> account) override;
            virtual bool removeAccount(int accountId) override;
            virtual const std::vector<std::shared_ptr<const IAccount>> getAccounts() const override;
            virtual bool transferFunds(const AccountType &sourceAccount, const AccountType &targetAccount, const double &amount) override;
            //virtual bool billPayment(const int& accountId, const double& amount)override;
            //virtual bool cardPurchase(const int &accountId, const double &amount) override;
            virtual ~AccountManager() = default;
    };
}
#endif  // ACCOUNT_MANAGER_H