using System;
namespace Scryber.Options
{
    public class ImagingOptions
    {

        public const string ImagingSection = ScryberOptions.ScryberSectionStub + "Imaging";

        public bool AllowMissingImages { get; set; }

        public int ImageCacheDuration { get; set; }

        public ImageDataFactoryOption[] Factories { get; set; }

        public ImagingOptions()
        {
            AllowMissingImages = false;
            ImageCacheDuration = -1;
        }
    }

    public class ImageDataFactoryOption
    {
        public string Match { get; set; }

        public string FactoryType { get; set; }

        public string FactoryAssembly { get; set; }

    }
}
