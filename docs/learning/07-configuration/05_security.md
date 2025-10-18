---
layout: default
title: Security
nav_order: 5
parent: Document Configuration
parent_url: /learning/07-configuration/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Security

Master PDF security features including encryption, password protection, and permission controls to protect sensitive documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Add password protection to PDFs
- Configure user and owner passwords
- Set document permissions
- Control printing, copying, and editing
- Understand encryption levels
- Implement security best practices

---

## PDF Security Basics

PDF security provides:
- **Encryption** - Protects document content
- **Password Protection** - Controls who can open the document
- **Permissions** - Controls what users can do with the document

---

## Basic Password Protection

### User Password (Document Open)

```csharp
using Scryber.Components;
using Scryber.PDF;
using System.IO;

var doc = Document.ParseDocument("template.html");

// Require password to open
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    UserPassword = "secretpassword"
};

doc.SaveAsPDF(stream);
```

**User Password:**
- Required to open the document
- Encrypts the content
- Users must enter password to view

### Owner Password (Permissions)

```csharp
// Owner password allows changing permissions
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    OwnerPassword = "adminpassword"
};

// Document can be opened without password
// But permissions can only be changed with owner password
```

### Both Passwords

```csharp
// Most secure: both passwords
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    UserPassword = "viewpassword",      // Required to open
    OwnerPassword = "adminpassword"     // Required to change permissions
};
```

---

## Document Permissions

### Common Permissions

```csharp
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    OwnerPassword = "admin123",

    // Printing permissions
    AllowPrinting = true,
    AllowHighQualityPrinting = true,

    // Content permissions
    AllowCopying = false,
    AllowAccessibility = true,

    // Editing permissions
    AllowAnnotations = false,
    AllowFormFilling = false,
    AllowDocumentAssembly = false,

    // Modification permissions
    AllowModification = false
};
```

### Permission Descriptions

| Permission | Description |
|------------|-------------|
| AllowPrinting | Allow document printing |
| AllowHighQualityPrinting | Allow high-resolution printing |
| AllowCopying | Allow copying text and graphics |
| AllowAccessibility | Allow screen readers (should be true) |
| AllowAnnotations | Allow adding comments and annotations |
| AllowFormFilling | Allow filling form fields |
| AllowDocumentAssembly | Allow inserting/deleting/rotating pages |
| AllowModification | Allow document modification |

---

## Security Profiles

### Read-Only Document

```csharp
// Users can view and print, but not modify
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    OwnerPassword = "admin123",
    AllowPrinting = true,
    AllowCopying = false,
    AllowAnnotations = false,
    AllowFormFilling = false,
    AllowModification = false,
    AllowAccessibility = true  // Important for accessibility
};
```

### Confidential Document

```csharp
// Password required, no printing or copying
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    UserPassword = "confidential",
    OwnerPassword = "admin123",
    AllowPrinting = false,
    AllowCopying = false,
    AllowAnnotations = false,
    AllowAccessibility = true  // Still allow screen readers
};
```

### Form Document

```csharp
// Allow form filling only
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    OwnerPassword = "admin123",
    AllowPrinting = true,
    AllowCopying = false,
    AllowFormFilling = true,
    AllowAnnotations = false,
    AllowModification = false,
    AllowAccessibility = true
};
```

---

## Practical Examples

### Example 1: Secure Invoice Generator

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.IO;

public class SecureInvoiceGenerator
{
    private readonly string _ownerPassword;

    public SecureInvoiceGenerator(string ownerPassword)
    {
        _ownerPassword = ownerPassword;
    }

    public void GenerateInvoice(Invoice invoice, Stream output)
    {
        var doc = Document.ParseDocument("invoice-template.html");

        // Set metadata
        doc.Info.Title = $"Invoice {invoice.Number}";
        doc.Info.Author = invoice.CompanyName;

        // Configure security
        doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
        {
            OwnerPassword = _ownerPassword,

            // Allow printing invoice
            AllowPrinting = true,
            AllowHighQualityPrinting = true,

            // Prevent copying (protect pricing/details)
            AllowCopying = false,

            // Prevent modification
            AllowAnnotations = false,
            AllowModification = false,
            AllowDocumentAssembly = false,

            // Allow accessibility
            AllowAccessibility = true
        };

        // Bind data
        doc.Params["invoice"] = invoice;

        // Generate
        doc.SaveAsPDF(output);

        Console.WriteLine($"Secure invoice {invoice.Number} generated");
    }
}

