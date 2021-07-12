
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        private readonly IDictionary<string, Func<IExpression[], IDictionary<string, object>, object>> registeredFunctions;
        private readonly IDictionary<string, IOperator> registeredOperators;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the data associate with the current data context
        /// </summary>
        public object CurrentDataContext { get; set; }

        public int CurrentDataIndex { get; set; }

        internal ExpressiveOptions Options { get; }

        internal CultureInfo CurrentCulture { get; }

        internal CultureInfo DecimalCurrentCulture { get; }

        internal char DecimalSeparator { get; }

        internal IEnumerable<string> FunctionNames => this.registeredFunctions.Keys.OrderByDescending(k => k.Length);

        internal IEnumerable<string> OperatorNames => this.registeredOperators.Keys.OrderByDescending(k => k.Length);

        private bool IsCaseInsensitiveEqualityEnabled => 
#pragma warning disable 618 // As it is our own warning this is safe enough until we actually get rid
            Options.HasFlag(ExpressiveOptions.IgnoreCase) || Options.HasFlag(ExpressiveOptions.IgnoreCaseForEquality);
#pragma warning restore 618

        internal StringComparison EqualityStringComparison => IsCaseInsensitiveEqualityEnabled
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        internal bool IsCaseInsensitiveParsingEnabled =>
#pragma warning disable 618 // As it is our own warning this is safe enough until we actually get rid
            Options.HasFlag(ExpressiveOptions.IgnoreCase) || Options.HasFlag(ExpressiveOptions.IgnoreCaseForParsing);
