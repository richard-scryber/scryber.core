using System.Collections.Generic;
using System.Text;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.Text;

namespace Scryber.PDF.Layout;

public class LayoutEngineSelect : LayoutEngineFormField
{

    protected Size InputSize;
    protected bool IsMultiple;
    protected bool HasText = false;
    protected HTMLSelect Select;
    
    public LayoutEngineSelect(HTMLSelect select, IPDFLayoutEngine parent)
        : base(select, parent)
    {
        this.Select = select;
        this.ShouldProxyText = false;
        this.ShouldAddXObject = false; //We will do this for the actual text
        this.IsMultiple = (select.Options & FormFieldOptions.Multiselect) == FormFieldOptions.Multiselect;
    }

    protected override void DoLayoutComponent()
    {
        var size = this.CalculateInputSize();
        this.FullStyle.Size.Width = size.Width;
        this.FullStyle.Size.Height = size.Height;
        
        this.InputSize = size;
        
        base.DoLayoutComponent();

        //For an input we extract the padding and 
        if (null != this.Result)
        {
            var pos = this.FullStyle.CreatePostionOptions(true);
            
            var rect = new Rect(0,0,this.Result.Width,this.Result.Height);

            var offset = Point.Empty;

            if (null != this.Line)
            {
                offset = new Point(this.Line.Width - this.Result.Width, this.Line.OffsetY);
            }

            rect.X += pos.Padding.Left;
            rect.Y += pos.Padding.Top;
            rect.Width -= pos.Padding.Left + pos.Padding.Right;
            rect.Height -= pos.Padding.Top + pos.Padding.Bottom;
             
            
            this.Result.ClipRect = rect;
            this.Result.PositionOptions.ViewPort = new Rect(Point.Empty, rect.Size);
            
            //var region = this.Result.ChildContainer as PDFLayoutPositionedRegion;
            
            
            
            this.RegisterAppearances(this.Result, pos, offset);
        }
    }

    #region protected virtual Size CalculateInputSize()
    /// <summary>
    /// Returns the calculated size of the input field including any padding.
    /// </summary>
    /// <returns></returns>
    protected virtual Size CalculateInputSize()
    {
        var pos = this.FullStyle.CreatePostionOptions(false);
        Unit w =Unit.Empty;
        Unit h = Unit.Empty;
        bool hasW = false;
        bool hasH = false;
        
        if (pos.Width.HasValue)
        {
            w = pos.Width.Value;
            hasW = true;
            
            if (pos.Height.HasValue)
            {
                h = pos.Height.Value;
                return new Size(w, h);
            }
        }
        else if (pos.Height.HasValue)
        {
            h = pos.Height.Value;
            hasH = true;
        }

        var lines = GetProxyLines(pos);
        Unit maxW = Unit.Empty;
        Unit maxH = Unit.Empty;
        var textOpts = this.FullStyle.CreateTextOptions();
        
        var prevFont = this.Context.Graphics.CurrentFont;
        
        //Set the font that will be used for the measurement
        this.Field.Document.GetFontResource(textOpts.Font, true, true);
        this.Context.Graphics.SetCurrentFont(textOpts.Font);
        
        //enumerate each line, and calculate the maximum width and total height
        foreach (var line in lines)
        {
            var lineSize = this.Context.Graphics.MeasureString(line, 0, 
                this.Context.Graphics.ContainerSize, textOpts, 
                out var fitted, out var appendChar);
            if(lineSize.Width > maxW)
                maxW = lineSize.Width;
            if(lineSize.Height < textOpts.GetLineHeight())
                lineSize.Height = textOpts.GetLineHeight();
                
            
            maxH += lineSize.Height;
        }
        
        //if we have padding then this should be added to the required size
        maxW += pos.Padding.Left + pos.Padding.Right;
        maxH += pos.Padding.Top + pos.Padding.Bottom;
        
        if(!hasH)
            h = maxH;
        
        if(!hasW)
            w = maxW;
        
        if(pos.MaximumHeight.HasValue && h > pos.MaximumHeight.Value)
            h = pos.MaximumHeight.Value;
        
        if(pos.MaximumWidth.HasValue && w > pos.MaximumWidth.Value)
            w = pos.MaximumWidth.Value;
        
        if(pos.MinimumHeight.HasValue && h < pos.MinimumHeight.Value)
            h = pos.MinimumHeight.Value;
        
        if(pos.MinimumWidth.HasValue && w < pos.MinimumWidth.Value)
            w = pos.MinimumWidth.Value;
        
        //now remove padding so not added to the input size
        w -= pos.Padding.Left + pos.Padding.Right;
        h -= pos.Padding.Top + pos.Padding.Bottom;

        //explicitly set the actual width and height.
        pos.Width = w;
        pos.Height = h;
        //clear the mins and maxes
        pos.MaximumWidth = null;
        pos.MaximumHeight = null;
        pos.MinimumWidth = null;
        pos.MinimumHeight = null;
        
        //restore the last font if set
        if (null != prevFont)
            this.Context.Graphics.SetCurrentFont(prevFont);
        
        //return as our required size.
        return new Size(w, h);
        
    }

