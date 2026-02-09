#ifndef ICUSTOMER_H
#define ICUSTOMER_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  ICustomer.h
 * @brief specifies a bank customer interface.
 *
 *////////////////////////////////////////////////////////////////////////////

 #include <interfaces/IAccountManager.h>
 #include <interfaces/IPerson.h>

namespace bank
{
    class ICustomer
    {
        public:
            virtual const bank::IPerson* getPerson() const = 0;
            virtual const bank::IAccountManager* getAccountManager() const = 0;
    };
}

#endif // ICUSTOMER_H