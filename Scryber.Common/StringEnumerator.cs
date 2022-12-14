using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber
{
    /// <summary>
    /// Enumerates over a single string allowing forward and backward movement
    /// </summary>
    public class StringEnumerator
    {

        #region ivars

        private string _text;
        private int _offset;
        private int _startIndex;
        private int _length;

        #endregion


        //
        // properties
        //

        /// <summary>
        /// Gets the current character at this enumerators offset
        /// </summary>
        public char Current
        {
            get
            {
                if (_offset < 0)
                    throw new InvalidOperationException("This string enumerator has not yet moved onto the first character. Always call MoveNext atleast once before accessing any character.");
                else if (_offset >= _length)
                    throw new InvalidOperationException("The enumerator has moved pat the end of the string");

                return _text[_offset + _startIndex];
            }
        }

        /// <summary>
        /// Gets or sets the current offset in the text for this enumerator
        /// </summary>
        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        /// <summary>
        /// End Of String - returns true if this enumerator has passed the end of the string.
        /// </summary>
        public bool EOS { get { return _offset >= _length; } }

        /// <summary>
        /// Gets the length of the entire string this enumerator uses
        /// </summary>
        public int Length { get { return _length; } }

        /// <summary>
        /// Gets the entire string of this enumerator
        /// </summary>
        //public string InnerString { get { return _text; } }


        //
        // .ctor(s)
        //

        /// <summary>
        /// Creates a new instance that will enumerate over the specified text
        /// </summary>
        /// <param name="text"></param>
        public StringEnumerator(string text)
            : this(text, 0, text.Length)
        {
            
        }

        public StringEnumerator(StringEnumerator baseString, int startIndex, int length)
            : this(baseString._text, baseString._startIndex + startIndex, length)
        {

        }


        public StringEnumerator(string text, int startIndex, int length)
        {
            _text = text;
            _offset = - 1;
            _startIndex = startIndex;
            _length = length;
        }

        
        //
        // methods
        //

        /// <summary>
        /// Moves the cursor onto the next character - returning true if there is one, 
        /// or false if we are past the end of the string
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            _offset++;
            return _offset < _length;
        }

        /// <summary>
        /// Moves the cursor onto the next character - returning true if there is one, 
        /// or false if we are past the end of the string.
        /// </summary>
        /// <param name="next">Set to the current character after moving next or \0 if could not move beyond EOS</param>
        /// <returns></returns>
        public bool MoveNext(out char next)
        {
            bool result = MoveNext();
            if (result)
                next = this.Current;
            else
                next = '\0';

            return result;
        }

        /// <summary>
        /// Moves the cursor onto the previous character - returning true if there is one,
        /// or false if we are before the start of the string.
        /// </summary>
        /// <returns></returns>
        public bool MovePrev()
        {
            _offset--;
            return _offset >= 0;
        }

        /// <summary>
        /// Resets this enumerator to the initial position
        /// </summary>
        public void Reset()
        {
            _offset = -1;
        }

        /// <summary>
        /// Peeks at the character before or after 
        /// the current character by the specified
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public char Peek(int delta)
        {
            return this._text[this.Offset + this._startIndex + delta];
        }

        /// <summary>
        /// Gets a substring of the entire text,
        /// starting at the current offset and with the specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string Substring(int startIndex)
        {
            return this._text.Substring(this._startIndex + startIndex, this._length - startIndex);
        }

        /// <summary>
        /// Gets a substring of the entire text from the specified offset, 
        /// with the specified length
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string Substring(int startIndex, int length)
        {
            return this._text.Substring(_startIndex + startIndex, length);
        }


        public override string ToString()
        {
            return this._text.Substring(this._startIndex, this._length);
        }


        public bool Matches(string pattern)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (_text[_offset + _startIndex + i] != pattern[i])
                    return false;
            }
            return true;
        }
    }
}
