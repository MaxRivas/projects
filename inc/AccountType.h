#ifndef ACCOUNT_TYPE_H
#define ACCOUNT_TYPE_H

///////////////////////ß////////////////////////////////////////////////////////
/**
 * @file  accountType.h
 * @brief specifies different types of bank accounts.
 * ß
 */
///////////////////////////////////////////////////////////////////////////////


#include <iostream>
// Add your function declarations here.

namespace bank {
    enum AccountType {
        CHECKING,
        SAVINGS,
        INVALID
    };
}

#endif  // ACCOUNT_TYPE_H