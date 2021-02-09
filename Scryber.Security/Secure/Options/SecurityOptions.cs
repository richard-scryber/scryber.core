using System;
using System.Runtime.CompilerServices;
using Scryber.Options;

namespace Scryber.Secure.Options
{
    public class SecurityOptions
    {

        public const string SecureOptionSection = ScryberOptions.ScryberSectionStub + "Security";

        public string DefaultOwnerPassword { get; set; }

        public PasswordPathOption PasswordPaths { get; set; }

        public SecurityOptions()
        {
        }


        public static bool TryGetPasswordSettings(string path, out IDocumentPasswordSettings settings)
        {
            var opts = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();

            settings = null;
            return false;
        }
    }


    public class PasswordPathOption
    {

        public string MatchPattern { get; set; }

        public PermissionFlags Permissions { get; set; }

        public string OwnerPassword { get; set; }

        public string UserPassword { get; set; }

        public string ProviderType { get; set; }

        public bool AllowOverrides { get; set; }

        public PasswordPathOption()
        {
            Permissions = 0;
            AllowOverrides = true;
        }


        private System.Text.RegularExpressions.Regex _expr;
        private IPDFSecurePasswordProvider _provider;

        public bool IsMatch(string path)
        {
            if(null == _expr)
            {
                if (string.IsNullOrEmpty(this.MatchPattern))
                    throw new NullReferenceException("The match pattern is null for this password path");

                _expr = new System.Text.RegularExpressions.Regex(this.MatchPattern);
            }
            return _expr.IsMatch(path);
        }

        public IPDFSecurePasswordProvider GetPasswordProvider(string defaultOwner)
        {
            if (null == _provider)
            {
                if (string.IsNullOrEmpty(this.ProviderType) == false)
                {
                    var type = Type.GetType(this.ProviderType, true);
                    var prov = Activator.CreateInstance(type, this);
                    if (prov is IPDFSecurePasswordProvider)
                        _provider = (IPDFSecurePasswordProvider)prov;
                    else
                        throw new InvalidCastException("Could not convert match pattern " + this.MatchPattern + " provider to an IPDFSecurePasswordProvider");
                }
                else
                    _provider = new SecurityOptionsPasswordProvider(this);
            }
            return _provider;
        }
    }
}
