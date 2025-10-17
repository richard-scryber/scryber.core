---
layout: default
title: data-page attributes
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-page-* : Frame Page Selection Attributes

The `data-page-start` and `data-page-count` attributes control which pages from a source document are inserted when using `<frame>` elements in Scryber PDF documents. These attributes enable selective page inclusion for document assembly and page range operations.

---

## Summary

The `data-page-*` attributes provide fine-grained control over page insertion when assembling PDF documents from multiple sources. They work specifically with the `<frame>` element to:

- Select specific pages from source PDFs
- Define page ranges to include
- Extract page subsets from templates
- Assemble multi-source documents
- Create merged documents from page selections

These attributes are essential for document assembly scenarios where you need to:
- Include only certain pages from a multi-page PDF
- Extract a range of pages from a larger document
- Combine specific pages from multiple sources
- Build custom documents from existing page libraries
- Create personalized documents with selected content

---

## Attributes Overview

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `data-page-start` | integer | Zero-based index of first page to include | 0 (first page) |
| `data-page-count` | integer | Number of pages to include from start index | All pages (int.MaxValue) |

---

## Usage

These attributes are applied to `<frame>` elements within a `<frameset>`:

### Basic Syntax

```html
<frameset>
    <!-- Include all pages (default) -->
    <frame src="document.pdf"></frame>

    <!-- Include only first page -->
    <frame src="document.pdf" data-page-count="1"></frame>

    <!-- Include pages 3-5 (indices 2-4) -->
    <frame src="document.pdf" data-page-start="2" data-page-count="3"></frame>

    <!-- Include from page 10 to end -->
    <frame src="document.pdf" data-page-start="9"></frame>
</frameset>
```

---

## Supported Elements

### Frame Element

- `<frame>` - Frame element for document inclusion (HTMLFrame)

**Requirements**:
- Must be within a `<frameset>` element
- Must have `src` attribute pointing to PDF or template
- Source document must exist and be accessible

---

## Attribute Details

### data-page-start

Specifies the zero-based index of the first page to include from the source document.

**Type**: Integer

**Default**: 0 (first page)

**Zero-Based**: Yes (page 1 = index 0, page 2 = index 1, etc.)

**Behavior**:
- Starts inclusion at specified page index
- Combined with `data-page-count` to define range
- If greater than total pages, no pages included
- Negative values treated as 0

**Property Mapping**: `PageStartIndex` on HTMLFrame component

```html
<!-- Include from second page onward -->
<frame src="report.pdf" data-page-start="1"></frame>

<!-- Include from page 10 onward -->
<frame src="report.pdf" data-page-start="9"></frame>
```

---

### data-page-count

Specifies the maximum number of pages to include starting from the start index.

**Type**: Integer

**Default**: `int.MaxValue` (all remaining pages)

**Behavior**:
- Limits the number of pages included
- Combined with `data-page-start` to define range
- If count exceeds available pages, includes all remaining
- Value of 0 includes no pages
- Negative values treated as 0

**Property Mapping**: `PageInsertCount` on HTMLFrame component

```html
<!-- Include only first page -->
<frame src="report.pdf" data-page-count="1"></frame>

<!-- Include first 3 pages -->
<frame src="report.pdf" data-page-start="0" data-page-count="3"></frame>

<!-- Include pages 5-7 (indices 4-6) -->
<frame src="report.pdf" data-page-start="4" data-page-count="3"></frame>
```

---

## Page Range Calculations

The effective page range is calculated as:

**Start Page** = `data-page-start` (zero-based)
**End Page** = `data-page-start + data-page-count - 1`

### Examples

| start | count | Included Pages (1-based) | Description |
|-------|-------|-------------------------|-------------|
| 0 | 1 | Page 1 | First page only |
| 0 | 3 | Pages 1-3 | First three pages |
| 2 | 2 | Pages 3-4 | Two pages starting at page 3 |
| 5 | - | Pages 6 to end | From page 6 onward |
| 0 | - | All pages | All pages (default) |

---

## Notes

### Zero-Based Indexing

**Important**: `data-page-start` uses zero-based indexing:
- First page (page 1) = index 0
- Second page (page 2) = index 1
- Third page (page 3) = index 2
- And so on...

This is consistent with array indexing in programming languages.

