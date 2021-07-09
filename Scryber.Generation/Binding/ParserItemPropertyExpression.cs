using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scryber.Binding
{
    public class ParserItemPropertyExpression : ParserItemExpression
    {
        public string PropertyOrFieldName { get; private set; }

        private MemberInfo _lastReflected;

        public ParserItemPropertyExpression(string name)
        {
            this.PropertyOrFieldName = name;
        }

        protected override object DoGetMyValue(object parent, PDFDataContext context)
        {
            var type = parent.GetType();

            PropertyInfo pi;
            FieldInfo fi;

            if (this.TryGetProperty(type, out pi))
                return pi.GetValue(parent, null);

            else if (this.TryGetField(type, out fi))
                return fi.GetValue(parent);

            else if (parent is System.Dynamic.ExpandoObject)
            {
                object found;
                IDictionary<string, object> expando = parent as System.Dynamic.ExpandoObject;
                if (expando.TryGetValue(this.PropertyOrFieldName, out found))
                    return found;
                else
                    return null; //As we are dynamic, let's be generous and not throw an error.
            }
            else if (parent is ICustomTypeDescriptor)
            {
                var prop = (parent as ICustomTypeDescriptor).GetProperties()[this.PropertyOrFieldName];
                if (null != prop)
                    return prop.GetValue(parent);
                else
                    throw new ArgumentOutOfRangeException(this.PropertyOrFieldName);
            }
            else
                throw new ArgumentOutOfRangeException(this.PropertyOrFieldName);
        }

        private bool TryGetProperty(Type fortype, out PropertyInfo found)
        {
            if (null != _lastReflected && _lastReflected.DeclaringType == fortype)
            {
                found = (PropertyInfo)this._lastReflected;
                return true;
            }
            else
            {
                found = fortype.GetProperty(this.PropertyOrFieldName);
                if (null == found)
                    return false;
                else
                {
                    _lastReflected = found;
                    return true;
                }
            }
        }

        private bool TryGetField(Type fortype, out FieldInfo found)
        {
            if (null != _lastReflected && _lastReflected.DeclaringType == fortype)
            {
                found = (FieldInfo)this._lastReflected;
                return true;
            }
            else
            {
                found = fortype.GetField(this.PropertyOrFieldName);
                if (null == found)
                    return false;
                else
                {
                    _lastReflected = found;
                    return true;
                }
            }
        }
    }
}
