using System;
using System.Collections.Generic;

namespace Scryber.Expressive
{
	public delegate object ExpressionResultSelector(object left, object right, IDictionary<string, object> variables);
}

