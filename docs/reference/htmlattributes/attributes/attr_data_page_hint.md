---
layout: default
title: data-page-hint
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-page-hint : The Page Count Hint Attribute

The `data-page-hint` attribute provides a hint about the total page count for documents, enabling optimized page number rendering and improved document generation performance. It helps the PDF generation engine estimate total pages before final layout calculation, particularly useful for page numbering in headers and footers.

## Summary

The `data-page-hint` attribute optimizes page numbering by providing an estimated total page count:

- Hints the expected total page count for performance optimization
- Enables early resolution of "Page X of Y" displays
- Reduces layout recalculation cycles during document generation
- Particularly useful for large documents with dynamic content
- Improves rendering speed for documents with page totals in headers/footers
- Does not affect the actual page count, only optimizes rendering

This attribute is essential for:
- Optimizing documents with page number totals
- Improving performance for large multi-page reports
- Reducing layout passes for complex documents
- Building efficient document generation pipelines
- Creating responsive page numbering systems
- Handling dynamic content with predictable page counts

---

## Usage

The `data-page-hint` attribute is used exclusively with the `<page>` element (page number component):

```html
<!-- Basic page number with hint -->
<page data-page-hint="50"></page>

<!-- Page number display with hint -->
<page data-format="Page {0} of {1}" data-page-hint="100"></page>

<!-- Dynamic hint from model -->
<page data-page-hint="{{model.estimatedPages}}"></page>

<!-- Property-specific usage -->
<page property="total" data-page-hint="{{model.totalPageEstimate}}"></page>
```

---

## Supported Elements

The `data-page-hint` attribute is supported exclusively on:

### Page Number Element
- `<page>` - The page number component for displaying page numbers and totals

**Note**: This is a specialized attribute for page numbering optimization and is not applicable to other elements.

---

## Binding Values

### Integer Values

**Type**: `int` (integer)
**Range**: Any positive integer (typically 1-10000)
**Default**: No default (hint is optional)
**Binding**: Supports data binding expressions

```html
<!-- Static hint -->
<page data-page-hint="50"></page>

<!-- Data binding -->
<page data-page-hint="{{model.expectedPages}}"></page>

<!-- Calculated hint -->
<page data-page-hint="{{model.items.count * 2 + 5}}"></page>

<!-- From template data -->
<page property="total" data-page-hint="{{model.pageCountEstimate}}"></page>
```

**Data Model Example**:
```csharp
public class DocumentModel
{
    public int ExpectedPages { get; set; }
    public int PageCountEstimate { get; set; }

    // Calculate estimate based on content
    public int CalculatePageEstimate()
    {
        // Rough calculation: items per page + header pages
        int itemPages = (Items.Count / 20) + 1;
        int headerPages = 3;
        int totalEstimate = itemPages + headerPages;

        return totalEstimate;
    }

    public List<Item> Items { get; set; }
}
```

---

## Notes

### How Page Hints Work

The `data-page-hint` attribute provides an optimization hint to the PDF generation engine:

1. **Initial Estimate**: Hint provides expected total page count before layout
2. **Early Resolution**: Enables early calculation of page number displays
3. **Layout Optimization**: Reduces iterative layout passes
4. **Final Accuracy**: Actual page count is still calculated accurately
5. **Performance Gain**: Particularly beneficial for large documents

**Without hint**:
```
1. Initial layout pass → unknown total pages
2. Calculate total pages → triggers relayout for "X of Y" displays
3. Second layout pass → finalize with correct totals
4. Additional passes if content shifts
```

**With hint**:
```
1. Initial layout pass → use hint for totals
2. Calculate actual total → if matches hint, done
3. Minimal relayout if hint was accurate
```

### When to Use Page Hints

Page hints provide the most benefit in these scenarios:

**High Value**:
- Large documents (50+ pages)
- Documents with "Page X of Y" in headers/footers
- Reports with predictable pagination
- Documents with repeating data templates
- Multi-section documents with known section lengths

