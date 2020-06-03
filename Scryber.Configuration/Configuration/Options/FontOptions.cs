using System;

namespace Scryber.Configuration.Options
{
    public class FontOptions
    {
        public string DefaultDirectory { get; set; }

        public bool UseSystemFonts { get; set; }

        public bool FontSubstitution { get; set; }

        public string DefaultFont { get; set; }

        public FontRegistrationOption[] RegisteredFonts { get; set; }

        public FontOptions()
        {
            DefaultDirectory = string.Empty;
            UseSystemFonts = true;
            FontSubstitution = false;
            DefaultFont = "Arial";
        }
    }


    public class FontRegistrationOption
    {
        public string FamilyName { get; set; }

        public string FontStyle { get; set; }

        public string FontFile { get; set; }
    }



}
