///////////////////////////////////////////////////////////////////////////////
/**
 * @file    AccountManager.cpp
 * @brief implements a bank account manager
 *
 */
///////////////////////////////////////////////////////////////////////////////

#include <Constants.h>
#include <AccountManager.h>

namespace bank {

    bool AccountManager::addAccount(std::unique_ptr<IAccount> account)
    {
        bool result = false;
        if (account && account->isValid())
        {
            _accounts.push_back(std::move(account));
            result = true;
        }

        return result;
    }

    bool AccountManager::removeAccount(int accountId)
    {
        bool result = false;
        auto account = getAccountById(accountId);
        if (account != _accounts.end())
        {
            _accounts.erase(account);
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

    bool AccountManager::transferFunds(const int& sourceAccount, const int& targetAccount, const double &amount)
    {
        bool result = false;

        if (validateInputs(sourceAccount, targetAccount, amount))
        {
            
            // Find source and destination accounts
            auto sourceIt = getAccountById(sourceAccount);
            auto destinationIt = getAccountById(targetAccount);

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

    bool AccountManager::billPayment(const int &accountId, const double &amount)
    {
        bool result = false;
        auto accountIt = getAccountById(accountId);

        if (accountIt != _accounts.end() && isAmountValid(amount))
        {
            auto &account = *accountIt;
            if (account->getBalance() > amount)
            {
                account->withdraw(amount);
                result = true;
            }
        }

        return result;
    }

    bool AccountManager::cardPurchase(const int &accountId, const double &amount)
    {
        bool result = false;
        auto accountIt = getAccountById(accountId);

        if (accountIt != _accounts.end() && isAmountValid(amount))
        {
            auto &account = *accountIt;
            if (account->getBalance() > amount)
            {
                account->withdraw(amount);
                result = true;
            }
        }

        return result;
    }

    bool AccountManager::validateInputs(const int& sourceAccount, const int& targetAccount, const double &amount) const
    {
        bool result = false;
        bool isValidAccount (sourceAccount > Constants::INVALID_ZERO &&
                            targetAccount > Constants::INVALID_ZERO);
        bool areDifferentAccounts (sourceAccount != targetAccount);
        bool isValidAmount = isAmountValid(amount);

        return isValidAccount && areDifferentAccounts && isValidAmount;
    }

    std::vector<std::shared_ptr<IAccount>>::const_iterator AccountManager::getAccountById(int accountId) const
    {
        return std::find_if(_accounts.begin(), _accounts.end(),
                            [accountId](const std::shared_ptr<IAccount> &account)
                            {
                                return account->getId() == accountId;
                            });
    }

    bool AccountManager::isAmountValid(const double &amount) const
    {
        return amount > Constants::MIN_BALANCE;
    }
}
