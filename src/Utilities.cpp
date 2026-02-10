
///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Utilities.cpp
 * @brief specifies utility functions.
 *
 */
///////////////////////////////////////////////////////////////////////////////

#include <atomic>
#include <cstdint>

#include "Constants.h"
#include "Utilities.h"

namespace bank {

    int Utilities::generateUniqueId()
    {
        static std::atomic_uint32_t counter{1};
        return static_cast<int>(counter.fetch_add(1, std::memory_order_relaxed));
    }

    bool Utilities::isValidName(const std::string &name)
    {
        if (name.empty())
            return false;

        bool hasAlpha = false;
        for (size_t index = 0; index < name.length(); ++index)
        {
            char character = name[index];

            if (std::isalpha(character))
            {
                hasAlpha = true;
                continue;
            }

            if (character == Constants::WHITESPACE_CHAR ||
                 character == Constants::DASH_CHAR ||
                  character == Constants::APOSTROPHE_CHAR)
            {
                // Allow space, hyphen, apostrophe (e.g., "Mary-Jane O'Neil")
                continue;
            }

            return false; // Invalid character
        }

        return hasAlpha;
    }

    bank::RelationshipTier Utilities::toRelationshipTier(int& value)
    {
        bank::RelationshipTier result = bank::RelationshipTier::INVALID;
        switch (value)
        {
            case 1:
            {
                result = bank::RelationshipTier::PLATINUM;
                break;
            }
            case 2:
            {
                result = bank::RelationshipTier::GOLD;
                break;
            }
            case 3:
            {
                result = bank::RelationshipTier::SILVER;
                break;
            }
            default:
            {
                break;
            }
        }

        return result;
    }

    bank::AccountType Utilities::toAccountType(int &value)
    {
        bank::AccountType result = bank::AccountType::INVALID;
        switch (value)
        {
            case 1:
            {
                result = bank::AccountType::CHECKING;
                break;
            }
            case 2:
            {
                return bank::AccountType::SAVINGS;
                break;
            }
            default:
            {
                break;
            }
        }

        return result;
    }
    
    bank::ParserResult Utilities::parseUserInput(int flags, char* arguments[])
    {
        bank::ParserResult result;
        result.status = bank::ParseStatus::Ok;

        constexpr int ONE = 1;
        bank::Options options;

        for (int counter = ONE; counter < argc; ++counter)
        {
            std::string arg = argv[counter];

            if (arg == "--help" || arg == "-h")
            {
                printHelp();
                result.status = ParseStatus::HelpRequested;
                return result;
            }
            if (arg == "--exit")
            {
                result.status = bank::ParseStatus::ExitRequested;
                return result;
            }

            if (arg == "--name" && counter + ONE < argc)
            {
                options.name = argv[++counter];
            }
            else if (arg == "--age" && counter + ONE < argc)
            {
                options.age = std::stoi(argv[++counter]);
            }
            else if (arg == "--relationship" && counter + 1 < argc)
            {
                int value = std::stoi(argv[++counter]);
                options.tier = bank::Utilities::toRelationshipTier(value);
                if (options.tier == bank::RelationshipTier::INVALID)
                {
                    result.status = ParseStatus::InvalidArguments;
                    result.error = "Invalid relationship tier";
                    return result;
                }
            }
            else if (arg == "--account" && counter + 1 < argc)
            {
                int value = std::stoi(argv[++counter]);
                options.type = bank::Utilities::toAccountType(value);
                if (options.type == bank::AccountType::INVALID)
                {
                    result.status = ParseStatus::InvalidArguments;
                    result.error = "Invalid account type";
                    return result;
                }
            }
            else
            {
                result.status = ParseStatus::InvalidArguments;
                result.error = "Unknown argument: " + arg;
                return result;
            }
        }

        result.options = options;
        return result;
    }

    void Utilities::printHelp()
    {
        std::cout << "Supported arguments:\n"
                  << " --help, -h Show this help message\n"
                  << " --exit Exit immediately\n"
                  << " --name <string> Customer name\n"
                  << " --age <int> Customer age\n"
                  << " --relationship <int> Relationship tier:\n"
                  << " 1 = Bronze\n"
                  << " 2 = Silver\n"
                  << " 3 = Gold\n"
                  << " 4 = Platinum\n"
                  << " --account <int> Account type:\n"
                  << " 1 = Checking\n"
                  << " 2 = Savings\n"
                  << " 3 = Credit\n";
    }
}