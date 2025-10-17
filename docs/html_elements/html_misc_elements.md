---
layout: default
title: Miscellaneous Semantic Elements
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# Miscellaneous Semantic HTML Elements

Scryber supports a variety of semantic HTML elements that provide meaning to inline text content. These elements not only convey semantic information but also apply default styling appropriate to their purpose. This document covers: `<abbr>`, `<cite>`, `<dfn>`, `<q>`, `<output>`, `<slot>`, `<num>`, and `<var>`.

## Overview

These inline semantic elements help structure and style text with specific meanings:

- **`<abbr>`**: Abbreviations and acronyms (italic by default)
- **`<cite>`**: Citations and references (italic by default)
- **`<dfn>`**: Definitions of terms (italic by default)
- **`<q>`**: Short inline quotations
- **`<output>`**: Output or result values from calculations
- **`<slot>`**: Placeholder for dynamic content insertion
- **`<num>`**: Formatted numeric values with custom display
- **`<var>`**: Variables and placeholders (italic by default)

---

## &lt;abbr&gt; : Abbreviation Element

The `<abbr>` element represents an abbreviation or acronym, with optional expansion text.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `title` | string | Full expansion or description of the abbreviation (not used for outline). |
| `data-outline-title` | string | Optional outline/bookmark title. |
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `hidden` | string | Set to "hidden" to hide the element. |

### Default Styling

- Font style: **Italic**

### Examples

```html
<!-- Basic abbreviation -->
<p>The <abbr title="World Health Organization">WHO</abbr> issued new guidelines.</p>

<!-- With custom styling -->
<p>Contact us at <abbr title="United States" style="font-variant: small-caps;">US</abbr> headquarters.</p>

<!-- Technical abbreviation -->
<p>Files are stored in <abbr title="Portable Document Format">PDF</abbr> format.</p>

<!-- With data binding -->
<p>The <abbr title="{{model.abbreviationExpansion}}">{{model.abbreviation}}</abbr> applies.</p>
```

---

## &lt;cite&gt; : Citation Element

The `<cite>` element represents a citation or reference to a creative work, publication, or source.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `title` | string | Additional information about the citation (not used for outline). |
| `data-outline-title` | string | Optional outline/bookmark title. |
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `hidden` | string | Set to "hidden" to hide the element. |

### Default Styling

- Font style: **Italic**

### Examples

```html
<!-- Book citation -->
<p>As described in <cite>The Art of Programming</cite>, best practices include...</p>

<!-- Article citation -->
<p>According to <cite>Journal of Modern Science</cite>, the results show...</p>

<!-- With styling -->
<p>The study <cite style="color: #336699;">Smith et al. 2023</cite> found that...</p>

<!-- Multiple citations -->
<p>Several sources (<cite>Brown 2022</cite>, <cite>Jones 2023</cite>) confirm this.</p>

<!-- Data-bound citation -->
<p>Reference: <cite>{{model.citationTitle}}</cite> ({{model.citationYear}})</p>
```

---

## &lt;dfn&gt; : Definition Element

The `<dfn>` element represents the defining instance of a term, typically the first occurrence where the term is explained.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `title` | string | Additional information about the definition (not used for outline). |
| `data-outline-title` | string | Optional outline/bookmark title. |
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `hidden` | string | Set to "hidden" to hide the element. |

### Default Styling

- Font style: **Italic**

### Examples

```html
<!-- Basic definition -->
<p><dfn>Scryber</dfn> is a PDF generation library for .NET applications.</p>

<!-- Definition with context -->
<p>A <dfn>template</dfn> in Scryber is a reusable document structure that can be populated with dynamic data.</p>

<!-- Multiple definitions -->
<p>The <dfn>frameset</dfn> contains one or more <dfn>frames</dfn> that reference content sources.</p>

<!-- Styled definition -->
<p><dfn style="font-weight: bold; color: #336699;">Data binding</dfn> is the process of connecting template elements to data sources.</p>

<!-- With data binding -->
<p><dfn>{{model.termName}}</dfn>: {{model.termDefinition}}</p>
```

