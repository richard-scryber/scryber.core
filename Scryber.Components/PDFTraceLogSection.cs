using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;

namespace Scryber
{
    /// <summary>
    /// A document section that is included by the trace log document so that it can add all the logging content to the end of it.
    /// </summary>
    internal class PDFTraceLogSection : Section
    {
        public PDFDocumentGenerationData GenerationData { get; set; }

        protected override void DoInit(PDFInitContext context)
        {
            if (this.GenerationData != null)
                this.InitAllContent();
            base.DoInit(context);
        }

        private void InitAllContent()
        {
            Head1 title = new Head1() { Text = "Trace Output"};
            this.Contents.Add(title);

            AddDocumentOverview();
            if (null != this.GenerationData.PerformanceMetrics)
                AddPerformance(this.GenerationData.PerformanceMetrics);

            if (null != this.GenerationData.TraceLog)
                AddTraceLog(this.GenerationData.TraceLog);
        }

        private void AddDocumentOverview()
        {
            Head3 head = new Head3() { Text = "Document Overview" };
            this.Contents.Add(head);

            TableGrid tbl = new TableGrid();
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
            TableRow row = new TableRow();
            tbl.Rows.Add(row);

            TableCell cell = new TableHeaderCell() { Width = 100 };
            cell.Contents.Add(new TextLiteral(name));
            row.Cells.Add(cell);

            cell = new TableCell();
            if (string.IsNullOrEmpty(value) == false)
                cell.Contents.Add(new TextLiteral(value));
            row.Cells.Add(cell);


        }

        private void AddPerformance(PDFPerformanceMonitor perfdata)
        {
            Head3 head = new Head3() { Text = "Performance Metrics"};
            this.Contents.Add(head);

            TableGrid tbl = new TableGrid();
            this.Contents.Add(tbl);

            TableHeaderRow top = new TableHeaderRow();
            tbl.Rows.Add(top);

            TableCell cell = new TableHeaderCell();
            cell.Contents.Add(new TextLiteral("Entry"));
            top.Cells.Add(cell);


            cell = new TableHeaderCell() { StyleClass = "number", Width = 100 };
            
            cell.Contents.Add(new TextLiteral("Duration (ms)"));
            top.Cells.Add(cell);

            cell = new TableHeaderCell() { StyleClass = "number", Width = 60 };
            cell.Contents.Add(new TextLiteral("Count"));
            top.Cells.Add(cell);

            foreach (PDFPerformanceMonitorEntry entry in perfdata)
            {
                if (entry.MonitorCount > 0)
                    this.AddPerformanceEntry(tbl, entry);
            }
        }

        private void AddPerformanceEntry(TableGrid grid, PDFPerformanceMonitorEntry entry)
        {
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cell = new TableCell() { DataStyleIdentifier = "PerfCategoryKey" };
            cell.Contents.Add(new TextLiteral(entry.MonitorKey));
            row.Cells.Add(cell);

            cell = new TableCell() { StyleClass = "number", DataStyleIdentifier = "PerfCategoryEntryRight" };
            cell.Contents.Add(new TextLiteral(entry.MonitorElapsed.TotalMilliseconds.ToString("#,##0.00")));
            row.Cells.Add(cell);

            cell = new TableCell() { StyleClass = "number", DataStyleIdentifier = "PerfCategoryEntryRight" };
            cell.Contents.Add(new TextLiteral(entry.MonitorCount.ToString()));
            row.Cells.Add(cell);

            if (entry.HasMeasurements)
            {
                foreach (PDFPerformanceMonitorMeasurement measure in entry.GetMeasurements())
                {
                    this.AddPerformanceMeasurement(grid, measure);
                }
            }
        }

        private void AddPerformanceMeasurement(TableGrid grid, PDFPerformanceMonitorMeasurement measure)
        {
            TableRow row = new TableRow() { StyleClass = "Debug" };
            grid.Rows.Add(row);


            TableCell cell = new TableCell() { DataStyleIdentifier = "PerfCategoryMeasure"};
            cell.Contents.Add(new TextLiteral(measure.Key));
            row.Cells.Add(cell);

            cell = new TableCell(){ DataStyleIdentifier = "PerfCategoryEntryRight"};
            cell.Contents.Add(new TextLiteral(measure.Elapsed.TotalMilliseconds.ToString("#,##0.00")));
            row.Cells.Add(cell);

            cell = new TableCell() { DataStyleIdentifier = "PerfCategoryEntryRight" };
            row.Cells.Add(cell);
        }

        private void AddTraceLog(Logging.PDFCollectorTraceLog log)
        {
            Head3 head = new Head3() { Text = "Document Log" };
            this.Contents.Add(head);

            TableGrid tbl = new TableGrid() { StyleClass = "log-grid" };
            this.Contents.Add(tbl);

            TableHeaderRow top = new TableHeaderRow();
            tbl.Rows.Add(top);

            TableCell cell = new TableHeaderCell() { Width = 60 };
            cell.Contents.Add(new TextLiteral("Time (ms)"));
            top.Cells.Add(cell);

            cell = new TableHeaderCell() { Width = 60 };
            cell.Contents.Add(new TextLiteral("Level"));
            top.Cells.Add(cell);

            cell = new TableHeaderCell() { Width = 100};
            cell.Contents.Add(new TextLiteral("Category"));
            top.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.Contents.Add(new TextLiteral("Message"));
            top.Cells.Add(cell);

            foreach (Logging.PDFCollectorTraceLogEntry entry in log)
            {
                this.AddLogEntry(tbl, entry);
            }
        }

        private void AddLogEntry(TableGrid tbl, Logging.PDFCollectorTraceLogEntry entry)
        {
            
            string rowidentifier = entry.Level.ToString();
            string cellidentifier = "cell_" + rowidentifier;
            string cellnumidentifier = cellidentifier + "_number";
            TableRow row = new TableRow() { DataStyleIdentifier = rowidentifier, StyleClass = rowidentifier };
            tbl.Rows.Add(row);

            TableCell cell = new TableCell() { StyleClass = "number" };
            cell.DataStyleIdentifier = cellnumidentifier;
            cell.Contents.Add(new TextLiteral(entry.TimeStamp.TotalMilliseconds.ToString("#000.0000")));
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.DataStyleIdentifier = cellidentifier;
            cell.Contents.Add(new TextLiteral(entry.Level.ToString()));
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.DataStyleIdentifier = cellidentifier;
            if (!string.IsNullOrEmpty(entry.Category))
                cell.Contents.Add(new TextLiteral(entry.Category));
            row.Cells.Add(cell);

            cell = new TableCell();
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
