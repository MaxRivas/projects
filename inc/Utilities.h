#ifndef IDGENERATOR_H
#define IDGENERATOR_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Utilities.h
 * @brief specifies utility functions.
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <string>

#include <AccountType.h>
#include <Parser.h>
#include <RelationshipTier.h>

namespace bank {

    class Utilities {
        public:
            static int generateUniqueId();
            static bool isValidName(const std::string &name);
            static bank::RelationshipTier toRelationshipTier(int& value);
            static bank::AccountType toAccountType(int& value);
            static ParserResult parseUserInput(int flags, char* arguments[]);
            static void printHelp();
    };
}

#endif  // UTILITIES_H