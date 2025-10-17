---
layout: default
title: hyphens-limit-chars-before
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# hyphens-limit-chars-before : Minimum Characters Before Hyphen

The `hyphens-limit-chars-before` property specifies the minimum number of characters that must appear before the hyphenation point in a word. This ensures readable word fragments before the hyphen in PDF documents with automatic hyphenation.

## Usage

```css
/* Numeric value */
hyphens-limit-chars-before: 2;
hyphens-limit-chars-before: 3;
hyphens-limit-chars-before: 4;

/* Keyword value */
hyphens-limit-chars-before: auto;
```

---

## Values

### Numeric Values

- **integer** - Minimum number of characters required before the hyphenation point (e.g., `2`, `3`, `4`)

### Keyword Value

- **auto** - Use default minimum (typically 2 or 3 characters depending on language)

### Default Value

- **auto** - Default minimum based on language or implementation

---

## Notes

- Only applies when `hyphens: auto` is enabled
- Prevents creating very short, unreadable fragments before the hyphen
- Common values are 2, 3, or 4 characters
- Value of 2 is aggressive, allowing breaks like "a-bout"
- Value of 3 is standard, preventing very short initial fragments
- Value of 4 is conservative, ensuring substantial initial fragments
- Higher values result in fewer hyphenation opportunities
- Should be balanced with `hyphens-limit-chars-after` for best results
- Very low values (1) create poor readability
- Language-specific rules may influence optimal values

---

## Data Binding

The `hyphens-limit-chars-before` property can be dynamically controlled through data binding, enabling adaptive hyphenation based on column layout, language requirements, or formatting standards.

### Example 1: Adaptive Column Formatting

```html
<div style="width: {{model.column.width}}; padding: 15pt" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-before: {{model.column.charsBeforeHyphen}}; text-align: justify; font-size: 11pt">
        {{model.content}}
    </p>
</div>

<!-- Data model for narrow column (aggressive):
{
    "column": {
        "width": "150pt",
        "charsBeforeHyphen": "2"
    },
    "content": "Implementation methodologies telecommunications..."
}
-->

<!-- Data model for standard column (balanced):
{
    "column": {
        "width": "250pt",
        "charsBeforeHyphen": "3"
    },
    "content": "Implementation methodologies telecommunications..."
}
-->
```

### Example 2: Language-Specific Hyphenation

```html
<div style="width: 200pt; padding: 15pt" lang="{{model.language.code}}">
    <p style="hyphens: auto; hyphens-limit-chars-before: {{model.language.charsBefore}}; text-align: justify; font-size: 11pt">
        {{model.content}}
    </p>
</div>

<!-- Data model for English:
{
    "language": {
        "code": "en",
        "charsBefore": "3"
    },
    "content": "Telecommunications infrastructure systems..."
}
-->

<!-- Data model for French (may allow shorter fragments):
{
    "language": {
        "code": "fr",
        "charsBefore": "2"
    },
    "content": "Systèmes de télécommunications..."
}
-->
```

### Example 3: Quality-Based Settings

```html
<div style="width: 220pt; padding: 18pt" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-before: {{model.quality.charsBefore}}; text-align: justify; font-size: 11pt">
        {{model.document.text}}
    </p>
</div>

<!-- Data model for high quality:
{
    "quality": {
        "charsBefore": "4"
    },
    "document": {
        "text": "Interdepartmental collaboration methodologies..."
    }
}
-->

<!-- Data model for compact:
{
    "quality": {
        "charsBefore": "2"
    },
    "document": {
        "text": "Interdepartmental collaboration methodologies..."
    }
}
-->
```

---

## Examples

### Example 1: Default Auto Setting

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-before: auto; text-align: justify; font-size: 11pt">
        Telecommunications implementation requires comprehensive
        documentation and systematic planning methodologies.
    </p>
</div>
```

### Example 2: Conservative Setting (4 characters)

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-before: 4; text-align: justify; font-size: 11pt">
        Words will only break after at least four characters,
        ensuring substantial word fragments before the hyphen.
    </p>
</div>
```

### Example 3: Standard Setting (3 characters)

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-before: 3; text-align: justify; font-size: 11pt">
        Standard hyphenation requires three characters before the
        break, providing good readability in most situations.
    </p>
</div>
```

### Example 4: Aggressive Setting (2 characters)

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-before: 2; text-align: justify; font-size: 11pt">
        More hyphenation opportunities with only two characters
        required before the break, useful for very narrow columns.
    </p>
</div>
```

### Example 5: Newsletter Column

