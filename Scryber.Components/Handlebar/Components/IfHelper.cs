namespace Scryber.Handlebar.Components;

[PDFParsableComponent("if")]
public class IfHelper : Scryber.Html.Components.HTMLIf
{
    [PDFElement("elseif")]
    public ElseIfHelper ElIfHelper { get; set; }
    
    [PDFElement("else")]
    public ElseHelper ElseHelper { get; set; }
}

[PDFParsableComponent("elseif")]
public class ElseIfHelper : Scryber.Html.Components.HTMLIf
{
    [PDFElement("elseif")]
    public ElseIfHelper ElIfHelper { get; set; }
    
    [PDFElement("else")]
    public ElseHelper ElseHelper { get; set; }
}

[PDFParsableComponent("else")]
public class ElseHelper : Scryber.Html.Components.HTMLIf
{
}