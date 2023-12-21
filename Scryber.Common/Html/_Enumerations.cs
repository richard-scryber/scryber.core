using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Html
{
    public enum CSSBorder
    {
        Solid,
        Dotted,
        Dashed,
        Double,
        Groove,
        Ridge,
        Inset,
        Outset,
        Hidden,
        None
    }

    public enum HtmlComponentType
    {
        None,
        Body,
        Panel,
        Span,
        Text,
        Image,
        Table,
        TableRow,
        TableCell,
        List,
        ListItem,
        LineBreak,
        PageBreak,
        Heading,
        Preformatted,
        HorizontalLine,
        Link,
        DocType,
        Comment,
        ProcessingInstruction,
        CData,
        Unknown
    }

    public enum HtmlFormatType
    {
        Html,
        Markdown
    }

    /// <summary>
    /// The different types of understood css value types that the item reader knows about
    /// </summary>
    public enum CSSValueType
    {
        Text,
        Number,
        Color,
        Url,
        None
    }

}
