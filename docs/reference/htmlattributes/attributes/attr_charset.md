---
layout: default
title: charset
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @charset : The Character Encoding Attribute

The `charset` attribute specifies the character encoding for an HTML document or external resource. When used in `<meta>` elements, it declares the document's character encoding, ensuring proper interpretation and display of text content in the generated PDF. UTF-8 is the recommended encoding for maximum compatibility.

## Usage

The `charset` attribute defines character encoding:
- Declares the document's character encoding in `<meta>` elements
- Must be UTF-8 for proper PDF text rendering
- Ensures correct interpretation of special characters and international text
- Should be declared early in the `<head>` section
- Critical for documents containing non-ASCII characters
- Supports data binding for dynamic encoding specification

```html
<!-- Standard UTF-8 encoding (recommended) -->
<meta charset="UTF-8" />

<!-- Alternative declaration method -->
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

<!-- Inside head section -->
<head>
    <meta charset="UTF-8" />
    <title>Document Title</title>
</head>
```

---

## Supported Elements

The `charset` attribute is used with:

### Meta Element
- `<meta>` - Declares document character encoding (primary use)

In HTML5, the simplified `<meta charset="UTF-8">` syntax is preferred over the older `http-equiv` method.

---

## Binding Values

The `charset` attribute supports data binding for dynamic encoding specification:

```html
<!-- Dynamic charset from model -->
<meta charset="{{model.encoding}}" />

<!-- Conditional encoding -->
<meta charset="{{model.useUnicode ? 'UTF-8' : 'ISO-8859-1'}}" />

<!-- Default to UTF-8 if not specified -->
<meta charset="{{model.charset || 'UTF-8'}}" />
```

**Data Model Example:**
```json
{
  "encoding": "UTF-8",
  "useUnicode": true,
  "charset": "UTF-8"
}
```

---

## Notes

### UTF-8 is Required for PDF Generation

Scryber requires **UTF-8** encoding for proper PDF generation:

```html
<!-- CORRECT: UTF-8 encoding -->
<meta charset="UTF-8" />

<!-- Avoid other encodings in PDF context -->
<meta charset="ISO-8859-1" />  <!-- May cause rendering issues -->
<meta charset="Windows-1252" />  <!-- May cause rendering issues -->
```

UTF-8 supports:
- All Unicode characters
- International alphabets (Arabic, Chinese, Japanese, Korean, Cyrillic, etc.)
- Mathematical symbols and special characters
- Emoji and extended character sets

### Position in Document

The `charset` declaration should appear:
- Early in the `<head>` section (preferably first)
- Before any content that contains special characters
- Before `<title>` elements with non-ASCII characters

```html
<!DOCTYPE html>
<html>
<head>
    <!-- Charset should be first -->
    <meta charset="UTF-8" />
    <title>Documento en Español</title>
    <meta name="description" content="Document description" />
</head>
<body>
    <p>Content with special characters: é, ñ, ü, ç</p>
</body>
</html>
```

### HTML5 vs Legacy Syntax

HTML5 introduced a simplified syntax:

```html
<!-- HTML5 syntax (preferred) -->
<meta charset="UTF-8" />

<!-- Legacy HTML4 syntax (still supported) -->
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
```

Both syntaxes work in Scryber, but the HTML5 syntax is cleaner and recommended.

### Case Insensitivity

Character encoding names are case-insensitive:

```html
<!-- All equivalent -->
<meta charset="UTF-8" />
<meta charset="utf-8" />
<meta charset="Utf-8" />
```

However, **uppercase UTF-8** is the conventional standard.

### Special Characters Without Encoding

Without proper charset declaration, special characters may not render correctly:

```html
<!-- Without charset - may display incorrectly -->
<html>
<head>
    <title>Document</title>
</head>
<body>
    <p>Café, naïve, Zürich</p>  <!-- May show as Caf�, na�ve, Z�rich -->
</body>
</html>

<!-- With charset - renders correctly -->
<html>
<head>
    <meta charset="UTF-8" />
    <title>Document</title>
</head>
<body>
    <p>Café, naïve, Zürich</p>  <!-- Displays correctly -->
</body>
</html>
```

