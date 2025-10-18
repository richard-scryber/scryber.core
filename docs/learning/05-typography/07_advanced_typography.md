---
layout: default
title: Advanced Typography
nav_order: 7
parent: Typography & Fonts
parent_url: /learning/05-typography/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Advanced Typography

Master advanced typographic techniques including drop caps, special characters, and OpenType features.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Create drop caps for articles
- Use special characters and entities
- Apply OpenType font features (when supported)
- Handle ligatures and special glyphs
- Use subscript and superscript
- Create professional typographic details

---

## Drop Caps

Large first letter at the beginning of a paragraph.

### Basic Drop Cap

```css
p.drop-cap::first-letter {
    float: left;
    font-size: 4em;  /* 4 times larger */
    line-height: 0.8;
    margin-right: 0.1em;
    font-weight: bold;
    color: #2563eb;
}
```

### Styled Drop Cap

```css
.fancy-drop-cap::first-letter {
    float: left;
    font-size: 60pt;
    line-height: 50pt;
    margin: 5pt 10pt 0 0;
    padding: 10pt;
    background-color: #2563eb;
    color: white;
    border-radius: 5pt;
}
```

**Note:** `::first-letter` pseudo-element support may vary in PDF generators.

---

## Special Characters

### HTML Entities

```html
<!-- Common entities -->
&copy;   <!-- © Copyright symbol -->
&reg;    <!-- ® Registered trademark -->
&trade;  <!-- ™ Trademark -->
&nbsp;   <!-- Non-breaking space -->
&mdash;  <!-- — Em dash -->
&ndash;  <!-- – En dash -->
&hellip; <!-- … Ellipsis -->
&quot;   <!-- " Quotation mark -->
&apos;   <!-- ' Apostrophe -->
&lt;     <!-- < Less than -->
&gt;     <!-- > Greater than -->
&amp;    <!-- & Ampersand -->
```

### Typographic Quotes

```html
<!-- Smart quotes -->
&ldquo; &rdquo;  <!-- " " Left and right double quotes -->
&lsquo; &rsquo;  <!-- ' ' Left and right single quotes -->

<!-- Example -->
<p>&ldquo;This is a proper quotation,&rdquo; she said.</p>
```

### Dashes

```html
<!-- Hyphen (on keyboard) -->
-

<!-- En dash (ranges) &ndash; -->
Pages 10&ndash;20

<!-- Em dash (breaks in text) &mdash; -->
She said&mdash;without hesitation&mdash;that it was true.
```

### Mathematical Symbols

```html
&times;   <!-- × Multiplication -->
&divide;  <!-- ÷ Division -->
&minus;   <!-- − Minus -->
&plusmn;  <!-- ± Plus-minus -->
&frac12;  <!-- ½ Fraction one half -->
&frac14;  <!-- ¼ Fraction one quarter -->
&deg;     <!-- ° Degree symbol -->
```

---

## Subscript and Superscript

### HTML Elements

```html
<!-- Subscript -->
<p>H<sub>2</sub>O is water.</p>

<!-- Superscript -->
<p>E = mc<sup>2</sup></p>

<!-- Footnotes -->
<p>This statement requires citation.<sup>1</sup></p>
```

### CSS Styling

```css
sub, sup {
    font-size: 75%;
    line-height: 0;
    position: relative;
    vertical-align: baseline;
}

sub {
    bottom: -0.25em;
}

sup {
    top: -0.5em;
}
```

---

## Ligatures

Special combined characters for better typography.

### Common Ligatures

- fi → fi
- fl → fl
- ff → ff
- ffi → ffi
- ffl → ffl

### Enabling Ligatures (CSS)

```css
p {
    font-variant-ligatures: common-ligatures;
}

/* Or more specific */
.with-ligatures {
    font-variant-ligatures: common-ligatures discretionary-ligatures;
}

/* Disable ligatures */
.no-ligatures {
    font-variant-ligatures: none;
}
```

**Note:** Requires OpenType font with ligature support. Support varies in PDF.

---

## Small Caps

### Using font-variant

```css
.small-caps {
    font-variant: small-caps;
}
```

```html
<p class="small-caps">
    This Text Uses Small Caps
</p>
<!-- Renders approximately as: Tʜɪs Tᴇxᴛ Usᴇs Sᴍᴀʟʟ Cᴀᴘs -->
```

### Real vs Faux Small Caps

**Real small caps (OpenType feature):**
```css
.real-small-caps {
    font-variant: small-caps;
    font-feature-settings: "smcp";  /* OpenType feature */
}
```

**Note:** Real small caps require font support. PDF may synthesize.

