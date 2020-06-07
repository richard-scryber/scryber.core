using System;

namespace Scryber.Options
{
    public class FontOptions
    {

        public const string FontsSection = ScryberOptions.ScryberSectionStub + "Fonts";

        public string DefaultDirectory { get; set; }

        public bool UseSystemFonts { get; set; }

        public bool FontSubstitution { get; set; }

        public string DefaultFont { get; set; }

        public FontRegistrationOption[] Register { get; set; }

        public FontOptions()
        {
            DefaultDirectory = string.Empty;
            UseSystemFonts = true;
            FontSubstitution = false;
            DefaultFont = "Sans-Serif";
        }
    }


    public class FontRegistrationOption
    {
        public string Family { get; set; }

        public System.Drawing.FontStyle Style { get; set; }

        public string File { get; set; }
    }



}
