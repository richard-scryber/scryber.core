---
layout: default
title: hyphenate-character
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# hyphenate-character : Custom Hyphenation Character

The `hyphenate-character` property specifies the character to use at the end of a line when a word is hyphenated. This allows customization of the hyphen symbol for different languages, stylistic preferences, or special typographic requirements in PDF documents.

## Usage

```css
/* String value */
hyphenate-character: "-";
hyphenate-character: "‐";
hyphenate-character: "­";
hyphenate-character: "⁃";

/* Keyword value */
hyphenate-character: auto;
```

---

## Values

### String Values

- **"-"** - Standard hyphen-minus (U+002D) - default ASCII hyphen
- **"‐"** - Hyphen (U+2010) - dedicated hyphen character
- **"‑"** - Non-breaking hyphen (U+2011)
- **"­"** - Soft hyphen (U+00AD)
- **"⁃"** - Hyphen bullet (U+2043)
- Any single character string for custom hyphenation

### Keyword Value

- **auto** - Use default hyphen character (typically "-")

### Default Value

- **auto** - Standard hyphen character

---

## Notes

- Only applies when `hyphens: auto` or `hyphens: manual` is enabled
- The character appears at the end of the line where the word breaks
- Different languages may have different traditional hyphenation characters
- Unicode provides several hyphen-like characters with subtle visual differences
- The standard hyphen-minus ("-") works for most Western languages
- Some fonts may render hyphen characters differently
- The character should be visually appropriate for the font and context
- Very few use cases require changing from the default hyphen
- Custom characters are typically used for special typographic effects or non-Latin scripts

---

## Data Binding

The `hyphenate-character` property can be dynamically controlled through data binding, allowing hyphen symbols to vary based on language preferences, typographic standards, or document formatting requirements.

### Example 1: Language-Specific Hyphen Characters

```html
<div style="width: 200pt; padding: 15pt" lang="{{model.language.code}}">
    <p style="hyphens: auto; hyphenate-character: {{model.language.hyphenChar}}; text-align: justify; font-size: 11pt">
        {{model.content}}
    </p>
</div>

<!-- Data model for English (standard hyphen):
{
    "language": {
        "code": "en",
        "hyphenChar": "'-'"
    },
    "content": "Telecommunications infrastructure implementation..."
}
-->

<!-- Data model for formal typography (Unicode hyphen):
{
    "language": {
        "code": "en",
        "hyphenChar": "'‐'"
    },
    "content": "Telecommunications infrastructure implementation..."
}
-->
```

### Example 2: Document Format-Based Character

```html
<div style="width: 220pt; padding: 18pt" lang="en">
    <p style="hyphens: auto; hyphenate-character: {{model.format.hyphenChar}}; text-align: justify; font-size: 11pt">
        {{model.document.text}}
    </p>
</div>

<!-- Data model for standard document:
{
    "format": {
        "hyphenChar": "'-'"
    },
    "document": {
        "text": "Professional telecommunications systems..."
    }
}
-->

<!-- Data model for legal document (Unicode hyphen):
{
    "format": {
        "hyphenChar": "'‐'"
    },
    "document": {
        "text": "Professional telecommunications systems..."
    }
}
-->
```

### Example 3: Typography Style Settings

```html
<div style="width: 250pt; padding: 20pt; font-family: {{model.typography.fontFamily}}" lang="en">
    <p style="hyphens: auto; hyphenate-character: {{model.typography.hyphenChar}}; text-align: justify; font-size: 11pt">
        {{model.content}}
    </p>
</div>

<!-- Data model for print typography:
{
    "typography": {
        "fontFamily": "'Times New Roman', serif",
        "hyphenChar": "'‐'"
    },
    "content": "Contemporary methodological frameworks demonstrate..."
}
-->

<!-- Data model for digital typography:
{
    "typography": {
        "fontFamily": "Arial, sans-serif",
        "hyphenChar": "'-'"
    },
    "content": "Contemporary methodological frameworks demonstrate..."
}
-->
```

---

## Examples

### Example 1: Default Auto Character

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphenate-character: auto; text-align: justify; font-size: 11pt">
        Telecommunications implementation requires comprehensive
        documentation and systematic planning methodologies.
    </p>
</div>
```

### Example 2: Standard Hyphen-Minus

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphenate-character: '-'; text-align: justify; font-size: 11pt">
        Using the standard ASCII hyphen character for word breaks
        in telecommunications and infrastructure documentation.
    </p>
</div>
```

