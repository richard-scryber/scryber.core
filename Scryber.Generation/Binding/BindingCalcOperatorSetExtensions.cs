using System;
using System.Collections.Generic;
using Scryber.Expressive.Exceptions;
using Scryber.Expressive.Operators;
using Scryber.Expressive.Operators.Additive;
using Scryber.Expressive.Operators.Bitwise;
using Scryber.Expressive.Operators.Conditional;
using Scryber.Expressive.Operators.Grouping;
using Scryber.Expressive.Operators.Logical;
using Scryber.Expressive.Operators.Multiplicative;
using Scryber.Expressive.Operators.Relational;

namespace Scryber.Binding
{
    public static class BindingCalcOperatorSetExtensions
    {
        private static readonly List<IOperator> _all;
        private static object _lock;


        static BindingCalcOperatorSetExtensions()
        {
            _lock = new object();
            _all = new List<IOperator>();

            _all.Add(new PlusOperator());
            _all.Add(new SubtractOperator());
            //Bitwise
            _all.Add(new BitwiseAndOperator());
            _all.Add(new BitwiseOrOperator());
            _all.Add(new BitwiseExclusiveOrOperator());
            _all.Add(new LeftShiftOperator());
            _all.Add(new RightShiftOperator());
            //Conditional
            _all.Add(new NullCoalescingOperator());
            //Grouping
            _all.Add(new ParenthesisCloseOperator());
            _all.Add(new ParenthesisOpenOperator());
            _all.Add(new IndexOpenOperator());
            _all.Add(new IndexCloseOperator());
            _all.Add(new PropertyOperator());
            // Logic
            _all.Add(new AndOperator());
            _all.Add(new NotOperator());
            _all.Add(new OrOperator());
            //Multiplicative
            _all.Add(new DivideOperator());
            _all.Add(new ModulusOperator());
            _all.Add(new MultiplyOperator());
            //Relational
            _all.Add(new EqualOperator());
            _all.Add(new GreaterThanOperator());
            _all.Add(new GreaterThanOrEqualOperator());
            _all.Add(new LessThanOperator());
            _all.Add(new LessThanOrEqualOperator());
            _all.Add(new NotEqualOperator());
        }


        // <summary>
        /// Extension method that adds all the standard functions to the FunctionSet
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static OperatorSet AddDefaultOperators(this OperatorSet set)
        {
            lock (_lock)
            {
                foreach (var o in _all)
                {
                    set.RegisterOperator(o);
                }
            }
            return set;
        }

        /// <summary>
        /// Registers a new operator with the name, that can be used in calculations
        /// </summary>
        /// <param name="function">The name to be used.</param>
        /// <returns>True</returns>
        internal static bool RegisterOperator(IOperator operation)
        {
            if (null == operation)
                throw new ArgumentNullException(nameof(operation));

            //Just make it thread safe for the insertion
            lock (_lock)
            {
                _all.Insert(0, operation);
            }
            return true;
        }

        /// <summary>
        /// Registers one or more functions with the default set
        /// </summary>
        /// <param name="functions"></param>
        /// <returns></returns>
        internal static int RegisterOperator(params IOperator[] operators)
        {
            if (null == operators)
                return 0;

            //Just make it thread safe for the insertion
            lock (_lock)
            {
                for (var i = 0; i < operators.Length; i++)
                {
                    if (operators[i] == null)
                        throw new NullReferenceException("Null operator reference at index " + i);

                    _all.Insert(0, operators[i]);
                }

            }

            return operators.Length;
        }

    }
}
