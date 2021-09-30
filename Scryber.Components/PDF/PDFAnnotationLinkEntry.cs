using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Native;
using Scryber.Styles;

namespace Scryber.PDF
{
    public class PDFAnnotationLinkEntry : PDFAnnotationEntry
    {
        public PDFAction Action { get; set; }

        public Component Component { get; private set; }

        public Styles.Style AnnotationStyle { get; private set; }

        public PDFAnnotationLinkEntry(Component component, Styles.Style style)
        {
            this.Component = component;
            if (null == component)
                throw new ArgumentNullException("compontent");
            this.AnnotationStyle = style;
        }

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {

            List<PDFObjectRef> all = new List<PDFObjectRef>();

            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Debug, "Link Annotation", "Outputting all required link annotations for component " + this.Component.UniqueID);

            int pageindex = context.PageIndex;
            ComponentArrangement arrange = this.Component.GetFirstArrangement();
            int index = 1;
            while (null != arrange && arrange.PageIndex == pageindex)
            {
                PDFObjectRef annotref = writer.BeginObject();
                writer.BeginDictionary();
                writer.WriteDictionaryNameEntry("Type", "Annot");
                writer.WriteDictionaryNameEntry("Subtype", "Link");
                if (!string.IsNullOrEmpty(this.AlternateText))
                    writer.WriteDictionaryStringEntry("Contents", this.AlternateText);
                PDFRect bounds = arrange.RenderBounds;
                if (bounds != PDFRect.Empty && bounds.Size != PDFSize.Empty)
                {
                    if (context.DrawingOrigin == DrawingOrigin.TopLeft)
                    {
                        //PDFs have origin at bottom so need to convert.
                        PDFReal value = context.Graphics.GetXPosition(bounds.X.RealValue);
                        bounds.X = new PDFUnit(value.Value, PageUnits.Points);
                        value = context.Graphics.GetYPosition(bounds.Y.RealValue);
                        bounds.Y = new PDFUnit(value.Value, PageUnits.Points);
                        bounds.Width = bounds.X + bounds.Width;
                        bounds.Height = bounds.Y - bounds.Height;
                    }
                    else
                    {
                        bounds.Width = bounds.X + bounds.Width;
                        bounds.Height = bounds.Y + bounds.Height;
                    }

                    writer.BeginDictionaryEntry("Rect");
                    writer.WriteArrayRealEntries(bounds.X.Value, bounds.Y.Value, bounds.Width.Value, bounds.Height.Value);
                    writer.EndDictionaryEntry();
                }
                string name = this.Component.UniqueID + "_" + index.ToString();
                writer.WriteDictionaryStringEntry("NM", name);

                //Draw the border
                StyleValue<LineType> lstyle;

                /* if (this.AnnotationStyle != null && this.AnnotationStyle.TryGetValue(StyleKeys.BorderStyleKey, out lstyle) && lstyle != null && lstyle.Value != LineType.None)
                {
                    PDFUnit corner = this.AnnotationStyle.GetValue(StyleKeys.BorderCornerRadiusKey, (PDFUnit)0);
                    PDFUnit width = this.AnnotationStyle.GetValue(StyleKeys.BorderWidthKey, (PDFUnit)1);
                    PDFColor c = this.AnnotationStyle.GetValue(StyleKeys.BorderColorKey, PDFColors.Transparent);

                    if (c != null && width > 0)
                    {
                        writer.BeginDictionaryEntry("Border");
                        writer.WriteArrayRealEntries(corner.PointsValue, corner.PointsValue, width.PointsValue);
                        writer.EndDictionaryEntry();

                        writer.BeginDictionaryEntry("C");
                        if (c.ColorSpace == ColorSpace.G)
                            writer.WriteArrayRealEntries(c.Gray.Value);
                        else if (c.ColorSpace == ColorSpace.RGB)
                            writer.WriteArrayRealEntries(c.Red.Value, c.Green.Value, c.Blue.Value);
                        else if (context.Conformance == ParserConformanceMode.Strict)
                            RecordAndRaise.ArgumentOutOfRange("c", Errors.ColorValueIsNotCurrentlySupported, c.ColorSpace);
                        else
                            context.TraceLog.Add(TraceLevel.Error, "Link Annotation", string.Format(Errors.ColorValueIsNotCurrentlySupported, c.ColorSpace));

                        writer.EndDictionaryEntry();
                    }

                }
                else
                {   */

                    writer.BeginDictionaryEntry("Border");
                    //writer.WriteArrayRealEntries(1.0, 1.0, 1.0);
                    writer.WriteArrayRealEntries(0.0, 0.0, 0.0);
                    writer.EndDictionaryEntry();
                //}

                if (null != this.Action)
                {
                    writer.BeginDictionaryEntry("A");
                    PDFObjectRef actionref = this.Action.OutputToPDF(context, writer);
                    if (null != actionref)
                        writer.WriteObjectRefS(actionref);
                    writer.EndDictionaryEntry();
                }
                writer.EndDictionary();
                writer.EndObject();

                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, "Link Annotation", "Annotation added " + name + " for bounds " + bounds + " on page " + context.PageIndex);

                all.Add(annotref);

                //If we have more than one arrangement on the object then move to the next one
                if (arrange is ComponentMultiArrangement)
                    arrange = ((ComponentMultiArrangement)arrange).NextArrangement;
                index++;
            }



            if (all.Count == 0)
            {
                if (context.ShouldLogDebug)
                    context.TraceLog.End(TraceLevel.Debug, "Link Annotation", "No required link annotations for component " + this.Component.UniqueID);

                return null;
            }
            else if(all.Count == 1)
            {
                return all[0];
            }
            else
            {
                if (context.ShouldLogDebug)
                    context.TraceLog.End(TraceLevel.Debug, "Link Annotation", "All " + all.Count + " link annotations for component " + this.Component.UniqueID + " were output");
                PDFObjectRef array = writer.BeginObject();
                writer.WriteArrayRefEntries(all.ToArray());
                writer.EndObject();
                return array;
            }
        }

        protected virtual PDFRect GetComponentBounds(Component comp, ComponentArrangement arrange)
        {
            if (null != arrange)
                return arrange.RenderBounds;
            else
                return PDFRect.Empty;
        }

    }

}
