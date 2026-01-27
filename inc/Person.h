#ifndef PERSON_H
#define PERSON_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Person.h
 * @brief specifies a person.
 *
 *////////////////////////////////////////////////////////////////////////////
#include <interfaces/IPerson.h>

#include <vector>

namespace bank
{
    class Person : public bank::IPerson
    {
        private:
            int id;
            int _age;
            std::string _name;
            std::vector<std::shared_ptr<IAccount>> _accounts;

        public:
            Person(const std::string &name, int age);
            std::string getName() const override;
            int getAge() const override;
            int getId() const override;
            bool addAccount(std::unique_ptr<IAccount> account) override;
            bool removeAccount(int accountId) override;
            const std::vector<std::shared_ptr<const IAccount>> getAccounts() const override;
    };
}

#endif // PERSON_H