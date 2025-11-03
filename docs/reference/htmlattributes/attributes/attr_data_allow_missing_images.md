---
layout: default
title: data-allow-missing-images
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-allow-missing-images : The Missing Images Tolerance Attribute

The `data-allow-missing-images` attribute controls error handling behavior when image files cannot be loaded in Scryber PDF documents. Setting this attribute to `true` enables graceful degradation, allowing document generation to continue even when images are missing, inaccessible, or invalid, instead of throwing exceptions that halt the process.

## Usage

The `data-allow-missing-images` attribute provides error tolerance for images:
- Controls behavior when image files are missing or inaccessible
- Set to `true` to allow missing images without throwing errors
- Set to `false` (default) for strict validation requiring all images
- Maps to the `AllowMissingImages` property on Image component
- Useful for dynamic environments with optional images
- Enables graceful degradation in production documents
- Works with all image loading methods (src, data-img-data, data-img)
- Applies to individual `<img>` elements (per-image control)
- Can be combined with `alt` attribute for missing image text

```html
<!-- Default: Missing image throws error -->
<img src="photo.jpg" width="200pt" height="150pt" />

<!-- Lenient: Missing image allowed, document generation continues -->
<img src="photo.jpg"
     width="200pt" height="150pt"
     data-allow-missing-images="true" />

<!-- With alternative text for missing images -->
<img src="{{model.optionalPhoto}}"
     width="150pt" height="100pt"
     alt="User Photo (if available)"
     data-allow-missing-images="true" />

<!-- Dynamic image with error tolerance -->
<img src="{{model.imagePath}}"
     width="300pt" height="200pt"
     data-allow-missing-images="true"
     style="border: 1pt solid #ddd;" />
```

---

## Supported Elements

The `data-allow-missing-images` attribute is used exclusively with:

### Image Element
- `<img>` - Displays images in PDF documents (primary use)

---

## Binding Values

The `data-allow-missing-images` attribute supports data binding for dynamic error handling:

```html
<!-- Static value -->
<img src="optional-image.jpg"
     width="200pt" height="150pt"
     data-allow-missing-images="true" />

<!-- Dynamic value from model -->
<img src="{{model.imagePath}}"
     width="200pt" height="150pt"
     data-allow-missing-images="{{model.allowMissingImages}}" />

<!-- Conditional tolerance -->
<img src="{{model.imagePath}}"
     width="200pt" height="150pt"
     data-allow-missing-images="{{model.isOptional}}" />

<!-- Different tolerance for different images -->
<template data-bind="{{model.photos}}">
    <img src="{{.url}}"
         width="150pt" height="150pt"
         data-allow-missing-images="{{.optional}}"
         alt="{{.caption}}" />
</template>
```

**Data Model Example:**
```json
{
  "imagePath": "photos/user-photo.jpg",
  "allowMissingImages": true,
  "isOptional": true,
  "photos": [
    {
      "url": "photo1.jpg",
      "optional": false,
      "caption": "Required photo"
    },
    {
      "url": "photo2.jpg",
      "optional": true,
      "caption": "Optional photo"
    }
  ]
}
```

---

## Notes

### Default Behavior (Strict Mode)

By default, when `data-allow-missing-images` is not set or is `false`:

**Missing Image File**:
- Exception is thrown immediately
- Document generation stops
- Error message identifies the missing file
- Stack trace available for debugging

```html
<!-- Strict mode: Error thrown if photo.jpg doesn't exist -->
<img src="photo.jpg" width="200pt" height="150pt" />

<!-- Equivalent explicit strict mode -->
<img src="photo.jpg"
     width="200pt" height="150pt"
     data-allow-missing-images="false" />
```

**Benefits of Strict Mode**:
- Ensures document integrity
- Catches missing files during development
- Prevents incomplete documents in production
- Clear error messages for troubleshooting

### Lenient Behavior (Allow Missing)

