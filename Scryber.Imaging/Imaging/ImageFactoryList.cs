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