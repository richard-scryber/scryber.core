using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Scryber.Imaging;

namespace Scryber.Options
{
    public static class ImageOptionExtensions
    {

        public static ImageFactoryList GetConfiguredFactories(this ImagingOptions options)
        {
            ImageDataFactoryOption[] configured = null;


            var standard = GetStandardFactories();
            var list = new ImageFactoryList();
            if (null != options && null != options.Factories && options.Factories.Length > 0)
            {
                configured = options.Factories;
                foreach (var configFactory in configured)
                {
                    var instance = configFactory.GetInstance() as IPDFImageDataFactory;
                    if (null == instance)
                        throw new InvalidCastException(
                            "The configured image data factory entry '" + (configFactory.Name ?? "UNNAMED") +
                            "' does not implement the IImageDataFactory interface");

                    
                    if(instance is ImageFactoryBase ifb)
                        list.Add(ifb);
                    else
                    {
                        //Wrap the IPDFImageDataFactory in a custom class that will handle the remote requests.
                        
                        var factory = new ImageFactoryCustom(new Regex(configFactory.Match), configFactory.Name,
                            instance.ShouldCache, instance);

                        list.Add(factory);
                    }
                }
            }
            
            //Add the standard factories after any configured factories
            list.AddRange(standard);

            
            return list;
        }

        private static readonly ImageFactoryBase[] Standards = new ImageFactoryBase[]
        {
            new ImageFactoryGif(),
            new ImageFactoryPng(),
            new ImageFactoryTiff(),
            new ImageFactoryJpeg(),
            new ImageFactoryPngDataUrl(),
            new ImageFactoryJpegDataUrl(),
            new ImageFactoryGifDataUrl()
        };
        
        private static IEnumerable<ImageFactoryBase> GetStandardFactories()
        {
            return Standards;
        }
    }
}