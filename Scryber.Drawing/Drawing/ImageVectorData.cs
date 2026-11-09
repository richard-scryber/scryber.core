namespace Scryber.Drawing
{
    /// <summary>
    /// Defines an image that contains vector graphics, rather than raster (pixel based).
    /// </summary>
    public abstract class ImageVectorData : ImageData
    {
        
        public ImageVectorData(string source) : this(ObjectTypes.ImageData, source) { }

        public ImageVectorData(ObjectType type, string source) : base(type, source, ImageType.Vector)
        {
            
        }
        
    }
}