### International Content

UTF-8 supports all international character sets:

```html
<meta charset="UTF-8" />

<!-- Arabic -->
<p>مرحبا بك</p>

<!-- Chinese -->
<p>欢迎</p>

<!-- Japanese -->
<p>ようこそ</p>

<!-- Korean -->
<p>환영합니다</p>

<!-- Cyrillic -->
<p>Добро пожаловать</p>

<!-- Greek -->
<p>Καλώς ήρθατε</p>
```

### Mathematical and Technical Symbols

UTF-8 enables mathematical and technical symbols:

```html
<meta charset="UTF-8" />

<p>Mathematical: ∑ ∫ ∞ ≤ ≥ ≠ ± × ÷ √ π</p>
<p>Currency: $ € £ ¥ ₹ ₽</p>
<p>Arrows: → ← ↑ ↓ ⇒ ⇐</p>
<p>Symbols: © ® ™ § ¶ † ‡</p>
<p>Fractions: ½ ⅓ ¼ ¾</p>
```

### Emoji Support

UTF-8 includes emoji characters:

```html
<meta charset="UTF-8" />

<p>Status: ✅ Approved ❌ Rejected ⚠️ Warning</p>
<p>Ratings: ⭐⭐⭐⭐⭐</p>
<p>Contact: 📧 📞 🏠</p>
```

Note: Emoji rendering in PDFs depends on font support. Not all PDF viewers display all emoji correctly.

### Byte Order Mark (BOM)

UTF-8 files should generally not include a Byte Order Mark (BOM):

- UTF-8 without BOM is preferred
- BOM can cause issues with some parsers
- Modern editors default to UTF-8 without BOM

If you encounter issues with special characters:
1. Verify file is saved as UTF-8
2. Check for BOM and remove if present
3. Ensure `charset` meta tag is declared

---

## Examples

### Basic UTF-8 Declaration

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>My Document</title>
</head>
<body>
    <h1>Welcome</h1>
    <p>This document uses UTF-8 encoding.</p>
</body>
</html>
```

### Document with Special Characters

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Café Menu</title>
</head>
<body>
    <h1>Café Menu</h1>

    <h2>Beverages</h2>
    <ul>
        <li>Café au lait - $4.50</li>
        <li>Espresso - $3.00</li>
        <li>Café crème - $4.00</li>
        <li>Cappuccino - $4.50</li>
    </ul>

    <h2>Desserts</h2>
    <ul>
        <li>Crème brûlée - $6.50</li>
        <li>Tarte tatin - $5.50</li>
        <li>Éclair au chocolat - $4.00</li>
    </ul>

    <p>Bon appétit!</p>
</body>
</html>
```

### Multilingual Document

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Multilingual Welcome</title>
    <style>
        .language-section {
            margin: 20pt 0;
            padding: 15pt;
            border: 1pt solid #ccc;
        }
    </style>
</head>
<body>
    <h1>Welcome in Multiple Languages</h1>

    <div class="language-section">
        <h2>English</h2>
        <p>Welcome to our service!</p>
    </div>

    <div class="language-section">
        <h2>Español</h2>
        <p>¡Bienvenido a nuestro servicio!</p>
    </div>

    <div class="language-section">
        <h2>Français</h2>
        <p>Bienvenue à notre service!</p>
    </div>

    <div class="language-section">
        <h2>Deutsch</h2>
        <p>Willkommen zu unserem Service!</p>
    </div>

    <div class="language-section">
        <h2>日本語</h2>
        <p>私たちのサービスへようこそ！</p>
    </div>

    <div class="language-section">
        <h2>中文</h2>
        <p>欢迎使用我们的服务！</p>
    </div>

    <div class="language-section">
        <h2>한국어</h2>
        <p>서비스에 오신 것을 환영합니다!</p>
    </div>

    <div class="language-section">
        <h2>العربية</h2>
        <p>مرحبا بكم في خدمتنا!</p>
    </div>

    <div class="language-section">
        <h2>Русский</h2>
        <p>Добро пожаловать в наш сервис!</p>
    </div>
