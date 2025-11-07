using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Scryber.Components;

namespace ScryberDocs
{
    /// <summary>
    /// Converts CLAUDE.md to PDF using Scryber
    /// Simple markdown to HTML converter for basic formatting
    /// </summary>
    public class ClaudeMdToPdf
    {
        public static void Main(string[] args)
        {
            string mdPath = args.Length > 0 ? args[0] : "../../../CLAUDE.md";
            string outputPath = args.Length > 1 ? args[1] : "CLAUDE.pdf";

            GeneratePdf(mdPath, outputPath);
        }

        public static void GeneratePdf(string markdownPath, string outputPath)
        {
            // Read markdown content
            string markdown = File.ReadAllText(markdownPath);

            // Convert markdown to HTML
            string htmlContent = ConvertMarkdownToHtml(markdown);

            // Create complete HTML document
            string html = CreateHtmlDocument(htmlContent);

            // Save temporary HTML file
            string tempHtml = Path.Combine(Path.GetTempPath(), "claude_temp.html");
            File.WriteAllText(tempHtml, html);

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

                Console.WriteLine($"PDF generated successfully: {outputPath}");
            }
            finally
            {
                // Clean up temp file
                if (File.Exists(tempHtml))
                    File.Delete(tempHtml);
            }
        }

        private static string ConvertMarkdownToHtml(string markdown)
        {
            var html = new StringBuilder();
            var lines = markdown.Split('\n');
            bool inCodeBlock = false;
            string codeBlockLang = "";
            var codeLines = new List<string>();
            bool inList = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');

                // Handle code blocks
                if (line.StartsWith("```"))
                {
                    if (!inCodeBlock)
                    {
                        inCodeBlock = true;
                        codeBlockLang = line.Substring(3).Trim();
                        codeLines.Clear();
                    }
                    else
                    {
                        // End code block
                        html.AppendLine($"<div class=\"code-block\">");
                        if (!string.IsNullOrEmpty(codeBlockLang))
                        {
                            html.AppendLine($"<div class=\"code-lang\">{EscapeHtml(codeBlockLang)}</div>");
                        }
                        html.AppendLine("<pre><code>");
                        foreach (var codeLine in codeLines)
                        {
                            html.AppendLine(EscapeHtml(codeLine));
                        }
                        html.AppendLine("</code></pre>");
                        html.AppendLine("</div>");
                        inCodeBlock = false;
                        codeBlockLang = "";
                    }
                    continue;
                }

                if (inCodeBlock)
                {
                    codeLines.Add(line);
                    continue;
                }

                // Handle headings
                if (line.StartsWith("# "))
                {
                    html.AppendLine($"<h1>{EscapeHtml(line.Substring(2))}</h1>");
                }
                else if (line.StartsWith("## "))
                {
                    html.AppendLine($"<h2>{EscapeHtml(line.Substring(3))}</h2>");
                }
                else if (line.StartsWith("### "))
                {
                    html.AppendLine($"<h3>{EscapeHtml(line.Substring(4))}</h3>");
                }
                else if (line.StartsWith("#### "))
                {
                    html.AppendLine($"<h4>{EscapeHtml(line.Substring(5))}</h4>");
                }
                // Handle horizontal rules
                else if (line.Trim() == "---")
                {
                    if (inList)
                    {
                        html.AppendLine("</ul>");
                        inList = false;
                    }
                    html.AppendLine("<hr />");
                }
                // Handle blockquotes
                else if (line.StartsWith(">"))
                {
                    if (inList)
                    {
                        html.AppendLine("</ul>");
                        inList = false;
                    }
                    string content = line.Substring(1).Trim();
                    html.AppendLine($"<blockquote>{ProcessInlineFormatting(content)}</blockquote>");
                }
                // Handle lists
                else if (line.TrimStart().StartsWith("- ") || line.TrimStart().StartsWith("* "))
                {
                    if (!inList)
                    {
                        html.AppendLine("<ul>");
                        inList = true;
                    }
                    string content = line.TrimStart().Substring(2);
                    html.AppendLine($"<li>{ProcessInlineFormatting(content)}</li>");
                }
                // Handle ordered lists
                else if (Regex.IsMatch(line.TrimStart(), @"^\d+\. "))
                {
                    if (!inList)
                    {
                        html.AppendLine("<ol>");
                        inList = true;
                    }
                    string content = Regex.Replace(line.TrimStart(), @"^\d+\. ", "");
                    html.AppendLine($"<li>{ProcessInlineFormatting(content)}</li>");
                }
                // Handle empty lines
                else if (string.IsNullOrWhiteSpace(line))
                {
                    if (inList)
                    {
                        html.AppendLine("</ul>");
                        inList = false;
                    }
                    html.AppendLine("<p class=\"spacer\"></p>");
                }
                // Regular paragraph
                else
                {
                    if (inList)
                    {
                        html.AppendLine("</ul>");
                        inList = false;
                    }
                    html.AppendLine($"<p>{ProcessInlineFormatting(line)}</p>");
                }
            }

