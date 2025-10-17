---
layout: default
title: data-img
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-img, @data-img-data, @data-img-type : Image Data Attributes

The `data-img`, `data-img-data`, and `data-img-type` attributes provide alternative methods for loading images in Scryber PDF documents. These attributes enable binding image data directly from memory, databases, or APIs without requiring file system access, supporting dynamic binary image data with explicit MIME type specification.

## Usage

These attributes provide programmatic image loading:
- `data-img` - Binds directly to an ImageData object (binding only)
- `data-img-data` - Accepts binary image data as a byte array (binding only)
- `data-img-type` - Specifies the MIME type for binary image data
- Used when images come from databases, APIs, or memory
- Bypasses file system and URL loading
- Supports all standard image formats (JPEG, PNG, GIF)
- Complements the standard `src` attribute
- Required MIME type specification for proper image decoding
- Enables dynamic image generation and manipulation

```html
<!-- Bind to ImageData object -->
<img data-img="{{model.imageData}}" width="200pt" height="150pt" />

<!-- Binary data with MIME type -->
<img data-img-data="{{model.photoBytes}}"
     data-img-type="image/jpeg"
     width="300pt" height="200pt" />

<!-- PNG image from database -->
<img data-img-data="{{model.logoBytes}}"
     data-img-type="image/png"
     width="150pt" height="75pt"
     alt="Company Logo" />

<!-- Dynamic image with fallback -->
<img src="fallback.jpg"
     data-img-data="{{model.dynamicImage}}"
     data-img-type="image/png"
     width="400pt" height="300pt" />
```

---

## Supported Elements

These attributes are used exclusively with:

### Image Element
- `<img>` - Displays images in PDF documents (primary use)

---

## Binding Values

These attributes are designed specifically for data binding scenarios:

### data-img Binding

```html
<!-- Bind to ImageData object -->
<!-- Model: { logo: ImageData object } -->
<img data-img="{{model.logo}}" width="100pt" height="50pt" />

<!-- Collection of ImageData objects -->
<!-- Model: { images: [ImageData, ImageData, ...] } -->
<template data-bind="{{model.images}}">
    <img data-img="{{.}}" width="200pt" height="150pt"
         style="margin: 5pt;" />
</template>
```

### data-img-data and data-img-type Binding

```html
<!-- Simple binary binding -->
<!-- Model: { photoBytes: byte[], photoType: "image/jpeg" } -->
<img data-img-data="{{model.photoBytes}}"
     data-img-type="{{model.photoType}}"
     width="200pt" height="150pt" />

<!-- Static MIME type -->
<img data-img-data="{{model.pngData}}"
     data-img-type="image/png"
     width="150pt" height="100pt" />

<!-- Multiple images from database -->
<!-- Model: { photos: [{ data: byte[], type: "image/jpeg" }, ...] } -->
<template data-bind="{{model.photos}}">
    <img data-img-data="{{.data}}"
         data-img-type="{{.type}}"
         width="150pt" height="150pt"
         alt="{{.caption}}"
         style="margin: 5pt; border: 1pt solid #ddd;" />
</template>

<!-- Conditional binary or URL loading -->
<img src="{{model.useDefault ? 'default.jpg' : ''}}"
     data-img-data="{{model.useDefault ? null : model.customImageBytes}}"
     data-img-type="image/jpeg"
     width="200pt" height="150pt" />
```

**Data Model Example:**
```json
{
  "photoBytes": "<byte array>",
  "photoType": "image/jpeg",
  "pngData": "<byte array>",
  "photos": [
    {
      "data": "<byte array>",
      "type": "image/jpeg",
      "caption": "Photo 1"
    },
    {
      "data": "<byte array>",
      "type": "image/png",
      "caption": "Photo 2"
    }
  ],
  "useDefault": false,
  "customImageBytes": "<byte array>"
}
```

---

## Notes

### Attribute Purposes

**`data-img`** (Binding Only):
- Binds to a complete `ImageData` object
- ImageData contains all image information (dimensions, format, binary data)
- Most direct approach when working with Scryber's image infrastructure
- BindingOnly attribute - only works in data binding expressions