When `data-allow-missing-images="true"`:

**Missing Image File**:
- No exception is thrown
- Document generation continues
- Image space may be empty or show placeholder
- Error is logged to trace output (if logging enabled)
- Alternative text (if provided) may be shown

```html
<!-- Lenient mode: Continue if photo.jpg doesn't exist -->
<img src="photo.jpg"
     width="200pt" height="150pt"
     data-allow-missing-images="true"
     alt="User Photo" />
```

**Benefits of Lenient Mode**:
- Enables optional images
- Graceful degradation for user-uploaded content
- Robust document generation in dynamic environments
- Suitable for variable image availability

### When to Use Allow Missing Images

**Use `data-allow-missing-images="true"` for**:

1. **User-Generated Content**: Images uploaded by users that may not always be present
   ```html
   <img src="{{model.userAvatar}}"
        width="100pt" height="100pt"
        data-allow-missing-images="true"
        alt="User Avatar" />
   ```

2. **Optional Images**: Images that enhance but aren't critical to the document
   ```html
   <img src="{{model.optionalLogo}}"
        width="150pt" height="50pt"
        data-allow-missing-images="true" />
   ```

3. **Dynamic Content**: Images from external sources that may be temporarily unavailable
   ```html
   <img src="https://api.example.com/image/{{model.imageId}}"
        width="200pt" height="150pt"
        data-allow-missing-images="true" />
   ```

4. **Template Reuse**: Shared templates used with varying data sources
   ```html
   <img src="{{model.productImage}}"
        width="200pt" height="150pt"
        data-allow-missing-images="true"
        alt="Product Image" />
   ```

5. **Beta/Development Features**: New image sources being tested
   ```html
   <img src="{{model.experimentalFeatureImage}}"
        width="200pt" height="150pt"
        data-allow-missing-images="true" />
   ```

**Use default strict mode (false) for**:

1. **Critical Images**: Logos, headers, required branding
2. **Static Assets**: Images that should always exist
3. **Development**: To catch missing files early
4. **Quality Control**: Ensuring complete documents

### Error Handling Flow

When image loading fails with `data-allow-missing-images="true"`:

1. **File Access Attempt**: System tries to load image from src/data-img-data
2. **Failure Detection**: File not found, network error, or invalid format
3. **Error Logging**: Error details logged to trace output (if enabled)
4. **Graceful Continuation**: No exception thrown
5. **Rendering**: Document continues without the image
6. **Alternative Content**: Alt text or empty space may appear

```html
<!-- With logging enabled, missing image logs warning but continues -->
<img src="missing-file.jpg"
     width="200pt" height="150pt"
     data-allow-missing-images="true"
     alt="This image is missing" />
```

### Interaction with Conformance Mode

The attribute works in conjunction with parser conformance mode:

**Strict Conformance + data-allow-missing-images="false"** (default):
- Missing images throw exceptions
- Document generation stops immediately

**Strict Conformance + data-allow-missing-images="true"**:
- Missing images are allowed
- Document generation continues
- Per-image override of strict conformance

**Lax Conformance + data-allow-missing-images="false"**:
- Missing images may log errors but might not stop generation
- Behavior depends on implementation

**Lax Conformance + data-allow-missing-images="true"**:
- Missing images definitely allowed
- Explicit confirmation of lenient behavior

### Binary Data with Allow Missing

The attribute works with all image loading methods:

```html
<!-- Missing binary data allowed -->
<img data-img-data="{{model.imageBytes}}"
     data-img-type="image/jpeg"
     width="200pt" height="150pt"
     data-allow-missing-images="true" />

<!-- Missing ImageData object allowed -->
<img data-img="{{model.imageData}}"
     width="200pt" height="150pt"
     data-allow-missing-images="true" />
```

### Combining with Alternative Text

Use `alt` attribute to provide text when images are missing:

