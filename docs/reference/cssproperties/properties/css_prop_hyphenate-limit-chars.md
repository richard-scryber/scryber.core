---
layout: default
title: hyphenate-limit-chars
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# hyphenate-limit-chars : Hyphenation Character Limits

The `hyphenate-limit-chars` property specifies the minimum number of characters in a word before hyphenation, and the minimum number of characters before and after the hyphenation point. This is a shorthand property for fine-tuning automatic hyphenation behavior in PDF documents.

## Usage

```css
/* Three-value syntax: word-length min-before min-after */
hyphenate-limit-chars: 6 3 2;

/* Two-value syntax: word-length min-before (min-after = min-before) */
hyphenate-limit-chars: 6 3;

/* One-value syntax: word-length only */
hyphenate-limit-chars: 6;

/* Keyword value */
hyphenate-limit-chars: auto;
```

---

## Values

### Numeric Values

- **word-length** - Minimum number of characters a word must have before it can be hyphenated
- **min-before** - Minimum number of characters that must appear before the hyphen
- **min-after** - Minimum number of characters that must appear after the hyphen

### Keyword Value

- **auto** - Use default values (typically 5 2 2 or language-specific defaults)

### Default Value

- **auto** - Default hyphenation limits based on language

---

## Syntax Details

When using multiple values:
- **One value**: Sets only the minimum word length
- **Two values**: Sets word length and characters before hyphen (after defaults to same as before)
- **Three values**: Sets word length, characters before, and characters after

Common patterns:
- `hyphenate-limit-chars: 6 3 2` - Words must be 6+ chars, 3 before hyphen, 2 after
- `hyphenate-limit-chars: 5 2 2` - Default-like behavior
- `hyphenate-limit-chars: 8 4 3` - Conservative hyphenation

---

## Notes

- Only applies when `hyphens: auto` is set
- Prevents hyphenation of very short words
- Ensures readable word fragments on both sides of the hyphen
- Different languages may have different optimal values
- Higher values result in less frequent hyphenation
- Lower values allow more aggressive hyphenation
- Setting minimum word length too low can create awkward breaks
- Very short fragments (1-2 characters) generally reduce readability
- These limits work with the hyphenation dictionary rules

---

## Data Binding

The `hyphenate-limit-chars` property can be dynamically configured through data binding, allowing hyphenation behavior to adapt based on column widths, language requirements, or document formatting preferences.

### Example 1: Responsive Hyphenation Limits

```html
<div style="width: {{model.column.width}}; padding: 15pt" lang="en">
    <p style="hyphens: auto; hyphenate-limit-chars: {{model.column.hyphenateLimits}}; text-align: justify; font-size: 11pt">
        {{model.content}}
    </p>
</div>

<!-- Data model for narrow column (aggressive hyphenation):
{
    "column": {
        "width": "150pt",
        "hyphenateLimits": "5 2 2"
    },
    "content": "Telecommunications infrastructure implementation..."
}
-->

<!-- Data model for wide column (conservative hyphenation):
{
    "column": {
        "width": "350pt",
        "hyphenateLimits": "8 4 3"
    },
    "content": "Telecommunications infrastructure implementation..."
}
-->
```

### Example 2: Language-Specific Hyphenation Rules

```html
<div style="width: 200pt; padding: 15pt" lang="{{model.language.code}}">
    <p style="hyphens: auto; hyphenate-limit-chars: {{model.language.hyphenateLimits}}; text-align: justify; font-size: 11pt">
        {{model.content}}
    </p>
</div>

<!-- Data model for English:
{
    "language": {
        "code": "en",
        "hyphenateLimits": "6 3 2"
    },
    "content": "Professional telecommunications documentation..."
}
-->

<!-- Data model for German (longer compound words):
{
    "language": {
        "code": "de",
        "hyphenateLimits": "5 2 2"
    },
    "content": "Telekommunikationsinfrastruktur dokumentation..."
}
-->
```

### Example 3: Document Quality Settings

```html
<div style="width: 250pt; padding: 20pt" lang="en">
    <p style="hyphens: auto; hyphenate-limit-chars: {{model.quality.hyphenateLimits}}; text-align: justify; font-size: 11pt">
        {{model.document.content}}
    </p>
</div>

<!-- Data model for high quality (conservative):
{
    "quality": {
        "hyphenateLimits": "7 3 3"
    },
    "document": {
        "content": "Enterprise telecommunications infrastructure..."
    }
}
-->

<!-- Data model for compact (aggressive):
{
    "quality": {
        "hyphenateLimits": "5 2 2"
    },
    "document": {
        "content": "Enterprise telecommunications infrastructure..."
    }
}
-->
```

---

## Examples

