#ifndef CUSTOMER_H
#define CUSTOMER_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Customer.h
 * @brief specifies a bank customer class.
 *
 */
///////////////////////////////////////////////////////////////////////////

#include <interfaces/IAccountManager.h>
#include <interfaces/IPerson.h>

namespace bank
{
    class Customer : public bank::ICustomer
    {
        private:
            std::unique_ptr<bank::IPerson> _person;
            std::unique_ptr<bank::IAccountManager> _accountManager;

        public:
            const bank::IPerson *getPerson() const override;
            const bank::IAccountManager *getAccountManager() const override;
    };
}

#endif // CUSTOMER_H