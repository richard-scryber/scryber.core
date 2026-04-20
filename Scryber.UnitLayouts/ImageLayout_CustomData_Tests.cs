using System.Collections.Generic;
using System;
using System.IO;

namespace Scryber.UnitLayouts;

public static class ImageLayout_CustomData_Tests
{

    public class CatalogPdfDocument {
    public CatalogPdfHeader Header { get; set; } = new();
    public CatalogPdfLabels Labels { get; set; } = new();
    public List<CatalogPdfTocItem> TableOfContents { get; set; } = new();
    public List<CatalogPdfAssemblySection> Assemblies { get; set; } = new();
    public bool ShowPrices { get; set; } = true;
}

public class CatalogPdfLabels {
    public string SerialNumber { get; set; } = string.Empty;
    public string TableOfContents { get; set; } = string.Empty;
    public string Balloon { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
}

public class CatalogPdfHeader {
    public string LogoPath { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string ProductModel { get; set; } = string.Empty;
    public string ProductFamily { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public string ProductNumber { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}

public class CatalogPdfTocItem {
    public string AnchorId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Level { get; set; }
}

public class CatalogPdfAssemblySection {
    public string AnchorId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? PartNumber { get; set; }
    public string? Balloon { get; set; }
    public string ImageFilePath { get; set; } = string.Empty;
    public int Level { get; set; }
    public List<CatalogPdfPartRow> Parts { get; set; } = new();
}

public class CatalogPdfPartRow {
    public string Balloon { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
}

    public static object GetGDubeData()
    {
        var model = new CatalogPdfDocument {
    ShowPrices = true,
    Header = new CatalogPdfHeader {
        LogoPath = "",
        SerialNumber = "SN-2024-00123",
        ProductNumber = "EDB-456789",
        ProductModel = "HydroMax 3000",
        ProductFamily = "Hydraulic Cranes",
        ProductType = "Heavy Duty",
        Currency = "USD"
    },
    Labels = new CatalogPdfLabels {
        SerialNumber = "Serial Number",
        TableOfContents = "Table of Contents",
        Balloon = "Bal.",
        PartNumber = "Part #",
        Description = "Description",
        Quantity = "Qty",
        Weight = "Weight",
        Price = "Price"
    },
    Assemblies = [
        new CatalogPdfAssemblySection {
            AnchorId = "asm-main",
            Title = "EDB-456789 - Main Hydraulic Assembly",
            ImageFilePath = "../images/assembly.jpg",
            Parts = [
                new() { Balloon = "1",  PartNumber = "HYD-001", Description = "Hydraulic Cylinder - 3\" Bore",     Quantity = "2",  Weight = "12.50 kg", Price = "450.00" },
                new() { Balloon = "2",  PartNumber = "HYD-002", Description = "Pressure Relief Valve",             Quantity = "1",  Weight = "0.80 kg",  Price = "125.50" },
                new() { Balloon = "3",  PartNumber = "HYD-003", Description = "High Pressure Hose Assembly - 6ft", Quantity = "4",  Weight = "2.10 kg",  Price = "89.99"  },
                new() { Balloon = "4",  PartNumber = "HYD-004", Description = "Mounting Bracket - Heavy Duty",     Quantity = "2",  Weight = "3.40 kg",  Price = "67.25"  },
                new() { Balloon = "5",  PartNumber = "HYD-005", Description = "O-Ring Kit - Viton",                Quantity = "1",  Weight = "0.10 kg",  Price = "22.00"  },
                new() { Balloon = "6",  PartNumber = "STR-010", Description = "Structural Support Beam",           Quantity = "1",  Weight = "18.00 kg", Price = "320.00" },
                new() { Balloon = "7",  PartNumber = "ELC-020", Description = "Solenoid Valve - 24V DC",           Quantity = "2",  Weight = "1.20 kg",  Price = "195.75" },
                new() { Balloon = "8",  PartNumber = "FAS-030", Description = "Hex Bolt M12x40 Grade 10.9",        Quantity = "16", Weight = "0.05 kg",  Price = "1.50"   },
            ]
        }
    ]
};

        return model;
    }
}