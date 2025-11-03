---
layout: default
title: data-outline-title
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-outline-title : The Custom Outline Title Attribute

The `data-outline-title` attribute specifies a custom title for PDF bookmarks and outlines. It allows you to provide a different text for the document navigation structure than what appears in the visible content, enabling more descriptive or concise bookmark entries.

## Usage

The `data-outline-title` attribute is used to:
- Create custom bookmark titles in PDF navigation panels
- Provide descriptive outline entries different from visible content
- Simplify long headings for navigation purposes
- Add context to bookmark titles
- Control PDF document navigation structure

```html
<!-- Visible heading -->
<h1 data-outline-title="Executive Summary">
    Annual Financial Report 2024 - Executive Summary and Key Findings
</h1>

<!-- PDF bookmark will show "Executive Summary" -->
```

---

## Supported Elements

The `data-outline-title` attribute is supported by various elements that can generate PDF bookmarks:

| Element | Description |
|---------|-------------|
| Heading elements (`<h1>`-`<h6>`) | Section headings |
| `<section>` | Document sections |
| `<div>` | Generic containers with explicit titles |
| `<abbr>` | Abbreviations |
| `<cite>` | Citations |
| `<defn>` | Definitions |
| Other semantic elements | Various inline and block elements |

---

## Attribute Values

### Syntax

```html
<h1 data-outline-title="Bookmark Title">Actual Heading Text</h1>
```

### Value Type

| Type | Description | Example |
|------|-------------|---------|
| String | Any text string for the bookmark | `data-outline-title="Chapter 1"` |
| Short text | Concise bookmark label | `data-outline-title="Summary"` |
| Descriptive | More context than visible text | `data-outline-title="Q4 Financial Results"` |

### Best Practices

**Good outline titles:**
- Concise: `"Introduction"` instead of `"Introduction to the Annual Financial Report for Fiscal Year 2024"`
- Descriptive: `"Q4 Revenue Analysis"` instead of just `"Revenue"`
- Hierarchical: `"3.2 Customer Segmentation"` for numbered sections
- Meaningful: `"Executive Summary"` instead of `"Summary"`

**Avoid:**
- Very long titles that clutter navigation
- Special characters that may not render well
- Duplicate titles (makes navigation confusing)
- Empty or whitespace-only values

---

## Binding Values

The `data-outline-title` attribute supports data binding:

### Static Outline Title

```html
<h1 data-outline-title="Chapter 1: Introduction">
    Chapter 1: Introduction to PDF Generation with Scryber
</h1>
```

### Dynamic Outline Title with Binding

```html
<!-- Model: { chapterNum: 3, chapterTitle: "Advanced Features" } -->
<h1 data-outline-title="Chapter {{model.chapterNum}}: {{model.chapterTitle}}">
    Chapter {{model.chapterNum}}: {{model.chapterTitle}} - Comprehensive Guide
</h1>
```

### Computed Outline Title

```html
<!-- Model: { quarter: "Q4", year: 2024 } -->
<h2 data-outline-title="{{model.quarter}} {{model.year}} Report">
    Quarterly Financial Report for {{model.quarter}} {{model.year}} - Detailed Analysis
</h2>
```

---

## Notes

### PDF Bookmarks/Outlines

In PDF documents:
- **Bookmarks** (also called **Outlines**) provide hierarchical navigation
- They appear in the navigation panel of PDF readers
- Clicking a bookmark navigates to that section
- The bookmark hierarchy follows the document structure (h1 > h2 > h3, etc.)

### Fallback Behavior

If `data-outline-title` is not specified:
- The element's visible text content becomes the bookmark title
- For headings, the full heading text is used
- For elements with complex content, text is extracted

### Title vs Outline Title

| Attribute | Purpose | Visibility |
|-----------|---------|------------|
| `title` | Tooltip/hover text | Visible on hover (web), metadata (PDF) |
| `data-outline-title` | PDF bookmark title | PDF navigation panel only |

You can use both:
```html
<h1 title="Tooltip text" data-outline-title="Bookmark Title">
    Visible Heading Text
</h1>
```

### Hierarchical Structure

PDF bookmarks automatically create a hierarchy based on heading levels:

