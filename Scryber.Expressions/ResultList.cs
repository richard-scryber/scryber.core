using System;
using System.Collections;
using System.Text;

namespace Scryber
{
	/// <summary>
	/// A standard array list collection, but the ToString method returns all the contents as their string representation.
	/// </summary>
	public class ResultList : System.Collections.ArrayList
	{
		public ResultList()
		{
		}

		public ResultList(ICollection content) : base(content)
		{ }

		public ResultList(int capacity): base(capacity)
		{

		}

        public override string ToString()
        {
			StringBuilder sb = new StringBuilder();
			bool first = true;

			foreach(var item in this)
			{
				if (!first)
					sb.Append(";");

				sb.Append(item);
				first = false;

			}
            return sb.ToString();
        }
    }
}

