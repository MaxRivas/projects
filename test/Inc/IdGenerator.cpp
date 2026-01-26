////////////////////////////////////////////////////////////////////////////
/**
 *  @file   IdGeneratorTest.cpp
 *  @brief  Unit tests for IdGenerator class
 */
////////////////////////////////////////////////////////////////////////////

#include <IdGenerator.h>

#include <gtest/gtest.h>
#include <gmock/gmock.h>

using namespace bank;
using namespace testing;

TEST(Bank_IdGenerator_Tests, Test_GenerateUniqueId_Multiple_Calls)
{
    int id1 = bank::generateUniqueId();
    int id2 = bank::generateUniqueId();
    int id3 = bank::generateUniqueId();

    EXPECT_NE(id1, id2);
    EXPECT_NE(id1, id3);
    EXPECT_NE(id2, id3);
}

TEST(Bank_IdGenerator_Tests, Test_GenerateUniqueId_Range)
{
    for (int i = 0; i < 100; ++i) {
        int id = bank::generateUniqueId();
        EXPECT_GE(id, 100000000); // Minimum expected value
        EXPECT_LE(id, 1000999998); // Maximum expected value
    }
}

TEST(Bank_IdGenerator_Tests, Test_GenerateUniqueId_Performance)
{
    const int numIds = 10000;
    std::set<int> ids;

    for (int i = 0; i < numIds; ++i) {
        int id = bank::generateUniqueId();
        ids.insert(id);
    }

    EXPECT_EQ(ids.size(), numIds); // Ensure all generated IDs are unique
}