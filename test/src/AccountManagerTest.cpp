////////////////////////////////////////////////////////////////////////////
/**
 *  @file   AccountManagerTest.cpp
 *  @brief  Unit tests for AccountManager class
 */
////////////////////////////////////////////////////////////////////////////

#include <Account.h>
#include <AccountManager.h>
#include <../inc/interfaces/IAccount.h>
#include <Constants.h>

#include <gtest/gtest.h>
#include <gmock/gmock.h>
#include <memory>

using namespace testing;

class Bank_AccountManager_Tests : public Test
{
    protected:
    
        std::unique_ptr<bank::AccountManager> _accountManager;
        const float FIVE_HUNDRED = 500.0;
        const float ONE_THOUSAND = 1000.0;
        const float MINUS_FIFTY = -50.0;
        const float ONE_HUNDRED = 100.0;
        const int RANDOM_ACCOUNT_ID = 8765;

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
            return std::make_unique<bank::Account>(bank::AccountType::INVALID, MINUS_FIFTY);
        }
};

/** 
 * Test cases for Add account
 */
TEST_F(Bank_AccountManager_Tests, AddAccount_Success)
{
    auto account = Bank_AccountManager_Tests::createSavingsAccount(ONE_THOUSAND);
    EXPECT_TRUE(_accountManager->addAccount(std::move(account)));
}

TEST_F(Bank_AccountManager_Tests, AddAccount_Nullptr)
{
    EXPECT_FALSE(_accountManager->addAccount(nullptr));
}

TEST_F(Bank_AccountManager_Tests, AddAccount_InvalidAccount)
{
    auto account = Bank_AccountManager_Tests::createInvalidAccount();
    EXPECT_FALSE(_accountManager->addAccount(std::move(account)));
}

/** 
 * Test cases for Remove account
 */
TEST_F(Bank_AccountManager_Tests, RemoveAccount_Success)
{
    auto account = Bank_AccountManager_Tests::createCheckingAccount(FIVE_HUNDRED);
    _accountManager->addAccount(std::move(account));
    auto accounts = _accountManager->getAccounts();
    EXPECT_FALSE(accounts.empty());

    int accountId = accounts.front()->getId();
    EXPECT_TRUE(_accountManager->removeAccount(accountId));
}

TEST_F(Bank_AccountManager_Tests, RemoveAccount_NotFound)
{
    EXPECT_FALSE(_accountManager->removeAccount(RANDOM_ACCOUNT_ID));
}

TEST_F(Bank_AccountManager_Tests, RemoveAccount_AlreadyRemoved)
{
    auto account = Bank_AccountManager_Tests::createCheckingAccount(FIVE_HUNDRED);
    _accountManager->addAccount(std::move(account));
    auto accounts = _accountManager->getAccounts();
    EXPECT_FALSE(accounts.empty());

    int accountId = accounts.front()->getId();
    EXPECT_TRUE(_accountManager->removeAccount(accountId)); // First removal should succeed
    EXPECT_FALSE(_accountManager->removeAccount(accountId)); // Second removal should fail
}

TEST_F(Bank_AccountManager_Tests, RemoveAccount_EmptyManager)
{
    EXPECT_FALSE(_accountManager->removeAccount(RANDOM_ACCOUNT_ID)); // Assuming RANDOM_ACCOUNT_ID is an invalid ID
}

/** 
 * Test cases for Get accounts
 */
TEST_F(Bank_AccountManager_Tests, GetAccounts_Empty)
{
    auto accounts = _accountManager->getAccounts();
    EXPECT_TRUE(accounts.empty());
}

TEST_F(Bank_AccountManager_Tests, GetAccounts_NonEmpty)
{
    auto account1 = Bank_AccountManager_Tests::createSavingsAccount(ONE_THOUSAND);
    auto account2 = Bank_AccountManager_Tests::createCheckingAccount(FIVE_HUNDRED);
    _accountManager->addAccount(std::move(account1));
    _accountManager->addAccount(std::move(account2));

    auto accounts = _accountManager->getAccounts();
    EXPECT_EQ(accounts.size(), 2);
}

/** 
 * Test cases for Transfer funds
 */
TEST_F(Bank_AccountManager_Tests, TransferFunds_Success)
{
    auto account1 = Bank_AccountManager_Tests::createSavingsAccount(ONE_THOUSAND);
    bool added = _accountManager->addAccount(std::move(account1));
    EXPECT_TRUE(added);
    auto account2 = Bank_AccountManager_Tests::createCheckingAccount(FIVE_HUNDRED);
    added = _accountManager->addAccount(std::move(account2));
    EXPECT_TRUE(added);
    
    auto accounts = _accountManager->getAccounts();
    int sourceId = accounts[0]->getId();
    int targetId = accounts[1]->getId();

    EXPECT_TRUE(_accountManager->transferFunds(sourceId, targetId, 200.0));
    EXPECT_FLOAT_EQ(accounts[0]->getBalance(), 800.0);
    EXPECT_FLOAT_EQ(accounts[1]->getBalance(), 700.0);
}

