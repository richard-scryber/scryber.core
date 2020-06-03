using System;
namespace Scryber.Configuration.Options
{
    public class ImagingOptions
    {

        public bool AllowMissingImages { get; set; }

        public int ImageCacheDuration { get; set; }

        public ImageDataFactoryOption[] ImageFactories { get; set; }

        public ImagingOptions()
        {
            AllowMissingImages = false;
            ImageCacheDuration = -1;
        }
    }

    public class ImageDataFactoryOption
    {
        public string Name { get; set; }

        public string Match { get; set; }

        public string FactoryType { get; set; }

    }
}
