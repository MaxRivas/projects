#ifndef IACCOUNT_MANAGER_H
#define IACCOUNT_MANAGER_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  IAccountManager.h
 * @brief specifies an interface for a bank account manager.
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <memory>
#include <vector>

#include <AccountType.h>
#include <interfaces/IAccount.h>

namespace bank {
    class IAccountManager
    {
        public:
            virtual bool addAccount(std::unique_ptr<IAccount> account) = 0;
            virtual bool removeAccount(int accountId) = 0;
            virtual const std::vector<std::shared_ptr<const IAccount>> getAccounts() const = 0;
            virtual bool transferFunds(const AccountType &sourceAccount, const AccountType &targetAccount, const double &amount) = 0;
            //virtual bool billPayment(const int& accountId, const double& amount) = 0;
            //virtual bool cardPurchase(const int &accountId, const double &amount) = 0;
    };
}
#endif  // IACCOUNT_MANAGER_H