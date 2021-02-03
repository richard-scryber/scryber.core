using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Parsing
{
    public class CSSStyleParser : IEnumerable<StyleBase>
    {
        private List<CSSParsingError> _err;
        private static System.Text.RegularExpressions.Regex CommentMatcher = new System.Text.RegularExpressions.Regex("/\\*.*\\*/");
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

        private PDFTraceLog Log;

        public CSSStyleParser(string content, PDFContextBase context)
        {
            this.Content = content;
            this._err = new List<CSSParsingError>();
            this.Context = context;
            if (null == context)
                this.Log = new Logging.DoNothingTraceLog(TraceRecordLevel.Off);
            else
                this.Log = context.TraceLog;
        }

        public IEnumerator<StyleBase> GetEnumerator()
        {
            var content = this.Content;
            content = this.RemoveComments(content);

            var strEnum = new StringEnumerator(content);
            return new CSSStyleEnumerator(strEnum, this, this.Log);
        }

        internal void RegisterParsingError(int offset, string selector, Exception ex)
        {
            this._err.Add(new CSSParsingError(offset, selector, ex));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private string RemoveComments(string contnet)
        {
            if (this.Log.ShouldLog(TraceLevel.Verbose))
                this.Log.Add("CSS", "Removing comments from css styles");

            int start = contnet.IndexOf("/*");
            while (start >= 0)
            {
                int end = contnet.IndexOf("*/");

                if (end <= start)
                    return contnet;

                contnet = contnet.Substring(0, start) + contnet.Substring(end + 2);

                start = contnet.IndexOf("/*");
            }
            return contnet;
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
