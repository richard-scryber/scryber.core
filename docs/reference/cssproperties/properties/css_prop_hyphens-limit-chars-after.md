---
layout: default
title: hyphens-limit-chars-after
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# hyphens-limit-chars-after : Minimum Characters After Hyphen

The `hyphens-limit-chars-after` property specifies the minimum number of characters that must appear after the hyphenation point in a word. This ensures readable word fragments following the hyphen in PDF documents with automatic hyphenation.

## Usage

```css
/* Numeric value */
hyphens-limit-chars-after: 2;
hyphens-limit-chars-after: 3;
hyphens-limit-chars-after: 4;

/* Keyword value */
hyphens-limit-chars-after: auto;
```

---

## Values

### Numeric Values

- **integer** - Minimum number of characters required after the hyphenation point (e.g., `2`, `3`, `4`)

### Keyword Value

- **auto** - Use default minimum (typically 2 or 3 characters depending on language)

### Default Value

- **auto** - Default minimum based on language or implementation

---

## Notes

- Only applies when `hyphens: auto` is enabled
- Prevents creating very short, unreadable fragments after the hyphen
- Common values are 2, 3, or 4 characters
- Value of 2 is standard for most languages
- Value of 3 provides more conservative hyphenation
- Value of 4 ensures substantial trailing fragments
- Higher values result in fewer hyphenation opportunities
- Should be balanced with `hyphens-limit-chars-before` for best results
- Very low values (1) create poor readability and orphaned characters
- The last fragment should be substantial enough to be meaningful

---

## Data Binding

The `hyphens-limit-chars-after` property supports data binding for dynamic control of trailing fragment length, enabling adaptive hyphenation based on layout, language, or quality requirements.

### Example 1: Responsive Column Hyphenation

```html
<div style="width: {{model.column.width}}; padding: 15pt" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-after: {{model.column.charsAfterHyphen}}; text-align: justify; font-size: 11pt">
        {{model.content}}
    </p>
</div>

<!-- Data model for narrow column (more flexible):
{
    "column": {
        "width": "150pt",
        "charsAfterHyphen": "2"
    },
    "content": "Implementation methodologies telecommunications..."
}
-->

<!-- Data model for wide column (more conservative):
{
    "column": {
        "width": "300pt",
        "charsAfterHyphen": "4"
    },
    "content": "Implementation methodologies telecommunications..."
}
-->
```

### Example 2: Language-Based Configuration

```html
<div style="width: 200pt; padding: 15pt" lang="{{model.language.code}}">
    <p style="hyphens: auto; hyphens-limit-chars-after: {{model.language.charsAfter}}; text-align: justify; font-size: 11pt">
        {{model.content}}
    </p>
</div>

<!-- Data model for English:
{
    "language": {
        "code": "en",
        "charsAfter": "2"
    },
    "content": "Telecommunications infrastructure implementation..."
}
-->

<!-- Data model for Spanish:
{
    "language": {
        "code": "es",
        "charsAfter": "3"
    },
    "content": "Implementación de infraestructura de telecomunicaciones..."
}
-->
```

### Example 3: Balanced Before and After Settings

```html
<div style="width: 220pt; padding: 18pt" lang="en">
    <p style="hyphens: auto;
             hyphens-limit-chars-before: {{model.hyphenation.charsBefore}};
             hyphens-limit-chars-after: {{model.hyphenation.charsAfter}};
             text-align: justify; font-size: 11pt">
        {{model.document.text}}
    </p>
</div>

<!-- Data model for balanced hyphenation:
{
    "hyphenation": {
        "charsBefore": "3",
        "charsAfter": "2"
    },
    "document": {
        "text": "Interdepartmental collaboration facilitates knowledge transfer..."
    }
}
-->

<!-- Data model for symmetrical hyphenation:
{
    "hyphenation": {
        "charsBefore": "3",
        "charsAfter": "3"
    },
    "document": {
        "text": "Interdepartmental collaboration facilitates knowledge transfer..."
    }
}
-->
```

---

## Examples

### Example 1: Default Auto Setting

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-after: auto; text-align: justify; font-size: 11pt">
        Telecommunications implementation requires comprehensive
        documentation and systematic planning methodologies.
    </p>
