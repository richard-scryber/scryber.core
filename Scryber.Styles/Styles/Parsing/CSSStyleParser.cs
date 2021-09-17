using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Scryber.Logging;

namespace Scryber.Styles.Parsing
{
    public class CSSStyleParser : IEnumerable<StyleBase>
    {
        private List<CSSParsingError> _err;
        private static System.Text.RegularExpressions.Regex CommentMatcher = new System.Text.RegularExpressions.Regex("\\/\\*[^*]*\\*+([^/*][^*]*\\*+)*\\/", System.Text.RegularExpressions.RegexOptions.Multiline);

        public IEnumerable<CSSParsingError> Errors
        {
            get { return _err; }
        }

        public bool HasErrors
        {
            get { return this._err.Count > 0; }
        }

        public string Content { get; set; }

        public PDFContextBase Context
        {
            get;
            private set;
        }

        private PDFTraceLog _log;

        public CSSStyleParser(string content, PDFContextBase context)
        {
            this.Context = context;
            this.Content = content;
            this._err = new List<CSSParsingError>();
            this._log = context == null ? new DoNothingTraceLog(TraceRecordLevel.Off) : context.TraceLog;
        }

        public IEnumerator<StyleBase> GetEnumerator()
        {
            var content = this.Content;
            content = this.RemoveComments(content);

            var strEnum = new StringEnumerator(content);
            return new CSSStyleEnumerator(strEnum, this, this.Context);
        }

        internal void RegisterParsingError(int offset, string selector, Exception ex)
        {
            this._err.Add(new CSSParsingError(offset, selector, ex));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private string RemoveComments(string content)
        {
            if (this._log.ShouldLog(TraceLevel.Verbose))
                this._log.Add(TraceLevel.Verbose, "CSS", "Removing comments from css styles");

            
            content = CommentMatcher.Replace(content, "");

            return content;
        }
    }

    public class CSSParsingError
    {
        public int Offest { get; set; }

        public string Selector { get; set; }

        public Exception Exception { get; set; }

        public CSSParsingError(int offset, string selector, Exception ex)
        {
            this.Offest = offset;
            this.Selector = selector;
            this.Exception = ex;
        }
    }
}
