---
layout: default
title: title
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @title : The Title Attribute

The `title` attribute provides additional information about an element. In Scryber PDF documents, it primarily serves to create navigable bookmarks and outline entries, enabling users to quickly jump to different sections of the document through the PDF viewer's navigation panel.

## Usage

The `title` attribute provides element metadata that:
- Creates entries in the PDF document outline/bookmarks panel
- Establishes hierarchical document navigation structure
- Provides descriptive text for PDF bookmarks
- Works in conjunction with element `id` attributes for internal linking
- Supports data binding for dynamic bookmark text
- Enhances document accessibility and navigation

```html
<!-- Create a bookmark entry -->
<h1 id="chapter1" title="Chapter 1: Introduction">
    Chapter 1: Introduction
</h1>

<!-- Create nested bookmark hierarchy -->
<h2 id="section1-1" title="1.1 Overview">
    1.1 Overview
</h2>

<!-- Dynamic bookmark text -->
<h1 id="{{model.chapterId}}" title="{{model.chapterTitle}}">
    {{model.chapterTitle}}
</h1>
```

---

## Supported Elements

The `title` attribute is technically supported on **all HTML elements**, but its bookmark functionality is most commonly used with:

### Heading Elements (Primary Use)
- `<h1>`, `<h2>`, `<h3>`, `<h4>`, `<h5>`, `<h6>`

These elements automatically create hierarchical bookmark structures based on heading levels.

### Structural Elements
- `<section>`, `<article>`, `<aside>`, `<nav>`
- `<header>`, `<footer>`, `<main>`
- `<div>` (when used as major section containers)

### Other Elements
- `<a>` - Can provide tooltip text for links
- `<img>` - Can provide descriptive information
- Any element where bookmark/outline entry is desired

---

## Binding Values

The `title` attribute supports data binding for dynamic bookmark text:

```html
<!-- Dynamic chapter titles -->
<h1 id="chapter-{{model.chapterNum}}" title="Chapter {{model.chapterNum}}: {{model.chapterName}}">
    Chapter {{model.chapterNum}}: {{model.chapterName}}
</h1>

<!-- Dynamic section titles -->
<section id="{{model.sectionId}}" title="{{model.sectionTitle}}">
    <h2>{{model.sectionTitle}}</h2>
    <p>{{model.content}}</p>
</section>

<!-- Repeated content with bookmarks -->
<template data-bind="{{model.chapters}}">
    <div style="page-break-before: always;">
        <h1 id="chapter-{{.number}}" title="{{.number}}. {{.title}}">
            {{.title}}
        </h1>
        <p>{{.summary}}</p>
    </div>
</template>

<!-- Conditional title text -->
<h2 id="summary" title="{{model.isExecutive ? 'Executive Summary' : 'Summary'}}">
    Summary
</h2>
```

**Data Model Example:**
```json
{
  "chapterNum": 3,
  "chapterName": "Advanced Features",
  "sectionId": "sec-3-1",
  "sectionTitle": "Configuration Options",
  "chapters": [
    {
      "number": 1,
      "title": "Getting Started",
      "summary": "Introduction to the basics"
    },
    {
      "number": 2,
      "title": "Core Concepts",
      "summary": "Understanding the fundamentals"
    }
  ],
  "isExecutive": true
}
```

---

## Notes

### PDF Bookmarks and Outlines

The primary purpose of the `title` attribute in Scryber is to create PDF bookmarks (also called outlines). These appear in the navigation panel of PDF viewers and allow users to:

- Quickly jump to different sections of the document
- See the document structure at a glance
- Navigate long documents efficiently
- Understand document organization

```html
<h1 id="intro" title="Introduction">Introduction</h1>
<!-- Creates a top-level bookmark "Introduction" -->

<h2 id="overview" title="Overview">Overview</h2>
<!-- Creates a second-level bookmark "Overview" under the parent section -->
```

### Bookmark Hierarchy

Bookmarks are automatically organized hierarchically based on heading levels:

- `<h1>` creates top-level bookmarks
- `<h2>` creates second-level bookmarks (nested under nearest `<h1>`)
- `<h3>` creates third-level bookmarks (nested under nearest `<h2>`)
- And so on through `<h6>`

```html
<h1 id="ch1" title="Chapter 1">Chapter 1</h1>          <!-- Level 1 -->
    <h2 id="s1-1" title="1.1 First Section">Section 1.1</h2>     <!-- Level 2 -->
        <h3 id="s1-1-1" title="1.1.1 Subsection">Subsection</h3>  <!-- Level 3 -->
    <h2 id="s1-2" title="1.2 Second Section">Section 1.2</h2>    <!-- Level 2 -->
<h1 id="ch2" title="Chapter 2">Chapter 2</h1>          <!-- Level 1 -->
```

