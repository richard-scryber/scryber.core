using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Imaging;
using Scryber.PDF.Resources;
using Scryber.Logging;

namespace Scryber.PDF
{
    /// <summary>
    /// A document section that is included by the trace log document so that it can add all the logging content to the end of it.
    /// </summary>
    internal class PDFTraceLogSection : Section
    {
        public PDFDocumentGenerationData GenerationData { get; set; }

        public PDFResourceCollection OwnerResources { get; set; }

        protected override void DoInit(InitContext context)
        {
            if (this.GenerationData != null)
                this.TryInitAllContent();
            base.DoInit(context);
        }

        private void TryInitAllContent()
        {
            try
            {
                this.InitAllContent();
            }
            catch(Exception ex)
            {
                throw new Scryber.PDFRenderException("Could not build the trace log: " + ex.Message, ex);
            }
        }
        private void InitAllContent()
        {
            Head1 title = new Head1() { Text = "Trace Output", ElementName = "h1"};
            this.Contents.Add(title);

            AddDocumentOverview();

            if (null != this.GenerationData.PerformanceMetrics)
                AddPerformance(this.GenerationData.PerformanceMetrics);

            if (null != this.OwnerResources)
                AddResources(this.OwnerResources);

            if (null != this.GenerationData.TraceLog)
                AddTraceLog(this.GenerationData.TraceLog);
        }

        private void AddDocumentOverview()
        {
            Head3 head = new Head3() { Text = "Document Overview", ElementName = "h3" };
            this.Contents.Add(head);

            TableGrid tbl = new TableGrid() { ElementName = "table"};
            this.Contents.Add(tbl);

            if (null != this.GenerationData.DocumentInfo &&
                !string.IsNullOrEmpty(this.GenerationData.DocumentInfo.Title))
                this.AddOverviewRow(tbl, "Title", this.GenerationData.DocumentInfo.Title);

            if (null != this.GenerationData.DocumentID)
                this.AddOverviewRow(tbl, "Document ID", Convert.ToBase64String(this.GenerationData.DocumentID.One) + "." + Convert.ToBase64String(this.GenerationData.DocumentID.Two));
            
            if (!string.IsNullOrEmpty(this.GenerationData.TemplatePath))
                this.AddOverviewRow(tbl, "Source Template", this.GenerationData.TemplatePath);

            this.AddOverviewRow(tbl, "Scryber Version", this.GenerationData.ScryberVersion.ToString() + " (" + this.GenerationData.ScryberFileVersion + ")");
            this.AddOverviewRow(tbl, "Document Size", GetMbSizeString(this.GenerationData.DocumentSize));
            this.AddOverviewRow(tbl, "Generation Time ", GetGenTimeString(this.GenerationData.DocumentGenerationTime));
            this.AddOverviewRow(tbl, "Trace Level", this.GenerationData.TraceLevel.ToString());
        }

        private void AddOverviewRow(TableGrid tbl, string name, string value)
        {
            TableRow row = new TableRow() {ElementName = "tr"};
            tbl.Rows.Add(row);

            TableCell cell = new TableHeaderCell() { Width = 100, ElementName = "td"};
            cell.Contents.Add(new TextLiteral(name));
            row.Cells.Add(cell);

            cell = new TableCell() {ElementName = "td"};
            if (string.IsNullOrEmpty(value) == false)
                cell.Contents.Add(new TextLiteral(value));
            row.Cells.Add(cell);


        }

