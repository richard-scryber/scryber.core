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

        public OperatorSet(ExpressiveOptions options)
            : this(
                  ((options & ExpressiveOptions.IgnoreCaseForParsing) > 0)
                        ? StringComparer.OrdinalIgnoreCase
                        : StringComparer.Ordinal)
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

    }
}