This creates a bookmark tree structure:
```
Chapter 1
  ├─ 1.1 First Section
  │   └─ 1.1.1 Subsection
  └─ 1.2 Second Section
Chapter 2
```

### Title vs Element Content

While the `title` attribute often matches the visible element content, they can differ:

```html
<!-- Title matches content -->
<h1 id="intro" title="Introduction">Introduction</h1>

<!-- Title provides shortened bookmark text -->
<h1 id="long" title="Summary">
    A Very Long Heading That Would Be Too Long For A Bookmark Entry
</h1>

<!-- Title includes chapter numbering for bookmarks -->
<h1 id="methods" title="3. Research Methods">
    Research Methods
</h1>
```

### Combining with ID Attribute

For bookmarks to work as navigation links, elements should have both `id` and `title` attributes:

```html
<!-- Both id and title for functional bookmarks -->
<h1 id="chapter1" title="Chapter 1: Getting Started">
    Chapter 1: Getting Started
</h1>

<!-- Without id, bookmark won't link properly -->
<h1 title="Chapter 2">Chapter 2</h1>  <!-- Less useful -->
```

### Bookmark Best Practices

**Do:**
- Provide clear, descriptive titles
- Use consistent title formatting
- Include chapter/section numbers when appropriate
- Keep titles concise but meaningful
- Maintain logical hierarchy with heading levels

