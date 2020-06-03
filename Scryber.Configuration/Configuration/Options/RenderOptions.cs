using System;
namespace Scryber.Configuration.Options
{
    public class RenderOptions
    {

        public OutputCompliance Compliance { get; set; }

        public OutputStringType StringType { get; set; }

        public ComponentNameOutput NameOutput { get; set; }

        public string PDFVersion { get; set; }

        public OutputCompressionType Compression { get; set; }

        public RenderOptions()
        {
            Compression = OutputCompressionType.FlateDecode;
            PDFVersion = "1.5";
            Compliance = OutputCompliance.None;
            StringType = OutputStringType.Hex;
            NameOutput = ComponentNameOutput.ExplicitOnly;
        }
    }
}