    /// <summary>
    /// Returns the text that should be measured for an input or text area,
    /// to match the rows and cols, or size values on the field.
    /// </summary>
    /// <param name="pos">The current position options</param>
    /// <returns></returns>
    private ICollection<string> GetProxyLines(PDFPositionOptions pos)
    {
        List<string> all = new List<string>();
        if (this.IsMultiple)
        {
            int rows = this.Field.Size;
            if (rows <= 0)
                rows = 4;
            
            int cols = this.GetLongestOptionString().Length;
            
            for (var r = 0; r < rows; r++)
            {
                var content = new string('X', cols - 1);
                content = "ỵ" + content;
                all.Add(content);
            }
            return all;
        }
        else
        {
            int size = this.Field.Size <= 0 ? 20 : this.Field.Size;
            var content = new string('X', size - 1);
            content = "ỵ" + content;
            
            return new string[]
            {
                content
            };
        }
    }

    private string GetLongestOptionString()
    {
        string longest = string.Empty;
        foreach (var choice in this.Select.Choices)
        {
            if(choice.Label.Length > longest.Length)
                longest = choice.Label;
        }
        return longest;
    }
    
    #endregion

    

    protected override void DoLayoutTextComponent(ITextComponent text, Style style)
    {
        if(text is Whitespace)
            return;
        
        if(null != this.Result)
            return;
        
        var pos = this.FullStyle.CreatePostionOptions(true);
        HasText = true;
        //We want to create or XObjectBlock here
        var block = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
        var region = block.CurrentRegion;
        
        PDFPositionOptions inline = new PDFPositionOptions();
        inline.PositionMode = PositionMode.Static;
        inline.DisplayMode = DisplayMode.Inline;
        inline.ColumnCount = 1;
        inline.FillWidth = true; //We have an explicit height.
        inline.OverflowAction = OverflowAction.Clip;
        inline.Width = this.InputSize.Width;
        inline.Height = this.InputSize.Height;
        inline.Padding = pos.Padding;

        var xObjRegion = this.BeginNewXObjectRegionForChild(inline, text, style);
        this.Line = region.CurrentItem as PDFLayoutLine;
        var chars = string.Empty;
        var restore = false;

        var literal = text as TextLiteral;
        if (this.IsMultiple)
        {
            // style.Text.PreserveWhitespace = true;
            // style.Text.WrapText = WordWrap.Word;
            var entries = this.GetAllEntries();
            chars = literal.Text;
            literal.Text = entries;
            literal.ReaderFormat = TextFormat.Plain;
            restore = true;

        }
        // if (null != literal && !this.IsMultiple)
        // {
        //     chars = literal.Text;
        //     
        //     literal.Text = string.Empty;
        //     restore = true;
        // }
        
        base.DoLayoutTextComponent(text, style);
        

        if (restore)
        {
            literal.Text = chars;
        }

        if (xObjRegion.CurrentItem != null)
            xObjRegion.CurrentItem.Close();
        if(xObjRegion.IsClosed == false)
            xObjRegion.Close();

        if (region.CurrentItem == null)
            region.BeginNewLine(this.InputSize.Height.PointsValue);
        
        var line = (PDFLayoutLine)region.CurrentItem;
        
        var run = line.AddXObjectRun(this, text, xObjRegion, inline, style);
        xObjRegion.ExcludeFromOutput = true;
        this.Result = run;
        
    }

    /// <summary>
    /// Gets all the label options in a string
    /// </summary>
    /// <returns></returns>
    protected string GetAllEntries(string separator = "\r\n")
    {
        var sb = new StringBuilder();
        
        foreach (var choice in this.Select.Choices)
        {
            if(sb.Length > 0)
                sb.Append(separator);
            sb.Append(choice.Label);
        }
        return sb.ToString();
    }
}