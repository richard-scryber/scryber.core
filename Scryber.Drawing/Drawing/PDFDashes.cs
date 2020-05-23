using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Scryber.Drawing
{
    /// <summary>
    /// A static class for the named dashes
    /// </summary>
    public static class PDFDashes
    {

        [XmlAttribute("dot")]
        public static readonly PDFDash Dot = new PDFDash(new int[] { 1, 1 }, 0);

        [XmlAttribute("sparse-dot")]
        public static readonly PDFDash SparseDot = new PDFDash(new int[] { 1, 4 }, 0);

        [XmlAttribute("dash")]
        public static readonly PDFDash Dash = new PDFDash(new int[] { 4, 3 }, 0);

        [XmlAttribute("long-dash")]
        public static readonly PDFDash LongDash = new PDFDash(new int[] { 9, 3 }, 0);

        [XmlAttribute("long-dash-dot")]
        public static readonly PDFDash LongDashDot = new PDFDash(new int[] { 9, 3, 1, 3 }, 0);

        [XmlAttribute("long-dash-dot-dot")]
        public static readonly PDFDash LongDashDotDot = new PDFDash(new int[] { 9, 3, 1, 3, 1, 3 }, 0);



        private static Dictionary<string, PDFDash> _named;


        static PDFDashes()
        {
            _named = new Dictionary<string, PDFDash>(StringComparer.OrdinalIgnoreCase);
            LoadDashesFromFields(_named);
        }

        private static void LoadDashesFromFields(Dictionary<string,PDFDash> hash)
        {
            System.Reflection.FieldInfo[] fields = typeof(PDFDashes).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            foreach (System.Reflection.FieldInfo fi in fields)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(XmlAttributeAttribute),false);
                if(null != attrs && attrs.Length > 0)
                {
                    XmlAttributeAttribute attr = (XmlAttributeAttribute)attrs[0];
                    string name = attr.AttributeName;
                    if (string.IsNullOrEmpty(name))
                        name = fi.Name;
                    object dash = fi.GetValue(null);
                    if(null != dash && dash is PDFDash)
                    {
                        hash.Add(name, (PDFDash)dash);
                    }
                }
            }
        }

        public static bool TryGetNamedDash(string name, out PDFDash found)
        {
            return _named.TryGetValue(name, out found);
        }
    }
}
