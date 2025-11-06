---
layout: default
title: color, face, and size (deprecated)
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @color, @face, @size : Deprecated Font Element Attributes

The `color`, `face`, and `size` attributes are deprecated HTML attributes historically used with the `<font>` element for styling text appearance. While still supported for backward compatibility, **modern CSS properties are strongly recommended** for all new templates.

## Usage

These attributes were used with the deprecated `<font>` element to control text styling:
- **`color`**: Sets the text color (use CSS `color` instead)
- **`face`**: Sets the font family name (use CSS `font-family` instead)
- **`size`**: Sets the font size (use CSS `font-size` instead)

**⚠️ Deprecation Notice**: These attributes are deprecated in favor of CSS. They are maintained only for legacy HTML document compatibility and should not be used in new templates.

```html
<!-- Deprecated approach (still works) -->
<font color="red" face="Arial" size="14pt">Styled text</font>

<!-- Modern CSS approach (strongly recommended) -->
<span style="color: red; font-family: Arial; font-size: 14pt;">Styled text</span>
```

---

## Supported Elements

These attributes are only supported by the deprecated `<font>` element:

| Element | Description |
|---------|-------------|
| `<font>` | Deprecated font styling element |

**Note**: These attributes do not work on other HTML elements. Use CSS styling with `style` attribute or CSS classes instead.

---

## Attribute Details

### @color Attribute

Sets the text color using various color formats.

#### Syntax
```html
<font color="value">Text</font>
```

#### Accepted Values

| Value Type | Example | Description |
|------------|---------|-------------|
| Color name | `"red"`, `"blue"`, `"green"` | Standard HTML color names |
| Hex value | `"#FF0000"`, `"#336699"` | Six-digit hexadecimal RGB color |
| Short hex | `"#F00"`, `"#369"` | Three-digit hexadecimal shorthand |
| RGB function | `"rgb(255, 0, 0)"` | RGB color function notation |
| RGBA function | `"rgba(255, 0, 0, 0.5)"` | RGB with alpha transparency |

#### CSS Equivalent
```html
<!-- Old: -->
<font color="red">Text</font>

<!-- New: -->
<span style="color: red;">Text</span>
```

---

### @face Attribute

Sets the font family (typeface) for the text.

#### Syntax
```html
<font face="font-name">Text</font>
```

#### Accepted Values

| Font Family | Example | Description |
|-------------|---------|-------------|
| Single font | `"Arial"` | Single font family name |
| Font list | `"Arial, Helvetica, sans-serif"` | Fallback font list |
| Generic family | `"serif"`, `"sans-serif"`, `"monospace"` | Generic font family keywords |

#### Common Font Families

**Serif Fonts**:
- `"Times New Roman"`
- `"Georgia"`
- `"Garamond"`

**Sans-Serif Fonts**:
- `"Arial"`
- `"Helvetica"`
- `"Verdana"`
- `"Tahoma"`

**Monospace Fonts**:
- `"Courier New"`
- `"Consolas"`
- `"Monaco"`

#### CSS Equivalent
```html
<!-- Old: -->
<font face="Arial">Text</font>

<!-- New: -->
<span style="font-family: Arial, sans-serif;">Text</span>
```

---

### @size Attribute

Sets the font size in points.

#### Syntax
```html
<font size="value">Text</font>
```

#### Accepted Values

| Unit Type | Example | Description |
|-----------|---------|-------------|
| Points | `"12pt"`, `"14pt"`, `"16pt"` | Absolute size in typographic points |
| Pixels | `"16px"`, `"20px"` | Absolute size in screen pixels (less common) |

**Note**: Unlike modern HTML where font size can use relative units, the deprecated `size` attribute primarily works with absolute point values in Scryber PDF generation.

#### CSS Equivalent
```html
<!-- Old: -->
<font size="14pt">Text</font>

<!-- New: -->
<span style="font-size: 14pt;">Text</span>
```

---

## Migration Guide

### Why Migrate to CSS?

1. **Standards Compliance**: Modern HTML standards deprecate presentational markup
2. **Maintainability**: CSS classes centralize styling and make updates easier
3. **Flexibility**: CSS offers far more styling options
4. **Separation of Concerns**: Keep HTML structure separate from presentation
5. **Reusability**: CSS classes can be reused across multiple elements

### Migration Table

| Old (Deprecated) | New (CSS) | Notes |
|-----------------|-----------|-------|
| `<font color="red">` | `style="color: red;"` | Direct inline style |
| `<font face="Arial">` | `style="font-family: Arial;"` | Always include fallback fonts |
| `<font size="14pt">` | `style="font-size: 14pt;"` | Points work best for PDFs |
| Combined attributes | Combined CSS properties | Use semicolons to separate |

