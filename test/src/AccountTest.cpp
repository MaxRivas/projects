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
    _account = new bank::Account(bank::AccountType::CHECKING);
    EXPECT_EQ(_account->getAccountType(), bank::AccountType::CHECKING);
}

TEST_F(Bank_Account_Tests, Test_Account_Creation_Savings)
{
    _account = new bank::Account(bank::AccountType::SAVINGS);
    EXPECT_EQ(_account->getAccountType(), bank::AccountType::SAVINGS);
}