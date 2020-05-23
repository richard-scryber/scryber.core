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
    internal class PDFTraceLogSection : PDFSection
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
            PDFHead1 title = new PDFHead1() { Text = "Trace Output"};
            this.Contents.Add(title);

            AddDocumentOverview();
            if (null != this.GenerationData.PerformanceMetrics)
                AddPerformance(this.GenerationData.PerformanceMetrics);

            if (null != this.GenerationData.TraceLog)
                AddTraceLog(this.GenerationData.TraceLog);
        }

        private void AddDocumentOverview()
        {
            PDFHead3 head = new PDFHead3() { Text = "Document Overview" };
            this.Contents.Add(head);

            PDFTableGrid tbl = new PDFTableGrid();
            this.Contents.Add(tbl);

            if (null != this.GenerationData.DocumentInfo &&
                !string.IsNullOrEmpty(this.GenerationData.DocumentInfo.Title))
                this.AddOverviewRow(tbl, "Title", this.GenerationData.DocumentInfo.Title);

            if (null != this.GenerationData.DocumentID)
                this.AddOverviewRow(tbl, "Document ID", this.GenerationData.DocumentID.One + " " + this.GenerationData.DocumentID.Two);

            if (!string.IsNullOrEmpty(this.GenerationData.TemplatePath))
                this.AddOverviewRow(tbl, "Source Template", this.GenerationData.TemplatePath);

            this.AddOverviewRow(tbl, "Scryber Version", this.GenerationData.ScryberVersion.ToString() + " (" + this.GenerationData.ScryberFileVersion + ")");
            this.AddOverviewRow(tbl, "Document Size", GetMbSizeString(this.GenerationData.DocumentSize));
            this.AddOverviewRow(tbl, "Generation Time ", GetGenTimeString(this.GenerationData.DocumentGenerationTime));
            this.AddOverviewRow(tbl, "Trace Level", this.GenerationData.TraceLevel.ToString());
        }

        private void AddOverviewRow(PDFTableGrid tbl, string name, string value)
        {
            PDFTableRow row = new PDFTableRow();
            tbl.Rows.Add(row);

            PDFTableCell cell = new PDFTableHeaderCell() { Width = 100 };
            cell.Contents.Add(new PDFTextLiteral(name));
            row.Cells.Add(cell);

            cell = new PDFTableCell();
            if (string.IsNullOrEmpty(value) == false)
                cell.Contents.Add(new PDFTextLiteral(value));
            row.Cells.Add(cell);


        }

        private void AddPerformance(PDFPerformanceMonitor perfdata)
        {
            PDFHead3 head = new PDFHead3() { Text = "Performance Metrics"};
            this.Contents.Add(head);

            PDFTableGrid tbl = new PDFTableGrid();
            this.Contents.Add(tbl);

            PDFTableHeaderRow top = new PDFTableHeaderRow();
            tbl.Rows.Add(top);

            PDFTableCell cell = new PDFTableHeaderCell();
            cell.Contents.Add(new PDFTextLiteral("Entry"));
            top.Cells.Add(cell);


            cell = new PDFTableHeaderCell() { StyleClass = "number", Width = 100 };
            
            cell.Contents.Add(new PDFTextLiteral("Duration (ms)"));
            top.Cells.Add(cell);

            cell = new PDFTableHeaderCell() { StyleClass = "number", Width = 60 };
            cell.Contents.Add(new PDFTextLiteral("Count"));
            top.Cells.Add(cell);

            foreach (PDFPerformanceMonitorEntry entry in perfdata)
            {
                if (entry.MonitorCount > 0)
                    this.AddPerformanceEntry(tbl, entry);
            }
        }

        private void AddPerformanceEntry(PDFTableGrid grid, PDFPerformanceMonitorEntry entry)
        {
            PDFTableRow row = new PDFTableRow();
            grid.Rows.Add(row);

            PDFTableCell cell = new PDFTableCell() { DataStyleIdentifier = "PerfCategoryKey" };
            cell.Contents.Add(new PDFTextLiteral(entry.MonitorKey));
            row.Cells.Add(cell);

            cell = new PDFTableCell() { StyleClass = "number", DataStyleIdentifier = "PerfCategoryEntryRight" };
            cell.Contents.Add(new PDFTextLiteral(entry.MonitorElapsed.TotalMilliseconds.ToString("#,##0.00")));
            row.Cells.Add(cell);

            cell = new PDFTableCell() { StyleClass = "number", DataStyleIdentifier = "PerfCategoryEntryRight" };
            cell.Contents.Add(new PDFTextLiteral(entry.MonitorCount.ToString()));
            row.Cells.Add(cell);

            if (entry.HasMeasurements)
            {
                foreach (PDFPerformanceMonitorMeasurement measure in entry.GetMeasurements())
                {
                    this.AddPerformanceMeasurement(grid, measure);
                }
            }
        }

        private void AddPerformanceMeasurement(PDFTableGrid grid, PDFPerformanceMonitorMeasurement measure)
        {
            PDFTableRow row = new PDFTableRow() { StyleClass = "Debug" };
            grid.Rows.Add(row);


            PDFTableCell cell = new PDFTableCell() { DataStyleIdentifier = "PerfCategoryMeasure"};
            cell.Contents.Add(new PDFTextLiteral(measure.Key));
            row.Cells.Add(cell);

            cell = new PDFTableCell(){ DataStyleIdentifier = "PerfCategoryEntryRight"};
            cell.Contents.Add(new PDFTextLiteral(measure.Elapsed.TotalMilliseconds.ToString("#,##0.00")));
            row.Cells.Add(cell);

            cell = new PDFTableCell() { DataStyleIdentifier = "PerfCategoryEntryRight" };
            row.Cells.Add(cell);
        }

        private void AddTraceLog(Logging.PDFCollectorTraceLog log)
        {
            PDFHead3 head = new PDFHead3() { Text = "Document Log" };
            this.Contents.Add(head);

            PDFTableGrid tbl = new PDFTableGrid() { StyleClass = "log-grid" };
            this.Contents.Add(tbl);

            PDFTableHeaderRow top = new PDFTableHeaderRow();
            tbl.Rows.Add(top);

            PDFTableCell cell = new PDFTableHeaderCell() { Width = 60 };
            cell.Contents.Add(new PDFTextLiteral("Time (ms)"));
            top.Cells.Add(cell);

            cell = new PDFTableHeaderCell() { Width = 60 };
            cell.Contents.Add(new PDFTextLiteral("Level"));
            top.Cells.Add(cell);

            cell = new PDFTableHeaderCell() { Width = 100};
            cell.Contents.Add(new PDFTextLiteral("Category"));
            top.Cells.Add(cell);

            cell = new PDFTableHeaderCell();
            cell.Contents.Add(new PDFTextLiteral("Message"));
            top.Cells.Add(cell);

            foreach (Logging.PDFCollectorTraceLogEntry entry in log)
            {
                this.AddLogEntry(tbl, entry);
            }
        }

        private void AddLogEntry(PDFTableGrid tbl, Logging.PDFCollectorTraceLogEntry entry)
        {
            
            string rowidentifier = entry.Level.ToString();
            string cellidentifier = "cell_" + rowidentifier;
            string cellnumidentifier = cellidentifier + "_number";
            PDFTableRow row = new PDFTableRow() { DataStyleIdentifier = rowidentifier, StyleClass = rowidentifier };
            tbl.Rows.Add(row);

            PDFTableCell cell = new PDFTableCell() { StyleClass = "number" };
            cell.DataStyleIdentifier = cellnumidentifier;
            cell.Contents.Add(new PDFTextLiteral(entry.TimeStamp.TotalMilliseconds.ToString("#000.0000")));
            row.Cells.Add(cell);

            cell = new PDFTableCell();
            cell.DataStyleIdentifier = cellidentifier;
            cell.Contents.Add(new PDFTextLiteral(entry.Level.ToString()));
            row.Cells.Add(cell);

            cell = new PDFTableCell();
            cell.DataStyleIdentifier = cellidentifier;
            if (!string.IsNullOrEmpty(entry.Category))
                cell.Contents.Add(new PDFTextLiteral(entry.Category));
            row.Cells.Add(cell);

            cell = new PDFTableCell();
            cell.DataStyleIdentifier = cellidentifier;
            if (!string.IsNullOrEmpty(entry.Message))
                cell.Contents.Add(new PDFTextLiteral(entry.Message));

            if (entry.HasException)
            {
                cell.Contents.Add(new PDFLineBreak());
                cell.Contents.Add(new PDFTextLiteral(entry.Exception.ToString()));
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
