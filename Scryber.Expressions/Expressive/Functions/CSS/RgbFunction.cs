using System;
using System.Collections.Generic;
using Scryber.Drawing;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.CSS
{
    /// <summary>
    /// Implements the Var(name,default) expressive function
    /// </summary>
    public class RGBFunction : FunctionBase
    {

        /// <summary>
        /// Gets the name of the function  - Var
        /// </summary>
        public override string Name
        {
            get { return "rgb"; }
        }

        /// <summary>
        /// Creates a new instance of the var function
        /// </summary>
        public RGBFunction()
        {
        }

        /// <summary>
        /// Evaluates the parameters in a Var expression returning either a parameter value, or the default if available.
        /// </summary>
        /// <param name="parameters">The expression parameters</param>
        /// <param name="context">The current expression context</param>
        /// <returns>The value of the parameter, or the default (second parameter) or null</returns>
        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 3);
            object r = parameters[0].Evaluate(variables);
            object g = parameters[1].Evaluate(variables);
            object b = parameters[2].Evaluate(variables);

            int rd, gd, bd;
            if (r is Unit ur)
            {
                if(ur.IsRelative && ur.Units == PageUnits.Percent)
                    rd = (int)Math.Round(255.0 * (ur.Value / 100.0));
                else
                {
                    throw new NotSupportedException("The units " + ur.Units + " is not supported in RGB calculations.");
                }
            }
            else
            {
                rd = (int)Math.Round(Convert.ToDouble(r));
            }
            
            if (g is Unit gr )
            {
                if(gr.IsRelative && gr.Units == PageUnits.Percent)
                    gd = (int)Math.Round(255.0 * (gr.Value / 100.0));
                else
                {
                    throw new NotSupportedException("The units " + gr.Units + " is not supported in RGB calculations.");
                }
            }
            else
            {
                gd = (int)Math.Round(Convert.ToDouble(g));
            }
            
            if (b is Unit br )
            {
                if(br.IsRelative && br.Units == PageUnits.Percent)
                    bd= (int)Math.Round(255.0 * (br.Value / 100.0));
                else
                {
                    throw new NotSupportedException("The units " + br.Units + " is not supported in RGB calculations.");
                }
            }
            else
            {
                bd = (int)Math.Round(Convert.ToDouble(b));
            }


            var c = new Color(rd, gd, bd);
            
            
            
            return c;
        }
    }
}
