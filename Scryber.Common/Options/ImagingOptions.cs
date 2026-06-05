using System;
using System.Collections.Generic;
namespace Scryber.Options
{
    public class ImagingOptions
    {

        public const string ImagingSection = ScryberOptions.ScryberSectionStub + "Imaging";

        public bool AllowMissingImages { get; set; }

        public double MinimumScaleReduction { get; set; }

        public int ImageCacheDuration { get; set; }

        public List<ImageDataFactoryOption> Factories { get; set; }

        public ImagingOptions()
        {
            AllowMissingImages = true;
            ImageCacheDuration = -1;
            MinimumScaleReduction = 0.249;
        }
    }

    public class ImageDataFactoryOption
    {

        public string Name { get; set; }

        public string Match { get; set; }

        public string FactoryType { get; set; }

        public string FactoryAssembly { get; set; }

        private object _factory;
        
        public ImageDataFactoryOption(){}

        public ImageDataFactoryOption(string name, string match, string factoryType, string factoryAssembly)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrEmpty(match))
                throw new ArgumentNullException(nameof(match));
            if(string.IsNullOrEmpty(factoryType))
                throw new ArgumentNullException(nameof(factoryType));
            if(string.IsNullOrEmpty(factoryAssembly))
                throw new ArgumentNullException(nameof(factoryAssembly));
            
            Name = name;
            Match = match;
            FactoryType = factoryType;
            FactoryAssembly = factoryAssembly;
        }

        public ImageDataFactoryOption(string name, string match, Type factoryType)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrEmpty(match))
                throw new ArgumentNullException(nameof(match));
            if(null == factoryType)
                throw new ArgumentNullException(nameof(factoryType));
            
            Name = name;
            Match = match;
            FactoryType = factoryType.FullName;
            FactoryAssembly = factoryType.Assembly.FullName;
            
            try
            {
                _factory = Utilities.TypeHelper.GetInstance<object>(factoryType);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not create an image factory instance from the type " + factoryType.FullName + ". The type should have a parameterless constructor, and implement the IPDFImageDataFactory interface.", ex);
            }
        }
        
        
        public virtual object GetInstance()
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