```html
<div style="width: 170pt; padding: 15pt; background-color: #f9fafb" lang="en">
    <h4 style="font-size: 12pt; color: #1e40af; margin-bottom: 10pt">
        Technology Insights
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-before: 3; text-align: justify; font-size: 10pt; line-height: 1.6">
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
                Min Before
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
                <p style="hyphens: auto; hyphens-limit-chars-before: 4; text-align: justify; font-size: 9pt">
                    Implementation methodologies telecommunications.
                </p>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                3 chars
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 180pt" lang="en">
                <p style="hyphens: auto; hyphens-limit-chars-before: 3; text-align: justify; font-size: 9pt">
                    Implementation methodologies telecommunications.
                </p>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                2 chars
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 180pt" lang="en">
                <p style="hyphens: auto; hyphens-limit-chars-before: 2; text-align: justify; font-size: 9pt">
                    Implementation methodologies telecommunications.
                </p>
            </td>
        </tr>
    </tbody>
</table>
```

### Example 7: Technical Documentation

```html
<div style="width: 300pt; padding: 20pt" lang="en">
    <h3 style="font-size: 14pt; margin-bottom: 12pt">
        System Configuration
    </h3>
    <p style="hyphens: auto; hyphens-limit-chars-before: 3; text-align: justify; font-size: 10pt; line-height: 1.6">
        Prerequisites include telecommunications infrastructure,
        computational resources, operating system configurations,
        and environmental monitoring systems meeting specifications.
    </p>
</div>
```

### Example 8: Legal Document

```html
<div style="width: 400pt; padding: 25pt; font-size: 10pt" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-before: 3; text-align: justify; line-height: 1.6">
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
        Advanced Features
    </h3>
    <p style="hyphens: auto; hyphens-limit-chars-before: 3; text-align: justify; font-size: 11pt; line-height: 1.6">
        Unparalleled performance through weatherproof construction,
        reinforced polycarbonate materials, state-of-the-art
        environmental protection, and comprehensive warranty coverage.
    </p>
</div>
```

### Example 10: Academic Paper

```html
<div style="width: 350pt; padding: 30pt; font-family: 'Times New Roman', serif" lang="en">
    <p style="hyphens: auto; hyphens-limit-chars-before: 3; text-align: justify; font-size: 11pt; line-height: 1.8">
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
        Science Fact
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-before: 2; text-align: justify; font-size: 9pt; line-height: 1.5">
        Photosynthesis transforms electromagnetic radiation into
        biochemical energy utilizing chlorophyll-based molecular
        mechanisms within specialized cellular structures.
    </p>
</div>
```

### Example 12: Pharmaceutical Information

```html
<div style="width: 280pt; padding: 18pt; border: 2pt solid #dc2626; background-color: #fef2f2" lang="en">
    <h4 style="font-size: 12pt; color: #991b1b; margin-bottom: 10pt">
        Contraindications
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-before: 3; text-align: justify; font-size: 9pt; line-height: 1.5">
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
        Luxury Residence
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-before: 3; text-align: justify; font-size: 10pt; line-height: 1.6">
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
        Abstract
    </h4>
    <p style="hyphens: auto; hyphens-limit-chars-before: 3; text-align: justify; font-size: 10pt; line-height: 1.7">
        Spectrophotometric measurements demonstrate wavelength-dependent
        absorption characteristics. High-performance chromatographic
        separation methodologies employed fluorescence detection systems.
    </p>
</div>
```

### Example 15: Multi-Column Comparison

```html
<table style="width: 100%">
    <tr>
        <td style="width: 31%; vertical-align: top; padding: 12pt; border: 1pt solid #e5e7eb" lang="en">
            <h5 style="font-size: 10pt; color: #dc2626; margin-bottom: 6pt">
                Before: 4
            </h5>
            <p style="hyphens: auto; hyphens-limit-chars-before: 4; text-align: justify; font-size: 9pt; line-height: 1.4">
                Interdepartmental collaboration facilitates
                organizational knowledge transfer mechanisms.
            </p>
        </td>
        <td style="width: 2%"></td>
        <td style="width: 31%; vertical-align: top; padding: 12pt; border: 1pt solid #e5e7eb" lang="en">
            <h5 style="font-size: 10pt; color: #f59e0b; margin-bottom: 6pt">
                Before: 3
            </h5>
            <p style="hyphens: auto; hyphens-limit-chars-before: 3; text-align: justify; font-size: 9pt; line-height: 1.4">
                Interdepartmental collaboration facilitates
                organizational knowledge transfer mechanisms.
            </p>
        </td>
        <td style="width: 2%"></td>
        <td style="width: 31%; vertical-align: top; padding: 12pt; border: 1pt solid #e5e7eb" lang="en">
            <h5 style="font-size: 10pt; color: #16a34a; margin-bottom: 6pt">
                Before: 2
            </h5>
            <p style="hyphens: auto; hyphens-limit-chars-before: 2; text-align: justify; font-size: 9pt; line-height: 1.4">
                Interdepartmental collaboration facilitates
                organizational knowledge transfer mechanisms.
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
- [hyphens-limit-chars-after](/reference/cssproperties/hyphens-limit-chars-after) - Minimum characters after hyphen
- [hyphenate-character](/reference/cssproperties/hyphenate-character) - Custom hyphen character
- [text-align](/reference/cssproperties/text-align) - Text alignment
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
