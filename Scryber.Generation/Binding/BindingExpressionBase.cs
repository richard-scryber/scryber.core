using System;
using System.Reflection;
namespace Scryber.Binding
{
    public class BindingExpressionBase
    {

        private bool? _parsable;
        private MethodInfo _parseMethod;

        public BindingExpressionBase()
        {
        }


        protected virtual void SetPropertyValue(object owner, object value, string expression, PropertyInfo toProperty, ContextBase context)
        {
            try
            {
                if (null != value)
                {
                    if (context.ShouldLogVerbose)
                        context.TraceLog.Add(TraceLevel.Verbose, "Item Binding", "Setting property '" + toProperty.Name + "' with the item binding expression '" + expression + "' to value '" + value.ToString() + "'");
                    MethodInfo parse;

                    if (toProperty.PropertyType == typeof(string))
                    {
                        value = value.ToString();
                    }
                    else if (toProperty.PropertyType == value.GetType())
                    {
                        //Do nothing
                    }
                    else if (this.IsParsable(toProperty, out parse) && ((value is string) || (value is IConvertible)))
                    {
                        value = parse.Invoke(null, new object[] { value.ToString() });
                    }
                    else if (value is IConvertible)
                    {
                        if (toProperty.PropertyType.IsEnum)
                        {
                            var enumType = toProperty.PropertyType;
                            
                            if (value is string strValue)
                            {
                                value = Enum.Parse(toProperty.PropertyType, strValue, true);
                            }
                            else 
                                value = (value as IConvertible).ToType(toProperty.PropertyType, null);
                        }
                        else
                            value = (value as IConvertible).ToType(toProperty.PropertyType, null);
                    }

                    toProperty.SetValue(owner, value, null);
                }
                else if (context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Item Binding", "NULL value returned for expression '" + expression + "' so not setting property '" + toProperty.Name + "'");
            }
            catch (Exception ex)
            {
                string id;
                if (owner is IComponent component)
                    id = component.ID;
                else
                    id = "";

                string message = "Cannot bind the values for '" + owner.ToString() + "' with id " + id;

                if (context.Conformance == ParserConformanceMode.Lax)
                    context.TraceLog.Add(TraceLevel.Error, "Data Binding", message, ex);
                else
                    throw new Scryber.PDFBindException(message, ex);
            }
        }

        private static Type[] _ParseParams = new Type[] { typeof(string) };

        private bool IsParsable(PropertyInfo property, out MethodInfo found)
        {
            if (!_parsable.HasValue)
            {
                _parsable = false;
                var type = property.PropertyType;
                var attr = type.GetCustomAttribute(typeof(PDFParsableValueAttribute), true);
                if (attr != null)
                {
                    var meth = type.GetMethod("Parse", _ParseParams);
                    if (null == meth)
                        throw new InvalidOperationException("The class " + type.FullName + " declares the PDFParsableComponentAttribute, but does not appear to have a Parse(string) static method");
                    _parseMethod = meth;
                    _parsable = true;
                }
            }

            found = _parseMethod;
            return _parsable.Value;
        }
    }
}