            if (inList)
            {
                html.AppendLine("</ul>");
            }

            return html.ToString();
        }

        private static string ProcessInlineFormatting(string text)
        {
            // Escape HTML first
            text = EscapeHtml(text);

            // Bold with **
            text = Regex.Replace(text, @"\*\*(.+?)\*\*", "<strong>$1</strong>");

            // Italic with *
            text = Regex.Replace(text, @"\*(.+?)\*", "<em>$1</em>");

            // Inline code with `
            text = Regex.Replace(text, @"`(.+?)`", "<code>$1</code>");

            // Links [text](url)
            text = Regex.Replace(text, @"\[(.+?)\]\((.+?)\)", "<a href=\"$2\">$1</a>");

            return text;
        }

        private static string EscapeHtml(string text)
        {
            return text
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;");
        }

        private static string CreateHtmlDocument(string content)
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"" />
    <title>Scryber.Core - CLAUDE.md</title>
    <style>
        /* Page Setup */
        @page {{
            size: A4;
            margin: 20mm 15mm;
        }}

        body {{
            font-family: 'Helvetica', 'Arial', sans-serif;
            font-size: 10pt;
            color: #2d2d2d;
            line-height: 1.6;
            margin: 0;
            padding: 0;
        }}

        /* Headings */
        h1 {{
            font-size: 24pt;
            color: #1a1a1a;
            margin: 30pt 0 15pt 0;
            font-weight: bold;
            border-bottom: 2pt solid #336699;
            padding-bottom: 8pt;
            page-break-after: avoid;
        }}

        h2 {{
            font-size: 18pt;
            color: #336699;
            margin: 25pt 0 12pt 0;
            font-weight: bold;
            page-break-after: avoid;
        }}

        h3 {{
            font-size: 14pt;
            color: #2d2d2d;
            margin: 20pt 0 10pt 0;
            font-weight: bold;
            page-break-after: avoid;
        }}

        h4 {{
            font-size: 12pt;
            color: #2d2d2d;
            margin: 15pt 0 8pt 0;
            font-weight: bold;
            page-break-after: avoid;
        }}

        /* Paragraphs */
        p {{
            margin: 0 0 10pt 0;
            text-align: left;
        }}

        p.spacer {{
            margin: 5pt 0;
        }}

        /* Code blocks */
        .code-block {{
            background: #f5f5f5;
            border: 1pt solid #ddd;
            border-left: 4pt solid #336699;
            margin: 12pt 0;
            page-break-inside: avoid;
        }}

        .code-lang {{
            background: #336699;
            color: white;
            padding: 4pt 8pt;
            font-size: 8pt;
            font-weight: bold;
            text-transform: uppercase;
        }}

        pre {{
            margin: 0;
            padding: 10pt;
            overflow-x: auto;
        }}

        code {{
            font-family: 'Courier New', Courier, monospace;
            font-size: 9pt;
            color: #2d2d2d;
        }}

        p code {{
            background: #f5f5f5;
            padding: 2pt 4pt;
            border: 1pt solid #ddd;
            border-radius: 2pt;
        }}

        /* Lists */
        ul, ol {{
            margin: 10pt 0 10pt 20pt;
            padding: 0;
        }}

        li {{
            margin: 5pt 0;
            padding-left: 5pt;
        }}

        /* Blockquotes */
        blockquote {{
            border-left: 4pt solid #336699;
            padding: 10pt 15pt;
            margin: 15pt 0;
            background: #f9f9f9;
            font-style: italic;
        }}

        /* Horizontal rules */
        hr {{
            border: none;
            border-top: 2pt solid #ddd;
            margin: 20pt 0;
        }}

        /* Links */
        a {{
            color: #336699;
            text-decoration: underline;
        }}

        /* Inline formatting */
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
{content}
</body>
</html>";
        }
    }
}
