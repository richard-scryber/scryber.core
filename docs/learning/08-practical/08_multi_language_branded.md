---
layout: default
title: Multi-Language & Branded Documents
nav_order: 8
parent: Practical Applications
parent_url: /learning/08-practical/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Multi-Language & Branded Documents

Build enterprise-ready document templates with multi-language support, consistent branding, reusable components, and centralized management.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Implement multi-language document templates
- Apply consistent branding across documents
- Create reusable document components
- Manage document templates centrally
- Handle localization and internationalization
- Build scalable template systems

---

## Multi-Language Template Architecture

### Base Template with Language Support

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml' lang="{{lang}}">
<head>
    <title>{{strings.documentTitle}}</title>
    <style>
        /* Brand Colors */
        :root {
            --brand-primary: {{brand.colors.primary}};
            --brand-secondary: {{brand.colors.secondary}};
            --brand-accent: {{brand.colors.accent}};
            --brand-text: {{brand.colors.text}};
            --brand-light: {{brand.colors.light}};
        }

        @page {
            size: letter;
            margin: {{brand.margins.top}}pt {{brand.margins.right}}pt
                    {{brand.margins.bottom}}pt {{brand.margins.left}}pt;
        }

        body {
            font-family: {{brand.fonts.primary}}, sans-serif;
            font-size: {{brand.fonts.baseSize}}pt;
            line-height: {{brand.fonts.lineHeight}};
            color: var(--brand-text);
        }

        /* Brand header */
        .brand-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-bottom: 2pt solid var(--brand-primary);
            padding-bottom: 15pt;
            margin-bottom: 30pt;
        }

        .brand-logo {
            height: {{brand.logo.height}}pt;
        }

        .brand-tagline {
            font-size: 10pt;
            color: var(--brand-secondary);
            font-style: italic;
        }

        /* Headings */
        h1 {
            font-size: {{brand.fonts.h1Size}}pt;
            color: var(--brand-primary);
            font-weight: bold;
            margin: 0 0 15pt 0;
            font-family: {{brand.fonts.heading}}, sans-serif;
        }

        h2 {
            font-size: {{brand.fonts.h2Size}}pt;
            color: var(--brand-secondary);
            font-weight: 600;
            margin: 20pt 0 10pt 0;
        }

        h3 {
            font-size: {{brand.fonts.h3Size}}pt;
            color: var(--brand-text);
            font-weight: 600;
            margin: 15pt 0 8pt 0;
        }

        /* Brand footer */
        .brand-footer {
            position: absolute;
            bottom: {{brand.margins.bottom}}pt;
            left: {{brand.margins.left}}pt;
            right: {{brand.margins.right}}pt;
            text-align: center;
            font-size: 8pt;
            color: #666;
            border-top: 1pt solid var(--brand-light);
            padding-top: 10pt;
        }

        /* Language-specific adjustments */
        {{#if lang.isRTL}}
        body {
            direction: rtl;
            text-align: right;
        }
        {{/if}}

        /* Content sections */
        .content-section {
            margin-bottom: 25pt;
        }

        .section-title {
            background-color: var(--brand-light);
            color: var(--brand-primary);
            padding: 10pt 15pt;
            margin-bottom: 15pt;
            font-weight: bold;
            border-left: 4pt solid var(--brand-primary);
        }

        /* Data tables */
        .data-table {
            width: 100%;
            border-collapse: collapse;
            margin: 15pt 0;
        }

        .data-table thead {
            background-color: var(--brand-primary);
            color: white;
        }

        .data-table th {
            padding: 10pt;
            text-align: left;
            font-weight: bold;
        }

        .data-table td {
            padding: 8pt 10pt;
            border-bottom: 1pt solid var(--brand-light);
        }

        .data-table tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        /* Call-to-action */
        .cta {
            background: linear-gradient(135deg,
                var(--brand-primary) 0%,
                var(--brand-secondary) 100%);
            color: white;
            padding: 20pt;
            border-radius: 8pt;
            text-align: center;
            margin: 30pt 0;
        }

        .cta h2 {
            color: white;
            margin-top: 0;
        }
    </style>
</head>
<body>
    <!-- Branded Header -->
    <div class="brand-header">
        <div>
            <img src="{{brand.logo.path}}" class="brand-logo" alt="{{brand.name}}" />
            {{#if brand.tagline}}
            <p class="brand-tagline">{{brand.tagline}}</p>
            {{/if}}
        </div>
        <div style="text-align: right;">
            <h1>{{strings.documentTitle}}</h1>
            <p style="margin: 5pt 0 0 0; font-size: 10pt; color: #666;">
                {{strings.documentSubtitle}}
            </p>
        </div>
    </div>

    <!-- Document Content -->
    <div class="content-section">
        <div class="section-title">{{strings.section1Title}}</div>

        <p>{{strings.section1Content}}</p>

        <!-- Example table with localized headers -->
        <table class="data-table">
            <thead>
                <tr>
                    <th>{{strings.table.columnName}}</th>
                    <th>{{strings.table.columnDescription}}</th>
                    <th>{{strings.table.columnValue}}</th>
                </tr>
            </thead>
            <tbody>
                {{#each items}}
                <tr>
                    <td>{{this.name}}</td>
                    <td>{{this.description}}</td>
                    <td>{{this.value}}</td>
                </tr>
                {{/each}}
            </tbody>
        </table>
    </div>

    <!-- Additional sections -->
    {{#each sections}}
    <div class="content-section">
        <div class="section-title">{{this.title}}</div>
        {{#each this.paragraphs}}
        <p>{{this}}</p>
        {{/each}}
    </div>
    {{/each}}

    <!-- Call to Action -->
    {{#if strings.cta}}
    <div class="cta">
        <h2>{{strings.cta.title}}</h2>
        <p>{{strings.cta.message}}</p>
    </div>
    {{/if}}

    <!-- Branded Footer -->
    <div class="brand-footer">
        <p>{{brand.name}} | {{brand.contact.phone}} | {{brand.contact.email}} | {{brand.contact.website}}</p>
        <p>{{strings.footer.copyright}} {{brand.copyrightYear}} {{brand.name}}. {{strings.footer.allRightsReserved}}</p>
    </div>
</body>
</html>
```
{% endraw %}

---

## C# Multi-Language Document System

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class MultiLanguageDocumentGenerator
{
    private readonly string _templatePath;
    private readonly Dictionary<string, LocalizationStrings> _localization;
    private readonly BrandConfiguration _brandConfig;

    public MultiLanguageDocumentGenerator(
        string templatePath,
        BrandConfiguration brandConfig)
    {
        _templatePath = templatePath;
        _brandConfig = brandConfig;
        _localization = new Dictionary<string, LocalizationStrings>();

        // Load localization resources
        LoadLocalizationResources();
    }

    public void GenerateDocument(
        DocumentData documentData,
        string languageCode,
        Stream output)
    {
        // Get localized strings
        var strings = GetLocalizedStrings(languageCode);

        // Load template
        var doc = Document.ParseDocument(_templatePath);

        // Set metadata
        doc.Info.Title = strings.DocumentTitle;
        doc.Info.Author = _brandConfig.Name;
        doc.Info.CreationDate = DateTime.Now;

        // Bind data
        doc.Params["brand"] = _brandConfig;
        doc.Params["strings"] = strings;
        doc.Params["lang"] = new LanguageInfo
        {
            Code = languageCode,
            IsRTL = IsRightToLeft(languageCode)
        };
        doc.Params["items"] = documentData.Items;
        doc.Params["sections"] = documentData.Sections;

        // Configure
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
        doc.RenderOptions.FontSubsetting = true;

        // Generate
        doc.SaveAsPDF(output);
    }

    private void LoadLocalizationResources()
    {
        // English
        _localization["en"] = new LocalizationStrings
        {
            DocumentTitle = "Product Catalog",
            DocumentSubtitle = "Premium Solutions for Your Business",
            Section1Title = "Our Products",
            Section1Content = "Discover our comprehensive range of innovative products designed to meet your needs.",
            Table = new TableStrings
            {
                ColumnName = "Product Name",
                ColumnDescription = "Description",
                ColumnValue = "Price"
            },
            Cta = new CtaStrings
            {
                Title = "Ready to Get Started?",
                Message = "Contact us today to learn more about our products and services."
            },
            Footer = new FooterStrings
            {
                Copyright = "Copyright ©",
                AllRightsReserved = "All rights reserved."
            }
        };

        // Spanish
        _localization["es"] = new LocalizationStrings
        {
            DocumentTitle = "Catálogo de Productos",
            DocumentSubtitle = "Soluciones Premium para su Negocio",
            Section1Title = "Nuestros Productos",
            Section1Content = "Descubra nuestra gama completa de productos innovadores diseñados para satisfacer sus necesidades.",
            Table = new TableStrings
            {
                ColumnName = "Nombre del Producto",
                ColumnDescription = "Descripción",
                ColumnValue = "Precio"
            },
            Cta = new CtaStrings
            {
                Title = "¿Listo para Comenzar?",
                Message = "Contáctenos hoy para conocer más sobre nuestros productos y servicios."
            },
            Footer = new FooterStrings
            {
                Copyright = "Derechos de Autor ©",
                AllRightsReserved = "Todos los derechos reservados."
            }
        };

        // French
        _localization["fr"] = new LocalizationStrings
        {
            DocumentTitle = "Catalogue de Produits",
            DocumentSubtitle = "Solutions Premium pour Votre Entreprise",
            Section1Title = "Nos Produits",
            Section1Content = "Découvrez notre gamme complète de produits innovants conçus pour répondre à vos besoins.",
            Table = new TableStrings
            {
                ColumnName = "Nom du Produit",
                ColumnDescription = "Description",
                ColumnValue = "Prix"
            },
            Cta = new CtaStrings
            {
                Title = "Prêt à Commencer?",
                Message = "Contactez-nous aujourd'hui pour en savoir plus sur nos produits et services."
            },
            Footer = new FooterStrings
            {
                Copyright = "Droits d'auteur ©",
                AllRightsReserved = "Tous droits réservés."
            }
        };

        // German
        _localization["de"] = new LocalizationStrings
        {
            DocumentTitle = "Produktkatalog",
            DocumentSubtitle = "Premium-Lösungen für Ihr Unternehmen",
            Section1Title = "Unsere Produkte",
            Section1Content = "Entdecken Sie unser umfassendes Sortiment an innovativen Produkten, die Ihren Anforderungen gerecht werden.",
            Table = new TableStrings
            {
                ColumnName = "Produktname",
                ColumnDescription = "Beschreibung",
                ColumnValue = "Preis"
            },
            Cta = new CtaStrings
            {
                Title = "Bereit anzufangen?",
                Message = "Kontaktieren Sie uns noch heute, um mehr über unsere Produkte und Dienstleistungen zu erfahren."
            },
            Footer = new FooterStrings
            {
                Copyright = "Urheberrecht ©",
                AllRightsReserved = "Alle Rechte vorbehalten."
            }
        };
    }

    private LocalizationStrings GetLocalizedStrings(string languageCode)
    {
        if (_localization.ContainsKey(languageCode))
        {
            return _localization[languageCode];
        }

        // Default to English
        return _localization["en"];
    }

    private bool IsRightToLeft(string languageCode)
    {
        var rtlLanguages = new[] { "ar", "he", "fa", "ur" };
        return rtlLanguages.Contains(languageCode);
    }
}

// Brand Configuration
public class BrandConfiguration
{
    public string Name { get; set; }
    public string Tagline { get; set; }
    public int CopyrightYear { get; set; }

    public LogoConfig Logo { get; set; }
    public ColorScheme Colors { get; set; }
    public FontConfig Fonts { get; set; }
    public MarginConfig Margins { get; set; }
    public ContactInfo Contact { get; set; }
}

public class LogoConfig
{
    public string Path { get; set; }
    public int Height { get; set; }
}

public class ColorScheme
{
    public string Primary { get; set; }
    public string Secondary { get; set; }
    public string Accent { get; set; }
    public string Text { get; set; }
    public string Light { get; set; }
}

public class FontConfig
{
    public string Primary { get; set; }
    public string Heading { get; set; }
    public int BaseSize { get; set; }
    public decimal LineHeight { get; set; }
    public int H1Size { get; set; }
    public int H2Size { get; set; }
    public int H3Size { get; set; }
}

public class MarginConfig
{
    public int Top { get; set; }
    public int Right { get; set; }
    public int Bottom { get; set; }
    public int Left { get; set; }
}

public class ContactInfo
{
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
}

// Localization models
public class LocalizationStrings
{
    public string DocumentTitle { get; set; }
    public string DocumentSubtitle { get; set; }
    public string Section1Title { get; set; }
    public string Section1Content { get; set; }
    public TableStrings Table { get; set; }
    public CtaStrings Cta { get; set; }
    public FooterStrings Footer { get; set; }
}

public class TableStrings
{
    public string ColumnName { get; set; }
    public string ColumnDescription { get; set; }
    public string ColumnValue { get; set; }
}

public class CtaStrings
{
    public string Title { get; set; }
    public string Message { get; set; }
}

public class FooterStrings
{
    public string Copyright { get; set; }
    public string AllRightsReserved { get; set; }
}

public class LanguageInfo
{
    public string Code { get; set; }
    public bool IsRTL { get; set; }
}

// Document data
public class DocumentData
{
    public List<DocumentItem> Items { get; set; } = new List<DocumentItem>();
    public List<DocumentSection> Sections { get; set; } = new List<DocumentSection>();
}

public class DocumentItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Value { get; set; }
}

public class DocumentSection
{
    public string Title { get; set; }
    public List<string> Paragraphs { get; set; }
}
```

---

## Usage Example

```csharp
// Configure brand
var brandConfig = new BrandConfiguration
{
    Name = "Acme Corporation",
    Tagline = "Innovation for Tomorrow",
    CopyrightYear = 2025,
    Logo = new LogoConfig
    {
        Path = "./images/acme-logo.png",
        Height = 50
    },
    Colors = new ColorScheme
    {
        Primary = "#2563eb",
        Secondary = "#1e40af",
        Accent = "#3b82f6",
        Text = "#1f2937",
        Light = "#eff6ff"
    },
    Fonts = new FontConfig
    {
        Primary = "Helvetica",
        Heading = "Arial",
        BaseSize = 11,
        LineHeight = 1.6m,
        H1Size = 20,
        H2Size = 16,
        H3Size = 13
    },
    Margins = new MarginConfig
    {
        Top = 72,
        Right = 72,
        Bottom = 100,
        Left = 72
    },
    Contact = new ContactInfo
    {
        Phone = "(555) 123-4567",
        Email = "info@acme.com",
        Website = "www.acme.com"
    }
};

// Create document data
var documentData = new DocumentData
{
    Items = new List<DocumentItem>
    {
        new DocumentItem
        {
            Name = "Product A",
            Description = "Premium solution",
            Value = "$999"
        },
        new DocumentItem
        {
            Name = "Product B",
            Description = "Standard solution",
            Value = "$499"
        }
    },
    Sections = new List<DocumentSection>
    {
        new DocumentSection
        {
            Title = "Why Choose Us",
            Paragraphs = new List<string>
            {
                "We provide the best quality products.",
                "Customer satisfaction is our priority."
            }
        }
    }
};

// Generate documents in multiple languages
var generator = new MultiLanguageDocumentGenerator(
    "multilanguage-template.html",
    brandConfig
);

var languages = new[] { "en", "es", "fr", "de" };

foreach (var lang in languages)
{
    using (var output = new FileStream($"catalog-{lang}.pdf", FileMode.Create))
    {
        generator.GenerateDocument(documentData, lang, output);
        Console.WriteLine($"Generated {lang} version");
    }
}
```

---

## Template Management System

```csharp
public class TemplateManager
{
    private readonly string _templatesDirectory;
    private readonly Dictionary<string, BrandConfiguration> _brands;

    public TemplateManager(string templatesDirectory)
    {
        _templatesDirectory = templatesDirectory;
        _brands = new Dictionary<string, BrandConfiguration>();

        LoadBrandConfigurations();
    }

    private void LoadBrandConfigurations()
    {
        // Load brand configurations from JSON files
        // This is a simplified example
    }

    public Stream GenerateDocument(
        string templateName,
        string brandId,
        string languageCode,
        object data)
    {
        var templatePath = Path.Combine(_templatesDirectory, $"{templateName}.html");
        var brand = _brands[brandId];

        var generator = new MultiLanguageDocumentGenerator(templatePath, brand);
        var output = new MemoryStream();

        // Convert data to DocumentData
        var documentData = ConvertToDocumentData(data);

        generator.GenerateDocument(documentData, languageCode, output);
        output.Position = 0;

        return output;
    }

    private DocumentData ConvertToDocumentData(object data)
    {
        // Convert generic data to DocumentData
        // Implementation depends on your data structure
        return new DocumentData();
    }
}
```

---

## Try It Yourself

### Exercise 1: Add Languages

Extend the system with additional languages:
- Add Chinese (simplified/traditional)
- Add Japanese
- Add Arabic (RTL support)
- Test with Unicode characters

### Exercise 2: Brand Themes

Create multiple brand themes:
- Corporate theme
- Modern theme
- Classic theme
- Implement theme switching

### Exercise 3: Resource Management

Build a resource management system:
- Store strings in JSON files
- Load from database
- Cache localization resources
- Support fallback languages

---

## Best Practices

1. **Externalize Strings** - Never hardcode text in templates
2. **Unicode Support** - Use UTF-8 encoding throughout
3. **RTL Support** - Handle right-to-left languages properly
4. **Brand Consistency** - Use centralized brand configuration
5. **Template Versioning** - Track template versions
6. **Testing** - Test all language variations
7. **Performance** - Cache compiled templates
8. **Documentation** - Document string keys and usage

---

## Localization Checklist

- [ ] All strings externalized
- [ ] Language files created for each locale
- [ ] RTL support implemented (if needed)
- [ ] Date/number formatting localized
- [ ] Currency symbols localized
- [ ] Unicode characters tested
- [ ] Fallback language configured
- [ ] Brand colors consistent across languages
- [ ] Tested with native speakers

---

## Brand Consistency Checklist

- [ ] Logo usage consistent
- [ ] Colors match brand guidelines
- [ ] Typography follows standards
- [ ] Spacing and margins consistent
- [ ] Headers and footers branded
- [ ] Contact information accurate
- [ ] Legal notices included
- [ ] Version control implemented

---

## Congratulations!

You've completed the **Practical Applications** series and mastered real-world PDF generation with Scryber!

### What You've Learned

Through this series, you've built complete, production-ready examples:

1. **[Invoice Template](01_invoice_template.md)** - Professional invoicing
2. **[Business Letter](02_business_letter.md)** - Formal correspondence
3. **[Report Template](03_report_template.md)** - Multi-section reports
4. **[Certificate Template](04_certificate_template.md)** - Awards and certificates
5. **[Data-Driven Report](05_data_driven_report.md)** - Database integration
6. **[Catalog & Brochure](06_catalog_brochure.md)** - Product catalogs
7. **[Form Template](07_form_template.md)** - Print-and-fill forms
8. **Multi-Language & Branded Documents** - Enterprise systems

### Next Steps

**Continue Your Journey:**

- **Review Previous Series** - Reinforce your learning
- **Build Your Own Projects** - Apply these patterns to your needs
- **Explore API Documentation** - Deep dive into Scryber features
- **Join the Community** - Share your experiences and learn from others

### Additional Resources

- **[Complete Reference](/reference/)** - API documentation
- **[Code Examples](/examples/)** - More sample code
- **[GitHub Repository](https://github.com/richard-scryber/scryber.core)** - Source code and issues
- **[Community Forum](https://github.com/richard-scryber/scryber.core/discussions)** - Ask questions

---

**Explore more →** [Learning Guide Home](/learning/)
