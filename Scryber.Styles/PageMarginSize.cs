using System.Security.AccessControl;
using Scryber.Drawing;

namespace Scryber
{
    /// <summary>
    /// Defines any explicit margins on a page
    /// </summary>
    public class PageMarginSize
    {
        public Unit? Top { get; set; }
        
        public Unit? Bottom { get; set; }
        
        public Unit? Left { get; set; }
        
        public Unit? Right { get; set; }

        public PageMarginSize()
        {
        }
    }
    
    /// <summary>
    /// Extends the PageMarginSize to add support for individual margins on the First, Left and Right margins.
    /// </summary>
    public class PageMarginHandedSize : PageMarginSize
    {
        public PageMarginSize PageLeftMargins { get; set; }
        
        public PageMarginSize PageRightMargins { get; set; }
    
        public PageMarginSize PageFirstMargins { get; set; }
    }
}