### Complete Migration Example

**Before (Deprecated):**
```html
<font color="#336699" face="Arial" size="16pt">
    Important heading text
</font>
```

**After (CSS Inline):**
```html
<span style="color: #336699; font-family: Arial, sans-serif; font-size: 16pt;">
    Important heading text
</span>
```

**After (CSS Class - Best Practice):**
```html
<style>
    .heading {
        color: #336699;
        font-family: Arial, sans-serif;
        font-size: 16pt;
        font-weight: bold;
    }
</style>

<span class="heading">Important heading text</span>
```

---

## Binding Values

While deprecated, these attributes support data binding:

### Static Values
```html
<font color="red" face="Arial" size="12pt">Static text</font>
```

### Dynamic Values with Data Binding
```html
<!-- Model: { textColor: "blue", fontFamily: "Helvetica", fontSize: "14pt" } -->
<font color="{{model.textColor}}"
      face="{{model.fontFamily}}"
      size="{{model.fontSize}}">
    Dynamically styled text
</font>
```

### Conditional Values
```html
<!-- Model: { isError: true } -->
<font color="{{model.isError ? 'red' : 'black'}}"
      size="{{model.isError ? '14pt' : '12pt'}}">
    Conditional styling
</font>
```

**Better Alternative with CSS:**
```html
<span style="color: {{model.textColor}}; font-family: {{model.fontFamily}}; font-size: {{model.fontSize}};">
    Dynamically styled text
</span>
```

---

## Notes

### Legacy Support

These attributes are maintained for:
- Converting legacy HTML documents to PDF
- Supporting old HTML templates
- Backward compatibility with existing systems
- Quick prototyping (though CSS is still preferred)

### Limitations

**Cannot control:**
- Font weight (bold)
- Font style (italic)
- Text decoration (underline)
- Letter spacing
- Line height
- Text transform
- Text shadow
- And dozens of other CSS properties

**CSS provides:**
- All font styling capabilities
- Text effects and transformations
- Responsive sizing with relative units
- Fine-grained control over typography
- Better browser/PDF reader compatibility

### Performance

These deprecated attributes have **no performance difference** from CSS in Scryber PDF generation. Both are processed identically during rendering.

### Mixing with CSS

You can combine deprecated attributes with CSS, though this is not recommended:

```html
<font color="blue" size="14pt"
      style="font-weight: bold; text-decoration: underline;">
    Mixed styling (not recommended)
</font>
```

The CSS `style` attribute takes precedence over the deprecated attributes when conflicts occur.

---

## Examples

### Example 1: Basic Color Styling

```html
<!-- Red text -->
<font color="red">This text is red</font>

<!-- Blue text with hex -->
<font color="#0000FF">This text is blue</font>

<!-- Green text with RGB -->
<font color="rgb(0, 128, 0)">This text is green</font>

<!-- Modern CSS equivalent -->
<span style="color: red;">This text is red</span>
<span style="color: #0000FF;">This text is blue</span>
<span style="color: rgb(0, 128, 0);">This text is green</span>
```

### Example 2: Font Family Variations

```html
<!-- Serif font -->
<font face="Times New Roman">Serif text in Times New Roman</font>

<!-- Sans-serif font -->
<font face="Arial">Sans-serif text in Arial</font>

<!-- Monospace font -->
<font face="Courier New">Monospace text in Courier</font>

<!-- Font with fallbacks -->
<font face="Helvetica, Arial, sans-serif">Text with fallback fonts</font>

<!-- Modern CSS equivalent -->
<span style="font-family: 'Times New Roman', serif;">Serif text</span>
<span style="font-family: Arial, sans-serif;">Sans-serif text</span>
<span style="font-family: 'Courier New', monospace;">Monospace text</span>
```

### Example 3: Font Size Variations

```html
<!-- Small text -->
<font size="10pt">Small text (10pt)</font>

<!-- Normal text -->
<font size="12pt">Normal text (12pt)</font>

<!-- Large text -->
<font size="16pt">Large text (16pt)</font>

<!-- Heading size -->
<font size="24pt">Heading size (24pt)</font>

<!-- Modern CSS equivalent -->
<span style="font-size: 10pt;">Small text (10pt)</span>
<span style="font-size: 12pt;">Normal text (12pt)</span>
<span style="font-size: 16pt;">Large text (16pt)</span>
<span style="font-size: 24pt;">Heading size (24pt)</span>
```

### Example 4: Combined Attributes

