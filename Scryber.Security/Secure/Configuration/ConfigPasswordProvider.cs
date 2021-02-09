using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace Scryber.Secure.Configuration
{
    internal class ConfigPasswordProvider : IPDFSecurePasswordProvider
    {
        private SecureString configowner;
        private SecureString configuser;
        private PermissionFlags restrictions;
        private bool allowoverride;
        private SecurityType securityType;

        public ConfigPasswordProvider(PasswordPathElement element)
        {
            configowner = GetSecureString(element.DefaultOwnerPassword);
            configuser = GetSecureString(element.DefaultUserPassword);
            restrictions = (PermissionFlags)element.DocumentRestrictions;
            allowoverride = element.AllowOverrides;
            securityType = SecurityType.Standard128Bit;
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
            settings = new PDFDocumentPasswordSettings(documentpath, configowner, configuser, restrictions, allowoverride);
            return true;
        }
    }
}
