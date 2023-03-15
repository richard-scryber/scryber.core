using System;
namespace Scryber.Styles
{
	[PDFParsableValue]
	public class CounterStyleValue
	{

		public string Name { get; private set; }

		public int Value { get; set; }

		public CounterStyleValue Next { get; set; }

		public CounterStyleValue(string name, int value = 0)
		{
			this.Name = name;
			this.Value = value;
		}

		public void Append(CounterStyleValue value)
		{
			if (null == this.Next)
				this.Next = value;
			else
				this.Next.Append(value);
		}

        public override string ToString()
        {
			var val = this.Name + ":" + this.Value;
			if (null != this.Next)
			{
				val += " -> " + this.Next.ToString();
			}
			return val;
        }
        private static readonly char[] whiteSpace = new char[] { ' ', '\r', '\n' };

		public static CounterStyleValue Parse(string full)
		{
			return Parse(full, 0);
		}

        public static CounterStyleValue Parse(string full, int defaultValue)
		{
			if (string.IsNullOrEmpty(full))
				return null;

			full = full.Trim();
			if (full.IndexOfAny(whiteSpace) > 0)
			{
				var all = full.Split(whiteSpace);
				CounterStyleValue root = null;
				CounterStyleValue last = null;

				foreach (var one in all)
				{
					var name = one.Trim();

					if (!string.IsNullOrEmpty(name))
					{
						int value;
						if (int.TryParse(name, out value))
						{
							if (null != last)
								last.Value = value;
						}
						else if (name.StartsWith("reversed("))
						{
							throw new NotSupportedException("Reveresed counters are not currently suppported");
						}
						else
						{
							CounterStyleValue counter = new CounterStyleValue(name, defaultValue);

							if (null == root)
							{
								root = counter;
							}
							else
								last.Next = counter;

							last = counter;
						}
					}
				}
				return root;
			}
			else //just a single name
			{
				var name = full.Trim();
				int value;
				if (int.TryParse(full, out value))
				{
					throw new ArgumentOutOfRangeException("The counter value must specify a name before a value can be set");
				}
				else if (name.StartsWith("reversed("))
				{
					throw new NotSupportedException("Reveresed counters are not currently suppported");
				}
				else
				{
					return new CounterStyleValue(name, defaultValue);
				}
			}
		}
	}
}