---

## &lt;q&gt; : Inline Quotation Element

The `<q>` element represents a short inline quotation. Browsers typically add quotation marks automatically, but Scryber renders the content as-is.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `hidden` | string | Set to "hidden" to hide the element. |
| `title` | string | Optional outline/bookmark title. |

### Examples

```html
<!-- Basic quotation -->
<p>The report stated <q>results exceeded expectations</q> in Q3.</p>

<!-- Nested quotations -->
<p>She said, <q>The manager mentioned <q>we need more time</q> yesterday</q>.</p>

<!-- Styled quotation -->
<p>The motto is <q style="font-style: italic; color: #666;">Innovation through collaboration</q>.</p>

<!-- With data binding -->
<p>Customer feedback: <q>{{model.customerQuote}}</q></p>

<!-- Custom quote styling -->
<style>
    q {
        quotes: "«" "»" "‹" "›";
    }
    q:before { content: open-quote; }
    q:after { content: close-quote; }
</style>
<p>They remarked <q>this is excellent work</q>.</p>
```

---

## &lt;output&gt; : Output Element

The `<output>` element represents the result of a calculation, user action, or output value.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `val` | string | The output value to display. |
| `for` | string | ID of related element(s) that contributed to this output. |
| `form` | string | ID of associated form element. |
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `title` | string | Optional outline/bookmark title. |
| `hidden` | string | Set to "hidden" to hide the element. |

### Examples

```html
<!-- Calculation result -->
<p>Total: <output val="{{model.total}}" style="font-weight: bold;">${{model.total}}</output></p>

<!-- Formula result -->
<p>The area is <output>{{model.width * model.height}}</output> square units.</p>

<!-- With for attribute -->
<p>Sum of values: <output for="value1 value2">{{model.sum}}</output></p>

<!-- Styled output -->
<style>
    .result {
        background-color: #f0f0f0;
        padding: 5pt;
        border-radius: 3pt;
        font-family: monospace;
    }
</style>
<p>Result: <output class="result">{{model.calculationResult}}</output></p>

<!-- Multiple outputs in table -->
<table>
    <tr>
        <td>Subtotal:</td>
        <td><output>${{model.subtotal}}</output></td>
    </tr>
    <tr>
        <td>Tax:</td>
        <td><output>${{model.tax}}</output></td>
    </tr>
    <tr style="font-weight: bold;">
        <td>Total:</td>
        <td><output style="font-size: 12pt;">${{model.total}}</output></td>
    </tr>
</table>
```

---

## &lt;slot&gt; : Content Slot Element

The `<slot>` element represents a placeholder for dynamic content insertion.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `title` | string | Optional outline/bookmark title. |
| `hidden` | string | Set to "hidden" to hide the element. |

### Examples

```html
<!-- Basic slot -->
<div>
    <slot>Default content if no content provided</slot>
</div>

<!-- Named content areas -->
<div class="card">
    <div class="card-header">
        <slot>Header Content</slot>
    </div>
    <div class="card-body">
        <slot>Body Content</slot>
    </div>
</div>

<!-- Styled slot -->
<div style="border: 1pt solid #ccc; padding: 10pt;">
    <slot style="color: #666; font-style: italic;">
        Content will be inserted here
    </slot>
</div>

<!-- Data-bound slot -->
<div>
    <slot>{{model.dynamicContent}}</slot>
</div>
```

---

## &lt;num&gt; : Number Formatting Element

The `<num>` element formats and displays numeric values with custom formatting.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-value` | double | The numeric value to format and display. |
| `data-format` | string | .NET format string for number formatting (e.g., "N2", "C", "P"). |
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `title` | string | Optional outline/bookmark title. |
| `hidden` | string | Set to "hidden" to hide the element. |

### Number Format Strings

Common .NET number format strings:
- `N` or `N2`: Number with commas (e.g., 1,234.56)
- `C` or `C2`: Currency (e.g., $1,234.56)
- `P` or `P2`: Percentage (e.g., 12.34%)
- `F2`: Fixed-point with 2 decimals (e.g., 1234.56)
- `#,##0.00`: Custom format