**Limited Value**:
- Small documents (< 10 pages)
- Documents without page totals displayed
- Highly dynamic content with unpredictable length
- Single-page documents

```html
<!-- High value: large report with page totals -->
<header>
    <page data-format="Page {0} of {1}" data-page-hint="150"></page>
</header>

<!-- Limited value: small document -->
<footer>
    <page></page> <!-- Just page number, no hint needed -->
</footer>
```

### Accuracy of Hints

The hint does not need to be perfectly accurate:

**Good hint**: Within 10-20% of actual page count
**Acceptable hint**: Within 50% of actual page count
**Poor hint**: Off by more than 100%

```csharp
// Good hint calculation
public int CalculatePageHint()
{
    // Known: 40 items, ~5 items per page
    int dataPages = (Items.Count / 5) + 1; // = 9 pages

    // Known: 2 cover pages + 1 summary
    int staticPages = 3;

    // Estimate: 12 pages
    return dataPages + staticPages;
    // Actual might be 11-13, which is fine
}
```

### Impact of Inaccurate Hints

If the hint is significantly inaccurate:

- **Too Low**: May trigger additional layout pass (minor performance impact)
- **Too High**: Minimal negative impact, hint is adjusted automatically
- **Very Wrong**: Falls back to standard layout process

The system is resilient to hint inaccuracy - a rough estimate is better than no hint.

```html
<!-- Even a rough estimate helps -->
<page data-format="Page {0} of {1}" data-page-hint="50"></page>
<!-- If actual is 45 or 55, still provides optimization benefit -->
```

### Performance Considerations

Performance improvement scales with:

1. **Document Size**: Larger documents see more benefit
2. **Total Display Frequency**: More "X of Y" displays = more benefit
3. **Content Complexity**: Complex layouts benefit more
4. **Hint Accuracy**: More accurate hints = better performance

**Benchmark example** (100-page document):
- Without hint: 3-4 layout passes
- With accurate hint: 1-2 layout passes
- Time savings: 20-40% for documents with page totals

### Page Number Display Formats

The hint is most useful when displaying total page counts:

```html
<!-- Shows "Page 5 of 100" -->
<page data-format="Page {0} of {1}" data-page-hint="100"></page>

<!-- Shows "100" (total only) -->
<page property="total" data-page-hint="100"></page>

<!-- Shows "5" (current page only, hint less useful) -->
<page></page>

<!-- Shows "Section 2/5" -->
<page property="section" data-format="{2} / {3}" data-page-hint="50"></page>
```

**Format placeholders**:
- `{0}` - Current page number
- `{1}` - Total page count (benefits from hint)
- `{2}` - Current section page
- `{3}` - Section total pages

### Dynamic Hint Calculation

Calculate hints based on content:

```csharp
public class ReportModel
{
    public List<DataItem> Items { get; set; }

    public int PageHint
    {
        get
        {
            // Estimate based on content
            const int itemsPerPage = 20;
            const int headerPages = 2;
            const int footerPages = 1;

            int dataPages = (Items.Count / itemsPerPage) + 1;
            return headerPages + dataPages + footerPages;
        }
    }
}
```

```html
<!-- Use calculated hint -->
<footer>
    <page data-format="Page {0} of {1}"
          data-page-hint="{{model.pageHint}}"></page>
</footer>
```

### Sections and Page Hints

For documents with multiple sections:

```html
<!-- Hint for entire document -->
<page property="total" data-page-hint="200"></page>

<!-- Section-specific hints -->
<page property="sectionTotal" data-page-hint="50"></page>
```

The hint applies to the scope of the page number display (document total or section total).

### Headers and Footers

Common use case for page hints:

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        @page {
            @bottom-center {
                content: "Page " counter(page) " of " counter(pages);
            }
        }
    </style>