**`data-img-data`** (Binding Only):
- Accepts raw binary image data as byte array
- Requires `data-img-type` to identify image format
- Used for images from databases, APIs, or in-memory generation
- BindingOnly attribute - only works in data binding expressions

**`data-img-type`**:
- Specifies MIME type for binary image data
- Required when using `data-img-data`
- Can be static or data-bound
- Supports: `image/jpeg`, `image/png`, `image/gif`

### Loading Priority

When multiple image source attributes are present:

1. **data-img-data** with **data-img-type** - Highest priority
2. **data-img** - Second priority
3. **src** attribute - Fallback if above are not present

```html
<!-- data-img-data takes precedence -->
<img src="fallback.jpg"
     data-img-data="{{model.bytes}}"
     data-img-type="image/png"
     width="200pt" height="150pt" />
```

### Supported MIME Types

The `data-img-type` attribute accepts standard image MIME types:

| MIME Type | Format | Description |
|-----------|--------|-------------|
| `image/jpeg` | JPEG/JPG | Compressed photos, no transparency |
| `image/png` | PNG | Lossless compression, supports transparency |
| `image/gif` | GIF | Supports transparency and animation |
| `image/bmp` | BMP | Uncompressed bitmap (may work) |
| `image/tiff` | TIFF | High-quality images (may work) |

```html
<!-- JPEG image -->
<img data-img-data="{{model.jpegBytes}}"
     data-img-type="image/jpeg"
     width="300pt" height="200pt" />

<!-- PNG with transparency -->
<img data-img-data="{{model.pngBytes}}"
     data-img-type="image/png"
     width="150pt" height="150pt" />

<!-- GIF image -->
<img data-img-data="{{model.gifBytes}}"
     data-img-type="image/gif"
     width="100pt" height="100pt" />
```

### Binary Data Sources

Common sources for binary image data:

1. **Database BLOBs**: Images stored in database binary columns
2. **APIs**: Image data returned from REST/SOAP services
3. **File Uploads**: User-uploaded images from web forms
4. **Dynamic Generation**: Images created at runtime (charts, QR codes)
5. **Memory Caching**: Pre-loaded images cached in memory
6. **Image Processing**: Modified/transformed images

```html
<!-- From database -->
<img data-img-data="{{model.userPhoto}}"
     data-img-type="image/jpeg"
     width="100pt" height="100pt" />

<!-- From API response -->
<img data-img-data="{{model.apiImageData}}"
     data-img-type="{{model.apiImageType}}"
     width="200pt" height="150pt" />

<!-- Dynamically generated QR code -->
<img data-img-data="{{model.qrCodeBytes}}"
     data-img-type="image/png"
     width="80pt" height="80pt" />
```

### Error Handling

When binary image data cannot be loaded:

**Missing MIME Type**:
```html
<!-- This will fail - no MIME type specified -->
<img data-img-data="{{model.bytes}}" width="200pt" />
```

**Strict Mode**: Throws exception, stops generation
**Lax Mode**: Logs warning, continues without image

**Invalid Binary Data**:
- Corrupt or invalid image bytes
- MIME type doesn't match actual data format
- Empty or null byte array

```html
<!-- Proper error handling -->
<img data-img-data="{{model.bytes}}"
     data-img-type="image/jpeg"
     data-allow-missing-images="true"
     alt="User Photo"
     width="150pt" height="150pt" />
```

### Performance Considerations

Binary image loading:

**Advantages**:
- No file system I/O required
- No network requests
- Faster for pre-loaded images
- Enables dynamic image generation

**Disadvantages**:
- Images remain in memory
- Larger memory footprint
- No automatic caching by path
- Binary data must be included in model

**Best Practices**:
```html
<!-- Good: Small images from database -->
<img data-img-data="{{model.thumbnailBytes}}"
     data-img-type="image/jpeg"
     width="100pt" height="75pt" />

<!-- Consider file path for large images -->
<!-- Instead of loading 5MB image into memory: -->
<img src="{{model.largeImagePath}}" width="600pt" height="400pt" />
```

### Memory Management

For large documents with many binary images:

1. **Stream When Possible**: Use file paths instead of loading all images into memory
2. **Lazy Loading**: Load images just before rendering
3. **Image Sizing**: Resize images before converting to byte arrays
4. **Compression**: Use JPEG for photos to reduce size
5. **Cleanup**: Clear image data after PDF generation

