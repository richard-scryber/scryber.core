---
layout: default
title: data-template
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-template : The Inline Template Content Attribute

The `data-template` attribute allows you to define template content inline as a string attribute value, rather than as child elements. This provides a convenient way to specify simple templates, dynamically generate template content, or create templates from external sources.

---

## Summary

The `data-template` attribute enables inline template definition, providing:
- **Inline HTML content** specification as an attribute value
- **Dynamic template generation** from code or external sources
- **Simplified syntax** for simple templates
- **Programmatic template creation** without DOM manipulation

This attribute is useful for:
- Simple conditional templates with minimal content
- Dynamically generated template content
- Templates loaded from databases or external files
- Programmatic document generation
- Templates with content determined at runtime

**Note**: When `data-template` is specified, it takes precedence over child elements within the template or conditional component.

---

## Usage

The `data-template` attribute is applied to `<if>` elements and can be used with `<template>` elements (via the `data-content` attribute synonym):

```html
<!-- Simple conditional template -->
<if data-test="{{model.showMessage}}"
    data-template="<div style='color: red;'>Important message</div>">
</if>

<!-- On template element (use data-content) -->
<template data-bind="{{items}}"
          data-content="<div>{{.name}}</div>">
</template>
```

### Basic Syntax

```html
<!-- Conditional rendering with inline template -->
<if data-test="{{condition}}"
    data-template="<p>HTML content here</p>">
</if>

<!-- Bound value for dynamic template -->
<if data-test="{{condition}}"
    data-template="{{model.templateHtml}}">
</if>

<!-- Template element (use data-content attribute) -->
<template data-bind="{{items}}"
          data-content="<span>{{.text}}</span>">
</template>
```

---

## Supported Elements

The `data-template` attribute is supported on the following elements:

- `<if>` - Conditional rendering element

**Note**: For `<template>` elements, use the synonym attribute `data-content` instead.

---

## Binding Values

### Attribute Values

| Value Type | Description |
|------------|-------------|
| **HTML String** | Literal HTML markup as a string |
| **Binding Expression** | Expression that evaluates to HTML string: `{{expression}}` |

### Type

**Type**: `string` (HTML markup)
**Default**: `null` (use child elements)

### Expression Support

The attribute accepts literal HTML strings or binding expressions:

```html
<!-- Literal HTML -->
<if data-test="{{model.show}}"
    data-template="<div class='alert'>Alert content</div>">
</if>

<!-- Bound from model -->
<if data-test="{{model.show}}"
    data-template="{{model.contentHtml}}">
</if>

<!-- Conditional expression -->
<if data-test="{{model.isUrgent}}"
    data-template="{{model.isUrgent ? '<span style=\'color:red;\'>URGENT</span>' : '<span>Normal</span>'}}">
</if>
```

---

## Notes

### Precedence Over Child Elements

When `data-template` is specified, it takes precedence over any child elements:

```html
<!-- This child content is IGNORED -->
<if data-test="{{condition}}"
    data-template="<p>Inline template</p>">
    <p>This content will NOT be rendered</p>
</if>

<!-- Rendered output will be: <p>Inline template</p> -->
```

### HTML Escaping

HTML content within the attribute value must be properly escaped:

```html
<!-- Single quotes inside double quotes -->
<if data-test="{{condition}}"
    data-template="<div style='color: red;'>Text</div>">
</if>

<!-- Or use HTML entities -->
<if data-test="{{condition}}"
    data-template="<div style=&quot;color: red;&quot;>Text</div>">
</if>
```

### Data Binding Within Templates

Inline templates support full data binding syntax:

```html
<if data-test="{{model.showUser}}"
    data-template="<div><strong>{{model.userName}}</strong> - {{model.userEmail}}</div>">
</if>
```

### Namespace Handling

When used within HTML documents, the inline template content inherits the HTML namespace automatically:

```html
<html xmlns='http://www.w3.org/1999/xhtml'>
<body>
    <if data-test="{{condition}}"
        data-template="<div>Content</div>">
    </if>
    <!-- Div element has HTML namespace automatically -->
</body>
</html>
```

### Dynamic Template Loading

Templates can be loaded from external sources:

