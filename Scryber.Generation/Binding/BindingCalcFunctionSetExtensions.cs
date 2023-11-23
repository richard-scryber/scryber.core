using System;
using System.Collections.Generic;

using Scryber.Expressive;
using Scryber.Expressive.Exceptions;
using Scryber.Expressive.Functions;
using Scryber.Expressive.Functions.Conversion;
using Scryber.Expressive.Functions.Date;
using Scryber.Expressive.Functions.Logical;
using Scryber.Expressive.Functions.Mathematical;
using Scryber.Expressive.Functions.Relational;
using Scryber.Expressive.Functions.Statistical;
using Scryber.Expressive.Functions.Coalesce;
using Scryber.Expressive.Functions.String;
using Scryber.Expressive.Functions.CSS;

namespace Scryber.Binding
{
    public static class BindingCalcFunctionSetExtensions
    {

        private static readonly List<IFunction> _all;
        private static object _lock;

        /// <summary>
        /// Static constructor that will create our new default set of functions
        /// </summary>
        static BindingCalcFunctionSetExtensions()
        {
            _lock = new object();
            _all = new List<IFunction>();
            //conversion
            _all.Add(new DateFunction());
            _all.Add(new DecimalFunction());
            _all.Add(new DoubleFunction());
            _all.Add(new IntegerFunction());
            _all.Add(new LongFunction());
            _all.Add(new StringFunction());
            _all.Add(new BoolFunction());
            _all.Add(new TypeOfFunction());
            // Date
            _all.Add(new AddDaysFunction());
            _all.Add(new AddHoursFunction());
            _all.Add(new AddMillisecondsFunction());
            _all.Add(new AddMinutesFunction());
            _all.Add(new AddMonthsFunction());
            _all.Add(new AddSecondsFunction());
            _all.Add(new AddYearsFunction());
            _all.Add(new DayOfFunction());
            _all.Add(new DaysBetweenFunction());
            _all.Add(new HourOfFunction());
            _all.Add(new HoursBetweenFunction());
            _all.Add(new MillisecondOfFunction());
            _all.Add(new MillisecondsBetweenFunction());
            _all.Add(new MinuteOfFunction());
            _all.Add(new MinutesBetweenFunction());
            _all.Add(new MonthOfFunction());
            _all.Add(new SecondOfFunction());
            _all.Add(new SecondsBetweenFunction());
            _all.Add(new YearOfFunction());
            // Mathematical
            _all.Add(new AbsFunction());
            _all.Add(new AcosFunction());
            _all.Add(new AsinFunction());
            _all.Add(new AtanFunction());
            _all.Add(new CeilingFunction());
            _all.Add(new CosFunction());
            _all.Add(new ExpFunction());
            _all.Add(new FloorFunction());
            _all.Add(new IEEERemainderFunction());
            _all.Add(new Log10Function());
            _all.Add(new LogFunction());
            _all.Add(new PowFunction());
            _all.Add(new RandomFunction());
            _all.Add(new RoundFunction());
            _all.Add(new SignFunction());
            _all.Add(new SinFunction());
            _all.Add(new SqrtFunction());
            _all.Add(new SumFunction());
            _all.Add(new SumOfFunction());
            _all.Add(new TanFunction());
            _all.Add(new TruncateFunction());
            _all.Add(new RadiansFunction());
            _all.Add(new DegreesFunction());
            // Mathematical Constants
            _all.Add(new EFunction());
            _all.Add(new PIFunction());
            // Logical
            _all.Add(new IfFunction());
            _all.Add(new InFunction());
            _all.Add(new IndexFunction());
            // Relational
            _all.Add(new MaxFunction());
            _all.Add(new MinFunction());
            _all.Add(new MinOfFunction());
            _all.Add(new MaxOfFunction());
            _all.Add(new CountFunction());
            _all.Add(new CountOfFunction());
            //coalesce
            _all.Add(new EachFunction());
            _all.Add(new EachOfFunction());
            _all.Add(new SortByFunction());
            _all.Add(new SelectWhereFunction());
            _all.Add(new FirstWhereFunction());
            //TODO: Add Merge, Splice

            // Statistical
            _all.Add(new AverageFunction());
            _all.Add(new AverageOfFunction());
            _all.Add(new MeanFunction());
            _all.Add(new MedianFunction());
            _all.Add(new ModeFunction());
            // String
            _all.Add(new ContainsFunction());
            _all.Add(new EndsWithFunction());
            _all.Add(new LengthFunction());
            _all.Add(new PadLeftFunction());
            _all.Add(new PadRightFunction());
            _all.Add(new RegexFunction());
            _all.Add(new StartsWithFunction());
            _all.Add(new SubstringFunction());
            _all.Add(new ConcatFunction());
            _all.Add(new IndexOfFunction());
            _all.Add(new JoinFunction());
            _all.Add(new SplitFunction());
            _all.Add(new ReplaceFunction());
            _all.Add(new ToUpperFunction());
            _all.Add(new ToLowerFunction());
            _all.Add(new TrimFunction());
            _all.Add(new TrimLeftFunction());
            _all.Add(new TrimRightFunction());

            //css
            _all.Add(new VarFunction());
            _all.Add(new CalcFunction());

            _all.Add(new EvalFunction());
        }

        /// <summary>
        /// Extension method that adds all the standard functions to the FunctionSet
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static FunctionSet AddDefaultFunctions(this FunctionSet set)
        {
            lock (_lock)
            {
                foreach (var f in _all)
                {
                    set.RegisterFunction(f);
                }
            }
            return set;
        }

        /// <summary>
        /// Registers a new function with the name, that can be used in calculations
        /// </summary>
        /// <param name="function">The name to be used.</param>
        /// <returns>True</returns>
        internal static bool RegisterFunction(IFunction function)
        {
            if (null == function)
                throw new ArgumentNullException(nameof(function));

            //Just make it thread safe for the insertion
            lock (_lock)
            {
                _all.Insert(0, function);
            }
            return true;
        }

        /// <summary>
        /// Registers one or more functions with the default set
        /// </summary>
        /// <param name="functions"></param>
        /// <returns></returns>
        internal static int RegisterFunction(params IFunction[] functions)
        {
            if (null == functions)
                return 0;

            //Just make it thread safe for the insertion
            lock (_lock)
            {
                for(var i = 0; i < functions.Length; i++)
                {
                    if (functions[i] == null)
                        throw new NullReferenceException("Null function reference at index " + i);

                    _all.Insert(0, functions[i]);
                }
                
            }

            return functions.Length;
        }
    }
}