</head>
<body>
    <header>
        <div style="text-align: right;">
            <page data-format="Page {0} of {1}"
                  data-page-hint="{{model.totalPages}}"></page>
        </div>
    </header>

    <!-- Document content -->
    <div class="content">
        <!-- ... -->
    </div>

    <footer>
        <div style="text-align: center;">
            <page data-format="{0} / {1}"
                  data-page-hint="{{model.totalPages}}"></page>
        </div>
    </footer>
</body>
</html>
```

### Null or Missing Hint

If `data-page-hint` is not provided or is null:

- Standard page numbering layout process is used
- No performance optimization from hinting
- Document still renders correctly
- Multiple layout passes may be needed for totals

```html
<!-- These work identically in terms of output -->
<page data-format="Page {0} of {1}"></page>
<page data-format="Page {0} of {1}" data-page-hint="50"></page>
<!-- Second version may render faster -->
```

### Zero or Negative Hints

Invalid hint values are handled gracefully:

```html
<!-- Invalid hints are ignored -->
<page data-page-hint="0"></page>     <!-- Ignored -->
<page data-page-hint="-10"></page>   <!-- Ignored -->
<page data-page-hint=""></page>      <!-- Ignored -->
```

The system falls back to standard layout without the hint.

### Best Practices

**Hint Calculation**:
- Base hints on content metrics (item counts, section counts)
- Include static pages (covers, TOC, appendices)
- Round up rather than down
- Don't worry about perfect accuracy

**Implementation**:
- Calculate hints server-side before template rendering
- Use consistent calculation logic across document types
- Document your hint calculation strategy
- Test with various content sizes

**Maintenance**:
- Review hints when template layouts change significantly
- Update calculations if items-per-page estimates change
- Monitor actual vs. hinted page counts in production
- Adjust algorithms based on patterns

```csharp
// Good practice: centralized hint calculation
public class PageHintCalculator
{
    public static int CalculateForReport(ReportData data)
    {
        const int coverPages = 2;
        const int tocPages = 1;
        const int itemsPerPage = 15;

        int contentPages = (data.Items.Count / itemsPerPage) + 1;
        int appendixPages = data.IncludeAppendix ? 5 : 0;

        return coverPages + tocPages + contentPages + appendixPages;
    }
}
```

---

## Examples

### 1. Basic Page Number with Hint

Simple page numbering with total page hint:

```html
<footer style="text-align: center; padding: 10pt;">
    <page data-format="Page {0} of {1}" data-page-hint="25"></page>
</footer>
```

### 2. Dynamic Hint from Model

Calculate hint based on content:

```csharp
public class ReportModel
{
    public int EstimatedPages => CalculatePageEstimate();

    private int CalculatePageEstimate()
    {
        // 20 items per page
        int dataPages = (DataItems.Count / 20) + 1;

        // Add static pages
        int staticPages = 3; // Cover + TOC + Summary

        return dataPages + staticPages;
    }

    public List<DataItem> DataItems { get; set; }
}
```

```html
<footer>
    <page data-format="Page {0} of {1}"
          data-page-hint="{{model.estimatedPages}}"></page>
</footer>
```

### 3. Large Report with Accurate Hint

Optimize large document rendering:

```html
<!-- Model: { totalItems: 1000 } -->

<!DOCTYPE html>
<html>
<head>
    <title>Annual Sales Report</title>
</head>
<body>
    <header style="text-align: right; padding: 10pt; border-bottom: 1pt solid #ccc;">
        <!-- Hint based on 20 items per page + 5 header pages -->
        <page data-format="Page {0} of {1}"
              data-page-hint="{{(model.totalItems / 20) + 5}}"></page>
    </header>

    <!-- Document content -->
    <h1>Sales Report</h1>

    <template data-bind="{{model.salesData}}">
        <div class="sales-item">
            <!-- ... -->
        </div>
    </template>

    <footer style="text-align: center; padding: 10pt; border-top: 1pt solid #ccc;">
        <page data-format="{0} of {1}"
              data-page-hint="{{(model.totalItems / 20) + 5}}"></page>
    </footer>
