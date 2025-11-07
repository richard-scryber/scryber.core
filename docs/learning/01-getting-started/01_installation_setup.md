---
layout: default
title: Installation & Setup
nav_order: 1
parent: Getting Started
parent_url: /learning/01-getting-started/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Installation & Setup

Get Scryber.Core installed and configured in your .NET project in minutes.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Install Scryber.Core via NuGet
- Configure your project for PDF generation
- Verify your installation
- Set up your IDE for optimal development

---

## Prerequisites

- **.NET 6.0 or later** (or .NET Framework 4.6.2+)
- **Visual Studio 2022**, **VS Code**, or **Rider**
- **Basic C# knowledge**

---

## Installation Methods

### Method 1: NuGet Package Manager (Visual Studio)

1. **Open your project** in Visual Studio
2. **Right-click** on your project in Solution Explorer
3. Select **Manage NuGet Packages**
4. Click the **Browse** tab
5. Search for **`Scryber.Core`**
6. Click **Install**

![NuGet Package Manager](../../images/nuget-install.png)

### Method 2: Package Manager Console

```powershell
Install-Package Scryber.Core
```

### Method 3: .NET CLI

```bash
dotnet add package Scryber.Core
```

### Method 4: Direct `.csproj` Edit

```xml
<ItemGroup>
  <PackageReference Include="Scryber.Core" Version="6.*" />
</ItemGroup>
```

Then restore packages:

```bash
dotnet restore
```

---

## Project Types Supported

Scryber.Core works with various .NET project types:

| Project Type | Supported | Notes |
|--------------|-----------|-------|
| **ASP.NET Core** | ✅ | Web applications, APIs |
| **Console App** | ✅ | Batch processing, CLI tools |
| **Windows Forms** | ✅ | Desktop applications |
| **WPF** | ✅ | Desktop applications |
| **Azure Functions** | ✅ | Serverless PDF generation |
| **Blazor Server** | ✅ | Interactive web apps |
| **Blazor WebAssembly** | ⚠️ | Limited (requires server-side generation) |
| **.NET Framework 4.6.2+** | ✅ | Legacy applications |

---

## Verifying Installation

Create a simple test to verify Scryber is installed correctly:

```csharp
using Scryber.Components;
using System.IO;

namespace ScryberTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a simple HTML document
            string html = @"
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Installation Test</title>
</head>
<body>
    <h1>Hello, Scryber!</h1>
    <p>If you can see this PDF, Scryber is installed correctly.</p>
</body>
</html>";

            // Parse and generate PDF
            using (var reader = new StringReader(html))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = new FileStream("test.pdf", FileMode.Create))
                {
                    doc.SaveAsPDF(stream);
                }
            }

            Console.WriteLine("PDF generated successfully: test.pdf");
        }
    }
}
```

**Run the program:**

```bash
dotnet run
```

**Expected output:**
```
PDF generated successfully: test.pdf
```

Open `test.pdf` to verify it contains "Hello, Scryber!"

---

## IDE Setup

### Visual Studio

**Recommended Extensions:**
- **C# IntelliSense** (built-in)
- **NuGet Package Manager** (built-in)
- **HTML/CSS IntelliSense** (built-in)

**Project Configuration:**
1. **Target Framework**: .NET 6.0 or later recommended
2. **Language Version**: C# 10.0 or later
3. **Nullable Reference Types**: Optional but recommended

### VS Code

**Required Extensions:**
- **C# Dev Kit** (Microsoft)
- **HTML CSS Support**
- **XML Tools** (for XHTML templates)

**Create `.vscode/launch.json`:**

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": ".NET Core Launch (console)",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/bin/Debug/net6.0/YourProject.dll",
            "args": [],
            "cwd": "${workspaceFolder}",
            "stopAtEntry": false,
            "console": "internalConsole"
        }
    ]
}
```

### Rider

**Setup:**
1. Open your solution in Rider
2. NuGet packages are managed automatically
3. Rider provides excellent HTML/CSS support out of the box

---

## Project Structure

Recommended folder structure for Scryber projects:

```
YourProject/
├── Program.cs              # Entry point
├── Templates/              # HTML/XHTML templates
│   ├── invoice.html
│   ├── report.html
│   └── letter.html
├── Styles/                 # External CSS files
│   ├── common.css
│   └── invoice.css
├── Images/                 # Static images
│   ├── logo.png
│   └── background.jpg
├── Fonts/                  # Custom fonts (optional)
│   └── CustomFont.ttf
└── Output/                 # Generated PDFs
    └── .gitkeep
```

**Add to `.gitignore`:**

```
# Scryber output
Output/*.pdf

# But keep the directory
!Output/.gitkeep
```

---

## Configuration Options

### appsettings.json (ASP.NET Core)

```json
{
  "Scryber": {
    "TemplateDirectory": "./Templates",
    "ImageDirectory": "./Images",
    "OutputDirectory": "./Output",
    "DefaultFontFamily": "Arial",
    "CacheTemplates": true
  }
}
```

### Dependency Injection Setup (ASP.NET Core)

```csharp
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Add Scryber services (if using ASP.NET Core)
        services.AddScryber();
    }
}
```

---

## Common Installation Issues

### Issue 1: NuGet Package Not Found

**Problem:** Cannot find `Scryber.Core` in NuGet

**Solution:**
- Ensure NuGet package sources include nuget.org
- Clear NuGet cache: `dotnet nuget locals all --clear`
- Check internet connection

### Issue 2: Version Conflicts

**Problem:** Dependency version conflicts

**Solution:**
```xml
<ItemGroup>
  <PackageReference Include="Scryber.Core" Version="6.*" />
  <!-- Explicitly specify conflicting dependencies if needed -->
</ItemGroup>
```

### Issue 3: .NET Framework Compatibility

**Problem:** Cannot install on .NET Framework project

**Solution:**
- Ensure project targets .NET Framework 4.6.2 or later
- Update project file:
```xml
<TargetFramework>net472</TargetFramework>
```

### Issue 4: File Access Permissions

**Problem:** Cannot write PDF files

**Solution:**
- Ensure output directory exists
- Check folder permissions
- Use absolute paths for testing:

```csharp
string outputPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
    "test.pdf"
);
```

---

## Performance Considerations

### For High-Volume Scenarios

```csharp
// Use object pooling for better performance
public class PdfService
{
    private readonly ObjectPool<Document> _docPool;

    public PdfService()
    {
        _docPool = new DefaultObjectPool<Document>(
            new DocumentPooledObjectPolicy(),
            maxRetained: 10
        );
    }

    public byte[] GeneratePdf(string template, object data)
    {
        var doc = _docPool.Get();
        try
        {
            // Generate PDF
            using (var ms = new MemoryStream())
            {
                doc.SaveAsPDF(ms);
                return ms.ToArray();
            }
        }
        finally
        {
            _docPool.Return(doc);
        }
    }
}
```

---

## Next Steps

Now that Scryber is installed and configured:

1. **[Create Your First Document](02_first_document.md)** - Build a simple PDF from scratch
2. **[HTML to PDF](03_html_to_pdf.md)** - Learn how HTML converts to PDF
3. **[Troubleshooting](08_troubleshooting.md)** - If you encounter issues

---

## Additional Resources

- **[Scryber GitHub](https://github.com/richard-scryber/scryber.core)** - Source code and issues
- **[NuGet Package](https://www.nuget.org/packages/Scryber.Core/)** - Official package
- **[API Documentation](/api/)** - Complete API reference

---

**Ready to create your first PDF?** → [Your First Document](02_first_document.md)
