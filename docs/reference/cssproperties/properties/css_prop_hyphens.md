---
layout: default
title: hyphens
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# hyphens : Hyphenation Property

The `hyphens` property controls whether and how words should be hyphenated when text wraps across multiple lines. This is particularly important for justified text, narrow columns, and languages with long compound words in PDF documents.

## Usage

```css
/* Keyword values */
hyphens: none;
hyphens: manual;
hyphens: auto;
```

---

## Values

### Hyphenation Keywords

- **none** - Words are never hyphenated, even if soft hyphens (`&shy;` or `\u00AD`) are present
- **manual** - Words are hyphenated only where there are soft hyphen characters in the text (default)
- **auto** - Words are automatically hyphenated at appropriate points based on language rules and hyphenation dictionaries

### Default Value

- **manual** - Only hyphenate at soft hyphen characters

---

## Notes

- Automatic hyphenation requires language information (typically from `lang` attribute)
- The `auto` value uses language-specific hyphenation rules and dictionaries
- Soft hyphens (`&shy;` or Unicode U+00AD) suggest hyphenation points for manual mode
- Hyphenation is most useful for narrow columns and justified text
- The hyphenation algorithm respects minimum character counts before and after hyphens
- Hyphenation can be fine-tuned with related properties like `hyphenate-limit-chars`
- Not all languages support automatic hyphenation equally
- Hyphenation improves text justification by reducing word spacing variations
- The hyphen character used can be customized with `hyphenate-character`

---

## Data Binding

The `hyphens` property can be dynamically controlled through data binding, enabling language-specific hyphenation, column width adaptations, and document format preferences.

### Example 1: Language-Specific Hyphenation

```html
<div style="width: 200pt; padding: 15pt; border: 1pt solid #ccc" lang="{{model.language.code}}">
    <p style="hyphens: {{model.language.hyphenation}}; text-align: justify; font-size: 11pt">
        {{model.content}}
    </p>
</div>

<!-- Data model for English with auto hyphenation:
{
    "language": {
        "code": "en",
        "hyphenation": "auto"
    },
    "content": "Telecommunications infrastructure implementation requires comprehensive documentation..."
}
-->

<!-- Data model for German with auto hyphenation:
{
    "language": {
        "code": "de",
        "hyphenation": "auto"
    },
    "content": "Telekommunikationsinfrastrukturimplementierung erfordert umfassende Dokumentation..."
}
-->
```

### Example 2: Column Width-Based Hyphenation

```html
<div style="width: {{model.column.width}}; padding: 15pt" lang="en">
    <p style="hyphens: {{model.column.hyphenation}}; text-align: justify; font-size: 10pt">
        {{model.column.text}}
    </p>
</div>

<!-- Data model for narrow column:
{
    "column": {
        "width": "150pt",
        "hyphenation": "auto",
        "text": "Advanced telecommunications methodologies..."
    }
}
-->

<!-- Data model for wide column:
{
    "column": {
        "width": "400pt",
        "hyphenation": "manual",
        "text": "Advanced telecommunications methodologies..."
    }
}
-->
```

### Example 3: Accessibility-Enhanced Hyphenation Control

```html
<div style="width: 250pt; padding: 20pt" lang="{{model.document.language}}">
    <p style="hyphens: {{model.accessibility.hyphenationMode}}; text-align: {{model.document.alignment}}; font-size: 11pt">
        {{model.document.content}}
    </p>
</div>

<!-- Data model example:
{
    "document": {
        "language": "en",
        "alignment": "justify",
        "content": "Professional telecommunications infrastructure documentation..."
    },
    "accessibility": {
        "hyphenationMode": "auto"  // or "none" for dyslexia-friendly mode
    }
}
-->
```

---

## Examples

### Example 1: No Hyphenation

```html
<div style="width: 150pt; border: 1pt solid #ccc; padding: 10pt">
    <p style="hyphens: none; text-align: justify; font-size: 11pt">
        Supercalifragilisticexpialidocious is an extraordinarily
        long word that will not be hyphenated.
    </p>
</div>
```

### Example 2: Manual Hyphenation

```html
<div style="width: 150pt; border: 1pt solid #ccc; padding: 10pt">
    <p style="hyphens: manual; text-align: justify; font-size: 11pt">
        Super&shy;cali&shy;fragi&shy;listic&shy;expi&shy;ali&shy;docious
        is a long word with soft hyphens for manual hyphenation.
    </p>
</div>
```

### Example 3: Automatic Hyphenation

