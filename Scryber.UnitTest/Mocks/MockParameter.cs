using System;
using Scryber.Drawing;

namespace Scryber.Core.UnitTests.Mocks
{
    public class MockParameter
    {
        public string Title { get; set; }

        public bool BoldTitle { get; set; }

        public Color Background { get; set; }

        public MockParameter()
        {
        }
    }


    public class MockSubParameter : MockParameter
    {
        public Unit SizeField;

        


        public MockSubParameter()
        {
            this.SizeField = (Unit)30;
            this.BoldTitle = true;
            this.Title = "Mock object title";
            this.Background = new Color(1, 0, 0);
        }
    }

    public class MockOtherParameter
    {
        public string Title { get; set; }

        public bool BoldTitle { get; set; }

        public Color Background { get; set; }

        public MockOtherParameter()
        {
        }
    }

}
