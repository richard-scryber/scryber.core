using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Scryber.Configuration
{
    /// <summary>
    /// A configuration element that when declared in the configuration file will attempt to 
    /// load a specific factory based on the requested version number and revision
    /// </summary>
    public class SecurityFactoryElement : ConfigurationElement
    {
        //
        // attributes
        //

        #region public int Version{get;set;} @version

        private const string VersionKey = "version";

        /// <summary>
        /// Gets or sets the version number that the defined security factory is associated 
        /// with
        /// </summary>
        [ConfigurationProperty(VersionKey,IsKey=false, IsRequired=true)]
        public int Version
        {
            get { return (int)this[VersionKey]; }
            set { this[VersionKey] = value; }
        }

        #endregion

        #region public int Revision {get;set;} @revision

        private const string RevisionKey = "revision";

        /// <summary>
        /// Gets or sets the revision number that the defined security factory is associated with to generate PDFSecurityInformation instances
        /// </summary>
        [ConfigurationProperty(RevisionKey, IsKey = false, IsRequired = true)]
        public int Revision
        {
            get { return (int)this[RevisionKey]; }
            set { this[RevisionKey] = value; }
        }

        #endregion

        #region public string FactoryType {get;set;} @factory-type


        private const string FactoryTypeKey = "factory-type";

        /// <summary>
        /// Gets or sets the full assembly qualifed name of the factory type that sould be used for the specified revision and version
        /// </summary>
        [ConfigurationProperty(FactoryTypeKey,IsKey=false,IsRequired=true)]
        public string FactoryType
        {
            get { return this[FactoryTypeKey] as string; }
            set { this[FactoryTypeKey] = value; }
        }


        #endregion

        //
        // type accessor
        //

        private Type _derived;
        private static object _lock = new object();

        /// <summary>
        /// Gets a Type reference to the declared factory type that should be used for this elements revision and version
        /// </summary>
        public Type FactoryDerivedType
        {
            get
            {
                if (null == _derived)
                {
                    if (string.IsNullOrEmpty(this.FactoryType))
                        throw new ConfigurationErrorsException("Required configuration attribute is not set on security factory element - factory-type");
                    lock (_lock)
                    {
                        Type t = Support.GetTypeFromName(this.FactoryType);
                        _derived = t;
                    }
                }
                return _derived;
               
            }
        }

    }
}