</div>
```

### Example 2: Conservative Setting (4 characters)

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-after: 4; text-align: justify; font-size: 11pt">
        Words will only break when at least four characters remain
        after the hyphen, ensuring substantial trailing fragments.
    </p>
</div>
```

### Example 3: Standard Setting (3 characters)

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-after: 3; text-align: justify; font-size: 11pt">
        Standard hyphenation requires three characters after the
        break, providing good readability for most content types.
    </p>
</div>
```

### Example 4: Minimum Setting (2 characters)

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-after: 2; text-align: justify; font-size: 11pt">
        More hyphenation opportunities with only two characters
        required after the break, useful for narrow layouts.
    </p>
</div>
```

### Example 5: Newsletter Column

```html
<div style="width: 170pt; padding: 15pt; background-color: #f9fafb" lang="en">
    <h4 style="font-size: 12pt; color: #1e40af; margin-bottom: 10pt">
        Industry Updates
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-after: 2; text-align: justify; font-size: 10pt; line-height: 1.6">
        Revolutionary microprocessor architecture demonstrates
        unprecedented computational capabilities and efficiency
        improvements across enterprise platforms.
    </p>
</div>
```

### Example 6: Comparison of Settings

```html
<table style="width: 100%; border-collapse: collapse">
    <thead>
        <tr style="background-color: #f0f0f0">
            <th style="padding: 8pt; border: 1pt solid #ccc; text-align: left">
                Min After
            </th>
            <th style="padding: 8pt; border: 1pt solid #ccc; text-align: left">
                Result
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                4 chars
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 180pt" lang="en">
                <p style="hyphens: auto; hyphens-limit-chars-after: 4; text-align: justify; font-size: 9pt">
                    Implementation methodologies telecommunications.
                </p>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                3 chars
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 180pt" lang="en">
                <p style="hyphens: auto; hyphens-limit-chars-after: 3; text-align: justify; font-size: 9pt">
                    Implementation methodologies telecommunications.
                </p>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                2 chars
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 180pt" lang="en">
                <p style="hyphens: auto; hyphens-limit-chars-after: 2; text-align: justify; font-size: 9pt">
                    Implementation methodologies telecommunications.
                </p>
            </td>
        </tr>
    </tbody>
</table>
```

### Example 7: Technical Manual

```html
<div style="width: 300pt; padding: 20pt" lang="en">
    <h3 style="font-size: 14pt; margin-bottom: 12pt">
        Installation Requirements
    </h3>
    <p style="hyphens: auto; hyphens-limit-chars-after: 3; text-align: justify; font-size: 10pt; line-height: 1.6">
        Prerequisites include telecommunications infrastructure,
        computational resources, operating system configurations,
        and environmental monitoring systems meeting specifications.
    </p>
</div>
```

### Example 8: Legal Document

```html
<div style="width: 400pt; padding: 25pt; font-size: 10pt" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-after: 3; text-align: justify; line-height: 1.6">
        The parties acknowledge responsibilities regarding confidentiality
        obligations, indemnification procedures, liability limitations,
        and dispute resolution mechanisms comprehensively detailed herein.
    </p>
</div>
```

### Example 9: Product Brochure

```html
<div style="width: 240pt; padding: 20pt; background-color: #fef3c7; border: 2pt solid #f59e0b" lang="en">
    <h3 style="font-size: 15pt; color: #92400e; margin-bottom: 12pt">
        Premium Quality
    </h3>
    <p style="hyphens: auto; hyphens-limit-chars-after: 2; text-align: justify; font-size: 11pt; line-height: 1.6">
        Unparalleled performance through weatherproof construction,
        reinforced polycarbonate materials, state-of-the-art
        environmental protection, and comprehensive warranty coverage.
    </p>
</div>
```

### Example 10: Academic Paper

```html
<div style="width: 350pt; padding: 30pt; font-family: 'Times New Roman', serif" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-after: 3; text-align: justify; font-size: 11pt; line-height: 1.8">
        Contemporary methodological frameworks demonstrate significant
        phenomenological implications for interdisciplinary research
        paradigms in postmodern academic discourse.
    </p>
</div>
```

### Example 11: Magazine Sidebar

