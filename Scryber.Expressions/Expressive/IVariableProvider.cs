using Scryber.Drawing;

namespace Scryber.Expressive
{
    /// <summary>
    /// Interface definition for providing variable values.
    /// </summary>
    public interface IVariableProvider
    {
        /// <summary>
        /// Attempts to safely get the <paramref name="value"/> for the supplied <paramref name="variableName"/>.
        /// </summary>
        /// <param name="variableName">The name of the variable.</param>
        /// <param name="value">The value of the variable or <b>null</b> if it does not exist.</param>
        /// <returns>true if the variable exists, false otherwise.</returns>
        bool TryGetValue(string variableName, out object value);


        void AddRelativeDimensions(Size page, Size container, Size font, Unit rootFont, bool useWidth);
    }
}
