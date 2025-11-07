---
layout: default
title: output, slot, num
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;output&gt;, &lt;slot&gt;, &lt;num&gt; : Inline Textual Value Elements
{: .no_toc }

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---


## Summary

The library supports a variety of semantic HTML elements that provide meaning to inline text content. These elements not only convey semantic information but also apply default styling appropriate to their purpose. This document covers: `<output>`, `<slot>` and `<num>` that are appropriate for use with value information..

They can each be styled indepently or as a tag group, and support the full styling capabilities of a [span](html_span_element.html).

The `<num>` element can also have numeric values bound from the data and formatted to a readable/local variation.

## Overview

These inline semantic elements help structure and style text with specific meanings:

- **`<output>`**: Output or result values from calculations
- **`<slot>`**: Placeholder for dynamic content insertion
- **`<num>`**: Formatted numeric values with custom display

---

## &lt;output&gt; : Output Element

The `<output>` element represents the result of a calculation, user action, or output value.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `title` | string | Optional outline/bookmark title. |
| `hidden` | string | Set to "hidden" to hide the element. |

### Examples

```html
{% raw %}<!-- Calculation result -->
<p>Total: <output style="font-weight: bold;">${{model.total}}</output></p>

<!-- Formula result -->
<p>The area is <output>{{model.width * model.height}}</output> square units.</p>

<!-- With for attribute -->
<p>Sum of values: <output>{{model.sum}}</output></p>

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

<!-- Multiple outputs with calculation in table -->
<table>
    <tr>
        <td>Subtotal:</td>
        <td><output>${{model.subtotal}}</output></td>
    </tr>
    <tr>
        <td>Tax @ {{model.tax * 100}}%</td>
        <td><output>${{model.subtotal * model.tax}}</output></td>
    </tr>
    <tr style="font-weight: bold;">
        <td>Total:</td>
        <td>
            <var data-id='grandTotal' data-value='{{model.subtotal + (model.subtotal * model.tax)}}' ></var>
            <output style="font-size: 12pt;">${{grandTotal}}</output>
        </td>
    </tr>
</table>{% endraw %}
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
{% raw %}<!-- Basic slot -->
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
</div>{% endraw %}
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
- `£#,##0.00`: Custom format

### Examples

```html
{% raw %}<!-- Currency formatting -->
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
<p>Phone: <num data-value="{{model.phoneNumber}}" data-format="(###) ###-####"></num></p>{% endraw %}
```

---

## CSS Styling

All these elements support standard CSS styling:

```css

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
