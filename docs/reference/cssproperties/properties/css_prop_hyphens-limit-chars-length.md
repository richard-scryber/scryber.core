---
layout: default
title: hyphens-limit-chars-length
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# hyphens-limit-chars-length : Minimum Word Length for Hyphenation

The `hyphens-limit-chars-length` property specifies the minimum number of characters a word must have before it can be hyphenated. This property provides granular control over which words are eligible for automatic hyphenation in PDF documents.

## Usage

```css
/* Numeric value */
hyphens-limit-chars-length: 6;
hyphens-limit-chars-length: 8;
hyphens-limit-chars-length: 10;

/* Keyword value */
hyphens-limit-chars-length: auto;
```

---

## Values

### Numeric Values

- **integer** - Minimum number of characters required in a word for hyphenation (e.g., `5`, `6`, `8`, `10`)

### Keyword Value

- **auto** - Use default minimum word length (typically 5 characters)

### Default Value

- **auto** - Default minimum word length based on language or implementation

---

## Notes

- Only applies when `hyphens: auto` is enabled
- Prevents hyphenation of short words that would create awkward breaks
- Higher values result in more conservative hyphenation (fewer hyphens)
- Lower values allow more aggressive hyphenation (more hyphens)
- Common values range from 5 to 10 characters
- Very short words (3-4 characters) rarely benefit from hyphenation
- This property is part of the hyphenation control suite
- Can be combined with `hyphens-limit-chars-before` and `hyphens-limit-chars-after`
- Language-specific defaults may vary

---

## Data Binding

The `hyphens-limit-chars-length` property supports data binding for dynamic control of minimum word length based on column width, language characteristics, or formatting preferences.

### Example 1: Column Width Adaptation

```html
<div style="width: {{model.column.width}}; padding: 15pt" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-length: {{model.column.minWordLength}}; text-align: justify; font-size: 11pt">
        {{model.content}}
    </p>
</div>

<!-- Data model for narrow column:
{
    "column": {
        "width": "150pt",
        "minWordLength": "5"
    },
    "content": "Telecommunications implementation requires documentation..."
}
-->

<!-- Data model for wide column:
{
    "column": {
        "width": "350pt",
        "minWordLength": "10"
    },
    "content": "Telecommunications implementation requires documentation..."
}
-->
```

### Example 2: Language-Specific Settings

```html
<div style="width: 200pt; padding: 15pt" lang="{{model.language.code}}">
    <p style="hyphens: auto; hyphens-limit-chars-length: {{model.language.minWordLength}}; text-align: justify; font-size: 11pt">
        {{model.content}}
    </p>
</div>

<!-- Data model for English:
{
    "language": {
        "code": "en",
        "minWordLength": "6"
    },
    "content": "Professional telecommunications systems..."
}
-->

<!-- Data model for German (accepts shorter words due to compounds):
{
    "language": {
        "code": "de",
        "minWordLength": "5"
    },
    "content": "Professionelle Telekommunikationssysteme..."
}
-->
```

### Example 3: Document Format Preferences

```html
<div style="width: 220pt; padding: 18pt" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-length: {{model.format.minWordLength}}; text-align: justify; font-size: 11pt">
        {{model.document.text}}
    </p>
</div>

<!-- Data model for professional format (conservative):
{
    "format": {
        "minWordLength": "8"
    },
    "document": {
        "text": "Enterprise telecommunications infrastructure..."
    }
}
-->

<!-- Data model for compact format (aggressive):
{
    "format": {
        "minWordLength": "5"
    },
    "document": {
        "text": "Enterprise telecommunications infrastructure..."
    }
}
-->
```

---

## Examples

### Example 1: Default Auto Length

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-length: auto; text-align: justify; font-size: 11pt">
        Telecommunications implementation requires comprehensive
        documentation and systematic planning methodologies.
    </p>
</div>
```

### Example 2: Conservative Setting (10 characters)

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-length: 10; text-align: justify; font-size: 11pt">
        Only very long words like telecommunications and implementation
        will be hyphenated with this conservative setting.
    </p>
</div>
```

