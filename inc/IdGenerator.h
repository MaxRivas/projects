#ifndef IDGENERATOR_H
#define IDGENERATOR_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  IdGenerator.h
 * @brief specifies an ID generator.
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <random>
#include <string>

namespace bank {
    static int generateRandomId(int min = 100000, int max = 999999) {
        static std::random_device rd;
        static std::mt19937 gen(rd());
        std::uniform_int_distribution<> dist(min, max);
        return dist(gen);
    }   
} // namespace bank 

#endif  // IDGENERATOR_H