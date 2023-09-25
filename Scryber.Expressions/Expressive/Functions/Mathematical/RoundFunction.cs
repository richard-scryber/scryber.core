﻿using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Mathematical
{
    public class RoundFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Round"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);
            double result;
            if (parameters.Length == 1)
            {
                result = Math.Round(Convert.ToDouble(parameters[0].Evaluate(variables)));
            }
            else
            {
                result = Math.Round(Convert.ToDouble(parameters[0].Evaluate(variables)), Convert.ToInt32(parameters[1].Evaluate(variables)));
            }

            return result;
        }

        #endregion
    }
}
