using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Data
{
    /// <summary>
    /// An abstract data bound component for a single item. The current context is set to the data in th
    /// </summary>
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withItem")]
    public abstract class PDFWithBase : Scryber.Components.PDFVisualComponent, IPDFViewPortComponent, IPDFInvisibleContainer
    {

        public virtual string SelectPath { get; set; }

        public virtual string DataSourceID { get; set; }

        public virtual object Data { get; set; }


        #region public PDFComponentList Contents {get;}

        /// <summary>
        /// Gets the content collection of page Components in this panel
        /// </summary>
        [PDFArray(typeof(PDFComponent))]
        [PDFElement()]
        public virtual PDFComponentList Contents
        {
            get
            {
                return this.InnerContent;
            }
        }

        #endregion

        public PDFWithBase(PDFObjectType type)
            :base(type)
        {
        }

        protected override void DoDataBindChildren(PDFDataContext context)
        {
            bool setup = this.DoSetupWithData(context);

            base.DoDataBindChildren(context);
            this.DoBindDataIntoContainer(this, this.Contents.Count, context);

            this.DoTearDownWithData(setup, context);
        }

        //
        // Implementation
        //

        #region protected IPDFDataSource GetDataSourceComponent(string datasourceComponentID, PDFDataContext context)

        /// <summary>
        /// Looks up and returns the IPDFDataSource based on the provided datasourceComponentID. Throws an expcetion if the datasourceComponentID is null or the component cannot be found
        /// </summary>
        /// <param name="datasourceComponentID">The ID of the IPDFDataSource</param>
        /// <param name="context">The current data context</param>
        /// <returns></returns>
        protected IPDFDataSource GetDataSourceComponent(string datasourceComponentID, PDFDataContext context)
        {
            IPDFDataSource datasourceComponent = null;

            if (string.IsNullOrEmpty(datasourceComponentID))
                throw new ArgumentNullException("datasourceComponentID");

            PDFComponent found = base.FindDocumentComponentById(datasourceComponentID);
            if (found == null)
                throw RecordAndRaise.ArgumentNull("DataSourceID", Errors.CouldNotFindControlWithID, datasourceComponentID);
            else if (!(found is IPDFDataSource))
                throw RecordAndRaise.Argument("DataSourceID", Errors.AssignedDataSourceIsNotIPDFDataSource, datasourceComponentID);
            else
                datasourceComponent = ((IPDFDataSource)found);


            return datasourceComponent;
        }

        #endregion

        #region protected bool HasAssignedDataSourceComponent(PDFDataContext context, out IPDFDataSource datasourceComponent)

        /// <summary>
        /// If this component has a specified datasourceId then the component will be loaded and set to the out parameter. The method will then return true.
        /// If this component does not have a datasourceId then method will return false.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="datasourceComponent"></param>
        /// <returns></returns>
        protected bool HasAssignedDataSourceComponent(PDFDataContext context, out IPDFDataSource datasourceComponent)
        {
            if (string.IsNullOrEmpty(this.DataSourceID) == false)
            {
                datasourceComponent = GetDataSourceComponent(this.DataSourceID, context);
                return null != datasourceComponent;
            }
            else
            {
                datasourceComponent = null;
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="containerposition"></param>
        /// <param name="template"></param>
        /// <param name="context"></param>
        protected virtual void DoBindDataIntoContainer(IPDFContainerComponent container, int containerposition, PDFDataContext context)
        {

        }

        #region protected virtual bool DoSetupWithData(PDFDataContext context)

        /// <summary>
        /// Holds a references to any namspaces that can be restored after binding
        /// </summary>
        System.Xml.IXmlNamespaceResolver lastResolver;

        /// <summary>
        /// Sets up the context with any required data
        /// </summary>
        /// <param name="context">The context to set up</param>
        /// <returns>True if new data was added to the context</returns>
        protected virtual bool DoSetupWithData(PDFDataContext context)
        {
            bool hasdata = false;

            lastResolver = context.NamespaceResolver;
            IPDFDataSource datasource;
            object data = null;

            //Get the datasource
            if(null != this.Data)
            {
                data = this.Data;
                datasource = context.DataStack.HasData ? context.DataStack.Source : null;
                hasdata = true;
            }
            else if (this.HasAssignedDataSourceComponent(context, out datasource))
            {
                data = datasource.Select(this.SelectPath, context);
                if (null == data)
                    context.TraceLog.Add(TraceLevel.Warning, "Data With", string.Format("NULL data was returned for the path '{0}' on the PDFWith component {1} with datasource {2}", this.SelectPath, this.ID, datasource.ID));
                hasdata = true;
            }
            else if (string.IsNullOrEmpty(this.SelectPath) == false)
            {
                datasource = context.DataStack.Source;
                data = datasource.Select(this.SelectPath, context.DataStack.Current, context);
                if (null == data)
                    context.TraceLog.Add(TraceLevel.Warning, "Data For Each", string.Format("NULL data was returned for the path '{0}' on the PDFForEach component {1} with data source {2}", this.SelectPath, this.ID, datasource.ID));
                hasdata = true;
            }
            else if (!context.DataStack.HasData)
            {
                context.TraceLog.Add(TraceLevel.Warning, "Data With", string.Format("There is no current data for the PDFWith component {0}", this.ID));
            }
            else
            {
                data = context.DataStack.Current;
                datasource = context.DataStack.Source;
            }

            if(hasdata)
            {
                data = GetCurrentOrFirstItem(data);
                if (null == data)
                    hasdata = false;
                else
                    context.DataStack.Push(data, datasource);
            }

            return hasdata;
        }

        #endregion

        protected virtual object GetCurrentOrFirstItem(object data)
        {
            if (data is System.Xml.XPath.XPathNodeIterator)
            {
                System.Xml.XPath.XPathNodeIterator itter = (System.Xml.XPath.XPathNodeIterator)data;
                if (itter.MoveNext())
                    return itter.Current;
                else
                    return null;
            }
            else if (data is Array)
            {
                Array all = (Array)data;
                if (all.Length > 0)
                    return all.GetValue(0);
                else
                    return null;
            }
            else if (data is System.Collections.IList)
            {
                System.Collections.IList col = (System.Collections.IList)data;
                return col[0];
            }
            else
                return data;
        }

        #region protected virtual void DoTearDownWithData(bool hasData, PDFDataContext context)

        /// <summary>
        /// Removes any data that was set up in the context.
        /// </summary>
        /// <param name="hasData"></param>
        /// <param name="context"></param>
        protected virtual void DoTearDownWithData(bool hasData, PDFDataContext context)
        {
            if(hasData)
            {
                context.DataStack.Pop();
            }

            context.NamespaceResolver = lastResolver;
        }

        public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, PDFStyle fullstyle)
        {
            return new Scryber.Layout.LayoutEnginePanel(this, parent);
        }

        #endregion

    }


}
