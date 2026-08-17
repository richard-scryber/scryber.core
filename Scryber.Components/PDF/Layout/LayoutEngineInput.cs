using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// Layout engine for the input fields
    /// </summary>
    public class LayoutEngineInput : LayoutEnginePanel
    {

        protected FormInputField Field { get; private set; }

        protected PDFLayoutPage LayoutPage { get; private set; }

        protected PDFLayoutLine Line { get; private set; }

        private bool _addedProxyText = false;

        public LayoutEngineInput(FormInputField container, IPDFLayoutEngine parent) :base(container, parent)
        {
            this.Field = container;
        }

        protected override void DoLayoutComponent()
        {
            PDFPositionOptions pos = this.FullStyle.CreatePostionOptions(this.Context.PositionDepth > 0);

            PDFLayoutXObjectRun xObject = this.CreateAndAddInput(pos);
            if (null == xObject)
            {
                this.ContinueLayout =  false;
                return;
            }
            
            _addedProxyText = false;

            //A signature field is either unsigned (nothing to show - the reader draws its own
            //"click to sign" UI) or signed (a reader-owned concern entirely out of scope here) -
            //either way it should never get placeholder text baked into its appearance.
            if (string.IsNullOrEmpty(this.Field.Value) && this.Field.FieldType != FormInputFieldType.Signature)
            {
                this.Field.Value = "Proxy Text";
                _addedProxyText = true;
            }

            base.DoLayoutComponent();

            

            xObject.Close();
            
            var width = xObject.Width;
            
            if (this.Line.AvailableWidth < 0)
            {
                this.Line.RemoveRun(xObject);
                this.CloseCurrentLine();
                
                var newLine = this.Line.Region.BeginNewLine();
                
                newLine.AddRun(xObject);
                
                xObject.SetOffsetX(newLine.OffsetX);
                xObject.SetOffsetY(newLine.OffsetY + this.Line.Height);
                
                this.Line = newLine;
                xObject.SetParent(this.Line);
            }
            else if (pos.DisplayMode == Drawing.DisplayMode.Block)
            {
                this.CloseCurrentLine();
            }

            this.LayoutPage = this.Context.DocumentLayout.CurrentPage;
            IArtefactCollection annots;
            if (!this.LayoutPage.Artefacts.TryGetCollection(PDFArtefactTypes.Annotations, out annots))
            {
                annots = new PDFAnnotationCollection(PDFArtefactTypes.Annotations);
                this.LayoutPage.Artefacts.Add(annots);
            }

            
            this.LayoutPage = this.Context.DocumentLayout.CurrentPage;

            annots.Register(this.Field.Widget);
            this.Field.Widget.SetAppearance(FormFieldAppearanceState.Normal, xObject, this.LayoutPage, this.FullStyle);

            //A :hover/:active rule that actually matched this field carries its own state style -
            //pass it through so the widget can repaint Normal's box with its colours. Without one,
            //this state's appearance stays exactly the Normal xObject, unchanged from before.
            Style downStyle;
            this.FullStyle.TryGetStyleState(ComponentState.Down, out downStyle);
            this.Field.Widget.SetAppearance(FormFieldAppearanceState.Down, xObject, this.LayoutPage, this.FullStyle, downStyle);

            Style overStyle;
            this.FullStyle.TryGetStyleState(ComponentState.Over, out overStyle);
            this.Field.Widget.SetAppearance(FormFieldAppearanceState.Over, xObject, this.LayoutPage, this.FullStyle, overStyle);

            if (pos.PositionMode == PositionMode.Static)
            {
                //recalculate the offset
                var loc = Point.Empty;
                loc.X += this.Line.Width - xObject.ChildContainer.Width;
                
                xObject.SetOffsetX(loc.X);
            }

            //this.Field.Widget.XObjectOffset = new Size(this.Line.OffsetX + this.Line.Width - xObject.Width, this.Line.OffsetY);
        }

        protected override void DoLayoutChildren()
        {
            base.DoLayoutChildren();
            
        }

        protected override void DoLayoutTextComponent(ITextComponent text, Style style)
        {
            var region = this.Context.DocumentLayout.CurrentPage.LastOpenBlock().CurrentRegion;

            var line = region.CurrentItem as PDFLayoutLine;
            if (null == line)
                line = region.BeginNewLine();

            var bmc = line.AddMarkedContentStart(this, this.Component, PDFMarkedContentType.Text);

            base.DoLayoutTextComponent(text, style);

            if(this._addedProxyText)
            {
                for (int i = line.Runs.Count - 1; i >= 0; i--)
                {
                    if(line.Runs[i] is PDFTextRunCharacter)
                    {
                        PDFTextRunCharacter chars = (PDFTextRunCharacter)line.Runs[i];
                        chars.Characters = "";
                        break;
                    }
                }
            }
            line.AddMarkedContentEnd(this, bmc);
        }

        private PDFLayoutXObjectRun CreateAndAddInput(PDFPositionOptions pos)
        {
            PDFLayoutBlock containerBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion containerRegion = containerBlock.CurrentRegion;
            
            //If we have a position of static, then we need to create a new positioned region (without an associated run)
            //And then add a new XObjectRun to t

            if (this.HasExistingPositionedRegion(pos))
            {
                if(containerRegion is PDFLayoutPositionedRegion posRegion)
                {
                    var posRun = posRegion.AssociatedRun;
                    this.Line = posRun.Line;
                    this.Line.RemoveRun(posRun);
                }
                else
                {
                    this.ContinueLayout = false;
                    return null;
                }
                
            }
            else if (pos.DisplayMode == Drawing.DisplayMode.Inline)
            {
                if (containerRegion.HasOpenItem == false)
                    containerRegion.BeginNewLine(); // this will hold our xObjectRun
                
                this.Line = containerRegion.CurrentItem as PDFLayoutLine;
                
                containerRegion = containerBlock.BeginNewPositionedRegion(pos, this.DocumentLayout.CurrentPage,
                    this.Component, this.FullStyle, isfloating: false, addAssociatedRun: false);

            }
            else //maps to all types, as it has no diverse content, then it appears on it's own line and in a xObjectRun
            {
                if (containerRegion.HasOpenItem)
                    containerRegion.CloseCurrentItem(); // this will hold our xObjectRun
                
                this.Line = containerRegion.BeginNewLine();
                
                containerRegion = containerBlock.BeginNewPositionedRegion(pos, this.DocumentLayout.CurrentPage,
                    this.Component, this.FullStyle, isfloating: false, addAssociatedRun: false);
            }
            
            
            
            //pos.Y = 200;
            
            PDFLayoutXObjectRun begin = this.Line.AddXObjectRun(this, this.Field, containerRegion, pos, this.FullStyle);
            
            
            // System.IO.File.AppendAllText("/tmp/scryber_xobj_debug.log",
            //     $"CreateAndAddInput field={this.Field.ID} region={containerRegion.GetHashCode()} line={this.Line.GetHashCode()} line.OffsetY={this.Line.OffsetY}\n");

            return begin;
        }

        /// <summary>
        /// Returns true if the parent engines have already created a new positioned block for this input, (e.g. display inlineBlock, or position absolute)
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected virtual bool HasExistingPositionedRegion(PDFPositionOptions pos)
        {
            if (pos.DisplayMode == DisplayMode.InlineBlock)
            {
                return true;
            }
            
            return false;
        }

        private void CloseCurrentLine()
        {
            if (!this.Line.IsClosed)
                this.Line.Region.CloseCurrentItem();
        }

    }
}