### ImageData Object Structure

The `data-img` attribute expects an `ImageData` object:

```csharp
// C# Example creating ImageData
var imageData = new ImageData(width, height, imageFormat);
imageData.SetData(byteArray);

// Pass to model
model.ImageData = imageData;
```

```html
<!-- Use in template -->
<img data-img="{{model.imageData}}" width="200pt" height="150pt" />
```

### Combining with Standard Attributes

Binary image attributes work with all standard image attributes:

```html
<!-- Full featured binary image -->
<img data-img-data="{{model.imageBytes}}"
     data-img-type="image/png"
     width="300pt"
     height="200pt"
     alt="Product Photo"
     title="Product Details"
     class="product-image"
     style="border: 1pt solid #ddd; padding: 5pt;"
     data-allow-missing-images="true" />
```

### Dynamic MIME Type Determination

MIME type can be determined at runtime:

```html
<!-- Model determines type based on data -->
<!-- Model: { imageData: byte[], imageFormat: "jpeg|png|gif" } -->

<img data-img-data="{{model.imageData}}"
     data-img-type="image/{{model.imageFormat}}"
     width="200pt" height="150pt" />
```

### Base64 Data URIs vs Binary Data

Comparison of approaches:

**Base64 in src attribute**:
```html
<img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA..."
     width="100pt" height="100pt" />
```

**Binary data with data-img-data**:
```html
<img data-img-data="{{model.bytes}}"
     data-img-type="image/png"
     width="100pt" height="100pt" />
```

**Recommendations**:
- Use `src` with data URI for small, embedded images
- Use `data-img-data` for dynamic, database-driven images
- Use `src` with file path for large, static images

### Security Considerations

When handling binary image data:

1. **Validation**: Verify image data is valid before rendering
2. **Size Limits**: Enforce maximum image size limits
3. **Type Verification**: Ensure MIME type matches actual data
4. **Sanitization**: Validate user-uploaded images
5. **Memory Limits**: Prevent memory exhaustion from large images

```html
<!-- Validated and size-limited image -->
<img data-img-data="{{model.validatedImageBytes}}"
     data-img-type="{{model.verifiedMimeType}}"
     width="{{model.maxWidth}}pt"
     height="{{model.maxHeight}}pt"
     data-allow-missing-images="true" />
```

### Image Factory Processing

The `data-img-data` attribute uses Scryber's image factory system:

1. **Factory Lookup**: System finds factory matching MIME type
2. **Data Validation**: Factory validates binary data
3. **Image Loading**: Binary data is decoded into ImageData
4. **Error Handling**: Invalid data triggers error handling
5. **Rendering**: Valid ImageData is rendered to PDF

If no factory matches the MIME type, an error is raised.

---

## Examples

### Basic Binary Image Loading

```html
<!-- JPEG from byte array -->
<img data-img-data="{{model.jpegBytes}}"
     data-img-type="image/jpeg"
     width="300pt" height="200pt" />

<!-- PNG from byte array -->
<img data-img-data="{{model.pngBytes}}"
     data-img-type="image/png"
     width="200pt" height="150pt" />

<!-- GIF from byte array -->
<img data-img-data="{{model.gifBytes}}"
     data-img-type="image/gif"
     width="150pt" height="150pt" />
```

### Database-Driven Images

```html
<!-- User profile photo from database -->
<!-- Model: { user: { photoData: byte[], photoType: "image/jpeg" } } -->

<div style="text-align: center; padding: 20pt;">
    <img data-img-data="{{model.user.photoData}}"
         data-img-type="{{model.user.photoType}}"
         width="150pt" height="150pt"
         alt="{{model.user.name}}"
         style="border-radius: 75pt; border: 3pt solid #336699;" />
    <h2>{{model.user.name}}</h2>
    <p>{{model.user.title}}</p>
</div>
```

### Product Images from Database

