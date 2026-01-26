///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Account.cpp
 * @brief implements a bank account
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <Account.h>
#include <IdGenerator.h>


namespace bank {

    
    Account::Account(AccountType type, double balance)
    {
        if (balance > MIN_INITIAL_BALANCE) {
            _accountType = type;
            _balance = balance;
        } else
        {   
            _accountType = type;
            _balance = 0.0;
            id = bank::generateRandomId();
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
        if (amount > MIN_INITIAL_BALANCE) {
            _balance += amount;
            result = true;
        }

        return result;
    }

    bool Account::withdraw(double amount)
    {
        bool result = false;
        if (amount > MIN_INITIAL_BALANCE && amount <= _balance) {
            _balance -= amount;
            result = true;
        }

        return result;
    }
    
    int Account::getId() const
    {
        return id;
    }
}