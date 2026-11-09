using System;
using System.Collections.Generic;

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

        public List<FontRegistrationOption> Register { get; set; }

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
        
        public string Resource { get; set; }
        
        public  FontRegistrationOption()
        {}

        public FontRegistrationOption(string family, string style, int weight, string file, string resource)
        {
            if(string.IsNullOrWhiteSpace(family))
                throw new ArgumentNullException(nameof(family));
            if(string.IsNullOrWhiteSpace(style))
                throw new ArgumentNullException(nameof(style));
            if(weight < 100 || weight > 900)
                throw new ArgumentOutOfRangeException(nameof(weight));

            Family = family;
            Style = style;
            Weight = weight;
            
            if(string.IsNullOrEmpty(file) && string.IsNullOrEmpty(resource))
                throw new ArgumentException($"At least one of {nameof(file)} or {nameof(resource)} is required. Both cannot be null or empty");
            
            File = file;
            Resource = resource;
        }
    }



}
