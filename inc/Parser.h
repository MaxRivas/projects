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

namespace bank {

    struct Options {
        std::string name;
        int age = 0;
        RelationshipTier tier = RelationshipTier::INVALID;
        AccountType type = AccountType::INVALID;
    };

    enum class ParseStatus
    {
        Ok,
        HelpRequested,
        ExitRequested,
        InvalidArguments
    };

    struct ParserResult
    {
        ParseStatus status;
        Options options;   // only valid when status == Ok
        std::string error; // only used when status == InvalidArguments
    };
}

#endif // PARSER_H