// Usage
var generator = new SecureInvoiceGenerator("owner-pass-2024");
using (var output = new FileStream("invoice.pdf", FileMode.Create))
{
    generator.GenerateInvoice(invoice, output);
}
```

### Example 2: Confidential Report with Password

```csharp
public class ConfidentialReportGenerator
{
    public void GenerateReport(ReportData data, string password, Stream output)
    {
        var doc = Document.ParseDocument("report-template.html");

        // Set metadata
        doc.Info.Title = $"{data.Classification} Report - {data.Title}";
        doc.Info.Author = data.Department;
        doc.Info.Subject = "Confidential Information";

        // High security configuration
        doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
        {
            // Both passwords required
            UserPassword = password,
            OwnerPassword = GenerateOwnerPassword(data.ReportId),

            // Minimal permissions
            AllowPrinting = false,        // No printing
            AllowCopying = false,         // No copying
            AllowAnnotations = false,     // No annotations
            AllowFormFilling = false,     // No forms
            AllowModification = false,    // No modifications
            AllowDocumentAssembly = false, // No assembly
            AllowAccessibility = true     // Allow screen readers
        };

        // Bind data
        doc.Params["report"] = data;

        // Generate
        doc.SaveAsPDF(output);

        LogSecureDocumentCreation(data.ReportId, data.Classification);
    }

    private string GenerateOwnerPassword(string reportId)
    {
        // Generate secure owner password based on report ID
        return Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes($"owner-{reportId}-{DateTime.Now.Year}")
        );
    }

    private void LogSecureDocumentCreation(string reportId, string classification)
    {
        Console.WriteLine($"[SECURITY] Confidential report generated: {reportId}");
        Console.WriteLine($"[SECURITY] Classification: {classification}");
        Console.WriteLine($"[SECURITY] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
    }
}

// Usage
var generator = new ConfidentialReportGenerator();
using (var output = new FileStream("confidential-report.pdf", FileMode.Create))
{
    generator.GenerateReport(reportData, "SecretPassword123!", output);
}
```

### Example 3: Dynamic Security Based on Classification

```csharp
public class ClassificationBasedGenerator
{
    public void GenerateDocument(
        DocumentData data,
        SecurityClassification classification,
        Stream output)
    {
        var doc = Document.ParseDocument("template.html");

        // Set metadata
        doc.Info.Title = data.Title;
        doc.Info.Subject = $"{classification} Information";

        // Configure security based on classification
        doc.RenderOptions.SecurityOptions = GetSecurityOptions(classification, data.DocumentId);

        // Bind data
        doc.Params["document"] = data;

        // Generate
        doc.SaveAsPDF(output);
    }

    private PDFSecurityOptions GetSecurityOptions(
        SecurityClassification classification,
        string documentId)
    {
        switch (classification)
        {
            case SecurityClassification.Public:
                // No restrictions
                return null;

            case SecurityClassification.Internal:
                // Basic protection
                return new PDFSecurityOptions
                {
                    OwnerPassword = $"internal-{documentId}",
                    AllowPrinting = true,
                    AllowCopying = true,
                    AllowModification = false,
                    AllowAccessibility = true
                };

            case SecurityClassification.Confidential:
                // Medium protection
                return new PDFSecurityOptions
                {
                    UserPassword = GenerateUserPassword(documentId),
                    OwnerPassword = GenerateOwnerPassword(documentId),
                    AllowPrinting = true,
                    AllowCopying = false,
                    AllowModification = false,
                    AllowAnnotations = false,
                    AllowAccessibility = true
                };

            case SecurityClassification.Secret:
                // High protection
                return new PDFSecurityOptions
                {
                    UserPassword = GenerateUserPassword(documentId),
                    OwnerPassword = GenerateOwnerPassword(documentId),
                    AllowPrinting = false,
                    AllowCopying = false,
                    AllowModification = false,
                    AllowAnnotations = false,
                    AllowFormFilling = false,
                    AllowDocumentAssembly = false,
                    AllowAccessibility = true
                };

            default:
                throw new ArgumentException($"Unknown classification: {classification}");
        }
    }

    private string GenerateUserPassword(string documentId)
    {
        // Generate user password (in production, use proper key management)
        return Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes($"view-{documentId}")
        );
    }

    private string GenerateOwnerPassword(string documentId)
    {
        // Generate owner password (in production, use proper key management)
        return Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes($"admin-{documentId}")
        );
    }
}

