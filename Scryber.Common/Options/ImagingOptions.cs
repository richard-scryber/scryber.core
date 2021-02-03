using System;
namespace Scryber.Options
{
    public class ImagingOptions
    {

        public const string ImagingSection = ScryberOptions.ScryberSectionStub + "Imaging";

        public bool AllowMissingImages { get; set; }

        public double MinimumScaleReduction { get; set; }

        public int ImageCacheDuration { get; set; }

        public ImageDataFactoryOption[] Factories { get; set; }

        public ImagingOptions()
        {
            AllowMissingImages = true;
            ImageCacheDuration = -1;
            MinimumScaleReduction = 0.2;
        }
    }

    public class ImageDataFactoryOption
    {

        public string Name { get; set; }

        public string Match { get; set; }

        public string FactoryType { get; set; }

        public string FactoryAssembly { get; set; }

    }
}