### Example 3: Unicode Hyphen Character

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphenate-character: '‐'; text-align: justify; font-size: 11pt">
        Using the dedicated Unicode hyphen character (U+2010) for
        professional typographic appearance in telecommunications.
    </p>
</div>
```

### Example 4: Custom Bullet Character

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphenate-character: '•'; text-align: justify; font-size: 11pt; font-family: 'Arial', sans-serif">
        Using a custom bullet character for artistic effect in
        telecommunications infrastructure implementation projects.
    </p>
</div>
```

### Example 5: Newsletter with Standard Hyphen

```html
<div style="width: 180pt; padding: 15pt; background-color: #f9fafb" lang="en">
    <h4 style="font-size: 12pt; color: #1e40af; margin-bottom: 10pt">
        Technology News
    </h4>
    <p style="hyphens: auto; hyphenate-character: '-'; text-align: justify; font-size: 10pt; line-height: 1.6">
        Revolutionary microprocessor architecture demonstrates
        unprecedented computational capabilities and efficiency
        improvements across enterprise platforms worldwide.
    </p>
</div>
```

### Example 6: Comparison of Hyphen Characters

```html
<table style="width: 100%; border-collapse: collapse">
    <thead>
        <tr style="background-color: #f0f0f0">
            <th style="padding: 8pt; border: 1pt solid #ccc; text-align: left">
                Character
            </th>
            <th style="padding: 8pt; border: 1pt solid #ccc; text-align: left">
                Result
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                Auto
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 180pt" lang="en">
                <p style="hyphens: auto; hyphenate-character: auto; text-align: justify; font-size: 9pt">
                    Telecommunications infrastructure implementation.
                </p>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                Hyphen-minus
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 180pt" lang="en">
                <p style="hyphens: auto; hyphenate-character: '-'; text-align: justify; font-size: 9pt">
                    Telecommunications infrastructure implementation.
                </p>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                Unicode hyphen
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 180pt" lang="en">
                <p style="hyphens: auto; hyphenate-character: '‐'; text-align: justify; font-size: 9pt">
                    Telecommunications infrastructure implementation.
                </p>
            </td>
        </tr>
    </tbody>
</table>
```

### Example 7: Technical Documentation

```html
<div style="width: 320pt; padding: 20pt" lang="en">
    <h3 style="font-size: 14pt; margin-bottom: 12pt">
        System Architecture
    </h3>
    <p style="hyphens: auto; hyphenate-character: '-'; text-align: justify; font-size: 10pt; line-height: 1.6">
        Telecommunications infrastructure prerequisites include adequate
        computational resources, operating system configurations, and
        environmental monitoring systems meeting specifications.
    </p>
</div>
```

### Example 8: Legal Document

```html
<div style="width: 400pt; padding: 25pt; font-size: 10pt; font-family: 'Times New Roman', serif" lang="en">
    <p style="hyphens: auto; hyphenate-character: '‐'; text-align: justify; line-height: 1.6">
        The parties acknowledge responsibilities regarding confidentiality
        obligations, indemnification procedures, liability limitations,
        and dispute resolution mechanisms comprehensively detailed herein.
    </p>
</div>
```

### Example 9: Product Brochure

```html
<div style="width: 250pt; padding: 20pt; background-color: #fef3c7; border: 2pt solid #f59e0b" lang="en">
    <h3 style="font-size: 15pt; color: #92400e; margin-bottom: 12pt">
        Professional Quality
    </h3>
    <p style="hyphens: auto; hyphenate-character: '-'; text-align: justify; font-size: 11pt; line-height: 1.6">
        Unparalleled performance through weatherproof construction,
        reinforced polycarbonate materials, state-of-the-art
        environmental protection, and comprehensive warranty coverage.
    </p>
</div>
```

### Example 10: Academic Paper

```html
<div style="width: 360pt; padding: 30pt; font-family: 'Times New Roman', serif" lang="en">
    <h4 style="font-size: 14pt; margin-bottom: 15pt; text-align: center">
        Methodology
    </h4>
    <p style="hyphens: auto; hyphenate-character: '‐'; text-align: justify; font-size: 11pt; line-height: 1.8">
        Contemporary methodological frameworks demonstrate significant
        phenomenological implications for interdisciplinary research
        paradigms utilizing quantitative and qualitative approaches.
    </p>
</div>
```

