////////////////////////////////////////////////////////////////////////////
/**
 *  @file   PersonTest.cpp
 *  @brief  Unit tests for Person class
 */
////////////////////////////////////////////////////////////////////////////

#include <Person.h>
#include <../inc/interfaces/IPerson.h>

#include <gtest/gtest.h>
#include <gmock/gmock.h>
#include <memory>

using namespace testing;
using bank::Person;

class Bank_Person_Tests : public Test
{
    protected:
        std::unique_ptr<bank::IPerson> _person;
};

TEST_F(Bank_Person_Tests, Test_Person_Creation)
{
    std::string name = "John Doe";
    int age = 30;
    _person = std::make_unique<bank::Person>(name, age);
    EXPECT_EQ(_person->getName(), name);
    EXPECT_EQ(_person->getAge(), age);
}

TEST_F(Bank_Person_Tests, Test_Person_InvalidName_Creation)
{
    std::string name = "";
    int age = -5;
    _person = std::make_unique<bank::Person>(name, age);
    EXPECT_EQ(_person->getName(), "");
    EXPECT_EQ(_person->getAge(), 0);
}

TEST_F(Bank_Person_Tests, Test_Person_InvalidAge_Creation)
{
    std::string name = "Jane Doe";
    int age = -10;
    _person = std::make_unique<bank::Person>(name, age);
    EXPECT_EQ(_person->getName(), "");
    EXPECT_EQ(_person->getAge(), 0);
}

TEST_F(Bank_Person_Tests, Test_Person_UniqueId)
{
    std::string name1 = "Person One";
    int age1 = 40;
    auto person1 = std::make_unique<bank::Person>(name1, age1);

    std::string name2 = "Person Two";
    int age2 = 35;
    auto person2 = std::make_unique<bank::Person>(name2, age2);

    EXPECT_NE(person1->getId(), person2->getId()); // Each person should have a unique ID
}