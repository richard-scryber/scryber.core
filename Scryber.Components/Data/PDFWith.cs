using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("With")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withItem")]
    public class PDFWith : PDFWithBase
    {

        #region public override string SelectPath {get;set;}

        /// <summary>
        /// Gets or sets the select expression for the 
        /// </summary>
        [PDFAttribute("select")]
        [PDFDesignable("Select", Priority = 2, Category = "Data", Type = "SelectDataPicker", JSOptions = "{\"context\":\"@datasource-id\",\"type\":\"Collection\"}")]
        public override string SelectPath
        {
            get => base.SelectPath;
            set => base.SelectPath = value;
        }

        #endregion

        #region public override string DataSourceID {get;set;}

        [PDFAttribute("datasource-id")]
        [PDFDesignable("Data Source", Category = "Data", Priority = 2, Type = "SelectDataSource")]
        public override string DataSourceID
        {
            get => base.DataSourceID;
            set => base.DataSourceID = value;
        }

        #endregion

        


        //
        // ctor
        //

        public PDFWith()
            : this(PDFObjectTypes.DataWith)
        {
        }

        public PDFWith(PDFObjectType type)
            : base(type)
        {
        }

        protected override void DoDataBind(PDFDataContext context, bool includeChildren)
        {
            base.DoDataBind(context, includeChildren);
            return;

        //    this.RemoveAnyExistingContent();

        //    object data = null;
        //    bool hasdata = false;
        //    IPDFDataSource source;

        //    System.Xml.IXmlNamespaceResolver resolver = context.NamespaceResolver;

        //    if (this.HasAssignedDataSourceComponent(context, out source))
        //    {
        //        data = source.Select(this.SelectPath, context);
        //        if (null == data)
        //            context.TraceLog.Add(TraceLevel.Warning, "Data With", string.Format("NULL data was returned for the path '{0}' on the PDFWith component {1} with data source {2}", this.SelectPath, this.ID, source.ID));
        //        else
        //            hasdata = true;
        //    }
        //    else if (string.IsNullOrEmpty(this.SelectPath) == false)
        //    {
        //        source = context.DataStack.Source;
        //        data = source.Select(this.SelectPath, context.DataStack.Current, context);
        //        if (null == data)
        //            context.TraceLog.Add(TraceLevel.Warning, "Data With", string.Format("NULL data was returned for the path '{0}' on the PDFWith component {1}", this.SelectPath, this.ID));
        //        else
        //        {
        //            hasdata = true;
        //        }
        //    }
        //    else if (!context.DataStack.HasData)
        //    {
        //        context.TraceLog.Add(TraceLevel.Warning, "Data With", string.Format("There is no current data for the PDFWith component {0}", this.ID));
        //    }

        //    if (hasdata)
        //    {
        //        data = GetCurrentOrFirstItem(data);
        //        if (null == data)
        //            hasdata = false;
        //        else
        //            context.DataStack.Push(data, source);
        //    }

        //    base.DoDataBind(context, includeChildren);

        //    if (hasdata && includeChildren)
        //    {
        //        int index;
        //        IPDFContainerComponent container = GetContainerParent(out index);
        //        DoBindDataIntoContainer(container, index, context);
        //    }

        //    if (hasdata)
        //        context.DataStack.Pop();

        //    context.NamespaceResolver = resolver;
        //}

        ///// <summary>
        ///// Removes any existing bound contents created by this grid
        ///// </summary>
        //protected virtual void RemoveAnyExistingContent()
        //{
        //    if (null != _built && _built.Parent != null)
        //    {
        //        ((IPDFContainerComponent)_built.Parent).Content.Remove(_built);
        //    }
        //}

        //protected override object GetCurrentOrFirstItem(object data)
        //{
        //    if (data is System.Xml.XPath.XPathNodeIterator)
        //    {
        //        System.Xml.XPath.XPathNodeIterator itter = (System.Xml.XPath.XPathNodeIterator)data;
        //        if (itter.MoveNext())
        //            return itter.Current;
        //        else
        //            return null;
        //    }
        //    else if (data is Array)
        //    {
        //        Array all = (Array)data;
        //        if (all.Length > 0)
        //            return all.GetValue(0);
        //        else
        //            return null;
        //    }
        //    else if (data is System.Collections.IList)
        //    {
        //        System.Collections.IList col = (System.Collections.IList)data;
        //        return col[0];
        //    }
        //    else
        //        return data;
        //}

        //protected IPDFContainerComponent GetContainerParent(out int index)
        //{
        //    PDFComponent ele = this;
        //    while (null != ele)
        //    {
        //        PDFComponent par = ele.Parent;
        //        if (par == null)
        //            throw RecordAndRaise.ArgumentNull(Errors.TemplateComponentParentMustBeContainer);

        //        else if ((par is IPDFContainerComponent) == false)
        //            ele = par;
        //        else
        //        {
        //            IPDFContainerComponent container = par as IPDFContainerComponent;
        //            index = container.Content.IndexOf(ele);
        //            return container;
        //        }
        //    }

        //    //If we get this far then we haven't got a viable container to add our items to.
        //    throw RecordAndRaise.ArgumentNull(Errors.TemplateComponentParentMustBeContainer);
        }

        protected override void DoDataBindChildren(PDFDataContext context)
        {
            base.DoDataBindChildren(context);
        }

        



        
    }
}