```html
<!-- Descriptive alternative text -->
<img src="{{model.userPhoto}}"
     width="150pt" height="150pt"
     alt="User photo for {{model.userName}}"
     data-allow-missing-images="true" />

<!-- Informative message -->
<img src="{{model.chartImage}}"
     width="400pt" height="300pt"
     alt="Sales chart - data visualization not available"
     data-allow-missing-images="true" />
```

The `alt` text may be displayed or used for accessibility when the image is missing.

### Performance Considerations

Allowing missing images affects performance:

**Network Images**:
```html
<!-- May cause timeout waits for unreachable URLs -->
<img src="https://slow-server.com/image.jpg"
     width="200pt" height="150pt"
     data-allow-missing-images="true" />
```

Consider timeout settings for network images when using lenient mode.

**Multiple Optional Images**:
```html
<!-- Each attempt takes time, even when failing gracefully -->
<template data-bind="{{model.optionalImages}}">
    <img src="{{.url}}"
         width="100pt" height="100pt"
         data-allow-missing-images="true" />
</template>
```

Pre-validate image availability when possible to improve performance.

### Best Practices

1. **Be Explicit**: Always specify the attribute for clarity
   ```html
   <!-- Good: Explicit intention -->
   <img src="logo.jpg" width="100pt" data-allow-missing-images="false" />
   <img src="optional.jpg" width="100pt" data-allow-missing-images="true" />
   ```

2. **Provide Alt Text**: Always include alt text for missing images
   ```html
   <img src="{{model.photo}}"
        width="150pt" height="150pt"
        alt="Photo of {{model.name}}"
        data-allow-missing-images="true" />
   ```

3. **Log and Monitor**: Enable logging to track missing images
   ```csharp
   // Enable trace logging to see missing image warnings
   document.TraceLog.SetRecording(TraceRecordLevel.Verbose);
   ```

4. **Validate in Development**: Use strict mode during development
   ```html
   <!-- Development: Catch issues early -->
   <img src="{{model.image}}" width="200pt" height="150pt" />

   <!-- Production: Allow graceful degradation -->
   <img src="{{model.image}}" width="200pt" height="150pt"
        data-allow-missing-images="true" />
   ```

5. **Pre-Check Availability**: Validate image existence before document generation
   ```csharp
   // Pseudo-code: Check before generating
   if (File.Exists(model.ImagePath)) {
       model.ImageAvailable = true;
   }
   ```

6. **Fallback Images**: Provide fallback for missing images
   ```html
   <!-- If userPhoto missing, use default avatar -->
   <img src="{{model.userPhoto ?? 'images/default-avatar.png'}}"
        width="100pt" height="100pt"
        data-allow-missing-images="true" />
   ```

### Debugging Missing Images

When images fail to load:

**With Logging Enabled**:
```
Warning: Image source 'photos/missing.jpg' could not be loaded.
Error: File not found at path 'C:\path\photos\missing.jpg'
```

**Common Causes**:
1. File doesn't exist at specified path
2. Incorrect file path (typo, wrong directory)
3. File permissions issue
4. Network unavailable (for URLs)
5. Invalid image format
6. Corrupt image file
7. Binary data is null or empty

**Troubleshooting Steps**:
1. Verify file exists at exact path
2. Check file permissions
3. Validate image format
4. Test network connectivity (for URLs)
5. Enable trace logging
6. Use absolute paths for testing

### Environment-Specific Behavior

Different behaviors for different environments:

```html
<!-- Model: { isProduction: true, allowMissing: true } -->

<!-- Development: Strict -->
<img src="{{model.imagePath}}"
     width="200pt" height="150pt"
     data-allow-missing-images="{{model.isProduction && model.allowMissing}}" />

<!-- Production: Lenient -->
```

### Security Considerations

Be cautious with path validation:

```html
<!-- Validate and sanitize user-provided paths -->
<img src="{{model.sanitizedImagePath}}"
     width="200pt" height="150pt"
     data-allow-missing-images="true" />
```