```html
<div style="width: 150pt; border: 1pt solid #ccc; padding: 10pt" lang="en">
    <p style="hyphens: auto; text-align: justify; font-size: 11pt">
        Automatic hyphenation helps improve readability in narrow
        columns by breaking long words appropriately.
    </p>
</div>
```

### Example 4: Narrow Column Newsletter

```html
<div style="width: 200pt; padding: 15pt; background-color: #f9fafb" lang="en">
    <h3 style="font-size: 14pt; color: #1e40af; margin-bottom: 10pt">
        Technology Update
    </h3>
    <p style="hyphens: auto; text-align: justify; font-size: 10pt; line-height: 1.6">
        Revolutionary advancements in telecommunications and
        microprocessor technology have fundamentally transformed
        the computational landscape, enabling unprecedented
        interconnectivity and performance capabilities.
    </p>
</div>
```

### Example 5: Multi-Column Layout

```html
<table style="width: 100%">
    <tr>
        <td style="width: 48%; vertical-align: top; padding-right: 10pt" lang="en">
            <p style="hyphens: auto; text-align: justify; font-size: 11pt; line-height: 1.5">
                Advanced implementation methodologies require comprehensive
                documentation and systematic architectural planning to
                ensure successful deployment across organizational
                boundaries.
            </p>
        </td>
        <td style="width: 4%"></td>
        <td style="width: 48%; vertical-align: top; padding-left: 10pt" lang="en">
            <p style="hyphens: auto; text-align: justify; font-size: 11pt; line-height: 1.5">
                Interdepartmental collaboration facilitates knowledge
                transfer and promotes standardization of best practices
                throughout the entire development lifecycle.
            </p>
        </td>
    </tr>
</table>
```

### Example 6: Brochure with Justified Text

```html
<div style="width: 250pt; padding: 20pt; border: 2pt solid #1e40af" lang="en">
    <h2 style="text-align: center; font-size: 18pt; color: #1e40af; margin-bottom: 15pt">
        Professional Services
    </h2>
    <p style="hyphens: auto; text-align: justify; font-size: 11pt; line-height: 1.7">
        Our comprehensive suite of professional consulting services
        encompasses strategic planning, organizational development,
        and technological transformation initiatives designed to
        maximize operational effectiveness and competitive advantage.
    </p>
</div>
```

### Example 7: Legal Document with Long Terms

```html
<div style="width: 400pt; padding: 20pt; font-size: 10pt" lang="en">
    <p style="hyphens: auto; text-align: justify; line-height: 1.6">
        The aforementioned parties acknowledge their responsibilities
        regarding confidentiality, non-disclosure, and indemnification
        obligations as specifically enumerated throughout this
        comprehensive agreement.
    </p>
</div>
```

### Example 8: Product Description in Narrow Format

```html
<div style="width: 180pt; padding: 15pt; background-color: #fef3c7; border-left: 4pt solid #f59e0b" lang="en">
    <h4 style="font-size: 13pt; color: #92400e; margin-bottom: 8pt">
        Product Features
    </h4>
    <p style="hyphens: auto; text-align: justify; font-size: 10pt; line-height: 1.5">
        Weatherproof construction with reinforced polycarbonate
        materials ensures durability and longevity in challenging
        environmental conditions.
    </p>
</div>
```

### Example 9: Academic Journal Format

```html
<div style="width: 300pt; padding: 25pt; font-family: 'Times New Roman', serif" lang="en">
    <h3 style="font-size: 14pt; margin-bottom: 12pt; text-align: center">
        Abstract
    </h3>
    <p style="hyphens: auto; text-align: justify; font-size: 10pt; line-height: 1.6">
        This investigation examines the phenomenological implications
        of interdisciplinary methodological frameworks in contemporary
        socioeconomic research paradigms. Statistical analyses
        demonstrate significant correlations between organizational
        restructuring and productivity metrics.
    </p>
</div>
```

### Example 10: Magazine Sidebar

```html
<div style="width: 160pt; padding: 15pt; background-color: #e0f2fe; border: 2pt solid #0284c7" lang="en">
    <h4 style="font-size: 12pt; color: #0c4a6e; margin-bottom: 10pt; text-transform: uppercase">
        Did You Know?
    </h4>
    <p style="hyphens: auto; font-size: 9pt; line-height: 1.5; text-align: justify">
        Photosynthesis is the biochemical process through which
        plants convert electromagnetic radiation into chemical
        energy, producing carbohydrates from carbon dioxide and
        water molecules.
    </p>
</div>
```

### Example 11: Technical Manual

