#!/usr/bin/env dotnet-script
#r "nuget: Scryber.Core, 6.0.1.4-beta"

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Scryber.Components;

// Read markdown content
string markdown = File.ReadAllText("CLAUDE.md");

// Convert markdown to HTML (simplified)
var html = new StringBuilder();
html.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"" />
    <title>Scryber.Core Documentation</title>
    <style>
        @page { size: A4; margin: 20mm; }
        body { font-family: Arial; font-size: 10pt; line-height: 1.6; }
        h1 { font-size: 20pt; color: #336699; border-bottom: 2pt solid #336699; padding-bottom: 5pt; }
        h2 { font-size: 16pt; color: #336699; margin-top: 20pt; }
        h3 { font-size: 13pt; color: #2d2d2d; margin-top: 15pt; }
        code { font-family: Courier; font-size: 9pt; background: #f5f5f5; padding: 2pt 4pt; }
        pre { background: #f5f5f5; border-left: 4pt solid #336699; padding: 10pt; margin: 10pt 0; }
        pre code { background: none; padding: 0; }
        ul, ol { margin: 10pt 0; }
        li { margin: 5pt 0; }
        strong { font-weight: bold; }
    </style>
</head>
<body>");

bool inCodeBlock = false;
foreach (var line in markdown.Split('\n'))
{
    string trimmed = line.Trim();

    if (trimmed.StartsWith("```"))
    {
        if (!inCodeBlock)
        {
            html.AppendLine("<pre><code>");
            inCodeBlock = true;
        }
        else
        {
            html.AppendLine("</code></pre>");
            inCodeBlock = false;
        }
        continue;
    }

    if (inCodeBlock)
    {
        html.AppendLine(System.Net.WebUtility.HtmlEncode(line));
        continue;
    }

    if (trimmed.StartsWith("# "))
        html.AppendLine($"<h1>{System.Net.WebUtility.HtmlEncode(trimmed.Substring(2))}</h1>");
    else if (trimmed.StartsWith("## "))
        html.AppendLine($"<h2>{System.Net.WebUtility.HtmlEncode(trimmed.Substring(3))}</h2>");
    else if (trimmed.StartsWith("### "))
        html.AppendLine($"<h3>{System.Net.WebUtility.HtmlEncode(trimmed.Substring(4))}</h3>");
    else if (trimmed.StartsWith("- "))
        html.AppendLine($"<li>{System.Net.WebUtility.HtmlEncode(trimmed.Substring(2))}</li>");
    else if (!string.IsNullOrWhiteSpace(trimmed))
        html.AppendLine($"<p>{System.Net.WebUtility.HtmlEncode(trimmed)}</p>");
}

html.AppendLine("</body></html>");

// Save temp HTML
string tempHtml = "claude_temp.html";
File.WriteAllText(tempHtml, html.ToString());

// Generate PDF
using (var doc = Document.ParseDocument(tempHtml))
{
    using (var stream = File.Create("CLAUDE.pdf"))
    {
        doc.SaveAsPDF(stream);
    }
}

Console.WriteLine("PDF generated: CLAUDE.pdf");
File.Delete(tempHtml);