### Example 3: Standard Setting (6 characters)

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-length: 6; text-align: justify; font-size: 11pt">
        Words with six or more characters can be hyphenated, providing
        better text distribution in narrow columns.
    </p>
</div>
```

### Example 4: Aggressive Setting (5 characters)

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-length: 5; text-align: justify; font-size: 11pt">
        More words become eligible for hyphenation with this lower
        threshold, improving justification quality.
    </p>
</div>
```

### Example 5: Newsletter Column

```html
<div style="width: 180pt; padding: 15pt; background-color: #f9fafb" lang="en">
    <h4 style="font-size: 12pt; color: #1e40af; margin-bottom: 10pt">
        Technology Update
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-length: 7; text-align: justify; font-size: 10pt; line-height: 1.6">
        Revolutionary microprocessor architecture demonstrates
        unprecedented computational capabilities and efficiency
        improvements across multiple platforms.
    </p>
</div>
```

### Example 6: Comparison Table

```html
<table style="width: 100%; border-collapse: collapse">
    <thead>
        <tr style="background-color: #f0f0f0">
            <th style="padding: 8pt; border: 1pt solid #ccc; text-align: left">
                Min Length
            </th>
            <th style="padding: 8pt; border: 1pt solid #ccc; text-align: left">
                Result
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                10 chars
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 200pt" lang="en">
                <p style="hyphens: auto; hyphens-limit-chars-length: 10; text-align: justify; font-size: 9pt">
                    Telecommunications infrastructure implementation.
                </p>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                6 chars
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 200pt" lang="en">
                <p style="hyphens: auto; hyphens-limit-chars-length: 6; text-align: justify; font-size: 9pt">
                    Telecommunications infrastructure implementation.
                </p>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                5 chars
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 200pt" lang="en">
                <p style="hyphens: auto; hyphens-limit-chars-length: 5; text-align: justify; font-size: 9pt">
                    Telecommunications infrastructure implementation.
                </p>
            </td>
        </tr>
    </tbody>
</table>
```

### Example 7: Technical Manual

```html
<div style="width: 320pt; padding: 20pt" lang="en">
    <h3 style="font-size: 14pt; margin-bottom: 12pt">
        Installation Prerequisites
    </h3>
    <p style="hyphens: auto; hyphens-limit-chars-length: 8; text-align: justify; font-size: 10pt; line-height: 1.6">
        Verify telecommunications bandwidth, computational resources,
        and operating system configurations meet manufacturer
        specifications before commencing installation procedures.
    </p>
</div>
```

### Example 8: Legal Document

```html
<div style="width: 400pt; padding: 25pt; font-size: 10pt" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-length: 7; text-align: justify; line-height: 1.6">
        The aforementioned parties acknowledge responsibilities regarding
        confidentiality obligations, indemnification procedures, and
        liability limitations comprehensively enumerated throughout
        this contractual agreement.
    </p>
</div>
```

### Example 9: Product Brochure

```html
<div style="width: 240pt; padding: 20pt; background-color: #fef3c7; border: 2pt solid #f59e0b" lang="en">
    <h3 style="font-size: 15pt; color: #92400e; margin-bottom: 12pt">
        Premium Features
    </h3>
    <p style="hyphens: auto; hyphens-limit-chars-length: 6; text-align: justify; font-size: 11pt; line-height: 1.6">
        Experience unparalleled performance with weatherproof
        construction, reinforced polycarbonate materials, and
        state-of-the-art environmental protection systems.
    </p>
</div>
```

### Example 10: Academic Paper

```html
<div style="width: 350pt; padding: 30pt; font-family: 'Times New Roman', serif" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-length: 6; text-align: justify; font-size: 11pt; line-height: 1.8">
        Contemporary methodological frameworks demonstrate
        phenomenological implications for interdisciplinary research
        paradigms. Statistical analyses reveal significant correlations.
    </p>
</div>
```

### Example 11: Magazine Sidebar

