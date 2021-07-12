namespace Scryber.Expressive.Operators
{
    // TODO: This is starting to feel wrong. Perhaps just a set of integers could be returned from IOperator?

    /// <summary>
    /// Enumeration of the different possible precedences of operators (basically the order in which operator gets chosen).
    /// </summary>
    public enum OperatorPrecedence
    {
        /// <summary>
        /// The minimum precedence.
        /// </summary>
        Minimum = 0,

        /// <summary>
        /// The Or operator precedence.
        /// </summary>
        Or = 1,

        /// <summary>
        /// The And operator precedence.
        /// </summary>
        And = 2,

        /// <summary>
        /// The Equal operator precedence.
        /// </summary>
        Equal = 3,

        /// <summary>
        /// The NotEqual operator precedence.
        /// </summary>
        NotEqual = 4,

        /// <summary>
        /// The LessThan operator precedence.
        /// </summary>
        LessThan = 5,

        /// <summary>
        /// The GreaterThan operator precedence.
        /// </summary>
        GreaterThan = 6,

        /// <summary>
        /// The LessThanOrEqual operator precedence.
        /// </summary>
        LessThanOrEqual = 7,

        /// <summary>
        /// The GreaterThanOrEqual operator precedence.
        /// </summary>
        GreaterThanOrEqual = 8,

        /// <summary>
        /// The Not operator precedence.
        /// </summary>
        Not = 9,

        /// <summary>
        /// The BitwiseOr operator precedence.
        /// </summary>
        BitwiseOr = 10,

        /// <summary>
        /// The BitwiseXOr operator precedence.
        /// </summary>
        BitwiseXOr = 11,

        /// <summary>
        /// The BitwiseAnd operator precedence.
        /// </summary>
        BitwiseAnd = 12,

        /// <summary>
        /// The LeftShift operator precedence.
        /// </summary>
        LeftShift = 13,

        /// <summary>
        /// The RightShift operator precedence.
        /// </summary>
        RightShift = 14,

        /// <summary>
        /// The Add operator precedence.
        /// </summary>
        Add = 15,

        /// <summary>
        /// The Subtract operator precedence.
        /// </summary>
        Subtract = 16,

        /// <summary>
        /// The Multiply operator precedence.
        /// </summary>
        Multiply = 17,

        /// <summary>
        /// The Modulus operator precedence.
        /// </summary>
        Modulus = 18,

        /// <summary>
        /// The Divide operator precedence.
        /// </summary>
        Divide = 19,

        /// <summary>
        /// The NullCoalescing operator precedence.
        /// </summary>
        NullCoalescing = 20,

        /// <summary>
        /// The UnaryPlus operator precedence.
        /// </summary>
        UnaryPlus = 21,

        /// <summary>
        /// The UnaryMinus operator precedence.
        /// </summary>
        UnaryMinus = 22,

        /// <summary>
        /// The indexor operator '[' precedence
        /// </summary>
        IndexorOpen = 23,

        /// <summary>
        /// The indexor close operator ']' precedence
        /// </summary>
        IndexorClose = 24,

        /// <summary>
        /// The property '.' operator
        /// </summary>
        PropertyOperator = 25,

        /// <summary>
        /// The ParenthesisOpen '(' operator precedence.
        /// </summary>
        ParenthesisOpen = 26,

        /// <summary>
        /// The ParenthesisClose ')' operator precedence.
        /// </summary>
        ParenthesisClose = 27
    }
}
