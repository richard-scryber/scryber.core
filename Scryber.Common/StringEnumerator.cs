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
                else if (_offset >= _text.Length)
                    throw new InvalidOperationException("The enumerator has moved pat the end of the string");

                return _text[_offset];
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
        public bool EOS { get { return _offset >= _text.Length; } }

        /// <summary>
        /// Gets the length of the entire string this enumerator uses
        /// </summary>
        public int Length { get { return _text.Length; } }

        /// <summary>
        /// Gets the entire string of this enumerator
        /// </summary>
        public string InnerString { get { return _text; } }


        //
        // .ctor(s)
        //

        /// <summary>
        /// Creates a new instance that will enumerate over the specified text
        /// </summary>
        /// <param name="text"></param>
        public StringEnumerator(string text)
        {
            _text = text;
            _offset = -1;
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
            return _offset < _text.Length;
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
            return this._text[this.Offset + delta];
        }

        /// <summary>
        /// Gets a substring of the entire text,
        /// starting at the current offset and with the specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string Substring(int length)
        {
            return this._text.Substring(this._offset, length);
        }

        /// <summary>
        /// Gets a substring of the entire text from the specified offset, 
        /// with the specified length
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string Substring(int offset, int length)
        {
            return this._text.Substring(offset, length);
        }


        public bool Matches(string pattern)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (_text[_offset + i] != pattern[i])
                    return false;
            }
            return true;
        }
    }
}