Don't expose file system details in error messages:
- Enable lenient mode to prevent path disclosure
- Log errors securely server-side
- Don't include error details in generated PDFs

---

## Examples

### Basic Missing Image Tolerance

```html
<!-- Image may or may not exist -->
<img src="optional-banner.jpg"
     width="600pt" height="200pt"
     data-allow-missing-images="true" />

<!-- Critical logo - must exist -->
<img src="company-logo.png"
     width="150pt" height="50pt"
     data-allow-missing-images="false" />

<!-- User photo - optional -->
<img src="{{model.userPhoto}}"
     width="100pt" height="100pt"
     alt="User Photo"
     data-allow-missing-images="true" />
```

### User Profile with Optional Avatar

```html
<!-- Model: { user: { name: "John Doe", avatar: "avatars/john.jpg" | null } } -->

<div style="text-align: center; padding: 20pt;">
    <!-- Avatar may not exist for all users -->
    <img src="{{model.user.avatar ?? 'images/default-avatar.png'}}"
         width="150pt" height="150pt"
         alt="{{model.user.name}}"
         data-allow-missing-images="true"
         style="border-radius: 75pt; border: 3pt solid #336699;" />

    <h2>{{model.user.name}}</h2>
</div>
```

### Product Catalog with Variable Images

```html
<!-- Model: { products: [
    { name: "Widget A", image: "products/widget-a.jpg", price: 29.99 },
    { name: "Widget B", image: null, price: 39.99 }  // No image
]} -->

<div>
    <h1>Product Catalog</h1>

    <template data-bind="{{model.products}}">
        <div style="border: 1pt solid #ddd; padding: 15pt; margin-bottom: 20pt;">
            <!-- Some products may not have images -->
            <img src="{{.image ?? 'images/no-image.png'}}"
                 width="200pt" height="200pt"
                 alt="{{.name}}"
                 data-allow-missing-images="true"
                 style="display: block; margin: 0 auto 10pt auto;" />

            <h2 style="text-align: center;">{{.name}}</h2>
            <p style="text-align: center; font-size: 18pt; color: #336699;">
                ${{.price}}
            </p>
        </div>
    </template>
</div>
```

### Report with Optional Charts

```html
<!-- Model: {
    report: { title: "Sales Analysis" },
    charts: [
        { path: "charts/sales-trend.png", available: true },
        { path: "charts/forecast.png", available: false }  // Not generated yet
    ]
} -->

<div>
    <h1>{{model.report.title}}</h1>

    <template data-bind="{{model.charts}}">
        <div style="margin: 30pt 0;">
            <h2>Sales Trend Chart</h2>

            <!-- Chart may not be generated yet -->
            <div style="text-align: center; padding: 20pt; background-color: #f9f9f9;">
                <img src="{{.path}}"
                     width="600pt" height="400pt"
                     alt="Chart not available"
                     data-allow-missing-images="true"
                     style="border: 1pt solid #ddd;" />
            </div>

            <p style="text-align: center; font-style: italic; color: #666;">
                {{.available ? 'Chart generated from latest data' : 'Chart generation in progress'}}
            </p>
        </div>
    </template>
</div>
```

### Employee Directory with Photos

```html
<!-- Model: { employees: [
    { name: "Alice", photo: "photos/alice.jpg", dept: "Engineering" },
    { name: "Bob", photo: null, dept: "Sales" }  // Photo not provided
]} -->

<div>
    <h1>Employee Directory</h1>

    <template data-bind="{{model.employees}}">
        <div style="display: flex; align-items: center; padding: 15pt;
                    border-bottom: 1pt solid #ddd;">
            <!-- Some employees may not have photos -->
            <img src="{{.photo ?? 'images/no-photo.png'}}"
                 width="80pt" height="100pt"
                 alt="{{.name}}"
                 data-allow-missing-images="true"
                 style="border: 1pt solid #ccc; margin-right: 20pt;" />

            <div>
                <h3 style="margin: 0 0 5pt 0;">{{.name}}</h3>
                <p style="margin: 0; color: #666;">{{.dept}}</p>
            </div>
        </div>
    </template>
</div>
```

