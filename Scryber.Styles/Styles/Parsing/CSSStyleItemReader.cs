using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scryber.Styles.Parsing
{
    /// <summary>
    /// Reads css style items
    /// </summary>
    public class CSSStyleItemReader
    {
        #region public StringEnumerator InnerEnumerator {get;}

        StringEnumerator _innerEnumerator;

        /// <summary>
        /// Gets the inner string enumerator for this CSSStyleItemReader
        /// </summary>
        public StringEnumerator InnerEnumerator
        {
            get { return _innerEnumerator; }
        }

        #endregion

        #region public int StartOffset {get;}

        private int _startOffset = -1;

        /// <summary>
        /// Gets the starting offset of the style attribute value
        /// </summary>
        public int StartOffset
        {
            get { return _startOffset; }
        }

        #endregion

        #region public int MaxOffset {get;}

        private int _endOffset = -1;

        /// <summary>
        /// Gets the effective end of the CSSStyle string
        /// </summary>
        public int EndOffset
        {
            get { return _endOffset; }
        }

        #endregion

        

        #region protected StringBuilder Buffer {get;}

        private StringBuilder _buffer = new StringBuilder();

        /// <summary>
        /// Gets the inner buffer that can be populated
        /// </summary>
        protected StringBuilder Buffer
        {
            get { return _buffer; }
        }

        #endregion

        #region public string CurrentAttribute {get;}

        private string _attr;

        /// <summary>
        /// Gets the name of the current attribute being read
        /// </summary>
        public string CurrentAttribute
        {
            get { return _attr; }
        }

        #endregion

        #region public string CurrentTextValue {get;}

        private string _value = string.Empty;

        /// <summary>
        /// Gets the current value within the CSS items as a string
        /// </summary>
        public string CurrentTextValue
        {
            get { return _value; }
        }

        #endregion


        //
        // .ctor
        //

        public CSSStyleItemReader(string styles)
            : this(new StringEnumerator(styles))
        { }


        public CSSStyleItemReader(string styles, int startIndex, int length)
            : this(new StringEnumerator(styles, startIndex, length))
        { }


        public CSSStyleItemReader(StringEnumerator stringEnumerator)
        {
            if (null == stringEnumerator)
                throw new NullReferenceException("stringEnumerator");

            _innerEnumerator = stringEnumerator;
            FindRange();
        }

        


        public bool IsMatch(string value)
        {
            return false;
        }

        public bool IsMatch(string[] any, out int index)
        {
            index = -1;
            return false;
        }

        public bool MoveToNextValue()
        {
            return ReadNextValue(';');
        }

        public bool SkipToNextAttribute()
        {
            while(this.InnerEnumerator.Offset < this.EndOffset)
            {
                if (!this.InnerEnumerator.MoveNext())
                    return false;

                if (this.InnerEnumerator.Current == ';')
                    break;
            }
            this.Buffer.Clear();
            return this.InnerEnumerator.MoveNext();
        }

        public bool MoveToNextAttribute()
        {
            while (this.InnerEnumerator.Current == ';' && this.InnerEnumerator.Offset < this.EndOffset)
            {
                if (!this.InnerEnumerator.MoveNext())
                    return false;
            }
            this.Buffer.Clear();

            return this.InnerEnumerator.Offset < this.EndOffset;
        }

        public bool ReadNextAttributeName()
        {
            if (!this.BeginNameRead())
                return false;

            int start = this.InnerEnumerator.Offset;
            int end = this.InnerEnumerator.Offset;
            bool started = false;
            bool ended = false;

            if (CurrentIsWhiteSpace())
                start += 1;


            while (this.InnerEnumerator.MoveNext() 
                && this.InnerEnumerator.Current != ':' 
                && this.InnerEnumerator.Offset <= this.EndOffset)
            {
                if (!started && (CurrentIsWhiteSpace() || this.InnerEnumerator.Current == ';'))
                    start = this.InnerEnumerator.Offset + 1;
                else if (!ended)
                {
                    end = this.InnerEnumerator.Offset;
                    started = true;
                }
            }
            if (end > start)
            {
                this.Buffer.Append(this.InnerEnumerator.Substring(start, (end - start) + 1));
                this._attr = this.Buffer.ToString().TrimEnd();
                
                return true;
            }
            else
            {
                this._attr = string.Empty;
                return false;
            }
        }

        public bool ReadNextValue()
        {
            return ReadNextValue(' ',';');
        }

        public bool ReadNextValue(char separator, bool ignoreWhiteSpace = false)
        {
            if (!this.BeginValueRead())
                return false;

            if (this.InnerEnumerator.Current == separator)
                return false;

            int end = this.InnerEnumerator.Offset;
            int start = this.InnerEnumerator.Offset;

            bool inquote = this.InnerEnumerator.Current == '\'';
            bool indoublequote = this.InnerEnumerator.Current == '"';
            bool inparentheses = this.InnerEnumerator.Current == '(';

            while (this.InnerEnumerator.MoveNext() && this.InnerEnumerator.Offset <= this.EndOffset)
            {
                char cur = this.InnerEnumerator.Current;
                if (CurrentIsWhiteSpace() && !ignoreWhiteSpace)
                {
                    if (!inquote && !indoublequote && !inparentheses)
                        break;
                }
                else if (cur == separator)
                {
                    if (!inquote && !indoublequote && !inparentheses)
                        break;
                }
                else if (cur == '\'')
                    inquote = !inquote;

                else if (cur == '"')
                    indoublequote = !indoublequote;

                else if (cur == '(')
                    inparentheses = true;

                else if (cur == ')')
                    inparentheses = false;

                end = this.InnerEnumerator.Offset;
            }

            if (end >= start)
            {
                this.Buffer.Append(this.InnerEnumerator.Substring(start, (end-start) + 1));
                this._value = this.Buffer.ToString();
                return true;
            }
            else
                return false;
        }

        public bool ReadNextValue(char separator1, char separator2, bool ignoreWhiteSpace = false)
        {
            if (!this.BeginValueRead())
                return false;

            if (this.InnerEnumerator.Current == separator1 || this.InnerEnumerator.Current == separator2)
                return false;

            int end = this.InnerEnumerator.Offset;
            int start = this.InnerEnumerator.Offset;

            bool inquote = this.InnerEnumerator.Current == '\'';
            bool indoublequote = this.InnerEnumerator.Current == '"';
            bool inparentheses = this.InnerEnumerator.Current == '(';

            while (this.InnerEnumerator.MoveNext() && this.InnerEnumerator.Offset <= this.EndOffset)
            {
                char cur = this.InnerEnumerator.Current;
                if (CurrentIsWhiteSpace() && !ignoreWhiteSpace)
                {
                    if (!inquote && !indoublequote && !inparentheses)
                        break;
                }
                else if (cur == separator1 || cur == separator2)
                {
                    if (!inquote && !indoublequote && !inparentheses)
                        break;
                }

                else if (cur == '\'')
                    inquote = !inquote;

                else if (cur == '"')
                    indoublequote = !indoublequote;

                else if (cur == '(')
                    MoveToEndParentheses();

                else if (cur == ')')
                    inparentheses = false;

                end = this.InnerEnumerator.Offset;
            }

            if (end >= start)
            {
                this.Buffer.Append(this.InnerEnumerator.Substring(start, (end - start) + 1));
                this._value = this.Buffer.ToString();
                return true;
            }
            else
                return false;
        }

        private void MoveToEndParentheses()
        {
            if (this.InnerEnumerator.Current != '(')
                throw new InvalidOperationException("The string enumerator is not on an opening parenthese");

            int count = 1;

            while(this.InnerEnumerator.MoveNext() && this.InnerEnumerator.Offset <= this.EndOffset)
            {
                if (this.InnerEnumerator.Current == ')')
                {
                    count--;
                    if (count == 0)
                        break;
                }
                else if (this.InnerEnumerator.Current == '(')
                {
                    count++;
                }
                else if (this.InnerEnumerator.Offset == this.EndOffset)
                    throw new InvalidOperationException("There are an odd number of open and close parenthese");
            }
        }

        public bool ReadNextValue(char separator1, char separator2, char separator3, bool ignoreWhiteSpace = false)
        {
            if (!this.BeginValueRead())
                return false;

            if (this.InnerEnumerator.Current == separator1 
                || this.InnerEnumerator.Current == separator2
                || this.InnerEnumerator.Current == separator3)

                return false;

            int end = this.InnerEnumerator.Offset;
            int start = this.InnerEnumerator.Offset;

            bool inquote = this.InnerEnumerator.Current == '\'';
            bool indoublequote = this.InnerEnumerator.Current == '"';
            bool inparentheses = this.InnerEnumerator.Current == '(';
            
            while (this.InnerEnumerator.MoveNext() && this.InnerEnumerator.Offset <= this.EndOffset)
            {
                char cur = this.InnerEnumerator.Current;
                if (CurrentIsWhiteSpace() && !ignoreWhiteSpace)
                {
                    if (!inquote && !indoublequote && !inparentheses)
                        break;
                }
                else if (cur == separator1 || cur == separator2 || cur == separator3)
                {
                    if (!inquote && !indoublequote && !inparentheses)
                        break;
                }

                else if (cur == '\'')
                    inquote = !inquote;

                else if (cur == '"')
                    indoublequote = !indoublequote;

                else if (cur == '(')
                    inparentheses = true;

                else if (cur == ')')
                    inparentheses = false;

                end = this.InnerEnumerator.Offset;
            }

            if (end > start)
            {
                this.Buffer.Append(this.InnerEnumerator.Substring(start, (end - start) + 1));
                this._value = this.Buffer.ToString();
                return true;
            }
            else
                return false;
        }


        

        //
        // private implementation
        //

        private bool BeginValueRead()
        {
            this._value = string.Empty;
            //this._valueType = CSSValueType.None;

            this.Buffer.Clear();

            if (this.InnerEnumerator.EOS)
                return false;

            if (this.InnerEnumerator.Current == ':')
            {
                if (!this.InnerEnumerator.MoveNext() || this.InnerEnumerator.Offset > this.EndOffset)
                    return false;
            }
            while (CurrentIsWhiteSpace())
            {
                if (!this.InnerEnumerator.MoveNext() || this.InnerEnumerator.Offset > this.EndOffset)
                    return false;
            }
            return true;
        }

        private bool BeginNameRead()
        {
            this._attr = string.Empty;
            this._value = string.Empty;
            //this._valueType = CSSValueType.None;

            this.Buffer.Clear();

            if (this.InnerEnumerator.EOS)
                return false;

            if (this.InnerEnumerator.Current == ';')
            {
                if (!this.InnerEnumerator.MoveNext())
                    return false;
            }
            return true;
        }

        #region private int FindRange()

        private void FindRange()
        {
            int start;
            int end;
            int length = FindRange(out start, out end);
            this._startOffset = start;
            this._endOffset = end;
            
            _innerEnumerator.Offset = this.StartOffset;
        }

        /// <summary>
        /// Returns the offset of the end of this style item based on the global length of the inner string enumerator.
        /// the inner enumerator is moved beyond any start marker (' or ") and the offset does not include the matching end marker.
        /// </summary>
        /// <returns></returns>
        private int FindRange(out int start, out int end)
        {
            
            //Make sure we have initialized the enumerator
            if (this.InnerEnumerator.Offset < 0)
                this.InnerEnumerator.MoveNext();

            start = this.InnerEnumerator.Offset;
            char escape = '\\';
            char last;

            if (this.InnerEnumerator.Current == '\'')
            {
                do
                {
                    end = this.InnerEnumerator.Offset;
                    last = this.InnerEnumerator.Current;
                }
                while (this.InnerEnumerator.MoveNext() && (this.InnerEnumerator.Current != '\'' || last == escape));

                start += 1;
            }
            else if (this.InnerEnumerator.Current == '"')
            {
                do
                {
                    end = this.InnerEnumerator.Offset;
                    last = this.InnerEnumerator.Current;
                }
                while (this.InnerEnumerator.MoveNext() && (this.InnerEnumerator.Current != '"' || last == escape));

                start += 1;
            }
            else
            {
                end = this.InnerEnumerator.Length - 1;
            }

            return end - start;
        }

        #endregion

        private bool CurrentIsWhiteSpace()
        {
            char c = this.InnerEnumerator.Current;
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
    }
}