        private void AddResources(PDFResourceCollection resources)
        {
            Head3 head = new Head3() { Text = "Document Resources", ElementName = "h3"};
            this.Contents.Add(head);

            TableGrid tbl = new TableGrid() {ElementName = "table"};
            this.Contents.Add(tbl);

            if(resources.Count == 0)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.Contents.Add(new TextLiteral("This document contains no resources"));
                row.Cells.Add(cell);
                tbl.Rows.Add(row);
            }
            else
            {
                TableRow row = new TableRow() {ElementName = "tr"};
                TableCell cell = new TableCell() {ElementName = "td"};
                cell.CellColumnSpan = 2;
                cell.Contents.Add(new TextLiteral("This document contains " + resources.Count.ToString() + " resources"));
                row.Cells.Add(cell);
                tbl.Rows.Add(row);

                foreach (var rsrc in resources)
                {
                    if (rsrc.ResourceType == PDFResource.FontDefnResourceType)
                        this.AddFontResource(tbl, rsrc as PDFFontResource);
                    else if (rsrc.ResourceType == PDFFontResource.XObjectResourceType)
                        this.AddXObjectResource(tbl, rsrc);
                    else
                        this.AddOtherResource(tbl, rsrc);
                }
            }
        }

        private void AddFontResource(TableGrid tbl, PDFFontResource fnt)
        {
            TableRow row = new TableRow() {ElementName = "tr"};
            TableCell cell = new TableCell() {ElementName = "td"};
            cell.Width = 100;
            if (fnt.Definition.IsStandard)
                cell.Contents.Add(new TextLiteral("Standard Font"));
            else if (fnt.Definition.IsUnicode)
                cell.Contents.Add(new TextLiteral("Composite Font"));
            else
                cell.Contents.Add(new TextLiteral("Ansi Font"));

            row.Cells.Add(cell);

            cell = new TableCell() {ElementName = "td"};
            var bold = new BoldSpan();
            bold.Contents.Add(new TextLiteral(fnt.Definition.Family + ", weight : " + fnt.Definition.Weight + ", style : " + (fnt.Definition.Italic ? "Italic" : "Regular")));
            cell.Contents.Add(bold);
            row.Cells.Add(cell);

            if (!string.IsNullOrEmpty(fnt.Definition.FilePath))
            {
                cell.Contents.Add(new LineBreak());
                cell.Contents.Add(new TextLiteral(fnt.Definition.FilePath));
            }
            tbl.Rows.Add(row);
        }

        private void AddXObjectResource(TableGrid tbl, PDFResource rsrc)
        {
            TableRow row = new TableRow() {ElementName = "tr"};
            TableCell cell = new TableCell() {ElementName = "td"};
            cell.Width = 100;
            
            string type;
            
            if (rsrc is PDFImageXObject)
                type = "Image";
            else if (rsrc is PDFLayoutXObjectResource)
                type = "XObject";
            else
                type = "Other";

            
                
            cell.Contents.Add(new TextLiteral(type));
            row.Cells.Add(cell);

            cell = new TableCell() {ElementName = "td"};
            var bold = new BoldSpan();
            if (rsrc is PDFImageXObject img)
            {
                if (img.ImageData != null)
                {
                    bold.Contents.Add(new TextLiteral(img.ImageData.SourcePath));
                    cell.Contents.Add(bold);
                    cell.Contents.Add(new LineBreak());
                    var data = img.ImageData;
                    if(data is ImageDataProxy proxy)
                        data = proxy.ImageData;

                    string sizeStr;

                    if (data is ImageRasterData raster)
                        sizeStr = "Raster image " + raster.PixelWidth + " by " + raster.PixelHeight + " pixels, ";
                    else if(data is ImageVectorData vector) //vector
                        sizeStr = "vector image " + data.GetSize();
                    else if(null == data)
                        sizeStr = "image is null";
                    else
                        sizeStr = "Unknown image " + data.GetSize();
                    
                    cell.Contents.Add(new TextLiteral(sizeStr));
                }
                else
                {
                    bold.Contents.Add(new TextLiteral(img.Source));
                    cell.Contents.Add(bold);
                }
            }
            else if (rsrc is PDFLayoutXObjectResource xobj)
            {
                if (null == xobj.Layout)
                {
                    if (null == xobj.Renderer)
                        bold.Contents.Add(new TextLiteral("Layout xObject - unknown owner"));
                    else
                    {
                        bold.Contents.Add(new TextLiteral("Layout XObject for " + xobj.Renderer.Owner.ID));
                    }

                    cell.Contents.Add(bold);
                }
                else
                {
                    bold.Contents.Add(new TextLiteral("Layout XObject for " + xobj.Layout.Owner.ID));
                    cell.Contents.Add(bold);
                }
            }

            row.Cells.Add(cell);
            tbl.Rows.Add(row);
        }

