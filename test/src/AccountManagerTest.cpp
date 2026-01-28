////////////////////////////////////////////////////////////////////////////
/**
 *  @file   AccountManagerTest.cpp
 *  @brief  Unit tests for AccountManager class
 */
////////////////////////////////////////////////////////////////////////////

#include <Account.h>
#include <AccountManager.h>
#include <../inc/interfaces/IAccount.h>

#include <gtest/gtest.h>
#include <gmock/gmock.h>
#include <memory>

using namespace testing;

class Bank_AccountManager_Tests : public Test
{
    protected:
        void SetUp() override
        {
            // Code here will be called immediately after the constructor (right
            // before each test).

            _accountManager = std::make_unique<bank::AccountManager>();
        }

        void TearDown() override
        {
            // Code here will be called immediately after each test (right
            // before the destructor).
        }

        std::unique_ptr<bank::AccountManager> _accountManager;

        private:
            std::unique_ptr<bank::IAccount> createSavingsAccount(double initialBalance)
            {
                return std::make_unique<bank::Account>(bank::AccountType::SAVINGS, initialBalance);
            }

            std::unique_ptr<bank::IAccount> createCheckingAccount(double initialBalance)
            {
                return std::make_unique<bank::Account>(bank::AccountType::CHECKING, initialBalance);
            }

            std::unique_ptr<bank::IAccount> createInvalidAccount()
            {
                return std::make_unique<bank::Account>(bank::AccountType::INVALID, -100.0);
            }
};

TEST_F(Bank_AccountManager_Tests, Test_TransferFunds_SameAccountType)
{
    bank::AccountType sourceAccount = bank::AccountType::SAVINGS;
    bank::AccountType targetAccount = bank::AccountType::SAVINGS;
    double amount = -100.0;

    bool result = _accountManager->transferFunds(sourceAccount, targetAccount, amount);
    EXPECT_FALSE(result);
}

TEST_F(Bank_AccountManager_Tests, Test_TransferFunds_InvalidAmount)
{
    bank::AccountType sourceAccount = bank::AccountType::CHECKING;
    bank::AccountType targetAccount = bank::AccountType::SAVINGS;
    double amount = -50.0;

    bool result = _accountManager->transferFunds(sourceAccount, targetAccount, amount);
    EXPECT_FALSE(result);
}

TEST_F(Bank_AccountManager_Tests, Test_TransferFunds_InvalidDestinationAccounts)
{
    bool result = _accountManager->transferFunds(bank::AccountType::INVALID, bank::AccountType::SAVINGS, 100.0);
    EXPECT_FALSE(result);
}

TEST_F(Bank_AccountManager_Tests, Test_TransferFunds_InvalidSourceAccounts)
{
    bool result = _accountManager->transferFunds(bank::AccountType::SAVINGS, bank::AccountType::INVALID, 100.0);
    EXPECT_FALSE(result);
}

TEST_F(Bank_AccountManager_Tests, Test_TransferFunds_BothInvalidAccounts)
{
    bool result = _accountManager->transferFunds(bank::AccountType::INVALID, bank::AccountType::INVALID, 100.0);
    EXPECT_FALSE(result);
}