### Source Document Types

These attributes work with:
1. **PDF Files**: Direct PDF file inclusion
2. **Template Files**: HTML/XHTML templates that generate pages
3. **Inline HTML**: HTML documents within the frame

**Note**: For templates, pages are counted after template generation and layout.

### Document Assembly Process

When frames with page attributes are processed:

1. **Source Loading**: Document or template is loaded
2. **Page Generation**: If template, rendered to generate pages
3. **Page Selection**: Specified range extracted
4. **Insertion**: Selected pages inserted into result
5. **Page Numbering**: Adjusted based on insertion point

### Modification Types

The frame's modification type is determined automatically:

- **Append**: When no explicit page start, or start is at end
- **Insert**: When specific page start index is set
- **None**: When frame is hidden or has no content

### Error Handling

**Out of Range Indices**:
- Start beyond document length: No pages included
- Count exceeds available pages: Includes remaining pages
- No errors thrown for out-of-range values

**Missing Sources**:
- Strict mode: Throws exception
- Lax mode: Logs warning, continues

### Performance Considerations

- Page extraction is efficient (no re-rendering required for PDFs)
- Template frames require generation before extraction
- Large page ranges have minimal overhead
- Consider memory for large multi-page assemblies

### Page Numbering Implications

When assembling documents with page ranges:
- Page numbers in included content are preserved
- Document-wide page numbering may need adjustment
- Consider using page number restart markers
- Total page counts reflect final assembled document

### Visibility and Frames

Hidden frames (using `hidden="hidden"`) are not processed:
- Page attributes ignored if frame hidden
- Useful for conditional document assembly
- ModificationType set to None for hidden frames

---

## Examples

### 1. Include Complete Document

Default behavior includes all pages:

```html
<frameset>
    <frame src="header.pdf"></frame>
    <frame src="report.pdf"></frame>
    <frame src="footer.pdf"></frame>
</frameset>

<!-- All pages from report.pdf included -->
```

### 2. Include First Page Only

Extract cover page:

```html
<frameset>
    <frame src="report.pdf" data-page-count="1"></frame>
</frameset>

<!-- Only page 1 from report.pdf -->
```

### 3. Include Specific Page Range

Extract pages 3 through 5:

```html
<frameset>
    <!-- Pages 3-5 (indices 2-4) -->
    <frame src="manual.pdf" data-page-start="2" data-page-count="3"></frame>
</frameset>

<!-- Pages 3, 4, 5 from manual.pdf -->
```

### 4. Skip First Page

Exclude cover, include rest:

```html
<frameset>
    <!-- Start from second page (index 1), include all remaining -->
    <frame src="document.pdf" data-page-start="1"></frame>
</frameset>

<!-- All pages except page 1 -->
```

### 5. Multi-Source Document Assembly

Combine pages from multiple sources:

```html
<frameset>
    <!-- Cover page -->
    <frame src="cover.pdf" data-page-count="1"></frame>

    <!-- Table of contents (pages 2-3 from master) -->
    <frame src="master.pdf" data-page-start="1" data-page-count="2"></frame>

    <!-- Main content -->
    <frame src="content.pdf"></frame>

    <!-- Appendix (last 5 pages from reference) -->
    <frame src="reference.pdf" data-page-start="45" data-page-count="5"></frame>

    <!-- Back cover -->
    <frame src="cover.pdf" data-page-start="1" data-page-count="1"></frame>
</frameset>
```

### 6. Extract Chapter from Book

Get specific chapter pages:

```html
<frameset>
    <!-- Chapter 3: pages 42-58 (17 pages) -->
    <frame src="book.pdf" data-page-start="41" data-page-count="17"></frame>
</frameset>
```

### 7. Conditional Page Inclusion

Include pages based on data:

```html
<frameset>
    <frame src="report.pdf"
           data-page-start="{{model.startPage}}"
           data-page-count="{{model.pageCount}}"></frame>
</frameset>

<!-- Model: { startPage: 5, pageCount: 3 } -->
<!-- Includes pages 6-8 -->
```

### 8. Summary Document

Create executive summary from full report:

