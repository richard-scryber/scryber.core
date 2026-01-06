# Scryber.Core Test Coverage Analysis

**Date:** 2026-01-04
**Analysis Scope:** Unit test coverage gaps and missing test scenarios

---

## Executive Summary

The scryber.core project has **good coverage for layout and visual rendering** but **significant gaps in unit testing for core functionality**, particularly:

- **Expression Engine Functions**: 90+ functions with minimal direct unit testing
- **CSS Property Parsers**: 118 parsers with limited parser-specific tests
- **Error Handling**: Very few exception/error scenario tests
- **Font System**: No dedicated font handling tests
- **Data Binding Edge Cases**: Limited testing of error paths and edge cases

---

## Table of Contents

1. [What IS Well Tested](#what-is-well-tested)
2. [What is Missing or Under-Tested](#what-is-missing-or-under-tested)
3. [Priority Areas for Testing](#priority-areas-for-testing)
4. [Specific Test Gap Examples](#specific-test-gap-examples)
5. [Recommendations](#recommendations)
6. [Conclusion](#conclusion)

---

## What IS Well Tested

### 1. Layout Engine (542+ test methods in Scryber.UnitLayouts)

**Excellent Coverage:**
- ✅ Positioning (absolute, relative, fixed, floating, inline-block) - 197 tests
- ✅ Tables (basic layout, sizing) - 33 tests
- ✅ Breaks (page breaks, column breaks) - 11 tests
- ✅ Text layout and whitespace - 38 tests
- ✅ SVG rendering - 70 tests
- ✅ Backgrounds and borders - 24 tests
- ✅ Overflow handling - 32 tests
- ✅ Panel/container layout - 16 tests
- ✅ Header/footer positioning - 19 tests
- ✅ Links and outlines - 3 tests

**Files:** `Scryber.UnitLayouts/` (26 test files)

### 2. Drawing Primitives (Well tested)

**Good Coverage:**
- ✅ PDFUnit, PDFPoint, PDFRect, PDFSize - dimension types
- ✅ Color parsing and standard colors
- ✅ Pen, Brush, Dash patterns
- ✅ Transformation matrices
- ✅ Graphics paths
- ✅ Point arrays
- ✅ Thickness calculations

**Files:** `Scryber.UnitTest/Drawing/` (20+ test files)

### 3. PDF Native Types (Good coverage)

- ✅ PDFArray, PDFDictionary, PDFName, PDFNumber, PDFReal
- ✅ PDFString, PDFBoolean, PDFNull
- ✅ PDFStream, PDFIndirectObject
- ✅ PDFXRefTable

**Files:** `Scryber.UnitTest/Native/` (11 test files)

### 4. Style System (Good coverage)

- ✅ Style merging and cascading
- ✅ Style stacks and priority
- ✅ Selector matching and specificity
- ✅ Individual style properties (font, text, background, border, etc.)
- ✅ Style collections and groups

**Files:** `Scryber.UnitTest/Styles/` (30+ test files)

### 5. HTML/CSS Parsing (Moderate coverage)

- ✅ Basic HTML parsing (HtmlParsing_Tests.cs)
- ✅ CSS parsing fundamentals (CssParsing_Tests.cs - 41 tests)
- ✅ HTML transforms
- ✅ Data images and gradients
- ✅ SVG parsing

**Files:** `Scryber.UnitTest/Html/` (8 test files)

### 6. Data Binding Basics (Moderate coverage)

- ✅ Expression binding fundamentals
- ✅ Handlebars helpers (each, with, if)
- ✅ XML/HTML/JSON binding
- ✅ Loop binding
- ✅ Image binding
- ✅ Controller binding

**Files:** `Scryber.UnitTest/Binding/` (9 test files)

---

## What is Missing or Under-Tested

### 1. CRITICAL: Expression Engine Functions (90+ functions, ~5-10% tested)

**Implementation Location:** `Scryber.Expressions/Expressive/Functions/`

**Test Coverage:** Only ~10-15 functions tested in `IndividualExpressionTests.cs` and scattered through binding tests

#### STRING FUNCTIONS (22 implementations, ~3 tested)

**Missing tests for:**
- `padLeft()`, `padRight()` - String padding
- `trimStart()`, `trimEnd()` - Partial trimming
- `indexOf()` - String position finding
- `startsWith()`, `endsWith()`, `contains()` - String testing
- `split()` - String splitting
- `regexIsMatch()`, `regexMatches()`, `regexReplace()` - Regex operations
- `toUpper()`, `toLower()` - Case conversion
- `length()` - String length
- `replace()` - String replacement
- `join()` - Array joining (only tested in complex scenarios)
- `format()` (aka `string()`) - Formatting with various format strings
- `eval()` - Dynamic expression evaluation

**Tested:** `concat()`, `substring()`, basic string operations in binding tests

#### MATHEMATICAL FUNCTIONS (23 implementations, 0 directly tested)

**Missing tests for ALL:**
- `abs()`, `ceiling()`, `floor()`, `round()`, `truncate()` - Rounding functions
- `sqrt()`, `pow()`, `exp()`, `log()`, `log10()` - Power/logarithm functions
- `sin()`, `cos()`, `tan()`, `asin()`, `acos()`, `atan()` - Trigonometric functions
- `degrees()`, `radians()` - Angle conversion
- `sign()` - Sign determination
- `pi()`, `e()` - Mathematical constants
- `random()` - Random number generation
- `IEEERemainder()` - IEEE remainder calculation

**Tested:** None directly (only used in complex expressions)

#### DATE/TIME FUNCTIONS (21 implementations, ~2 tested)

**Missing tests for:**
- **Add Functions** (6): `addDays()`, `addMonths()`, `addYears()`, `addHours()`, `addMinutes()`, `addSeconds()`, `addMilliseconds()`
- **Between Functions** (4): `hoursBetween()`, `minutesBetween()`, `secondsBetween()`, `millisecondsBetween()`
- **Extract Functions** (9): `yearOf()`, `monthOfYear()`, `dayOfMonth()`, `dayOfWeek()`, `hourOf()`, `minuteOf()`, `secondOf()`, `millisecondOf()`
- `date()` function with format strings

**Tested:** `dayOfYear()`, `daysBetween()` (partially)

#### STATISTICAL FUNCTIONS (5 implementations, ~3 tested)

**Missing tests for:**
- `mean()` - Arithmetic mean
- `mode()` - Most frequent value (only basic test exists)

**Tested:** `average()`, `averageOf()`, `median()` (basic tests)

#### COLLECTION/COALESCE FUNCTIONS (7 implementations, ~3 tested)

**Missing comprehensive tests for:**
- `eachOf()` - Extract property from collection
- `selectWhere()` - Filter collection
- `reverse()` - Reverse array
- `collect()` - Flatten/merge collections (only complex test exists)

**Tested:** `sortBy()`, `firstWhere()` (partial)

#### RELATIONAL FUNCTIONS (8 implementations, ~4 tested)

**Missing tests for:**
- `countOf()` - Conditional counting
- `sumOf()` - Sum of property values (used but not directly tested)
- `minOf()`, `maxOf()` - Min/max of property values (partially tested)

**Tested:** `count()`, `sum()`, `min()`, `max()` (basic tests)

#### CONVERSION FUNCTIONS (8 implementations, ~2 tested)

**Missing tests for:**
- `bool()` - Boolean conversion
- `long()` - Long integer conversion
- `decimal()`, `double()` - Decimal conversions
- `typeof()` - Type information
- `date()` - Date parsing with various formats

**Tested:** `integer()`, `string()` (partially)

#### LOGICAL FUNCTIONS (4 implementations, ~2 tested)

**Missing tests for:**
- `index()` - Index-based access
- Complex `if()` scenarios with nested expressions

**Tested:** `ifError()`, `in()`, basic `if()`

#### CSS FUNCTIONS (2 implementations, 0 tested)

**Missing tests for:**
- `calc()` - CSS calc() expression generation
- `var()` - CSS variable reference

---

### 2. CSS PROPERTY PARSERS (118 parsers, minimal parser-specific tests)

**Implementation Location:** `Scryber.Styles/Styles/Parsing/Typed/`

**Test Coverage:** `CssParsing_Tests.cs` has 41 tests, but most test CSS integration, not individual parser edge cases

#### Missing Parser-Specific Tests:

**Box Model Parsers** (24 parsers):
- Margin parsers (8): top, right, bottom, left, all, inline-start, inline-end, inline-both
- Padding parsers (8): top, right, bottom, left, all, inline-start, inline-end, inline-both
- Border parsers (8): border-width, border-style, border-color, border-radius, border-top/right/bottom/left
- Min/max width/height (4)

**Typography Parsers** (12 parsers):
- `font`, `font-family`, `font-size`, `font-weight`, `font-style`, `font-stretch`
- `line-height`, `letter-spacing`, `word-spacing`
- `text-align`, `text-decoration`, `vertical-align`

**Background Parsers** (8 parsers):
- `background`, `background-color`, `background-image`
- `background-position`, `background-position-x`, `background-position-y`
- `background-repeat`, `background-size`

**Positioning Parsers** (7 parsers):
- `position`, `top`, `right`, `bottom`, `left`
- `float`, `display`

**Page Break Parsers** (6 parsers):
- `page-break-before`, `page-break-after`, `page-break-inside`
- `break-before`, `break-after`, `break-inside`

**Column Parsers** (6 parsers):
- `column-count`, `column-width`, `column-gap`, `column-span`
- `column-break-before`, `column-break-after`, `column-break-inside`

**Hyphenation Parsers** (5 parsers):
- `hyphens`, `hyphenate-min-length`
- `hyphens-min-before`, `hyphens-min-after`
- `hyphenate-limits`

**SVG-Specific Parsers** (10 parsers):
- `fill`, `fill-color`, `fill-opacity`
- `stroke`, `stroke-color`, `stroke-width`, `stroke-opacity`
- `stroke-dash`, `stroke-dash-offset`
- `stroke-line-cap`, `stroke-line-join`
- `paint-order`, `dominant-baseline`, `text-anchor`

**List Style Parsers** (8 parsers):
- `list-style`, `list-style-type`
- `list-item-alignment`, `list-item-concatenation`
- `list-item-group`, `list-item-inset`
- `list-item-prefix`, `list-item-postfix`

**Other Parsers** (12 parsers):
- `overflow`, `overflow-x`, `overflow-y`, `overflow-action`
- `white-space`, `content`
- `counter-reset`, `counter-increment`
- `opacity`, `transform`
- `page`, `page-size`

#### Edge Cases Missing Tests:

- Invalid CSS values for each property
- Boundary values (0, negative, max values)
- Unit conversions (pt, px, em, rem, %, mm, cm, in)
- Color formats (hex, rgb, rgba, hsl, hsla, named colors, transparent)
- Shorthand property expansion (`margin: 10pt 20pt`, `border: 1pt solid red`)
- Inheritance and cascading edge cases
- CSS variable substitution edge cases
- calc() expression parsing edge cases

---

### 3. TABLE LAYOUT COMPLEX SCENARIOS (Limited coverage)

**Current Coverage:** 33 tests in `Table_Tests.cs`

**Missing Tests:**

#### Colspan edge cases:
- Colspan spanning multiple columns with varying widths
- Colspan at end of row
- Colspan with auto-width columns
- Nested tables with colspan

#### Rowspan edge cases:
- Rowspan across page breaks
- Rowspan with varying row heights
- Complex rowspan + colspan combinations
- Rowspan in thead/tbody/tfoot

#### Table layout algorithms:
- Auto table layout algorithm
- Fixed table layout algorithm
- Table width calculation with percentage columns
- Min-width/max-width constraints on tables

#### Table headers/footers:
- Thead repeating on multiple pages
- Tfoot appearing on last page only
- Complex thead/tfoot with merged cells

#### Table borders:
- Border collapse algorithm
- Border-spacing with various values
- Borders with rowspan/colspan cells

#### Table overflow:
- Wide tables exceeding page width
- Table wrapping behavior
- Horizontal scrolling scenarios

---

### 4. FONT HANDLING (No dedicated tests)

**Current Coverage:** `FontMetrics_Test.cs`, `PDFFont_Test.cs`, `FontSelector_Test.cs` exist but limited

**Missing Tests:**

#### Font loading:
- TrueType font loading and validation
- OpenType font loading
- Font fallback chain
- Missing font handling
- Font embedding vs. subsetting

#### Font metrics:
- Ascent, descent, line height calculations
- Character width measurements
- Kerning pair handling
- Ligature handling

#### Font matching:
- Font family fallback (e.g., "Arial, Helvetica, sans-serif")
- Font weight matching (100-900)
- Font style matching (normal, italic, oblique)
- Font stretch matching

#### Unicode handling:
- Multi-byte character support
- Right-to-left text
- Complex scripts (Arabic, Hindi, Thai)
- Emoji rendering

#### Web fonts:
- Google Fonts loading
- @font-face CSS rule parsing
- Font format detection (ttf, otf, woff, woff2)
- Font loading errors and fallbacks

---

### 5. IMAGE PROCESSING (Minimal tests)

**Current Coverage:** `ImageLoad_Tests.cs`, `ImageType_Tests.cs` have 7-8 exception tests

**Missing Tests:**

#### Image format handling:
- JPEG loading (quality, progressive, EXIF)
- PNG loading (transparency, interlacing, color depths)
- GIF loading (animation, transparency)
- TIFF loading (multi-page, compression)

#### Data URL images:
- Base64 encoding/decoding
- Various image formats in data URLs
- Large data URL handling
- Invalid data URL handling

#### Image sizing:
- Aspect ratio preservation
- Image scaling algorithms
- Max-width/max-height constraints
- Object-fit CSS property

#### Image optimization:
- Image compression
- Image resampling
- Color space conversion
- JPEG pass-through optimization

#### Image errors:
- Missing images
- Corrupt image files
- Unsupported formats
- Network errors loading remote images
- Timeout handling

---

### 6. ERROR HANDLING AND EDGE CASES (Very limited)

**Current Coverage:** Only 174 occurrences of try/catch across 35 files, **ZERO** tests using `ExpectedException` or `Assert.Throws`

**Missing Error Scenario Tests:**

#### Expression Evaluation Errors:
- Division by zero in expressions
- Type mismatch errors (string + number)
- Undefined variable access
- Null reference in expressions
- Array index out of bounds
- Function parameter count mismatch
- Recursive expression evaluation
- Stack overflow in nested expressions

#### CSS Parsing Errors:
- Malformed CSS syntax
- Invalid property values
- Circular @import references
- Missing semicolons
- Unclosed brackets/quotes
- Invalid selectors
- CSS variable circular references

#### Data Binding Errors:
- Missing data properties
- Type conversion failures
- Circular references in data
- Null data objects
- Invalid JSON
- XML parsing errors
- SQL query errors
- Network timeouts

#### Layout Errors:
- Infinite layout loops
- Negative dimensions
- Extremely large content
- Invalid positioning values
- Circular containment
- Missing required properties

#### PDF Generation Errors:
- Disk full during write
- Stream write errors
- Encoding errors
- Security/encryption errors
- Malformed PDF structure
- Resource limit exceeded

#### Font Errors:
- Missing font files
- Corrupt font data
- Unsupported font formats
- Font embedding failures
- Character not in font

#### Image Errors:
- Out of memory with large images
- Corrupt image data
- Unsupported formats
- Network failures
- Access denied errors

---

### 7. DATA BINDING EDGE CASES (Limited coverage)

**Current Coverage:** 9 binding test files, mostly happy path scenarios

**Missing Tests:**

#### Complex Object Graphs:
- Deeply nested objects (10+ levels)
- Circular references detection
- Very large arrays (10,000+ items)
- Sparse arrays
- Objects with null properties
- Mixed type arrays

#### Expression Edge Cases:
- Very long expressions (1000+ chars)
- Deeply nested function calls
- Complex operator precedence
- Whitespace handling in expressions
- Unicode in expressions
- Special characters in property names

#### Handlebars Edge Cases:
- Nested {{#each}} loops (3+ levels)
- {{#each}} with empty arrays
- {{#each}} with null data
- {{#with}} with missing properties
- {{#if}} with complex conditions
- Combining multiple helpers
- Helper parameter edge cases

#### Context Navigation:
- Multiple levels of `../` parent access
- Context switching in nested templates
- Variable shadowing
- Scope resolution with same-named properties

#### Performance Scenarios:
- Binding 1000+ elements
- Expression evaluation performance
- Large data set pagination
- Memory usage with large documents

---

### 8. HYPHENATION (No comprehensive tests)

**Current Coverage:** `Hyphenation_Test.cs` exists (1 file)

**Missing Tests:**
- Hyphenation dictionary loading
- Language-specific hyphenation rules
- Min-word-length before hyphenation
- Min-characters before/after hyphen
- Hyphenation limits (max consecutive lines)
- Custom hyphenation patterns
- Soft hyphen (­) handling
- Non-breaking hyphen handling
- Hyphenation in different languages
- Hyphenation with right-to-left text

---

### 9. PAGE BREAKS AND LAYOUT FLOW (Partial coverage)

**Current Coverage:** `Breaks_Tests.cs` (11 tests)

**Missing Tests:**

#### Orphans and widows:
- Orphan control (min lines at end of page)
- Widow control (min lines at start of page)
- Orphan/widow with tables
- Orphan/widow with lists

#### Page break avoidance:
- `page-break-inside: avoid` edge cases
- Keep-together scenarios
- Unbreakable content exceeding page height
- Forced breaks vs. natural breaks

#### Column breaks:
- Multi-column layout balancing
- Column break with spanning elements
- Column fill (auto vs. balance)

#### Complex break scenarios:
- Nested break contexts
- Breaks in positioned elements
- Breaks in floated elements
- Breaks in absolutely positioned content

---

### 10. SVG ADVANCED FEATURES (Partial coverage)

**Current Coverage:** 70 tests in SVG_Tests.cs, SVGComponent_Tests.cs

**Missing Tests:**

#### SVG Filters:
- Blur filters
- Drop shadows
- Color matrix filters
- Composite filters

#### SVG Gradients Advanced:
- Gradient transforms
- Gradient units (objectBoundingBox vs userSpaceOnUse)
- Gradient spread methods (pad, reflect, repeat)

#### SVG Clipping and Masking:
- clipPath elements
- Mask elements
- Complex clipping shapes

#### SVG Symbols and Defs:
- Symbol reuse
- Defs reference handling
- Use element with transforms

#### SVG Text Advanced:
- Text on path
- Text spans with varying styles
- Text rotation
- Text anchor positions

---

## Priority Areas for Testing

### HIGH PRIORITY (Critical for reliability)

#### 1. Expression Functions Unit Tests
- Create comprehensive unit tests for each of the 90+ functions
- Test with various data types (null, undefined, empty, boundary values)
- Test error scenarios (invalid parameters, type mismatches)
- **Estimated:** 300-500 test methods needed

#### 2. Error Handling Tests
- Add tests for all exception paths
- Test invalid input handling across all components
- Test resource failure scenarios (file not found, network errors)
- **Estimated:** 100-200 test methods needed

#### 3. CSS Parser Edge Case Tests
- Test each of the 118 parsers with invalid inputs
- Test boundary values and unit conversions
- Test shorthand property expansion
- **Estimated:** 200-300 test methods needed

#### 4. Font Handling Tests
- Test font loading (TrueType, OpenType, Web fonts)
- Test font fallback chains
- Test missing font handling
- Test font metrics and measurements
- **Estimated:** 50-100 test methods needed

### MEDIUM PRIORITY (Important for robustness)

#### 5. Table Layout Complex Scenarios
- Colspan/rowspan edge cases
- Table overflow and wrapping
- Page breaks in tables
- Border collapse algorithm
- **Estimated:** 50-100 test methods needed

#### 6. Image Processing Tests
- Test each image format with various edge cases
- Test data URL handling
- Test image sizing and scaling
- Test error scenarios
- **Estimated:** 40-80 test methods needed

#### 7. Data Binding Edge Cases
- Large and complex object graphs
- Circular reference detection
- Performance tests with large datasets
- Nested template scenarios
- **Estimated:** 50-100 test methods needed

#### 8. Hyphenation Tests
- Language-specific rules
- Hyphenation limits and boundaries
- Custom patterns
- **Estimated:** 20-40 test methods needed

### LOW PRIORITY (Nice to have)

#### 9. SVG Advanced Features
- Filters, clipping, masking
- Advanced text features
- **Estimated:** 30-60 test methods needed

#### 10. Page Break Advanced Scenarios
- Orphan/widow control
- Complex nesting scenarios
- **Estimated:** 20-40 test methods needed

---

## Specific Test Gap Examples

### Example 1: String Function - `padLeft()`

**Implementation:** `Scryber.Expressions/Expressive/Functions/String/PadLeftFunction.cs`
**Tests:** None found
**Needed Tests:**

```csharp
[TestMethod]
public void PadLeft_WithDefaultChar_PadsCorrectly()
{
    // Test: padLeft('test', 10) should return '      test'
}

[TestMethod]
public void PadLeft_WithCustomChar_PadsCorrectly()
{
    // Test: padLeft('test', 10, '0') should return '000000test'
}

[TestMethod]
public void PadLeft_WithNegativeLength_ThrowsException()
{
    // Test: padLeft('test', -1) should throw
}

[TestMethod]
public void PadLeft_WithNullString_HandlesGracefully()
{
    // Test: padLeft(null, 10) behavior
}
```

---

### Example 2: Mathematical Function - `sqrt()`

**Implementation:** `Scryber.Expressions/Expressive/Functions/Mathematical/SqrtFunction.cs`
**Tests:** None found
**Needed Tests:**

```csharp
[TestMethod]
public void Sqrt_OfPositiveNumber_ReturnsCorrectValue()
{
    // Test: sqrt(16) should return 4
}

[TestMethod]
public void Sqrt_OfZero_ReturnsZero()
{
    // Test: sqrt(0) should return 0
}

[TestMethod]
public void Sqrt_OfNegativeNumber_ThrowsOrReturnsNaN()
{
    // Test: sqrt(-1) behavior
}

[TestMethod]
public void Sqrt_OfNonNumeric_ThrowsException()
{
    // Test: sqrt('text') should throw
}
```

---

### Example 3: CSS Parser - `CSSBorderRadiusParser`

**Implementation:** `Scryber.Styles/Styles/Parsing/Typed/CSSBorderRadiusParser.cs`
**Tests:** Minimal in CssParsing_Tests.cs
**Needed Tests:**

```csharp
[TestMethod]
public void BorderRadius_SingleValue_AppliesAllCorners()
{
    // Test: border-radius: 10pt
}

[TestMethod]
public void BorderRadius_TwoValues_AppliesTopBottomAndLeftRight()
{
    // Test: border-radius: 10pt 20pt
}

[TestMethod]
public void BorderRadius_FourValues_AppliesEachCorner()
{
    // Test: border-radius: 10pt 20pt 30pt 40pt
}

[TestMethod]
public void BorderRadius_WithSlash_AppliesElliptical()
{
    // Test: border-radius: 10pt / 20pt
}

[TestMethod]
public void BorderRadius_InvalidValue_ThrowsOrIgnores()
{
    // Test: border-radius: invalid
}
```

---

### Example 4: Table Colspan Edge Case

**Implementation:** `Scryber.Components/PDF/Layout/LayoutEngineTable.cs`
**Tests:** Basic colspan test exists
**Needed Tests:**

```csharp
[TestMethod]
public void Table_ColspanExceedingColumns_ThrowsOrHandles()
{
    // Test: colspan="5" in 3-column table
}

[TestMethod]
public void Table_ColspanWithAutoWidth_DistributesCorrectly()
{
    // Test: colspan cells with auto-width siblings
}

[TestMethod]
public void Table_ColspanAcrossPageBreak_BreaksCorrectly()
{
    // Test: large colspan cell spanning page break
}

[TestMethod]
public void Table_ColspanAndRowspanCombined_LayoutsCorrectly()
{
    // Test: cell with both colspan and rowspan
}
```

---

### Example 5: Error Handling - Division by Zero

**Implementation:** Throughout expression engine
**Tests:** None found using Assert.Throws or ExpectedException
**Needed Tests:**

```csharp
[TestMethod]
[ExpectedException(typeof(DivideByZeroException))]
public void Expression_DivisionByZero_ThrowsException()
{
    // Test: {{10 / 0}} should throw
}

[TestMethod]
public void Expression_DivisionByZeroInFunction_HandlesGracefully()
{
    // Test: {{if(true, 10/0, 5)}} error handling
}
```

---

### Example 6: Font Fallback

**Implementation:** `Scryber.Components/Components/DocumentFontMatcher.cs`
**Tests:** Limited in FontSelector_Test.cs
**Needed Tests:**

```csharp
[TestMethod]
public void FontMatcher_WithMissingFont_UsesFirstFallback()
{
    // Test: "NonExistentFont, Arial, sans-serif" uses Arial
}

[TestMethod]
public void FontMatcher_WithAllMissingFonts_UsesDefaultFont()
{
    // Test: "Font1, Font2, Font3" all missing
}

[TestMethod]
public void FontMatcher_WithWeightNotAvailable_UsesClosestWeight()
{
    // Test: font-weight: 650 with only 400, 700 available
}
```

---

## Recommendations

### 1. Create Dedicated Test Projects

Consider splitting tests into more focused projects:
- `Scryber.Expressions.UnitTests` - For all 90+ expression functions
- `Scryber.Styles.UnitTests` - For CSS parser edge cases
- `Scryber.ErrorHandling.Tests` - For exception scenarios
- `Scryber.Font.UnitTests` - For font system tests

### 2. Implement Test Data Builders

Create builder pattern classes for complex test scenarios:

```csharp
var table = new TableBuilder()
    .WithColumns(3)
    .AddRow(row => row.AddCell(colspan: 2).AddCell())
    .AddRow(row => row.AddCell(rowspan: 2).AddCell().AddCell())
    .Build();
```

### 3. Add Property-Based Testing

Use FsCheck or similar for property-based testing:
- CSS parser with random valid/invalid inputs
- Expression evaluation with random expressions
- Layout engine with random dimensions

### 4. Performance Benchmarks

Add performance tests for:
- Large document generation (100+ pages)
- Complex table layouts (1000+ rows)
- Expression evaluation performance (1000+ evaluations)
- Image processing with large images

### 5. Integration Test Suite

Create comprehensive integration tests for:
- End-to-end document generation scenarios
- Multi-page documents with headers/footers
- Complex data binding with real-world data
- Error recovery scenarios

---

## Conclusion

Scryber.core has **excellent layout and visual rendering test coverage** but **significant gaps in unit testing for core functionality**. The most critical missing areas are:

1. **Expression Engine Functions** (90+ functions, <10% tested)
2. **CSS Property Parsers** (118 parsers, minimal edge case testing)
3. **Error Handling** (Zero tests using Assert.Throws/ExpectedException)
4. **Font System** (No dedicated font handling tests)
5. **Complex Table Scenarios** (Colspan/rowspan edge cases)
6. **Image Processing** (Limited format-specific tests)

**Priority:** Focus on adding comprehensive unit tests for the Expression Engine and implementing systematic error scenario testing across all components. These are the highest-risk areas that could lead to production issues.

**Estimated Test Effort:** 1000-2000 additional test methods needed to achieve comprehensive coverage of missing areas.

---

## Summary Statistics

| Category | Current State | Missing Tests | Priority |
|----------|--------------|---------------|----------|
| Expression Functions | ~10% tested | 300-500 tests | HIGH |
| Error Handling | 0 exception tests | 100-200 tests | HIGH |
| CSS Parsers | Integration only | 200-300 tests | HIGH |
| Font Handling | Limited | 50-100 tests | HIGH |
| Table Complex Scenarios | Basic only | 50-100 tests | MEDIUM |
| Image Processing | Minimal | 40-80 tests | MEDIUM |
| Data Binding Edge Cases | Happy path only | 50-100 tests | MEDIUM |
| Hyphenation | 1 file | 20-40 tests | MEDIUM |
| SVG Advanced | Partial | 30-60 tests | LOW |
| Page Breaks Advanced | Partial | 20-40 tests | LOW |

**Total Estimated Missing Tests:** 1000-2000 test methods

---

*This analysis was generated on 2026-01-04 through comprehensive codebase exploration and test coverage review.*
