#ifndef ACCOUNT_TYPE_H
#define ACCOUNT_TYPE_H

///////////////////////ß////////////////////////////////////////////////////////
/**
 * @file  accountType.h
 * @brief specifies different types of bank accounts.
 * ß
 */
///////////////////////////////////////////////////////////////////////////////

namespace bank {
    enum class AccountType {
        CHECKING = 1,
        SAVINGS = 2,
        INVALID = -1
    };
}

#endif  // ACCOUNT_TYPE_H