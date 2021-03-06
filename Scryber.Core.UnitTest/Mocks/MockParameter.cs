﻿using System;
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
        public PDFUnit SizeField;

        


        public MockSubParameter()
        {
            this.SizeField = (PDFUnit)30;
            this.BoldTitle = true;
            this.Title = "Mock object title";
            this.Background = new PDFColor(1, 0, 0);
        }
    }

    public class MockOtherParameter
    {
        public string Title { get; set; }

        public bool BoldTitle { get; set; }

        public PDFColor Background { get; set; }

        public MockOtherParameter()
        {
        }
    }

}
