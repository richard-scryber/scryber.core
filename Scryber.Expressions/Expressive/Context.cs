
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using Scryber.Expressive.Exceptions;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Functions;
using Scryber.Expressive.Functions.Conversion;
using Scryber.Expressive.Functions.Date;
using Scryber.Expressive.Functions.Logical;
using Scryber.Expressive.Functions.Mathematical;
using Scryber.Expressive.Functions.Relational;
using Scryber.Expressive.Functions.Statistical;
using Scryber.Expressive.Functions.String;
using Scryber.Expressive.Operators;
using Scryber.Expressive.Operators.Additive;
using Scryber.Expressive.Operators.Bitwise;
using Scryber.Expressive.Operators.Conditional;
using Scryber.Expressive.Operators.Grouping;
using Scryber.Expressive.Operators.Logical;
using Scryber.Expressive.Operators.Multiplicative;
using Scryber.Expressive.Operators.Relational;

namespace Scryber.Expressive
{

    public delegate object ExecFunction(IExpression[] parameters, IDictionary<string, object> variables, Context context);

    /// <summary>
    /// Represents context related details about compiling and evaluating an <see cref="IExpression"/>.
    /// </summary>
    public class Context
    {
        internal const char DateSeparator = '#';
        internal const char ParameterSeparator = ',';

        internal const string CurrentDataVariableName = "{current}";

        #region Fields

        // TODO: Perhaps this knowledge is better held under specific expression compilers? Or is that a level too far?
        private readonly FunctionSet registeredFunctions;
        private readonly OperatorSet registeredOperators;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the data associate with the current data context
        /// </summary>
        public object CurrentDataContext { get; set; }

        public int CurrentDataIndex { get; set; }

        internal ExpressiveOptions Options { get; }

        public CultureInfo CurrentCulture { get; }

        public CultureInfo DecimalCurrentCulture { get; }

        internal char DecimalSeparator { get; }

        //internal IEnumerable<string> FunctionNames => this.registeredFunctions.Keys.OrderByDescending(k => k.Length);

        //internal IEnumerable<string> OperatorNames => this.registeredOperators.Keys.OrderByDescending(k => k.Length);

        private bool IsCaseInsensitiveEqualityEnabled => 
#pragma warning disable 618 // As it is our own warning this is safe enough until we actually get rid
            Options.HasFlag(ExpressiveOptions.IgnoreCase) || Options.HasFlag(ExpressiveOptions.IgnoreCaseForEquality);
#pragma warning restore 618

        public StringComparison EqualityStringComparison => IsCaseInsensitiveEqualityEnabled
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        public bool IsCaseInsensitiveParsingEnabled =>
#pragma warning disable 618 // As it is our own warning this is safe enough until we actually get rid
            Options.HasFlag(ExpressiveOptions.IgnoreCase) || Options.HasFlag(ExpressiveOptions.IgnoreCaseForParsing);
#pragma warning restore 618

        public IEqualityComparer<string> ParsingStringComparer => IsCaseInsensitiveParsingEnabled
            ? StringComparer.OrdinalIgnoreCase
            : (IEqualityComparer<string>)EqualityComparer<string>.Default;

        public StringComparison ParsingStringComparison => IsCaseInsensitiveParsingEnabled
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class with the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The <see cref="ScryberOptions"/> to use when evaluating.</param>
        /// <param name="functions">All the standard functions</param>
        /// <param name="operators">All The standard Operators</param>
        public Context(ExpressiveOptions options, FunctionSet functions, OperatorSet operators)
            : this(options, CultureInfo.CurrentCulture, CultureInfo.InvariantCulture, functions, operators)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class with the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The <see cref="ExpressiveOptions"/> to use when evaluating.</param>
        /// <param name="mainCurrentCulture">The <see cref="CultureInfo"/> for use in general parsing/conversions.</param>
        /// <param name="decimalCurrentCulture">The <see cref="CultureInfo"/> for use in decimal parsing/conversions.</param>
        public Context(ExpressiveOptions options, CultureInfo mainCurrentCulture, CultureInfo decimalCurrentCulture, FunctionSet functions, OperatorSet operators)
        {
            Options = options;

            this.CurrentDataIndex = -1;
            this.CurrentDataContext = null;

            this.CurrentCulture = mainCurrentCulture ?? throw new ArgumentNullException(nameof(mainCurrentCulture));
            // For now we will ignore any specific cultures but keeping it in a single place to simplify changing later if required.
            this.DecimalCurrentCulture = decimalCurrentCulture ?? throw new ArgumentNullException(nameof(decimalCurrentCulture));

            DecimalSeparator = Convert.ToChar(this.DecimalCurrentCulture.NumberFormat.NumberDecimalSeparator, this.DecimalCurrentCulture);
            this.registeredFunctions = functions ?? throw new ArgumentNullException(nameof(functions));
            this.registeredOperators = operators ?? throw new ArgumentNullException(nameof(operators));
        }

        #endregion


        #region Internal Methods

        internal bool TryGetFunction(string functionName, out ExecFunction value)
        {
            return this.registeredFunctions.TryGetValue(functionName, out value);
        }

        internal bool TryGetOperator(string operatorName, out IOperator value)
        {
            return this.registeredOperators.TryGetValue(operatorName, out value);
        }

        #endregion

        #region Private Methods

        private void CheckForExistingFunctionName(string functionName, bool force)
        {
            if (!force && this.registeredFunctions.ContainsKey(functionName))
            {
                throw new FunctionNameAlreadyRegisteredException(functionName);
            }
        }

        #endregion
    }
}