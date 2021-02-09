using System;
using System.Security;

namespace Scryber.Secure.Options
{
    public class SecurityOptionsPasswordProvider : IPDFSecurePasswordProvider
    {

        private SecureString configowner;
        private SecureString configuser;
        private PermissionFlags restrictions;
        private bool allowoverride;

        public SecurityOptionsPasswordProvider(PasswordPathOption options)
        {
            this.configowner = GetSecureString(options.OwnerPassword);
            this.configuser = GetSecureString(options.UserPassword);
            this.restrictions = options.Permissions;
            this.allowoverride = options.AllowOverrides;
        }


        protected SecureString GetSecureString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            SecureString all = new SecureString();
            foreach (char c in value)
            {
                all.AppendChar(c);
            }

            return all;
        }

        public bool IsSecure(string documentpath, out IDocumentPasswordSettings settings)
        {
            settings = new SecureDocumentPasswordSettings(documentpath, configowner, configuser, restrictions, allowoverride, SecurityType.Standard128Bit);
            return true;
        }
    }

}