```html
<!-- All three attributes -->
<font color="#336699" face="Arial" size="14pt">
    Fully styled text with color, font, and size
</font>

<!-- Modern CSS equivalent -->
<span style="color: #336699; font-family: Arial, sans-serif; font-size: 14pt;">
    Fully styled text with color, font, and size
</span>
```

### Example 5: Nested Font Tags (Not Recommended)

```html
<!-- Nested font tags override parent attributes -->
<font color="blue" size="12pt">
    Blue text at 12pt with
    <font color="red">red text nested</font>
    and back to blue
</font>

<!-- Better with CSS -->
<span style="color: blue; font-size: 12pt;">
    Blue text at 12pt with
    <span style="color: red;">red text nested</span>
    and back to blue
</span>
```

### Example 6: Legacy Email Template

```html
<!-- Typical legacy email HTML -->
<table width="600" border="0" cellpadding="10" bgcolor="#f0f0f0">
    <tr>
        <td>
            <font color="#333333" face="Verdana, Arial, sans-serif" size="11pt">
                <strong>Newsletter Header</strong><br/><br/>

                Dear Subscriber,<br/><br/>

                Thank you for signing up. Your confirmation code is:
                <font color="#ff0000" size="14pt"><strong>ABC123</strong></font>
                <br/><br/>

                <font size="9pt" color="#666666">
                    This is an automated message. Please do not reply.
                </font>
            </font>
        </td>
    </tr>
</table>
```

### Example 7: Status Messages with Colors

```html
<!-- Error message -->
<font color="#d32f2f" face="Arial" size="12pt">
    ✗ Error: Operation failed
</font>

<!-- Success message -->
<font color="#388e3c" face="Arial" size="12pt">
    ✓ Success: Operation completed
</font>

<!-- Warning message -->
<font color="#f57c00" face="Arial" size="12pt">
    ⚠ Warning: Check your input
</font>

<!-- Info message -->
<font color="#1976d2" face="Arial" size="12pt">
    ℹ Info: Processing in progress
</font>
```

### Example 8: Modernized Version with CSS Classes

```html
<style>
    .error {
        color: #d32f2f;
        font-family: Arial, sans-serif;
        font-size: 12pt;
        font-weight: bold;
    }
    .success {
        color: #388e3c;
        font-family: Arial, sans-serif;
        font-size: 12pt;
        font-weight: bold;
    }
    .warning {
        color: #f57c00;
        font-family: Arial, sans-serif;
        font-size: 12pt;
        font-weight: bold;
    }
    .info {
        color: #1976d2;
        font-family: Arial, sans-serif;
        font-size: 12pt;
    }
</style>

<!-- Much cleaner and maintainable -->
<p class="error">✗ Error: Operation failed</p>
<p class="success">✓ Success: Operation completed</p>
<p class="warning">⚠ Warning: Check your input</p>
<p class="info">ℹ Info: Processing in progress</p>
```

### Example 9: Data-Bound Legacy Document

```html
<!-- Model: {
    userName: "John Doe",
    statusColor: "green",
    statusMessage: "Active",
    companyName: "Acme Corp"
} -->

<h1>
    <font color="#2c3e50" face="Arial" size="24pt">
        Welcome, {{model.userName}}
    </font>
</h1>

<p>
    <font face="Arial" size="12pt">
        Company:
        <font color="#336699" size="14pt">{{model.companyName}}</font>
    </font>
</p>

<p>
    <font face="Arial" size="12pt">
        Account Status:
        <font color="{{model.statusColor}}" size="13pt">
            <strong>{{model.statusMessage}}</strong>
        </font>
    </font>
</p>
```

### Example 10: Report Header with Mixed Styling

```html
<div style="padding: 20pt; background-color: #f5f5f5; border: 2pt solid #cccccc;">
    <font color="#2c3e50" face="Arial" size="20pt">
        <strong>Quarterly Financial Report</strong>
    </font>
    <br/>
    <font color="#666666" face="Arial" size="11pt">
        Q4 2024 - Generated on January 15, 2025
    </font>
    <br/><br/>
    <font color="#333333" face="Arial" size="12pt">
        This report contains confidential financial information.
    </font>
</div>
```

### Example 11: Product Description with Highlighting

```html
<p>
    <font face="Arial" size="12pt">
        The <font color="#336699" size="14pt"><strong>Premium Widget Pro</strong></font>
        is now available for just
        <font color="#d32f2f" size="16pt"><strong>$99.99</strong></font>!
        <br/><br/>
        Features include:
    </font>
</p>

<ul>
    <li><font face="Arial" size="11pt">Advanced functionality</font></li>
    <li><font face="Arial" size="11pt">5-year warranty</font></li>
    <li><font face="Arial" size="11pt">Free shipping</font></li>
</ul>

<p>
    <font color="#388e3c" face="Arial" size="11pt">
        <strong>Limited time offer!</strong> Order within 24 hours.
    </font>
</p>
```

