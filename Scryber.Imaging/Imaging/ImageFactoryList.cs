using System;
using System.Collections.Generic;


namespace Scryber.Imaging
{
    /// <summary>
    /// A list of image factories
    /// </summary>
    public class ImageFactoryList : List<ImageFactoryBase>
    {
        public ImageFactoryList(): base()
        {}
        
        public ImageFactoryList(int capacity) : base(capacity)
        {}
        
        public ImageFactoryList(IEnumerable<ImageFactoryBase> factories): base(factories)
        {}


        public bool TryGetMatch(string path, out ImageFactoryBase factory)
        {
            if (path.StartsWith("data:"))
            {
                //Do Nothing as we are a data URL.
            }
            else if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
            {
                var param = path.IndexOf('?');

                if (param > 0)
                {
                    path = path.Substring(0, param);
                }
                
                var hash = path.IndexOf('#');
                if (hash > 0)
                {
                    path = path.Substring(0, hash + 1);
                }
            }

            foreach (var match in this)
            {
                if (match.IsMatch(path))
                {
                    factory = match;
                    return true;
                }
            }

            factory = null;
            return false;
        }
        
        
    }
}