```html
<frameset>
    <!-- Title page -->
    <frame src="full-report.pdf" data-page-count="1"></frame>

    <!-- Executive summary (pages 2-4) -->
    <frame src="full-report.pdf" data-page-start="1" data-page-count="3"></frame>

    <!-- Key findings (page 25) -->
    <frame src="full-report.pdf" data-page-start="24" data-page-count="1"></frame>

    <!-- Recommendations (pages 50-52) -->
    <frame src="full-report.pdf" data-page-start="49" data-page-count="3"></frame>
</frameset>
```

### 9. Skip Pages in Middle

Include beginning and end, skip middle:

```html
<frameset>
    <!-- First 10 pages -->
    <frame src="document.pdf" data-page-count="10"></frame>

    <!-- Skip pages 11-90 -->

    <!-- Last 10 pages (starting at page 91) -->
    <frame src="document.pdf" data-page-start="90"></frame>
</frameset>
```

### 10. Template with Page Limit

Limit generated template pages:

```html
<frameset>
    <frame src="header-template.html"></frame>

    <!-- Only include first 5 generated pages from template -->
    <frame src="data-template.html" data-page-count="5"></frame>

    <frame src="footer-template.html"></frame>
</frameset>
```

### 11. Personalized Document Assembly

Build custom document based on user selection:

```html
<!-- Model: { includeIntro: true, includeTech: true, includeAppendix: false } -->
<frameset>
    <!-- Always include cover -->
    <frame src="library/cover.pdf" data-page-count="1"></frame>

    <!-- Conditional sections -->
    <frame src="library/introduction.pdf"
           hidden="{{model.includeIntro ? '' : 'hidden'}}"></frame>

    <frame src="library/technical.pdf"
           hidden="{{model.includeTech ? '' : 'hidden'}}"></frame>

    <frame src="library/appendix.pdf"
           hidden="{{model.includeAppendix ? '' : 'hidden'}}"></frame>
</frameset>
```

### 12. Extract Odd Pages Only

Use template with bound data to extract odd pages:

```html
<frameset>
    <!-- Generate frames for odd pages 1, 3, 5, 7, 9 -->
    <template data-bind="{{range(0, 10, 2)}}">
        <frame src="document.pdf"
               data-page-start="{{.}}"
               data-page-count="1"></frame>
    </template>
</frameset>
```

### 13. Legal Document Assembly

Assemble contract with variable clauses:

```html
<!-- Model: { includeClause5: true, includeClause7: false } -->
<frameset>
    <!-- Standard opening (pages 1-2) -->
    <frame src="contract-master.pdf" data-page-count="2"></frame>

    <!-- Standard clauses 1-4 (pages 3-6) -->
    <frame src="contract-master.pdf" data-page-start="2" data-page-count="4"></frame>

    <!-- Optional clause 5 (page 7) -->
    <frame src="contract-master.pdf"
           data-page-start="6"
           data-page-count="1"
           hidden="{{model.includeClause5 ? '' : 'hidden'}}"></frame>

    <!-- Standard clause 6 (page 8) -->
    <frame src="contract-master.pdf" data-page-start="7" data-page-count="1"></frame>

    <!-- Optional clause 7 (page 9) -->
    <frame src="contract-master.pdf"
           data-page-start="8"
           data-page-count="1"
           hidden="{{model.includeClause7 ? '' : 'hidden'}}"></frame>

    <!-- Standard closing (pages 10-12) -->
    <frame src="contract-master.pdf" data-page-start="9"></frame>
</frameset>
```

### 14. Report with Dynamic Sections

Include sections based on data availability:

```html
<!-- Model: { hasSalesData: true, hasMarketData: false, pageRanges: {...} } -->
<frameset>
    <!-- Executive summary -->
    <frame src="reports/executive-summary.pdf"></frame>

    <!-- Sales analysis (if data available) -->
    <frame src="reports/full-analysis.pdf"
           data-page-start="{{model.pageRanges.salesStart}}"
           data-page-count="{{model.pageRanges.salesCount}}"
           hidden="{{model.hasSalesData ? '' : 'hidden'}}"></frame>

    <!-- Market analysis (if data available) -->
    <frame src="reports/full-analysis.pdf"
           data-page-start="{{model.pageRanges.marketStart}}"
           data-page-count="{{model.pageRanges.marketCount}}"
           hidden="{{model.hasMarketData ? '' : 'hidden'}}"></frame>

    <!-- Always include conclusions -->
    <frame src="reports/conclusions.pdf"></frame>
</frameset>
```

