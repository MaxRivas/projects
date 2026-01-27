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
#include <memory>
#include <string>
#include <vector>

namespace bank
{
    class IPerson
    {
    public:
        virtual std::string getName() const = 0;
        virtual int getAge() const = 0;
        virtual int getId() const = 0;
        virtual bool addAccount(std::unique_ptr<IAccount> account) = 0;
        virtual bool removeAccount(int accountId) = 0;
        virtual const std::vector<std::shared_ptr<const IAccount>> getAccounts() const = 0;
    };
}

#endif // IPERSON_H