TEST_F(Bank_AccountManager_Tests, TransferFunds_InvalidAmount)
{
    auto account1 = Bank_AccountManager_Tests::createSavingsAccount(ONE_THOUSAND);
    auto account2 = Bank_AccountManager_Tests::createCheckingAccount(FIVE_HUNDRED);
    _accountManager->addAccount(std::move(account1));
    _accountManager->addAccount(std::move(account2));

    auto accounts = _accountManager->getAccounts();
    int sourceId = accounts[0]->getId();
    int targetId = accounts[1]->getId();

    EXPECT_FALSE(_accountManager->transferFunds(sourceId, targetId, MINUS_FIFTY)); // Invalid amount
    EXPECT_FLOAT_EQ(accounts[0]->getBalance(), ONE_THOUSAND); // Balance should remain unchanged
    EXPECT_FLOAT_EQ(accounts[1]->getBalance(), FIVE_HUNDRED);  // Balance should remain unchanged
}

TEST_F(Bank_AccountManager_Tests, TransferFunds_InsufficientFunds)
{
    auto account1 = Bank_AccountManager_Tests::createSavingsAccount(ONE_THOUSAND);
    auto account2 = Bank_AccountManager_Tests::createCheckingAccount(FIVE_HUNDRED);
    _accountManager->addAccount(std::move(account1));
    _accountManager->addAccount(std::move(account2));

    auto accounts = _accountManager->getAccounts();
    int sourceId = accounts[0]->getId();
    int targetId = accounts[1]->getId();

    EXPECT_FALSE(_accountManager->transferFunds(sourceId, targetId, 1500.0)); // Insufficient funds
    EXPECT_FLOAT_EQ(accounts[0]->getBalance(), ONE_THOUSAND); // Balance should remain unchanged
    EXPECT_FLOAT_EQ(accounts[1]->getBalance(), FIVE_HUNDRED);  // Balance should remain unchanged
}

TEST_F(Bank_AccountManager_Tests, TransferFunds_SameAccount)
{
    auto account = Bank_AccountManager_Tests::createSavingsAccount(ONE_THOUSAND);
    _accountManager->addAccount(std::move(account));

    auto accounts = _accountManager->getAccounts();
    int accountId = accounts[0]->getId();

    EXPECT_FALSE(_accountManager->transferFunds(accountId, accountId, ONE_HUNDRED)); // Same account
    EXPECT_FLOAT_EQ(accounts[0]->getBalance(), ONE_THOUSAND); // Balance should remain unchanged
}

TEST_F(Bank_AccountManager_Tests, TransferFunds_AccountNotFound)
{
    auto account = Bank_AccountManager_Tests::createSavingsAccount(ONE_THOUSAND);
    _accountManager->addAccount(std::move(account));

    auto accounts = _accountManager->getAccounts();
    int sourceId = accounts[0]->getId();

    EXPECT_FALSE(_accountManager->transferFunds(sourceId, RANDOM_ACCOUNT_ID, ONE_HUNDRED)); // Target account not found
    EXPECT_FALSE(_accountManager->transferFunds(RANDOM_ACCOUNT_ID, sourceId, ONE_HUNDRED)); // Source account not found
}

TEST_F(Bank_AccountManager_Tests, TransferFunds_InvalidAccounts)
{
    auto account1 = Bank_AccountManager_Tests::createInvalidAccount();
    bool added1 = _accountManager->addAccount(std::move(account1));
    auto account2 = Bank_AccountManager_Tests::createInvalidAccount();
    bool added2 = _accountManager->addAccount(std::move(account2));

    // Invalid accounts should not be added
    EXPECT_FALSE(added1);
    EXPECT_FALSE(added2);

    auto accounts = _accountManager->getAccounts();
    EXPECT_TRUE(accounts.empty());
}

/**
 * Test cases for Bill payment
 */
TEST_F(Bank_AccountManager_Tests, BillPayment_Success)
{
    auto account = Bank_AccountManager_Tests::createCheckingAccount(FIVE_HUNDRED);
    _accountManager->addAccount(std::move(account));

    auto accounts = _accountManager->getAccounts();
    int accountId = accounts[0]->getId();

    EXPECT_TRUE(_accountManager->billPayment(accountId, ONE_HUNDRED));
    EXPECT_FLOAT_EQ(accounts[0]->getBalance(), 400.0);
}

TEST_F(Bank_AccountManager_Tests, BillPayment_InvalidAmount)
{
    auto account = Bank_AccountManager_Tests::createCheckingAccount(FIVE_HUNDRED);
    _accountManager->addAccount(std::move(account));

    auto accounts = _accountManager->getAccounts();
    int accountId = accounts[0]->getId();

    EXPECT_FALSE(_accountManager->billPayment(accountId, MINUS_FIFTY)); // Invalid amount
    EXPECT_FLOAT_EQ(accounts[0]->getBalance(), FIVE_HUNDRED);  // Balance should remain unchanged
}

TEST_F(Bank_AccountManager_Tests, BillPayment_InsufficientFunds)
{
    auto account = Bank_AccountManager_Tests::createCheckingAccount(FIVE_HUNDRED);
    _accountManager->addAccount(std::move(account));

    auto accounts = _accountManager->getAccounts();
    int accountId = accounts[0]->getId();

    EXPECT_FALSE(_accountManager->billPayment(accountId, 600.0)); // Insufficient funds
    EXPECT_FLOAT_EQ(accounts[0]->getBalance(), FIVE_HUNDRED);  // Balance should remain unchanged
}

TEST_F(Bank_AccountManager_Tests, BillPayment_AccountNotFound)
{
    EXPECT_FALSE(_accountManager->billPayment(RANDOM_ACCOUNT_ID, ONE_HUNDRED)); // Assuming RANDOM_ACCOUNT_ID is an invalid ID
}