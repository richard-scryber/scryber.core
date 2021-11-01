using System;
namespace Scryber.PDF.Resources
{
    public class PDFFontInitException : ApplicationException
    {

        public PDFFontInitException(string message)
            : base(message)
        {

        }

        public PDFFontInitException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