```html
<!-- Model: { condition: true, templateContent: "<div>Loaded content</div>" } -->
<if data-test="{{model.condition}}"
    data-template="{{model.templateContent}}">
</if>
```

### Multi-Line Content

For readability, use appropriate quoting:

```html
<if data-test="{{model.show}}"
    data-template="<div style='padding: 10pt; border: 1pt solid black;'>
                      <h3>Title</h3>
                      <p>Content goes here</p>
                   </div>">
</if>
```

### Performance Considerations

- Inline templates are parsed once during document initialization
- No significant performance difference vs. child elements
- Useful for programmatic generation to avoid DOM manipulation
- Cache template strings when generating many similar templates

### Comparison with Child Elements

**Use inline templates when**:
- Content is simple and fits in one line
- Template is generated dynamically
- Content is loaded from external source
- Programmatic generation is easier

**Use child elements when**:
- Content is complex or multi-line
- Better readability is desired
- IDE support for HTML editing is important
- Content contains many nested elements

---

## Examples

### 1. Simple Conditional Message

Display a simple message conditionally:

```html
<if data-test="{{model.hasError}}"
    data-template="<div style='color: red; font-weight: bold;'>Error occurred!</div>">
</if>
```

### 2. Dynamic Content from Model

Load template content from model property:

```html
<!-- Model: { showNotice: true, noticeHtml: "<div class='notice'>Important notice</div>" } -->
<if data-test="{{model.showNotice}}"
    data-template="{{model.noticeHtml}}">
</if>
```

### 3. Conditional Badge

Display different badge styles:

```html
<if data-test="{{model.status == 'urgent'}}"
    data-template="<span style='background-color: red; color: white; padding: 5pt; border-radius: 3pt;'>URGENT</span>">
</if>

<if data-test="{{model.status == 'normal'}}"
    data-template="<span style='background-color: green; color: white; padding: 5pt; border-radius: 3pt;'>NORMAL</span>">
</if>
```

### 4. Inline Alert Box

Conditional alert with styling:

```html
<if data-test="{{model.showWarning}}"
    data-template="<div style='background-color: #fff3cd; border: 1pt solid #ffc107; padding: 15pt; margin: 10pt 0;'>
                      <strong>Warning:</strong> {{model.warningMessage}}
                   </div>">
</if>
```

### 5. Dynamic Icon Display

Show different icons based on status:

```html
<if data-test="{{model.isComplete}}"
    data-template="<span style='color: green; font-size: 14pt;'>✓ Complete</span>">
</if>

<if data-test="{{!model.isComplete}}"
    data-template="<span style='color: orange; font-size: 14pt;'>⧗ In Progress</span>">
</if>
```

### 6. Programmatically Generated Template

Generate template content in code:

```html
<!-- Model: { showDetails: true, detailsHtml: GenerateDetailsHtml() } -->
<if data-test="{{model.showDetails}}"
    data-template="{{model.detailsHtml}}">
</if>
```

```csharp
// C# code to generate template
string GenerateDetailsHtml()
{
    var sb = new StringBuilder();
    sb.Append("<div style='padding: 10pt;'>");
    sb.Append("<h3>Generated Details</h3>");
    sb.Append("<ul>");
    foreach (var item in items)
    {
        sb.AppendFormat("<li>{0}</li>", item);
    }
    sb.Append("</ul>");
    sb.Append("</div>");
    return sb.ToString();
}
```

### 7. Database-Loaded Template

Load template content from database:

```html
<!-- Model: { showCustomSection: true, customHtml: LoadFromDatabase("section_template") } -->
<if data-test="{{model.showCustomSection}}"
    data-template="{{model.customHtml}}">
</if>
```

### 8. Conditional Pricing Display

Show pricing with conditional formatting:

```html
<if data-test="{{model.onSale}}"
    data-template="<div style='text-align: center;'>
                      <div style='text-decoration: line-through; color: #999;'>${{model.regularPrice}}</div>
                      <div style='color: red; font-size: 16pt; font-weight: bold;'>${{model.salePrice}}</div>
                      <div style='color: red; font-size: 10pt;'>Save {{model.discountPercent}}%!</div>
                   </div>">
</if>
```

### 9. Conditional Footer Note

