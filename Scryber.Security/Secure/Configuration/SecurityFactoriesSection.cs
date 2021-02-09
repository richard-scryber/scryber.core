using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Scryber.Configuration
{
    
    public class SecurityFactoriesSection : System.Configuration.ConfigurationElement
    {
        public const int NullVersionOrRevision = -1;

        private const string DefaultVersionKey = "default-version";

        /// <summary>
        /// Gets or sets the default encryption version for all secure 
        /// </summary>
        [ConfigurationProperty(DefaultVersionKey,IsRequired=false)]
        public int DefaultVersion
        {
            get
            {
                object val = this[DefaultVersionKey];
                if (null == val || !(val is int))
                    return NullVersionOrRevision;
                else
                    return (int)val;
            }
            set
            {
                this[DefaultVersionKey] = value;
            }
        }

        private const string DefaultRevisionKey = "default-revision";

        public int DefaultRevision
        {
            get
            {
                object val = this[DefaultRevisionKey];
                if (null == val || !(val is int))
                    return NullVersionOrRevision;
                else
                    return (int)val;
            }
            set
            {
                this[DefaultRevisionKey] = value;
            }

        }
    }
}