### Example 12: Invoice with Color-Coded Amounts

```html
<table border="1" cellpadding="10" style="width: 100%;">
    <tr>
        <td><font face="Arial" size="12pt"><strong>Description</strong></font></td>
        <td><font face="Arial" size="12pt"><strong>Amount</strong></font></td>
    </tr>
    <tr>
        <td><font face="Arial" size="11pt">Web Development</font></td>
        <td><font face="Courier New" size="11pt" color="#333333">$5,000.00</font></td>
    </tr>
    <tr>
        <td><font face="Arial" size="11pt">Design Services</font></td>
        <td><font face="Courier New" size="11pt" color="#333333">$2,000.00</font></td>
    </tr>
    <tr>
        <td><font face="Arial" size="12pt"><strong>Total</strong></font></td>
        <td><font face="Courier New" size="14pt" color="#336699"><strong>$7,000.00</strong></font></td>
    </tr>
</table>
```

### Example 13: Multi-Language Support

```html
<!-- English -->
<font face="Arial" size="12pt" color="#333333">
    Welcome to our service
</font>

<!-- Spanish -->
<font face="Arial" size="12pt" color="#333333">
    Bienvenido a nuestro servicio
</font>

<!-- German -->
<font face="Arial" size="12pt" color="#333333">
    Willkommen bei unserem Service
</font>

<!-- Chinese (requires appropriate font) -->
<font face="SimSun" size="12pt" color="#333333">
    欢迎使用我们的服务
</font>
```

### Example 14: Certificate Text Styling

```html
<div style="text-align: center; padding: 40pt; border: 5pt double #336699;">
    <font face="Georgia" size="32pt" color="#2c3e50">
        <strong>Certificate of Achievement</strong>
    </font>
    <br/><br/>

    <font face="Georgia" size="14pt" color="#666666">
        This certifies that
    </font>
    <br/>

    <font face="Georgia" size="24pt" color="#336699">
        <strong>{{model.recipientName}}</strong>
    </font>
    <br/><br/>

    <font face="Georgia" size="12pt" color="#666666">
        has successfully completed the course
    </font>
    <br/>

    <font face="Georgia" size="16pt" color="#2c3e50">
        <strong>{{model.courseName}}</strong>
    </font>
    <br/><br/>

    <font face="Georgia" size="11pt" color="#999999">
        Date: {{model.completionDate}}
    </font>
</div>
```

### Example 15: Legacy to Modern Migration Example

```html
<!-- BEFORE: Legacy font tags -->
<div>
    <font color="#2c3e50" face="Arial" size="18pt">
        Section Title
    </font>
    <br/>
    <font color="#666666" face="Arial" size="11pt">
        Section subtitle or description
    </font>
    <br/><br/>
    <font face="Arial" size="12pt">
        Regular paragraph text with <font color="blue">highlighted</font> words
        and <font size="14pt">larger emphasized</font> text.
    </font>
</div>

<!-- AFTER: Modern CSS -->
<style>
    .section-title {
        color: #2c3e50;
        font-family: Arial, sans-serif;
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 5pt;
    }

    .section-subtitle {
        color: #666666;
        font-family: Arial, sans-serif;
        font-size: 11pt;
        margin-bottom: 15pt;
    }

    .body-text {
        font-family: Arial, sans-serif;
        font-size: 12pt;
        line-height: 1.6;
    }

    .highlight {
        color: blue;
    }

    .emphasize {
        font-size: 14pt;
        font-weight: bold;
    }
</style>

<div>
    <div class="section-title">Section Title</div>
    <div class="section-subtitle">Section subtitle or description</div>
    <p class="body-text">
        Regular paragraph text with <span class="highlight">highlighted</span> words
        and <span class="emphasize">larger emphasized</span> text.
    </p>
</div>
```

---

## See Also

- [font element](/reference/htmltags/font.html) - The deprecated font element
- [span element](/reference/htmltags/span.html) - Modern inline container (recommended)
- [style attribute](/reference/htmlattributes/style.html) - Inline CSS styling
- [class attribute](/reference/htmlattributes/class.html) - CSS class reference
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Color Values](/reference/styles/colors.html) - CSS color reference
- [Font Properties](/reference/styles/fonts.html) - CSS font styling
- [Text Formatting](/reference/htmltags/text-formatting.html) - Modern text formatting elements

---