#pragma warning restore 618

        internal IEqualityComparer<string> ParsingStringComparer => IsCaseInsensitiveParsingEnabled
            ? StringComparer.OrdinalIgnoreCase
            : (IEqualityComparer<string>)EqualityComparer<string>.Default;

        internal StringComparison ParsingStringComparison => IsCaseInsensitiveParsingEnabled
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class with the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The <see cref="ScryberOptions"/> to use when evaluating.</param>
        public Context(ExpressiveOptions options)
            : this(options, CultureInfo.CurrentCulture, CultureInfo.InvariantCulture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class with the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The <see cref="ScryberOptions"/> to use when evaluating.</param>
        /// <param name="mainCurrentCulture">The <see cref="CultureInfo"/> for use in general parsing/conversions.</param>
        /// <param name="decimalCurrentCulture">The <see cref="CultureInfo"/> for use in decimal parsing/conversions.</param>
        public Context(ExpressiveOptions options, CultureInfo mainCurrentCulture, CultureInfo decimalCurrentCulture)
        {
            Options = options;

            this.CurrentDataIndex = -1;
            this.CurrentDataContext = null;

            this.CurrentCulture = mainCurrentCulture ?? throw new ArgumentNullException(nameof(mainCurrentCulture));
            // For now we will ignore any specific cultures but keeping it in a single place to simplify changing later if required.
            this.DecimalCurrentCulture = decimalCurrentCulture ?? throw new ArgumentNullException(nameof(decimalCurrentCulture));

            DecimalSeparator = Convert.ToChar(this.DecimalCurrentCulture.NumberFormat.NumberDecimalSeparator, this.DecimalCurrentCulture);
            this.registeredFunctions = new Dictionary<string, Func<IExpression[], IDictionary<string, object>, object>>(this.ParsingStringComparer);
            this.registeredOperators = new Dictionary<string, IOperator>(this.ParsingStringComparer);

            #region Operators
            // TODO: Do we allow for turning off operators?
            // Additive
            this.RegisterOperator(new PlusOperator());
            this.RegisterOperator(new SubtractOperator());
            // Bitwise
            this.RegisterOperator(new BitwiseAndOperator());
            this.RegisterOperator(new BitwiseOrOperator());
            this.RegisterOperator(new BitwiseExclusiveOrOperator());
            this.RegisterOperator(new LeftShiftOperator());
            this.RegisterOperator(new RightShiftOperator());
            // Conditional
            this.RegisterOperator(new NullCoalescingOperator());
            // Grouping
            this.RegisterOperator(new ParenthesisCloseOperator());
            this.RegisterOperator(new ParenthesisOpenOperator());
            this.RegisterOperator(new IndexOpenOperator());
            this.RegisterOperator(new IndexCloseOperator());
            this.RegisterOperator(new PropertyOperator());
            // Logic
            this.RegisterOperator(new AndOperator());
            this.RegisterOperator(new NotOperator());
            this.RegisterOperator(new OrOperator());
            // Multiplicative
            this.RegisterOperator(new DivideOperator());
            this.RegisterOperator(new ModulusOperator());
            this.RegisterOperator(new MultiplyOperator());
            // Relational
            this.RegisterOperator(new EqualOperator());
            this.RegisterOperator(new GreaterThanOperator());
            this.RegisterOperator(new GreaterThanOrEqualOperator());
            this.RegisterOperator(new LessThanOperator());
            this.RegisterOperator(new LessThanOrEqualOperator());
            this.RegisterOperator(new NotEqualOperator());
            
            #endregion

            #region Functions
            // Conversion
            this.RegisterFunction(new DateFunction());
            this.RegisterFunction(new DecimalFunction());
            this.RegisterFunction(new DoubleFunction());
            this.RegisterFunction(new IntegerFunction());
            this.RegisterFunction(new LongFunction());
            this.RegisterFunction(new StringFunction());
            // Date
            this.RegisterFunction(new AddDaysFunction());
            this.RegisterFunction(new AddHoursFunction());
            this.RegisterFunction(new AddMillisecondsFunction());
            this.RegisterFunction(new AddMinutesFunction());
            this.RegisterFunction(new AddMonthsFunction());
            this.RegisterFunction(new AddSecondsFunction());
            this.RegisterFunction(new AddYearsFunction());
            this.RegisterFunction(new DayOfFunction());
            this.RegisterFunction(new DaysBetweenFunction());
            this.RegisterFunction(new HourOfFunction());
            this.RegisterFunction(new HoursBetweenFunction());
            this.RegisterFunction(new MillisecondOfFunction());
            this.RegisterFunction(new MillisecondsBetweenFunction());
            this.RegisterFunction(new MinuteOfFunction());
            this.RegisterFunction(new MinutesBetweenFunction());
            this.RegisterFunction(new MonthOfFunction());
            this.RegisterFunction(new SecondOfFunction());
            this.RegisterFunction(new SecondsBetweenFunction());
            this.RegisterFunction(new YearOfFunction());
            // Mathematical
            this.RegisterFunction(new AbsFunction());
            this.RegisterFunction(new AcosFunction());
            this.RegisterFunction(new AsinFunction());
            this.RegisterFunction(new AtanFunction());
            this.RegisterFunction(new CeilingFunction());
            this.RegisterFunction(new CosFunction());
            this.RegisterFunction(new CountFunction());
            this.RegisterFunction(new ExpFunction());
            this.RegisterFunction(new FloorFunction());
            this.RegisterFunction(new IEEERemainderFunction());
            this.RegisterFunction(new Log10Function());
            this.RegisterFunction(new LogFunction());
            this.RegisterFunction(new PowFunction());
            this.RegisterFunction(new RandomFunction());
            this.RegisterFunction(new RoundFunction());
            this.RegisterFunction(new SignFunction());
            this.RegisterFunction(new SinFunction());
            this.RegisterFunction(new SqrtFunction());
            this.RegisterFunction(new SumFunction());
            this.RegisterFunction(new TanFunction());
            this.RegisterFunction(new TruncateFunction());
            // Mathematical Constants
            this.RegisterFunction(new EFunction());
            this.RegisterFunction(new PIFunction());
            // Logical
            this.RegisterFunction(new IfFunction());
            this.RegisterFunction(new InFunction());
            // Relational
            this.RegisterFunction(new MaxFunction());
            this.RegisterFunction(new MinFunction());
            // Statistical
            this.RegisterFunction(new AverageFunction());
            this.RegisterFunction(new MeanFunction());
            this.RegisterFunction(new MedianFunction());
            this.RegisterFunction(new ModeFunction());
            // String
            this.RegisterFunction(new ContainsFunction());
            this.RegisterFunction(new EndsWithFunction());
            this.RegisterFunction(new LengthFunction());
            this.RegisterFunction(new PadLeftFunction());
            this.RegisterFunction(new PadRightFunction());
            this.RegisterFunction(new RegexFunction());
            this.RegisterFunction(new StartsWithFunction());
            this.RegisterFunction(new SubstringFunction());
            this.RegisterFunction(new ConcatFunction());
            this.RegisterFunction(new IndexOfFunction());
            this.RegisterFunction(new JoinFunction());
            #endregion
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers the supplied <paramref name="function"/> for use within compiling and evaluating an <see cref="Expression"/>.
        /// </summary>
        /// <param name="functionName">The name of the function to register.</param>
        /// <param name="function">A <see cref="Func{T}"/> to perform the function evaluation.</param>
        /// <param name="force">Whether to forcefully override any existing function.</param>
        public void RegisterFunction(string functionName, Func<IExpression[], IDictionary<string, object>, object> function, bool force = false)
        {
            this.CheckForExistingFunctionName(functionName, force);

            this.registeredFunctions[functionName] = function;
        }

        /// <summary>
        /// Registers the supplied <paramref name="function"/> for use within compiling and evaluating an <see cref="Expression"/>.
        /// </summary>
        /// <param name="function">The <see cref="IFunction"/> to perform the function evaluation.</param>
        /// <param name="force">Whether to forcefully override any existing function.</param>
        public void RegisterFunction(IFunction function, bool force = false)
        {
            if (function is null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            this.RegisterFunction(
                function.Name,
                (p, a) =>
                {
                    function.Variables = a;

                    return function.Evaluate(p, this);
                },
                force);
        }

        /// <summary>
        /// Registers the supplied <paramref name="op"/> for use within compiling and evaluating an <see cref="Expression"/>.
        /// </summary>
        /// <param name="op">The <see cref="IOperator"/> implementation to register.</param>
        /// <param name="force">Whether to forcefully override any existing <see cref="IOperator"/>.</param>
        /// <remarks>
        /// Please if you are calling this with your own <see cref="IOperator"/> implementations do seriously consider raising an issue to add it in to the general framework:
        /// https://github.com/bijington/Scryber
        /// </remarks>
        public void RegisterOperator(IOperator op, bool force = false)
        {
            if (op is null)
            {
                throw new ArgumentNullException(nameof(op));
            }

            foreach (var tag in op.Tags)
            {
                if (!force && this.registeredOperators.ContainsKey(tag))
                {
                    throw new OperatorNameAlreadyRegisteredException(tag);
                }

                this.registeredOperators[tag] = op;
            }
        }

        /// <summary>
        /// Removes the function from the available set of functions when evaluating. 
        /// </summary>
        /// <param name="functionName">The name of the function to remove.</param>
        public void UnregisterFunction(string functionName)
        {
            if (!this.registeredFunctions.ContainsKey(functionName))
            {
                // TODO: do we throw?
            }

            this.registeredFunctions.Remove(functionName);
        }

        /// <summary>
        /// Removes the operator from the available set of operators when evaluating. 
        /// </summary>
        /// <param name="tag">The tag of the operator to remove.</param>
        public void UnregisterOperator(string tag)
        {
            if (!this.registeredOperators.ContainsKey(tag))
            {
                // TODO: do we throw?
            }

            this.registeredOperators.Remove(tag);
        }

        #endregion

        #region Internal Methods

        internal bool TryGetFunction(string functionName, out Func<IExpression[], IDictionary<string, object>, object> value)
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