### Example 1: Default Auto Limits

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphenate-limit-chars: auto; text-align: justify; font-size: 11pt">
        Telecommunications infrastructure implementation requires
        comprehensive documentation and systematic architectural planning.
    </p>
</div>
```

### Example 2: Conservative Hyphenation

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphenate-limit-chars: 8 4 3; text-align: justify; font-size: 11pt">
        Only longer words with sufficient characters before and after
        the hyphenation point will be hyphenated in this paragraph.
    </p>
</div>
```

### Example 3: Aggressive Hyphenation

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphenate-limit-chars: 5 2 2; text-align: justify; font-size: 11pt">
        More words will be hyphenated with these lower limits, allowing
        better text distribution in narrow columns.
    </p>
</div>
```

### Example 4: Narrow Column Newsletter

```html
<div style="width: 150pt; padding: 12pt; background-color: #f9fafb" lang="en">
    <h4 style="font-size: 12pt; color: #1e40af; margin-bottom: 8pt">
        Technology News
    </h4>
    <p style="hyphens: auto; hyphenate-limit-chars: 6 3 2; text-align: justify; font-size: 9pt; line-height: 1.5">
        Revolutionary developments in microprocessor architecture
        demonstrate unprecedented computational capabilities and
        efficiency improvements.
    </p>
</div>
```

### Example 5: Two-Column Layout with Different Settings

```html
<table style="width: 100%">
    <tr>
        <td style="width: 48%; vertical-align: top; padding: 15pt; border: 1pt solid #e5e7eb" lang="en">
            <h4 style="font-size: 11pt; margin-bottom: 8pt">
                Conservative (8 4 3)
            </h4>
            <p style="hyphens: auto; hyphenate-limit-chars: 8 4 3; text-align: justify; font-size: 10pt">
                Interdepartmental collaboration facilitates knowledge
                transfer and promotes standardization throughout
                organizational boundaries.
            </p>
        </td>
        <td style="width: 4%"></td>
        <td style="width: 48%; vertical-align: top; padding: 15pt; border: 1pt solid #e5e7eb" lang="en">
            <h4 style="font-size: 11pt; margin-bottom: 8pt">
                Aggressive (5 2 2)
            </h4>
            <p style="hyphens: auto; hyphenate-limit-chars: 5 2 2; text-align: justify; font-size: 10pt">
                Interdepartmental collaboration facilitates knowledge
                transfer and promotes standardization throughout
                organizational boundaries.
            </p>
        </td>
    </tr>
</table>
```

### Example 6: Technical Documentation

```html
<div style="width: 300pt; padding: 20pt" lang="en">
    <h3 style="font-size: 14pt; margin-bottom: 12pt">
        System Requirements
    </h3>
    <p style="hyphens: auto; hyphenate-limit-chars: 7 3 3; text-align: justify; font-size: 10pt; line-height: 1.6">
        Installation prerequisites include adequate telecommunications
        bandwidth, sufficient computational resources, and compatible
        operating system configurations meeting manufacturer specifications.
    </p>
</div>
```

### Example 7: Legal Document Format

```html
<div style="width: 400pt; padding: 25pt; font-size: 10pt" lang="en">
    <p style="hyphens: auto; hyphenate-limit-chars: 6 3 3; text-align: justify; line-height: 1.6">
        The aforementioned parties acknowledge their responsibilities
        regarding confidentiality obligations, indemnification procedures,
        and liability limitations as comprehensively enumerated throughout
        this contractual agreement.
    </p>
</div>
```

### Example 8: Product Brochure

```html
<div style="width: 250pt; padding: 20pt; background-color: #fef3c7; border: 2pt solid #f59e0b" lang="en">
    <h3 style="font-size: 16pt; color: #92400e; margin-bottom: 12pt">
        Professional Features
    </h3>
    <p style="hyphens: auto; hyphenate-limit-chars: 6 3 2; text-align: justify; font-size: 11pt; line-height: 1.6">
        Experience unparalleled performance with weatherproof construction,
        reinforced polycarbonate materials, and state-of-the-art
        environmental protection systems.
    </p>
</div>
```

### Example 9: Magazine Sidebar with Tight Spacing

```html
<div style="width: 160pt; padding: 15pt; background-color: #e0f2fe; border-left: 4pt solid #0284c7" lang="en">
    <h4 style="font-size: 11pt; color: #0c4a6e; margin-bottom: 8pt; text-transform: uppercase">
        Quick Fact
    </h4>
    <p style="hyphens: auto; hyphenate-limit-chars: 5 2 2; text-align: justify; font-size: 9pt; line-height: 1.5">
        Photosynthesis converts electromagnetic radiation into
        biochemical energy through complex molecular transformations
        within chloroplast membranes.
    </p>