### Examples

```html
<!-- Currency formatting -->
<p>Price: <num data-value="{{model.price}}" data-format="C2"></num></p>

<!-- Percentage formatting -->
<p>Completion: <num data-value="{{model.completion}}" data-format="P1"></num></p>

<!-- Number with thousands separator -->
<p>Population: <num data-value="{{model.population}}" data-format="N0"></num></p>

<!-- Fixed decimal places -->
<p>Measurement: <num data-value="{{model.measurement}}" data-format="F3"></num> meters</p>

<!-- Text content parsing -->
<p>Value: <num data-format="N2">1234.567</num></p>

<!-- Table with formatted numbers -->
<table>
    <tr>
        <td>Revenue:</td>
        <td><num data-value="{{model.revenue}}" data-format="C0"></num></td>
    </tr>
    <tr>
        <td>Growth:</td>
        <td><num data-value="{{model.growth}}" data-format="P1"></num></td>
    </tr>
    <tr>
        <td>Units Sold:</td>
        <td><num data-value="{{model.unitsSold}}" data-format="N0"></num></td>
    </tr>
</table>

<!-- Styled numbers -->
<style>
    .positive { color: green; }
    .negative { color: red; }
</style>
<p>
    Profit: <num data-value="{{model.profit}}" data-format="C2"
                 class="{{model.profit >= 0 ? 'positive' : 'negative'}}"></num>
</p>

<!-- Custom format -->
<p>Phone: <num data-value="{{model.phoneNumber}}" data-format="(###) ###-####"></num></p>
```

---

## &lt;var&gt; : Variable Element

The `<var>` element represents a variable, placeholder, or mathematical expression variable.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-id` | string | Identifier for storing the value in document parameters. |
| `data-value` | object | Value to store in document parameters (binding only). |
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `title` | string | Optional outline/bookmark title. |
| `hidden` | string | Set to "hidden" to hide the element. |

### Default Styling

- Font style: **Italic**

### Special Behavior

When both `data-id` and `data-value` are set, the value is stored in the document's parameters collection and can be referenced elsewhere. If the element has no visible content, it becomes invisible.

### Examples

```html
<!-- Mathematical variable -->
<p>Let <var>x</var> equal the total value.</p>

<!-- Code variable -->
<p>The function returns <var>result</var> when successful.</p>

<!-- Multiple variables -->
<p>The formula is <var>a</var> + <var>b</var> = <var>c</var></p>

<!-- Styled variable -->
<p>Set <var style="color: #d63384; font-family: monospace;">userName</var> to the input value.</p>

<!-- Store value in document parameters -->
<var data-id="totalAmount" data-value="{{model.total}}"></var>

<!-- Use stored value elsewhere -->
<p>Total: {{params.totalAmount}}</p>

<!-- Visible variable with stored value -->
<p>Total: <var data-id="displayTotal" data-value="{{model.total}}">{{model.total}}</var></p>

<!-- Template with variables -->
<div style="font-family: monospace; background-color: #f5f5f5; padding: 10pt;">
    <p>for (<var>i</var> = 0; <var>i</var> &lt; <var>n</var>; <var>i</var>++)</p>
</div>

<!-- Data binding -->
<p>The current value of <var>{{model.variableName}}</var> is {{model.variableValue}}.</p>
```

---

## Combined Examples

### Technical Documentation

```html
<div class="technical-doc">
    <h3>API Reference</h3>

    <p>
        The <dfn>GetTotal()</dfn> method returns a <var>decimal</var> value
        representing the sum of all items.
    </p>

    <p>
        <strong>Returns:</strong>
        <output>decimal</output> - The total amount
    </p>

    <p>
        <strong>Example:</strong>
        <code>
            <var>total</var> = GetTotal();
        </code>
    </p>

    <p>
        <cite>API Documentation v2.0</cite>
    </p>
