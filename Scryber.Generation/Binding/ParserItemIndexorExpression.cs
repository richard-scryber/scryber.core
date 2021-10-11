using System;
using System.Collections;

namespace Scryber.Binding
{
    public class ParserItemIndexorExpression : ParserItemExpression
    {
        public int IndexValue { get; private set; }

        public ParserItemIndexorExpression(int index)
        {
            this.IndexValue = index;
        }

        protected override object DoGetMyValue(object parent, DataContext context)
        {
            if (parent is IList)
                return (parent as IList)[IndexValue];
            else
            {
                System.Reflection.PropertyInfo pi = parent.GetType().GetProperty("Item", new Type[] { typeof(int) });
                if (null == pi)
                    throw new ArgumentOutOfRangeException("Item[int]");

                return pi.GetValue(parent, new object[] { IndexValue });
            }
        }
    }
}
