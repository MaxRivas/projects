////////////////////////////////////////////////////////////////////////////
/**
 *  @file   CustomerTest.cpp
 *  @brief  Unit tests for Customer class
 */
////////////////////////////////////////////////////////////////////////////

#include <Customer.h>
#include <../inc/interfaces/ICustomer.h>

#include <gtest/gtest.h>
#include <memory>

using namespace testing;

class Bank_Customer_Tests : public Test
{
protected:
    std::unique_ptr<bank::ICustomer> _customer; 
};