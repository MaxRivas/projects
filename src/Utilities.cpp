
///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Utilities.cpp
 * @brief specifies utility functions.
 *
 */
///////////////////////////////////////////////////////////////////////////////

#include <chrono>
#include <random>

#include "Utilities.h"
#include "Constants.h"

namespace bank {

    int Utilities::generateUniqueId()
    {
        static std::mt19937 gen(std::random_device{}());
        std::uniform_int_distribution<> dist(Constants::MIN_DISTRIBUTION, Constants::MAX_DISTRIBUTION);
        auto now = std::chrono::high_resolution_clock::now().time_since_epoch().count();
        return static_cast<int>((now % Constants::MODULE_GENERATOR) + dist(gen));
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
}