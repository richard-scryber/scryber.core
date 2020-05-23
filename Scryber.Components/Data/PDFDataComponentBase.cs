using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;

namespace Scryber.Data
{
    public abstract class PDFDataComponentBase : PDFComponent, IPDFDataComponent
    {

        public PDFDataComponentBase(PDFObjectType type) : base(type)
        {

        }

    }
}