### Example 11: Magazine Article

```html
<div style="width: 200pt; padding: 20pt" lang="en">
    <h4 style="font-size: 13pt; color: #1e293b; margin-bottom: 10pt">
        Innovation Report
    </h4>
    <p style="hyphens: auto; hyphenate-character: '-'; text-align: justify; font-size: 10pt; line-height: 1.6">
        Groundbreaking technological advancements revolutionize
        telecommunications infrastructure through unprecedented
        computational methodologies and architectural frameworks.
    </p>
</div>
```

### Example 12: Pharmaceutical Information

```html
<div style="width: 280pt; padding: 18pt; border: 2pt solid #dc2626; background-color: #fef2f2" lang="en">
    <h4 style="font-size: 12pt; color: #991b1b; margin-bottom: 10pt">
        Prescribing Information
    </h4>
    <p style="hyphens: auto; hyphenate-character: '-'; text-align: justify; font-size: 9pt; line-height: 1.5">
        Contraindications include hypersensitivity reactions,
        anticoagulation therapy, hemophilia, thrombocytopenia,
        and gastroenterological hemorrhagic disorders requiring
        immediate medical intervention.
    </p>
</div>
```

### Example 13: Real Estate Listing

```html
<div style="width: 240pt; padding: 18pt; background-color: #f0fdf4; border-left: 4pt solid #16a34a" lang="en">
    <h4 style="font-size: 13pt; color: #166534; margin-bottom: 10pt">
        Luxury Estate
    </h4>
    <p style="hyphens: auto; hyphenate-character: '‐'; text-align: justify; font-size: 10pt; line-height: 1.6">
        Spectacular Mediterranean-style architectural masterpiece
        featuring professionally landscaped gardens, environmentally
        sustainable infrastructure, and breathtaking panoramic vistas.
    </p>
</div>
```

### Example 14: Scientific Abstract

```html
<div style="width: 340pt; padding: 25pt; font-family: 'Times New Roman', serif" lang="en">
    <h4 style="font-size: 13pt; margin-bottom: 12pt; text-align: center">
        Research Abstract
    </h4>
    <p style="hyphens: auto; hyphenate-character: '-'; text-align: justify; font-size: 10pt; line-height: 1.7">
        Spectrophotometric measurements demonstrate wavelength-dependent
        absorption characteristics utilizing ultraviolet-visible
        spectroscopy. High-performance chromatographic separation
        methodologies employed advanced fluorescence detection systems.
    </p>
</div>
```

### Example 15: Multi-Column Layout

```html
<table style="width: 100%">
    <tr>
        <td style="width: 48%; vertical-align: top; padding: 15pt; border: 1pt solid #e5e7eb" lang="en">
            <h4 style="font-size: 11pt; color: #1e40af; margin-bottom: 8pt">
                Standard Hyphen
            </h4>
            <p style="hyphens: auto; hyphenate-character: '-'; text-align: justify; font-size: 10pt; line-height: 1.5">
                Interdepartmental collaboration facilitates organizational
                knowledge transfer and promotes standardization of best
                practices throughout enterprise boundaries.
            </p>
        </td>
        <td style="width: 4%"></td>
        <td style="width: 48%; vertical-align: top; padding: 15pt; border: 1pt solid #e5e7eb" lang="en">
            <h4 style="font-size: 11pt; color: #16a34a; margin-bottom: 8pt">
                Unicode Hyphen
            </h4>
            <p style="hyphens: auto; hyphenate-character: '‐'; text-align: justify; font-size: 10pt; line-height: 1.5">
                Interdepartmental collaboration facilitates organizational
                knowledge transfer and promotes standardization of best
                practices throughout enterprise boundaries.
            </p>
        </td>
    </tr>
</table>
```

---

## See Also

- [hyphens](/reference/cssproperties/hyphens) - Enable/disable hyphenation
- [hyphenate-limit-chars](/reference/cssproperties/hyphenate-limit-chars) - Hyphenation character limits
- [hyphens-limit-chars-length](/reference/cssproperties/hyphens-limit-chars-length) - Minimum word length
- [hyphens-limit-chars-before](/reference/cssproperties/hyphens-limit-chars-before) - Minimum characters before hyphen
- [hyphens-limit-chars-after](/reference/cssproperties/hyphens-limit-chars-after) - Minimum characters after hyphen
- [text-align](/reference/cssproperties/text-align) - Text alignment
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