public enum SecurityClassification
{
    Public,
    Internal,
    Confidential,
    Secret
}
```

---

## Encryption Levels

### 40-bit Encryption (PDF 1.3)

```csharp
// Weak encryption - for legacy compatibility only
// Not recommended for sensitive data
```

### 128-bit Encryption (PDF 1.4+)

```csharp
// Standard encryption - good for most uses
doc.RenderOptions.PDFVersion = PDFVersion.PDF14;
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    UserPassword = "password"
};
```

### 256-bit AES Encryption (PDF 1.7+)

```csharp
// Strong encryption - recommended for sensitive data
doc.RenderOptions.PDFVersion = PDFVersion.PDF17;
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    UserPassword = "strongpassword"
};
```

---

## Security Best Practices

### Password Strength

```csharp
// ❌ Weak passwords
UserPassword = "12345"
UserPassword = "password"

// ✅ Strong passwords
UserPassword = "Tr0ng!P@ssw0rd2024"  // 8+ chars, mixed case, numbers, symbols
```

### Password Management

```csharp
// ❌ Hard-coded passwords
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    OwnerPassword = "admin123"  // Hard-coded!
};

// ✅ Secure password management
var ownerPassword = Configuration["Security:OwnerPassword"];
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    OwnerPassword = ownerPassword
};
```

### Accessibility

```csharp
// ✅ Always allow accessibility
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    AllowAccessibility = true,  // Important for screen readers
    // ... other permissions
};
```

---

## Try It Yourself

### Exercise 1: Security Profiles

Create security profiles for:
- Public documents (no restrictions)
- Internal documents (basic protection)
- Confidential documents (password + restrictions)
- Test each profile

### Exercise 2: Password Generator

Build a password generator that:
- Creates strong passwords
- Stores passwords securely
- Retrieves passwords for verification
- Rotates passwords periodically

### Exercise 3: Permission Tester

Create a tool that:
- Generates PDFs with different permissions
- Tests each permission in PDF viewer
- Documents viewer compatibility
- Verifies security works as expected

---

## Common Pitfalls

### ❌ Weak Passwords

```csharp
// Too simple
UserPassword = "pass"
OwnerPassword = "admin"
```

✅ **Solution:**

```csharp
// Strong passwords
UserPassword = "Tr0ng!Us3rP@ss2024"
OwnerPassword = "Adm!nP@ssw0rd#2024"
```

### ❌ Disabling Accessibility

```csharp
// Blocks screen readers
AllowAccessibility = false
```

✅ **Solution:**

```csharp
// Always allow accessibility
AllowAccessibility = true
```

### ❌ Security with PDF/A

```csharp
// PDF/A doesn't allow encryption
doc.RenderOptions.Conformance = PDFConformance.PDFA2B;
doc.RenderOptions.SecurityOptions = new PDFSecurityOptions
{
    UserPassword = "password"  // ❌ Will fail!
};
```

✅ **Solution:**

```csharp
// No security with PDF/A
doc.RenderOptions.Conformance = PDFConformance.PDFA2B;
doc.RenderOptions.SecurityOptions = null;  // ✅
```

---

## Security Checklist

- [ ] Appropriate security profile chosen
- [ ] Strong passwords used (8+ characters)
- [ ] Owner password set
- [ ] Permissions configured appropriately
- [ ] AllowAccessibility set to true
- [ ] PDF version supports encryption level
- [ ] Passwords stored securely (not hard-coded)
- [ ] Tested in PDF viewers
- [ ] Compliance checked (no encryption with PDF/A)

---

## Best Practices

1. **Strong Passwords** - 8+ characters, mixed case, numbers, symbols
2. **Secure Storage** - Never hard-code passwords
3. **Owner Password** - Always set to protect permissions
4. **Accessibility** - Always allow (set to true)
5. **Test Thoroughly** - Verify in multiple PDF viewers
6. **Document Policy** - Clear security classification system
7. **Audit Trail** - Log security-related operations
8. **Regular Review** - Update passwords periodically

---

## Security Limitations

**PDF Security is NOT:**
- Unbreakable (password-protected PDFs can be cracked)
- DRM (doesn't prevent screenshots)
- Perfect protection (determined users can bypass)

**PDF Security IS:**
- A deterrent for casual users
- Compliance with basic security requirements
- Protection for basic confidentiality

**For high security needs, consider:**
- Secure transmission channels (HTTPS, SFTP)
- Access control at distribution level
- Watermarks for traceability
- Digital signatures for authenticity

---

## Next Steps

1. **[Optimization & Performance](06_optimization_performance.md)** - File size and speed optimization
2. **[Production & Deployment](07_production_deployment.md)** - Production configuration
3. **[Practical Applications](/learning/08-practical/)** - Real-world examples

---

**Continue learning →** [Optimization & Performance](06_optimization_performance.md)