### Dynamic External Images

```html
<!-- Model: { articles: [
    { title: "Article 1", imageUrl: "https://cdn.example.com/img1.jpg" },
    { title: "Article 2", imageUrl: "https://cdn.example.com/img2.jpg" }
]} -->

<div>
    <h1>News Articles</h1>

    <template data-bind="{{model.articles}}">
        <div style="margin: 20pt 0; padding: 15pt; border: 1pt solid #ddd;">
            <!-- External images may be unavailable -->
            <img src="{{.imageUrl}}"
                 width="100%" height="200pt"
                 alt="{{.title}} image"
                 data-allow-missing-images="true"
                 style="margin-bottom: 10pt; border: 1pt solid #ccc;" />

            <h2>{{.title}}</h2>
            <p>Article content...</p>
        </div>
    </template>
</div>
```

### Conditional Missing Image Handling

```html
<!-- Model: {
    isDevelopment: false,
    isProduction: true,
    images: [...]
} -->

<template data-bind="{{model.images}}">
    <!-- Strict in development, lenient in production -->
    <img src="{{.url}}"
         width="200pt" height="150pt"
         data-allow-missing-images="{{model.isProduction}}"
         alt="{{.description}}" />
</template>
```

### Real Estate Listings with Variable Photos

```html
<!-- Model: { properties: [
    {
        address: "123 Main St",
        photos: ["p1.jpg", "p2.jpg", "p3.jpg", null, null]  // Only 3 photos
    }
]} -->

<div>
    <h1>Property Listings</h1>

    <template data-bind="{{model.properties}}">
        <div style="page-break-inside: avoid; margin: 20pt 0;">
            <h2>{{.address}}</h2>

            <div style="display: grid; grid-template-columns: repeat(3, 1fr); gap: 10pt;">
                <template data-bind="{{.photos}}">
                    <!-- Some photo slots may be empty -->
                    <img src="{{. ?? 'images/no-photo.png'}}"
                         width="180pt" height="135pt"
                         alt="Property photo"
                         data-allow-missing-images="true"
                         style="border: 1pt solid #ddd;" />
                </template>
            </div>
        </div>
    </template>
</div>
```

### Invoice with Optional Logo

```html
<!-- Model: {
    invoice: { number: "INV-123", total: 1250.00 },
    companyLogo: "logos/company.png" | null
} -->

<div>
    <div style="display: flex; justify-content: space-between; align-items: center;
                border-bottom: 2pt solid #336699; padding-bottom: 15pt;">
        <!-- Logo optional for some clients -->
        <img src="{{model.companyLogo ?? 'images/default-logo.png'}}"
             width="150pt" height="50pt"
             alt="Company Logo"
             data-allow-missing-images="true" />

        <div style="text-align: right;">
            <h1 style="margin: 0;">INVOICE</h1>
            <p style="margin: 5pt 0 0 0;">{{model.invoice.number}}</p>
        </div>
    </div>

    <h2>Total: ${{model.invoice.total}}</h2>
</div>
```

### Training Manual with Optional Screenshots

