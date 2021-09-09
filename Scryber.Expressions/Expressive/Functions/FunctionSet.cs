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

        public FunctionSet(ExpressiveOptions options)
            : this(
                  ((options & ExpressiveOptions.IgnoreCaseForParsing) > 0)
                        ? StringComparer.OrdinalIgnoreCase
                        : StringComparer.Ordinal)
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

    }
}
