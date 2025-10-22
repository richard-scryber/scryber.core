using Scryber.Data;

namespace Scryber.Handlebar.Components;

/// <summary>
/// Outer wrapper for a chain of {{#if}} {{else if}} ... {{else}} {{/if}} that will be converted to Choose .. When .. When .. Otherwise contents
/// </summary>
[PDFParsableComponent("choose")]
public class ChooseHelper : Scryber.Data.Choose
{
    /// <summary>
    /// Use the otherwise helper class and push it back to the base Otherwise property for stasn
    /// </summary>
    [PDFElement("otherwise")]
    public ChooseOtherwiseHelper OtherwiseHelper
    {
        get{ return base.Otherwise as ChooseOtherwiseHelper; }
        set { base.Otherwise = value; }
    }
}

/// <summary>
/// The where decision choice (either from an {{#if }} test or an {{else if   }} test in a handlebars template
/// </summary>
[PDFParsableComponent("when")]
public class ChooseWhenHelper : Scryber.Data.ChooseWhen
{
    [PDFAttribute("data-test")]
    public override bool Test
    {
        get { return base.Test;}
        set { base.Test = value; }
    }

    [PDFTemplate]
    [PDFElement("")]
    public override ITemplate Template
    {
        get { return base.Template;}
        set { base.Template = value; }
    }
}

/// <summary>
/// The final otherwise decision choice (either from an {{else }} inside an {{#if }} [{{else if   }}] test chain in a
/// handlebars template. The template will be used if all other tests are false in a chain.
/// </summary>
[PDFParsableComponent("otherwise")]
public class ChooseOtherwiseHelper : Scryber.Data.ChooseOtherwise
{
    [PDFTemplate]
    [PDFElement("")]
    public override ITemplate Template
    {
        get { return base.Template; }
        set { base.Template = value; }
    }
    
}