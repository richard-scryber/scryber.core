using System;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components;

public class HTMLColBase : VisualComponent
{
    private VerticalAlignment _verticalAlignment;
    private Unit _width;

    [PDFAttribute("span")] public int Span { get; set; } = -1;

    [PDFAttribute("class")]
    public override string StyleClass
    {
        get => base.StyleClass;
        set => base.StyleClass = value;
    }

    [PDFAttribute("align")]
    public override HorizontalAlignment HorizontalAlignment {
        get => base.HorizontalAlignment;
        set => base.HorizontalAlignment = value;
    }
    
    [PDFAttribute("bgcolor")]
    public override Color BackgroundColor {
        get => base.BackgroundColor;
        set => base.BackgroundColor = value;
    }

    [PDFAttribute("valign")]
    public override VerticalAlignment VerticalAlignment
    {
        get => base.VerticalAlignment;
        set => base.VerticalAlignment = value;
    }

    [PDFAttribute("width")]
    public override Unit Width
    {
        get => base.Width;
        set => base.Width = value;
    }

    /// <summary>
    /// Global Html hidden attribute used with xhtml as hidden='hidden'. A hidden col/colgroup marks its
    /// matching cells as not visible (see ApplyStyleToColumnCell), but - per the HTML/CSS table spec for
    /// a collapsed column - the column's width is still accounted for; only its content/rendering is
    /// suppressed.
    /// </summary>
    [PDFAttribute("hidden")]
    public string Hidden
    {
        get
        {
            if (this.Visible)
                return string.Empty;
            else
                return "hidden";
        }
        set
        {
            if (string.IsNullOrEmpty(value) || value != "hidden")
                this.Visible = true;
            else
                this.Visible = false;
        }
    }

    /// <summary>
    /// Defines the offset of this column or group within the table. Must be set before Applying styles to cells.
    /// </summary>
    public virtual int ColumnOffset { get; set; } = -1;

    /// <summary>
    /// The number of table columns this col/colgroup actually covers - an unset (-1) span defaults to 1
    /// column, per the HTML span attribute default. Used to advance the running column offset counter
    /// when walking a table's col/colgroup definitions in order.
    /// </summary>
    public virtual int EffectiveSpan => this.Span < 0 ? 1 : this.Span;


    public HTMLColBase(ObjectType type) : base(type)
    {
    }

    public virtual bool ApplyStyleToColumnCell(int colindex, IStyledComponent component)
    {
        if(this.ColumnOffset < 0)
            throw new InvalidOperationException("The column offset has not been set.");
        if (this.IsMatchingColumn(colindex))
        {
            this.DoApplyStyleToCell(component);
            return true;
        }
        return false;
    }

    protected virtual bool IsMatchingColumn(int colindex)
    {
        // An unset span (-1) defaults to covering exactly one column, matching the HTML span attribute
        // default. The match range is exclusive of ColumnOffset + span, so a span of N covers exactly
        // N columns (ColumnOffset .. ColumnOffset + N - 1), not N + 1.
        var span = this.EffectiveSpan;

        if (colindex >= this.ColumnOffset && colindex < this.ColumnOffset + span)
            return true;
        else
            return false;
    }

    protected virtual void DoApplyStyleToCell(IStyledComponent component)
    {
        var style = component.Style;
        this.Style.MergeInto(style, Style.DirectStylePriority - 1); //don't override explicit values on the cell.
        if (!string.IsNullOrEmpty(this.StyleClass))
        {
            var all = this.StyleClass;
            if(!string.IsNullOrEmpty(component.StyleClass))
                all = all + " " +  component.StyleClass;

            component.StyleClass = all;
        }

        // A hidden col/colgroup only ever hides a cell, never force-shows one that was already hidden
        // for its own reasons.
        if (!this.Visible && component is Component asComponent)
            asComponent.Visible = false;
    }
}

[PDFParsableComponent("colgroup")]
public class HTMLColGroup : HTMLColBase
{
    private HTMLColList _colList;
    [PDFArray(typeof(HTMLCol))]
    [PDFElement("")]
    public HTMLColList Columns {
        get
        {
            if (null == _colList)
                _colList = new HTMLColList(this.InnerContent);
            return _colList;
        }
    }

    public bool HasColumns
    {
        get
        {
            return this._colList != null &&  this._colList.Count > 0;
        }
    }
    
    public HTMLColGroup(): base((ObjectType)"colg")
    {}

    /// <summary>
    /// A colgroup's own span covers the number of columns it explicitly declares (span attribute),
    /// or - when it has no explicit span and instead delegates to child &lt;col&gt; elements - the sum
    /// of those children's own effective spans. A colgroup with neither an explicit span nor any
    /// children covers no columns at all.
    /// </summary>
    public override int EffectiveSpan
    {
        get
        {
            if (this.Span >= 0)
                return this.Span;
            else if (this.HasColumns)
            {
                int total = 0;
                foreach (var col in this.Columns)
                    total += col.EffectiveSpan;
                return total;
            }
            else
                return 0;
        }
    }

    override public bool ApplyStyleToColumnCell(int colindex, IStyledComponent component)
    {
        if (this.Span < 0 && this.ColumnOffset >= 0)
        {
            if (this.HasColumns)
            {
                foreach (var col in this.Columns)
                {
                    if(col.ApplyStyleToColumnCell(colindex, component))
                        return true;
                }
                
            }
            return false;
        }
        else
        {
            return base.ApplyStyleToColumnCell(colindex, component);
        }
    }
}

[PDFParsableComponent("col")]
public class HTMLCol : HTMLColBase
{
    public HTMLCol() : base((ObjectType)"colc")
    {
    }
    
}

public class HTMLColList : ComponentWrappingList<HTMLCol>
{
    public HTMLColList(ComponentList wrapped) : base(wrapped)
    {
    }
}