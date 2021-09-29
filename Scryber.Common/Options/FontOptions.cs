using System;

namespace Scryber.Options
{
    public class FontOptions
    {

        public const string FontsSection = ScryberOptions.ScryberSectionStub + "Fonts";

        public const bool DefaultUseSystemFonts = true;
        public const string DefaultFontName = "Sans-Serif";
        public const bool DefaultUseFontSubstitution = true;

        public string DefaultDirectory { get; set; }

        public bool UseSystemFonts { get; set; }

        public bool FontSubstitution { get; set; }

        public string DefaultFont { get; set; }

        public FontRegistrationOption[] Register { get; set; }

        public FontOptions()
        {
            DefaultDirectory = string.Empty;
            UseSystemFonts = DefaultUseSystemFonts;
            FontSubstitution = DefaultUseFontSubstitution;
            DefaultFont = DefaultFontName;
        }
    }



    public class FontRegistrationOption
    {
        public string Family { get; set; }

        public string Style { get; set; }

        public int Weight { get; set; }

        public string File { get; set; }
    }



}
