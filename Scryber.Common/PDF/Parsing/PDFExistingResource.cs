using Scryber.PDF.Native;

namespace Scryber.PDF.Parsing
{
    public class PDFExistingResource : Scryber.PDF.Resources.PDFResource
    {
        private string _type;
        private IPDFFileObject _reference;
        private string _fullkey;

        public override string ResourceType
        {
            get { return _type; }
        }

        public override string ResourceKey
        {
            get { return _fullkey; }
        }


        public PDFExistingResource(string type, string key, string name, IPDFFileObject data)
            : base((ObjectType)"rExt")
        {
            _type = type;
            _reference = data;
            _fullkey = key;
            this.Name = (PDFName)name;
        }

        
        protected override PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer)
        {
            if (_reference is PDFObjectRef)
                return (PDFObjectRef)_reference;
            else
            {
                PDFObjectRef external = writer.BeginObject();
                _reference.WriteData(writer);
                writer.EndObject();
                _reference = external;
                return (PDFObjectRef)_reference;
            }
        }
    }
}