</div>
```

### Example 10: Academic Paper Format

```html
<div style="width: 350pt; padding: 30pt; font-family: 'Times New Roman', serif" lang="en">
    <p style="hyphens: auto; hyphenate-limit-chars: 6 3 2; text-align: justify; font-size: 11pt; line-height: 1.8">
        Contemporary methodological frameworks demonstrate phenomenological
        implications for interdisciplinary research paradigms. Statistical
        analyses reveal significant correlations between organizational
        restructuring and productivity measurements.
    </p>
</div>
```

### Example 11: Minimum Word Length Only

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="en">
    <p style="hyphens: auto; hyphenate-limit-chars: 10; text-align: justify; font-size: 11pt">
        Only words with ten or more characters will be considered
        for hyphenation, regardless of the break position.
    </p>
</div>
```

### Example 12: Pharmaceutical Information

```html
<div style="width: 280pt; padding: 18pt; border: 2pt solid #dc2626; background-color: #fef2f2" lang="en">
    <h4 style="font-size: 12pt; color: #991b1b; margin-bottom: 10pt">
        Contraindications
    </h4>
    <p style="hyphens: auto; hyphenate-limit-chars: 7 3 3; text-align: justify; font-size: 9pt; line-height: 1.5">
        Hypersensitivity to acetylsalicylic acid, anticoagulant
        therapy, hemophilia, thrombocytopenia, and gastroenterological
        bleeding disorders constitute absolute contraindications.
    </p>
</div>
```

### Example 13: Real Estate Description

```html
<div style="width: 240pt; padding: 18pt; background-color: #f0fdf4; border-left: 4pt solid #16a34a" lang="en">
    <h4 style="font-size: 13pt; color: #166534; margin-bottom: 10pt">
        Luxury Property
    </h4>
    <p style="hyphens: auto; hyphenate-limit-chars: 6 3 2; text-align: justify; font-size: 10pt; line-height: 1.6">
        Spectacular Mediterranean-style residence featuring
        architecturally significant details, professionally landscaped
        grounds, and environmentally sustainable climate systems.
    </p>
</div>
```

### Example 14: Scientific Abstract

```html
<div style="width: 320pt; padding: 25pt; font-family: 'Times New Roman', serif" lang="en">
    <h4 style="font-size: 13pt; margin-bottom: 10pt; text-align: center">
        Abstract
    </h4>
    <p style="hyphens: auto; hyphenate-limit-chars: 6 3 3; text-align: justify; font-size: 10pt; line-height: 1.7">
        Spectrophotometric measurements obtained through ultraviolet-visible
        spectroscopy demonstrate wavelength-dependent absorption characteristics.
        Chromatographic separation employed high-performance liquid
        chromatography with fluorescence detection methodologies.
    </p>
</div>
```

### Example 15: Comparison Table

```html
<table style="width: 100%; border-collapse: collapse">
    <thead>
        <tr style="background-color: #f0f0f0">
            <th style="padding: 8pt; border: 1pt solid #ccc; text-align: left">
                Setting
            </th>
            <th style="padding: 8pt; border: 1pt solid #ccc; text-align: left">
                Example Text
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                10 4 4
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 200pt" lang="en">
                <p style="hyphens: auto; hyphenate-limit-chars: 10 4 4; text-align: justify; font-size: 9pt">
                    Telecommunications infrastructure implementation methodologies.
                </p>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                6 3 2
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 200pt" lang="en">
                <p style="hyphens: auto; hyphenate-limit-chars: 6 3 2; text-align: justify; font-size: 9pt">
                    Telecommunications infrastructure implementation methodologies.
                </p>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ccc; white-space: nowrap">
                5 2 2
            </td>
            <td style="padding: 8pt; border: 1pt solid #ccc; width: 200pt" lang="en">
                <p style="hyphens: auto; hyphenate-limit-chars: 5 2 2; text-align: justify; font-size: 9pt">
                    Telecommunications infrastructure implementation methodologies.
                </p>
            </td>
        </tr>
    </tbody>
</table>
```

---

## See Also

- [hyphens](/reference/cssproperties/hyphens) - Enable/disable hyphenation
- [hyphens-limit-chars-length](/reference/cssproperties/hyphens-limit-chars-length) - Minimum word length only
- [hyphens-limit-chars-before](/reference/cssproperties/hyphens-limit-chars-before) - Minimum characters before hyphen
- [hyphens-limit-chars-after](/reference/cssproperties/hyphens-limit-chars-after) - Minimum characters after hyphen
- [hyphenate-character](/reference/cssproperties/hyphenate-character) - Custom hyphen character
- [text-align](/reference/cssproperties/text-align) - Text alignment
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
