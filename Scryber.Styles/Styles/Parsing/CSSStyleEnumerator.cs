using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Parsing
{
    public class CSSStyleEnumerator : IEnumerator<Style>
    {
        private StringEnumerator _str;
        private CSSStyleParser _owner;
        private Style _curr;

        public CSSStyleEnumerator(StringEnumerator str, CSSStyleParser owner)
        {
            this._str = str;
            this._owner = owner;
        }

        Style IEnumerator<Style>.Current
        {
            get { return _curr; }
        }

        object IEnumerator.Current
        {
            get { return this._curr; }
        }


        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                this._str = null;
            }
        }

        public bool MoveNext()
        {
            this._curr = ParseNextStyle();
            return null != this._curr;
        }

        public void Reset()
        {
            _str.Reset();
        }

        //
        // Private Implementation
        //

        private const char CSSStart = '{';
        private const char CSSEnd = '}';
        

        private Style ParseNextStyle()
        {
            if (!this._str.MoveNext())
                return null;

            var start = this._str.Offset;
            if (start >= this._str.Length)
                return null;

            var next = MoveToNextStyleStart();
            if (next < 0)
            {
                this._str.Offset = this._str.Length;
                return null;
            }
            var end = MoveToNextStyleEnd();
            if(end < next)
            {
                this._str.Offset = this._str.Length;
                return null;
            }

            string selector = "";
            Style parsed = null;

            try
            {
                selector = this._str.Substring(start, next - start);
                string style = this._str.Substring(next + 1, end - (next + 1));
                StyleDefn defn = new StyleDefn();

                defn.Match = selector;
                CSSStyleItemReader reader = new CSSStyleItemReader(style);
                CSSStyleItemAllParser parser = new CSSStyleItemAllParser();

                while (reader.ReadNextAttributeName())
                {
                    parser.SetStyleValue(defn, reader);
                }

                //success, so we can return
                parsed = defn;
            }
            catch (Exception ex)
            {
                if (null != this._owner)
                    this._owner.RegisterParsingError(start, selector, ex);

                this._str.Offset = this._str.Length;
            }

            this._str.Offset = end;
            return parsed;
        }

        private int MoveToNextStyleStart()
        {
            char c;
            while(_str.MoveNext(out c))
            {
                if (c == CSSStart)
                    return _str.Offset;

            }
            return -1;

        }

        private int MoveToNextStyleEnd()
        {
            char c;

            while (_str.MoveNext(out c))
            {
                if (c == CSSEnd)
                    return _str.Offset;

            }
            return -1;
        }
    }

    
}
