using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Scryber.Secure.Configuration
{
    internal static class SecurityConfiguration
    {
        private const string SecuritySection = Scryber.Configuration.ScryberConfiguration.ScryberConfigGroupKey + "/secure";

        internal static bool TryGetPasswordSettings(string fullpath, out PDFDocumentPasswordSettings settings)
        {
            settings = null;
            SecurityConfigurationSection section = ConfigurationManager.GetSection(SecuritySection) as SecurityConfigurationSection;
            
            IPDFSecurePasswordProvider provider;
            if (null != section && section.TryGetProviderForPath(fullpath, out provider))
            {
                if (provider.IsSecure(fullpath, out settings))
                    return null != settings;
            }
            return false;
        }
    }
}