</body>
</html>
```

### 4. Total Page Count Display Only

Display only total page count:

```html
<div style="text-align: center; margin: 20pt 0;">
    <p>
        This document contains
        <page property="total" data-page-hint="75"></page>
        pages.
    </p>
</div>
```

### 5. Multi-Section Document with Section Hints

Different hints for different sections:

```csharp
public class MultiSectionReport
{
    public Section ExecutiveSummary { get; set; } // ~5 pages
    public Section DetailedAnalysis { get; set; } // ~40 pages
    public Section Appendices { get; set; }       // ~15 pages

    public int TotalPageHint => 5 + 40 + 15; // 60 pages
}
```

```html
<!DOCTYPE html>
<html>
<body>
    <!-- Overall document header -->
    <header>
        <div style="float: right;">
            <page data-format="Page {0} of {1}"
                  data-page-hint="{{model.totalPageHint}}"></page>
        </div>
    </header>

    <!-- Executive Summary Section -->
    <div>
        <h1>Executive Summary</h1>
        <iframe src="sections/exec-summary.html"></iframe>
        <div style="text-align: right; margin-top: 10pt;">
            <small>
                Section: <page property="section" data-page-hint="5"></page>
            </small>
        </div>
    </div>

    <!-- Detailed Analysis Section -->
    <div style="page-break-before: always;">
        <h1>Detailed Analysis</h1>
        <iframe src="sections/detailed-analysis.html"></iframe>
        <div style="text-align: right; margin-top: 10pt;">
            <small>
                Section: <page property="section" data-page-hint="40"></page>
            </small>
        </div>
    </div>

    <!-- Appendices -->
    <div style="page-break-before: always;">
        <h1>Appendices</h1>
        <iframe src="sections/appendices.html"></iframe>
        <div style="text-align: right; margin-top: 10pt;">
            <small>
                Section: <page property="section" data-page-hint="15"></page>
            </small>
        </div>
    </div>
</body>
</html>
```

### 6. Conditional Page Hint Based on Content

Adjust hint based on content flags:

```csharp
public class FlexibleReport
{
    public bool IncludeAppendix { get; set; }
    public bool IncludeCharts { get; set; }
    public List<Item> Items { get; set; }

    public int PageHint
    {
        get
        {
            int basePages = 3; // Cover, TOC, Summary

            // Data pages
            int itemsPerPage = IncludeCharts ? 10 : 20;
            int dataPages = (Items.Count / itemsPerPage) + 1;

            // Optional sections
            int appendixPages = IncludeAppendix ? 8 : 0;

            return basePages + dataPages + appendixPages;
        }
    }
}
```

```html
<footer>
    <page data-format="Page {0} of {1}"
          data-page-hint="{{model.pageHint}}"></page>
</footer>
```

### 7. Invoice Series with Predictable Pagination

Generate multiple invoices with known page counts:

```csharp
public class InvoiceBatch
{
    public List<Invoice> Invoices { get; set; }

    public int TotalPageHint
    {
        get
        {
            // Each invoice is 2 pages
            int invoicePages = Invoices.Count * 2;

            // Cover sheet: 1 page
            int coverPages = 1;

            return coverPages + invoicePages;
        }
    }
}
```

```html
<!DOCTYPE html>
<html>
<body>
    <header>
        <page data-format="Page {0} of {1}"
              data-page-hint="{{model.totalPageHint}}"></page>
    </header>

    <h1>Invoice Batch {{model.batchNumber}}</h1>

    <template data-bind="{{model.invoices}}">
        <div style="page-break-before: always;">
            <!-- Invoice content (2 pages each) -->
            <iframe src="invoice-template.html"></iframe>
        </div>
    </template>
</body>
</html>
```

### 8. Catalog with Item-Based Hint Calculation

Product catalog with calculated page hint:

```csharp
public class ProductCatalog
{
    public List<Category> Categories { get; set; }