```html
<h1 data-outline-title="Part 1">Part 1: Foundations</h1>
    <h2 data-outline-title="Chapter 1">Chapter 1: Getting Started</h2>
        <h3 data-outline-title="1.1 Installation">1.1: Installation Guide</h3>
    <h2 data-outline-title="Chapter 2">Chapter 2: Basic Concepts</h2>
```

Creates this bookmark structure:
```
📖 Part 1
   📄 Chapter 1
      📄 1.1 Installation
   📄 Chapter 2
```

### Use Cases

1. **Simplify Long Headings**: Shorten verbose headings for navigation
2. **Add Context**: Provide more context than the visible heading
3. **Numbered Sections**: Include section numbers in bookmarks
4. **Multi-Language**: Provide language-specific bookmark titles
5. **Technical Documents**: Use descriptive bookmarks for technical sections

---

## Examples

### Example 1: Simplifying Long Headings

```html
<h1 data-outline-title="Executive Summary">
    Annual Financial Report for Fiscal Year 2024 - Executive Summary and Key Performance Indicators
</h1>

<h2 data-outline-title="Revenue Analysis">
    Comprehensive Analysis of Revenue Streams, Growth Patterns, and Market Performance
</h2>
```

### Example 2: Report with Numbered Sections

```html
<h1 data-outline-title="1. Introduction">
    1. Introduction to the Report
</h1>

<h2 data-outline-title="1.1 Purpose">
    1.1 Purpose and Scope of This Document
</h2>

<h2 data-outline-title="1.2 Methodology">
    1.2 Research Methodology and Data Collection Procedures
</h2>

<h1 data-outline-title="2. Findings">
    2. Key Findings and Analysis
</h1>

<h2 data-outline-title="2.1 Market Trends">
    2.1 Current Market Trends and Industry Dynamics
</h2>
```

### Example 3: Book Chapters

```html
<h1 data-outline-title="Chapter 1: The Beginning">
    Chapter 1
</h1>
<p class="chapter-subtitle">The Beginning of an Adventure</p>

<h1 data-outline-title="Chapter 2: The Journey">
    Chapter 2
</h1>
<p class="chapter-subtitle">The Long Journey Through Unknown Lands</p>

<h1 data-outline-title="Chapter 3: The Revelation">
    Chapter 3
</h1>
<p class="chapter-subtitle">The Revelation That Changed Everything</p>
```

### Example 4: Technical Documentation

```html
<h1 data-outline-title="API Reference">API Reference Documentation</h1>

<h2 data-outline-title="Authentication">User Authentication and Authorization</h2>

<h3 data-outline-title="OAuth 2.0 Flow">
    OAuth 2.0 Authentication Flow - Implementation Guide and Best Practices
</h3>

<h3 data-outline-title="API Keys">
    API Key Management - Generation, Storage, and Security Considerations
</h3>

<h2 data-outline-title="Endpoints">API Endpoints and Methods</h2>

<h3 data-outline-title="GET /users">
    GET /users - Retrieve User List with Filtering and Pagination
</h3>
```

### Example 5: Academic Paper

```html
<h1 data-outline-title="Abstract">Abstract</h1>
<p>Research abstract content...</p>

<h1 data-outline-title="1. Introduction">
    1. Introduction to the Research Problem
</h1>

<h2 data-outline-title="1.1 Background">
    1.1 Background and Context of the Study
</h2>

<h2 data-outline-title="1.2 Research Questions">
    1.2 Primary and Secondary Research Questions
</h2>

<h1 data-outline-title="2. Literature Review">
    2. Review of Existing Literature and Related Work
</h1>

<h1 data-outline-title="3. Methodology">
    3. Research Methodology and Experimental Design
</h1>

<h1 data-outline-title="4. Results">
    4. Results and Statistical Analysis
</h1>

<h1 data-outline-title="5. Discussion">
    5. Discussion of Findings and Implications
</h1>

<h1 data-outline-title="6. Conclusions">
    6. Conclusions and Future Research Directions
</h1>
```

### Example 6: Data-Bound Outline Titles

```html
<!-- Model: {
    chapters: [
        {num: 1, title: "Introduction", subtitle: "Getting Started"},
        {num: 2, title: "Basics", subtitle: "Core Concepts"},
        {num: 3, title: "Advanced", subtitle: "Expert Techniques"}
    ]
} -->

<template data-bind="{{model.chapters}}">
    <h1 data-outline-title="Chapter {{.num}}: {{.title}}">
        Chapter {{.num}}: {{.title}} - {{.subtitle}}
    </h1>
</template>
```

