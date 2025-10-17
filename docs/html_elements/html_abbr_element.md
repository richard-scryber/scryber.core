---
layout: default
title: abbr Element
parent: HTML Elements
parent_url: /reference/htmlelements/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;abbr&gt; : The Abbreviation Element

The `<abbr>` element represents an abbreviation or acronym. The `title` attribute provides the full expansion or description of the abbreviation, which can be useful for accessibility and documentation purposes in PDF generation.

## Summary

The `<abbr>` element is an inline semantic HTML element that marks up abbreviations and acronyms. In Scryber, it renders text in italic by default and supports the `title` attribute to provide the full form of the abbreviation. Unlike standard HTML, where `title` creates tooltips, in PDF documents the title can be used for accessibility, annotations, or displayed contextually.

---

## Usage

```html
<!-- Basic abbreviation -->
<abbr title="HyperText Markup Language">HTML</abbr>

<!-- With styling -->
<abbr title="Cascading Style Sheets" class="technical-term">CSS</abbr>

<!-- With data binding -->
<abbr title="{{model.abbreviationExpansion}}">{{model.abbreviationText}}</abbr>
```

---

## Supported Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `title` | string | The full expansion or description of the abbreviation |
| `data-outline-title` | string | Alternative title for PDF outline/bookmarks |
| `id` | string | Unique identifier for the element |
| `class` | string | CSS class name(s) for styling |
| `style` | string | Inline CSS styles |
| `hidden` | string | If set to "hidden", the element is not visible |
| `data-bind` | expression | Data binding expression for dynamic content |

---

## Default Styling

By default, the `<abbr>` element renders text in **italic** style to visually distinguish abbreviations from regular text. This can be overridden using CSS:

```css
abbr {
    font-style: normal; /* Remove italic */
    text-decoration: underline; /* Add underline */
    text-decoration-style: dotted; /* Dotted underline */
}
```

---

## Notes

- The `title` attribute in `<abbr>` does NOT create a PDF outline entry by default
- To create an outline entry, use the `data-outline-title` attribute instead
- By default renders in italic to match semantic HTML conventions
- The element is inline and flows with surrounding text
- Supports full data binding for both content and title
- Can be nested within other inline elements
- Useful for technical documents, medical reports, and any content with specialized terminology

---

## Examples

### Example 1: Basic Abbreviation

```html
<p>
    The <abbr title="World Wide Web Consortium">W3C</abbr> defines web standards.
</p>
```

### Example 2: Technical Documentation

```html
<p>
    <abbr title="Application Programming Interface">API</abbr> endpoints use
    <abbr title="Representational State Transfer">REST</abbr> architecture
    with <abbr title="JavaScript Object Notation">JSON</abbr> payloads.
</p>
```

### Example 3: Medical Terms

```html
<p>
    Patient shows elevated <abbr title="Blood Pressure">BP</abbr>
    (140/90 <abbr title="millimeters of mercury">mmHg</abbr>) and
    increased <abbr title="Heart Rate">HR</abbr>.
</p>
```

### Example 4: Business Abbreviations

```html
<p>
    The <abbr title="Chief Executive Officer">CEO</abbr> and
    <abbr title="Chief Financial Officer">CFO</abbr> will present
    the <abbr title="Quarter 4">Q4</abbr> results.
</p>
```

### Example 5: With Custom Styling

```html
<style>
    abbr.technical {
        font-style: normal;
        font-weight: bold;
        color: #0066cc;
        border-bottom: 1pt dotted #0066cc;
    }
</style>
<p>
    Using <abbr class="technical" title="HyperText Markup Language">HTML</abbr>
    and <abbr class="technical" title="Cascading Style Sheets">CSS</abbr>.
</p>
```

### Example 6: Data Binding with Abbreviations

