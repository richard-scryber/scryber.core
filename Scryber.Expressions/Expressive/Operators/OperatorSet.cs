using System;
using System.Collections.Generic;
using Scryber.Expressive.Exceptions;
using Scryber.Expressive.Operators.Additive;
using Scryber.Expressive.Operators.Bitwise;
using Scryber.Expressive.Operators.Conditional;
using Scryber.Expressive.Operators.Grouping;
using Scryber.Expressive.Operators.Logical;
using Scryber.Expressive.Operators.Multiplicative;
using Scryber.Expressive.Operators.Relational;

namespace Scryber.Expressive.Operators
{
    public class OperatorSet : Dictionary<string, IOperator>
    {
        public OperatorSet()
            : this(StringComparer.OrdinalIgnoreCase)
        {
        }

        public OperatorSet(StringComparer comparer)
            : base(comparer)
        { }

        public void RegisterOperator(IOperator op, bool force = false)
        {
            if (op is null)
            {
                throw new ArgumentNullException(nameof(op));
            }

            foreach (var tag in op.Tags)
            {
                if (!force && this.ContainsKey(tag))
                {
                    throw new OperatorNameAlreadyRegisteredException(tag);
                }

                this[tag] = op;
            }
        }

        public void UnregisterOperator(string tag)
        {
            if (this.ContainsKey(tag))
            {
                this.Remove(tag);
            }
        }

        public static OperatorSet CreateDefault(ExpressiveOptions options)
        {
            if ((options & ExpressiveOptions.IgnoreCaseForParsing) > 0)
                return CreateDefault(StringComparer.OrdinalIgnoreCase);
            else
                return CreateDefault(StringComparer.Ordinal);
        }

        public static OperatorSet CreateDefault(StringComparer comparer)
        {
            OperatorSet set = new OperatorSet(comparer);
            // Additive
            set.RegisterOperator(new PlusOperator());
            set.RegisterOperator(new SubtractOperator());
            //Bitwise
            set.RegisterOperator(new BitwiseAndOperator());
            set.RegisterOperator(new BitwiseOrOperator());
            set.RegisterOperator(new BitwiseExclusiveOrOperator());
            set.RegisterOperator(new LeftShiftOperator());
            set.RegisterOperator(new RightShiftOperator());
            //Conditional
            set.RegisterOperator(new NullCoalescingOperator());
            //Grouping
            set.RegisterOperator(new ParenthesisCloseOperator());
            set.RegisterOperator(new ParenthesisOpenOperator());
            set.RegisterOperator(new IndexOpenOperator());
            set.RegisterOperator(new IndexCloseOperator());
            set.RegisterOperator(new PropertyOperator());
            // Logic
            set.RegisterOperator(new AndOperator());
            set.RegisterOperator(new NotOperator());
            set.RegisterOperator(new OrOperator());
            //Multiplicative
            set.RegisterOperator(new DivideOperator());
            set.RegisterOperator(new ModulusOperator());
            set.RegisterOperator(new MultiplyOperator());
            //Relational
            set.RegisterOperator(new EqualOperator());
            set.RegisterOperator(new GreaterThanOperator());
            set.RegisterOperator(new GreaterThanOrEqualOperator());
            set.RegisterOperator(new LessThanOperator());
            set.RegisterOperator(new LessThanOrEqualOperator());
            set.RegisterOperator(new NotEqualOperator());

            return set;
        }
    }
}
