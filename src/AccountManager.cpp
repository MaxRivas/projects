///////////////////////////////////////////////////////////////////////////////
/**
 * @file    AccountManager.cpp
 * @brief implements a bank account manager
 *
 */
///////////////////////////////////////////////////////////////////////////////

#include <AccountManager.h>

namespace bank {


    bool AccountManager::addAccount(std::unique_ptr<IAccount> account)
    {
        bool result = false;
        if (account)
        {
            _accounts.push_back(std::move(account));
            result = true;
        }
        return result;
    }

    bool AccountManager::removeAccount(int accountId)
    {
        bool result = false;
        auto iterator = std::remove_if(_accounts.begin(), _accounts.end(),
                                       [accountId](const std::shared_ptr<IAccount> &account)
                                       {
                                           return account->getId() == accountId;
                                       });
        if (iterator != _accounts.end())
        {
            _accounts.erase(iterator, _accounts.end());
            result = true;
        }
        return result;
    }

    const std::vector<std::shared_ptr<const IAccount>> AccountManager::getAccounts() const
    {
        std::vector<std::shared_ptr<const IAccount>> result;
        for (const auto &account : _accounts)
        {
            result.push_back(account);
        }
        return result;
    }

    bool AccountManager::transferFunds(const AccountType &sourceAccount, const AccountType &targetAccount, const double &amount)
    {
        bool result = false;

        if (validateInputs(sourceAccount, targetAccount, amount))
        {
            
            // Find source and destination accounts
            auto sourceIt = std::find_if(_accounts.begin(), _accounts.end(),
                                        [sourceAccount](const std::shared_ptr<IAccount> &account)
                                        {
                                            return account->getAccountType() == sourceAccount;
                                        });

            auto destinationIt = std::find_if(_accounts.begin(), _accounts.end(),
                                            [targetAccount](const std::shared_ptr<IAccount> &account)
                                            {
                                                return account->getAccountType() == targetAccount;
                                            });

            // Perform transfer if both accounts are found
            if (sourceIt != _accounts.end() && destinationIt != _accounts.end())
            {
                auto &source = *sourceIt;
                auto &destination = *destinationIt;

                if (source->getBalance() > amount) {
                    source->withdraw(amount);
                    destination->deposit(amount);
                    result = true;
                }
            }
        }

        return result;
    }

    bool AccountManager::validateInputs(const AccountType &sourceAccount, const AccountType &targetAccount, const double &amount) const
    {
        bool result = false;
        bool isValidAccount (sourceAccount != AccountType::INVALID &&
                            targetAccount != AccountType::INVALID);
        bool isDifferentAccounts (sourceAccount != targetAccount);
        bool isValidAmount (amount > 0.0);

        return isValidAccount && isDifferentAccounts && isValidAmount;
    }
}