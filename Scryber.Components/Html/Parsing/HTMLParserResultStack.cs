using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Html.Parsing
{
    /// <summary>
    /// 
    /// </summary>
    public class HTMLParserResultStack
    {

        #region protected List<ParserResult> InnerList

        private List<HTMLParserResult> _inner;

        /// <summary>
        /// Gets the inner list of items in this stack
        /// </summary>
        protected List<HTMLParserResult> InnerList
        {
            get { return _inner; }
        }

        #endregion

        #region public int Count {get;}

        /// <summary>
        /// Gets the number of items in this stack
        /// </summary>
        public int Count
        {
            get { return this.InnerList.Count; }
        }

        #endregion

        public HTMLParserResultStack()
        {
            _inner = new List<HTMLParserResult>();
        }

        public int Push(HTMLParserResult result)
        {
            int index = this.InnerList.Count;
            this.InnerList.Add(result);
            return index;
        }


        public HTMLParserResult Peek()
        {
            return this.Peek(this.Count - 1);
        }

        public HTMLParserResult Peek(int index)
        {
            return this.InnerList[index];
        }

        public void Clear()
        {
            this.InnerList.Clear();
        }

        
        public bool Contains(string value)
        {
            return IndexOf(value) >= 0;
        }


        
        public int IndexOf(string name)
        {
            for (int i = this.InnerList.Count - 1; i >= 0; i--)
            {
                HTMLParserResult item = this.InnerList[i];
                if (item.Valid && (name == item.Value))
                {
                    return i;
                }
            }
            return -1;
        }


        public HTMLParserResult Pop()
        {
            int index = this.Count - 1;
            if (index < 0)
                throw new InvalidOperationException("Empty Stack");

            HTMLParserResult value = this.InnerList[index];
            this.InnerList.RemoveAt(index);
            return value;
        }


        public HTMLParserResult[] PopToTag(string name)
        {
            int index = this.IndexOf(name);
            if (index < 0)
                throw new IndexOutOfRangeException("A result with the specified type (and optional value) does not exist in this stack");

            HTMLParserResult[] all = new HTMLParserResult[this.Count - index];

            int pos = all.Length - 1;

            while (this.Count > index)
            {
                all[pos] = this.Pop();
                pos--;
            }
            return all;

        }

        public void PushRange(IEnumerable<HTMLParserResult> items)
        {
            this.InnerList.AddRange(items);
        }
    }
}
