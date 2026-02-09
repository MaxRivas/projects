///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Account.cpp
 * @brief implements a bank account
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <Account.h>
#include <Utilities.h>
#include <Constants.h>

namespace bank {

    
    Account::Account(AccountType type, double balance)
    {
        if (balance > Constants::MIN_BALANCE)
        {
            _accountType = type;
            _balance = balance;
            id = bank::Utilities::generateUniqueId();
        }
        else
        {
            invalidateMembers();
        }
    }

    AccountType Account::getAccountType() const
    {
        return _accountType;
    }

    double Account::getBalance() const
    {
        return _balance;
    }

    bool Account::deposit(double amount)
    {
        bool result = false;
        if (amount > Constants::MIN_BALANCE) {
            _balance += amount;
            result = true;
        }

        return result;
    }

    bool Account::withdraw(double amount)
    {
        bool result = false;
        if (amount > Constants::MIN_BALANCE && (_balance - amount) >= Constants::MIN_BALANCE) {
            _balance -= amount;
            result = true;
        }

        return result;
    }

    int Account::getId() const
    {
        return id;
    }

    bool Account::isValid() const
    {
        return _accountType != bank::AccountType::INVALID;
    }

    bool Account::closeAccount(int& accountId)
    {
        bool result = false;
        if (isValid() && accountId == id)
        {
            invalidateMembers();
            result = true;
        }
        return result;
    }

    void Account::invalidateMembers()
    {
        _accountType = bank::AccountType::INVALID;
        _balance = bank::Constants::INVALID_BALANCE;
        id = Constants::INVALID_ID;
    }
}