```html
<div style="width: 160pt; padding: 15pt; background-color: #e0f2fe; border-left: 4pt solid #0284c7" lang="en">
    <h4 style="font-size: 11pt; color: #0c4a6e; margin-bottom: 8pt; text-transform: uppercase">
        Fun Fact
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-after: 2; text-align: justify; font-size: 9pt; line-height: 1.5">
        Photosynthesis transforms electromagnetic radiation into
        biochemical energy utilizing chlorophyll-based molecular
        mechanisms within specialized cellular structures.
    </p>
</div>
```

### Example 12: Pharmaceutical Label

```html
<div style="width: 280pt; padding: 18pt; border: 2pt solid #dc2626; background-color: #fef2f2" lang="en">
    <h4 style="font-size: 12pt; color: #991b1b; margin-bottom: 10pt">
        Warning Information
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-after: 3; text-align: justify; font-size: 9pt; line-height: 1.5">
        Hypersensitivity reactions, anticoagulation therapy,
        hemophilia, thrombocytopenia, and gastroenterological
        hemorrhagic disorders constitute absolute contraindications.
    </p>
</div>
```

### Example 13: Real Estate Listing

```html
<div style="width: 230pt; padding: 18pt; background-color: #f0fdf4; border-left: 4pt solid #16a34a" lang="en">
    <h4 style="font-size: 13pt; color: #166534; margin-bottom: 10pt">
        Executive Property
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-after: 2; text-align: justify; font-size: 10pt; line-height: 1.6">
        Spectacular Mediterranean-style architectural masterpiece
        featuring professionally landscaped gardens, environmentally
        sustainable infrastructure, and panoramic mountain vistas.
    </p>
</div>
```

### Example 14: Scientific Abstract

```html
<div style="width: 320pt; padding: 25pt; font-family: 'Times New Roman', serif" lang="en">
    <h4 style="font-size: 13pt; margin-bottom: 10pt; text-align: center">
        Research Abstract
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-after: 3; text-align: justify; font-size: 10pt; line-height: 1.7">
        Spectrophotometric measurements demonstrate wavelength-dependent
        absorption characteristics. High-performance chromatographic
        separation methodologies employed fluorescence detection systems.
    </p>
</div>
```

### Example 15: Balanced Before and After Settings

```html
<table style="width: 100%">
    <tr>
        <td style="width: 48%; vertical-align: top; padding: 15pt; border: 1pt solid #e5e7eb" lang="en">
            <h4 style="font-size: 11pt; color: #1e40af; margin-bottom: 8pt">
                Balanced (3 before, 2 after)
            </h4>
            <p style="hyphens: auto; hyphens-limit-chars-before: 3; hyphens-limit-chars-after: 2; text-align: justify; font-size: 10pt; line-height: 1.5">
                Interdepartmental collaboration facilitates knowledge
                transfer and promotes standardization of best practices
                throughout organizational boundaries and divisions.
            </p>
        </td>
        <td style="width: 4%"></td>
        <td style="width: 48%; vertical-align: top; padding: 15pt; border: 1pt solid #e5e7eb" lang="en">
            <h4 style="font-size: 11pt; color: #16a34a; margin-bottom: 8pt">
                Symmetrical (3 before, 3 after)
            </h4>
            <p style="hyphens: auto; hyphens-limit-chars-before: 3; hyphens-limit-chars-after: 3; text-align: justify; font-size: 10pt; line-height: 1.5">
                Interdepartmental collaboration facilitates knowledge
                transfer and promotes standardization of best practices
                throughout organizational boundaries and divisions.
            </p>
        </td>
    </tr>
</table>
```

---

## See Also

- [hyphens](/reference/cssproperties/hyphens) - Enable/disable hyphenation
- [hyphenate-limit-chars](/reference/cssproperties/hyphenate-limit-chars) - Shorthand for all hyphenation limits
- [hyphens-limit-chars-length](/reference/cssproperties/hyphens-limit-chars-length) - Minimum word length
- [hyphens-limit-chars-before](/reference/cssproperties/hyphens-limit-chars-before) - Minimum characters before hyphen
- [hyphenate-character](/reference/cssproperties/hyphenate-character) - Custom hyphen character
- [text-align](/reference/cssproperties/text-align) - Text alignment
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
