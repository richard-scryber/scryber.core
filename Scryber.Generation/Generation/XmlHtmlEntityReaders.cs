using System;
using System.Xml;
using System.Collections.Generic;

namespace Scryber.Generation
{
    public class XmlHtmlEntityReader : XmlTextReader
    {
        private string _nextEntity;
        private IDictionary<string, char> _entities;

        protected IDictionary<string, char> Entities
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
            _entities = Html.HtmlEntities.DefaultKnownHTMLAndXMLEntities;
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

        

        
    }
}
