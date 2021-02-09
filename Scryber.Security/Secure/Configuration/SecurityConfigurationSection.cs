using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Scryber.Secure.Configuration
{
    /// <summary>
    /// Configuration section for the security options. 
    /// Contains a collection of password path configuration elements.
    /// </summary>
    public class SecurityConfigurationSection : ConfigurationSection
    {

        public SecurityConfigurationSection() : base() { }

        public const string PasswordsKey = "";

        #region public PasswordPathElementCollection Passwords {get; set;}

        /// <summary>
        /// Gets or sets the collection of Password entries.
        /// </summary>
        [ConfigurationProperty(PasswordsKey, IsDefaultCollection=true)]
        [ConfigurationCollection(typeof(PasswordPathElement),AddItemName="password")]
        public PasswordPathElementCollection Passwords
        {
            get { return this[PasswordsKey] as PasswordPathElementCollection; }
            set { this[PasswordsKey] = value; }
        }

        #endregion


        /// <summary>
        /// Returns true if there is any configured password security for the document based on the path, and sets the provider to the configured value.
        /// </summary>
        /// <param name="fullpath">The full path of the PDF document template that is being generated.</param>
        /// <param name="provider">Set to the configured password provider, if there is one.</param>
        /// <returns>false if no provider was found, otherwise true.</returns>
        public bool TryGetProviderForPath(string fullpath, out IPDFSecurePasswordProvider provider)
        {
            provider = null;
            PasswordPathElementCollection passes = this.Passwords;
            if (null == passes || passes.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (PasswordPathElement ele in passes)
                {
                    if (ele.IsMatch(fullpath))
                    {
                        provider = ele.GetPasswordProvider();
                        return null != provider;
                    }
                }
                //no matches - this is not an error condition.
                return false;
            }
        }
    }
}
