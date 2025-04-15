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
            MinimumScaleReduction = 0.02;
        }
    }

    public class ImageDataFactoryOption
    {

        public string Name { get; set; }

        public string Match { get; set; }

        public string FactoryType { get; set; }

        public string FactoryAssembly { get; set; }

        private object _factory;
        
        public object GetInstance()
        {
            if (null == _factory)
            {
                // Don't care that this is not thread safe, as it may be used twice or more and still assigned.
                var instance = Utilities.TypeHelper.GetInstance<object>(this.FactoryType, this.FactoryAssembly, true);
                _factory = instance;
            }

            return _factory;
        }

    }
}
