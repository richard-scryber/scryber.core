using System;
namespace Scryber.Binding
{
    public class ParserItemTopExpression : ParserItemExpression
    {
        public string ItemName { get; private set; }

        public ParserItemTopExpression(string item)
        {
            this.ItemName = item;
        }

        protected override object DoGetMyValue(object parent, DataContext context)
        {
            if (this.ItemName == CurrentDataContextName)
            {
                if (context.DataStack.HasData)
                    return context.DataStack.Current;
                else
                    return null;
            }
            else
            {
                ItemCollection items = (ItemCollection)parent;
                return items[this.ItemName];
            }
        }

    }
}
