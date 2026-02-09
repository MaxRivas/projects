#ifndef IPERSON_H
#define IPERSON_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  IPerson.h
 * @brief specifies a person interface.
 *
 */
///////////////////////////////////////////////////////////////////////////////

#include <AccountType.h>
#include <interfaces/IAccount.h>

namespace bank
{
    class IPerson
    {
        public:
            virtual std::string getName() const = 0;
            virtual int getAge() const = 0;
            virtual int getId() const = 0;
    };
}

#endif // IPERSON_H