</body>
</html>
```

### Technical Document with Symbols

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Mathematical Formulas</title>
    <style>
        .formula {
            font-family: 'Times New Roman', serif;
            font-size: 14pt;
            margin: 10pt 0;
            padding: 10pt;
            background-color: #f8f9fa;
        }
    </style>
</head>
<body>
    <h1>Mathematical Formulas</h1>

    <div class="formula">
        <strong>Pythagorean Theorem:</strong><br/>
        a² + b² = c²
    </div>

    <div class="formula">
        <strong>Quadratic Formula:</strong><br/>
        x = (-b ± √(b² - 4ac)) / 2a
    </div>

    <div class="formula">
        <strong>Euler's Identity:</strong><br/>
        e^(iπ) + 1 = 0
    </div>

    <div class="formula">
        <strong>Sum Notation:</strong><br/>
        ∑(i=1 to n) i = n(n+1)/2
    </div>

    <div class="formula">
        <strong>Integral:</strong><br/>
        ∫(a to b) f(x)dx
    </div>

    <div class="formula">
        <strong>Set Theory:</strong><br/>
        A ∩ B ⊆ A ∪ B<br/>
        A ∈ U, B ∈ U<br/>
        ∅ ⊂ A ⊂ U
    </div>

    <h2>Inequalities</h2>
    <p>
        x ≤ 10<br/>
        y ≥ 5<br/>
        a ≠ b<br/>
        c ≈ 3.14159
    </p>

    <h2>Greek Letters</h2>
    <p>
        α (alpha), β (beta), γ (gamma), δ (delta), ε (epsilon),
        π (pi), σ (sigma), ω (omega), Σ (Sigma), Ω (Omega)
    </p>
</body>
</html>
```

### Currency and Financial Document

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>International Pricing</title>
    <style>
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }
        th, td {
            border: 1pt solid #ddd;
            padding: 10pt;
            text-align: left;
        }
        th {
            background-color: #f2f2f2;
        }
    </style>
</head>
<body>
    <h1>International Pricing</h1>

    <table>
        <thead>
            <tr>
                <th>Region</th>
                <th>Currency</th>
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>United States</td>
                <td>US Dollar</td>
                <td>$99.99</td>
            </tr>
            <tr>
                <td>European Union</td>
                <td>Euro</td>
                <td>€89.99</td>
            </tr>
            <tr>
                <td>United Kingdom</td>
                <td>Pound Sterling</td>
                <td>£79.99</td>
            </tr>
            <tr>
                <td>Japan</td>
                <td>Yen</td>
                <td>¥11,000</td>
            </tr>
            <tr>
                <td>India</td>
                <td>Rupee</td>
                <td>₹8,299</td>
            </tr>
            <tr>
                <td>Russia</td>
                <td>Ruble</td>
                <td>₽9,999</td>
            </tr>
        </tbody>
    </table>

    <p><strong>Note:</strong> Prices shown in local currencies. Exchange rates as of today.</p>
</body>
</html>
```

### Data-Bound Charset

```html
<!-- Model: { document: { encoding: "UTF-8", language: "en" } } -->

<!DOCTYPE html>
<html lang="{{model.document.language}}">
<head>
    <meta charset="{{model.document.encoding}}" />
    <title>Dynamic Document</title>
</head>
<body>
    <h1>Document with Dynamic Encoding</h1>
    <p>Character encoding: {{model.document.encoding}}</p>
    <p>Language: {{model.document.language}}</p>
