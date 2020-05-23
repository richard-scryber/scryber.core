using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Html.Parsing
{
    /// <summary>
    /// Implements the IEnumerator interface and is returned from the HTMLParser to loop over each parsed component in the HTMLParser content
    /// </summary>
    public class HTMLParserEnumerator : IEnumerator<HTMLParserResult>
    {
        private const char HTMLStartTag = '<';
        private const char HTMLEncodedCharStart = '&';
        private const char HTMLCloseTag = '>';
        private const char HTMLEndMarker = '/';
        private const char HTMLWhiteSpace = ' ';
        private const char HTMLCommentSecondChar = '!';
        private const char HTMLStyleItemTerminator = ';';
        private const char HTMLStyleValuePairSeparator = ':';
        internal const char HTMLEntityStartMarker = '&';
        internal const char HTMLEntityEndMarker = ';';
        internal const char HTMLEntityNumberMarker = '#';
        private const char HTMLAttributeKVSeparator = '=';

        private const string HTMLStyleAttribute = "style";
        private const string HTMLCssClassName = "class";

        private const string HTMLCommentStart = "<!--";
        private const string HTMLCommentEnd = "-->";

        private const string HTMLCDATAStart = "<![CDATA[";
        private const string HTMLCDATAEnd = "]]>";
        
        private const string HTMLDocTypeStart = "<!DOCTYPE";
        private const string HTMLDocTypeEnd = ">";

        private const string HTMLProcessingInstructionStart = "<?";
        private const string HTMLProcessingInstructionEnd = ">";

        //
        // properties
        //

        #region protected HTMLParser Parser {get;}

        private HTMLParser _owner; // The Parser that owns this enumerator (and has the text to enumerate over).

        /// <summary>
        /// Gets the original parser that this enumerator belongs to.
        /// </summary>
        protected HTMLParser Parser
        {
            get { return _owner; }
        }

        #endregion

        #region protected StringEnumerator Source {get;}

        private StringEnumerator _src;

        /// <summary>
        /// Gets the source content of this enumerator to parse over.
        /// </summary>
        internal virtual StringEnumerator Source
        {
            get { return _src; }
        }

        #endregion

        #region protected ParserResultStack ParsedPath

        private HTMLParserResultStack _parsedpath = new HTMLParserResultStack(); //A stack that describes the HTML tags containing the cursor.

        /// <summary>
        /// Gets the current open results that we have not closed yet
        /// </summary>
        protected HTMLParserResultStack ParsedPath
        {
            get { return _parsedpath; }
        }

        #endregion

        #region protected ParserResultStack TagsToClose {get;}

        private HTMLParserResultStack _toclose = new HTMLParserResultStack();

        /// <summary>
        /// Gets the stack of tags that should be closed next before moving on to the next token
        /// </summary>
        protected HTMLParserResultStack TagsToClose
        {
            get { return _toclose; }
        }

        #endregion

        #region public ParserResult Current {get;} + IEnumerator.Current

        private HTMLParserResult _current = HTMLParserResult.Invalid;

        /// <summary>
        /// Gets the current parser result (inheritors can set)
        /// </summary>
        public HTMLParserResult Current
        {
            get { return _current; }
            protected set { _current = value; }
        }


        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        #endregion

        #region protected StringBuilder Buffer {get;}
        
        private StringBuilder _buff = new StringBuilder(50);

        /// <summary>
        /// Gets the buffer to use for string and character concatenation (helps to save memory)
        /// </summary>
        protected StringBuilder Buffer
        {
            get { return _buff; }
        }

        #endregion

        //
        // .ctor
        //

        #region internal HTMLParserEnumerator(HTMLParser parser)

        /// <summary>
        /// Creates a new instance of the HTMLParserEnumerator
        /// </summary>
        /// <param name="parser"></param>
        internal HTMLParserEnumerator(HTMLParser parser)
        {
            if (null == parser)
                throw new ArgumentNullException("parser");

            this._owner = parser;
            this._src = new StringEnumerator(_owner.Source);
        }

        #endregion

        //
        // public methods
        //

        #region public bool MoveNext()

        /// <summary>
        /// Either moves this enumerator onto the next result and returns true, or returns false if there are no more results to return
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (this.TagsToClose.Count > 0)
            {
                this.Current = this.TagsToClose.Pop();
                this.Current = new HTMLParserResult(this.Current.Parsed, this.Current.Type, this.Current.Value, this.Current.OffsetStart, this.Current.OffsetEnd, false);
                return true;
            }
            else
                return this.ExtractNextResult();
        }

        #endregion

        #region public void Reset()

        /// <summary>
        /// Resets this enumerator, so it will (re)start from the beginning
        /// </summary>
        public void Reset()
        {
            this._current = HTMLParserResult.Invalid;
            this.Source.Reset();
            this.ParsedPath.Clear();
            this.TagsToClose.Clear();
            this.Buffer.Clear();
        }

        #endregion

        #region public void Dispose()

        /// <summary>
        /// IDisposable implementation of the IEnumerator interface
        /// </summary>
        public void Dispose()
        {
            this.Reset();
        }

        #endregion

        //
        // implementation methods
        //

        #region protected virtual bool ExtractNextToken()

        /// <summary>
        /// Attempts to extract the next parser result token from the current source. 
        /// Returns true if there is another result or false if not.
        /// </summary>
        /// <returns></returns>
        protected virtual bool ExtractNextResult()
        {

            this.ClearCurrent();
            this.Source.MoveNext();

            if (this.Source.EOS)
            {
                //Last was a ending but we have more on the stack to close off
                if (this.ParsedPath.Count > 0)
                {
                    this.Current = this.ParsedPath.Pop();

                    //Implicit closing of the start tags - even if they shouldn't be
                    if (this.Current.IsStart)
                    {
                        this.Current = GetImplicitEnd(this.Current);
                        return this.Current.Valid;
                    }
                }

                return false;
            }
            else if (this.Source.Current == HTMLStartTag)
            {
                return ExtractHTMLTag();
            }
            else //text characters - read to the next tag
            {
                return ExtractTextualContent();
            }
        }

        #endregion

        #region protected virtual bool ExtractHTMLTag()

        /// <summary>
        /// With the Source positioned at the opening of an HTML tag (&lt;) - read in the next tag as the current result.
        /// Returns true if a tag was parsed (or ignored and the next token parsed after it).
        /// </summary>
        /// <returns></returns>
        protected virtual bool ExtractHTMLTag()
        {
            int start;
            
            HtmlComponentType type;
            string terminator;
            if (IsNonHtmlTag(out type, out terminator)) //CData, Processing Instruction, Comment, DocType
            {
                start = this.Source.Offset;
                int length = MoveToEnd(terminator);
                if (ShouldSkipHtmlType(type))
                {
                    return ExtractNextResult();
                }
                else
                {
                    string subs = this.Source.Substring(start, length);
                    if (type == HtmlComponentType.ProcessingInstruction && subs.EndsWith("?"))
                    {
                        //we have an XML processing instruction, not just a SGML Processing Instruction 
                        //that ends with ?> not just >
                        length -= 1;
                        subs = this.Source.Substring(start, length);
                    }
                    this.Current = new HTMLParserResult(null, type, subs, start, start + length, true);
                    this.TagsToClose.Push(this.Current); //Autoclose the non-html tag
                    return true;
                }
                
                
            }

            start = this.Source.Offset;
            string name;
            bool autoend;
            bool isEndMarker;

            IPDFComponent com = GetCurrentTag(out name, out type, out autoend, out isEndMarker);
            

            if (null != com)
            {
                int end = this.Source.Offset;
                bool isStart = !isEndMarker;
                HTMLParserResult result = new HTMLParserResult(com, type, name, start, end, isStart);
                this.Current = result;

                if (autoend)
                    this.TagsToClose.Push(result);
                else if (isStart)
                    this.ParsedPath.Push(result);


                return true;
            }
            else if (this.Parser.Settings.SkipUnknownTags)
            {
                //We could not parse the current token but we are set to skip, 
                //so let's just go for the next token
                return ExtractNextResult();
            }
            else
                return false;
        }

        #endregion

        #region protected virtual bool ExtractTextualContent()

        /// <summary>
        /// With the source positioned over the starting character of some textual content - 
        /// read all the characters up to the next tag, or the end of the source. RESETS THE BUFFER
        /// </summary>
        /// <returns></returns>
        protected virtual bool ExtractTextualContent()
        {
            this.Buffer.Clear();
            int start = this.Source.Offset;
            char cur = this.Source.Current;
            bool lastwasWhiteSpace = false;

            while (cur != HTMLStartTag)
            {
                this.Source.MoveNext();
                if (IsWhiteSpace(cur))
                {
                    if (lastwasWhiteSpace)
                    {
                        if (this.Source.EOS)
                            break;
                        cur = this.Source.Current;
                        continue;
                    }
                    lastwasWhiteSpace = true;
                    cur = ' ';
                }
                else if (cur == HTMLEntityStartMarker)
                {
                    cur = ReadHtmlEscapedChar(this.Source);
                    lastwasWhiteSpace = (cur == HTMLWhiteSpace);
                }
                else
                    lastwasWhiteSpace = false;

                this.Buffer.Append(cur);

                if (this.Source.EOS)
                    break;
                cur = this.Source.Current;

            }

            if (cur == HTMLStartTag)
                this.Source.MovePrev();

            if(this.Buffer.Length == 0 || ContentIsJustSingleWhiteSpace(this.Buffer))
                return ExtractNextResult();
            else
            {
                string content = this.Buffer.ToString();

                IPDFComponent text = _owner.ComponentFactory.GetTextComponent(this.Parser, this.Buffer.ToString());
                HTMLParserResult result = new HTMLParserResult(text, HtmlComponentType.Text, null, start, this.Source.Offset - 1, true);
                this.TagsToClose.Push(result);
                this.Current = result;
                return true;
            }
            
        }

        #endregion

        protected virtual bool ShouldSkipHtmlType(HtmlComponentType type)
        {
            switch (type)
            {
                case HtmlComponentType.DocType:
                    return this.Parser.Settings.SkipDocType;

                case HtmlComponentType.Comment:
                    return this.Parser.Settings.SkipComments;

                case HtmlComponentType.ProcessingInstruction:
                    return this.Parser.Settings.SkipProcessingInstructions;

                case HtmlComponentType.CData:
                    return this.Parser.Settings.SkipCData;

                case HtmlComponentType.Unknown:
                    return this.Parser.Settings.SkipUnknownTags;

                default:
                    return false;
            }
        }
        #region protected bool IsCommentOrCDATAStart(out bool isCdata)

        /// <summary>
        /// Returns true if the current cursor is over a CDATA, Processing Instruction, DocType or Comment section.
        /// </summary>
        /// <param name="isCdata"></param>
        /// <returns></returns>
        protected bool IsNonHtmlTag(out HtmlComponentType type, out string terminator)
        {
            //CData is <![CDATA[ ... content ... ]]>
            //Comment is <!-- ... content ... -->
            //Processing Instruction is <? .... instruction ... ?>
            //Doc Type is <!DOCTYPE ... type ... >

            //Quick check on second char first
            if (this.Source.Offset + 1 < this.Source.Length)
            {
                if (this.Source.Peek(1) == HTMLCommentSecondChar) //second char for comment, doctype and CData are the same.)
                {
                    //Full check on comment string
                    if (this.Source.Offset + HTMLCommentStart.Length < this.Source.Length
                        && this.Source.Substring(HTMLCommentStart.Length) == HTMLCommentStart)
                    {
                        this.Source.Offset += HTMLCommentStart.Length;
                        type = HtmlComponentType.Comment;
                        terminator = HTMLCommentEnd;
                        return true;
                    }
                    //Full check on CDATA string
                    if (this.Source.Offset + HTMLCDATAStart.Length < this.Source.Length && this.Source.Substring(HTMLCDATAStart.Length) == HTMLCDATAStart)
                    {
                        this.Source.Offset += HTMLCDATAStart.Length;
                        type = HtmlComponentType.CData;
                        terminator = HTMLCDATAEnd;
                        return true;
                    }

                    if(this.Source.Offset + HTMLDocTypeStart.Length < this.Source.Length && this.Source.Substring(HTMLDocTypeStart.Length) == HTMLDocTypeStart)
                    {
                        this.Source.Offset += HTMLDocTypeStart.Length;
                        type = HtmlComponentType.DocType;
                        terminator = HTMLDocTypeEnd;
                        return true;
                    }
                }
                else if (this.Source.Offset + HTMLProcessingInstructionStart.Length < this.Source.Length && this.Source.Substring(HTMLProcessingInstructionStart.Length) == HTMLProcessingInstructionStart)
                {
                    this.Source.Offset += HTMLProcessingInstructionStart.Length;
                    type = HtmlComponentType.ProcessingInstruction;
                    terminator = HTMLProcessingInstructionEnd;
                    return true;
                }
            }
            type = HtmlComponentType.None;
            terminator = string.Empty;
            return false;
        }

        #endregion

        #region protected void MoveToCDATAEnd() + protected void MoveToCommentEnd() + private void MoveToEnd(string teminator)

        /// <summary>
        /// Moves the cursor to the end of the &lt;[CDATA[ section
        /// </summary>
        protected int MoveToCDATAEnd()
        {
            return MoveToEnd(HTMLCDATAEnd);
        }

        /// <summary>
        /// Moves the cursor to the end of an HTML comment (--&gt;) section, returning the length of the comment
        /// </summary>
        protected int MoveToCommentEnd()
        {
            return MoveToEnd(HTMLCommentEnd);
        }

        /// <summary>
        /// Moves the cursor to the position of the next terminator and returns the length of the inner content.
        /// </summary>
        /// <param name="terminator"></param>
        /// <returns></returns>
        private int MoveToEnd(string terminator)
        {
            if (string.IsNullOrEmpty(terminator))
                throw new ArgumentNullException("terminator");

            int start = this.Source.Offset;
            int max = this.Source.Length - terminator.Length;
            while (this.Source.Offset < max)
            {
                if (this.Source.Matches(terminator))
                {
                    this.Source.Offset += terminator.Length - 1; //to the closing tag
                    int end = this.Source.Offset;
                    end -= terminator.Length - 1;
                    return end - start;
                }
                this.Source.MoveNext();

            }
            if(this.Source.Matches(terminator)) //terminator is at the very end
            {
                this.Source.Offset = this.Source.Length;
                int end = this.Source.Length;
                end -= terminator.Length;
                return end - start;
            }
            if (this.Source.Offset >= max) //did't find a closing tag
                this.Source.Offset = this.Source.Length;
            return this.Source.Length - start;
        }

        #endregion

        #region private bool ContentIsJustSingleWhiteSpace(StringBuilder content)

        /// <summary>
        /// Checks the string builder proveided and if it is just a single space returns true
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool ContentIsJustSingleWhiteSpace(StringBuilder content)
        {
            if (content.Length == 1 && content[0] == ' ')
                return true;
            else
                return false;
        }

        #endregion

        #region private bool IsWhiteSpace(char c)

        /// <summary>
        /// Retuns true if the passed character is whitespace (carriage return, line feed)
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsWhiteSpace(char c)
        {
            switch (c)
            {
                case ' ':
                case '\r':
                case '\n':
                case '\t':
                    return true;

                default:
                    return false;
            }
        }

        #endregion

        #region private char ReadHtmlEscapedChar(StringEnumerator src)

        /// <summary>
        /// Returns the character that has been escasped as the current position (just after the ampersand)
        /// </summary>
        /// <returns></returns>
        private char ReadHtmlEscapedChar(StringEnumerator src)
        {
            

            //Fall back if not found is just to output as is.
            //So we need to remember where we were.
            int curPos = src.Offset;

            int ampersandPos = curPos - 1;

            char cur = src.Current;
            
            bool terminated = true;

            while (cur != HTMLEntityEndMarker)
            {

                if (!src.MoveNext())
                {
                    terminated = false;
                    break;
                }
                if (src.Offset > curPos + 10) //Max limit of HTML Entities - we are missing a teminating ;
                {
                    terminated = false;
                    break;
                }

                cur = src.Current;
            }

            if (terminated)
            {
                int length = 1 + src.Offset - ampersandPos;
                string entity = src.Substring(ampersandPos, length);
                char found;

                if (entity.Length < 3)
                {
                    src.Offset = curPos;
                    return HTMLEntityStartMarker;
                }
                else if (entity[1] == HTMLEntityNumberMarker) //we have the character number
                {
                    int charNum;

                    if (int.TryParse(entity.Substring(2, entity.Length - 3), out charNum))
                    {
                        found = (char)charNum;
                        src.MoveNext();
                        return found;
                    }
                    else //could not parse the number
                    {
                        src.Offset = curPos;
                        return HTMLEntityStartMarker;
                    }
                }
                else if (this.Parser.Settings.HTMLEntityMappings.TryGetValue(entity, out found))
                {
                    src.MoveNext(); //past the ;
                    return found;
                }
                else //Don't know this one so just go back to the orginal offset and return
                {
                    src.Offset = curPos;
                    return HTMLEntityStartMarker;
                }
            }
            else //Hit End of String or character limit before terminator
            {
                src.Offset = curPos;
                return HTMLEntityStartMarker;
            }
        }

        #endregion

        #region private IPDFComponent GetCurrentTag(out string name, out bool autoend, out bool isEndMarker)

        /// <summary>
        /// Based on the current source reads in a single HTML tag and all it's attributes.
        /// If it is a known component (based on the parsers component factory) then this will be returned, otherwise null.
        /// </summary>
        /// <param name="name">The actual characters read as the tag name</param>
        /// <param name="autoend">Set to true if this tag / component should always end after it has begun (does not contain other components / tags)</param>
        /// <param name="isEndMarker">Set to true if this is the end marker of another tag (&lt;/span&gt; etc)</param>
        /// <returns>The parsed component or null</returns>
        private IPDFComponent GetCurrentTag(out string name, out HtmlComponentType type, out bool autoend, out bool isEndMarker)
        {
            autoend = false;
            isEndMarker = false;
            name = string.Empty;
            this.Source.MoveNext();
            char cur = this.Source.Current;

            if (cur == HTMLEndMarker) //We are parsing a closing tag and want the inner text
            {
                if (this.Source.MoveNext())
                {
                    cur = this.Source.Current;
                    isEndMarker = true;
                }
                else
                {
                    type = HtmlComponentType.None;
                    return null;
                }
            }

            this.Buffer.Clear();

            while (!this.Source.EOS)
            {
                cur = this.Source.Current;
                if (cur == HTMLEndMarker || cur == HTMLCloseTag || cur == HTMLWhiteSpace)
                    break;
                else
                    this.Buffer.Append(cur);

                this.Source.MoveNext();
            }

            name = this.Buffer.ToString();

            if (isEndMarker)
            {
                if (this.ParsedPath.Count > 0)
                {
                    int index = this.ParsedPath.IndexOf(name);

                    if (index == this.ParsedPath.Count - 1)
                    {
                        //Last one so just pop it
                        HTMLParserResult prev = this.ParsedPath.Pop();
                        type = prev.Type;
                        return prev.Parsed;
                    }
                    else if (index > -1)
                    {
                        HTMLParserResult[] tags = this.ParsedPath.PopToTag(name);
                        this.TagsToClose.PushRange(tags);
                        HTMLParserResult prev = this.TagsToClose.Pop();
                        type = prev.Type;
                        return prev.Parsed;
                    }
                }
                //Orphan end marker
                type = HtmlComponentType.None;
                return null;
            }
            else if (IsSkippedTag(name))
            {
                string endTag = HTMLStartTag.ToString() + HTMLEndMarker.ToString() + name + HTMLCloseTag.ToString();
                MoveToEnd(endTag);
                type = HtmlComponentType.None;
                return null;
            }

            IPDFComponent parsed = this._owner.ComponentFactory.GetComponent(this.Parser, name, out type);

            if (!this.Source.EOS)
            {
                if (cur == HTMLWhiteSpace)
                {
                    ReadAttributes(parsed, name);
                }
                else
                {
                    while (cur == HTMLEndMarker || cur == HTMLCloseTag)
                    {
                        
                        if (!this.Source.MoveNext())
                            break;
                        cur = this.Source.Current;
                    }

                    if (!this.Source.EOS) 
                        this.Source.MovePrev(); //extra character for looking past marker removed
                }
            }
            

            
            
            if (null != parsed)
                autoend = !this._owner.ComponentFactory.IsContainerComponent(this.Parser, parsed, name);
            else
                autoend = false;

            return parsed;
        }

        #endregion

        #region private bool IsSkippedTag(string name)

        /// <summary>
        /// Returns true if the tag name is one of the known tags to skip over
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsSkippedTag(string name)
        {
            foreach (string skip in this.Parser.Settings.SkipOverTags)
            {
                if (skip.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        #endregion

        #region private Dictionary<string, string> GetSplitStyleItems(string styleitems)

        /// <summary>
        /// Based on the provided string of a style attribute. Splits each of the items up into 
        /// </summary>
        /// <param name="styleitems"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetSplitStyleItems(string styleitems)
        {
            if (styleitems.StartsWith("'") || styleitems.StartsWith("\""))
                styleitems = styleitems.Substring(1);
            if (styleitems.EndsWith("'") || styleitems.EndsWith("\""))
                styleitems = styleitems.Substring(0, styleitems.Length - 1);

            if (styleitems.Length > 0)
            {
                Buffer.Clear();
                Dictionary<string, string> split = new Dictionary<string, string>();
                StringEnumerator enumerate = new StringEnumerator(styleitems);
                bool inkey = true;
                string keyname = string.Empty;
                char stylevaluegroupchar = '\0';
                while (enumerate.MoveNext())
                {
                    char cur = enumerate.Current;
                    if (cur == HTMLStyleItemTerminator)
                    {
                        if (inkey == false)
                        {
                            if (stylevaluegroupchar != '\0')
                            {
                                //Still in a value grouping so ignore the terminator and just add 
                                Buffer.Append(cur);
                            }
                            //full value has been read - if we have a key, then set the value
                            else if (!string.IsNullOrEmpty(keyname))
                            {
                                split[keyname] = Buffer.ToString().Trim();
                                Buffer.Clear();
                                inkey = true;
                                stylevaluegroupchar = '\0';
                            }
                            else
                            {
                                //No key so ignore and move on
                                Buffer.Clear();
                                inkey = true;
                                stylevaluegroupchar = '\0';
                            }
                        }
                    }
                    else if (cur == stylevaluegroupchar)
                    {
                        Buffer.Append(cur);
                        stylevaluegroupchar = '\0';
                    }
                    else if (cur == HTMLStyleValuePairSeparator)
                    {
                        if (inkey)
                        {
                            keyname = Buffer.ToString().Trim();
                            Buffer.Clear();
                            inkey = false;
                        }
                        else if (stylevaluegroupchar != '\0')
                            Buffer.Append(cur);
                    }
                    else if (cur == '(' && Buffer.ToString() == "url")
                    {
                        stylevaluegroupchar = ')';
                        Buffer.Append(cur);
                    }
                    else if (cur == HTMLEntityStartMarker)
                    {
                        enumerate.MoveNext();
                        cur = ReadHtmlEscapedChar(enumerate);
                        Buffer.Append(cur);
                        enumerate.MovePrev();
                    }
                    else
                        Buffer.Append(cur);
                }

                if (inkey == false && Buffer.Length > 0 && keyname.Length > 0)
                {
                    split[keyname] = Buffer.ToString().Trim();
                }

                if (split.Count > 0)
                    return split;
                else
                    return null;
            }

            return null;
        }

        #endregion

        #region private string HTMLUnEntities(string value)

        /// <summary>
        /// Checks the string and restores any encoded HTML Entities such as &amp; and &quot; to their 
        /// standard character & and " returning the result. Resets the Buffer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string HTMLUnEntities(string value)
        {
            Buffer.Clear();

            if (value.IndexOf(HTMLEntityStartMarker) > -1)
            {
                StringEnumerator enumerate = new StringEnumerator(value);
                while (enumerate.MoveNext())
                {
                    if (enumerate.Current == HTMLEntityStartMarker)
                    {
                        if (!enumerate.MoveNext())
                        {
                            Buffer.Append(enumerate.Current);
                            return Buffer.ToString();
                        }
                        else
                        {
                            char unencoded = ReadHtmlEscapedChar(enumerate);
                            Buffer.Append(unencoded);
                            enumerate.MovePrev();
                        }
                    }
                    else
                        Buffer.Append(enumerate.Current);
                }
                value = Buffer.ToString();
            }
            return value;
        }

        #endregion

        #region private void ReadAttributes(IComponent parsed, string tagname)

        /// <summary>
        /// Reads each of the atributes in turn (if any) in the current Source and sets them onto the parsed compent. 
        /// Style attributes will be split again and set individually. Resets the buffer
        /// </summary>
        /// <param name="parsed"></param>
        /// <param name="tagname"></param>
        private void ReadAttributes(IPDFComponent parsed, string tagname)
        {
            //Read to the start of each attribute in turn
            //Then check the current name aginst the Style and Class attribute values (including the equals)
            //If they are a match then read the values, otherwise skip onto the next white space.

            while (!this.Source.EOS)
            {
                while (!this.Source.EOS && IsWhiteSpace(this.Source.Current))
                {
                    if (!this.Source.MoveNext())
                        return;
                }

                Buffer.Clear();
                bool foundAttrName = false;

                char cur = this.Source.Current;
                if (cur == HTMLCloseTag)
                    break;
                else if(cur == HTMLEndMarker)
                {
                    if (!this.Source.MoveNext() || this.Source.Current == HTMLCloseTag)
                        break;
                    Buffer.Append(cur);
                }
                else
                {
                    Buffer.Append(cur);
                    

                    while (this.Source.MoveNext())
                    {
                        cur = this.Source.Current;

                        if (this.Source.Current == HTMLAttributeKVSeparator)
                        {
                            foundAttrName = true;
                            break;
                        }
                        Buffer.Append(cur);
                    }
                }

                if (foundAttrName && Buffer.Length > 0)
                {
                    string attrName = Buffer.ToString();
                    Buffer.Clear();

                    if (!this.Source.MoveNext())
                        return;

                    string attrValue = ReadAttributeValue();

                    if (!string.IsNullOrEmpty(attrValue))
                    {
                        if (string.Equals(attrName, HTMLStyleAttribute, StringComparison.OrdinalIgnoreCase)) 
                        {
                            if (this.Parser.Settings.SkipStyles == false && parsed is IPDFStyledComponent)
                            {
                                IPDFStyledComponent parsedStyled = parsed as IPDFStyledComponent;

                                CSSStyleItemReader reader = new CSSStyleItemReader(attrValue);
                                while (reader.ReadNextAttributeName())
                                {
                                    this.Parser.StyleFactory.SetStyleValue(this.Parser, parsedStyled, reader);
                                }
                                
                            }
                        }
                        else if (string.Equals(attrName, HTMLCssClassName, StringComparison.OrdinalIgnoreCase))
                        {
                            if (this.Parser.Settings.SkipCssClasses == false)
                            {
                                attrValue = HTMLUnEntities(attrValue);
                                this.Parser.ComponentFactory.SetAttribute(this.Parser, parsed, tagname, attrName, attrValue);
                            }
                        }
                        else
                        {
                            attrValue = HTMLUnEntities(attrValue);
                            this.Parser.ComponentFactory.SetAttribute(this.Parser, parsed, tagname, attrName, attrValue);
                        }
                        
                    }

                    if (this.Source.Current == '\'' || this.Source.Current == '"')
                        this.Source.MoveNext(); //push past the end of the closing quotes;
                }
                else
                {
                    while (!this.Source.EOS && !IsWhiteSpace(this.Source.Current))
                    {
                        if (!this.Source.MoveNext())
                            return;
                        if (this.Source.Current == HTMLCloseTag) //safe because > is reserved and should not be used in attributes.
                            return;
                    }
                }
            }
        }

        #endregion

        #region private string ReadAttributeValue()

        /// <summary>
        /// Reads and Returns a single attribute value from the current source.
        /// </summary>
        /// <returns></returns>
        private string ReadAttributeValue()
        {
            char startquote = this.Source.Current;
            
            this.Buffer.Clear();
            bool hasquote = startquote == '\'' || startquote == '"';
            if (!hasquote)
                this.Buffer.Append(startquote);

            char cur;

            while (this.Source.MoveNext(out cur))
            {
                if (cur == startquote)
                    return this.Buffer.ToString();

                else if (IsWhiteSpace(cur) && !hasquote)
                    return this.Buffer.ToString();

                else if (cur == HTMLCloseTag)
                    return string.Empty;
                else
                    this.Buffer.Append(cur);
            }

            //This was an unclosed attribute value - no idea what it might be so let's just return an empty string.
            return string.Empty;
        }

        #endregion

        #region private void ClearCurrent()

        /// <summary>
        /// Resets the current Parser Result
        /// </summary>
        private void ClearCurrent()
        {
            this.Current = HTMLParserResult.Invalid;
        }

        #endregion

        #region private ParserResult GetImplicitEnd(ParserResult start)

        /// <summary>
        /// Converts a start result into a new end result for the same component and returns it.
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private HTMLParserResult GetImplicitEnd(HTMLParserResult start)
        {
            if (start.IsStart == false)
                throw new ArgumentException("Cannot get the implicit ending of a result that is itself not a start");

            bool isStart = false;
            HTMLParserResult copy = new HTMLParserResult(start.Parsed, start.Type, start.Value, start.OffsetStart, start.OffsetEnd, isStart);
            return copy;
        }

        #endregion
    }
}
