using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("Xml")]
    public class XmlData : Scryber.Components.Preformatted
    {
        [PDFAttribute("datasource-id")]
        public string DataSourceID { get; set; }

        [PDFAttribute("select")]
        public string Select { get; set; }


        private Scryber.Components.TextLiteral _literal;

        protected override void DoDataBind(PDFDataContext context, bool includeChildren)
        {
            base.DoDataBind(context, includeChildren);


            if (null != _literal)
                this.Contents.Remove(_literal);

            object data = GetRequiredData(context);
            if (null == data)
                return;

            _literal = new Components.TextLiteral();


            if (data is System.Xml.XPath.XPathNavigator)
                _literal.Text = ((System.Xml.XPath.XPathNavigator)data).OuterXml;

            else if (data is System.Xml.XPath.XPathNodeIterator)
            {
                System.Xml.XPath.XPathNodeIterator itter = (System.Xml.XPath.XPathNodeIterator)data;

                //Clone it so we do not affect anything else
                itter = itter.Clone();

                StringBuilder sb = new StringBuilder();
                while (itter.MoveNext())
                {
                    sb.AppendLine(itter.Current.OuterXml);
                }
                _literal.Text = sb.ToString();
            }

            this.Contents.Add(_literal);
        }


        protected IPDFDataSource GetDataSourceComponent(string datasourceComponentID, PDFDataContext context)
        {
            IPDFDataSource datasourceComponent = null;

            if (string.IsNullOrEmpty(datasourceComponentID))
                throw new ArgumentNullException("datasourceComponentID");

            Component found = base.FindDocumentComponentById(datasourceComponentID);
            if (found == null)
                throw RecordAndRaise.ArgumentNull("DataSourceID", Errors.CouldNotFindControlWithID, datasourceComponentID);
            else if (!(found is IPDFDataSource))
                throw RecordAndRaise.Argument("DataSourceID", Errors.AssignedDataSourceIsNotIPDFDataSource, datasourceComponentID);
            else
                datasourceComponent = ((IPDFDataSource)found);


            return datasourceComponent;
        }

        protected virtual object GetRequiredData(PDFDataContext context)
        {
            object data = null;
            IPDFDataSource src;
            bool hasdata = false;
            System.Xml.IXmlNamespaceResolver resolver = context.NamespaceResolver;

            if (!string.IsNullOrEmpty(this.DataSourceID))
            {
                src = GetDataSourceComponent(this.DataSourceID, context);
                data = src.Select(this.Select, context);
                if (null == data)
                    context.TraceLog.Add(TraceLevel.Warning, "Data XML", string.Format("NULL data was returned for the path '{0}' on the PDFForEach component {1} with datasource {2}", this.Select, this.ID, src.ID));
                hasdata = true;
            }
            else if(!string.IsNullOrEmpty(this.Select))
            {
                src = context.DataStack.Source;
                data = src.Select(this.Select, context.DataStack.Current, context);
                if (null == data)
                    context.TraceLog.Add(TraceLevel.Warning, "Data XML", string.Format("NULL data was returned for the path '{0}' on the PDFForEach component {1} with data source {2}", this.Select, this.ID, src.ID));
                hasdata = true;
            }
            else if(context.DataStack.HasData)
            {
                data = context.DataStack.Current;
                hasdata = true;
            }

            if (hasdata)
                return data;
            else
                return null;
        }

        protected override Style GetBaseStyle()
        {
            Style baseStyle= base.GetBaseStyle();
            baseStyle.Overflow.Split = Drawing.OverflowSplit.Any;
            baseStyle.Size.FullWidth = true;
            baseStyle.Padding.All = 5;
            baseStyle.Border.Color = new Drawing.PDFColor(255, 127, 80); //Coral;
            baseStyle.Background.Color = new Drawing.PDFColor(255, 248, 220); //Cornsilk
            baseStyle.Font.FontSize = 10;

            return baseStyle;
        }
    }
}
