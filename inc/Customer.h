#ifndef CUSTOMER_H
#define CUSTOMER_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Customer.h
 * @brief specifies a bank customer class.
 *
 */
///////////////////////////////////////////////////////////////////////////

#include <interfaces/ICustomer.h>
#include <interfaces/IAccountManager.h>
#include <interfaces/IPerson.h>

namespace bank
{
    class Customer : public bank::ICustomer
    {
        private:
            bank::RelationshipTier _tier;
            std::unique_ptr<bank::IAccountManager> _accountManager;

        public:
            Customer(const bank::RelationshipTier& tier);
            const bank::IAccountManager *getAccountManager() const override;
            const bank::RelationshipTier getTier() const override;
    };
}

#endif // CUSTOMER_H