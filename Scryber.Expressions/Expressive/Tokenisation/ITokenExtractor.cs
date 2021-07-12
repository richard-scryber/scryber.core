namespace Scryber.Expressive.Tokenisation
{
    /// <summary>
    /// Interface definition for extracting <see cref="Token"/>s from an expression.
    /// </summary>
    public interface ITokenExtractor
    {
        /// <summary>
        /// Extracts a <see cref="Token"/> from the supplied <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The expression to extract from.</param>
        /// <param name="currentIndex">Where to look within the <paramref name="expression"/>.</param>
        /// <param name="context">Any additional rules to apply when extracting.</param>
        /// <returns>The <see cref="Token"/> that is extract, null if extraction fails.</returns>
        Token ExtractToken(string expression, int currentIndex, Context context);
    }
}