```html
<div style="width: 350pt; padding: 20pt" lang="en">
    <h3 style="font-size: 15pt; color: #1e293b; margin-bottom: 12pt">
        Installation Instructions
    </h3>
    <p style="hyphens: auto; text-align: justify; font-size: 10pt; line-height: 1.6">
        Before commencing installation procedures, verify that all
        prerequisites including telecommunications infrastructure,
        environmental control systems, and electrical specifications
        meet the manufacturer's recommended configuration parameters.
    </p>
</div>
```

### Example 12: Comparison: No Hyphen vs Auto Hyphen

```html
<table style="width: 100%; border-collapse: collapse">
    <tr>
        <td style="width: 48%; vertical-align: top; padding: 15pt; border: 1pt solid #e5e7eb" lang="en">
            <h4 style="font-size: 11pt; color: #dc2626; margin-bottom: 10pt">
                Without Hyphens
            </h4>
            <p style="hyphens: none; text-align: justify; font-size: 9pt; line-height: 1.5">
                Telecommunications infrastructure implementation requires
                comprehensive documentation and systematic architectural
                planning methodologies.
            </p>
        </td>
        <td style="width: 4%"></td>
        <td style="width: 48%; vertical-align: top; padding: 15pt; border: 1pt solid #e5e7eb" lang="en">
            <h4 style="font-size: 11pt; color: #16a34a; margin-bottom: 10pt">
                With Auto Hyphens
            </h4>
            <p style="hyphens: auto; text-align: justify; font-size: 9pt; line-height: 1.5">
                Telecommunications infrastructure implementation requires
                comprehensive documentation and systematic architectural
                planning methodologies.
            </p>
        </td>
    </tr>
</table>
```

### Example 13: Pharmaceutical Information

```html
<div style="width: 300pt; padding: 20pt; border: 2pt solid #dc2626; background-color: #fef2f2" lang="en">
    <h4 style="font-size: 12pt; color: #991b1b; margin-bottom: 10pt">
        Important Safety Information
    </h4>
    <p style="hyphens: auto; text-align: justify; font-size: 9pt; line-height: 1.5">
        Contraindications include hypersensitivity to acetylsalicylic
        acid, anticoagulant therapy, hemophilia, thrombocytopenia,
        and gastroenterological bleeding disorders. Discontinue
        administration immediately if hypersensitivity reactions occur.
    </p>
</div>
```

### Example 14: Real Estate Listing

```html
<div style="width: 220pt; padding: 15pt; background-color: #f0fdf4; border-left: 4pt solid #16a34a" lang="en">
    <h4 style="font-size: 13pt; color: #166534; margin-bottom: 8pt">
        Property Description
    </h4>
    <p style="hyphens: auto; text-align: justify; font-size: 10pt; line-height: 1.6">
        Spectacular Mediterranean-style residence featuring
        architecturally significant details, professionally landscaped
        grounds, and state-of-the-art environmentally sustainable
        climate control systems throughout.
    </p>
</div>
```

### Example 15: Scientific Report

```html
<div style="width: 400pt; padding: 30pt; font-family: 'Times New Roman', serif" lang="en">
    <h2 style="font-size: 16pt; margin-bottom: 15pt; text-align: center">
        Methodology
    </h2>
    <p style="hyphens: auto; text-align: justify; font-size: 11pt; line-height: 1.7; margin-bottom: 12pt">
        Spectrophotometric measurements were obtained using
        ultraviolet-visible spectroscopy with wavelength resolution
        of approximately 0.5 nanometers. Chromatographic separation
        employed high-performance liquid chromatography with
        fluorescence detection.
    </p>
    <p style="hyphens: auto; text-align: justify; font-size: 11pt; line-height: 1.7">
        Statistical significance was determined through multivariate
        analysis of variance with Bonferroni post-hoc correction for
        multiple comparisons. Probability values below 0.05 were
        considered statistically significant.
    </p>
</div>
```

---

## See Also

- [hyphenate-limit-chars](/reference/cssproperties/hyphenate-limit-chars) - Control minimum word length for hyphenation
- [hyphens-limit-chars-length](/reference/cssproperties/hyphens-limit-chars-length) - Minimum word length
- [hyphens-limit-chars-before](/reference/cssproperties/hyphens-limit-chars-before) - Minimum characters before hyphen
- [hyphens-limit-chars-after](/reference/cssproperties/hyphens-limit-chars-after) - Minimum characters after hyphen
- [hyphenate-character](/reference/cssproperties/hyphenate-character) - Custom hyphen character
- [text-align](/reference/cssproperties/text-align) - Text alignment
- [word-spacing](/reference/cssproperties/word-spacing) - Space between words
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