```html
<div style="width: 160pt; padding: 15pt; background-color: #e0f2fe; border-left: 4pt solid #0284c7" lang="en">
    <h4 style="font-size: 11pt; color: #0c4a6e; margin-bottom: 8pt; text-transform: uppercase">
        Did You Know?
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-length: 5; text-align: justify; font-size: 9pt; line-height: 1.5">
        Photosynthesis converts electromagnetic radiation into
        biochemical energy through molecular transformations.
    </p>
</div>
```

### Example 12: Pharmaceutical Label

```html
<div style="width: 280pt; padding: 18pt; border: 2pt solid #dc2626; background-color: #fef2f2" lang="en">
    <h4 style="font-size: 12pt; color: #991b1b; margin-bottom: 10pt">
        Important Information
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-length: 8; text-align: justify; font-size: 9pt; line-height: 1.5">
        Contraindications include hypersensitivity to acetylsalicylic
        acid, anticoagulant therapy, hemophilia, thrombocytopenia,
        and gastroenterological bleeding disorders.
    </p>
</div>
```

### Example 13: Real Estate Description

```html
<div style="width: 230pt; padding: 18pt; background-color: #f0fdf4; border-left: 4pt solid #16a34a" lang="en">
    <h4 style="font-size: 13pt; color: #166534; margin-bottom: 10pt">
        Property Details
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-length: 6; text-align: justify; font-size: 10pt; line-height: 1.6">
        Spectacular Mediterranean-style residence featuring
        architecturally significant details and professionally
        landscaped grounds with environmentally sustainable systems.
    </p>
</div>
```

### Example 14: Scientific Abstract

```html
<div style="width: 320pt; padding: 25pt; font-family: 'Times New Roman', serif" lang="en">
    <h4 style="font-size: 13pt; margin-bottom: 10pt; text-align: center">
        Abstract
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-length: 7; text-align: justify; font-size: 10pt; line-height: 1.7">
        Spectrophotometric measurements demonstrate wavelength-dependent
        absorption characteristics. Chromatographic separation employed
        high-performance liquid chromatography with fluorescence detection.
    </p>
</div>
```

### Example 15: Two-Column Layout

```html
<table style="width: 100%">
    <tr>
        <td style="width: 48%; vertical-align: top; padding: 15pt; border: 1pt solid #e5e7eb" lang="en">
            <h4 style="font-size: 11pt; color: #dc2626; margin-bottom: 8pt">
                Conservative (10)
            </h4>
            <p style="hyphens: auto; hyphens-limit-chars-length: 10; text-align: justify; font-size: 10pt; line-height: 1.5">
                Interdepartmental collaboration facilitates knowledge
                transfer and promotes standardization of best practices
                throughout organizational boundaries.
            </p>
        </td>
        <td style="width: 4%"></td>
        <td style="width: 48%; vertical-align: top; padding: 15pt; border: 1pt solid #e5e7eb" lang="en">
            <h4 style="font-size: 11pt; color: #16a34a; margin-bottom: 8pt">
                Standard (6)
            </h4>
            <p style="hyphens: auto; hyphens-limit-chars-length: 6; text-align: justify; font-size: 10pt; line-height: 1.5">
                Interdepartmental collaboration facilitates knowledge
                transfer and promotes standardization of best practices
                throughout organizational boundaries.
            </p>
        </td>
    </tr>
</table>
```

---

## See Also

- [hyphens](/reference/cssproperties/hyphens) - Enable/disable hyphenation
- [hyphenate-limit-chars](/reference/cssproperties/hyphenate-limit-chars) - Shorthand for all hyphenation limits
- [hyphens-limit-chars-before](/reference/cssproperties/hyphens-limit-chars-before) - Minimum characters before hyphen
- [hyphens-limit-chars-after](/reference/cssproperties/hyphens-limit-chars-after) - Minimum characters after hyphen
- [hyphenate-character](/reference/cssproperties/hyphenate-character) - Custom hyphen character
- [text-align](/reference/cssproperties/text-align) - Text alignment
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
