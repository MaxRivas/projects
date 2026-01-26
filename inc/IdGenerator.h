#ifndef IDGENERATOR_H
#define IDGENERATOR_H

///////////////////////////////////////////////////////////////////////////////
/**
 * @file  IdGenerator.h
 * @brief specifies an ID generator.
 * 
 */
///////////////////////////////////////////////////////////////////////////////

#include <chrono>
 #include <random>

namespace bank {
    static int generateUniqueId()
    {
        static std::mt19937 gen(std::random_device{}());
        std::uniform_int_distribution<> dist(100, 999);
        auto now = std::chrono::high_resolution_clock::now().time_since_epoch().count();
        return static_cast<int>((now % 1000000000) + dist(gen));
    }
}

#endif  // IDGENERATOR_H