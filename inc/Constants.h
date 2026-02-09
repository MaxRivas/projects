///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Constants.h
 * @brief defines constants used in the bank application
 *
 */
///////////////////////////////////////////////////////////////////////////////

#include <string>

namespace bank {
    struct Constants
    {
        constexpr static double MIN_BALANCE = 1.0;
        constexpr static double INVALID_BALANCE = 0.0;
        constexpr static int INVALID_ID = -1;
        constexpr static char WHITESPACE_CHAR = ' ';
        constexpr static char DASH_CHAR = '-';
        constexpr static char APOSTROPHE_CHAR = '\'';
        constexpr static int INVALID_ZERO = 0;
        static constexpr std::string_view EMPTY_STRING = "";
    };
    
}