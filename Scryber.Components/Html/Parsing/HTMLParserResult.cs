using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Html.Parsing
{
    /// <summary>
    /// A lightweight reference to the last parsed component
    /// </summary>
    public struct HTMLParserResult
    {
        private bool _valid;
        private int _start;
        private int _end;
        private bool _isStart;
        private IComponent _parsed;
        private string _value;
        private HtmlComponentType _type;

        /// <summary>
        /// Returns true if this parser result actually corresponds to a valid component
        /// </summary>
        public bool Valid { get { return _valid; } }

        /// <summary>
        /// Gets the stating index in the content that represents the definition of this result.
        /// </summary>
        public int OffsetStart { get { return _start; } }

        /// <summary>
        /// Gets the ending index in the content that represents the definition of this result
        /// </summary>
        public int OffsetEnd { get { return _end; } }

        /// <summary>
        /// Returns true if this the start reference to the component
        /// </summary>
        public bool IsStart { get { return _isStart; } }

        /// <summary>
        /// Returns true if this is the end reference to the component
        /// </summary>
        public bool IsEnd { get { return !IsStart; } }

        /// <summary>
        /// Gets a reference to the component that has been parsed. If there is no component, then the Valid flag will be false.
        /// </summary>
        public IComponent Parsed { get { return _parsed; } }

        /// <summary>
        /// Gets the name of the tag for this result
        /// </summary>
        public string Value { get { return _value; } }

        public HtmlComponentType Type { get { return _type; } }

        /// <summary>
        /// Creates a new instance of of the ParserResult struct
        /// </summary>
        /// <param name="parsed"></param>
        /// <param name="offsetStart"></param>
        /// <param name="offsetEnd"></param>
        /// <param name="start"></param>
        public HTMLParserResult(IComponent parsed, HtmlComponentType type, string value, int offsetStart, int offsetEnd, bool start)
        {
            _value = value;
            _parsed = parsed;
            _type = type;
            _start = offsetStart;
            _end = offsetEnd;
            _isStart = start;
            _valid = true;
        }


        public override string ToString()
        {
            if (this.Valid && !string.IsNullOrEmpty(_value))
                return this._value;
            else
                return string.Empty;
        }

        private static HTMLParserResult _invalid = new HTMLParserResult();

        /// <summary>
        /// Returns an invalid empty Parser result value 
        /// </summary>
        public static HTMLParserResult Invalid
        {
            get { return _invalid; }
        }
    }
}