### 15. Multi-Language Document

Assemble document with language-specific pages:

```html
<!-- Model: { language: "es" } -->
<frameset>
    <!-- Universal cover -->
    <frame src="cover-universal.pdf" data-page-count="1"></frame>

    <!-- Language-specific content -->
    <frame src="{{concat('content-', model.language, '.pdf')}}"></frame>

    <!-- Language-specific legal (pages 50-52) -->
    <frame src="{{concat('legal-', model.language, '.pdf')}}"
           data-page-start="49"
           data-page-count="3"></frame>
</frameset>
```

### 16. Page Range Validation Example

Safe page extraction with bounds:

```html
<!-- Model: { startIdx: 5, count: 10, maxPages: 100 } -->
<frameset>
    <frame src="large-document.pdf"
           data-page-start="{{model.startIdx < model.maxPages ? model.startIdx : 0}}"
           data-page-count="{{model.count}}"></frame>
</frameset>
```

### 17. Certification Document

Assemble certification with selected modules:

```html
<!-- Model: { completedModules: [1, 3, 7] } -->
<frameset>
    <!-- Certificate cover -->
    <frame src="cert-cover.pdf" data-page-count="1"></frame>

    <!-- Include pages for completed modules -->
    <template data-bind="{{model.completedModules}}">
        <frame src="cert-modules.pdf"
               data-page-start="{{(. - 1) * 2}}"
               data-page-count="2"></frame>
    </template>

    <!-- Certificate back -->
    <frame src="cert-cover.pdf" data-page-start="1" data-page-count="1"></frame>
</frameset>
```

### 18. Datasheet Generator

Create custom product datasheet:

```html
<!-- Model: { productId: 42, includeSpecs: true, includeWarranty: true } -->
<frameset>
    <!-- Product page from catalog (2 pages per product) -->
    <frame src="catalog.pdf"
           data-page-start="{{(model.productId - 1) * 2}}"
           data-page-count="2"></frame>

    <!-- Technical specifications (pages 200-205) -->
    <frame src="technical-docs.pdf"
           data-page-start="199"
           data-page-count="6"
           hidden="{{model.includeSpecs ? '' : 'hidden'}}"></frame>

    <!-- Warranty information (page 250) -->
    <frame src="legal-docs.pdf"
           data-page-start="249"
           data-page-count="1"
           hidden="{{model.includeWarranty ? '' : 'hidden'}}"></frame>
</frameset>
```

### 19. Newsletter Assembly

Combine articles into newsletter:

```html
<!-- Model: { articles: [{start: 5, count: 3}, {start: 12, count: 2}, ...] } -->
<frameset>
    <!-- Newsletter header -->
    <frame src="newsletter-header.pdf" data-page-count="1"></frame>

    <!-- Dynamic articles -->
    <template data-bind="{{model.articles}}">
        <frame src="article-archive.pdf"
               data-page-start="{{.start}}"
               data-page-count="{{.count}}"></frame>
    </template>

    <!-- Newsletter footer -->
    <frame src="newsletter-footer.pdf" data-page-count="1"></frame>
</frameset>
```

### 20. Exam with Question Bank

Generate exam from question database:

```html
<!-- Model: { questionPages: [2, 5, 8, 12, 15, 20, 23, 28, 30, 35] } -->
<frameset>
    <!-- Exam instructions -->
    <frame src="exam-instructions.pdf"></frame>

    <!-- Selected questions (each question is 1 page) -->
    <template data-bind="{{model.questionPages}}">
        <frame src="question-bank.pdf"
               data-page-start="{{. - 1}}"
               data-page-count="1"></frame>
    </template>

    <!-- Answer sheet -->
    <frame src="answer-sheet.pdf"></frame>
</frameset>
```

---

## See Also

- [frame element](/reference/htmltags/frame.html) - Frame element reference
- [frameset element](/reference/htmltags/frameset.html) - Frameset container element
- [Document Assembly](/reference/assembly/) - Document assembly guide
- [Modifications](/reference/modifications/) - Document modification framework
- [Hidden Attribute](/reference/htmlattributes/hidden.html) - Controlling visibility
- [Data Binding](/reference/binding/) - Data binding expressions

---
