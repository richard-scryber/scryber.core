using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("TemplateColumn")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_datagridcolumn")]
    public class PDFDataGridTemplateColumn : PDFDataGridColumn
    {

        #region public override bool HasHeader {get; set;}

        /// <summary>
        /// Overrides the default behaviour to return true if this column has a footer template, 
        /// otherwise returns the base value
        /// </summary>
        public override bool HasHeader
        {
            get
            {
                if (null != this.HeaderTemplate)
                    return true;
                else
                    return base.HasHeader;
            }
            set
            {
                base.HasHeader = value;
            }
        }

        #endregion

        #region public override bool HasFooter {get;set;}

        /// <summary>
        /// Overrides the default behaviour to return true if this column has a footer template, 
        /// otherwise returns the base value
        /// </summary>
        public override bool HasFooter
        {
            get
            {
                if (null != this.FooterTemplate)
                    return true;
                else
                    return base.HasFooter;
            }
            set
            {
                base.HasFooter = value;
            }
        }

        #endregion

        //
        // template properties
        //

        [PDFTemplate()]
        [PDFElement("ItemTemplate")]
        public IPDFTemplate ItemTemplate { get; set; }

        [PDFTemplate()]
        [PDFElement("AlternatingItemTemplate")]
        public IPDFTemplate AlternatingItemTemplate { get; set; }

        [PDFTemplate()]
        [PDFElement("HeaderTemplate")]
        public IPDFTemplate HeaderTemplate { get; set; }

        [PDFTemplate()]
        [PDFElement("FooterTemplate")]
        public IPDFTemplate FooterTemplate { get; set; }

        //
        // .ctor
        //

        public PDFDataGridTemplateColumn()
            : this((PDFObjectType)"DgTm")
        {
        }

        protected PDFDataGridTemplateColumn(PDFObjectType type)
            : base(type)
        {
        }


        //
        // methods
        //

        protected override void ApplyAutoBindingMember(PDFDataItem item)
        {
            throw new NotSupportedException("AutoBinding onto a template column is not suported");
        }

        #region public override PDFComponent DoBuildHeaderCell(Components.PDFTableGrid grid, Components.PDFTableRow row, int rowindex, int columnindex, PDFDataContext context)

        /// <summary>
        /// Overrides base implmentation to instantiate the header template, if defined, and add to the columns cell.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="rowindex"></param>
        /// <param name="columnindex"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override PDFComponent DoBuildHeaderCell(PDFTableGrid grid, PDFTableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            PDFComponent comp = base.DoBuildHeaderCell(grid, row, rowindex, columnindex, context);
            if (null != this.HeaderTemplate)
            {
                this.AddTemplateToContainer((IPDFContainerComponent)comp, APPEND, this.HeaderTemplate);
            }
            return comp;
        }

        #endregion

        #region public override PDFComponent DoBuildItemCell(Components.PDFTableGrid grid, Components.PDFTableRow row, int rowindex, int columnindex, PDFDataContext context)

        /// <summary>
        /// Overrides base implmentation to instantiate either the alternating or item template and add to the columns cell.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="rowindex"></param>
        /// <param name="columnindex"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override PDFComponent DoBuildItemCell(PDFTableGrid grid, PDFTableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            PDFComponent comp = base.DoBuildItemCell(grid, row, rowindex, columnindex, context);

            if (rowindex % 2 == 1 && null != this.AlternatingItemTemplate)
            {
                this.AddTemplateToContainer((IPDFContainerComponent)comp, APPEND, this.AlternatingItemTemplate);
            }
            else if (null != this.ItemTemplate)
            {
                this.AddTemplateToContainer((IPDFContainerComponent)comp, APPEND, this.ItemTemplate);
            }

            return comp;
        }

        #endregion

        #region public override PDFComponent DoBuildFooterCell(Components.PDFTableGrid grid, Components.PDFTableRow row, int rowindex, int columnindex, PDFDataContext context)

        /// <summary>
        /// Overrides base implmentation to instantiate the footer template, if defined, and add to the columns cell.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="rowindex"></param>
        /// <param name="columnindex"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override PDFComponent DoBuildFooterCell(PDFTableGrid grid, PDFTableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            PDFComponent comp = base.DoBuildFooterCell(grid, row, rowindex, columnindex, context);
            if (null != this.FooterTemplate)
            {
                this.AddTemplateToContainer((IPDFContainerComponent)comp, APPEND, this.FooterTemplate);
            }
            return comp;
        }

        #endregion

        //
        // implementation
        //

        #region protected int AddTemplateToContainer(IPDFContainerComponent container, int index, IPDFTemplate template)

        private const int APPEND = -1; //mark the index so it appends to the container

        protected int AddTemplateToContainer(IPDFContainerComponent container, int index, IPDFTemplate template)
        {
            int count = 0;
            IEnumerable<IPDFComponent> generated = template.Instantiate(index, container);
            foreach (IPDFComponent comp in generated)
            {
                PDFComponent actual = comp as PDFComponent;
                if (null != actual)
                {
                    if (index >= 0 && index < container.Content.Count)
                        container.Content.Insert(index, actual);
                    else
                        container.Content.Add(actual);
                    count++;
                }
                if(comp is IPDFBindableComponent)
                    ((IPDFBindableComponent)comp).DataBinding += PDFDataGridTemplateColumn_DataBinding;
            }
            return count;
        }

        void PDFDataGridTemplateColumn_DataBinding(object sender, PDFDataBindEventArgs e)
        {
            ((PDFComponent)sender).Visible = this.Visible;
        }

        

        #endregion


    }
}