### Example 7: Business Report

```html
<h1 data-outline-title="Title Page">
    Quarterly Business Review - Q4 2024
</h1>

<h1 data-outline-title="Executive Summary">
    Executive Summary: Key Highlights and Strategic Initiatives
</h1>

<h1 data-outline-title="Financial Performance">
    Financial Performance and Revenue Analysis
</h1>

<h2 data-outline-title="Revenue Breakdown">
    Revenue Breakdown by Product Line and Geographic Region
</h2>

<h2 data-outline-title="Expense Analysis">
    Comprehensive Expense Analysis and Cost Management Strategies
</h2>

<h1 data-outline-title="Market Analysis">
    Market Analysis and Competitive Landscape Assessment
</h1>

<h1 data-outline-title="Future Outlook">
    Future Outlook and Strategic Recommendations for 2025
</h1>
```

### Example 8: User Manual

```html
<h1 data-outline-title="Getting Started">Getting Started with the Product</h1>

<h2 data-outline-title="Unboxing">
    Unboxing Your New Device - What's In The Box
</h2>

<h2 data-outline-title="Setup">
    Initial Setup and Configuration Process
</h2>

<h2 data-outline-title="First Use">
    Your First Time Using the Device - A Step-by-Step Guide
</h2>

<h1 data-outline-title="Features">Product Features and Capabilities</h1>

<h2 data-outline-title="Basic Features">
    Basic Features for Everyday Use
</h2>

<h2 data-outline-title="Advanced Features">
    Advanced Features for Power Users
</h2>

<h1 data-outline-title="Troubleshooting">Troubleshooting Guide and FAQ</h1>
```

### Example 9: Legal Document

```html
<h1 data-outline-title="Agreement">
    SERVICE LEVEL AGREEMENT
</h1>

<h2 data-outline-title="1. Definitions">
    1. DEFINITIONS AND INTERPRETATION
</h2>

<h2 data-outline-title="2. Service Description">
    2. DESCRIPTION OF SERVICES PROVIDED
</h2>

<h2 data-outline-title="3. Performance Standards">
    3. SERVICE PERFORMANCE STANDARDS AND METRICS
</h2>

<h3 data-outline-title="3.1 Availability">
    3.1 Service Availability and Uptime Requirements
</h3>

<h3 data-outline-title="3.2 Response Times">
    3.2 Response Time Requirements and Measurement Methods
</h3>

<h2 data-outline-title="4. Responsibilities">
    4. RESPONSIBILITIES OF THE PARTIES
</h2>

<h2 data-outline-title="5. Terms">
    5. TERMS AND TERMINATION
</h2>
```

### Example 10: Product Catalog

```html
<h1 data-outline-title="Catalog 2024">Product Catalog - Spring/Summer 2024 Edition</h1>

<h2 data-outline-title="Electronics">Electronics and Technology Products</h2>

<h3 data-outline-title="Laptops">
    Laptop Computers - Professional and Home Use
</h3>

<h3 data-outline-title="Tablets">
    Tablets and Mobile Devices
</h3>

<h2 data-outline-title="Home & Garden">Home and Garden Products</h2>

<h3 data-outline-title="Furniture">
    Furniture Collections for Every Room
</h3>

<h3 data-outline-title="Outdoor">
    Outdoor and Patio Furniture and Accessories
</h3>
```

### Example 11: Training Manual

```html
<h1 data-outline-title="Training Overview">
    Employee Training Program Overview and Objectives
</h1>

<h2 data-outline-title="Module 1: Orientation">
    Training Module 1: Company Orientation and Culture
</h2>

<h3 data-outline-title="1.1 Company History">
    1.1: Company History, Mission, and Values
</h3>

<h3 data-outline-title="1.2 Organizational Structure">
    1.2: Organizational Structure and Key Personnel
</h3>

<h2 data-outline-title="Module 2: Policies">
    Training Module 2: Company Policies and Procedures
</h2>

<h3 data-outline-title="2.1 Code of Conduct">
    2.1: Code of Conduct and Professional Ethics
</h3>

<h3 data-outline-title="2.2 Safety Protocols">
    2.2: Workplace Safety Protocols and Emergency Procedures
</h3>
```

### Example 12: Financial Statement

