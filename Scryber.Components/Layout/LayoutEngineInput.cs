using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Layout
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
            PDFPositionOptions pos = this.FullStyle.CreatePostionOptions();

            PDFLayoutXObject xObject = this.CreateAndAddInput(pos);
            _addedProxyText = false;

            if(string.IsNullOrEmpty(this.Field.Value))
            {
                this.Field.Value = "Proxy Text";
                _addedProxyText = true;
            }

            base.DoLayoutComponent();

            

            xObject.Close();

            if (pos.PositionMode == Drawing.PositionMode.Block)
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
            this.Field.Widget.SetAppearance(FormFieldAppearanceState.Down, xObject, this.LayoutPage, this.FullStyle);
            this.Field.Widget.SetAppearance(FormFieldAppearanceState.Over, xObject, this.LayoutPage, this.FullStyle);
        }

        protected override void DoLayoutChildren()
        {
            base.DoLayoutChildren();
            
        }

        protected override void DoLayoutTextComponent(IPDFTextComponent text, PDFStyle style)
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

        private PDFLayoutXObject CreateAndAddInput(PDFPositionOptions pos)
        {
            PDFLayoutBlock containerBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion containerRegion = containerBlock.CurrentRegion;
            if (containerRegion.HasOpenItem == false)
                containerRegion.BeginNewLine();
            //pos.Y = 200;
            PDFLayoutRegion container = containerBlock.BeginNewPositionedRegion(pos, this.DocumentLayout.CurrentPage, this.Component, this.FullStyle, false);

            this.Line = containerRegion.CurrentItem as PDFLayoutLine;
            PDFLayoutXObject begin = this.Line.AddXObjectRun(this, this.Field, container, pos, this.FullStyle);

            return begin;
        }

        private void CloseCurrentLine()
        {

            if (!this.Line.IsClosed)
                this.Line.Region.CloseCurrentItem();

            
        }

    }
}
