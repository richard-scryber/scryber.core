using System;
using System.Collections.Generic;

namespace Scryber.Binding
{
    public class ParserItemKeyedExpression : ParserItemExpression
    {
        public string KeyValue { get; private set; }

        public ParserItemKeyedExpression(string key)
        {
            this.KeyValue = key;
        }

        protected override object DoGetMyValue(object parent, PDFDataContext context)
        {
            System.Reflection.PropertyInfo pi = parent.GetType().GetProperty("Item", new Type[] { typeof(string) });
            if (null != pi)
                return pi.GetValue(parent, new object[] { KeyValue });
            else if (parent is IDictionary<string, object>)
            {
                object found;
                IDictionary<string, object> dict = parent as IDictionary<string, object>;
                if (dict.TryGetValue(KeyValue, out found))
                    return found;
                else
                    return null;
            }
            else
                throw new ArgumentOutOfRangeException("Item[string]", "Object does not support indexing with a string value or the the dictionary<string,object> interface");


        }
    }
}
