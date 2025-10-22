using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Scryber.Components;

// Get paths
string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));
string claudeMdPath = Path.Combine(projectRoot, "../../CLAUDE.md");
string outputPath = Path.Combine(projectRoot, "../../CLAUDE.pdf");

Console.WriteLine($"Reading: {claudeMdPath}");
Console.WriteLine($"Output: {outputPath}");

if (!File.Exists(claudeMdPath))
{
    Console.WriteLine("ERROR: CLAUDE.md not found!");
    return 1;
}

// Read markdown
string markdown = File.ReadAllText(claudeMdPath);

// Convert to HTML
string htmlContent = ConvertMarkdownToSimpleHtml(markdown);

// Write temp HTML file
string tempHtml = Path.Combine(Path.GetTempPath(), "claude_doc.html");
File.WriteAllText(tempHtml, htmlContent);

Console.WriteLine("Generating PDF...");

try
{
    // Generate PDF using Scryber
    using (var doc = Document.ParseDocument(tempHtml))
    {
        using (var stream = new FileStream(outputPath, FileMode.Create))
        {
            doc.SaveAsPDF(stream);
        }
    }

    Console.WriteLine($"✓ PDF generated successfully: {outputPath}");
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
    return 1;
}
finally
{
    if (File.Exists(tempHtml))
        File.Delete(tempHtml);
}

static string ConvertMarkdownToSimpleHtml(string markdown)
{
    var body = new StringBuilder();
    var lines = markdown.Split('\n');

    bool inCodeBlock = false;
    var codeLines = new List<string>();
    string codeLang = "";

    foreach (var line in lines)
    {
        string trimmed = line.Trim();

        // Handle code blocks
        if (trimmed.StartsWith("```"))
        {
            if (!inCodeBlock)
            {
                inCodeBlock = true;
                codeLang = trimmed.Substring(3).Trim();
                codeLines.Clear();
            }
            else
            {
                // Output code block
                body.AppendLine("<div class=\"code-block\">");
                if (!string.IsNullOrEmpty(codeLang))
                {
                    body.AppendLine($"<div class=\"code-lang\">{Escape(codeLang)}</div>");
                }
                body.AppendLine("<pre><code>");
                foreach (var codeLine in codeLines)
                {
                    body.AppendLine(Escape(codeLine));
                }
                body.AppendLine("</code></pre></div>");
                inCodeBlock = false;
                codeLang = "";
            }
            continue;
        }

        if (inCodeBlock)
        {
            codeLines.Add(line);
            continue;
        }

        // Handle headings
        if (trimmed.StartsWith("#### "))
            body.AppendLine($"<h4>{Escape(trimmed.Substring(5))}</h4>");
        else if (trimmed.StartsWith("### "))
            body.AppendLine($"<h3>{Escape(trimmed.Substring(4))}</h3>");
        else if (trimmed.StartsWith("## "))
            body.AppendLine($"<h2>{Escape(trimmed.Substring(3))}</h2>");
        else if (trimmed.StartsWith("# "))
            body.AppendLine($"<h1>{Escape(trimmed.Substring(2))}</h1>");
        // Handle HR
        else if (trimmed == "---")
            body.AppendLine("<hr />");
        // Handle blockquote
        else if (trimmed.StartsWith(">"))
            body.AppendLine($"<blockquote>{ProcessInline(trimmed.Substring(1).Trim())}</blockquote>");
        // Handle lists
        else if (trimmed.StartsWith("- ") || trimmed.StartsWith("* "))
            body.AppendLine($"<li>{ProcessInline(trimmed.Substring(2))}</li>");
        else if (Regex.IsMatch(trimmed, @"^\d+\. "))
            body.AppendLine($"<li>{ProcessInline(Regex.Replace(trimmed, @"^\d+\. ", ""))}</li>");
        // Regular paragraph
        else if (!string.IsNullOrWhiteSpace(trimmed))
            body.AppendLine($"<p>{ProcessInline(trimmed)}</p>");
    }

    return CreateHtmlDocument(body.ToString());
}

static string ProcessInline(string text)
{
    text = Escape(text);
    // Bold
    text = Regex.Replace(text, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
    // Italic
    text = Regex.Replace(text, @"\*(.+?)\*", "<em>$1</em>");
    // Inline code
    text = Regex.Replace(text, @"`(.+?)`", "<code>$1</code>");
    // Links
    text = Regex.Replace(text, @"\[(.+?)\]\((.+?)\)", "<span class=\"link\">$1</span>");
    return text;
}

static string Escape(string text)
{
    return text
        .Replace("&", "&amp;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;");
}

static string CreateHtmlDocument(string bodyContent)
{
    return $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"" />
    <title>Scryber.Core - CLAUDE.md Documentation</title>
    <style>
        @page {{
            size: A4;
            margin: 20mm 15mm;
        }}

        body {{
            font-family: 'Helvetica', Arial, sans-serif;
            font-size: 9pt;
            color: #2d2d2d;
            line-height: 1.5;
        }}

        h1 {{
            font-size: 20pt;
            color: #1a1a1a;
            margin: 25pt 0 12pt 0;
            border-bottom: 2pt solid #336699;
            padding-bottom: 6pt;
            page-break-after: avoid;
        }}

        h2 {{
            font-size: 15pt;
            color: #336699;
            margin: 20pt 0 10pt 0;
            font-weight: bold;
            page-break-after: avoid;
        }}

        h3 {{
            font-size: 12pt;
            color: #2d2d2d;
            margin: 15pt 0 8pt 0;
            font-weight: bold;
            page-break-after: avoid;
        }}

        h4 {{
            font-size: 10pt;
            color: #2d2d2d;
            margin: 12pt 0 6pt 0;
            font-weight: bold;
            page-break-after: avoid;
        }}

        p {{
            margin: 0 0 8pt 0;
        }}

        .code-block {{
            background: #f8f8f8;
            border: 1pt solid #ddd;
            border-left: 3pt solid #336699;
            margin: 10pt 0;
            page-break-inside: avoid;
        }}

        .code-lang {{
            background: #336699;
            color: white;
            padding: 3pt 6pt;
            font-size: 7pt;
            font-weight: bold;
            text-transform: uppercase;
        }}

        pre {{
            margin: 0;
            padding: 8pt;
        }}

        code {{
            font-family: 'Courier New', Courier, monospace;
            font-size: 8pt;
        }}

        p code {{
            background: #f0f0f0;
            padding: 1pt 3pt;
            border: 1pt solid #ddd;
        }}

        ul, ol {{
            margin: 8pt 0 8pt 15pt;
            padding: 0;
        }}

        li {{
            margin: 4pt 0;
        }}

        blockquote {{
            border-left: 3pt solid #336699;
            padding: 8pt 12pt;
            margin: 10pt 0;
            background: #f9f9f9;
            font-style: italic;
        }}

        hr {{
            border: none;
            border-top: 1pt solid #ddd;
            margin: 15pt 0;
        }}

        .link {{
            color: #336699;
            text-decoration: underline;
        }}

        strong {{
            font-weight: bold;
            color: #1a1a1a;
        }}

        em {{
            font-style: italic;
        }}
    </style>
</head>
<body>
{bodyContent}
</body>
</html>";
}
