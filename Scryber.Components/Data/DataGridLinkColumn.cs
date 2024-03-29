﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("LinkColumn")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_datagridcolumn")]
    public class DataGridLinkColumn : DataGridColumn
    {

        #region public LinkAction Action {get;set;}

        private LinkAction _action = LinkAction.Undefined;
        /// <summary>
        /// Gets or sets the action type for this link. 
        /// If left undefined then the value will be (attempted to be) determined.
        /// </summary>
        [PDFAttribute("action")]
        [PDFDesignable("Action", Category = "Data", Priority = 1, Type = "Select")]
        public LinkAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        #endregion

        #region public string Destination {get;set;}

        private string _dest;

        /// <summary>
        /// Gets or sets the destination name or component (prefix with # to look for a component with the specidied ID, otherwise
        /// use the components name or unique id).
        /// </summary>
        [PDFAttribute("destination")]
        [PDFDesignable("Document Destination", Category = "Data", Priority = 1, Type = "String")]
        public string Destination
        {
            get { return _dest; }
            set { _dest = value; }
        }

        #endregion

        #region public string File {get;set;}

        private string _file;

        /// <summary>
        /// Gets or sets the path to the remote file
        /// </summary>
        [PDFAttribute("file")]
        [PDFDesignable("File or Url", Category = "Data", Priority = 2, Type = "String")]
        public string File
        {
            get { return _file; }
            set { _file = value; }
        }

        #endregion

        #region public OutlineFit DestinationFit {get;set;}

        private OutlineFit _destfit = OutlineFit.PageWidth;
        /// <summary>
        /// Gets or sets the fit for the destination (only for local links)
        /// </summary>
        [PDFAttribute("destination-fit")]
        [PDFDesignable("Fit To", Category = "Data", Priority = 2, Type = "Select")]
        public OutlineFit DestinationFit
        {
            get { return _destfit; }
            set { _destfit = value; }
        }

        #endregion

        #region public bool NewWindow {get;set;}

        private bool _newWindow;

        [PDFAttribute("new-window")]
        [PDFDesignable("New Window", Category = "Data", Priority = 3, Type = "Boolean")]
        public bool NewWindow
        {
            get { return _newWindow; }
            set { _newWindow = value; }
        }

        #endregion

        [PDFAttribute("text")]
        [PDFDesignable("Cell Text", Category = "Data", Priority = 1, Type = "String")]
        public string Text { get; set; }

        [PDFAttribute("img-src")]
        [PDFDesignable(Ignore = true)]
        public string ImageSource { get; set; }

        [PDFAttribute("img-data")]
        [PDFDesignable(Ignore = true)]
        public Base64Data ImageData { get; set; }


        public DataGridLinkColumn()
            : this((ObjectType)"DgLk")
        {
        }

        protected DataGridLinkColumn(ObjectType type)
            : base(type)
        {
        }

        public override Component DoBuildItemCell(TableGrid grid, TableRow row, int rowindex, int columnindex, DataContext context)
        {
            TableCell cell = (TableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);

            Link link = new Link();
            link.Action = this.Action;
            link.Destination = this.Destination;
            link.DestinationFit = this.DestinationFit;
            link.File = this.File;
            link.NewWindow = this.NewWindow;
            link.DataBinding += new DataBindEventHandler(link_DataBinding);
            cell.Contents.Add(link);

            return cell;
        }

        void link_DataBinding(object sender, DataBindEventArgs args)
        {
            this.DataBind(args.Context);

            Link link = (Link)sender;

            if (null != this._autobindItemPath)
            {
                string href = AssertGetDataItemValue(_autobindItemPath, args) as string;
                if (!string.IsNullOrEmpty(href))
                {
                    this.Action = LinkAction.Uri;
                    this.File = href;
                    this.Text = href;
                }

            }
            link.Action = this.Action;
            link.Destination = this.Destination;
            link.DestinationFit = this.DestinationFit;
            link.File = this.File;
            link.NewWindow = this.NewWindow;
            link.Visible = this.Visible;
            

            if (null != this.ImageData)
            {
                DataImage dimg = new DataImage();
                dimg.Data = this.ImageData;
                link.Contents.Add(dimg);
            }
            else if (!string.IsNullOrEmpty(this.ImageSource))
            {
                Image image = new Image();
                image.Source = this.ImageSource;
                link.Contents.Add(image);
            }
            else if(!string.IsNullOrEmpty(this.Text))
            {
                Label lbl = new Label();
                lbl.Text = this.Text;
                link.Contents.Add(lbl);
            }
        }

        private string _autobindItemPath;

        protected override void ApplyAutoBindingMember(DataItem item)
        {
            this._autobindItemPath = item.RelativePath;
        }
        
    }
}
