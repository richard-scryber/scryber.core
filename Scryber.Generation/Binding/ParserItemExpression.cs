using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scryber.Binding
{
    public abstract class ParserItemExpression
    {

        protected const string CurrentDataContextName = "{current}";

        public ParserItemExpression Next { get; set; }

        public void AppendExpression(ParserItemExpression expr)
        {
            if (null == this.Next)
                this.Next = expr;
            else
                this.Next.AppendExpression(expr);
        }

        public object GetValue(object parent, DataContext context)
        {
            if (null == parent)
                return null;

            object value = DoGetMyValue(parent, context);
            if (null != Next)
                value = Next.GetValue(value, context);
            return value;
        }

        protected abstract object DoGetMyValue(object parent, DataContext context);
    }

}