```html
<div>
    <h1>Software Training Manual</h1>

    <h2>Step 1: Login</h2>
    <p>Navigate to the login page and enter your credentials.</p>

    <!-- Screenshot may not be available yet -->
    <div style="text-align: center; padding: 20pt; background-color: #f9f9f9;">
        <img src="screenshots/login-screen.png"
             width="500pt" height="350pt"
             alt="Login screen (screenshot pending)"
             data-allow-missing-images="true"
             style="border: 2pt solid #ddd;" />
        <p style="font-size: 9pt; color: #666; margin-top: 10pt;">
            Figure 1.1: Login Screen
        </p>
    </div>

    <h2>Step 2: Dashboard</h2>
    <p>After logging in, you'll see the main dashboard.</p>

    <!-- Screenshot may not be available yet -->
    <div style="text-align: center; padding: 20pt; background-color: #f9f9f9;">
        <img src="screenshots/dashboard.png"
             width="500pt" height="350pt"
             alt="Dashboard (screenshot pending)"
             data-allow-missing-images="true"
             style="border: 2pt solid #ddd;" />
        <p style="font-size: 9pt; color: #666; margin-top: 10pt;">
            Figure 1.2: Main Dashboard
        </p>
    </div>
</div>
```

### Social Media Style Feed

```html
<!-- Model: { posts: [
    { user: "alice", avatar: "avatars/alice.jpg", content: "Hello!", image: "posts/1.jpg" },
    { user: "bob", avatar: null, content: "Hi!", image: null }
]} -->

<div>
    <h1>Social Feed</h1>

    <template data-bind="{{model.posts}}">
        <div style="border: 1pt solid #ddd; padding: 15pt; margin-bottom: 15pt;
                    background-color: white;">
            <!-- User info -->
            <div style="display: flex; align-items: center; margin-bottom: 10pt;">
                <!-- Avatar may be missing -->
                <img src="{{.avatar ?? 'images/default-avatar.png'}}"
                     width="40pt" height="40pt"
                     alt="{{.user}}"
                     data-allow-missing-images="true"
                     style="border-radius: 20pt; margin-right: 10pt;" />
                <strong>{{.user}}</strong>
            </div>

            <!-- Post content -->
            <p>{{.content}}</p>

            <!-- Post image (optional) -->
            <img src="{{.image}}"
                 width="100%"
                 alt="Post image"
                 data-allow-missing-images="true"
                 hidden="{{.image ? '' : 'hidden'}}"
                 style="margin-top: 10pt; border: 1pt solid #eee;" />
        </div>
    </template>
</div>
```

### Medical Report with Optional Imaging

```html
<!-- Model: {
    patient: { name: "Jane Doe", id: "P-456" },
    scans: [
        { type: "X-Ray", date: "2024-01-15", image: "scans/xray.jpg", available: true },
        { type: "MRI", date: "2024-01-20", image: "scans/mri.jpg", available: false }
    ]
} -->

<div>
    <h1>Medical Imaging Report</h1>
    <p>Patient: {{model.patient.name}} ({{model.patient.id}})</p>

    <template data-bind="{{model.scans}}">
        <div style="margin: 30pt 0; page-break-inside: avoid;">
            <h2>{{.type}} - {{.date}}</h2>

            <!-- Scan image may still be processing -->
            <div style="text-align: center; padding: 30pt; background-color: #000;">
                <img src="{{.image}}"
                     width="500pt" height="500pt"
                     alt="{{.type}} scan from {{.date}}"
                     data-allow-missing-images="true"
                     style="{{.available ? '' : 'filter: grayscale(100%);'}}" />
            </div>

            <p style="text-align: center; margin-top: 10pt; font-style: italic;">
                {{.available ? 'Scan available for review' : 'Scan processing - image not yet available'}}
            </p>
        </div>
    </template>
</div>
```

### Photo Gallery with Lazy Loading

```html
<!-- Model: { gallery: [
    { id: 1, thumbnail: "thumbs/1.jpg", loaded: true },
    { id: 2, thumbnail: "thumbs/2.jpg", loaded: true },
    { id: 3, thumbnail: "thumbs/3.jpg", loaded: false }  // Not loaded yet
]} -->

<div>
    <h2>Photo Gallery</h2>

    <div style="display: grid; grid-template-columns: repeat(4, 1fr); gap: 10pt;">
        <template data-bind="{{model.gallery}}">
            <div style="text-align: center;">
                <!-- Thumbnails may not all be generated yet -->
                <img src="{{.thumbnail}}"
                     width="120pt" height="90pt"
                     alt="Photo {{.id}}"
                     data-allow-missing-images="true"
                     style="border: 2pt solid #ddd; background-color: #f0f0f0;" />
                <p style="margin: 5pt 0 0 0; font-size: 8pt; color: #666;">
                    {{.loaded ? 'Photo ' + .id : 'Loading...'}}
                </p>
            </div>
        </template>
    </div>
</div>
```