```html
<!-- Model: { products: [
    { name: "Widget A", imageData: byte[], imageType: "image/jpeg", price: 29.99 },
    { name: "Widget B", imageData: byte[], imageType: "image/png", price: 39.99 }
]} -->

<div>
    <h1>Product Catalog</h1>

    <template data-bind="{{model.products}}">
        <div style="border: 1pt solid #ddd; padding: 15pt; margin-bottom: 20pt;
                    display: flex; align-items: center;">
            <img data-img-data="{{.imageData}}"
                 data-img-type="{{.imageType}}"
                 width="150pt" height="150pt"
                 alt="{{.name}}"
                 style="margin-right: 20pt; border: 1pt solid #eee;" />
            <div>
                <h2 style="margin: 0 0 10pt 0;">{{.name}}</h2>
                <p style="font-size: 18pt; color: #336699; font-weight: bold;">
                    ${{.price}}
                </p>
            </div>
        </div>
    </template>
</div>
```

### Photo Gallery from API

```html
<!-- Model: { gallery: [
    { data: byte[], format: "jpeg", caption: "Sunset" },
    { data: byte[], format: "png", caption: "Mountains" }
]} -->

<div>
    <h2>Photo Gallery</h2>

    <div style="display: flex; flex-wrap: wrap; gap: 10pt;">
        <template data-bind="{{model.gallery}}">
            <div style="text-align: center;">
                <img data-img-data="{{.data}}"
                     data-img-type="image/{{.format}}"
                     width="200pt" height="150pt"
                     alt="{{.caption}}"
                     style="border: 2pt solid #ddd; border-radius: 4pt;" />
                <p style="margin-top: 5pt;">{{.caption}}</p>
            </div>
        </template>
    </div>
</div>
```

### Dynamic QR Code Generation

```html
<!-- Model: { qrCodes: [
    { code: byte[], label: "Product A", url: "https://..." },
    { code: byte[], label: "Product B", url: "https://..." }
]} -->

<div>
    <h2>Product QR Codes</h2>

    <template data-bind="{{model.qrCodes}}">
        <div style="display: inline-block; margin: 15pt; text-align: center;">
            <img data-img-data="{{.code}}"
                 data-img-type="image/png"
                 width="100pt" height="100pt"
                 alt="QR Code for {{.label}}"
                 style="border: 1pt solid #ccc; padding: 5pt;" />
            <p style="margin-top: 5pt; font-weight: bold;">{{.label}}</p>
            <p style="margin: 0; font-size: 8pt; color: #666;">{{.url}}</p>
        </div>
    </template>
</div>
```

### Employee Badge with Photo

```html
<!-- Model: {
    employee: {
        photo: byte[], name: "John Doe",
        id: "E12345", department: "Engineering"
    }
} -->

<div style="width: 300pt; border: 2pt solid #336699; padding: 20pt;
            text-align: center; background-color: #f9f9f9;">
    <img data-img-data="{{model.employee.photo}}"
         data-img-type="image/jpeg"
         width="150pt" height="180pt"
         alt="{{model.employee.name}}"
         style="border: 2pt solid #336699;" />

    <h2 style="margin: 15pt 0 5pt 0;">{{model.employee.name}}</h2>
    <p style="margin: 0; color: #666;">{{model.employee.department}}</p>
    <p style="margin: 10pt 0 0 0; font-family: monospace; font-size: 14pt;">
        ID: {{model.employee.id}}
    </p>
</div>
```

### Chart Images from Rendering Service

```html
<!-- Model: {
    charts: [
        { data: byte[], title: "Sales Trend", type: "image/png" },
        { data: byte[], title: "Regional Distribution", type: "image/png" }
    ]
} -->

<div>
    <h1>Sales Analytics Dashboard</h1>

    <template data-bind="{{model.charts}}">
        <div style="margin: 30pt 0; page-break-inside: avoid;">
            <h2>{{.title}}</h2>
            <div style="text-align: center; padding: 20pt; background-color: #f9f9f9;">
                <img data-img-data="{{.data}}"
                     data-img-type="{{.type}}"
                     width="600pt" height="400pt"
                     alt="{{.title}} Chart"
                     style="border: 1pt solid #ddd;" />
            </div>
        </div>
    </template>
</div>
```

### Image with Fallback Handling

