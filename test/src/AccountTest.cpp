////////////////////////////////////////////////////////////////////////////
/**
 *  @file   AccountTest.cpp
 *  @brief  Unit tests for Account class
 */
////////////////////////////////////////////////////////////////////////////

#include <../inc/Account.h>
#include <gtest/gtest.h>
#include <gmock/gmock.h>

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
        delete _account;
    }

    bank::Account *_account;
};

TEST_F(Bank_Account_Tests, Test_Account_Creation_Checking)
{
    double initial_balance = 1000.0;
    _account = new bank::Account(bank::AccountType::CHECKING, initial_balance);
    EXPECT_EQ(_account->getAccountType(), bank::AccountType::CHECKING);
    EXPECT_EQ(_account->getBalance(), initial_balance);
}

TEST_F(Bank_Account_Tests, Test_Account_Creation_Savings)
{
    double initial_balance = 500.0;
    _account = new bank::Account(bank::AccountType::SAVINGS, initial_balance);
    EXPECT_EQ(_account->getAccountType(), bank::AccountType::SAVINGS);
    EXPECT_EQ(_account->getBalance(), initial_balance);
}

TEST_F(Bank_Account_Tests, Test_Account_Creation_Negative_Balance)
{
    double initial_balance = -100.0;
    _account = new bank::Account(bank::AccountType::CHECKING, initial_balance);

    // Assuming that the account should not be created with negative balance
    EXPECT_EQ(_account->getBalance(), 0.0);
}

TEST_F(Bank_Account_Tests, Test_Account_Creation_Zero_Balance)
{
    double initial_balance = 0.0;
    _account = new bank::Account(bank::AccountType::SAVINGS, initial_balance);

    // Assuming that the account should not be created with zero balance
    EXPECT_EQ(_account->getBalance(), 0.0);
}