</body>
</html>
```

### Document with Diacritics

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>European Languages</title>
</head>
<body>
    <h1>Diacritical Marks in European Languages</h1>

    <h2>French</h2>
    <p>é (acute), è (grave), ê (circumflex), ë (dieresis), ç (cedilla)</p>
    <p>Example: Côte d'Azur, déjà vu, crème fraîche</p>

    <h2>German</h2>
    <p>ä, ö, ü, ß (eszett)</p>
    <p>Example: Müller, Größe, Äpfel, Straße</p>

    <h2>Spanish</h2>
    <p>á, é, í, ó, ú, ñ, ü</p>
    <p>Example: España, año, niño, más, José</p>

    <h2>Portuguese</h2>
    <p>ã, õ, á, à, â, é, ê, í, ó, ô, ú, ç</p>
    <p>Example: São Paulo, João, ação, coração</p>

    <h2>Italian</h2>
    <p>à, è, é, ì, ò, ù</p>
    <p>Example: caffè, città, però, università</p>

    <h2>Czech</h2>
    <p>á, č, ď, é, ě, í, ň, ó, ř, š, ť, ú, ů, ý, ž</p>
    <p>Example: Česká republika, Dvořák, Václav</p>

    <h2>Polish</h2>
    <p>ą, ć, ę, ł, ń, ó, ś, ź, ż</p>
    <p>Example: Polska, Gdańsk, Łódź</p>
</body>
</html>
```

### Scientific Document

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Physics Formulas</title>
    <style>
        .equation {
            font-family: 'Times New Roman', serif;
            font-size: 14pt;
            margin: 15pt 0;
            padding: 10pt;
            background-color: #f0f0f0;
            border-left: 4pt solid #336699;
        }
    </style>
</head>
<body>
    <h1>Physics Formulas and Constants</h1>

    <h2>Famous Equations</h2>

    <div class="equation">
        <strong>Einstein's Mass-Energy Equivalence:</strong><br/>
        E = mc²
    </div>

    <div class="equation">
        <strong>Newton's Second Law:</strong><br/>
        F = ma
    </div>

    <div class="equation">
        <strong>Planck's Constant:</strong><br/>
        E = hν<br/>
        h ≈ 6.626 × 10⁻³⁴ J·s
    </div>

    <div class="equation">
        <strong>Schrödinger Equation:</strong><br/>
        iℏ ∂ψ/∂t = Ĥψ
    </div>

    <h2>Physical Constants</h2>
    <ul>
        <li>Speed of light: c ≈ 3.0 × 10⁸ m/s</li>
        <li>Gravitational constant: G ≈ 6.674 × 10⁻¹¹ N·m²/kg²</li>
        <li>Planck constant: h ≈ 6.626 × 10⁻³⁴ J·s</li>
        <li>Electron mass: mₑ ≈ 9.109 × 10⁻³¹ kg</li>
        <li>Proton mass: mₚ ≈ 1.673 × 10⁻²⁷ kg</li>
    </ul>

    <h2>Units and Symbols</h2>
    <p>
        Temperature: °C (Celsius), °F (Fahrenheit), K (Kelvin)<br/>
        Angles: ° (degrees), ' (minutes), " (seconds)<br/>
        Micro: µ (micro), Ω (ohm), Å (angstrom)
    </p>
</body>
</html>
```

### Business Document with Special Characters

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Business Report</title>
    <style>
        .highlight {
            background-color: #ffff99;
            padding: 2pt;
        }
        .metric {
            font-weight: bold;
            color: #336699;
        }
    </style>
</head>
<body>
    <h1>Q4 Financial Report</h1>

    <h2>Executive Summary</h2>
    <p>
        Revenue increased by <span class="metric">23%</span> year-over-year,
        reaching <span class="metric">€4.5M</span> in Q4 2024.
    </p>

    <h2>Key Metrics</h2>
    <ul>
        <li>Revenue: €4,500,000 (↑ 23%)</li>
        <li>Profit Margin: 18.5% (↑ 2.3%)</li>
        <li>Customer Growth: +1,250 (↑ 15%)</li>
        <li>Average Order Value: €125 (↑ €8)</li>
        <li>Return Rate: 2.1% (↓ 0.4%)</li>
    </ul>

    <h2>Regional Performance</h2>
    <table style="width: 100%; border-collapse: collapse;">
        <thead>
            <tr style="background-color: #f2f2f2;">
                <th style="border: 1pt solid #ddd; padding: 8pt;">Region</th>
                <th style="border: 1pt solid #ddd; padding: 8pt;">Revenue</th>
                <th style="border: 1pt solid #ddd; padding: 8pt;">Growth</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">North America</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">$1.8M</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">↑ 18%</td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">Europe</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">€1.5M</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">↑ 25%</td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">Asia-Pacific</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">¥180M</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">↑ 32%</td>
            </tr>
        </tbody>
    </table>

    <p style="margin-top: 20pt;">
        <strong>Note:</strong> All figures are approximate and subject to final audit.
        © 2025 Company Name. All rights reserved.
    </p>
</body>
</html>
```