        private void AddOtherResource(TableGrid togrid, PDFResource rsrc)
        {
            
        }



        private void AddPerformance(PerformanceMonitor perfdata)
        {
            Head3 head = new Head3() { Text = "Performance Metrics", ElementName = "h3"};
            this.Contents.Add(head);

            TableGrid tbl = new TableGrid() {ElementName = "table"};
            this.Contents.Add(tbl);

            TableHeaderRow top = new TableHeaderRow() {ElementName = "tr", StyleClass = "head"};
            tbl.Rows.Add(top);

            TableCell cell = new TableHeaderCell() {ElementName = "td", StyleClass = "head"};
            cell.Contents.Add(new TextLiteral("Entry"));
            top.Cells.Add(cell);


            cell = new TableHeaderCell() { ElementName = "td", StyleClass = "number", Width = 100 };
            
            cell.Contents.Add(new TextLiteral("Duration (ms)"));
            top.Cells.Add(cell);

            cell = new TableHeaderCell() { ElementName = "td", StyleClass = "number", Width = 60 };
            cell.Contents.Add(new TextLiteral("Count"));
            top.Cells.Add(cell);

            foreach (PerformanceMonitorEntry entry in perfdata)
            {
                if (entry.MonitorCount > 0)
                    this.AddPerformanceEntry(tbl, entry);
            }
        }

        private void AddPerformanceEntry(TableGrid grid, PerformanceMonitorEntry entry)
        {
            TableRow row = new TableRow(){ ElementName = "tr",};
            grid.Rows.Add(row);

            TableCell cell = new TableCell() { ElementName = "td", DataStyleIdentifier = "PerfCategoryKey" };
            cell.Contents.Add(new TextLiteral(entry.MonitorKey));
            row.Cells.Add(cell);

            cell = new TableCell() {  ElementName = "td",StyleClass = "number", DataStyleIdentifier = "PerfCategoryEntryRight" };
            cell.Contents.Add(new TextLiteral(entry.MonitorElapsed.TotalMilliseconds.ToString("#,##0.00")));
            row.Cells.Add(cell);

            cell = new TableCell() {  ElementName = "td",StyleClass = "number", DataStyleIdentifier = "PerfCategoryEntryRight" };
            cell.Contents.Add(new TextLiteral(entry.MonitorCount.ToString()));
            row.Cells.Add(cell);

            if (entry.HasMeasurements)
            {
                foreach (PerformanceMonitorMeasurement measure in entry.GetMeasurements())
                {
                    this.AddPerformanceMeasurement(grid, measure);
                }
            }
        }

        private void AddPerformanceMeasurement(TableGrid grid, PerformanceMonitorMeasurement measure)
        {
            TableRow row = new TableRow() {  ElementName = "tr",StyleClass = "Debug" };
            grid.Rows.Add(row);


            TableCell cell = new TableCell() {  ElementName = "td",DataStyleIdentifier = "PerfCategoryMeasure"};
            cell.Contents.Add(new TextLiteral(measure.Key));
            row.Cells.Add(cell);

            cell = new TableCell(){  ElementName = "td",DataStyleIdentifier = "PerfCategoryEntryRight"};
            cell.Contents.Add(new TextLiteral(measure.Elapsed.TotalMilliseconds.ToString("#,##0.00")));
            row.Cells.Add(cell);

            cell = new TableCell() {  ElementName = "td",DataStyleIdentifier = "PerfCategoryEntryRight" };
            row.Cells.Add(cell);
        }

