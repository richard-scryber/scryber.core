using System;
using Scryber.Drawing;

namespace Scryber.PDF.Layout
{
    public partial class LayoutEngineTable : LayoutEngineBase
    {
        /// <summary>
        /// Holds a single dimension for a cell
        /// </summary>
        protected struct CellDimension
        {
            private Unit _sz;
            private bool _explicit;

            public Unit Size { get { return _sz; } set { _sz = value; } }
            public bool Explicit { get { return _explicit; } set { _explicit = value; } }

            public CellDimension(Unit size)
                : this(size, true)
            {
            }

            public CellDimension(Unit size, bool isExplicit)
            {
                _sz = size;
                _explicit = isExplicit;
            }

            public override string ToString()
            {
                return this.Explicit ? _sz.ToString() + " Explicit" : _sz.ToString();
            }
        }
    }
}