</div>
```

### Academic Citation

```html
<div class="reference">
    <p>
        <cite>The Elements of Style</cite> by Strunk and White emphasizes that
        <q>omit needless words</q> is a fundamental principle of good writing.
    </p>

    <p>
        The term <dfn>concision</dfn> refers to expressing ideas using the
        minimum number of words necessary.
    </p>

    <p>
        In linguistics, <abbr title="Natural Language Processing">NLP</abbr>
        systems analyze text for brevity and clarity.
    </p>
</div>
```

### Financial Report

```html
<div class="financial-summary">
    <h3>Quarterly Results</h3>

    <table style="width: 100%;">
        <tr>
            <td>Revenue:</td>
            <td>
                <output>
                    <num data-value="{{model.revenue}}" data-format="C0"></num>
                </output>
            </td>
        </tr>
        <tr>
            <td>Growth Rate:</td>
            <td>
                <output>
                    <num data-value="{{model.growthRate}}" data-format="P2"></num>
                </output>
            </td>
        </tr>
        <tr>
            <td>Units Sold:</td>
            <td>
                <output>
                    <num data-value="{{model.unitsSold}}" data-format="N0"></num>
                </output>
            </td>
        </tr>
    </table>

    <p style="margin-top: 15pt;">
        The <abbr title="Chief Executive Officer">CEO</abbr> stated
        <q>These results demonstrate strong market performance</q>
        in the quarterly report.
    </p>

    <p style="font-size: 8pt; color: #666;">
        Source: <cite>{{model.companyName}} Q{{model.quarter}} {{model.year}} Report</cite>
    </p>
</div>
```

### Formula Documentation

```html
<div class="formula-doc">
    <h3>Distance Formula</h3>

    <p>
        The <dfn>Euclidean distance</dfn> between two points is calculated as:
    </p>

    <p style="text-align: center; font-size: 14pt; margin: 20pt 0;">
        <var>d</var> = √((<var>x₂</var> - <var>x₁</var>)² + (<var>y₂</var> - <var>y₁</var>)²)
    </p>

    <p>
        Where <var>x₁</var>, <var>y₁</var> represent the first point and
        <var>x₂</var>, <var>y₂</var> represent the second point.
    </p>

    <p>
        For example, with points (3, 4) and (6, 8):
    </p>

    <p>
        <output>
            Distance = <num data-value="5" data-format="F2"></num> units
        </output>
    </p>
</div>
```

### Product Specifications

```html
<div class="product-specs">
    <h3>{{model.productName}} Specifications</h3>

    <dl>
        <dt>Dimensions (<abbr title="Width × Height × Depth">W×H×D</abbr>):</dt>
        <dd>
            <num data-value="{{model.width}}" data-format="F1"></num> ×
            <num data-value="{{model.height}}" data-format="F1"></num> ×
            <num data-value="{{model.depth}}" data-format="F1"></num> inches
        </dd>

        <dt>Weight:</dt>
        <dd><num data-value="{{model.weight}}" data-format="F2"></num> lbs</dd>

        <dt>Capacity:</dt>
        <dd><num data-value="{{model.capacity}}" data-format="N0"></num> items</dd>

        <dt>Price:</dt>
        <dd><num data-value="{{model.price}}" data-format="C2"></num></dd>
    </dl>

    <p style="font-size: 9pt; color: #666; margin-top: 15pt;">
        Specifications subject to change. See <cite>Product Manual v{{model.version}}</cite>
        for complete details.
    </p>
