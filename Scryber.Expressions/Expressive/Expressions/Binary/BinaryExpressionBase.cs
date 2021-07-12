using Scryber.Expressive.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scryber.Expressive.Expressions.Binary
{
    /// <summary>
    /// Base class implementation of <see cref="IExpression"/>.
    /// </summary>
    public abstract class BinaryExpressionBase : IExpression
    {
        #region Fields

        protected readonly IExpression leftHandSide;
        protected readonly IExpression rightHandSide;

        #endregion

        /// <summary>
        /// Gets the underlying <see cref="Context"/>.
        /// </summary>
        protected Context Context { get; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="BinaryExpressionBase"/>.
        /// </summary>
        /// <param name="lhs">The left hand <see cref="IExpression"/>.</param>
        /// <param name="rhs">The right hand <see cref="IExpression"/>.</param>
        /// <param name="context">The <see cref="Context"/> used during the evaluation.</param>
        protected BinaryExpressionBase(IExpression lhs, IExpression rhs, Context context)
        {
            this.leftHandSide = lhs;
            this.Context = context;
            this.rightHandSide = rhs;
        }

        #endregion

        #region IExpression Members

        /// <inheritdoc />
        public virtual object Evaluate(IDictionary<string, object> variables)
        {
            if (this.leftHandSide is null)
            {
                throw new MissingParticipantException("The left hand side of the operation is missing.");
            }

            if (this.rightHandSide is null)
            {
                throw new MissingParticipantException("The right hand side of the operation is missing.");
            }

            // We will evaluate the left hand side but hold off on the right hand side as it may not be necessary
            var lhsResult = this.leftHandSide.Evaluate(variables);

            return this.EvaluateImpl(lhsResult, this.rightHandSide, variables);
        }

        #endregion

        /// <summary>
        /// The core evaluation logic for the overriding expression implementation.
        /// </summary>
        /// <param name="lhsResult">The already evaluated result for the left hand side of the expression.</param>
        /// <param name="rightHandSide">
        /// The <see cref="IExpression"/> right hand side of the expression.
        /// <remarks>
        /// This is left up to the implementor to evaluate to allow for short-circuiting.
        /// </remarks>
        /// </param>
        /// <param name="variables">The list of variables for use in evaluating.</param>
        /// <returns>The result of the evaluation.</returns>
        protected abstract object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables);

        /// <summary>
        /// Evaluates the supplied <paramref name="lhsResult"/> and <paramref name="rhs"/> and checks for any possible aggregate value results.
        /// </summary>
        /// <param name="lhsResult">The left hand side result.</param>
        /// <param name="rhs">The <see cref="IExpression"/> right hand side of the expression.</param>
        /// <param name="variables">The list of variables for use in evaluating.</param>
        /// <param name="resultSelector">How to return the result(s). <remarks>NOTE this will be called once per aggregate value if they exist.</remarks></param>
        /// <returns>The result of the evaluation.</returns>
        public static object EvaluateAggregates(object lhsResult, IExpression rhs, IDictionary<string, object> variables, Func<object, object, object> resultSelector)
        {
            if (rhs is null)
            {
                throw new ArgumentNullException(nameof(rhs));
            }

            if (resultSelector is null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            IList<object> lhsParticipants = new List<object>();
            IList<object> rhsParticipants = new List<object>();

            var rhsResult = rhs.Evaluate(variables);

            if (!(lhsResult is ICollection) && !(rhsResult is ICollection))
            {
                return resultSelector(lhsResult, rhsResult);
            }

            if (lhsResult is ICollection leftCollection)
            {
                foreach (var item in leftCollection)
                {
                    lhsParticipants.Add(item);
                }
            }
            if (rhsResult is ICollection rightCollection)
            {
                foreach (var item in rightCollection)
                {
                    rhsParticipants.Add(item);
                }
            }

            object[] result = null;

            if (lhsParticipants.Count == rhsParticipants.Count)
            {
                IList<object> resultList = new List<object>();

                for (var i = 0; i < lhsParticipants.Count; i++)
                {
                    resultList.Add(resultSelector(lhsParticipants[i], rhsParticipants[i]));
                }

                result = resultList.ToArray();
            }
            else if (lhsParticipants.Count == 0)
            {
                IList<object> resultList = new List<object>();

                for (var i = 0; i < rhsParticipants.Count; i++)
                {
                    resultList.Add(resultSelector(lhsResult, rhsParticipants[i]));
                }

                result = resultList.ToArray();
            }
            else if (rhsParticipants.Count == 0)
            {
                IList<object> resultList = new List<object>();

                for (var i = 0; i < lhsParticipants.Count; i++)
                {
                    resultList.Add(resultSelector(lhsParticipants[i], rhsResult));
                }

                result = resultList.ToArray();
            }

            return result;
        }
    }
}
