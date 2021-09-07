using System;
using System.Collections.Generic;
using Scryber.Expressive.Exceptions;
using Scryber.Expressive.Functions.Conversion;
using Scryber.Expressive.Functions.Date;
using Scryber.Expressive.Functions.Logical;
using Scryber.Expressive.Functions.Mathematical;
using Scryber.Expressive.Functions.Relational;
using Scryber.Expressive.Functions.Statistical;
using Scryber.Expressive.Functions.String;
using Scryber.Expressive.Functions.CSS;
using System.Runtime.CompilerServices;

namespace Scryber.Expressive.Functions
{
    public class FunctionSet : Dictionary<string, ExecFunction>
    {
        public FunctionSet()
            : this(StringComparer.OrdinalIgnoreCase)
        {
        }

        public FunctionSet(StringComparer comparer)
            : base(comparer)
        {

        }

        public void RegisterFunction(IFunction function, bool force = false)
        {

            ExecFunction exec = new ExecFunction((p, a, c) =>
            {
                return function.Evaluate(p, a, c);
            });


            if (force)
                this[function.Name] = exec;
            else if (!this.ContainsKey(function.Name))
                this.Add(function.Name, exec);
            else
                throw new FunctionNameAlreadyRegisteredException(function.Name);

        }

        public void UnregisterFunction(string name)
        {
            if (this.ContainsKey(name))
            {
                this.Remove(name);
            }
        }

        public static FunctionSet CreateDefault(ExpressiveOptions options)
        {
            if ((options & ExpressiveOptions.IgnoreCaseForParsing) > 0)
                return CreateDefaultSet(StringComparer.OrdinalIgnoreCase);
            else
                return CreateDefaultSet(StringComparer.Ordinal);
        }

        private static readonly List<IFunction> _all;
        private static object _lock;

        static FunctionSet()
        {
            _lock = new object();
            _all = new List<IFunction>();

            _all.Add(new DateFunction());
            _all.Add(new DecimalFunction());
            _all.Add(new DoubleFunction());
            _all.Add(new IntegerFunction());
            _all.Add(new LongFunction());
            _all.Add(new StringFunction());
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
            _all.Add(new CountFunction());
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
            _all.Add(new TanFunction());
            _all.Add(new TruncateFunction());
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
            // Statistical
            _all.Add(new AverageFunction());
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
            //css
            _all.Add(new VarFunction());
            _all.Add(new CalcFunction());
        }

        public static FunctionSet CreateDefaultSet(StringComparer comparer)
        {
            var set = new FunctionSet(comparer);

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
        public static bool RegisterFunction(IFunction function)
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
    }
}
