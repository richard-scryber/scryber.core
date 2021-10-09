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
    public static class Dashes
    {

        [XmlAttribute("dot")]
        public static readonly Dash Dot = new Dash(new int[] { 1, 1 }, 0);

        [XmlAttribute("sparse-dot")]
        public static readonly Dash SparseDot = new Dash(new int[] { 1, 4 }, 0);

        [XmlAttribute("dash")]
        public static readonly Dash Dash = new Dash(new int[] { 4, 3 }, 0);

        [XmlAttribute("long-dash")]
        public static readonly Dash LongDash = new Dash(new int[] { 9, 3 }, 0);

        [XmlAttribute("long-dash-dot")]
        public static readonly Dash LongDashDot = new Dash(new int[] { 9, 3, 1, 3 }, 0);

        [XmlAttribute("long-dash-dot-dot")]
        public static readonly Dash LongDashDotDot = new Dash(new int[] { 9, 3, 1, 3, 1, 3 }, 0);



        private static Dictionary<string, Dash> _named;


        static Dashes()
        {
            _named = new Dictionary<string, Dash>(StringComparer.OrdinalIgnoreCase);
            LoadDashesFromFields(_named);
        }

        private static void LoadDashesFromFields(Dictionary<string,Dash> hash)
        {
            System.Reflection.FieldInfo[] fields = typeof(Dashes).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

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
                    if(null != dash && dash is Dash)
                    {
                        hash.Add(name, (Dash)dash);
                    }
                }
            }
        }

        public static bool TryGetNamedDash(string name, out Dash found)
        {
            return _named.TryGetValue(name, out found);
        }
    }
}