```html
<!-- Model: { userPhoto: byte[] | null, hasPhoto: boolean } -->

<div>
    <h2>User Profile</h2>

    <!-- Try binary data first, fallback to default image -->
    <img src="{{model.hasPhoto ? '' : 'images/default-avatar.png'}}"
         data-img-data="{{model.hasPhoto ? model.userPhoto : null}}"
         data-img-type="image/jpeg"
         width="150pt" height="150pt"
         data-allow-missing-images="true"
         alt="User Photo"
         style="border-radius: 75pt;" />
</div>
```

### Medical Imaging Report

```html
<!-- Model: {
    patient: { name: "Jane Smith", id: "P-789" },
    scans: [
        { imageData: byte[], type: "image/jpeg", view: "Frontal", date: "2024-01-15" },
        { imageData: byte[], type: "image/jpeg", view: "Lateral", date: "2024-01-15" }
    ]
} -->

<div>
    <h1>Medical Imaging Report</h1>
    <p>Patient: {{model.patient.name}} ({{model.patient.id}})</p>

    <template data-bind="{{model.scans}}">
        <div style="margin: 30pt 0; page-break-inside: avoid;">
            <h2>{{.view}} View - {{.date}}</h2>
            <div style="text-align: center; padding: 20pt; background-color: #000;">
                <img data-img-data="{{.imageData}}"
                     data-img-type="{{.type}}"
                     width="500pt" height="500pt"
                     alt="{{.view}} scan from {{.date}}" />
            </div>
        </div>
    </template>
</div>
```

### Signature Images from Database

```html
<!-- Model: {
    document: { title: "Service Agreement" },
    signatures: [
        { name: "Client", imageData: byte[], date: "2024-01-10" },
        { name: "Provider", imageData: byte[], date: "2024-01-10" }
    ]
} -->

<div>
    <h1>{{model.document.title}}</h1>
    <p>Agreement content...</p>

    <div style="margin-top: 50pt;">
        <h2>Signatures</h2>

        <template data-bind="{{model.signatures}}">
            <div style="display: inline-block; width: 45%; margin: 10pt;">
                <div style="border-bottom: 1pt solid #000; padding-bottom: 10pt;">
                    <img data-img-data="{{.imageData}}"
                         data-img-type="image/png"
                         width="200pt" height="80pt"
                         alt="{{.name}} Signature" />
                </div>
                <p style="margin: 5pt 0 0 0;">
                    <strong>{{.name}}</strong><br/>
                    Date: {{.date}}
                </p>
            </div>
        </template>
    </div>
</div>
```

### Real Estate Listing with Property Photos

```html
<!-- Model: {
    property: {
        address: "123 Main St",
        price: "$450,000",
        photos: [
            { data: byte[], type: "image/jpeg", caption: "Front View" },
            { data: byte[], type: "image/jpeg", caption: "Living Room" },
            { data: byte[], type: "image/jpeg", caption: "Kitchen" },
            { data: byte[], type: "image/jpeg", caption: "Master Bedroom" }
        ]
    }
} -->

<div>
    <h1>{{model.property.address}}</h1>
    <h2 style="color: #336699;">{{model.property.price}}</h2>

    <div style="display: grid; grid-template-columns: repeat(2, 1fr); gap: 15pt;">
        <template data-bind="{{model.property.photos}}">
            <div>
                <img data-img-data="{{.data}}"
                     data-img-type="{{.type}}"
                     width="280pt" height="210pt"
                     alt="{{.caption}}"
                     style="width: 100%; border: 1pt solid #ddd;" />
                <p style="margin: 5pt 0; text-align: center; font-weight: bold;">
                    {{.caption}}
                </p>
            </div>
        </template>
    </div>
</div>
```

### Dynamic Thumbnail Generation

```html
<!-- Model: {
    documents: [
        { name: "Report.pdf", thumbnail: byte[], pageCount: 15 },
        { name: "Analysis.pdf", thumbnail: byte[], pageCount: 8 }
    ]
} -->

<div>
    <h2>Document Library</h2>

    <template data-bind="{{model.documents}}">
        <div style="display: inline-block; margin: 10pt; width: 150pt;
                    text-align: center; border: 1pt solid #ddd; padding: 10pt;">
            <img data-img-data="{{.thumbnail}}"
                 data-img-type="image/png"
                 width="130pt" height="150pt"
                 alt="Thumbnail of {{.name}}"
                 style="border: 1pt solid #ccc; background-color: #f9f9f9;" />
            <p style="margin: 10pt 0 5pt 0; font-weight: bold;">{{.name}}</p>
            <p style="margin: 0; font-size: 9pt; color: #666;">
                {{.pageCount}} pages
            </p>
        </div>
    </template>
</div>
```

