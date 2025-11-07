using System;
using Scryber.Components;

namespace Scryber.Html.Components
{
    public static class HtmlComponentExtensions
    {

        public static void AddDataContent(this ContainerComponent container, string dataContent, ContextBase context)
        {
            var found = GetDataContent(container, dataContent, context);
            if (null == found)
                return;

            var content = found.Instantiate(0, container);

            if (null != content)
            {
                IContainerComponent icontainer = container;

                foreach (var item in content)
                {
                    if (item is Component)
                        icontainer.Content.Add((Component)item);
                }
            }

        }

        public static ITemplate GetDataContent(this ContainerComponent container, string dataContent, ContextBase context)
        {
            if (string.IsNullOrEmpty(dataContent))
                return null;

            Component found = null;

            if (dataContent.StartsWith("#"))
                found = container.Document.FindAComponentById(dataContent.Substring(1));
            

            if (null == found)
            {
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new PDFParserException("Could not load the data-content, as no element could be found with a matching ID or name " + dataContent);
                else
                    context.TraceLog.Add(TraceLevel.Error, "HTML", "Could not load the data-content, as no element could be found with a matching ID or name " + dataContent);

                return null;
            }
            else if (!(found is ITemplate))
            {
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new PDFParserException("Could not load the data-content, the element with ID or name " + dataContent + " does not support template creation");
                else
                    context.TraceLog.Add(TraceLevel.Error, "HTML", "Could not load the data-content, the element with ID or name " + dataContent + " does not support template creation");

                return null;
            }
            else
            {
                return (found as ITemplate);
            }

        }

        /// <summary>
        /// Actually creates an isntace of the data.Content and adds it to the container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="data"></param>
        /// <param name="context"></param>
        public static void ParseDataContent(this ContainerComponent container, Data.DataBindingContent data,
            DataContext context)
        {
            if (string.IsNullOrEmpty(data.Content))
            {
                if (context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Warning, "Binding", "Binding data-content value was null or empty on the '" + container.ID + " component, even though other options had been set");

                return;
            }


            if (null == data.Type)
                data.Type = container.Document.GetDefaultContentMimeType();

            //Load the parser from the document - will throw an error if not known
            var parser = container.Document.EnsureParser(data.Type);

            if (context.ShouldLogVerbose)
                context.TraceLog.Add(TraceLevel.Verbose, "Binding", "Binding data-content value onto the '" + container.ID + " component, with mime-type " + data.Type.ToString());

            
            using (var sr = new System.IO.StringReader(data.Content))
            {
                var component = parser.Parse(null, sr, ParseSourceType.Template) as Component;

                //We clear out even if the returned content is null
                if (data.Action == DataContentAction.Replace)
                {
                    ((IContainerComponent) container).Content.Clear();

                    if (context.ShouldLogDebug)
                        context.TraceLog.Add(TraceLevel.Debug, "Binding", "Cleared inner content of the '" + container.ID + " component, as data-content-action is set to replace.");
                }
                if (null != component)
                {
                    if (data.Action == DataContentAction.PrePend)
                    {
                        ((IContainerComponent) container).Content.Insert(0, component);

                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Debug, "Binding", "Inserted bound content of the '" + container.ID + " component at index 0, as data-content-action is set to prepend.");
                    }
                    else
                    {
                        ((IContainerComponent) container).Content.Add(component);

                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Debug, "Binding", "Added bound content of the '" + container.ID + " component, to the end.");
                    }
                    
                    //actually perfrom the data binding of the content
                    //TODO: check on the init and load implementation.

                    component.DataBind(context);
                }
                else if (context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Binding", "No component was retruned from the parser with data-content value '" + (data.Content.Length > 50 ? data.Content.Substring(45) + "..." : data.Content) + " on the '" + container.ID + " component");

            }
        }
    }
}