### Product Catalog with Symbols

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Product Catalog</title>
    <style>
        .product {
            border: 1pt solid #ccc;
            padding: 15pt;
            margin: 10pt 0;
        }
        .price {
            font-size: 18pt;
            font-weight: bold;
            color: #28a745;
        }
        .rating {
            color: #ffc107;
        }
    </style>
</head>
<body>
    <h1>Product Catalog</h1>

    <div class="product">
        <h2>Premium Widget™</h2>
        <p>The ultimate solution for your needs.</p>
        <p class="price">$149.99</p>
        <p class="rating">★★★★★ (5.0)</p>
        <p>✓ Free shipping • ✓ 2-year warranty • ✓ 30-day returns</p>
    </div>

    <div class="product">
        <h2>Basic Gadget®</h2>
        <p>Reliable performance at an affordable price.</p>
        <p class="price">€79.99</p>
        <p class="rating">★★★★☆ (4.2)</p>
        <p>✓ Free shipping • ✓ 1-year warranty</p>
    </div>

    <div class="product">
        <h2>Professional Tool Set</h2>
        <p>Complete toolkit for professionals.</p>
        <p class="price">£199.99</p>
        <p class="rating">★★★★★ (4.8)</p>
        <p>✓ Express shipping • ✓ Lifetime warranty • ✓ 60-day returns</p>
    </div>

    <div style="margin-top: 20pt; padding: 15pt; background-color: #f8f9fa;">
        <h3>Shipping Information</h3>
        <p>
            🚚 Standard Shipping: 3–5 business days<br/>
            ⚡ Express Shipping: 1–2 business days<br/>
            🌍 International Shipping: 7–14 business days
        </p>
    </div>
</body>
</html>
```

### Legacy Charset Syntax

```html
<!DOCTYPE html>
<html>
<head>
    <!-- Legacy HTML4 syntax (still supported) -->
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>Legacy Syntax Example</title>
</head>
<body>
    <h1>Document with Legacy Charset Declaration</h1>
    <p>This document uses the older charset declaration syntax.</p>
    <p>Special characters still work: café, naïve, Zürich</p>
</body>
</html>
```

### Conditional Charset with Fallback

```html
<!-- Model: { preferences: { encoding: null } } -->

<!DOCTYPE html>
<html>
<head>
    <!-- Use UTF-8 as fallback if encoding not specified -->
    <meta charset="{{model.preferences.encoding || 'UTF-8'}}" />
    <title>Document with Fallback Encoding</title>
</head>
<body>
    <h1>Dynamic Charset with Fallback</h1>
    <p>Using encoding: {{model.preferences.encoding || 'UTF-8 (default)'}}</p>
</body>
</html>
```

---

## See Also

- [meta](/reference/htmltags/meta.html) - Meta element for document metadata
- [lang](/reference/htmlattributes/lang.html) - Language attribute for content language
- [title](/reference/htmlattributes/title.html) - Document title element
- [http-equiv](/reference/htmlattributes/http-equiv.html) - HTTP equivalent headers
- [UTF-8 Encoding](/reference/encoding/utf8.html) - UTF-8 character encoding guide
- [International Content](/reference/international/) - Creating multilingual documents

---
