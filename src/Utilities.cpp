
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
        constexpr int ONE = 1;
        bank::ParserResult result;
        result.status = bank::ParseStatus::Ok;
        
        ArgumentMap map;

        for (int counter = ONE; counter < flags; ++counter)
        {
            int isInRange = counter + ONE < flags;
            std::string arg = arguments[counter];

            if (arg == bank::stringViewer(Constants::HELP_FLAG))
            {
                printHelp();
                result.status = ParseStatus::HelpRequested;
            }
            else if (arg == bank::stringViewer(Constants::EXIT_FLAG))
            {
                result.status = bank::ParseStatus::ExitRequested;
            }
            else if (arg == bank::stringViewer(Constants::NAME_FLAG) && isInRange)
            {
                map[Constants::NAME_KEY] = std::string(arguments[++counter]);
            }
            else if (arg == bank::stringViewer(Constants::AGE_FLAG) && isInRange)
            {
                map[Constants::AGE_KEY] = std::stoi(arguments[++counter]);
            }
            else if (arg == bank::stringViewer(Constants::RELATIONSHIP_FLAG) && isInRange)
            {
                int value = std::stoi(arguments[++counter]);
                auto relationship = bank::Utilities::toRelationshipTier(value);
                if (relationship == bank::RelationshipTier::INVALID)
                {
                    result.status = ParseStatus::InvalidArguments;
                    result.error = "Invalid relationship tier";
                }
                else
                {
                    map[Constants::RELATIONSHIP_KEY] = relationship;
                    result.status = ParseStatus::Ok;
                }
            }
            else if (arg == bank::stringViewer(Constants::ACCOUNT_FLAG) && isInRange)
            {
                int value = std::stoi(arguments[++counter]);
                auto account = bank::Utilities::toAccountType(value);
                if ( account == bank::AccountType::INVALID)
                {
                    result.status = ParseStatus::InvalidArguments;
                    result.error = "Invalid account type";
                }
                else
                {
                    map[Constants::ACCOUNT_KEY] = account;
                    result.status = ParseStatus::Ok;
                }
            }
            else
            {
                result.status = ParseStatus::InvalidArguments;
                result.error = "Unknown argument: " + arg;
                return result;
            }
        }

        result.arguments = map;
        return result;
    }

    void Utilities::printHelp()
    {
        std::cout << "Supported arguments:\n"
                  << " --help Show this help message\n"
                  << " --exit Exit immediately\n"
                  << " --name <string> Customer name\n"
                  << " --age <int> Customer age\n"
                  << " --relationship <int> Relationship tier:\n"
                  << " 1 = Platinum\n"
                  << " 2 = Gold\n"
                  << " 3 = Silver\n"
                  << " --account <int> Account type:\n"
                  << " 1 = Checking\n"
                  << " 2 = Savings\n";
    }
}