```html
<p>
    The system uses
    <abbr title="{{model.protocolFullName}}">{{model.protocolAbbr}}</abbr>
    for communication.
</p>

<!-- With model data:
{
    "protocolAbbr": "HTTPS",
    "protocolFullName": "HyperText Transfer Protocol Secure"
}
-->
```

### Example 7: Multiple Abbreviations in Technical Report

```html
<h2>System Architecture</h2>
<p>
    The application uses a <abbr title="Model-View-Controller">MVC</abbr>
    architecture with <abbr title="Object-Relational Mapping">ORM</abbr>
    for database access. Authentication is handled via
    <abbr title="JSON Web Tokens">JWT</abbr> and
    <abbr title="OAuth 2.0">OAuth</abbr>.
</p>
```

### Example 8: Units of Measurement

```html
<table>
    <tr>
        <td>Memory:</td>
        <td>16 <abbr title="Gigabytes">GB</abbr></td>
    </tr>
    <tr>
        <td>Storage:</td>
        <td>512 <abbr title="Gigabytes">GB</abbr>
            <abbr title="Solid State Drive">SSD</abbr></td>
    </tr>
    <tr>
        <td>Speed:</td>
        <td>100 <abbr title="Megabits per second">Mbps</abbr></td>
    </tr>
</table>
```

### Example 9: Academic Degrees

```html
<ul>
    <li>John Smith, <abbr title="Doctor of Philosophy">PhD</abbr></li>
    <li>Jane Doe, <abbr title="Master of Business Administration">MBA</abbr></li>
    <li>Bob Johnson, <abbr title="Bachelor of Science">BSc</abbr></li>
</ul>
```

### Example 10: Time and Date Abbreviations

```html
<p>
    Meeting scheduled for
    <abbr title="Monday">Mon</abbr>,
    <abbr title="December">Dec</abbr> 15th at
    2:00 <abbr title="Post Meridiem">PM</abbr>
    <abbr title="Eastern Standard Time">EST</abbr>.
</p>
```

### Example 11: Financial Report with Abbreviations

```html
<h2>Financial Summary</h2>
<p>
    <abbr title="Year to Date">YTD</abbr> revenue increased by 15%,
    with <abbr title="Earnings Before Interest, Taxes, Depreciation, and Amortization">EBITDA</abbr>
    reaching $2.5M. The <abbr title="Return on Investment">ROI</abbr>
    for <abbr title="Research and Development">R&D</abbr> was 8.2%.
</p>
```

### Example 12: Legal Document

```html
<p>
    The <abbr title="Defendant">Def.</abbr> entered into an agreement with
    <abbr title="Plaintiff">Pl.</abbr> on behalf of
    <abbr title="Example Corporation">Example Corp.</abbr>
    per <abbr title="Section">Sec.</abbr> 12.3.
</p>
```

### Example 13: Repeating Template with Dynamic Abbreviations

```html
<div data-bind="{{model.technicalTerms}}">
    <p>
        <abbr title="{{.fullName}}">{{.abbr}}</abbr>: {{.description}}
    </p>
</div>

<!-- With model data:
{
    "technicalTerms": [
        { "abbr": "API", "fullName": "Application Programming Interface", "description": "..." },
        { "abbr": "SDK", "fullName": "Software Development Kit", "description": "..." },
        { "abbr": "IDE", "fullName": "Integrated Development Environment", "description": "..." }
    ]
}
-->
```

### Example 14: Chemical and Scientific Abbreviations

```html
<p>
    The solution contains 5 <abbr title="milligrams">mg</abbr> of
    <abbr title="Sodium Chloride">NaCl</abbr> per
    <abbr title="milliliter">mL</abbr>. Temperature was maintained at
    37°<abbr title="Celsius">C</abbr> throughout the experiment.
</p>
```

### Example 15: Country and Organization Codes