**Don't:**
- Use extremely long titles (they'll be truncated in bookmark panels)
- Skip heading levels (h1 → h3) as it breaks hierarchy
- Use generic titles like "Section" or "Chapter" without identification
- Forget to include titles on major document sections

```html
<!-- Good bookmark titles -->
<h1 id="ch1" title="1. Introduction">Chapter 1: Introduction</h1>
<h2 id="s1-1" title="1.1 Background">1.1 Background</h2>
<h2 id="s1-2" title="1.2 Objectives">1.2 Objectives</h2>

<!-- Poor bookmark titles -->
<h1 id="ch1" title="Chapter">Chapter</h1>  <!-- Too generic -->
<h2 id="s1" title="This is a very long title that describes everything in great detail">
    Section
</h2>  <!-- Too long -->
```

### Non-Heading Elements

While headings are most common, other elements can also use `title` for bookmarks:

```html
<section id="overview" title="Product Overview">
    <div>
        <p>Content without a heading...</p>
    </div>
</section>

<div id="appendix" title="Appendix A: Resources">
    <p>Additional resources and references...</p>
</div>
```

However, using proper heading elements (`<h1>` through `<h6>`) is recommended for semantic structure and consistent hierarchy.

### Empty or Missing Titles

If a heading element has an `id` but no `title`, the element's text content may be used as the bookmark text:

```html
<!-- Title explicitly set -->
<h1 id="ch1" title="Chapter One">Chapter 1</h1>
<!-- Bookmark: "Chapter One" -->

<!-- No title - may use content -->
<h1 id="ch2">Chapter 2</h1>
<!-- Bookmark behavior: implementation-dependent -->
```

For consistent results, always provide explicit `title` attributes on elements you want to appear in bookmarks.

---

## Examples

### Basic Chapter Bookmarks

```html
<h1 id="chapter1" title="Chapter 1: Introduction">
    Chapter 1: Introduction
</h1>

<h1 id="chapter2" title="Chapter 2: Getting Started">
    Chapter 2: Getting Started
</h1>

<h1 id="chapter3" title="Chapter 3: Advanced Features">
    Chapter 3: Advanced Features
</h1>
```

### Hierarchical Document Structure

```html
<h1 id="ch1" title="1. Introduction">Chapter 1: Introduction</h1>

<h2 id="s1-1" title="1.1 Background">1.1 Background</h2>
<p>Background information...</p>

<h2 id="s1-2" title="1.2 Purpose">1.2 Purpose</h2>
<p>Purpose of this document...</p>

<h2 id="s1-3" title="1.3 Scope">1.3 Scope</h2>

<h3 id="s1-3-1" title="1.3.1 Inclusions">1.3.1 Inclusions</h3>
<p>What is included...</p>

<h3 id="s1-3-2" title="1.3.2 Exclusions">1.3.2 Exclusions</h3>
<p>What is not included...</p>

<h1 id="ch2" title="2. Methodology">Chapter 2: Methodology</h1>

<h2 id="s2-1" title="2.1 Research Design">2.1 Research Design</h2>
<p>Research design details...</p>
```

### Table of Contents with Bookmarks

```html
<!DOCTYPE html>
<html>
<head>
    <title>Technical Report</title>
</head>
<body>
    <!-- Table of Contents Page -->
    <div id="toc" title="Table of Contents">
        <h1>Table of Contents</h1>
        <ol>
            <li><a href="#executive-summary">Executive Summary</a></li>
            <li><a href="#introduction">Introduction</a></li>
            <li><a href="#methodology">Methodology</a></li>
            <li><a href="#results">Results</a></li>
            <li><a href="#conclusion">Conclusion</a></li>
        </ol>
    </div>

    <div style="page-break-after: always;"></div>

    <!-- Document Sections with Bookmarks -->
    <section id="executive-summary" title="Executive Summary">
        <h1>Executive Summary</h1>
        <p>Summary content...</p>
    </section>

    <div style="page-break-after: always;"></div>

    <section id="introduction" title="1. Introduction">
        <h1>1. Introduction</h1>

        <h2 id="background" title="1.1 Background">1.1 Background</h2>
        <p>Background information...</p>

        <h2 id="objectives" title="1.2 Objectives">1.2 Objectives</h2>
        <p>Project objectives...</p>
    </section>

    <div style="page-break-after: always;"></div>

    <section id="methodology" title="2. Methodology">
        <h1>2. Methodology</h1>
        <p>Methodology details...</p>
    </section>

    <div style="page-break-after: always;"></div>

    <section id="results" title="3. Results">
        <h1>3. Results</h1>
        <p>Results and findings...</p>
    </section>

    <div style="page-break-after: always;"></div>

    <section id="conclusion" title="4. Conclusion">
        <h1>4. Conclusion</h1>
        <p>Concluding remarks...</p>
    </section>
</body>
</html>
```

### Multi-Level Documentation

```html
<h1 id="guide" title="User Guide">Complete User Guide</h1>

<h2 id="getting-started" title="Getting Started">Getting Started</h2>

<h3 id="installation" title="Installation">Installation</h3>
<p>Installation instructions...</p>

<h4 id="windows" title="Windows Installation">Windows</h4>
<p>Windows-specific steps...</p>

<h4 id="macos" title="macOS Installation">macOS</h4>
<p>macOS-specific steps...</p>

<h4 id="linux" title="Linux Installation">Linux</h4>
<p>Linux-specific steps...</p>

<h3 id="configuration" title="Configuration">Configuration</h3>
<p>Configuration options...</p>

<h2 id="features" title="Features">Features</h2>

<h3 id="basic-features" title="Basic Features">Basic Features</h3>
<p>Basic feature descriptions...</p>

<h3 id="advanced-features" title="Advanced Features">Advanced Features</h3>
<p>Advanced feature descriptions...</p>
```

### Data-Bound Bookmarks

```html
<!-- Model: { report: { title: "Q4 2025 Report", sections: [...] } } -->

<h1 id="report-title" title="{{model.report.title}}">
    {{model.report.title}}
</h1>

<template data-bind="{{model.report.sections}}">
    <div style="page-break-before: always;">
        <h2 id="section-{{.id}}" title="{{.number}}. {{.title}}">
            {{.number}}. {{.title}}
        </h2>
        <p>{{.content}}</p>

        <!-- Nested subsections -->
        <template data-bind="{{.subsections}}">
            <h3 id="subsection-{{.id}}" title="{{.number}} {{.title}}">
                {{.number}} {{.title}}
            </h3>
            <p>{{.content}}</p>
        </template>
    </div>
</template>
```

### Report with Abbreviated Bookmarks

```html
<!-- Long content headings with short bookmark titles -->

<h1 id="exec" title="Executive Summary">
    Executive Summary: Key Findings and Recommendations for Fiscal Year 2025
</h1>

<h2 id="findings" title="Key Findings">
    Key Findings from the Annual Performance Review and Market Analysis
</h2>

<h2 id="recommendations" title="Recommendations">
    Strategic Recommendations for the Upcoming Fiscal Year
</h2>

<h1 id="detailed" title="Detailed Analysis">
    Detailed Analysis of Market Conditions, Competitive Landscape, and Growth Opportunities
</h1>
```

### Book with Chapters and Sections

```html
<!-- Front Matter -->
<div id="preface" title="Preface">
    <h1>Preface</h1>
    <p>Preface content...</p>
</div>

<div style="page-break-after: always;"></div>

<!-- Part 1 -->
<div id="part1" title="Part I: Foundations">
    <h1 style="text-align: center; font-size: 24pt;">PART I: FOUNDATIONS</h1>
</div>

<div style="page-break-after: always;"></div>

<h1 id="ch1" title="Chapter 1: Introduction">Chapter 1: Introduction</h1>
<h2 id="ch1-s1" title="What is PDF Generation?">What is PDF Generation?</h2>
<h2 id="ch1-s2" title="Why Use Scryber?">Why Use Scryber?</h2>

<div style="page-break-after: always;"></div>

<h1 id="ch2" title="Chapter 2: Core Concepts">Chapter 2: Core Concepts</h1>
<h2 id="ch2-s1" title="Document Structure">Document Structure</h2>
<h2 id="ch2-s2" title="Layout Engine">Layout Engine</h2>

<div style="page-break-after: always;"></div>

<!-- Part 2 -->
<div id="part2" title="Part II: Advanced Topics">
    <h1 style="text-align: center; font-size: 24pt;">PART II: ADVANCED TOPICS</h1>
</div>

<div style="page-break-after: always;"></div>

<h1 id="ch3" title="Chapter 3: Data Binding">Chapter 3: Data Binding</h1>
<h2 id="ch3-s1" title="Binding Syntax">Binding Syntax</h2>
<h2 id="ch3-s2" title="Complex Models">Complex Models</h2>
```

### Academic Paper Structure

```html
<article>
    <header id="header" title="Title Page">
        <h1>Research Paper Title</h1>
        <p>Author names and affiliations</p>
    </header>

    <div style="page-break-after: always;"></div>

    <section id="abstract" title="Abstract">
        <h1>Abstract</h1>
        <p>Paper abstract...</p>
    </section>

    <div style="page-break-after: always;"></div>

    <section id="intro" title="1. Introduction">
        <h1>1. Introduction</h1>
        <h2 id="intro-context" title="1.1 Research Context">1.1 Research Context</h2>
        <h2 id="intro-questions" title="1.2 Research Questions">1.2 Research Questions</h2>
    </section>

    <section id="literature" title="2. Literature Review">
        <h1>2. Literature Review</h1>
        <h2 id="lit-theory" title="2.1 Theoretical Framework">2.1 Theoretical Framework</h2>
        <h2 id="lit-studies" title="2.2 Previous Studies">2.2 Previous Studies</h2>
    </section>

    <section id="methods" title="3. Methodology">
        <h1>3. Methodology</h1>
        <h2 id="methods-design" title="3.1 Research Design">3.1 Research Design</h2>
        <h2 id="methods-sample" title="3.2 Sample Selection">3.2 Sample Selection</h2>
        <h2 id="methods-analysis" title="3.3 Data Analysis">3.3 Data Analysis</h2>
    </section>

    <section id="results" title="4. Results">
        <h1>4. Results</h1>
        <h2 id="results-descriptive" title="4.1 Descriptive Statistics">
            4.1 Descriptive Statistics
        </h2>
        <h2 id="results-inferential" title="4.2 Inferential Statistics">
            4.2 Inferential Statistics
        </h2>
    </section>

    <section id="discussion" title="5. Discussion">
        <h1>5. Discussion</h1>
        <h2 id="disc-findings" title="5.1 Interpretation of Findings">
            5.1 Interpretation of Findings
        </h2>
        <h2 id="disc-implications" title="5.2 Implications">5.2 Implications</h2>
        <h2 id="disc-limitations" title="5.3 Limitations">5.3 Limitations</h2>
    </section>

    <section id="conclusion" title="6. Conclusion">
        <h1>6. Conclusion</h1>
        <p>Concluding remarks...</p>
    </section>

    <section id="references" title="References">
        <h1>References</h1>
        <p>Citation list...</p>
    </section>
</article>
```

### Technical Manual with Appendices

```html
<h1 id="intro" title="Introduction">Introduction</h1>
<p>Manual introduction...</p>

<h1 id="installation" title="Installation">Installation Guide</h1>
<h2 id="requirements" title="System Requirements">System Requirements</h2>
<h2 id="install-steps" title="Installation Steps">Installation Steps</h2>

<h1 id="usage" title="Usage">Usage Instructions</h1>
<h2 id="basic-usage" title="Basic Usage">Basic Usage</h2>
<h2 id="advanced-usage" title="Advanced Usage">Advanced Usage</h2>

<h1 id="api" title="API Reference">API Reference</h1>
<h2 id="api-core" title="Core API">Core API</h2>
<h2 id="api-extensions" title="Extensions">Extensions</h2>

<h1 id="troubleshooting" title="Troubleshooting">Troubleshooting</h1>
<h2 id="common-issues" title="Common Issues">Common Issues</h2>
<h2 id="error-codes" title="Error Codes">Error Codes</h2>

<!-- Appendices -->
<div style="page-break-before: always;">
    <h1 id="appendix-a" title="Appendix A: Configuration">
        Appendix A: Configuration Reference
    </h1>
</div>

<div style="page-break-before: always;">
    <h1 id="appendix-b" title="Appendix B: Examples">
        Appendix B: Code Examples
    </h1>
</div>

<div style="page-break-before: always;">
    <h1 id="appendix-c" title="Appendix C: Glossary">
        Appendix C: Glossary of Terms
    </h1>
</div>
```

### Newsletter with Sections

```html
<header id="masthead" title="Newsletter Header">
    <h1>Monthly Newsletter - January 2025</h1>
</header>

<section id="news" title="Company News">
    <h2>Company News</h2>
    <article id="news1" title="Product Launch">
        <h3>New Product Launch</h3>
        <p>Details about the launch...</p>
    </article>
    <article id="news2" title="Award Received">
        <h3>Industry Award Received</h3>
        <p>Award details...</p>
    </article>
</section>

<section id="features" title="Featured Articles">
    <h2>Featured Articles</h2>
    <article id="feature1" title="Technology Trends">
        <h3>Top Technology Trends for 2025</h3>
        <p>Article content...</p>
    </article>
</section>

<section id="events" title="Upcoming Events">
    <h2>Upcoming Events</h2>
    <p>Event listings...</p>
</section>
```

### Dynamic Report with Conditional Titles

```html
<!-- Model: { reportType: "quarterly", quarter: "Q4", year: 2025, sections: [...] } -->

<h1 id="report-title"
    title="{{model.reportType == 'quarterly' ? model.quarter + ' ' + model.year : model.year}} Report">
    {{model.reportType == 'quarterly' ? model.quarter : 'Annual'}} {{model.year}} Report
</h1>

<template data-bind="{{model.sections}}">
    <section id="section-{{.id}}" title="{{.displayOrder}}. {{.title}}">
        <h2>{{.displayOrder}}. {{.title}}</h2>
        <p>{{.summary}}</p>

        <template data-bind="{{.items}}">
            <h3 id="item-{{.id}}" title="{{.category}}: {{.name}}">
                {{.name}}
            </h3>
            <p>{{.description}}</p>
        </template>
    </section>
</template>
```

### Instruction Manual

```html
<h1 id="safety" title="Safety Information">Safety Information</h1>
<h2 id="warnings" title="Warnings">Important Warnings</h2>
<h2 id="precautions" title="Precautions">Precautions</h2>

<h1 id="overview" title="Product Overview">Product Overview</h1>
<h2 id="components" title="Components">Components and Parts</h2>
<h2 id="specifications" title="Specifications">Technical Specifications</h2>

<h1 id="assembly" title="Assembly Instructions">Assembly Instructions</h1>
<h2 id="tools" title="Required Tools">Required Tools</h2>
<h2 id="step1" title="Step 1: Base Assembly">Step 1: Base Assembly</h2>
<h2 id="step2" title="Step 2: Main Unit">Step 2: Main Unit Assembly</h2>
<h2 id="step3" title="Step 3: Final Assembly">Step 3: Final Assembly</h2>

<h1 id="operation" title="Operation">Operation Instructions</h1>
<h2 id="startup" title="Starting Up">Starting Up</h2>
<h2 id="normal-use" title="Normal Use">Normal Operation</h2>
<h2 id="shutdown" title="Shutting Down">Shutting Down</h2>

<h1 id="maintenance" title="Maintenance">Maintenance and Care</h1>
<h2 id="cleaning" title="Cleaning">Cleaning Instructions</h2>
<h2 id="storage" title="Storage">Storage Guidelines</h2>

<h1 id="warranty" title="Warranty Information">Warranty Information</h1>
```

---

## See Also

- [id](/reference/htmlattributes/id.html) - Unique identifier attribute (required for bookmark navigation)
- [a](/reference/htmltags/a.html) - Anchor element for linking to bookmarked sections
- [href](/reference/htmlattributes/href.html) - Link reference attribute
- [h1-h6](/reference/htmltags/headings.html) - Heading elements (primary use for bookmarks)
- [PDF Bookmarks](/reference/document/bookmarks.html) - Document outline and navigation
- [Document Structure](/reference/document/structure.html) - Organizing PDF documents
- [Data Binding](/reference/binding/) - Dynamic content and attributes

---
