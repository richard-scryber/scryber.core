using System;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using Scryber.Components;

namespace Scryber.Html.Components
{
    public static class HtmlComponentExtensions
    {

        public static void AddDataContent(this ContainerComponent container, string dataContent, PDFContextBase context)
        {
            var found = GetDataContent(container, dataContent, context);
            if (null == found)
                return;

            var content = found.Instantiate(0, container);

            if (null != content)
            {
                IPDFContainerComponent icontainer = container;

                foreach (var item in content)
                {
                    if (item is Component)
                        icontainer.Content.Add((Component)item);
                }
            }

        }

        public static IPDFTemplate GetDataContent(this ContainerComponent container, string dataContent, PDFContextBase context)
        {
            if (string.IsNullOrEmpty(dataContent))
                return null;

            Component found;

            if (dataContent.StartsWith("#"))
                found = container.Document.FindAComponentById(dataContent.Substring(1));
            else
                found = container.Document.FindAComponentByName(dataContent);

            if (null == found)
            {
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new PDFParserException("Could not load the data-content, as no element could be found with a matching ID or name " + dataContent);
                else
                    context.TraceLog.Add(TraceLevel.Error, "HTML", "Could not load the data-content, as no element could be found with a matching ID or name " + dataContent);

                return null;
            }
            else if (!(found is IPDFTemplate))
            {
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new PDFParserException("Could not load the data-content, the element with ID or name " + dataContent + " does not support template creation");
                else
                    context.TraceLog.Add(TraceLevel.Error, "HTML", "Could not load the data-content, the element with ID or name " + dataContent + " does not support template creation");

                return null;
            }
            else
            {
                return (found as IPDFTemplate);
            }

        }
    }
}