```html
<table>
    <thead>
        <tr>
            <th>Country</th>
            <th>Organization</th>
            <th>Member Since</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><abbr title="United States of America">USA</abbr></td>
            <td><abbr title="United Nations">UN</abbr></td>
            <td>1945</td>
        </tr>
        <tr>
            <td><abbr title="United Kingdom">UK</abbr></td>
            <td><abbr title="European Union">EU</abbr></td>
            <td>1973</td>
        </tr>
    </tbody>
</table>
```

### Example 16: Software Version Numbers

```html
<p>
    Current version: <abbr title="Version">v</abbr>2.5.3
    (<abbr title="Release Candidate">RC</abbr>1)
</p>
<p>
    Requires: <abbr title="Minimum">Min</abbr>
    <abbr title="Version">v</abbr>1.0,
    <abbr title="Maximum">Max</abbr>
    <abbr title="Version">v</abbr>3.0
</p>
```

### Example 17: Conditional Abbreviation Display

```html
<p>
    Status:
    <abbr
        title="{{model.isComplete ? 'Completed' : 'In Progress'}}"
        style="color: {{model.isComplete ? 'green' : 'orange'}}">
        {{model.isComplete ? 'CMPL' : 'IP'}}
    </abbr>
</p>
```

### Example 18: Glossary with Abbreviations

```html
<h2>Glossary</h2>
<dl>
    <dt><abbr title="Portable Document Format">PDF</abbr></dt>
    <dd>A file format for documents that preserves layout and formatting.</dd>

    <dt><abbr title="Extensible Markup Language">XML</abbr></dt>
    <dd>A markup language that defines rules for encoding documents.</dd>

    <dt><abbr title="Uniform Resource Locator">URL</abbr></dt>
    <dd>The address of a resource on the internet.</dd>
</dl>
```

### Example 19: Medical Prescription

```html
<h2>Prescription</h2>
<p>
    <strong>Rx:</strong> Ibuprofen 400 <abbr title="milligrams">mg</abbr>
</p>
<p>
    <strong>Sig:</strong> Take 1 tablet <abbr title="by mouth">PO</abbr>
    <abbr title="every">q</abbr> 6 <abbr title="hours">h</abbr>
    <abbr title="as needed">PRN</abbr> pain
</p>
<p>
    <strong>Disp:</strong> 30 tablets
</p>
<p>
    <strong>Refills:</strong> 2
</p>
```

### Example 20: Product Specifications with Units

```html
<h2>Technical Specifications</h2>
<table>
    <tr>
        <td>Processor:</td>
        <td>Intel Core i7 @ 3.5 <abbr title="Gigahertz">GHz</abbr></td>
    </tr>
    <tr>
        <td>Memory:</td>
        <td>32 <abbr title="Gigabytes">GB</abbr>
            <abbr title="Double Data Rate 4">DDR4</abbr>
            <abbr title="Random Access Memory">RAM</abbr></td>
    </tr>
    <tr>
        <td>Graphics:</td>
        <td>NVIDIA GeForce with 8 <abbr title="Gigabytes">GB</abbr>
            <abbr title="Graphics Double Data Rate 6">GDDR6</abbr></td>
    </tr>
    <tr>
        <td>Display:</td>
        <td>27" <abbr title="Ultra High Definition">UHD</abbr>
            (3840 × 2160 <abbr title="pixels">px</abbr>)</td>
    </tr>
    <tr>
        <td>Connectivity:</td>
        <td><abbr title="Wireless Fidelity">WiFi</abbr> 6,
            <abbr title="Bluetooth">BT</abbr> 5.0,
            <abbr title="Universal Serial Bus">USB</abbr>-C</td>
    </tr>
</table>
```

---

## See Also

- [span Element](/reference/htmlelements/html_span_element)
- [abbr CSS Styling](/reference/styles/abbr)
- [@title attribute](/reference/htmlattributes/attr_title)
- [@class attribute](/reference/htmlattributes/attr_class)
- [Data Binding](/reference/databinding/)
- [Inline Elements](/reference/htmlelements/inline)

---
