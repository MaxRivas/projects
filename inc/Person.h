#ifndef PERSON_H
#define PERSON_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Person.h
 * @brief specifies a person.
 *
 *////////////////////////////////////////////////////////////////////////////

 #include <interfaces/IPerson.h>

namespace bank
{
    class Person : public bank::IPerson
    {
        private:
            int id;
            int _age;
            std::string _name;

        public:
            Person(const std::string &name, int age);
            std::string getName() const override;
            int getAge() const override;
            int getId() const override;
    };
}

#endif // PERSON_H