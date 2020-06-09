using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Scryber.Configuration
{
    [Obsolete("Use the IScryberConfigurationSerive, from the ServiceProvider", true)]
    public class ParserFactoryElementCollection : ConfigurationElementCollection
    {

        protected override object GetElementKey(ConfigurationElement element)
        {
            ParserFactoryElement fact = (ParserFactoryElement)element;
            return fact.Name;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ParserFactoryElement();
        }
    }
}