### Certificate with Optional Seal

```html
<!-- Model: {
    certificate: {
        recipient: "John Doe",
        course: "Advanced PDF Development",
        date: "2024-01-15",
        seal: "seals/official-seal.png" | null
    }
} -->

<div style="border: 10pt double #336699; padding: 50pt; text-align: center;">
    <h1 style="font-size: 32pt;">Certificate of Completion</h1>

    <p style="font-size: 16pt; margin: 30pt 0;">This certifies that</p>

    <h2 style="font-size: 28pt; color: #336699; margin: 20pt 0;">
        {{model.certificate.recipient}}
    </h2>

    <p style="font-size: 16pt; margin: 30pt 0;">has successfully completed</p>

    <h3 style="font-size: 20pt; margin: 20pt 0;">
        {{model.certificate.course}}
    </h3>

    <p style="font-size: 14pt; margin: 30pt 0;">
        Date: {{model.certificate.date}}
    </p>

    <!-- Official seal may not be available -->
    <div style="margin-top: 50pt;">
        <img src="{{model.certificate.seal ?? 'seals/pending-seal.png'}}"
             width="150pt" height="150pt"
             alt="Official Seal"
             data-allow-missing-images="true"
             style="opacity: 0.8;" />
    </div>
</div>
```

### Product Comparison with Variable Images

```html
<!-- Model: { products: [
    { name: "Basic", image: "products/basic.jpg", price: 99 },
    { name: "Pro", image: null, price: 199 },  // Coming soon, no image
    { name: "Enterprise", image: "products/enterprise.jpg", price: 499 }
]} -->

<div>
    <h1>Product Comparison</h1>

    <table style="width: 100%; border-collapse: collapse;">
        <thead>
            <tr style="background-color: #336699; color: white;">
                <th style="padding: 15pt;">Product</th>
                <th style="padding: 15pt;">Image</th>
                <th style="padding: 15pt;">Price</th>
            </tr>
        </thead>
        <tbody>
            <template data-bind="{{model.products}}">
                <tr style="border-bottom: 1pt solid #ddd;">
                    <td style="padding: 15pt; font-weight: bold;">{{.name}}</td>
                    <td style="padding: 15pt; text-align: center;">
                        <!-- Some products may not have images yet -->
                        <img src="{{.image ?? 'images/coming-soon.png'}}"
                             width="120pt" height="90pt"
                             alt="{{.name}}"
                             data-allow-missing-images="true"
                             style="border: 1pt solid #ddd;" />
                    </td>
                    <td style="padding: 15pt; text-align: right; font-size: 18pt;">
                        ${{.price}}
                    </td>
                </tr>
            </template>
        </tbody>
    </table>
</div>
```

### Event Flyer with Optional Sponsor Logos

