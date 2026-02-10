///////////////////////////////////////////////////////////////////////////////
/**
 * @file  Constants.h
 * @brief defines constants used in the bank application
 *
 */
///////////////////////////////////////////////////////////////////////////////

#include <array>
#include <string_view>
#include <string>

namespace bank {

    // ------------------------------------------------------------ 
    // Compile‑time concatenation for char arrays 
    // concat() must be defined BEFORE the struct
    // ------------------------------------------------------------
    template <std::size_t N1, std::size_t N2>
    constexpr auto concat(const char (&a)[N1], const char (&b)[N2])
    {
        std::array<char, N1 + N2 - 1> result{}; // -1 because both include '\0'

        std::size_t index = 0;

        for (; index < N1 - 1; ++index)
        {
            result[index] = a[index];
        }

        for (std::size_t j = 0; j < N2; ++j)
        {
            result[index + j] = b[j];
        }
        
        return result;
    }


    template <typename Array>
    static constexpr std::string_view stringViewer(const Array& arr)
    {
        return std::string_view(arr.data(), arr.size());
    }

    struct Constants
    {

        // ------------------------------------------------------------ 
        // Base pieces (must be char[] to work with concat)
        // ------------------------------------------------------------
        
        constexpr static char PREFIX[] = "--";
        constexpr static char HELP_KEY[] = "help";
        constexpr static char EXIT_KEY[] = "exit";
        constexpr static char NAME_KEY[] = "name";
        constexpr static char AGE_KEY[] = "age";
        constexpr static char RELATIONSHIP_KEY[] = "relationship";
        constexpr static char ACCOUNT_KEY[] = "account";
        
        // ------------------------------------------------------------ 
        // Compile‑time concatenated flags
        // ------------------------------------------------------------
        constexpr static auto EXIT_FLAG = concat(PREFIX, EXIT_KEY);
        constexpr static auto HELP_FLAG = concat(PREFIX, HELP_KEY);
        constexpr static auto NAME_FLAG = concat(PREFIX, NAME_KEY);
        constexpr static auto AGE_FLAG = concat(PREFIX, AGE_KEY);
        constexpr static auto RELATIONSHIP_FLAG = concat(PREFIX, RELATIONSHIP_KEY);
        constexpr static auto ACCOUNT_FLAG = concat(PREFIX, ACCOUNT_KEY);

        // ------------------------------------------------------------ 
        // Other constants
        // ------------------------------------------------------------
        constexpr static double MIN_BALANCE = 1.0;
        constexpr static double INVALID_BALANCE = 0.0;
        constexpr static int INVALID_ID = -1;
        constexpr static char WHITESPACE_CHAR = ' ';
        constexpr static char DASH_CHAR = '-';
        constexpr static char APOSTROPHE_CHAR = '\'';
        constexpr static int INVALID_ZERO = 0;
        constexpr static std::string_view EMPTY_STRING = "";
    };
    
}