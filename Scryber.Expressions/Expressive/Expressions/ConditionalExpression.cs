using System;
using System.Collections.Generic;
using Scryber.Expressive.Exceptions;

namespace Scryber.Expressive.Expressions
{
    /// <summary>
    /// The conditional (ternary) expression <c>condition ? whenTrue : whenFalse</c>.
    /// Only the branch that is chosen is evaluated.
    /// </summary>
    internal class ConditionalExpression : IExpression
    {
        private readonly IExpression condition;
        private readonly IExpression whenTrue;
        private readonly IExpression whenFalse;

        internal ConditionalExpression(IExpression condition, IExpression whenTrue, IExpression whenFalse)
        {
            this.condition = condition;
            this.whenTrue = whenTrue;
            this.whenFalse = whenFalse;
        }

        public object Evaluate(IDictionary<string, object> variables)
        {
            if (this.condition is null)
                throw new MissingParticipantException("The condition before the '?' is missing.");
            if (this.whenTrue is null)
                throw new MissingParticipantException("The value between the '?' and ':' is missing.");
            if (this.whenFalse is null)
                throw new MissingParticipantException("The value after the ':' is missing.");

            return IsTrue(this.condition.Evaluate(variables))
                ? this.whenTrue.Evaluate(variables)
                : this.whenFalse.Evaluate(variables);
        }

        /// <summary>
        /// The truth of a condition value: null, false, zero and an empty string are false.
        /// A string is true unless it is empty or reads as 'false'. Any other value is converted as a boolean if it can be,
        /// otherwise it is true.
        /// </summary>
        internal static bool IsTrue(object value)
        {
            switch (value)
            {
                case null:
                    return false;
                case bool b:
                    return b;
                case string s:
                    if (s.Length == 0)
                        return false;
                    if (bool.TryParse(s, out var parsed))
                        return parsed;
                    return true;
                case IConvertible convertible:
                    try
                    {
                        return convertible.ToBoolean(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (InvalidCastException)
                    {
                        return true;
                    }
                    catch (FormatException)
                    {
                        return true;
                    }
                default:
                    return true;
            }
        }
    }
}