</div>
```

### Scientific Paper Abstract

```html
<div class="abstract">
    <h2>Abstract</h2>

    <p>
        This study examines the effect of <var>temperature</var> on
        <dfn>crystallization rates</dfn> in polymer solutions. The term
        <dfn>nucleation rate</dfn> refers to the frequency at which
        crystal nuclei form per unit volume.
    </p>

    <p>
        Results show that at <num data-value="25" data-format="F0"></num>°C,
        the nucleation rate was <num data-value="{{model.rate1}}" data-format="E2"></num>
        nuclei/cm³/s, while at <num data-value="50" data-format="F0"></num>°C,
        it increased to <num data-value="{{model.rate2}}" data-format="E2"></num>
        nuclei/cm³/s, representing a
        <num data-value="{{model.percentIncrease}}" data-format="P1"></num> increase.
    </p>

    <p>
        As noted by <cite>Smith et al. (2023)</cite>,
        <q>temperature plays a critical role in phase transitions</q>.
    </p>

    <p style="margin-top: 15pt;">
        <strong>Keywords:</strong> crystallization, nucleation,
        <abbr title="Differential Scanning Calorimetry">DSC</abbr>,
        polymer science
    </p>
</div>
```

### Invoice Line Items with Formatting

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="border-bottom: 2pt solid black;">
            <th style="text-align: left;">Description</th>
            <th style="text-align: right;">Quantity</th>
            <th style="text-align: right;">Unit Price</th>
            <th style="text-align: right;">Total</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.lineItems}}">
            <tr style="border-bottom: 1pt solid #ccc;">
                <td>
                    {{.description}}
                    <if data-test="{{.hasDiscount}}">
                        <br/><span style="color: red; font-size: 9pt;">
                            (<num data-value="{{.discountPercent}}" data-format="P0"></num> discount applied)
                        </span>
                    </if>
                </td>
                <td style="text-align: right;">
                    <num data-value="{{.quantity}}" data-format="N0"></num>
                </td>
                <td style="text-align: right;">
                    <num data-value="{{.unitPrice}}" data-format="C2"></num>
                </td>
                <td style="text-align: right;">
                    <num data-value="{{.lineTotal}}" data-format="C2"></num>
                </td>
            </tr>
        </template>
    </tbody>
    <tfoot>
        <tr style="border-top: 2pt solid black; font-weight: bold;">
            <td colspan="3" style="text-align: right; padding-top: 10pt;">Subtotal:</td>
            <td style="text-align: right; padding-top: 10pt;">
                <num data-value="{{model.subtotal}}" data-format="C2"></num>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="text-align: right;">
                Tax (<num data-value="{{model.taxRate}}" data-format="P2"></num>):
            </td>
            <td style="text-align: right;">
                <num data-value="{{model.tax}}" data-format="C2"></num>
            </td>
        </tr>
        <tr style="font-size: 12pt;">
            <td colspan="3" style="text-align: right;">Grand Total:</td>
            <td style="text-align: right;">
                <output>
                    <num data-value="{{model.grandTotal}}" data-format="C2"></num>
                </output>
            </td>
        </tr>
    </tfoot>
</table>
```

---

## CSS Styling

All these elements support standard CSS styling:

```css
/* Override default italic styling */
abbr, cite, dfn, var {
    font-style: normal;
    font-weight: bold;
    color: #336699;
}

/* Quote styling */
q {
    font-style: italic;
    color: #666;
}

/* Output highlighting */
output {
    background-color: #f0f0f0;
    padding: 2pt 5pt;
    border-radius: 3pt;
    font-family: monospace;
}

/* Number formatting */
num {
    font-weight: bold;
    font-family: 'Arial', sans-serif;
}

/* Abbreviation with dotted underline */
abbr {
    text-decoration: underline;
    text-decoration-style: dotted;
}
```

---

## See Also

- [span](/reference/htmltags/span.html) - Generic inline container element
- [strong](/reference/htmltags/strong.html) - Strong emphasis (bold)
- [em](/reference/htmltags/em.html) - Emphasis (italic)
- [Text Formatting](/reference/htmltags/textformatting.html) - Text formatting elements
- [Data Binding](/reference/binding/) - Data binding expressions
- [CSS Styling](/reference/styles/) - CSS styling reference

---