        private void AddTraceLog(Logging.CollectorTraceLog log)
        {
            Head3 head = new Head3() { Text = "Document Log", ElementName = "h3"};
            this.Contents.Add(head);

            TableGrid tbl = new TableGrid() { StyleClass = "log-grid", ElementName = "table"};
            this.Contents.Add(tbl);

            TableHeaderRow top = new TableHeaderRow(){ ElementName = "tr"};
            tbl.Rows.Add(top);

            TableCell cell = new TableHeaderCell() {  ElementName = "td", StyleClass = "head",Width = 60 };
            cell.Contents.Add(new TextLiteral("Time (ms)"));
            top.Cells.Add(cell);

            cell = new TableHeaderCell() {  ElementName = "td", StyleClass = "head",Width = 60 };
            cell.Contents.Add(new TextLiteral("Level"));
            top.Cells.Add(cell);

            cell = new TableHeaderCell() {  ElementName = "td", StyleClass = "head",Width = 100};
            cell.Contents.Add(new TextLiteral("Category"));
            top.Cells.Add(cell);

            cell = new TableHeaderCell(){  ElementName = "td", StyleClass = "head"};
            cell.Contents.Add(new TextLiteral("Message"));
            top.Cells.Add(cell);

            foreach (Logging.CollectorTraceLogEntry entry in log)
            {
                this.AddLogEntry(tbl, entry);
            }
        }

        private void AddLogEntry(TableGrid tbl, Logging.CollectorTraceLogEntry entry)
        {
            
            string rowidentifier = entry.Level.ToString();
            string cellidentifier = "cell_" + rowidentifier;
            string cellnumidentifier = cellidentifier + "_number";
            TableRow row = new TableRow() { ElementName = "tr",DataStyleIdentifier = rowidentifier, StyleClass = rowidentifier };
            tbl.Rows.Add(row);

            TableCell cell = new TableCell() { ElementName = "td",StyleClass = "number" };
            cell.DataStyleIdentifier = cellnumidentifier;
            cell.Contents.Add(new TextLiteral(entry.TimeStamp.TotalMilliseconds.ToString("#000.0000")));
            row.Cells.Add(cell);

            cell = new TableCell(){ElementName = "td"};
            cell.DataStyleIdentifier = cellidentifier;
            cell.Contents.Add(new TextLiteral(entry.Level.ToString()));
            row.Cells.Add(cell);

            cell = new TableCell(){ElementName = "td"};
            cell.DataStyleIdentifier = cellidentifier;
            if (!string.IsNullOrEmpty(entry.Category))
                cell.Contents.Add(new TextLiteral(entry.Category));
            row.Cells.Add(cell);

            cell = new TableCell() {ElementName = "td"};
            cell.DataStyleIdentifier = cellidentifier;
            if (!string.IsNullOrEmpty(entry.Message))
                cell.Contents.Add(new TextLiteral(entry.Message));

            if (entry.HasException)
            {
                cell.Contents.Add(new LineBreak());
                cell.Contents.Add(new TextLiteral(entry.Exception.ToString()));
            }
            row.Cells.Add(cell);

        }


        private static string GetMbSizeString(long len)
        {
            const long Onekb = 1024;
            const long OneMb = 1024 * Onekb;

            StringBuilder sb = new StringBuilder();

            if (len > OneMb)
            {
                long mb = len / OneMb;
                len = len - (mb * OneMb);
                sb.Append(mb);
                sb.Append("Mb ");
            }
            if (len > Onekb)
            {
                long kb = len / Onekb;
                len = len - (kb * Onekb);
                sb.Append(kb);
                sb.Append("kb ");
            }

            sb.Append(len);

            return sb.ToString();
        }

        private static string GetGenTimeString(TimeSpan ts)
        {
            StringBuilder sb = new StringBuilder();
            if (ts.TotalMinutes > 1)
            {
                int mins = (int)ts.TotalSeconds;
                sb.Append(mins.ToString());
                sb.Append("mins ");
                ts = ts.Subtract(new TimeSpan(0, mins, 0));
            }
            if (ts.Seconds > 0)
            {
                sb.Append(ts.Seconds.ToString());
                sb.Append("secs ");
                ts = ts.Subtract(new TimeSpan(0, 0, ts.Seconds));
            }
            sb.Append(ts.TotalMilliseconds.ToString("##0.000"));
            sb.Append("ms");

            return sb.ToString();

        }

    }
}
