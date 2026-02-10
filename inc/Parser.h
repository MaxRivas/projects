#ifndef PARSER_H
#define PARSER_H

////////////////////////////////////////////////////////////////////////////////
/**
 * @file  Parser.h
 * @brief Command line argument into valid arguments
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <AccountType.h>
#include <RelationshipTier.h>

#include <string.h>
#include <iostream>
#include <variant>
#include <unordered_map>

namespace bank {

    enum class ParseStatus
    {
        Ok,
        HelpRequested,
        ExitRequested,
        InvalidArguments
    };

    using Argument = std::variant<std::string, int, bank::RelationshipTier, bank::AccountType>;
    using ArgumentMap = std::unordered_map<std::string, Argument>;

    struct ParserResult
    {
        ParseStatus status;
        std::string error; // only used when status == InvalidArguments
        ArgumentMap arguments;

    };
}

#endif // PARSER_H