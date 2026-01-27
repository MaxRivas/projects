////////////////////////////////////////////////////////////////////////////
/**
 *  @file   AccountManagerTest.cpp
 *  @brief  Unit tests for AccountManager class
 */
////////////////////////////////////////////////////////////////////////////

#include <AccountManager.h>

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
        }

        void TearDown() override
        {
            // Code here will be called immediately after each test (right
            // before the destructor).
        }
        std::unique_ptr<bank::AccountManager> _accountManager;
};

TEST_F(Bank_AccountManager_Tests, Test_AccountManager_Creation)
{
    // _accountManager = std::make_unique<bank::AccountManager>();
    // EXPECT_NE(_accountManager, nullptr);
    ASSERT_TRUE(false); // Placeholder since AccountManager is abstract
}