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

        public static FunctionSet CreateDefaultSet(StringComparer comparer)
        {
            var set = new FunctionSet(comparer);
            set.RegisterFunction(new DateFunction());
            set.RegisterFunction(new DecimalFunction());
            set.RegisterFunction(new DoubleFunction());
            set.RegisterFunction(new IntegerFunction());
            set.RegisterFunction(new LongFunction());
            set.RegisterFunction(new StringFunction());
            // Date
            set.RegisterFunction(new AddDaysFunction());
            set.RegisterFunction(new AddHoursFunction());
            set.RegisterFunction(new AddMillisecondsFunction());
            set.RegisterFunction(new AddMinutesFunction());
            set.RegisterFunction(new AddMonthsFunction());
            set.RegisterFunction(new AddSecondsFunction());
            set.RegisterFunction(new AddYearsFunction());
            set.RegisterFunction(new DayOfFunction());
            set.RegisterFunction(new DaysBetweenFunction());
            set.RegisterFunction(new HourOfFunction());
            set.RegisterFunction(new HoursBetweenFunction());
            set.RegisterFunction(new MillisecondOfFunction());
            set.RegisterFunction(new MillisecondsBetweenFunction());
            set.RegisterFunction(new MinuteOfFunction());
            set.RegisterFunction(new MinutesBetweenFunction());
            set.RegisterFunction(new MonthOfFunction());
            set.RegisterFunction(new SecondOfFunction());
            set.RegisterFunction(new SecondsBetweenFunction());
            set.RegisterFunction(new YearOfFunction());
            // Mathematical
            set.RegisterFunction(new AbsFunction());
            set.RegisterFunction(new AcosFunction());
            set.RegisterFunction(new AsinFunction());
            set.RegisterFunction(new AtanFunction());
            set.RegisterFunction(new CeilingFunction());
            set.RegisterFunction(new CosFunction());
            set.RegisterFunction(new CountFunction());
            set.RegisterFunction(new ExpFunction());
            set.RegisterFunction(new FloorFunction());
            set.RegisterFunction(new IEEERemainderFunction());
            set.RegisterFunction(new Log10Function());
            set.RegisterFunction(new LogFunction());
            set.RegisterFunction(new PowFunction());
            set.RegisterFunction(new RandomFunction());
            set.RegisterFunction(new RoundFunction());
            set.RegisterFunction(new SignFunction());
            set.RegisterFunction(new SinFunction());
            set.RegisterFunction(new SqrtFunction());
            set.RegisterFunction(new SumFunction());
            set.RegisterFunction(new TanFunction());
            set.RegisterFunction(new TruncateFunction());
            // Mathematical Constants
            set.RegisterFunction(new EFunction());
            set.RegisterFunction(new PIFunction());
            // Logical
            set.RegisterFunction(new IfFunction());
            set.RegisterFunction(new InFunction());
            set.RegisterFunction(new IndexFunction());
            // Relational
            set.RegisterFunction(new MaxFunction());
            set.RegisterFunction(new MinFunction());
            // Statistical
            set.RegisterFunction(new AverageFunction());
            set.RegisterFunction(new MeanFunction());
            set.RegisterFunction(new MedianFunction());
            set.RegisterFunction(new ModeFunction());
            // String
            set.RegisterFunction(new ContainsFunction());
            set.RegisterFunction(new EndsWithFunction());
            set.RegisterFunction(new LengthFunction());
            set.RegisterFunction(new PadLeftFunction());
            set.RegisterFunction(new PadRightFunction());
            set.RegisterFunction(new RegexFunction());
            set.RegisterFunction(new StartsWithFunction());
            set.RegisterFunction(new SubstringFunction());
            set.RegisterFunction(new ConcatFunction());
            set.RegisterFunction(new IndexOfFunction());
            set.RegisterFunction(new JoinFunction());
            //css
            set.RegisterFunction(new VarFunction());
            set.RegisterFunction(new CalcFunction());
            return set;
        }
    }
}
