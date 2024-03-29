﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("TemplateColumn")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_datagridcolumn")]
    public class DataGridTemplateColumn : DataGridColumn
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
        [PDFAttribute("item-template")]
        [PDFElement("ItemTemplate")]
        public ITemplate ItemTemplate { get; set; }

        [PDFTemplate()]
        [PDFAttribute("alternate-item-template")]
        [PDFElement("AlternatingItemTemplate")]
        public ITemplate AlternatingItemTemplate { get; set; }

        [PDFTemplate()]
        [PDFAttribute("header-template")]
        [PDFElement("HeaderTemplate")]
        public ITemplate HeaderTemplate { get; set; }

        [PDFTemplate()]
        [PDFAttribute("footer-template")]
        [PDFElement("FooterTemplate")]
        public ITemplate FooterTemplate { get; set; }

        //
        // .ctor
        //

        public DataGridTemplateColumn()
            : this((ObjectType)"DgTm")
        {
        }

        protected DataGridTemplateColumn(ObjectType type)
            : base(type)
        {
        }


        //
        // methods
        //

        protected override void ApplyAutoBindingMember(DataItem item)
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
        public override Component DoBuildHeaderCell(TableGrid grid, TableRow row, int rowindex, int columnindex, DataContext context)
        {
            Component comp = base.DoBuildHeaderCell(grid, row, rowindex, columnindex, context);
            if (null != this.HeaderTemplate)
            {
                this.AddTemplateToContainer((IContainerComponent)comp, APPEND, this.HeaderTemplate);
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
        public override Component DoBuildItemCell(TableGrid grid, TableRow row, int rowindex, int columnindex, DataContext context)
        {
            Component comp = base.DoBuildItemCell(grid, row, rowindex, columnindex, context);

            if (rowindex % 2 == 1 && null != this.AlternatingItemTemplate)
            {
                this.AddTemplateToContainer((IContainerComponent)comp, APPEND, this.AlternatingItemTemplate);
            }
            else if (null != this.ItemTemplate)
            {
                this.AddTemplateToContainer((IContainerComponent)comp, APPEND, this.ItemTemplate);
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
        public override Component DoBuildFooterCell(TableGrid grid, TableRow row, int rowindex, int columnindex, DataContext context)
        {
            Component comp = base.DoBuildFooterCell(grid, row, rowindex, columnindex, context);
            if (null != this.FooterTemplate)
            {
                this.AddTemplateToContainer((IContainerComponent)comp, APPEND, this.FooterTemplate);
            }
            return comp;
        }

        #endregion

        //
        // implementation
        //

        #region protected int AddTemplateToContainer(IPDFContainerComponent container, int index, IPDFTemplate template)

        private const int APPEND = -1; //mark the index so it appends to the container

        protected int AddTemplateToContainer(IContainerComponent container, int index, ITemplate template)
        {
            int count = 0;
            IEnumerable<IComponent> generated = template.Instantiate(index, container);
            foreach (IComponent comp in generated)
            {
                Component actual = comp as Component;
                if (null != actual)
                {
                    if (index >= 0 && index < container.Content.Count)
                        container.Content.Insert(index, actual);
                    else
                        container.Content.Add(actual);
                    count++;
                }
                if(comp is IBindableComponent)
                    ((IBindableComponent)comp).DataBinding += PDFDataGridTemplateColumn_DataBinding;
            }
            return count;
        }

        void PDFDataGridTemplateColumn_DataBinding(object sender, DataBindEventArgs e)
        {
            ((Component)sender).Visible = this.Visible;
        }

        

        #endregion


    }
}
