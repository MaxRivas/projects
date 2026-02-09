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
 #include <RelationshipTier.h>

namespace bank
{
    class ICustomer : public bank::IPerson
    {
        private:
            bank::RelationshipTier _tier;
            std::unique_ptr<bank::IAccountManager> _manager;
            
        public:
            virtual const bank::IAccountManager* getAccountManager() const = 0;
            virtual const bank::RelationshipTier getTier() const = 0;
    };
}

#endif // ICUSTOMER_H