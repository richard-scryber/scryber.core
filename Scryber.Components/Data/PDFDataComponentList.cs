using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;

namespace Scryber.Data
{
    public class PDFDataComponentList : PDFComponentWrappingList<PDFDataComponentBase>
    {

        public PDFDataComponentList(PDFDocument owner) : this(new PDFComponentList(owner, PDFObjectTypes.XmlData))
        {
        }

        public PDFDataComponentList(PDFComponentList inner)
            : base(inner)
        {

        }


        #region public void Init(PDFInitContext context)

        /// <summary>
        /// Initializes each of the components in this list
        /// </summary>
        /// <param name="context"></param>
        public void Init(PDFInitContext context)
        {
            foreach (IPDFComponent comp in this)
            {
                comp.Init(context);
            }
        }

        #endregion

        #region public void Load(PDFLoadContext context)

        /// <summary>
        /// Invokes the load on each of the components in this list
        /// </summary>
        /// <param name="context"></param>
        public void Load(PDFLoadContext context)
        {
            foreach (IPDFComponent comp in this)
            {
                comp.Load(context);
            }
        }

        #endregion

        #region public void DataBind(PDFDataContext context)

        /// <summary>
        /// Invokes the data binding on each of the components in this list
        /// that implement the IPDFBindable interface
        /// </summary>
        /// <param name="context"></param>
        public void DataBind(PDFDataContext context)
        {
            IPDFComponent[] all = this.ToArray();

            for (int i = 0; i < all.Length; i++)
            {
                IPDFComponent comp = all[i];
                if (comp is IPDFBindableComponent)
                    ((IPDFBindableComponent)comp).DataBind(context);
            }
        }

        #endregion

        protected override void BuildAllItems(PDFComponentList content, List<PDFDataComponentBase> found)
        {
            foreach (PDFComponent comp in content)
            {
                if (comp is IPDFInvisibleContainer)
                {
                    IPDFInvisibleContainer container = comp as IPDFInvisibleContainer;
                    if (container.HasContent)
                        this.BuildAllItems(container.Content, found);
                }
                else if (comp is PDFDataComponentBase)
                    found.Add(comp as PDFDataComponentBase);

            }
        }
    }
}
