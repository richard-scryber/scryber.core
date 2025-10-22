namespace Scryber.Handlebar.Components;

/// <summary>
/// Not used - switched to a choose structure instead
/// </summary>
[PDFParsableComponent("if")]
public class IfHelper : Scryber.Html.Components.HTMLIf
{
    [PDFElement("elseif")]
    public ElseIfHelper ElIfHelper { get; set; }
    
    [PDFElement("else")]
    public ElseHelper ElseHelper { get; set; }
}

/// <summary>
/// Not used - switched to a choose structure instead
/// </summary>
[PDFParsableComponent("elseif")]
public class ElseIfHelper : Scryber.Html.Components.HTMLIf
{
    [PDFElement("elseif")]
    public ElseIfHelper ElIfHelper { get; set; }
    
    [PDFElement("else")]
    public ElseHelper ElseHelper { get; set; }
}

/// <summary>
/// Not used - switched to a choose structure instead
/// </summary>
[PDFParsableComponent("else")]
public class ElseHelper : Scryber.Html.Components.HTMLIf
{
}