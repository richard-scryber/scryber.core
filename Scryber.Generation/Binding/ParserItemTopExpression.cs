﻿using System;
namespace Scryber.Binding
{
    public class ParserItemTopExpression : ParserItemExpression
    {
        public string ItemName { get; private set; }

        public ParserItemTopExpression(string item)
        {
            this.ItemName = item;
        }

        protected override object DoGetMyValue(object parent, PDFDataContext context)
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
                PDFItemCollection items = (PDFItemCollection)parent;
                return items[this.ItemName];
            }
        }

    }
}
