#ifndef IDGENERATOR_H
#define IDGENERATOR_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Utilities.h
 * @brief specifies utility functions.
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <chrono>
#include <random>
#include <string>

namespace bank {

    class Utilities {
        public:
            static int generateUniqueId();
            static bool isValidName(const std::string &name);
    };
}

#endif  // UTILITIES_H