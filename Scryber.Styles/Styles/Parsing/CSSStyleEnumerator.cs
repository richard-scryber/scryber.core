using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Parsing
{
    public class CSSStyleEnumerator : IEnumerator<StyleBase>
    {
        private StringEnumerator _str;
        private CSSStyleParser _owner;
        private StyleBase _curr;

        public CSSStyleEnumerator(StringEnumerator str, CSSStyleParser owner)
        {
            this._str = str;
            this._owner = owner;
        }

        public StyleBase Current
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
        

        private StyleBase ParseNextStyle()
        {
            if (!this._str.MoveNext())
                return null;

            var start = this._str.Offset;
            if (start >= this._str.Length)
                return null;

            StyleBase parsed = null;
            string selector = "";
            try
            {
                var next = MoveToNextStyleStart();
                if (next < 0)
                {
                    this._str.Offset = this._str.Length;
                    return null;
                }

                selector = this._str.Substring(start, next - start).Trim();

                if (this.IsMediaQuery(ref selector))
                {
                    var match = Selectors.MediaMatcher.Parse(selector);
                    StyleMediaGroup media = new StyleMediaGroup(match);
                    var innerEnd = MoveToGroupEnd();
                    if (innerEnd <= next)
                        return null;

                    this.ParseInnerStyles(media, next + 1, innerEnd - (next + 1));
                    parsed = media;
                    this._str.Offset = innerEnd;
                }
                else
                {
                    var end = MoveToNextStyleEnd();
                    if (end < next)
                    {
                        this._str.Offset = this._str.Length;
                        return null;
                    }

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
                    this._str.Offset = end;
                }

            }
            catch (Exception ex)
            {
                if (null != this._owner)
                    this._owner.RegisterParsingError(start, selector, ex);

                this._str.Offset = this._str.Length;
            }

            
            return parsed;
        }

        private bool IsMediaQuery(ref string selector)
        {
            if (!string.IsNullOrEmpty(selector) && selector.StartsWith("@media "))
            {
                selector = selector.Substring("@media ".Length);
                return true;
            }
            else
                return false;
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

        private int MoveToGroupEnd()
        {
            var c = ' ';
            var depth = 1;
            while(_str.MoveNext(out c))
            {
                if (c == CSSEnd)
                {
                    depth -= 1;
                    if (depth == 0)
                        return _str.Offset;
                }
                else if (c == CSSStart)
                    depth++;
            }
            _str.Offset = _str.Length;
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

        private void ParseInnerStyles(StyleGroup group, int grpStart, int grpLen)
        {
            var content = _str.Substring(grpStart, grpLen);
            var enumerator = new StringEnumerator(content);
            var prev = _str;
            
            _str = enumerator;

            while (this.MoveNext())
            {
                if (null != this.Current)
                    group.Styles.Add(this.Current);
            }
            _str = prev;
        }
    }

    
}
