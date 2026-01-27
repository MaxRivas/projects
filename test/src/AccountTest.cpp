////////////////////////////////////////////////////////////////////////////
/**
 *  @file   AccountTest.cpp
 *  @brief  Unit tests for Account class
 */
////////////////////////////////////////////////////////////////////////////

#include <Account.h>
#include <../inc/interfaces/IAccount.h>

#include <gtest/gtest.h>
#include <gmock/gmock.h>
#include <memory>

using namespace testing;
using bank::Account;

class Bank_Account_Tests : public Test
{
protected:
    void SetUp() override
    {
        // Code here will be called immediately after the constructor (right
        // before each test).
    }

    void TearDown() override
    {
        // Code here will be called immediately after each test (right
        // before the destructor).
    }

    std::unique_ptr<bank::IAccount> _account;
};

TEST_F(Bank_Account_Tests, Test_Account_Creation_Checking)
{
    double initial_balance = 1000.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::CHECKING, initial_balance);
    EXPECT_EQ(_account->getAccountType(), bank::AccountType::CHECKING);
    EXPECT_EQ(_account->getBalance(), initial_balance);
}

TEST_F(Bank_Account_Tests, Test_Account_Creation_Savings)
{
    double initial_balance = 500.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::SAVINGS, initial_balance);
    EXPECT_EQ(_account->getAccountType(), bank::AccountType::SAVINGS);
    EXPECT_EQ(_account->getBalance(), initial_balance);
}

TEST_F(Bank_Account_Tests, Test_Account_Creation_Negative_Balance)
{
    double initial_balance = -100.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::CHECKING, initial_balance);

    // Assuming that the account should not be created with negative balance
    EXPECT_EQ(_account->getBalance(), 0.0);
}

TEST_F(Bank_Account_Tests, Test_Account_Creation_Zero_Balance)
{
    double initial_balance = 0.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::SAVINGS, initial_balance);

    // Assuming that the account should not be created with zero balance
    EXPECT_EQ(_account->getBalance(), 0.0);
}

TEST_F(Bank_Account_Tests, Test_Deposit_Positive_Amount)
{
    double initial_balance = 200.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::CHECKING, initial_balance);

    double deposit_amount = 150.0;
    bool result = _account->deposit(deposit_amount);

    EXPECT_TRUE(result);
    EXPECT_EQ(_account->getBalance(), initial_balance + deposit_amount);
}

TEST_F(Bank_Account_Tests, Test_Deposit_Negative_Amount)
{
    double initial_balance = 300.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::SAVINGS, initial_balance);

    double deposit_amount = -50.0;
    bool result = _account->deposit(deposit_amount);

    EXPECT_FALSE(result);
    EXPECT_EQ(_account->getBalance(), initial_balance);
}

TEST_F(Bank_Account_Tests, Test_Deposit_Zero_Amount)
{
    double initial_balance = 400.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::CHECKING, initial_balance);

    double deposit_amount = 0.0;
    bool result = _account->deposit(deposit_amount);

    EXPECT_FALSE(result);
    EXPECT_EQ(_account->getBalance(), initial_balance);
}

TEST_F(Bank_Account_Tests, Test_Withdraw_Positive_Amount)
{
    double initial_balance = 500.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::SAVINGS, initial_balance);

    double withdraw_amount = 200.0;
    bool result = _account->withdraw(withdraw_amount);

    EXPECT_TRUE(result);
    EXPECT_EQ(_account->getBalance(), initial_balance - withdraw_amount);
}

TEST_F(Bank_Account_Tests, Test_Withdraw_Amount_Exceeding_Balance)
{
    double initial_balance = 600.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::CHECKING, initial_balance);

    double withdraw_amount = 700.0;
    bool result = _account->withdraw(withdraw_amount);

    EXPECT_FALSE(result);
    EXPECT_EQ(_account->getBalance(), initial_balance);
}

TEST_F(Bank_Account_Tests, Test_Withdraw_Negative_Amount)
{
    double initial_balance = 800.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::SAVINGS, initial_balance);

    double withdraw_amount = -100.0;
    bool result = _account->withdraw(withdraw_amount);

    EXPECT_FALSE(result);
    EXPECT_EQ(_account->getBalance(), initial_balance);
}

