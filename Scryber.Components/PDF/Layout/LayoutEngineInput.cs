using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Html.Components;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// Layout engine for the input fields
    /// </summary>
    public class LayoutEngineInput : LayoutEnginePanel
    {

        protected FormInputField Field { get; private set; }

        protected PDFLayoutPage LayoutPage { get; set; }

        protected PDFLayoutLine Line { get; set; }

        /// <summary>
        /// The run produced by this pass, once DoLayoutComponent has closed it - lets an external
        /// orchestrator (LayoutEngineStatedButton) retrieve the result of a plain, real in-flow
        /// Normal-state pass without needing its own field on this class.
        /// </summary>
        public PDFLayoutXObjectRun Result { get; protected set; }

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

            // Everything below except the empty-value case is swapping in a placeholder purely
            // to drive this component's own width/height measurement - it must not leak out as
            // the field's real, post-render Value (that would corrupt anything that reads the
            // component back afterward, e.g. select.Value). The widget's real /V was already
            // captured by GetFieldEntry during RegisterLayoutArtefacts, which runs before this
            // method, so restoring the original value here has no effect on the PDF output.
            string originalValue = this.Field.Value;
            bool restoreValueAfterLayout = false;

            //A signature field is either unsigned (nothing to show - the reader draws its own
            //"click to sign" UI) or signed (a reader-owned concern entirely out of scope here) -
            //either way it should never get placeholder text baked into its appearance.
            if (this.Field.FieldType == FormInputFieldType.Signature)
            {
                //Deliberately no-op - do not fall through to the empty-value proxy-text case below.
            }
            else if (string.IsNullOrEmpty(this.Field.Value))
            {
                this.Field.Value = "Proxy Text";
                _addedProxyText = true;
            }
            else if (this.Field.ButtonType == FormButtonFieldType.CheckBox || this.Field.ButtonType == FormButtonFieldType.Radio)
            {
                //A checkbox/radio's Value is the PDF on-state name (e.g. "yes"/"A"/"B") - already
                //captured into the widget's OnStateName/AS by GetFieldEntry, before this layout
                //pass runs, so overwriting it here doesn't lose anything. Left as-is, laying it out
                //like ordinary text content sizes the box to that string's width rather than the
                //small square a checkbox/radio should be; swap in a single glyph purely for this
                //measurement pass so width comes out based on one character, not the on-value text.
                this.Field.Value = "M";
                _addedProxyText = true;
                restoreValueAfterLayout = true;
            }
            else if (this.Field.FieldType == FormInputFieldType.Text && this.Field.Size > 0)
            {
                //HTML's size= means "roughly N average character widths", independent of the
                //field's actual value length - a short value in a size=20 field still gets a
                //20-character-wide box. The real value is already captured into the widget by
                //GetFieldEntry, before this layout pass runs, so swapping it here for a
                //Size-length placeholder only affects what our own appearance measures/paints -
                //and since NeedAppearances is on, a compliant reader regenerates the visible text
                //from /V anyway, so this placeholder is never what the user actually sees.
                this.Field.Value = new string('X', this.Field.Size);
                _addedProxyText = true;
                restoreValueAfterLayout = true;
            }
            else if (this.Field.FieldType == FormInputFieldType.Choice && this.Field is HTMLSelect select)
            {
                var longest = this.GetLongestOptionsText(select.Choices);

                if ((this.Field.Options & FormFieldOptions.Multiselect) == FormFieldOptions.Multiselect)
                {
                    if (Field.Size < 1)
                        this.Field.Size = 4;

                    // var multi = new StringBuilder();
                    // for (var i = 0; i < size; i++)
                    // {
                    //     if(multi.Length > 0)
                    //         multi.Append("V\r\n<br/>");
                    //     multi.Append(longest);
                    // }
                    //
                    //longest = multi.ToString();
                }
                this.Field.Value = longest + "V"; //add space for the dropdown/scroll bar too.

                _addedProxyText = true;
                restoreValueAfterLayout = true;
            }
            else
            {
                _addedProxyText = false;
            }

            base.DoLayoutComponent();

            if (restoreValueAfterLayout)
                this.Field.Value = originalValue;

            xObject.Close();
            this.Result = xObject;

            this.CompleteLineFlow(xObject, pos);
            this.RegisterAppearances(xObject, pos);
        }

        /// <summary>
        /// Wraps the field onto a fresh line if it overflowed the current one, or closes the
        /// line for a block-display field, then recalculates its final X offset. Overridden by
        /// LayoutEngineButtonState (a throwaway, isolated pass) as a no-op - that run is never
        /// attached to any real line/flow, so there's nothing here to complete.
        /// </summary>
        protected virtual void CompleteLineFlow(PDFLayoutXObjectRun xObject, PDFPositionOptions pos)
        {
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

            if (pos.PositionMode == PositionMode.Static)
            {
                //recalculate the offset
                var loc = Point.Empty;
                loc.X += this.Line.Width - xObject.ChildContainer.Width;

                xObject.SetOffsetX(loc.X);
            }
        }

        /// <summary>
        /// Registers the field's widget annotation and sets its Normal/Down/Over appearances -
        /// Down/Over default to a colour-only repaint of Normal's own box (see
        /// PDFAcrobatFormFieldWidget.WriteRepaintedAppearance) when a matching :hover/:active
        /// rule exists, or reuse Normal's exact xObject unchanged when none does. Overridden by
        /// LayoutEngineStatedButton to lay out genuinely independent appearances per state
        /// instead. Overridden by LayoutEngineButtonState (a throwaway, isolated pass) to just
        /// capture its own result rather than registering anything.
        /// </summary>
        protected virtual void RegisterAppearances(PDFLayoutXObjectRun xObject, PDFPositionOptions pos)
        {
            this.LayoutPage = this.Context.DocumentLayout.CurrentPage;
            IArtefactCollection annots;
            if (!this.LayoutPage.Artefacts.TryGetCollection(PDFArtefactTypes.Annotations, out annots))
            {
                annots = new PDFAnnotationCollection(PDFArtefactTypes.Annotations);
                this.LayoutPage.Artefacts.Add(annots);
            }

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
        }

        private string GetLongestOptionsText(FormFieldOptionList fieldOptions)
        {
            var found = string.Empty;
            foreach (var option in fieldOptions)
            {
                if(!string.IsNullOrEmpty(option.Label) &&  option.Label.Length > found.Length)
                    found = option.Label;
                
            }
            
            return found;
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

        protected virtual PDFLayoutXObjectRun CreateAndAddInput(PDFPositionOptions pos)
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

            PDFLayoutXObjectRun begin = this.Line.AddXObjectRun(this, this.Field, containerRegion, pos, this.FullStyle);

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
