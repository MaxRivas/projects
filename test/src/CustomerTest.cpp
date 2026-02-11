////////////////////////////////////////////////////////////////////////////
/**
 *  @file   CustomerTest.cpp
 *  @brief  Unit tests for Customer class
 */
////////////////////////////////////////////////////////////////////////////

#include <Account.h>
#include <Customer.h>
#include <../inc/interfaces/ICustomer.h>

#include <gtest/gtest.h>
#include <memory>

using namespace testing;

class Bank_Customer_Tests : public Test
{
protected:

    std::unique_ptr<bank::ICustomer> _customer;

    std::unique_ptr<bank::IAccount> createSavingsAccount(double initialBalance)
    {
        return std::make_unique<bank::Account>(bank::AccountType::SAVINGS, initialBalance);
    }

    std::unique_ptr<bank::IAccount> createCheckingAccount(double initialBalance)
    {
        return std::make_unique<bank::Account>(bank::AccountType::CHECKING, initialBalance);
    }
};

TEST_F (Bank_Customer_Tests, Test_Customer_Creation)
{
    // std::string name = "John Doe";
    // int age = 30;

    // _customer = std::make_unique<bank::Customer>(bank::RelationshipTier::SILVER, name, age);
    // // Test passes if creation doesn't crash
    // EXPECT_TRUE(_customer != nullptr);
}