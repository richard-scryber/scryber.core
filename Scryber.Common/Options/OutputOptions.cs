using System;

namespace Scryber.Options
{
    public class OutputOptions
    {

        public const string OutputSection = ScryberOptions.ScryberSectionStub + "Output";

        public OutputCompliance Compliance { get; set; }

        public OutputStringType StringType { get; set; }

        public ComponentNameOutput NameOutput { get; set; }

        public string PDFVersion { get; set; }

        public OutputCompressionType Compression { get; set; }

        public OutputOptions()
        {
            Compression = OutputCompressionType.FlateDecode;
            PDFVersion = "1.5";
            Compliance = OutputCompliance.None;
            StringType = OutputStringType.Hex;
            NameOutput = ComponentNameOutput.ExplicitOnly;
        }
    }
}