TEST_F(Bank_Account_Tests, Test_Withdraw_Zero_Amount)
{
    double initial_balance = 900.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::CHECKING, initial_balance);

    double withdraw_amount = 0.0;
    bool result = _account->withdraw(withdraw_amount);

    EXPECT_FALSE(result);
    EXPECT_EQ(_account->getBalance(), initial_balance);
}

TEST_F(Bank_Account_Tests, Test_Withdraw_Exact_Balance)
{
    double initial_balance = 1000.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::SAVINGS, initial_balance);

    double withdraw_amount = initial_balance;
    bool result = _account->withdraw(withdraw_amount);

    EXPECT_TRUE(result);
    EXPECT_EQ(_account->getBalance(), 0.0);
}

TEST_F(Bank_Account_Tests, Test_Multiple_Deposits_And_Withdrawals)
{
    double initial_balance = 1000.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::CHECKING, initial_balance);

    EXPECT_TRUE(_account->deposit(500.0));
    EXPECT_EQ(_account->getBalance(), 1500.0);

    EXPECT_TRUE(_account->withdraw(200.0));
    EXPECT_EQ(_account->getBalance(), 1300.0);

    EXPECT_FALSE(_account->withdraw(2000.0)); // Exceeds balance
    EXPECT_EQ(_account->getBalance(), 1300.0);

    EXPECT_TRUE(_account->deposit(300.0));
    EXPECT_EQ(_account->getBalance(), 1600.0);
}

TEST_F(Bank_Account_Tests, Test_GetId_Unique_Per_Account)
{
    double initial_balance1 = 100.0;
    double initial_balance2 = 200.0;

    auto account1 = std::make_unique<bank::Account>(bank::AccountType::CHECKING, initial_balance1);
    auto account2 = std::make_unique<bank::Account>(bank::AccountType::SAVINGS, initial_balance2);

    int id1 = account1->getId();
    int id2 = account2->getId();

    EXPECT_NE(id1, id2);
}

TEST_F(Bank_Account_Tests, Test_IsValid_Method)
{
    double initial_balance = 100.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::CHECKING, initial_balance);

    EXPECT_TRUE(_account->isValid());

    // Create an account with invalid balance
    double invalid_balance = -50.0;
    auto invalid_account = std::make_unique<bank::Account>(bank::AccountType::SAVINGS, invalid_balance);

    EXPECT_FALSE(invalid_account->isValid());
}

TEST_F(Bank_Account_Tests, Test_CloseAccount_Method)
{
    double initial_balance = 100.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::CHECKING, initial_balance);

    int accountId = _account->getId();
    EXPECT_TRUE(_account->closeAccount(accountId));
    EXPECT_FALSE(_account->isValid());
}

TEST_F(Bank_Account_Tests, Test_CloseAccount_InvalidId)
{
    double initial_balance = 100.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::SAVINGS, initial_balance);

    int invalidAccountId = _account->getId() + 1; // Assuming this ID does not exist
    EXPECT_FALSE(_account->closeAccount(invalidAccountId));
    EXPECT_TRUE(_account->isValid());
}

TEST_F(Bank_Account_Tests, Test_CloseAccount_OnInvalidAccount)
{
    double invalid_balance = -50.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::CHECKING, invalid_balance);

    int accountId = _account->getId();
    EXPECT_FALSE(_account->closeAccount(accountId));
    EXPECT_FALSE(_account->isValid());
}

TEST_F(Bank_Account_Tests, Test_CloseAccount_MultipleTimes)
{
    double initial_balance = 200.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::SAVINGS, initial_balance);

    int accountId = _account->getId();
    EXPECT_TRUE(_account->closeAccount(accountId));
    EXPECT_FALSE(_account->isValid());

    // Attempt to close the account again
    EXPECT_FALSE(_account->closeAccount(accountId));
}

TEST_F(Bank_Account_Tests, Bank_Account_Tests_CloseAccount_IdDoesnotExist)
{
    double initial_balance = 300.0;
    _account = std::make_unique<bank::Account>(bank::AccountType::CHECKING, initial_balance);

    int nonExistentAccountId = 9999; // Assuming this ID does not exist
    EXPECT_FALSE(_account->closeAccount(nonExistentAccountId));
    EXPECT_TRUE(_account->isValid());
}