---

## OpenType Features

### font-feature-settings

```css
/* Enable specific OpenType features */
.opentype {
    /* Ligatures */
    font-feature-settings: "liga" 1;

    /* Kerning */
    font-feature-settings: "kern" 1;

    /* Old-style numerals */
    font-feature-settings: "onum" 1;

    /* Tabular figures */
    font-feature-settings: "tnum" 1;

    /* Small caps */
    font-feature-settings: "smcp" 1;

    /* Multiple features */
    font-feature-settings: "liga" 1, "kern" 1, "onum" 1;
}
```

**Note:** Requires OpenType font. Support varies greatly in PDF generators.

---

## Practical Examples

### Example 1: Magazine Article with Drop Cap

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Drop Cap Article</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Georgia, "Times New Roman", serif;
            font-size: 12pt;
            line-height: 1.7;
            color: #333;
            margin: 0;
        }

        h1 {
            font-family: Helvetica, sans-serif;
            font-size: 32pt;
            text-align: center;
            margin: 0 0 30pt 0;
            color: #1e40af;
        }

        .byline {
            text-align: center;
            font-style: italic;
            color: #666;
            margin-bottom: 30pt;
        }

        /* ==============================================
           DROP CAP
           ============================================== */
        .drop-cap::first-letter {
            float: left;
            font-size: 4.5em;
            line-height: 0.85;
            margin: 5pt 8pt 0 0;
            font-weight: bold;
            color: #2563eb;
            font-family: Georgia, serif;
        }

        p {
            margin: 0 0 14pt 0;
            text-align: justify;
        }

        /* ==============================================
           PULL QUOTE
           ============================================== */
        .pullquote {
            font-size: 16pt;
            font-style: italic;
            line-height: 1.5;
            text-align: center;
            margin: 30pt 40pt;
            padding: 20pt 0;
            border-top: 2pt solid #2563eb;
            border-bottom: 2pt solid #2563eb;
            color: #2563eb;
        }
    </style>
</head>
<body>
    <h1>The Future of Typography</h1>
    <p class="byline">By Jane Smith, Typography Expert</p>

    <p class="drop-cap">
        Typography is the art and technique of arranging type to make written language legible, readable, and appealing when displayed. The arrangement of type involves selecting typefaces, point sizes, line lengths, line-spacing, and letter-spacing, as well as adjusting the space between pairs of letters.
    </p>

    <p>
        The term typography is also applied to the style, arrangement, and appearance of the letters, numbers, and symbols created by the process. Type design is a closely related craft, sometimes considered part of typography; most typographers do not design typefaces, and some type designers do not consider themselves typographers.
    </p>

    <div class="pullquote">
        &ldquo;Typography is two-dimensional architecture, based on experience and imagination.&rdquo;
        <div style="font-size: 12pt; margin-top: 10pt; font-style: normal; color: #666;">
            &mdash; Hermann Zapf
        </div>
    </div>

    <p>
        Typography has been revolutionized by digital technology. Modern typefaces are stored as digital files containing scalable outline fonts with associated metrics, and these fonts can be rendered at any size with no loss of quality. The introduction of desktop publishing and graphic design software has democratized typography and brought new creative possibilities.
    </p>
