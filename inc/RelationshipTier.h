#ifndef RELATIONSHIP_TIER_H
#define RELATIONSHIP_TIER_H

///////////////////////ß////////////////////////////////////////////////////////
/**
 * @file  RelationshipTier.h
 * @brief specifies different types of customer account tiers
 * ß
 */
///////////////////////////////////////////////////////////////////////////////

namespace bank
{
    enum class RelationshipTier
    {
        PLATINUM = 1,
        GOLD = 2,
        SILVER = 3,
        INVALID = -1
    };
}

#endif // RELATIONSHIP_TIER_H