    public int CalculatePageHint()
    {
        int totalProducts = Categories.Sum(c => c.Products.Count);

        // Layout: 6 products per page in 2-column layout
        int productsPerPage = 6;
        int productPages = (totalProducts / productsPerPage) + 1;

        // Front matter: cover + TOC + intro
        int frontPages = 3;

        // Back matter: index
        int backPages = 2;

        return frontPages + productPages + backPages;
    }
}
```

```html
<footer style="text-align: center;">
    <page data-format="{0} / {1}"
          data-page-hint="{{model.calculatePageHint()}}"></page>
</footer>
```

### 9. Academic Paper with Bibliography

Research paper with estimated bibliography length:

```csharp
public class ResearchPaper
{
    public string Content { get; set; }
    public List<Citation> Bibliography { get; set; }

    public int PageEstimate
    {
        get
        {
            // Estimate main content (rough: 500 words per page)
            int wordCount = Content.Split(' ').Length;
            int contentPages = (wordCount / 500) + 1;

            // Bibliography (15 citations per page)
            int bibPages = (Bibliography.Count / 15) + 1;

            // Front matter (title, abstract)
            int frontPages = 2;

            return frontPages + contentPages + bibPages;
        }
    }
}
```

```html
<footer style="border-top: 1pt solid #333; padding-top: 5pt; text-align: center;">
    <page data-page-hint="{{model.pageEstimate}}"></page>
</footer>
```

### 10. Monthly Newsletter with Known Sections

Newsletter with predictable section lengths:

```csharp
public class Newsletter
{
    public Article FeatureArticle { get; set; }      // 3 pages
    public List<Article> ShortArticles { get; set; } // 1 page each
    public List<Event> Events { get; set; }          // 2 pages total

    public int PageHint
    {
        get
        {
            int coverPage = 1;
            int featurePages = 3;
            int shortArticlePages = ShortArticles.Count * 1;
            int eventPages = 2;
            int backCoverPage = 1;

            return coverPage + featurePages + shortArticlePages + eventPages + backCoverPage;
        }
    }
}
```

```html
<footer>
    <div style="text-align: center; font-size: 9pt; color: #666;">
        Newsletter - <page data-format="Page {0} of {1}"
                           data-page-hint="{{model.pageHint}}"></page>
    </div>
</footer>
```

### 11. Conference Proceedings with Paper Collection

Multiple papers with known lengths:

```csharp
public class ConferenceProceedings
{
    public List<Paper> Papers { get; set; }

    public int TotalPageHint
    {
        get
        {
            // Front matter
            int frontPages = 5; // Cover, foreword, TOC, etc.

            // Each paper is 6-12 pages, average 8
            int paperPages = Papers.Count * 8;

            // Author index
            int indexPages = 3;

            return frontPages + paperPages + indexPages;
        }
    }
}
```

```html
<header>
    <div style="float: right; font-size: 9pt;">
        <page data-format="Page {0} of {1}"
              data-page-hint="{{model.totalPageHint}}"></page>
    </div>
</header>
```

### 12. Data-Driven Report with Table Pagination

Report with known table row counts:

```csharp
public class DataReport
{
    public List<DataRow> Rows { get; set; }

    public int PageHintForTable
    {
        get
        {
            // Table header and styling uses ~50pt per row
            // Page has ~700pt usable space
            // Therefore ~14 rows per page

            const int rowsPerPage = 14;
            int tablePages = (Rows.Count / rowsPerPage) + 1;

            // Report has header (1 page) and footer (1 page)
            int wrapperPages = 2;

            return tablePages + wrapperPages;
        }
    }
}
```

```html
<!DOCTYPE html>
<html>
<body>
    <header>
        <page data-format="Page {0} of {1}"
              data-page-hint="{{model.pageHintForTable}}"></page>
    </header>

    <h1>Data Report</h1>

    <table style="width: 100%;">
        <thead>
            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Value</th>
            </tr>
        </thead>
        <tbody>
            <template data-bind="{{model.rows}}">
                <tr>
                    <td>{{.id}}</td>
                    <td>{{.name}}</td>
                    <td>{{.value}}</td>
                </tr>
            </template>
        </tbody>
    </table>