</body>
</html>
```

### Example 2: Technical Document with Special Characters

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Special Characters</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        h1 {
            font-size: 24pt;
            margin: 0 0 20pt 0;
            color: #1e40af;
        }

        h2 {
            font-size: 18pt;
            margin: 25pt 0 15pt 0;
            color: #2563eb;
        }

        p {
            margin: 0 0 12pt 0;
        }

        /* ==============================================
           FORMULAS
           ============================================== */
        .formula {
            font-family: "Times New Roman", Times, serif;
            font-size: 13pt;
            padding: 15pt;
            background-color: #f9fafb;
            border: 1pt solid #e5e7eb;
            border-radius: 5pt;
            margin: 15pt 0;
        }

        /* ==============================================
           FOOTNOTES
           ============================================== */
        .footnote-ref {
            font-size: 9pt;
            vertical-align: super;
            color: #2563eb;
        }

        .footnotes {
            margin-top: 40pt;
            padding-top: 20pt;
            border-top: 1pt solid #d1d5db;
            font-size: 9pt;
            color: #666;
        }

        .footnotes p {
            margin: 5pt 0;
        }

        /* ==============================================
           ABBREVIATIONS
           ============================================== */
        abbr {
            text-decoration: underline dotted;
            cursor: help;
        }
    </style>
</head>
<body>
    <h1>Scientific Notation & Symbols</h1>

    <h2>Chemical Formulas</h2>

    <p>
        Water has the chemical formula H<sub>2</sub>O, indicating two hydrogen atoms bonded to one oxygen atom. Similarly, carbon dioxide is CO<sub>2</sub>, and sulfuric acid is H<sub>2</sub>SO<sub>4</sub>.
    </p>

    <h2>Mathematical Expressions</h2>

    <div class="formula">
        E = mc<sup>2</sup>
    </div>

    <p>
        Einstein&rsquo;s famous equation shows that energy (E) equals mass (m) times the speed of light (c) squared.
    </p>

    <div class="formula">
        a<sup>2</sup> + b<sup>2</sup> = c<sup>2</sup>
    </div>

    <p>
        The Pythagorean theorem states that in a right triangle, the square of the hypotenuse equals the sum of squares of the other two sides.
    </p>

    <h2>Temperature Ranges</h2>

    <p>
        Water freezes at 0&deg;C (32&deg;F) and boils at 100&deg;C (212&deg;F). The acceptable temperature range for this process is 20&deg;&ndash;25&deg;C.
    </p>

    <h2>Measurements & Symbols</h2>

    <p>
        The result was &plusmn;0.5mm, approximately &frac12; of the target value. The sample weighed 2.5&times;10<sup>3</sup> grams.
    </p>

    <h2>Proper Quotations</h2>

    <p>
        She said, &ldquo;The experiment was a success,&rdquo; and added, &ldquo;We achieved results beyond our expectations.&rdquo;
    </p>

    <p>
        The term &lsquo;hypothesis&rsquo; refers to a testable prediction about the relationship between variables.<span class="footnote-ref">1</span>
    </p>

    <h2>Corporate Symbols</h2>

    <p>
        Acme Corporation&reg; and its TechPro&trade; product line are registered trademarks. &copy; 2025 Acme Corporation. All rights reserved.
    </p>

    <!-- Footnotes -->
    <div class="footnotes">
        <p><sup>1</sup> From the Greek word &lsquo;hypothesis&rsquo; meaning &ldquo;a basis for reasoning&rdquo;</p>
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Drop Cap Article

Create an article with:
- Drop cap on first paragraph
- Proper quotation marks
- Em dashes for emphasis

### Exercise 2: Technical Document

Create a document with:
- Mathematical formulas with superscript
- Chemical formulas with subscript
- Proper symbols (©, ®, ™, °)

### Exercise 3: Typography Showcase

Create a showcase featuring:
- Various special characters
- Smart quotes throughout
- Proper dashes (en dash and em dash)

---

## Common Pitfalls

### ❌ Using Straight Quotes

```html
<p>"This uses straight quotes"</p>
```

✅ **Solution:**

```html
<p>&ldquo;This uses proper quotes&rdquo;</p>
```

### ❌ Using Hyphens for Dashes

```html
<p>The range is 10-20 pages.</p>  <!-- Should be en dash -->
<p>She said-without thinking-yes.</p>  <!-- Should be em dash -->
```

✅ **Solution:**

```html
<p>The range is 10&ndash;20 pages.</p>
<p>She said&mdash;without thinking&mdash;yes.</p>
```

### ❌ Assuming OpenType Support

```css
.fancy {
    font-feature-settings: "swsh" 1;  /* May not work in PDF */
}
```

✅ **Solution:**

```css
/* Use standard CSS properties that work reliably */
.reliable {
    font-variant: small-caps;
}
```

---

## Advanced Typography Checklist

- [ ] Drop caps styled appropriately
- [ ] Smart quotes used (&ldquo; &rdquo;)
- [ ] Proper dashes (en dash, em dash)
- [ ] Special characters as entities
- [ ] Subscript/superscript for formulas
- [ ] Copyright/trademark symbols correct
- [ ] Tested in generated PDF

---

## Best Practices

1. **Use smart quotes** - &ldquo; &rdquo; not ""
2. **Proper dashes** - En dash for ranges, em dash for breaks
3. **Test drop caps** - May not work in all PDF generators
4. **HTML entities** - For special characters
5. **Subscript/superscript** - Use `<sub>` and `<sup>` tags
6. **OpenType caution** - Limited support in PDF
7. **Professional details** - ©, ®, ™ symbols

---

## Next Steps

1. **[Typography Best Practices](08_typography_best_practices.md)** - Professional patterns
2. **[Content Components](/learning/06-content/)** - Images, lists, tables
3. **[Practical Applications](/learning/08-practical/)** - Real-world documents

---

**Continue learning →** [Typography Best Practices](08_typography_best_practices.md)