```html
<h1 data-outline-title="Financial Statements">
    Consolidated Financial Statements for the Year Ended December 31, 2024
</h1>

<h2 data-outline-title="Balance Sheet">
    Consolidated Balance Sheet as of December 31, 2024
</h2>

<h2 data-outline-title="Income Statement">
    Consolidated Statement of Income for the Year Ended December 31, 2024
</h2>

<h2 data-outline-title="Cash Flows">
    Consolidated Statement of Cash Flows for the Year Ended December 31, 2024
</h2>

<h2 data-outline-title="Notes">
    Notes to the Consolidated Financial Statements
</h2>

<h3 data-outline-title="Note 1: Accounting Policies">
    Note 1: Summary of Significant Accounting Policies and Methods
</h3>
```

### Example 13: Research Report

```html
<h1 data-outline-title="Title Page">
    Climate Change Impact Assessment: Arctic Ecosystems Study
</h1>

<h1 data-outline-title="Abstract">
    Abstract and Research Summary
</h1>

<h1 data-outline-title="Introduction">
    Introduction: Background and Research Objectives
</h1>

<h1 data-outline-title="Methods">
    Research Methodology and Data Collection Techniques
</h1>

<h2 data-outline-title="Study Area">
    Geographic Study Area and Site Selection Criteria
</h2>

<h2 data-outline-title="Data Collection">
    Field Data Collection Methods and Sampling Procedures
</h2>

<h1 data-outline-title="Results">
    Results: Findings and Observations
</h1>

<h2 data-outline-title="Temperature Trends">
    Temperature Trends and Seasonal Variations
</h2>

<h2 data-outline-title="Species Impact">
    Impact on Arctic Species and Ecosystem Dynamics
</h2>
```

### Example 14: Newsletter

```html
<h1 data-outline-title="Newsletter - January 2024">
    Company Newsletter - January 2024 Edition
</h1>

<h2 data-outline-title="Message from CEO">
    A Message from Our CEO - Looking Ahead to 2024
</h2>

<h2 data-outline-title="Company News">
    Company News and Announcements
</h2>

<h3 data-outline-title="New Product Launch">
    Exciting New Product Launch - Innovation Meets Design
</h3>

<h3 data-outline-title="Awards">
    Industry Awards and Recognition Achievements
</h3>

<h2 data-outline-title="Employee Spotlight">
    Employee Spotlight: Meet Our Team Members
</h2>

<h2 data-outline-title="Upcoming Events">
    Upcoming Events and Important Dates
</h2>
```

### Example 15: Complex Multi-Level Document

```html
<!-- Model: { year: 2024, sections: [...] } -->

<h1 data-outline-title="Annual Report {{model.year}}">
    Comprehensive Annual Report for Fiscal Year {{model.year}}
</h1>

<h1 data-outline-title="Part I: Financial Review">
    Part I: Financial Review and Performance Analysis
</h1>

<h2 data-outline-title="Chapter 1: Overview">
    Chapter 1: Financial Overview and Key Metrics
</h2>

<h3 data-outline-title="1.1 Revenue">
    Section 1.1: Revenue Analysis by Quarter and Product Line
</h3>

<h4 data-outline-title="1.1.1 Q1 Revenue">
    Subsection 1.1.1: First Quarter Revenue Performance
</h4>

<h4 data-outline-title="1.1.2 Q2 Revenue">
    Subsection 1.1.2: Second Quarter Revenue Performance
</h4>

<h3 data-outline-title="1.2 Expenses">
    Section 1.2: Expense Management and Cost Analysis
</h3>

<h2 data-outline-title="Chapter 2: Profitability">
    Chapter 2: Profitability Metrics and Margin Analysis
</h2>

<h1 data-outline-title="Part II: Strategic Initiatives">
    Part II: Strategic Initiatives and Future Direction
</h1>
```

---

## See Also

- [Heading elements](/reference/htmltags/headings.html) - h1-h6 heading elements
- [title attribute](/reference/htmlattributes/title.html) - Title tooltip attribute
- [id attribute](/reference/htmlattributes/id.html) - Element identifier
- [PDF Bookmarks](/reference/pdf/bookmarks.html) - PDF bookmark creation
- [PDF Outlines](/reference/pdf/outlines.html) - PDF outline structure
- [Document Navigation](/reference/pdf/navigation.html) - PDF navigation features
- [Data Binding](/reference/binding/) - Dynamic data binding

---
