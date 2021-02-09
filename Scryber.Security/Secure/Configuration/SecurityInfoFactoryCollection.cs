using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Scryber.Configuration
{
    public class SecurityInfoFactoryCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new SecurityFactoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            SecurityFactoryElement fact = (SecurityFactoryElement)element;
            string val = GetSecurityFactoryKey(fact.Version, fact.Revision);
            return val;
        }

        public static string GetSecurityFactoryKey(int version, int revision)
        {
            string val = version.ToString() + ":" + revision.ToString();
            return val;
        }
    }
}
