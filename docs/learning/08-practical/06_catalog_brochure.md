---
layout: default
title: Catalog & Brochure
nav_order: 6
parent: Practical Applications
parent_url: /learning/08-practical/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Catalog & Brochure

Create professional product catalogs and brochures with images, grid layouts, pricing, and descriptions for marketing and sales materials.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Design product catalog layouts
- Create grid-based product displays
- Handle product images efficiently
- Format pricing and specifications
- Build category sections
- Generate brochures from product data

---

## Complete Product Catalog Template

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>{{catalog.title}}</title>
    <style>
        @page {
            size: letter;
            margin: 36pt;
        }

        body {
            font-family: 'Helvetica', 'Arial', sans-serif;
            font-size: 10pt;
            line-height: 1.5;
            color: #333;
        }

        /* Cover page */
        .cover-page {
            page-break-after: always;
            height: 100%;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            text-align: center;
            margin: -36pt;
            padding: 72pt;
        }

        .cover-logo {
            height: 80pt;
            margin-bottom: 30pt;
        }

        .cover-title {
            font-size: 42pt;
            font-weight: bold;
            margin: 0 0 20pt 0;
        }

        .cover-subtitle {
            font-size: 18pt;
            margin: 0 0 40pt 0;
            opacity: 0.9;
        }

        .cover-year {
            font-size: 24pt;
            margin-top: 40pt;
        }

        /* Category page */
        .category-page {
            page-break-before: always;
        }

        .category-header {
            background-color: #2563eb;
            color: white;
            padding: 20pt;
            margin: -36pt -36pt 30pt -36pt;
            text-align: center;
        }

        .category-title {
            font-size: 24pt;
            font-weight: bold;
            margin: 0;
        }

        .category-description {
            font-size: 12pt;
            margin: 10pt 0 0 0;
            opacity: 0.9;
        }

        /* Product grid */
        .product-grid {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 20pt;
            margin-bottom: 20pt;
        }

        .product-card {
            border: 1pt solid #e5e7eb;
            border-radius: 8pt;
            overflow: hidden;
            background: white;
            page-break-inside: avoid;
        }

        .product-image-container {
            background-color: #f9fafb;
            height: 150pt;
            display: flex;
            align-items: center;
            justify-content: center;
            overflow: hidden;
        }

        .product-image {
            max-width: 100%;
            max-height: 150pt;
            object-fit: contain;
        }

        .product-content {
            padding: 15pt;
        }

        .product-badge {
            display: inline-block;
            background-color: #dc2626;
            color: white;
            padding: 3pt 8pt;
            border-radius: 3pt;
            font-size: 8pt;
            font-weight: bold;
            text-transform: uppercase;
            margin-bottom: 8pt;
        }

        .product-badge.new {
            background-color: #10b981;
        }

        .product-badge.sale {
            background-color: #f59e0b;
        }

        .product-name {
            font-size: 12pt;
            font-weight: bold;
            color: #1e3a8a;
            margin: 0 0 5pt 0;
        }

        .product-sku {
            font-size: 8pt;
            color: #666;
            margin: 0 0 10pt 0;
        }

        .product-description {
            font-size: 9pt;
            line-height: 1.4;
            color: #555;
            margin: 10pt 0;
            min-height: 40pt;
        }

        .product-features {
            font-size: 8pt;
            color: #666;
            margin: 10pt 0;
        }

        .product-features li {
            margin: 3pt 0;
        }

        .product-footer {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: 15pt;
            padding-top: 15pt;
            border-top: 1pt solid #e5e7eb;
        }

        .product-price {
            font-size: 16pt;
            font-weight: bold;
            color: #2563eb;
        }

        .product-price-old {
            font-size: 11pt;
            color: #999;
            text-decoration: line-through;
            margin-left: 5pt;
        }

        .product-stock {
            font-size: 8pt;
            color: #666;
        }

        .stock-available {
            color: #10b981;
            font-weight: bold;
        }

        .stock-low {
            color: #f59e0b;
            font-weight: bold;
        }

        .stock-out {
            color: #dc2626;
            font-weight: bold;
        }

        /* Full-width product (featured) */
        .product-featured {
            border: 2pt solid #2563eb;
            border-radius: 8pt;
            overflow: hidden;
            margin-bottom: 20pt;
            page-break-inside: avoid;
        }

        .product-featured-layout {
            display: flex;
            gap: 20pt;
            padding: 20pt;
        }

        .product-featured-image {
            flex: 0 0 200pt;
        }

        .product-featured-content {
            flex: 1;
        }

        .product-featured-title {
            font-size: 18pt;
            font-weight: bold;
            color: #2563eb;
            margin: 0 0 10pt 0;
        }

        /* Specifications table */
        .specs-table {
            width: 100%;
            border-collapse: collapse;
            font-size: 9pt;
            margin: 15pt 0;
        }

        .specs-table td {
            padding: 5pt 8pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        .specs-table td:first-child {
            font-weight: bold;
            color: #666;
            width: 120pt;
        }

        /* Page header/footer */
        .page-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding-bottom: 10pt;
            margin-bottom: 20pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        .page-footer {
            position: absolute;
            bottom: 20pt;
            left: 36pt;
            right: 36pt;
            display: flex;
            justify-content: space-between;
            font-size: 8pt;
            color: #666;
            border-top: 1pt solid #e5e7eb;
            padding-top: 10pt;
        }
    </style>
</head>
<body>
    <!-- Cover Page -->
    <div class="cover-page">
        <img src="{{company.logo}}" class="cover-logo" alt="{{company.name}}" />
        <h1 class="cover-title">{{catalog.title}}</h1>
        <p class="cover-subtitle">{{catalog.subtitle}}</p>
        <p class="cover-year">{{catalog.year}}</p>
    </div>

    <!-- Loop through categories -->
    {{#each categories}}
    <div class="category-page">
        <!-- Category Header -->
        <div class="category-header">
            <h1 class="category-title">{{this.name}}</h1>
            <p class="category-description">{{this.description}}</p>
        </div>

        <!-- Featured Product (if any) -->
        {{#if this.featuredProduct}}
        <div class="product-featured">
            <div class="product-featured-layout">
                <div class="product-featured-image">
                    <img src="{{this.featuredProduct.image}}"
                         style="max-width: 200pt; max-height: 200pt;" />
                </div>
                <div class="product-featured-content">
                    {{#if this.featuredProduct.badge}}
                    <span class="product-badge {{this.featuredProduct.badgeClass}}">
                        {{this.featuredProduct.badge}}
                    </span>
                    {{/if}}
                    <h2 class="product-featured-title">{{this.featuredProduct.name}}</h2>
                    <p style="font-size: 10pt; margin: 10pt 0;">
                        {{this.featuredProduct.description}}
                    </p>

                    {{#if this.featuredProduct.features}}
                    <ul style="margin: 15pt 0; font-size: 9pt;">
                        {{#each this.featuredProduct.features}}
                        <li>{{this}}</li>
                        {{/each}}
                    </ul>
                    {{/if}}

                    <div style="display: flex; justify-content: space-between; align-items: center; margin-top: 20pt;">
                        <div>
                            <span class="product-price">${{format(this.featuredProduct.price, '#,##0.00')}}</span>
                            {{#if this.featuredProduct.oldPrice}}
                            <span class="product-price-old">${{format(this.featuredProduct.oldPrice, '#,##0.00')}}</span>
                            {{/if}}
                        </div>
                        <div style="font-size: 9pt; color: #666;">
                            SKU: {{this.featuredProduct.sku}}
                        </div>
                    </div>
                </div>
            </div>
        </div>
        {{/if}}

        <!-- Product Grid -->
        <div class="product-grid">
            {{#each this.products}}
            <div class="product-card">
                <!-- Product Image -->
                <div class="product-image-container">
                    <img src="{{this.image}}" class="product-image" alt="{{this.name}}" />
                </div>

                <!-- Product Content -->
                <div class="product-content">
                    <!-- Badge -->
                    {{#if this.badge}}
                    <span class="product-badge {{this.badgeClass}}">{{this.badge}}</span>
                    {{/if}}

                    <!-- Name & SKU -->
                    <h3 class="product-name">{{this.name}}</h3>
                    <p class="product-sku">SKU: {{this.sku}}</p>

                    <!-- Description -->
                    <p class="product-description">{{this.description}}</p>

                    <!-- Features -->
                    {{#if this.features}}
                    <ul class="product-features">
                        {{#each this.features}}
                        <li>✓ {{this}}</li>
                        {{/each}}
                    </ul>
                    {{/if}}

                    <!-- Specifications -->
                    {{#if this.specs}}
                    <table class="specs-table">
                        {{#each this.specs}}
                        <tr>
                            <td>{{this.label}}</td>
                            <td>{{this.value}}</td>
                        </tr>
                        {{/each}}
                    </table>
                    {{/if}}

                    <!-- Footer: Price & Stock -->
                    <div class="product-footer">
                        <div>
                            <span class="product-price">${{format(this.price, '#,##0.00')}}</span>
                            {{#if this.oldPrice}}
                            <span class="product-price-old">${{format(this.oldPrice, '#,##0.00')}}</span>
                            {{/if}}
                        </div>
                        <div class="product-stock">
                            <span class="stock-{{this.stockClass}}">{{this.stockStatus}}</span>
                        </div>
                    </div>
                </div>
            </div>
            {{/each}}
        </div>
    </div>
    {{/each}}

    <!-- Page Footer -->
    <div class="page-footer">
        <span>{{company.name}} | {{company.phone}} | {{company.website}}</span>
        <span>Page <page-number /> of <page-count /></span>
        <span>{{catalog.year}} Catalog</span>
    </div>
</body>
</html>
```
{% endraw %}

---

## C# Catalog Generator

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class CatalogGenerator
{
    private readonly string _templatePath;

    public CatalogGenerator(string templatePath)
    {
        _templatePath = templatePath;
    }

    public void GenerateCatalog(CatalogData catalogData, Stream output)
    {
        // Load template
        var doc = Document.ParseDocument(_templatePath);

        // Set metadata
        doc.Info.Title = catalogData.Catalog.Title;
        doc.Info.Author = catalogData.Company.Name;
        doc.Info.Subject = "Product Catalog";
        doc.Info.Keywords = string.Join(", ",
            catalogData.Categories.Select(c => c.Name));
        doc.Info.CreationDate = DateTime.Now;

        // Bind data
        doc.Params["company"] = catalogData.Company;
        doc.Params["catalog"] = catalogData.Catalog;
        doc.Params["categories"] = catalogData.Categories;

        // Configure for images
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
        doc.RenderOptions.ImageQuality = 85;
        doc.RenderOptions.ImageCacheDurationMinutes = 120;

        // Generate PDF
        doc.SaveAsPDF(output);
    }

    public void GenerateFromDatabase(
        string connectionString,
        CatalogConfiguration config,
        Stream output)
    {
        // Load products from database
        var catalogData = LoadCatalogData(connectionString, config);

        // Generate catalog
        GenerateCatalog(catalogData, output);
    }

    private CatalogData LoadCatalogData(
        string connectionString,
        CatalogConfiguration config)
    {
        // In a real implementation, load from database
        // This is a simplified example

        return new CatalogData
        {
            Company = config.Company,
            Catalog = config.Catalog,
            Categories = LoadCategoriesFromDatabase(connectionString)
        };
    }

    private List<Category> LoadCategoriesFromDatabase(string connectionString)
    {
        // Implement database loading logic
        // This is a placeholder
        return new List<Category>();
    }
}

// Data models
public class CatalogData
{
    public CompanyInfo Company { get; set; }
    public Catalog Catalog { get; set; }
    public List<Category> Categories { get; set; } = new List<Category>();
}

public class CompanyInfo
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public string Phone { get; set; }
    public string Website { get; set; }
}

public class Catalog
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Year { get; set; }
}

public class Category
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Product FeaturedProduct { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
}

public class Product
{
    public string Name { get; set; }
    public string Sku { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public decimal? OldPrice { get; set; }
    public string Badge { get; set; }
    public string BadgeClass { get; set; }
    public List<string> Features { get; set; }
    public List<Specification> Specs { get; set; }
    public string StockStatus { get; set; }
    public string StockClass { get; set; }
}

public class Specification
{
    public string Label { get; set; }
    public string Value { get; set; }
}

public class CatalogConfiguration
{
    public CompanyInfo Company { get; set; }
    public Catalog Catalog { get; set; }
    public List<string> IncludeCategories { get; set; }
    public bool IncludeFeaturedProducts { get; set; } = true;
    public bool IncludeSpecifications { get; set; } = true;
}
```

---

## Usage Example

```csharp
// Create catalog data
var catalogData = new CatalogData
{
    Company = new CompanyInfo
    {
        Name = "Acme Electronics",
        Logo = "./images/acme-logo.png",
        Phone = "(555) 123-4567",
        Website = "www.acme-electronics.com"
    },
    Catalog = new Catalog
    {
        Title = "Product Catalog",
        Subtitle = "Premium Electronics & Accessories",
        Year = "2025"
    },
    Categories = new List<Category>
    {
        new Category
        {
            Name = "Laptops & Computers",
            Description = "High-performance laptops and desktop computers for work and play",
            FeaturedProduct = new Product
            {
                Name = "UltraBook Pro 15",
                Sku = "LAP-UBP-15",
                Description = "Premium ultrabook with stunning 4K display, Intel Core i9 processor, and all-day battery life. Perfect for professionals who demand the best.",
                Image = "./images/products/ultrabook-pro-15.jpg",
                Price = 1899.99m,
                OldPrice = 2199.99m,
                Badge = "SALE",
                BadgeClass = "sale",
                Features = new List<string>
                {
                    "15.6\" 4K OLED Display",
                    "Intel Core i9-13900H",
                    "32GB RAM, 1TB SSD",
                    "NVIDIA GeForce RTX 4060",
                    "16-hour battery life"
                }
            },
            Products = new List<Product>
            {
                new Product
                {
                    Name = "ProBook 14",
                    Sku = "LAP-PB-14",
                    Description = "Lightweight business laptop with excellent performance and security features.",
                    Image = "./images/products/probook-14.jpg",
                    Price = 1299.99m,
                    Badge = "NEW",
                    BadgeClass = "new",
                    Features = new List<string>
                    {
                        "14\" Full HD Display",
                        "Intel Core i7-13700H",
                        "16GB RAM, 512GB SSD"
                    },
                    Specs = new List<Specification>
                    {
                        new Specification { Label = "Processor", Value = "Intel Core i7-13700H" },
                        new Specification { Label = "RAM", Value = "16GB DDR5" },
                        new Specification { Label = "Storage", Value = "512GB NVMe SSD" },
                        new Specification { Label = "Weight", Value = "2.9 lbs" }
                    },
                    StockStatus = "In Stock",
                    StockClass = "available"
                },
                new Product
                {
                    Name = "Student Laptop 13",
                    Sku = "LAP-STU-13",
                    Description = "Affordable and reliable laptop perfect for students and everyday computing.",
                    Image = "./images/products/student-13.jpg",
                    Price = 599.99m,
                    Features = new List<string>
                    {
                        "13.3\" HD Display",
                        "Intel Core i5",
                        "8GB RAM, 256GB SSD"
                    },
                    StockStatus = "Low Stock",
                    StockClass = "low"
                }
            }
        },
        new Category
        {
            Name = "Accessories",
            Description = "Premium accessories to enhance your computing experience",
            Products = new List<Product>
            {
                new Product
                {
                    Name = "Wireless Mouse Pro",
                    Sku = "ACC-WMP-01",
                    Description = "Ergonomic wireless mouse with precision tracking and programmable buttons.",
                    Image = "./images/products/mouse-pro.jpg",
                    Price = 79.99m,
                    Features = new List<string>
                    {
                        "Ergonomic design",
                        "6 programmable buttons",
                        "16,000 DPI sensor"
                    },
                    StockStatus = "In Stock",
                    StockClass = "available"
                },
                new Product
                {
                    Name = "Mechanical Keyboard RGB",
                    Sku = "ACC-KBD-RGB",
                    Description = "Premium mechanical keyboard with customizable RGB lighting.",
                    Image = "./images/products/keyboard-rgb.jpg",
                    Price = 149.99m,
                    Badge = "POPULAR",
                    BadgeClass = "new",
                    Features = new List<string>
                    {
                        "Cherry MX switches",
                        "RGB per-key lighting",
                        "Aluminum frame"
                    },
                    StockStatus = "In Stock",
                    StockClass = "available"
                }
            }
        }
    }
};

// Generate catalog
var generator = new CatalogGenerator("catalog-template.html");

using (var output = new FileStream("product-catalog-2025.pdf", FileMode.Create))
{
    generator.GenerateCatalog(catalogData, output);
    Console.WriteLine("Catalog generated successfully!");
    Console.WriteLine($"Total products: {catalogData.Categories.Sum(c => c.Products.Count)}");
}
```

---

## Try It Yourself

### Exercise 1: Custom Layout

Design your own catalog layout:
- 3-column grid for small items
- Full-page spreads for featured products
- Mixed layouts within categories
- Test with different product counts

### Exercise 2: Dynamic Catalog

Build a database-driven catalog:
- Load products from database
- Filter by category and availability
- Sort by price or popularity
- Generate on demand

### Exercise 3: Multi-Format

Create multiple catalog formats:
- Quick reference (product list only)
- Detailed catalog (full specifications)
- Price list (pricing table)
- Digital vs. print versions

---

## Best Practices

1. **Image Optimization** - Compress images before adding to catalog
2. **Consistent Sizing** - Use standard image dimensions
3. **Grid Layouts** - Use CSS Grid for flexible layouts
4. **Page Breaks** - Prevent products from splitting across pages
5. **Category Organization** - Group related products
6. **Clear Pricing** - Make prices prominent and consistent
7. **Stock Status** - Include availability information
8. **Professional Design** - Use consistent colors and spacing

---

## Next Steps

1. **[Form Template](07_form_template.md)** - Print-and-fill forms
2. **[Multi-Language & Branded Documents](08_multi_language_branded.md)** - Enterprise templates
3. **[Invoice Template](01_invoice_template.md)** - Review invoicing

---

**Continue learning →** [Form Template](07_form_template.md)
