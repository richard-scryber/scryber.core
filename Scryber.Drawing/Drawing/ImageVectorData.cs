namespace Scryber.Drawing
{
    public abstract class ImageVectorData : ImageData
    {
        
        public ImageVectorData(string source) : this(ObjectTypes.ImageData, source) { }

        public ImageVectorData(ObjectType type, string source) : base(type, source)
        {
            
        }
        
    }
}