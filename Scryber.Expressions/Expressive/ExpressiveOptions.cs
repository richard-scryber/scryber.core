using System;

namespace Scryber.Expressive
{
    /// <summary>
    /// Options to alter the way in which an <see cref="Expression"/> is parsed and evaluated.
    /// </summary>
    [Flags]
    public enum ExpressiveOptions
    {
        /// <summary>
        /// Specifies that no options are set.
        /// </summary>
        None = 1,

        /// <summary>
        /// Specifies case-insensitive matching.
        /// </summary>
        [Obsolete("This will be removed in future versions with IgnoreCaseForParsing, IgnoreCaseForEquality and IgnoreCaseAll replacing it.")]
        IgnoreCase = 2,

        /// <summary>
        /// No-cache mode. Ignores any pre-compiled expression in the cache.
        /// </summary>
        NoCache = 4,

        /// <summary>
        /// When using Round(), if a number is halfway between two others, it is rounded toward the nearest number that is away from zero.
        /// </summary>
        RoundAwayFromZero = 8,

        /// <summary>
        /// Specifies case-insensitive matching for parsing expressions (e.g. function/operator/variable names do not need to match case).
        /// </summary>
        IgnoreCaseForParsing = 16,

        /// <summary>
        /// Specifies case-insensitive matching for performing equality comparisons (e.g. THIS == this).
        /// </summary>
        IgnoreCaseForEquality = 32,

        /// <summary>
        /// Specifies case-insensitive matching for both parsing and performing equality comparisons.
        /// </summary>
        IgnoreCaseAll = IgnoreCaseForParsing | IgnoreCaseForEquality,

        /// <summary>
        /// All options are used.
        /// </summary>
#pragma warning disable 618 // As it is our own warning this is safe enough until we actually get rid
        All = IgnoreCase | NoCache | RoundAwayFromZero | IgnoreCaseAll
#pragma warning restore 618
    }
}