</body>
</html>
```

### 13. Manual with Dynamic Chapter Inclusion

User manual with optional chapters:

```csharp
public class UserManual
{
    public bool IncludeAdvancedTopics { get; set; }
    public bool IncludeTroubleshooting { get; set; }
    public bool IncludeAPIReference { get; set; }

    public int PageHint
    {
        get
        {
            int basePages = 20; // Introduction, basics, etc.

            int advancedPages = IncludeAdvancedTopics ? 30 : 0;
            int troubleshootingPages = IncludeTroubleshooting ? 15 : 0;
            int apiPages = IncludeAPIReference ? 50 : 0;

            return basePages + advancedPages + troubleshootingPages + apiPages;
        }
    }
}
```

```html
<footer>
    <page data-format="Page {0} of {1}"
          data-page-hint="{{model.pageHint}}"></page>
</footer>
```

### 14. Batch Processing with Consistent Documents

Generate multiple similar documents:

```csharp
public class BatchProcessor
{
    public List<CustomerReport> Reports { get; set; }

    // Each customer report is consistently 3 pages
    public int PagesPerReport => 3;

    public int TotalPagesHint => Reports.Count * PagesPerReport + 1; // +1 for batch cover
}
```

```html
<template data-bind="{{model.reports}}">
    <div style="page-break-before: always;">
        <header>
            <page data-format="Page {0} of {1}"
                  data-page-hint="{{model.totalPagesHint}}"></page>
        </header>

        <!-- Customer report content (3 pages) -->
        <iframe src="customer-report-template.html"></iframe>
    </div>
</template>
```

### 15. Real-Time Hint Adjustment

Adjust hint based on runtime calculations:

```csharp
public class AdaptiveReport
{
    public List<Section> Sections { get; set; }

    public int CalculateDynamicHint()
    {
        int totalPages = 0;

        foreach (var section in Sections)
        {
            // Each section knows its estimated length
            totalPages += section.EstimatedPageCount;
        }

        // Add 10% buffer
        return (int)(totalPages * 1.1);
    }

    public int PageHint => CalculateDynamicHint();
}
```

```html
<!-- Hint recalculates for each document -->
<footer>
    <page data-format="Page {0} of {1}"
          data-page-hint="{{model.pageHint}}"></page>
</footer>
```

### 16. Performance Comparison View

Test document with and without hints:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Performance Test</title>
</head>
<body>
    <!-- Model: { testMode: true, estimatedPages: 200 } -->

    <div>
        <h1>Performance Test Document</h1>
        <p>This document tests rendering performance with page hints.</p>

        <!-- Generate 200 pages of content -->
        <template data-bind="{{range(1, 200)}}">
            <div style="page-break-after: always;">
                <h2>Page {{.}} Content</h2>
                <p>This is test content for page {{.}}.</p>
            </div>
        </template>
    </div>

    <!-- Footer with hint -->
    <footer style="text-align: center;">
        <page data-format="Page {0} of {1}"
              data-page-hint="{{model.testMode ? model.estimatedPages : 0}}"></page>
    </footer>
</body>
</html>
```

---

## See Also

- [page element](/reference/htmltags/page.html) - Page number element documentation
- [data-format attribute](/reference/htmlattributes/data-format.html) - Page number format control
- [property attribute](/reference/htmlattributes/property.html) - Page property selection
- [for attribute](/reference/htmlattributes/for.html) - Page number component reference
- [Page Numbering](/reference/pagenumbering/) - Complete page numbering guide
- [Document Performance](/reference/performance/) - Performance optimization techniques
- [Headers and Footers](/reference/headers-footers/) - Header and footer implementation

---
