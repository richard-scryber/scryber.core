using System;
using Scryber.Drawing;

namespace Scryber.Core.UnitTests.Mocks
{
    public class MockParameter
    {
        public string Title { get; set; }

        public bool BoldTitle { get; set; }

        public PDFColor Background { get; set; }

        public MockParameter()
        {
        }
    }


    public class MockSubParameter : MockParameter
    {

        public PDFUnit Size { get; set; }


        public MockSubParameter()
        {
            this.Size = (PDFUnit)30;
            this.BoldTitle = true;
            this.Title = "Mock object title";
            this.Background = new PDFColor(1, 0, 0);
        }
    }
}