### Barcode Labels

```html
<!-- Model: {
    products: [
        { sku: "ABC123", barcode: byte[], name: "Widget A", price: 29.99 },
        { sku: "DEF456", barcode: byte[], name: "Widget B", price: 39.99 }
    ]
} -->

<div>
    <h2>Product Labels</h2>

    <template data-bind="{{model.products}}">
        <div style="display: inline-block; width: 200pt; height: 150pt;
                    margin: 10pt; border: 1pt solid #000; padding: 10pt;">
            <div style="text-align: center;">
                <strong>{{.name}}</strong>
            </div>
            <div style="text-align: center; margin: 10pt 0;">
                <img data-img-data="{{.barcode}}"
                     data-img-type="image/png"
                     width="150pt" height="60pt"
                     alt="Barcode for {{.sku}}" />
            </div>
            <div style="text-align: center;">
                <p style="margin: 0; font-family: monospace;">{{.sku}}</p>
                <p style="margin: 5pt 0 0 0; font-size: 16pt; font-weight: bold;">
                    ${{.price}}
                </p>
            </div>
        </div>
    </template>
</div>
```

### Image Comparison Layout

```html
<!-- Model: {
    comparison: {
        before: { data: byte[], type: "image/jpeg" },
        after: { data: byte[], type: "image/jpeg" }
    }
} -->

<div>
    <h2>Before and After Comparison</h2>

    <div style="display: flex; justify-content: space-around; margin: 20pt 0;">
        <div style="text-align: center;">
            <img data-img-data="{{model.comparison.before.data}}"
                 data-img-type="{{model.comparison.before.type}}"
                 width="280pt" height="210pt"
                 alt="Before"
                 style="border: 2pt solid #ccc;" />
            <p style="font-weight: bold; margin-top: 10pt; color: #666;">Before</p>
        </div>

        <div style="text-align: center;">
            <img data-img-data="{{model.comparison.after.data}}"
                 data-img-type="{{model.comparison.after.type}}"
                 width="280pt" height="210pt"
                 alt="After"
                 style="border: 2pt solid #28a745;" />
            <p style="font-weight: bold; margin-top: 10pt; color: #28a745;">After</p>
        </div>
    </div>
</div>
```

### ID Card with Embedded Photo

```html
<!-- Model: {
    card: {
        photo: byte[], name: "Jane Smith", id: "ID-456789",
        department: "Research", issued: "2024-01-01", expires: "2025-01-01"
    }
} -->

<div style="width: 400pt; height: 250pt; border: 3pt solid #336699;
            border-radius: 10pt; padding: 20pt; background-color: #f0f8ff;">
    <div style="display: flex;">
        <img data-img-data="{{model.card.photo}}"
             data-img-type="image/jpeg"
             width="120pt" height="150pt"
             alt="{{model.card.name}}"
             style="border: 2pt solid #336699; margin-right: 20pt;" />

        <div style="flex: 1;">
            <h1 style="margin: 0 0 10pt 0; color: #336699;">
                {{model.card.name}}
            </h1>
            <p style="margin: 5pt 0; font-size: 12pt;">
                <strong>Department:</strong> {{model.card.department}}
            </p>
            <p style="margin: 5pt 0; font-family: monospace; font-size: 14pt;">
                <strong>ID:</strong> {{model.card.id}}
            </p>
            <hr style="margin: 15pt 0;"/>
            <p style="margin: 5pt 0; font-size: 9pt; color: #666;">
                Issued: {{model.card.issued}}<br/>
                Expires: {{model.card.expires}}
            </p>
        </div>
    </div>
</div>
```

### Image Grid from API