Add conditional footer information:

```html
<if data-test="{{model.isConfidential}}"
    data-template="<div style='margin-top: 20pt; padding: 10pt; background-color: #f8d7da; border: 2pt solid #dc3545;'>
                      <strong style='color: #dc3545;'>CONFIDENTIAL:</strong>
                      This document contains proprietary information.
                   </div>">
</if>
```

### 10. Inline User Badge

Display user type badge:

```html
<div>
    User: {{model.userName}}
    <if data-test="{{model.userType == 'admin'}}"
        data-template="<span style='background-color: #dc3545; color: white; padding: 3pt 8pt; margin-left: 5pt; font-size: 8pt; border-radius: 3pt;'>ADMIN</span>">
    </if>
    <if data-test="{{model.userType == 'premium'}}"
        data-template="<span style='background-color: #ffc107; color: black; padding: 3pt 8pt; margin-left: 5pt; font-size: 8pt; border-radius: 3pt;'>PREMIUM</span>">
    </if>
</div>
```

### 11. Conditional Watermark

Add watermark for draft documents:

```html
<if data-test="{{model.isDraft}}"
    data-template="<div style='position: absolute; top: 50%; left: 50%; transform: rotate(-45deg);
                               font-size: 72pt; color: rgba(255,0,0,0.2); font-weight: bold;'>
                      DRAFT
                   </div>">
</if>
```

### 12. Simple Table Row

Conditional table row with inline template:

```html
<table style="width: 100%;">
    <tr>
        <td>Order Status:</td>
        <td>
            <if data-test="{{model.orderStatus == 'shipped'}}"
                data-template="<span style='color: green; font-weight: bold;'>✓ Shipped</span>">
            </if>
            <if data-test="{{model.orderStatus == 'pending'}}"
                data-template="<span style='color: orange; font-weight: bold;'>⧗ Pending</span>">
            </if>
            <if data-test="{{model.orderStatus == 'cancelled'}}"
                data-template="<span style='color: red; font-weight: bold;'>✗ Cancelled</span>">
            </if>
        </td>
    </tr>
</table>
```

### 13. Internationalized Content

Load locale-specific templates:

```html
<!-- Model: { language: "en", contentEn: "<p>English content</p>", contentEs: "<p>Contenido español</p>" } -->
<if data-test="{{model.language == 'en'}}"
    data-template="{{model.contentEn}}">
</if>

<if data-test="{{model.language == 'es'}}"
    data-template="{{model.contentEs}}">
</if>
```

### 14. Conditional QR Code

Show QR code with description:

```html
<if data-test="{{model.includeQRCode}}"
    data-template="<div style='text-align: center; margin: 20pt 0;'>
                      <img src='{{model.qrCodeUrl}}' style='width: 100pt; height: 100pt;'/>
                      <div style='font-size: 8pt; color: #666; margin-top: 5pt;'>
                        Scan to access online version
                      </div>
                   </div>">
</if>
```

### 15. Terms and Conditions Section

Conditionally include terms:

```html
<if data-test="{{model.includeTerms}}"
    data-template="<div style='margin-top: 30pt; padding: 15pt; border-top: 2pt solid black;'>
                      <h3 style='margin: 0 0 10pt 0;'>Terms and Conditions</h3>
                      <div style='font-size: 8pt; line-height: 1.4;'>
                        {{model.termsText}}
                      </div>
                   </div>">
</if>
```

### 16. Status Indicator Panel

Multi-line status panel:

```html
<if data-test="{{model.systemStatus != 'operational'}}"
    data-template="<div style='background-color: #fff3cd; border-left: 5pt solid #ffc107; padding: 15pt; margin: 10pt 0;'>
                      <div style='font-weight: bold; font-size: 12pt; margin-bottom: 5pt;'>
                        System Status Alert
                      </div>
                      <div>
                        Current Status: <strong>{{model.systemStatus}}</strong>
                      </div>
                      <div style='margin-top: 5pt; font-size: 9pt; color: #666;'>
                        Last Updated: {{model.statusTimestamp}}
                      </div>
                   </div>">
</if>
```

### 17. Conditional Signature Block

Add signature block for certain document types:

```html
<if data-test="{{model.requiresSignature}}"
    data-template="<div style='margin-top: 40pt; padding: 20pt; border: 1pt solid #000;'>
                      <div style='margin-bottom: 30pt;'>
                        <div>Signature: _________________________________</div>
                      </div>
                      <div style='margin-bottom: 10pt;'>
                        <div>Print Name: _________________________________</div>
                      </div>
                      <div>
                        <div>Date: _________________________________</div>
                      </div>
                   </div>">
</if>
```

### 18. Simple Payment Instructions

Conditional payment details:

```html
<if data-test="{{model.paymentMethod == 'check'}}"
    data-template="<div style='background-color: #d1ecf1; padding: 10pt; margin: 10pt 0;'>
                      <strong>Payment Instructions:</strong><br/>
                      Please make check payable to: {{model.payeeName}}<br/>
                      Mail to: {{model.mailingAddress}}
                   </div>">
</if>

<if data-test="{{model.paymentMethod == 'wire'}}"
    data-template="<div style='background-color: #d1ecf1; padding: 10pt; margin: 10pt 0;'>
                      <strong>Wire Transfer Instructions:</strong><br/>
                      Bank: {{model.bankName}}<br/>
                      Account: {{model.accountNumber}}<br/>
                      Routing: {{model.routingNumber}}
                   </div>">
</if>
```

### 19. Promotional Banner

Conditional promotional content:

```html
<if data-test="{{model.showPromotion}}"
    data-template="<div style='background: linear-gradient(to right, #667eea, #764ba2);
                               color: white; padding: 20pt; text-align: center;
                               margin-bottom: 20pt; border-radius: 5pt;'>
                      <div style='font-size: 18pt; font-weight: bold; margin-bottom: 5pt;'>
                        {{model.promoTitle}}
                      </div>
                      <div style='font-size: 12pt;'>
                        {{model.promoMessage}}
                      </div>
                      <div style='margin-top: 10pt; font-size: 9pt;'>
                        Offer expires: {{model.promoExpiry}}
                      </div>
                   </div>">
</if>
```

### 20. Complex Conditional Layout

Multi-section conditional content:

```html
<if data-test="{{model.showDetailedSummary}}"
    data-template="<div style='border: 2pt solid #336699; padding: 15pt; margin: 20pt 0;'>
                      <div style='background-color: #336699; color: white; padding: 10pt; margin: -15pt -15pt 15pt -15pt;'>
                        <h2 style='margin: 0;'>Detailed Summary</h2>
                      </div>
                      <table style='width: 100%; border-collapse: collapse;'>
                        <tr>
                          <td style='padding: 5pt; border-bottom: 1pt solid #ddd; font-weight: bold;'>Total Items:</td>
                          <td style='padding: 5pt; border-bottom: 1pt solid #ddd; text-align: right;'>{{model.totalItems}}</td>
                        </tr>
                        <tr>
                          <td style='padding: 5pt; border-bottom: 1pt solid #ddd; font-weight: bold;'>Subtotal:</td>
                          <td style='padding: 5pt; border-bottom: 1pt solid #ddd; text-align: right;'>${{model.subtotal}}</td>
                        </tr>
                        <tr>
                          <td style='padding: 5pt; border-bottom: 1pt solid #ddd; font-weight: bold;'>Tax:</td>
                          <td style='padding: 5pt; border-bottom: 1pt solid #ddd; text-align: right;'>${{model.tax}}</td>
                        </tr>
                        <tr>
                          <td style='padding: 5pt; font-weight: bold; font-size: 12pt;'>Total:</td>
                          <td style='padding: 5pt; text-align: right; font-weight: bold; font-size: 12pt;'>${{model.total}}</td>
                        </tr>
                      </table>
                   </div>">
</if>
```

---

## See Also

- [if element](/reference/htmltags/if.html) - Conditional rendering element
- [data-content attribute](/reference/htmlattributes/data-content.html) - Synonym for template elements
- [data-test attribute](/reference/htmlattributes/data-test.html) - Conditional test expressions
- [template element](/reference/htmltags/template.html) - Template for repeating content
- [Data Binding](/reference/binding/) - Complete data binding guide
- [Expressions](/reference/expressions/) - Expression syntax reference

---
