using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;

namespace Scryber.Data
{
    public class DataComponentList : ComponentWrappingList<DataComponentBase>
    {

        public DataComponentList(Document owner) : this(new ComponentList(owner, PDFObjectTypes.XmlData))
        {
        }

        public DataComponentList(ComponentList inner)
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
            foreach (IComponent comp in this)
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
            foreach (IComponent comp in this)
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
            IComponent[] all = this.ToArray();

            for (int i = 0; i < all.Length; i++)
            {
                IComponent comp = all[i];
                if (comp is IBindableComponent)
                    ((IBindableComponent)comp).DataBind(context);
            }
        }

        #endregion

        protected override void BuildAllItems(ComponentList content, List<DataComponentBase> found)
        {
            foreach (Component comp in content)
            {
                if (comp is IPDFInvisibleContainer)
                {
                    IPDFInvisibleContainer container = comp as IPDFInvisibleContainer;
                    if (container.HasContent)
                        this.BuildAllItems(container.Content, found);
                }
                else if (comp is DataComponentBase)
                    found.Add(comp as DataComponentBase);

            }
        }
    }
}