```html
<!-- Model: {
    apiImages: [
        { bytes: byte[], format: "png", id: "img-1" },
        { bytes: byte[], format: "jpeg", id: "img-2" },
        { bytes: byte[], format: "png", id: "img-3" },
        { bytes: byte[], format: "jpeg", id: "img-4" }
    ]
} -->

<div>
    <h2>Image Collection</h2>

    <div style="display: grid; grid-template-columns: repeat(2, 1fr); gap: 15pt;">
        <template data-bind="{{model.apiImages}}">
            <div style="border: 1pt solid #ddd; padding: 10pt; text-align: center;">
                <img data-img-data="{{.bytes}}"
                     data-img-type="image/{{.format}}"
                     width="250pt" height="200pt"
                     alt="Image {{.id}}"
                     style="border: 1pt solid #ccc;" />
                <p style="margin-top: 5pt; font-size: 9pt; color: #666;">
                    {{.id}} ({{.format}})
                </p>
            </div>
        </template>
    </div>
</div>
```

### Dynamic Logo Variations

```html
<!-- Model: {
    logos: {
        color: byte[], grayscale: byte[], inverted: byte[]
    }
} -->

<div>
    <h2>Logo Variations</h2>

    <div style="text-align: center;">
        <div style="display: inline-block; margin: 15pt;">
            <img data-img-data="{{model.logos.color}}"
                 data-img-type="image/png"
                 width="150pt" height="75pt"
                 alt="Color Logo"
                 style="border: 1pt solid #ddd; padding: 10pt; background-color: #fff;" />
            <p style="margin-top: 5pt;">Color</p>
        </div>

        <div style="display: inline-block; margin: 15pt;">
            <img data-img-data="{{model.logos.grayscale}}"
                 data-img-type="image/png"
                 width="150pt" height="75pt"
                 alt="Grayscale Logo"
                 style="border: 1pt solid #ddd; padding: 10pt; background-color: #fff;" />
            <p style="margin-top: 5pt;">Grayscale</p>
        </div>

        <div style="display: inline-block; margin: 15pt;">
            <img data-img-data="{{model.logos.inverted}}"
                 data-img-type="image/png"
                 width="150pt" height="75pt"
                 alt="Inverted Logo"
                 style="border: 1pt solid #ddd; padding: 10pt; background-color: #000;" />
            <p style="margin-top: 5pt;">Inverted</p>
        </div>
    </div>
</div>
```

### Certificate with Seal Image

```html
<!-- Model: {
    certificate: {
        recipient: "John Doe", course: "Advanced PDF Development",
        date: "2024-01-15", seal: byte[]
    }
} -->

<div style="border: 10pt double #336699; padding: 40pt; text-align: center;">
    <h1 style="font-size: 32pt; margin-bottom: 30pt;">Certificate of Completion</h1>

    <p style="font-size: 16pt; margin: 20pt 0;">This certifies that</p>

    <h2 style="font-size: 28pt; margin: 20pt 0; color: #336699;">
        {{model.certificate.recipient}}
    </h2>

    <p style="font-size: 16pt; margin: 20pt 0;">has successfully completed</p>

    <h3 style="font-size: 20pt; margin: 20pt 0;">
        {{model.certificate.course}}
    </h3>

    <p style="font-size: 14pt; margin: 30pt 0;">
        Date: {{model.certificate.date}}
    </p>

    <div style="margin-top: 40pt;">
        <img data-img-data="{{model.certificate.seal}}"
             data-img-type="image/png"
             width="120pt" height="120pt"
             alt="Official Seal"
             style="opacity: 0.8;" />
    </div>
</div>
```

---

## See Also

- [img](/reference/htmltags/img.html) - Image element (uses data-img attributes)
- [src](/reference/htmlattributes/src.html) - Standard image source attribute
- [data-allow-missing-images](/reference/htmlattributes/data-allow-missing-images.html) - Error handling for missing images
- [width](/reference/htmlattributes/width.html) - Width attribute for sizing
- [height](/reference/htmlattributes/height.html) - Height attribute for sizing
- [alt](/reference/htmlattributes/alt.html) - Alternative text for images
- [Data Binding](/reference/binding/) - Dynamic content and attribute values
- [Image Formats](/reference/images/) - Supported image formats
- [ImageData Class](/reference/api/imagedata.html) - ImageData object reference

---
