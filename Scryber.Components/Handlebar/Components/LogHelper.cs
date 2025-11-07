using System;
using Scryber;
using Scryber.Html.Components;
using Scryber.Data;

namespace Scryber.Handlebar.Components;


/// <summary>
/// Supplements the HTMLLogEntry component with support for the handlebars {{log ...}} entry.
/// </summary>
[PDFParsableComponent("log")]
public class LogHelper : HTMLLogEntry
{
    public LogHelper() : base()
    {
        
    }
}