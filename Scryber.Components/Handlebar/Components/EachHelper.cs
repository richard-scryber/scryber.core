namespace Scryber.Handlebar.Components;

/// <summary>
/// Inherits from the looping template component with an &lt;each&gt; element. When the parser pre-filters any handlebar {{#each .. }} tags,
/// they will be replaced with the 'each' element and namespace that maps to this component. Secondary parsing will then pick up the 'each' element and create an instance of this class.
/// </summary>
[PDFParsableComponent("each")]
public class EachHelper : Scryber.Html.Components.HTMLTemplate
{
    
}