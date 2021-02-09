using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Scryber.Configuration;

namespace Scryber.Secure.Configuration
{
    /// <summary>
    /// Represetns the configuration element 'Password' in the Security element.
    /// </summary>
    /// <remarks> This class can return instances of IPDFSecurePasswordProvider and use the 
    /// IsMatch method to see if it is appropriate to use the password or password provider in this element</remarks>
    public class PasswordPathElement : ConfigurationElement
    {
        private const string PathPatternKey = "match-path";
        private const string DefaultUserPasswordKey = "user";
        private const string ProviderTypeKey = "provider-type";
        private const string AllowOverridesKey = "overridable";
        private const string DocumentRestrictionsKey = "permissions";
        private const string DefaultOwnerPasswordKey = "owner";

        //
        // ctor
        //

        public PasswordPathElement()
            : base()
        {
        }

        // 
        // Properties
        //

        #region public string PathPattern {get;set;}

        /// <summary>
        /// Gets or sets the pattern to match
        /// </summary>
        [ConfigurationProperty(PathPatternKey,IsKey=true,IsRequired=true)]
        public string MatchPathPattern
        {
            get { return this[PathPatternKey] as string; }
            set { this[PathPatternKey] = value; }
        }

        #endregion

        #region public int DocumentRestrictions {get;set;}

        /// <summary>
        /// Gets or sets the restrictions that will be applied to the document
        /// </summary>
        [ConfigurationProperty(DocumentRestrictionsKey,IsRequired=false)]
        public int DocumentRestrictions
        {
            get
            {
                object value = this[DocumentRestrictionsKey];
                if (null == value)
                    return 0;
                else
                    return (int)value;
            }
            set
            {
                this[DocumentRestrictionsKey] = value;
            }
        }

        #endregion

        #region public string DefaultOwnerPassword {get;set;}

        /// <summary>
        /// The default owner password for documents output with this security configuration
        /// </summary>
        [ConfigurationProperty(DefaultOwnerPasswordKey, IsRequired=false)]
        public string DefaultOwnerPassword
        {
            get { return this[DefaultOwnerPasswordKey] as string; }
            set { this[DefaultOwnerPasswordKey] = value; }

        }

        #endregion

        #region public string DefaultUserPassword

        /// <summary>
        /// The default user password for documents output with this security configuration
        /// </summary>
        [ConfigurationProperty(DefaultUserPasswordKey, IsRequired=false)]
        public string DefaultUserPassword
        {
            get { return this[DefaultUserPasswordKey] as string; }
            set { this[DefaultUserPasswordKey] = value; }
        }

        #endregion

        #region public string ProviderType

        /// <summary>
        /// Gets or sets the provider type. Providers must implement the IPDFSecurePasswordProvider
        /// </summary>
        [ConfigurationProperty(ProviderTypeKey,IsRequired=false)]
        public string ProviderType
        {
            get { return this[ProviderTypeKey] as string; }
            set { this[ProviderTypeKey] = value; }
        }

        #endregion

        #region public bool AllowOverrides

        /// <summary>
        /// If true then the document can apply its own restrictions on top of the configured set.
        /// </summary>
        [ConfigurationProperty(AllowOverridesKey,IsRequired=false, DefaultValue=true)]
        public bool AllowOverrides
        {
            get
            {
                object val = this[AllowOverridesKey];
                if (null == val)
                    return true;
                else
                    return (bool)val;
            }
            set { this[AllowOverridesKey] = value; }
        }

        #endregion


        //
        // methods to check and use the configuration data
        //


        #region static vars

        /// <summary>
        /// Lock object for thread safety
        /// </summary>
        static object _lock = new object();

        #endregion

        #region ivars

        /// <summary>
        /// Holds the cached regular expression to check if a path is a match
        /// </summary>
        System.Text.RegularExpressions.Regex _expr;
        
        /// <summary>
        /// Holds an instance of the IPasswordProvider
        /// </summary>
        IPDFSecurePasswordProvider _provider;

        #endregion


        #region internal bool IsMatch(string path)

        /// <summary>
        /// Returns true if this PasswordPathElement is a match (RegularExpression for path returns true).
        /// </summary>
        /// <param name="path">The full path to the source document</param>
        /// <returns></returns>
        internal bool IsMatch(string path)
        {
            if(null == _expr)
            {
                string tomatch = this.MatchPathPattern;
                if(string.IsNullOrEmpty(tomatch))
                    throw new ConfigurationErrorsException(Errors.PathIsRequiredInConfigSection);
                _expr = new System.Text.RegularExpressions.Regex(tomatch);
            }
            return _expr.IsMatch(path);
        }

        #endregion

        #region internal IPDFSecurePasswordProvider GetPasswordProvider()

        /// <summary>
        /// Returns the IPDFSecurePasswordProvider for this element
        /// </summary>
        /// <returns></returns>
        internal IPDFSecurePasswordProvider GetPasswordProvider()
        {
            if (null == _provider)
            {
                string typename = this.ProviderType;
                if (string.IsNullOrEmpty(typename))
                    _provider = new Scryber.Secure.Configuration.ConfigPasswordProvider(this);
                else
                    _provider = GetPasswordProviderFromName(typename);
            }
            return _provider;
        }

        #endregion

        #region private IPDFSecurePasswordProvider GetPasswordProviderFromName(string typename)

        /// <summary>
        /// Instantiates 
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        private IPDFSecurePasswordProvider GetPasswordProviderFromName(string typename)
        {
            lock (_lock)
            {
                IPDFSecurePasswordProvider factory;
                Type type = Support.GetTypeFromName(typename);
                object instance = Activator.CreateInstance(type);
                if (instance is IPDFSecurePasswordProvider)
                    factory = (IPDFSecurePasswordProvider)instance;
                else
                {
                    string msg = string.Format("", type, typeof(IPDFSecurePasswordProvider).Name);
                    throw new ConfigurationErrorsException(msg);
                }

                return factory;
            }
        }

        #endregion

    }


    /// <summary>
    /// A collection of PasswordPathElements that will be enumerated over to identify the first matching path (if any)
    /// </summary>
    public class PasswordPathElementCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new PasswordPathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PasswordPathElement)element).MatchPathPattern;
        }

    }
}
