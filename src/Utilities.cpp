
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
}