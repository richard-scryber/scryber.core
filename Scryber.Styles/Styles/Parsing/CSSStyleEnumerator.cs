using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Scryber.Logging;

namespace Scryber.Styles.Parsing
{
    public class CSSStyleEnumerator : IEnumerator<StyleBase>
    {
        private StringEnumerator _str;
        private CSSStyleParser _owner;
        private StyleBase _curr;
        private ContextBase _context;
        private TraceLog _log;

        public ContextBase Context
        {
            get { return this._context; }
        }

        public CSSStyleEnumerator(StringEnumerator str, CSSStyleParser owner, ContextBase context)
        {
            this._str = str;
            this._owner = owner;
            this._context = context;
            this._log = context == null ? new DoNothingTraceLog(TraceRecordLevel.Off) : context.TraceLog;
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

                    if (this._log.ShouldLog(TraceLevel.Debug))
                        this._log.Add(TraceLevel.Debug, "CSS", "Found media query at rule for " + selector + " parsing inner contents");

                    var match = Selectors.MediaMatcher.Parse(selector);
                    StyleMediaGroup media = new StyleMediaGroup(match);
                    var innerEnd = MoveToGroupEnd();
                    if (innerEnd <= next)
                        return null;

                    if (this.TryParseInnerStyles(match.ToString(), media, next + 1, innerEnd - (next + 1)))
                        parsed = media;
                    else
                        parsed = new StyleMediaGroup(); //add an empty one

                    this._str.Offset = innerEnd;
                }
                else if (this.IsPageQuery(ref selector))
                {
                    if (this._log.ShouldLog(TraceLevel.Debug))
                        this._log.Add(TraceLevel.Debug, "CSS", "Found page at rule for " + selector + " parsing inner contents");

                    var innerEnd = MoveToNextStyleEnd();
                    if (innerEnd <= next)
                        return null;

                    StylePageGroup pg;
                    if (TryReadPageQuery(selector, next, innerEnd, out pg))
                        parsed = pg;
                    else
                        parsed = new StylePageGroup();

                    this._str.Offset = innerEnd;
                }
                else if (this.IsFontFace(ref selector))
                {
                    if (this._log.ShouldLog(TraceLevel.Debug))
                        this._log.Add(TraceLevel.Debug, "CSS", "Found font-face at rule for " + selector + " parsing inner contents");

                    var innerEnd = MoveToNextStyleEnd();
                    if (innerEnd <= next)
                        return null;

                    StyleFontFace ff;
                    if (TryReadFontFace(selector, next, innerEnd, out ff))
                        parsed = ff;
                    else
                        parsed = new StyleFontFace();

                    this._str.Offset = innerEnd;
                }
                else if (IsRuleSelector(selector))
                {
                    if (this._log.ShouldLog(TraceLevel.Verbose))
                    {
                        this._log.Add(TraceLevel.Warning, "CSS", "Found unsupported rule " + selector + " so skipping content.");
                    }
                    //skip over the inner content, and then read the next
                    this._str.MoveNext();

                    this.MoveToGroupEnd();


                    if (this._log.ShouldLog(TraceLevel.Debug))
                        this._log.Add(TraceLevel.Warning, "CSS", "Next Chars after skipping are " + this._str.Substring(this._str.Offset, 20));

                    return this.ParseNextStyle();
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
                    //string style = this._str.Substring(next + 1, end - (next + 1));



                    StyleDefn defn;
                    if (this.TryReadStyleDefinition(selector, next, end, out defn))
                        parsed = defn;
                    else
                        parsed = new StyleDefn(selector); //just create an empty one
                    this._str.Offset = end;
                }

            }
            catch (Exception ex)
            {
                this._log.Add(TraceLevel.Error, "CSS", "Parsing of css failed with message : " + ex.Message, ex);

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

        private bool IsPageQuery(ref string selector)
        {
            if (!string.IsNullOrEmpty(selector) && selector.StartsWith("@page"))
            {
                if (selector == "@page")
                    selector = string.Empty;
                else
                    selector = selector.Substring("@page".Length).Trim();

                return true;
            }
            else
                return false;
        }

        private bool IsFontFace(ref string selector)
        {
            if (!string.IsNullOrEmpty(selector) && selector.StartsWith("@font-face"))
            {
                if (selector == "@font-face")
                    selector = string.Empty;
                else
                    selector = selector.Substring("@font-face".Length).Trim();

                return true;
            }
            else
                return false;
        }

        private bool IsRuleSelector(string selector)
        {
            if(!string.IsNullOrEmpty(selector) && selector.StartsWith("@"))
            {
                return true;
            }
            else
            {
                return false;
            }
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

        private bool TryReadFontFace(string selector, int next, int innerEnd, out StyleFontFace ff)
        {
            bool success = false;

            try
            {
                ff = this.ReadFontFace(next, innerEnd);
                success = true;
            }
            catch (Exception ex)
            {
                this._log.Add(TraceLevel.Error, "CSS", "Could not parse the inner styles for '" + selector + "' as an error occurred : " + ex.Message, ex);
                ff = null;
                success = false;
            }

            return success;
        }


        private StyleFontFace ReadFontFace(int next, int innerEnd)
        {
            StyleFontFace ff = new StyleFontFace();


            //string style = this._str.Substring(next + 1, innerEnd - (next + 1));
            CSSStyleItemReader reader = new CSSStyleItemReader(new StringEnumerator(this._str, next + 1, innerEnd - (next + 1)));
            CSSStyleItemAllParser parser = new CSSStyleItemAllParser();

            while (reader.ReadNextAttributeName())
            {
                if (!parser.SetStyleValue(ff, reader, this.Context))
                    reader.SkipToNextAttribute();
            }

            return ff;
        }

        private bool TryReadPageQuery(string selector, int next, int innerEnd, out StylePageGroup group)
        {
            bool success = false;

            try
            {
                group = this.ReadPageQuery(selector, next, innerEnd);
                success = true;
            }
            catch (Exception ex)
            {
                this._log.Add(TraceLevel.Error, "CSS", "Could not parse the inner styles for '" + selector + "' as an error occurred : " + ex.Message, ex);
                group = null;
                success = false;
            }

            return success;
        }

        private StylePageGroup ReadPageQuery(string selector, int next, int innerEnd)
        {
            var match = Selectors.PageMatcher.Parse(selector);
            StylePageGroup pg = new StylePageGroup(match);


            //string style = this._str.Substring(next + 1, innerEnd - (next + 1));

            CSSStyleItemReader reader = new CSSStyleItemReader(new StringEnumerator(this._str, next + 1, innerEnd - (next + 1)));
            CSSStyleItemAllParser parser = new CSSStyleItemAllParser();

            while (reader.ReadNextAttributeName())
            {
                parser.SetStyleValue(pg, reader, this.Context);
            }

            return pg;
        }

        private bool TryParseInnerStyles(string name, StyleGroup group, int grpStart, int grpLen)
        {
            bool success = false;
            try
            {
                this.ParseInnerStyles(group, grpStart, grpLen);
                success = true;
            }
            catch (Exception ex)
            {
                this._log.Add(TraceLevel.Error, "CSS", "Could not parse the inner styles for '" + name + "' as an error occurred : " + ex.Message, ex);
                success = false;
            }

            return success;
        }

        private void ParseInnerStyles(StyleGroup group, int grpStart, int grpLen)
        {
            var content = _str.Substring(grpStart, grpLen);
            var enumerator = new StringEnumerator(content);
            var prev = _str;
            
            _str = enumerator;

            while (this.MoveNext())
            {
                if (this._log.ShouldLog(TraceLevel.Debug))
                    this._log.Add(TraceLevel.Debug, "CSS", "Found css style for " + this.Current.ToString() + " parsing inner contents");

                if (null != this.Current)
                    group.Styles.Add(this.Current);
            }
            _str = prev;
        }

        private bool TryReadStyleDefinition(string selector, int next, int end, out StyleDefn defn)
        {
            bool success = false;
            try
            {
                defn = ReadStyleDefinition(selector, next, end);
                success = true;
            }
            catch (Exception ex)
            {
                this._log.Add(TraceLevel.Error, "CSS", "Could not parse the style definition for '" + selector + "' as an error occurred : " + ex.Message, ex);

                defn = null;
                success = false;
            }
            return success;
        }

        private StyleDefn ReadStyleDefinition(string selector, int next, int end)
        {
            StyleDefn defn = new StyleDefn();

            defn.Match = selector;

            if (this._log.ShouldLog(TraceLevel.Debug))
                this._log.Add(TraceLevel.Debug, "CSS", "Found css selector " + defn.Match.ToString() + " parsing inner contents");

            CSSStyleItemReader reader = new CSSStyleItemReader(new StringEnumerator(this._str, next + 1, end - (next + 1)));
            CSSStyleItemAllParser parser = new CSSStyleItemAllParser();

            while (reader.ReadNextAttributeName())
            {
                parser.SetStyleValue(defn, reader, this.Context);
            }

            return defn;
        }
    }

    
}
