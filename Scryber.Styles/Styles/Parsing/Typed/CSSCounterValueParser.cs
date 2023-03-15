using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSCounterValueParser : CSSStyleValueParser
    {
        public StyleKey<CounterStyleValue> Key { get; set; }

        public int DefaultValue { get; set; }

        public CSSCounterValueParser(string cssName, StyleKey<CounterStyleValue> valueKey, int defaultValue)
            : base(cssName)
        {
            this.Key = valueKey;
            this.DefaultValue = defaultValue;
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            bool bound = false;
            string full = "";
            while (reader.ReadNextValue())
            {
                string val = reader.CurrentTextValue;
                if (IsExpression(val))
                {
                    if (!string.IsNullOrEmpty(full))
                        throw new ArgumentOutOfRangeException("Cannot bind individual counters within multiple operations. Consider using a concat expression to build the string");

                    result = AttachExpressionBindingHandler(onStyle, this.Key, val, DoConvertValue);
                    bound = true;
                }
                else
                {
                    if (bound)
                        throw new ArgumentOutOfRangeException("Cannot bind individual counters within multiple operations. Consider using a concat expression to build the string");

                    full += " " + val;
                }


            }

            CounterStyleValue value;

            if (TryParseCounterValue(full, out value))
            {
                var curr = onStyle.GetValue(this.Key, null);
                if (curr == null)
                    onStyle.SetValue(this.Key, value);
                else
                    curr.Append(value);

                result = true;
            }
            else
            {
                result = false;
            }


            return result;

        }

        protected bool DoConvertValue(StyleBase style, object value, out CounterStyleValue converted)
        {
            if (null == value)
            {
                converted = null;
                return false;
            }
            else
            {
                converted = CounterStyleValue.Parse(value.ToString());
                return null != converted;
            }
        }

        


        public bool TryParseCounterValue(string value, out CounterStyleValue parsed)
        {
            bool result;
            if (value == "none")
            {
                parsed = new CounterStyleValue("none");
                result = true;
            }
            else
            {
                parsed = CounterStyleValue.Parse(value, this.DefaultValue);
                result = null != parsed;
            }

            return result;
        }
    }

    public class CSSCounterResetParser : CSSCounterValueParser
    {

        public CSSCounterResetParser()
            : base(CSSStyleItems.CounterReset, StyleKeys.CounterResetKey, 0)
        {
        }
    }

    public class CSSCounterIncrementParser : CSSCounterValueParser
    {

        public CSSCounterIncrementParser()
            : base(CSSStyleItems.CounterIncrement, StyleKeys.CounterIncrementKey, 1)
        {
        }
    }
}