```html
<!-- Model: {
    event: { name: "Tech Conference 2024", date: "March 15, 2024" },
    sponsors: [
        { name: "Company A", logo: "sponsors/company-a.png", tier: "Gold" },
        { name: "Company B", logo: null, tier: "Silver" },  // Logo pending
        { name: "Company C", logo: "sponsors/company-c.png", tier: "Bronze" }
    ]
} -->

<div style="text-align: center;">
    <h1 style="font-size: 36pt; color: #336699;">{{model.event.name}}</h1>
    <h2 style="font-size: 24pt; color: #666;">{{model.event.date}}</h2>

    <div style="margin-top: 50pt;">
        <h2>Our Sponsors</h2>

        <div style="display: flex; justify-content: center; flex-wrap: wrap; gap: 30pt;">
            <template data-bind="{{model.sponsors}}">
                <div style="text-align: center; width: 180pt;">
                    <!-- Some sponsor logos may not be provided yet -->
                    <img src="{{.logo ?? 'sponsors/placeholder.png'}}"
                         width="150pt" height="80pt"
                         alt="{{.name}}"
                         data-allow-missing-images="true"
                         style="border: 1pt solid #ddd; padding: 10pt;
                                background-color: white;" />
                    <p style="margin-top: 10pt; font-weight: bold;">{{.name}}</p>
                    <p style="margin: 0; font-size: 9pt; color: #666;">{{.tier}} Sponsor</p>
                </div>
            </template>
        </div>
    </div>
</div>
```

### Newsletter with Optional Header Images

```html
<!-- Model: {
    newsletter: { title: "Monthly Update", issue: "Jan 2024" },
    articles: [
        { title: "Article 1", headerImage: "headers/1.jpg", content: "..." },
        { title: "Article 2", headerImage: null, content: "..." }  // No header
    ]
} -->

<div>
    <h1>{{model.newsletter.title}}</h1>
    <p style="font-size: 12pt; color: #666;">Issue: {{model.newsletter.issue}}</p>

    <template data-bind="{{model.articles}}">
        <div style="margin: 30pt 0; page-break-inside: avoid;">
            <!-- Article header image is optional -->
            <img src="{{.headerImage}}"
                 width="100%" height="200pt"
                 alt="{{.title}} header"
                 data-allow-missing-images="true"
                 hidden="{{.headerImage ? '' : 'hidden'}}"
                 style="margin-bottom: 15pt; border: 1pt solid #ddd;" />

            <h2 style="{{.headerImage ? '' : 'margin-top: 0;'}}">{{.title}}</h2>
            <p>{{.content}}</p>
        </div>
    </template>
</div>
```

### Team Roster with Missing Photos

```html
<!-- Model: { team: [
    { name: "Alice", photo: "team/alice.jpg", role: "Lead", bio: "..." },
    { name: "Bob", photo: null, role: "Developer", bio: "..." },
    { name: "Carol", photo: "team/carol.jpg", role: "Designer", bio: "..." }
]} -->

<div>
    <h1>Meet Our Team</h1>

    <template data-bind="{{model.team}}">
        <div style="display: flex; padding: 20pt; margin: 15pt 0;
                    border: 1pt solid #ddd; background-color: #f9f9f9;">
            <!-- Some team members may not have photos yet -->
            <img src="{{.photo ?? 'images/silhouette.png'}}"
                 width="120pt" height="150pt"
                 alt="{{.name}}"
                 data-allow-missing-images="true"
                 style="border: 2pt solid #336699; margin-right: 20pt;" />

            <div style="flex: 1;">
                <h2 style="margin: 0 0 5pt 0;">{{.name}}</h2>
                <h3 style="margin: 0 0 15pt 0; color: #336699;">{{.role}}</h3>
                <p style="margin: 0;">{{.bio}}</p>
            </div>
        </div>
    </template>
</div>
```

---

## See Also

- [img](/reference/htmltags/img.html) - Image element (uses data-allow-missing-images attribute)
- [src](/reference/htmlattributes/src.html) - Image source attribute
- [data-img](/reference/htmlattributes/data-img.html) - Binary image data attributes
- [alt](/reference/htmlattributes/alt.html) - Alternative text for images
- [width](/reference/htmlattributes/width.html) - Width attribute for sizing
- [height](/reference/htmlattributes/height.html) - Height attribute for sizing
- [Data Binding](/reference/binding/) - Dynamic content and attribute values
- [Error Handling](/reference/errors/) - Error handling and conformance modes
- [Logging](/reference/logging/) - Trace logging configuration

---
