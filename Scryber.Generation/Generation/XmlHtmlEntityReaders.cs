using System;
using System.Xml;
using System.Collections.Generic;

namespace Scryber.Generation
{
    public class XmlHtmlEntityReader : XmlTextReader
    {
        private string _nextEntity;
        private Dictionary<string, char> _entities;

        protected Dictionary<string, char> Entities
        {
            get { return _entities; }
        }


        public XmlHtmlEntityReader(System.IO.Stream stream) : base(stream)
        {
            this.AddDefaultEntities();
        }

        public XmlHtmlEntityReader(System.IO.TextReader reader) : base(reader)
        {
            this.AddDefaultEntities();
        }

        protected virtual void AddDefaultEntities()
        {
            _entities = InitKnownHTMLEntities();
        }

        public void AddEntity(string entity, char value)
        {
            _entities[entity] = value;
        }

        public override bool Read()
        {
            _nextEntity = null;
            bool result = base.Read();

            if(result && base.NodeType == XmlNodeType.EntityReference)
            {
                char found;
                if (Entities.TryGetValue(base.LocalName, out found))
                    _nextEntity = found.ToString();
            }

            return result;
        }

        public override XmlNodeType NodeType
        {
            get
            {
                if (_nextEntity != null)
                    return XmlNodeType.Text;
                else
                    return base.NodeType;
            }
        }

        public override string Value
        {
            get
            {
                if (_nextEntity != null)
                {
                    return _nextEntity;
                }
                else
                    return base.Value;
            }
        }

        

        public override void ResolveEntity()
        {
            char found;
            // if not found, return the string as is
            if (!_entities.TryGetValue(LocalName, out found))
            {
                _nextEntity = found.ToString();
            }
            
            // NOTE: we don't use base here. Depends on the scenario
        }

        

        

        public static Dictionary<string, char> DefaultKnownHTMLEntities = InitKnownHTMLEntities();


        public static Dictionary<string, char> InitKnownHTMLEntities()
        {
            Dictionary<string, char> known = new Dictionary<string, char>();
            known.Add("nbsp", ' ');
            //known.Add("lt", '<');
            //known.Add("gt", '>');
            //known.Add("amp", '&');
            known.Add("apos", '\'');
            known.Add("quot", '"');
            known.Add("euro", '€');
            known.Add("ldquo", '“');
            known.Add("rdquo", '”');
            known.Add("lsquo", '‘');
            known.Add("rsquo", '’');
            known.Add("iexcl", '¡');
            known.Add("cent", '¢');
            known.Add("pound", '£');
            known.Add("ndash", '-');
            known.Add("mdash", '—');
            known.Add("curren", '¤');
            known.Add("yen", '¥');
            known.Add("brvbar", '¦');
            known.Add("sect", '§');
            known.Add("uml", '¨');
            known.Add("copy", '©');
            known.Add("ordf", 'ª');
            known.Add("not", '¬');
            known.Add("reg", '®');
            known.Add("macr", '¯');
            known.Add("deg", '°');
            known.Add("dagger", '†');
            known.Add("Dagger", '‡');
            known.Add("plusmn", '±');
            known.Add("sup1", '¹');
            known.Add("sup2", '²');
            known.Add("sup3", '³');
            known.Add("acute", '´');
            known.Add("micro", 'µ');
            known.Add("para", '¶');
            known.Add("middot", '·');
            known.Add("cedil", '¸');
            known.Add("ordm", 'º');
            known.Add("laquo", '«');
            known.Add("frac14", '¼');
            known.Add("frac12", '½');
            known.Add("frac34", '¾');
            known.Add("iquest", '¿');
            known.Add("Agrave", 'À');
            known.Add("Aacute", 'Á');
            known.Add("Acirc", 'Â');
            known.Add("Atilde", 'Ã');
            known.Add("Auml", 'Ä');
            known.Add("Aring", 'Å');
            known.Add("AElig", 'Æ');
            known.Add("Ccedil", 'Ç');
            known.Add("Egrave", 'È');
            known.Add("Eacute", 'É');
            known.Add("Ecirc", 'Ê');
            known.Add("Euml", 'Ë');
            known.Add("Igrave", 'Ì');
            known.Add("Iacute", 'Í');
            known.Add("Icirc", 'Î');
            known.Add("Iuml", 'Ï');
            known.Add("ETH", 'Ð');
            known.Add("Ntilde", 'Ñ');
            known.Add("Ograve", 'Ò');
            known.Add("Oacute", 'Ó');
            known.Add("Ocirc", 'Ô');
            known.Add("Otilde", 'Õ');
            known.Add("Ouml", 'Ö');
            known.Add("times", '×');
            known.Add("Oslash", 'Ø');
            known.Add("Ugrave", 'Ù');
            known.Add("Uacute", 'Ú');
            known.Add("Ucirc", 'Û');
            known.Add("Uuml", 'Ü');
            known.Add("Yacute", 'Ý');
            known.Add("THORN", 'Þ');
            known.Add("szlig", 'ß');
            known.Add("agrave", 'à');
            known.Add("aacute", 'á');
            known.Add("acirc", 'â');
            known.Add("atilde", 'ã');
            known.Add("auml", 'ä');
            known.Add("aring", 'å');
            known.Add("aelig", 'æ');
            known.Add("ccedil", 'ç');
            known.Add("egrave", 'è');
            known.Add("eacute", 'é');
            known.Add("ecirc", 'ê');
            known.Add("euml", 'ë');
            known.Add("igrave", 'ì');
            known.Add("iacute", 'í');
            known.Add("icirc", 'î');
            known.Add("iuml", 'ï');
            known.Add("eth", 'ð');
            known.Add("ntilde", 'ñ');
            known.Add("ograve", 'ò');
            known.Add("oacute", 'ó');
            known.Add("ocirc", 'ô');
            known.Add("otilde", 'õ');
            known.Add("ouml", 'ö');
            known.Add("divide", '÷');
            known.Add("oslash", 'ø');
            known.Add("ugrave", 'ù');
            known.Add("uacute", 'ú');
            known.Add("ucirc", 'û');
            known.Add("uuml", 'ü');
            known.Add("yacute", 'ý');
            known.Add("thorn", 'þ');
            known.Add("raquo", '»');
            known.Add("hellip", '…');
            known.Add("trade", '™');
            known.Add("asymp", '≈');
            known.Add("ne", '≠');
            known.Add("le", '≤');
            known.Add("ge", '≥');

            return known;
        }
    }
}
