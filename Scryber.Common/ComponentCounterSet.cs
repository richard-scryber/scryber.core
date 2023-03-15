using System;


namespace Scryber
{
	public class ComponentCounter
	{
		public string Name { get; private set; }

		public int Value { get; set; }

		public ComponentCounter Next { get; set; }

		public ComponentCounter(string name, int value = 0)
		{
			this.Name = name;
			this.Value = value;
		}

	}


	public class ComponentCounterSet
	{

        public const int UndefinedValue = 0;

		private ComponentCounter _root = null;
        private IComponent _owner;

        public IComponent Owner
        {
            get { return this._owner; }
        }

        public ComponentCounter Root
        {
            get { return _root; }
        }


        public int Count
        {
            get
            {
                var curr = _root;
                var index = 0;

                while (curr != null)
                {
                    index++;
                    curr = curr.Next;
                }

                return index;

            }
        }

        public ComponentCounterSet(IComponent owner)
        {
            this._owner = owner;
        }

        protected ComponentCounter GetCounter(string name, bool create)
        {
            var curr = _root;

            while (curr != null)
            {
                if (curr.Name == name)
                    return curr;

                else
                    curr = curr.Next;
            }

            //does not exist

            if (create)
            {
                curr = new ComponentCounter(name);
                curr.Next = _root;
                _root = curr;
            }

            return curr;
        }

        public int Value(string name)
        {
            var found = GetCounter(name, false);

            if (null == found)
            {
                return UndefinedValue;
            }
            else
            {
                return found.Value;
            }
        }

        public bool TryGetValue(string name, out int value)
        {
            var found = GetCounter(name, false);
            if (null != found)
            {
                value = found.Value;
                return true;
            }
            else
            {
                value = 0;
                return false;
            }
        }

        public int Increment(string name, int value = 1)
        {
            var found = GetCounter(name, false);

            if (null == found)
            {
                return UndefinedValue;
            }
            else
            {
                found.Value += value;
                return found.Value;
            }
        }


        public int Reset(string name, int toValue = 0)
        {
            var found = GetCounter(name, true);

            if (null == found)
            {
                throw new InvalidOperationException("The returned counter should not be null");
            }
            else
            {
                found.Value = toValue;
                return found.Value;
            }
        }
    }
}

