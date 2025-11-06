# RowSpan Implementation Plan

**Project:** Scryber.Core
**Component:** LayoutEngineTable.cs
**Feature:** HTML Table RowSpan Support
**Date:** 2025-11-03

---

## Executive Summary

This document outlines a comprehensive plan to implement `rowspan` attribute support in Scryber's table layout engine. The implementation follows the existing `colspan` pattern and addresses three critical requirements:

1. **Spanned rows cannot break across pages** - Row groups with rowspan must be atomic
2. **Page break brings all preceding cells** - When rowspan forces page break, entire row group moves
3. **Following cells respect spacing** - Cells in spanned rows maintain proper layout

---

## Table of Contents

1. [Current Architecture Analysis](#current-architecture-analysis)
2. [Implementation Phases](#implementation-phases)
3. [Test Cases](#test-cases)
4. [Implementation Details](#implementation-details)
5. [Risk Assessment](#risk-assessment)
6. [Validation Checklist](#validation-checklist)

---

## Current Architecture Analysis

### Key Components

**File:** `Scryber.Components/PDF/Layout/LayoutEngineTable.cs`

#### Class Structure

1. **CellReference** (line ~1631)
   - Holds individual cell data
   - Has `ColumnSpan` property (currently implemented)
   - Has `IsEmpty` flag for spanned positions
   - Needs: `RowSpan` property

2. **RowReference** (line ~1787)
   - Holds row data
   - Tracks explicit height
   - Has reference to layout block

3. **GridReference** (line ~2004)
   - Manages continuous table sections (for page breaks)
   - Tracks start and end row indices
   - Contains rows that fit in one region

4. **TableReference** (line ~2206)
   - 2D array of CellReferences: `[row, column]`
   - Methods for adding cells, rows
   - Height/width calculation methods

### How ColumnSpan Currently Works

**Phase 1: Style Building** (lines 637-658)
- Detects `StyleKeys.TableCellColumnSpanKey` in cell style
- Adds `null` entries to `_cellfullstyles` and `_cellappliedstyles` arrays for each spanned column
- Marks positions as "occupied" horizontally

**Phase 2: Grid Building** (lines 1030-1075)
- In `BuildReferenceCells`, checks if `_cellfullstyles[row, col]` is null
- If null, calls `AddEmptyCellReference` to create placeholder with `IsEmpty = true`
- Empty cells are skipped during layout

**Phase 3: Layout** (lines 353-415)
- In `DoLayoutRowCells`, calculates total width by summing across `ColumnSpan` columns:
  ```csharp
  Unit w = Unit.Zero;
  for (int i = 0; i < cref.ColumnSpan; i++)
  {
      w += _widths[cellindex + i].Size;
  }
  ```
- Advances column index by `ColumnSpan` count

**Phase 4: Width Distribution** (lines 1100-1164)
- `CalcExplicitSizes` handles cells with explicit widths
- `ApplySpannedWidths` distributes width across spanned columns

### Key Insight

RowSpan will follow the same pattern but **vertically** instead of horizontally:
- Mark spanned rows as `null` in style arrays
- Create empty CellReferences with link back to spanning cell
- Calculate height across spanned rows
- Handle page breaks by treating row groups as atomic units

---

## Implementation Phases

### Phase 1: Data Structure Changes

**Low Risk - Foundation for all other work**

#### 1.1 Add RowSpan Property to CellReference

**Location:** Line ~1631 in CellReference class

```csharp
private int _rowspan;

public int RowSpan
{
    get { return _rowspan; }
    set { _rowspan = value; }
}
```

#### 1.2 Update CellReference.PopulateStyleValues

**Location:** Line ~1700 in PopulateStyleValues method

```csharp
// Add after ColumnSpan extraction
StyleValue<int> rowspan;
if (fullstyle.TryGetValue(StyleKeys.TableCellRowSpanKey, out rowspan))
    _rowspan = rowspan.Value(fullstyle);
else
    _rowspan = 1;
```

#### 1.3 Add Spanning Cell Reference

**Location:** CellReference class

```csharp
private CellReference _spanningCell; // Points to actual cell that spans this position

/// <summary>
/// Gets or sets the cell that spans into this position (for row-spanned cells)
/// </summary>
public CellReference SpanningCell
{
    get { return _spanningCell; }
    set { _spanningCell = value; }
}

/// <summary>
/// Returns true if this cell position is covered by another cell's rowspan
/// </summary>
public bool IsRowSpanned
{
    get { return _spanningCell != null; }
}
```

---

### Phase 2: Style Building Changes

**Medium Risk - Handles detection and marking of rowspan cells**

#### 2.1 Update CalcRowsAndColumns Method

**Location:** Line ~630

**Current behavior:** Processes rows sequentially, adding null for colspan positions

**New behavior:** Also track rowspan cells for post-processing

```csharp
// After line 645 - after getting colspan value
StyleValue<int> rowSpanVal;
int rowSpan;
if (cellfull.TryGetValue(StyleKeys.TableCellRowSpanKey, out rowSpanVal))
    rowSpan = rowSpanVal.Value(cellfull);
else
    rowSpan = 1;

if (rowSpan < 1)
    throw new ArgumentOutOfRangeException("rowSpan", "Cell row span cannot be less than 1.");

// Store rowspan information for later processing
if (rowSpan > 1)
{
    // We can't add null entries vertically yet because we're building row-by-row
    // Store for post-processing
    rowSpanCells.Add(new RowSpanInfo
    {
        StartRow = currentRowIndex,
        Column = currentColumnIndex,
        RowSpan = rowSpan,
        ColumnSpan = span, // Also store colspan if present
        CellStyle = cellfull
    });
}
```

#### 2.2 Post-Process to Mark Spanned Rows

**Location:** After line 691 (after building `_cellfullstyles` array)

```csharp
// Helper structure for tracking rowspan cells
private class RowSpanInfo
{
    public int StartRow { get; set; }
    public int Column { get; set; }
    public int RowSpan { get; set; }
    public int ColumnSpan { get; set; }
    public Style CellStyle { get; set; }
}

// After building style arrays
List<RowSpanInfo> rowSpanCells = new List<RowSpanInfo>();
// ... populated during row/cell processing ...

// Mark vertically spanned cells as null
foreach (var spanInfo in rowSpanCells)
{
    // Mark subsequent rows as spanned (null) for this column
    for (int r = spanInfo.StartRow + 1; r < spanInfo.StartRow + spanInfo.RowSpan && r < rowcount; r++)
    {
        // Account for colspan - mark all spanned columns
        for (int c = spanInfo.Column; c < spanInfo.Column + spanInfo.ColumnSpan && c < maxcolcount; c++)
        {
            _cellfullstyles[r, c] = null;
            _cellappliedstyles[r, c] = null;
        }
    }
}

if (this.Context.ShouldLogDebug)
    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory,
        $"Marked {rowSpanCells.Count} cells with rowspan in style grid");
```

#### 2.3 Handle Display:none During Style Building

```csharp
// Check if entire row is hidden
DisplayMode rowDisplay = rowStyle.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);

if (rowDisplay == DisplayMode.None || rowDisplay == DisplayMode.Invisible)
{
    // Skip hidden row entirely
    if (this.Context.ShouldLogVerbose)
    {
        this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory,
            $"Skipping hidden row (display: {rowDisplay})");
    }
    continue;
}

// For cells
DisplayMode cellDisplay = cellFull.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);

if (cellDisplay == DisplayMode.None || cellDisplay == DisplayMode.Invisible)
{
    // Add null to style arrays to maintain column alignment
    rowcellstyles.Add(null);
    rowcellapplieds.Add(null);
    continue;
}
```

---

### Phase 3: Grid Building Changes

**Medium Risk - Creates proper cell references**

#### 3.1 Update BuildReferenceCells Method

**Location:** Line ~1030

```csharp
// At line 1072 where AddEmptyCellReference is called
if (null == full)
{
    CellReference emptyRef = tbl.AddEmptyCellReference(rref, column);

    // Check if this empty cell is due to rowspan
    // Look upward in previous rows to find the spanning cell
    for (int prevRow = rowIndex - 1; prevRow >= 0; prevRow--)
    {
        CellReference potentialSpanner = tbl.AllCells[prevRow, column];
        if (potentialSpanner != null && !potentialSpanner.IsEmpty)
        {
            // Check if this cell's rowspan covers current row
            if (prevRow + potentialSpanner.RowSpan > rowIndex)
            {
                emptyRef.SpanningCell = potentialSpanner;

                if (this.Context.ShouldLogVerbose)
                {
                    this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory,
                        $"Cell at row {rowIndex}, col {column} is spanned by cell at row {prevRow}, col {column}");
                }
                break;
            }
        }
    }
}
```

#### 3.2 Skip Hidden Cells During Grid Building

```csharp
if (null != full)
{
    // Check if cell is hidden via display:none
    DisplayMode display = full.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);

    if (display == DisplayMode.None || display == DisplayMode.Invisible)
    {
        // Cell is hidden - add empty reference but mark as hidden
        CellReference hiddenRef = tbl.AddEmptyCellReference(rref, column);
        hiddenRef.IsEmpty = true;

        if (this.Context.ShouldLogVerbose)
        {
            this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory,
                $"Cell at row {rowIndex}, col {column} is hidden (display: {display})");
        }
        continue;
    }

    // ... existing visible cell processing ...
}
```

---

### Phase 4: Height Calculation Changes

**Medium Risk - Ensures proper height distribution**

#### 4.1 Update GetMaxCellHeightForRow

**Location:** Line ~2547

```csharp
public Unit GetMaxCellHeightForRow(int rowindex)
{
    Unit maxH = Unit.Zero;
    for (int col = 0; col < _columncount; col++)
    {
        CellReference cref = this._cells[rowindex, col];
        if (null != cref && null != cref.Block)
        {
            Unit h;

            if (cref.RowSpan > 1)
            {
                // This cell spans multiple rows
                // For max height calculation, divide by rowspan
                // (actual distribution happens later)
                h = cref.Block.TotalBounds.Height / cref.RowSpan;
            }
            else if (cref.IsRowSpanned)
            {
                // This row is spanned by a cell above
                // Don't contribute to max height calculation
                continue;
            }
            else
            {
                h = cref.Block.TotalBounds.Height;
            }

            maxH = Unit.Max(h, maxH);
        }
    }
    return maxH;
}
```

#### 4.2 Create Helper Methods for Height Calculation

```csharp
/// <summary>
/// Calculates the minimum total height required for rows that contain a cell with rowspan
/// </summary>
public Unit GetMinimumHeightForRowGroup(int startRow, int rowSpan)
{
    Unit totalHeight = Unit.Zero;
    for (int i = 0; i < rowSpan; i++)
    {
        totalHeight += GetMaxCellHeightForRow(startRow + i);
    }
    return totalHeight;
}

/// <summary>
/// Gets total height needed across spanned rows for a specific cell
/// </summary>
private Unit GetTotalHeightForRowSpan(int startRow, int rowSpan)
{
    Unit totalHeight = Unit.Zero;

    for (int i = 0; i < rowSpan; i++)
    {
        int rowIndex = startRow + i;

        // Check if row is hidden
        RowReference rref = _tblRef.AllRows[rowIndex];
        DisplayMode display = rref.FullStyle.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);

        if (display == DisplayMode.None || display == DisplayMode.Invisible)
        {
            // Hidden row contributes 0 height
            continue;
        }

        totalHeight += GetMaxCellHeightForRow(rowIndex);
    }

    return totalHeight;
}
```

#### 4.3 Explicit Height Handling in Rowspan

```csharp
/// <summary>
/// Gets the effective height for a rowspan cell, considering explicit height
/// </summary>
private Unit GetRowSpanEffectiveHeight(CellReference cref)
{
    if (cref.RowSpan <= 1)
        return cref.Block?.TotalBounds.Height ?? Unit.Zero;

    // Check for explicit height on the cell
    Unit explicitHeight = Unit.Zero;
    bool hasExplicitHeight = false;

    if (cref.FullStyle != null)
    {
        StyleValue<Unit> heightValue;
        if (cref.FullStyle.TryGetValue(StyleKeys.SizeHeightKey, out heightValue))
        {
            explicitHeight = heightValue.Value(cref.FullStyle);
            hasExplicitHeight = true;
        }
    }

    // Get content height
    Unit contentHeight = cref.Block?.TotalBounds.Height ?? Unit.Zero;

    if (hasExplicitHeight)
    {
        // Use maximum of explicit and content height
        // Ensures content isn't clipped if larger than explicit height
        Unit effectiveHeight = Unit.Max(explicitHeight, contentHeight);

        if (this.Context.ShouldLogVerbose)
        {
            this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory,
                $"Rowspan cell at row {cref.RowIndex}, col {cref.ColumnIndex}: " +
                $"Explicit height={explicitHeight}, Content height={contentHeight}, " +
                $"Using effective height={effectiveHeight}");
        }

        return effectiveHeight;
    }

    return contentHeight;
}
```

#### 4.4 Height Distribution Across Spanned Rows

```csharp
/// <summary>
/// Distributes a rowspan cell's total height across its spanned rows
/// Considers explicit heights on individual rows
/// </summary>
private void DistributeRowSpanHeight(CellReference spanCell, Unit totalHeight)
{
    int startRow = spanCell.RowIndex;
    int rowSpan = spanCell.RowSpan;

    // Collect explicit heights for individual rows
    Unit[] rowExplicitHeights = new Unit[rowSpan];
    Unit totalExplicitHeight = Unit.Zero;
    int rowsWithExplicitHeight = 0;

    for (int i = 0; i < rowSpan; i++)
    {
        RowReference rref = _tblRef.AllRows[startRow + i];
        if (rref.HasExplicitHeight)
        {
            rowExplicitHeights[i] = rref.ExplicitHeight;
            totalExplicitHeight += rref.ExplicitHeight;
            rowsWithExplicitHeight++;
        }
    }

    Unit remainingHeight = Unit.Max(Unit.Zero, totalHeight - totalExplicitHeight);
    int rowsNeedingHeight = rowSpan - rowsWithExplicitHeight;

    // Distribute height
    for (int i = 0; i < rowSpan; i++)
    {
        Unit rowHeight;

        if (rowExplicitHeights[i] > Unit.Zero)
        {
            // Row has explicit height
            rowHeight = rowExplicitHeights[i];
        }
        else if (rowsNeedingHeight > 0)
        {
            // Distribute remaining height equally among rows without explicit height
            rowHeight = remainingHeight / rowsNeedingHeight;
        }
        else
        {
            // All rows have explicit height, use proportional distribution
            rowHeight = totalHeight / rowSpan;
        }

        // Set the row height (don't override if row has larger height from other cells)
        Unit currentMaxHeight = GetMaxCellHeightForRow(startRow + i);
        Unit finalHeight = Unit.Max(currentMaxHeight, rowHeight);

        SetCellHeightForRow(startRow + i, finalHeight);
    }
}
```

---

### Phase 5: Page Break Logic (Critical)

**High Risk - Most complex, addresses user requirements**

#### 5.1 Detect Rowspan Conflicts During Layout

**Location:** Add to `DoLayoutRowCells` (line ~361)

```csharp
private Unit DoLayoutRowCells(TableRow row, int rowindex, bool repeating)
{
    // BEFORE starting layout, check if any cells in this row have rowspan
    List<CellReference> rowspannedCells = GetRowSpannedCellsInRow(rowindex);

    if (rowspannedCells.Count > 0)
    {
        // Calculate total height needed for the rowspan group
        Unit requiredGroupHeight = GetMaxRowSpanHeight(rowspannedCells, rowindex);

        // Check if we have enough space in current region
        if (this._rowblock.CurrentRegion.AvailableHeight < requiredGroupHeight)
        {
            // NOT ENOUGH SPACE - must move entire row group to next page
            if (this.Context.ShouldLogVerbose)
            {
                this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory,
                    $"Row {rowindex} contains rowspan requiring {requiredGroupHeight} height, " +
                    $"but only {this._rowblock.CurrentRegion.AvailableHeight} available. " +
                    $"Moving row group to next region.");
            }

            MoveRowSpanGroupToNextRegion(rowspannedCells, rowindex);
        }
    }

    // Continue with normal layout...
    Unit offsetX = Unit.Zero;
    int cellcount = _tblRef.AllCells.GetLength(1);
    // ... rest of existing method ...
}
```

#### 5.2 Implement MoveRowSpanGroupToNextRegion

```csharp
/// <summary>
/// Moves all rows involved in a rowspan group to the next region/page
/// Handles edge case: rowspan in any column must bring ALL cells from affected rows
/// Per user requirement: "if cell with a row-span needs to break across a page,
/// it needs to bring with it all the appropriate preceding cells that would need to be moved"
/// </summary>
private void MoveRowSpanGroupToNextRegion(List<CellReference> rowspannedCells, int currentRowIndex)
{
    // Step 1: Find ALL rows affected by ANY rowspan in the group
    int earliestRow = currentRowIndex;
    int latestRow = currentRowIndex;

    foreach (var cell in rowspannedCells)
    {
        int spanStartRow;
        int spanEndRow;

        if (cell.IsRowSpanned && cell.SpanningCell != null)
        {
            // This cell is spanned by another cell above
            spanStartRow = cell.SpanningCell.RowIndex;
            spanEndRow = spanStartRow + cell.SpanningCell.RowSpan - 1;
        }
        else if (cell.RowSpan > 1)
        {
            // This cell spans multiple rows
            spanStartRow = cell.RowIndex;
            spanEndRow = spanStartRow + cell.RowSpan - 1;
        }
        else
        {
            continue;
        }

        earliestRow = Math.Min(earliestRow, spanStartRow);
        latestRow = Math.Max(latestRow, spanEndRow);
    }

    // Step 2: Collect ALL cells in ALL affected rows
    // This ensures cells in other columns also move (critical requirement)
    List<CellReference> allAffectedCells = new List<CellReference>();
    for (int row = earliestRow; row <= latestRow; row++)
    {
        for (int col = 0; col < _tblRef.TotalColumnCount; col++)
        {
            CellReference cellInRow = _tblRef.AllCells[row, col];
            if (cellInRow != null && !cellInRow.IsEmpty)
            {
                allAffectedCells.Add(cellInRow);
            }
        }
    }

    if (this.Context.ShouldLogVerbose)
    {
        this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory,
            $"Moving row group [{earliestRow} to {latestRow}] to next page. " +
            $"Affected cells: {allAffectedCells.Count} across {latestRow - earliestRow + 1} rows.");
    }

    // Step 3: Move entire row group to next region
    // This satisfies requirement: "spanned rows cannot break across pages"
    StartNewTableInAnotherRegion(this._rowblock, earliestRow);

    // Step 4: Re-layout all affected rows with proper spacing
    // This satisfies requirement: "following cells should respect spacing and layout"
    for (int row = earliestRow; row <= latestRow; row++)
    {
        // Re-layout will happen in DoLayoutRowCells when we continue
    }
}
```

#### 5.3 Implement Row Group Cohesion Check

```csharp
/// <summary>
/// Returns all cells in a row that are part of a rowspan (either spanning or spanned)
/// </summary>
private List<CellReference> GetRowSpannedCellsInRow(int rowIndex)
{
    List<CellReference> cells = new List<CellReference>();

    for (int col = 0; col < _tblRef.TotalColumnCount; col++)
    {
        CellReference cref = _tblRef.AllCells[rowIndex, col];
        if (cref != null)
        {
            // Cell with rowspan > 1
            if (cref.RowSpan > 1)
            {
                cells.Add(cref);
            }
            // Cell that is spanned by another cell
            else if (cref.IsRowSpanned)
            {
                cells.Add(cref);
            }
        }
    }

    return cells;
}

/// <summary>
/// Gets the maximum height needed for any rowspan in the cell list
/// </summary>
private Unit GetMaxRowSpanHeight(List<CellReference> rowspannedCells, int currentRow)
{
    Unit maxHeight = Unit.Zero;

    foreach (var cell in rowspannedCells)
    {
        Unit height;

        if (cell.IsRowSpanned && cell.SpanningCell != null)
        {
            // Use the spanning cell's total height
            int startRow = cell.SpanningCell.RowIndex;
            int span = cell.SpanningCell.RowSpan;
            height = GetTotalHeightForRowSpan(startRow, span);
        }
        else if (cell.RowSpan > 1)
        {
            // This is the spanning cell
            height = GetTotalHeightForRowSpan(cell.RowIndex, cell.RowSpan);
        }
        else
        {
            continue;
        }

        maxHeight = Unit.Max(maxHeight, height);
    }

    return maxHeight;
}
```

#### 5.4 Create Atomic Row Group Concept

```csharp
/// <summary>
/// Represents a group of rows that must stay together (no page break allowed)
/// Per user requirement: "Spanned rows cannot break across pages"
/// </summary>
private class RowGroup
{
    public int StartRow { get; set; }
    public int EndRow { get; set; }
    public Unit TotalHeight { get; set; }
    public List<CellReference> SpanningCells { get; set; }

    /// <summary>
    /// True if this row group contains cells with rowspan > 1
    /// Such groups cannot be broken across pages
    /// </summary>
    public bool CannotBreak => SpanningCells.Any(c => c.RowSpan > 1);

    public RowGroup()
    {
        SpanningCells = new List<CellReference>();
    }
}
```

#### 5.5 Validation Method

```csharp
/// <summary>
/// Validates that when a row group moves, ALL cells in affected rows are moved
/// Used for testing and debugging
/// </summary>
private void ValidateRowGroupCohesion(int startRow, int endRow, PDFLayoutBlock newBlock)
{
    for (int row = startRow; row <= endRow; row++)
    {
        for (int col = 0; col < _tblRef.TotalColumnCount; col++)
        {
            CellReference cref = _tblRef.AllCells[row, col];
            if (cref != null && cref.Block != null && !cref.IsEmpty)
            {
                // Verify this cell's block is in the new region
                if (cref.Block.Region != newBlock.Region)
                {
                    throw new PDFLayoutException(
                        $"Cell at row {row}, col {col} was not moved with its row group. " +
                        $"Expected region: {newBlock.Region}, Actual: {cref.Block.Region}");
                }
            }
        }
    }
}
```

#### 5.6 Nested Table Height Calculation

```csharp
/// <summary>
/// Calculates total height for a cell, accounting for nested tables
/// </summary>
private Unit GetCellTotalHeight(CellReference cref)
{
    if (cref.Block == null)
        return Unit.Zero;

    Unit height = cref.Block.TotalBounds.Height;

    // Check if cell contains a nested table
    if (cref.Cell != null && cref.Cell.Contents.Count > 0)
    {
        foreach (var content in cref.Cell.Contents)
        {
            if (content is Table nestedTable)
            {
                // Nested table height is already included in Block.TotalBounds
                // from the nested layout pass
                if (this.Context.ShouldLogVerbose)
                {
                    this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory,
                        $"Cell at row {cref.RowIndex}, col {cref.ColumnIndex} contains nested table");
                }
            }
        }
    }

    return height;
}
```

#### 5.7 Page Break Calculation with Hidden Rows

```csharp
/// <summary>
/// Calculates the actual visible height needed for a rowspan group
/// Excludes hidden rows from height calculation but includes them in row group
/// </summary>
private Unit GetVisibleRowSpanHeight(CellReference spanCell)
{
    Unit totalHeight = Unit.Zero;
    int startRow = spanCell.RowIndex;
    int rowSpan = spanCell.RowSpan;

    for (int i = 0; i < rowSpan; i++)
    {
        int rowIndex = startRow + i;
        RowReference rref = _tblRef.AllRows[rowIndex];

        // Check if row is hidden
        DisplayMode display = rref.FullStyle.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);
        if (display == DisplayMode.None || display == DisplayMode.Invisible)
        {
            // Hidden row - contributes 0 to height
            if (this.Context.ShouldLogVerbose)
            {
                this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory,
                    $"Row {rowIndex} in rowspan is hidden, contributing 0 height");
            }
            continue;
        }

        // Add visible row's height
        Unit rowHeight = GetMaxCellHeightForRow(rowIndex);
        totalHeight += rowHeight;
    }

    return totalHeight;
}
```

---

### Phase 6: Layout Rendering Changes

**Medium Risk - Final visual output**

#### 6.1 Update DoLayoutACell

**Location:** Line ~467

```csharp
private void DoLayoutACell(CellReference cref, TableCell cell, Styles.Style full, int colindex, int rowindex, bool repeating)
{
    try
    {
        // Apply explicit widths
        if (cref.HasExplicitWidth)
            full.Size.Width = cref.TotalWidth;

        // NEW: Handle rowspan height
        if (cref.HasExplicitHeight || cref.RowSpan > 1)
        {
            if (cref.RowSpan > 1)
            {
                // Height should span multiple rows
                // Calculate total available height across spanned rows
                Unit totalSpannedHeight = GetTotalHeightForRowSpan(rowindex, cref.RowSpan);

                // If cell has explicit height, use the maximum
                if (cref.HasExplicitHeight)
                {
                    totalSpannedHeight = Unit.Max(totalSpannedHeight, cref.TotalHeight);
                }

                full.Size.Height = totalSpannedHeight;
            }
            else if (cref.HasExplicitHeight)
            {
                full.Size.Height = cref.TotalHeight;
            }
        }

        // Skip layout for row-spanned cells (already laid out above)
        if (cref.IsRowSpanned)
        {
            // This cell position is covered by a rowspan from above
            // Don't layout content here - it's part of the spanning cell
            if (this.Context.ShouldLogVerbose)
            {
                this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory,
                    $"Skipping layout for row-spanned cell at row {rowindex}, col {colindex}");
            }
            return;
        }

        // Perform actual cell content layout
        using (IPDFLayoutEngine engine = cell.GetEngine(this, this.Context, full))
        {
            engine.Layout(this.Context, full);
        }

        // ... rest of existing method ...
    }
    catch (Exception ex)
    {
        // ... existing error handling ...
    }
}
```

#### 6.2 Update SetCellHeightForRow

**Location:** Line ~2635

```csharp
public void SetCellHeightForRow(int rowIndex, Unit h)
{
    RowReference rref = this._rows[rowIndex];
    for (int col = 0; col < _columncount; col++)
    {
        CellReference cref = rref[col];
        if (null != cref && null != cref.Block)
        {
            // Don't adjust height for rowspanned cells (controlled by spanning cell)
            if (cref.IsRowSpanned)
            {
                if (this.Context.ShouldLogVerbose)
                {
                    this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory,
                        $"Skipping height adjustment for row-spanned cell at row {rowIndex}, col {col}");
                }
                continue;
            }

            // For cells with rowspan, only set on first occurrence
            if (cref.RowSpan > 1 && cref.RowIndex == rowIndex)
            {
                // Set total height for the entire span
                Unit totalHeight = GetTotalHeightForRowSpan(rowIndex, cref.RowSpan);
                Rect total = cref.Block.TotalBounds;
                total.Height = totalHeight;
                cref.Block.TotalBounds = total;

                if (this.Context.ShouldLogVerbose)
                {
                    this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory,
                        $"Set rowspan cell height at row {rowIndex}, col {col} to {totalHeight} " +
                        $"spanning {cref.RowSpan} rows");
                }
            }
            else if (cref.RowSpan == 1)
            {
                // Single row cell - set normal height
                Rect total = cref.Block.TotalBounds;
                total.Height = h;
                cref.Block.TotalBounds = total;
            }
        }
    }
    rref.ExplicitHeight = h;
}
```

---

## Test Cases

### Basic Functionality Tests

#### Test Case 1: Simple Rowspan

```html
<table>
  <tr>
    <td rowspan="2">Spans 2 rows</td>
    <td>Row 1, Col 2</td>
  </tr>
  <tr>
    <td>Row 2, Col 2</td>
  </tr>
</table>
```

**Expected:** First cell spans 2 rows vertically, second column cells render normally

---

#### Test Case 2: Rowspan + Colspan Combined

```html
<table>
  <tr>
    <td rowspan="2" colspan="2">Spans 2x2</td>
    <td>R1C3</td>
  </tr>
  <tr>
    <td>R2C3</td>
  </tr>
</table>
```

**Expected:** Cell spans both 2 rows and 2 columns

---

#### Test Case 3: Multiple Rowspans in Same Table

```html
<table>
  <tr>
    <td rowspan="2">Span A</td>
    <td>Cell B</td>
    <td rowspan="3">Span C</td>
  </tr>
  <tr>
    <td>Cell D</td>
  </tr>
  <tr>
    <td>Cell E</td>
    <td>Cell F</td>
  </tr>
</table>
```

**Expected:** Multiple independent rowspans in different columns work correctly

---

### Page Break Tests

#### Test Case 4: Page Break with Rowspan

**Critical User Requirement Test**

```html
<table>
  <!-- Many rows to fill first page -->
  <tr><td>Regular cell</td></tr>
  <!-- ... more rows ... -->

  <!-- This should force page break and bring entire group -->
  <tr>
    <td rowspan="3">Spans 3 rows at page boundary</td>
    <td>Content 1</td>
  </tr>
  <tr><td>Content 2</td></tr>
  <tr><td>Content 3</td></tr>
</table>
```

**Expected:** All 3 rows move to next page as atomic unit

---

#### Test Case 5: Rowspan in Last Column with Overflow

**Tests: All preceding cells move with rowspan**

```html
<table style="width: 100%;">
  <!-- Fill page to boundary -->
  <tr><td>Regular rows...</td></tr>

  <!-- Last column rowspan triggers overflow -->
  <tr>
    <td>Cell A - MUST MOVE</td>
    <td>Cell B - MUST MOVE</td>
    <td rowspan="3">Last Column Rowspan - triggers overflow</td>
  </tr>
  <tr>
    <td>Cell D - MUST MOVE</td>
    <td>Cell E - MUST MOVE</td>
  </tr>
  <tr>
    <td>Cell F - MUST MOVE</td>
    <td>Cell G - MUST MOVE</td>
  </tr>
</table>
```

**Expected:** When last column rowspan causes overflow, ALL cells in all 3 rows move to next page

---

#### Test Case 6: Multiple Rowspans with Overflow

```html
<table>
  <!-- Near page boundary -->
  <tr>
    <td rowspan="2">Left Span</td>
    <td>Middle Cell 1</td>
    <td rowspan="3">Right Span - triggers overflow</td>
  </tr>
  <tr>
    <td>Middle Cell 2</td>
  </tr>
  <tr>
    <td>Left After Span</td>
    <td>Middle Cell 3</td>
  </tr>
</table>
```

**Expected:** Right span triggers overflow, ALL 3 rows move (including left span that only spans 2 rows)

---

#### Test Case 7: Rowspan in First Column with Overflow

**Tests: First column position doesn't affect behavior**

```html
<table style="width: 100%;">
  <!-- Fill page -->
  <tr><td>Regular rows...</td></tr>

  <!-- Rowspan in FIRST column triggers overflow -->
  <tr>
    <td rowspan="4">First Column Rowspan</td>
    <td>Cell B - MUST MOVE</td>
    <td>Cell C - MUST MOVE</td>
    <td>Cell D - MUST MOVE</td>
  </tr>
  <tr>
    <td>Cell E - MUST MOVE</td>
    <td>Cell F - MUST MOVE</td>
    <td>Cell G - MUST MOVE</td>
  </tr>
  <tr>
    <td>Cell H - MUST MOVE</td>
    <td>Cell I - MUST MOVE</td>
    <td>Cell J - MUST MOVE</td>
  </tr>
  <tr>
    <td>Cell K - MUST MOVE</td>
    <td>Cell L - MUST MOVE</td>
    <td>Cell M - MUST MOVE</td>
  </tr>
</table>
```

**Expected:** All 4 rows move completely to next page

---

#### Test Case 8: Rowspan in Middle Column with Overflow

```html
<table style="width: 100%;">
  <tr>
    <td>Cell A1</td>
    <td>Cell A2</td>
    <td rowspan="3">Middle Column Rowspan</td>
    <td>Cell A4</td>
    <td>Cell A5</td>
  </tr>
  <tr>
    <td>Cell B1 - MUST MOVE</td>
    <td>Cell B2 - MUST MOVE</td>
    <td>Cell B4 - MUST MOVE</td>
    <td>Cell B5 - MUST MOVE</td>
  </tr>
  <tr>
    <td>Cell C1 - MUST MOVE</td>
    <td>Cell C2 - MUST MOVE</td>
    <td>Cell C4 - MUST MOVE</td>
    <td>Cell C5 - MUST MOVE</td>
  </tr>
</table>
```

**Expected:** Cells on both sides of middle rowspan move together

---

### Nested Table Tests

#### Test Case 9: Nested Table with Rowspan (Outer Table)

```html
<table style="width: 100%;" id="outer-table">
  <!-- Fill page -->
  <tr><td>Outer Regular Row</td><td>Col 2</td></tr>

  <!-- Rowspan in outer table with nested table inside -->
  <tr>
    <td rowspan="3">
      <!-- NESTED TABLE INSIDE ROWSPAN CELL -->
      <table style="width: 100%;" id="inner-table">
        <tr><td>Inner Cell 1.1</td><td>Inner Cell 1.2</td></tr>
        <tr><td>Inner Cell 2.1</td><td>Inner Cell 2.2</td></tr>
        <tr><td>Inner Cell 3.1</td><td>Inner Cell 3.2</td></tr>
      </table>
    </td>
    <td>Outer Cell B - MUST MOVE</td>
  </tr>
  <tr>
    <td>Outer Cell C - MUST MOVE</td>
  </tr>
  <tr>
    <td>Outer Cell D - MUST MOVE</td>
  </tr>
</table>
```

**Expected:**
- Nested table renders completely inside outer rowspan cell
- Height calculation includes nested table
- If overflow, entire group moves including nested table

---

#### Test Case 10: Nested Table with Rowspan (Inner Table)

```html
<table style="width: 100%;" id="outer-table">
  <tr>
    <td>Outer Cell A</td>
    <td>
      <!-- NESTED TABLE WITH ROWSPAN INSIDE -->
      <table style="width: 100%;" id="inner-table">
        <tr>
          <td rowspan="2">Inner Rowspan</td>
          <td>Inner Cell 1.2</td>
        </tr>
        <tr>
          <td>Inner Cell 2.2</td>
        </tr>
      </table>
    </td>
  </tr>
  <tr>
    <td>Outer Cell B</td>
    <td>Outer Cell C</td>
  </tr>
</table>
```

**Expected:** Inner table's rowspan handled independently, doesn't affect outer table

---

#### Test Case 11: Nested Tables with Rowspan at Page Boundary

**Complex scenario - rowspan in both outer and inner table**

```html
<table style="width: 100%;" id="outer-table">
  <!-- Fill page -->
  <tr><td>Outer Regular Row</td><td>Col 2</td></tr>

  <!-- Outer rowspan AND inner table with rowspan -->
  <tr>
    <td rowspan="3">
      <table style="width: 100%;" id="inner-table">
        <tr>
          <td rowspan="2">Inner Rowspan</td>
          <td>Inner Cell 1.2</td>
          <td>Inner Cell 1.3</td>
        </tr>
        <tr>
          <td>Inner Cell 2.2</td>
          <td>Inner Cell 2.3</td>
        </tr>
        <tr>
          <td>Inner Cell 3.1</td>
          <td>Inner Cell 3.2</td>
          <td>Inner Cell 3.3</td>
        </tr>
      </table>
    </td>
    <td>Outer Cell B - MUST MOVE</td>
  </tr>
  <tr>
    <td>Outer Cell C - MUST MOVE</td>
  </tr>
  <tr>
    <td>Outer Cell D - MUST MOVE</td>
  </tr>
</table>
```

**Expected:**
- Inner table's rowspan laid out within its context
- Outer rowspan height includes fully laid out inner table
- Page break decision made at outer level
- All outer rows move together with complete inner table

---

### Explicit Height Tests

#### Test Case 12: Rowspan with Explicit Height (Larger than Content)

```html
<table style="width: 100%;">
  <tr>
    <td rowspan="3" style="height: 300pt;">
      Rowspan with EXPLICIT height of 300pt
      (Content is small, but height is forced)
    </td>
    <td>Cell B - should align with 300pt total</td>
  </tr>
  <tr>
    <td>Cell C - should share in 300pt distribution</td>
  </tr>
  <tr>
    <td>Cell D - should share in 300pt distribution</td>
  </tr>
</table>
```

**Expected:**
- Total height across 3 rows = 300pt
- Height distributed across rows (~100pt each)
- Cells B, C, D have row heights adjusted to match

---

#### Test Case 13: Rowspan with Explicit Height (Smaller than Content)

```html
<table style="width: 100%;">
  <tr>
    <td rowspan="2" style="height: 50pt;">
      Rowspan with SMALL explicit height (50pt)
      <br/>But this content is very long and needs more space.
      <br/>Lorem ipsum dolor sit amet, consectetur adipiscing elit.
      <br/>This text should overflow or expand the cell height.
    </td>
    <td>Cell B</td>
  </tr>
  <tr>
    <td>Cell C</td>
  </tr>
</table>
```

**Expected:**
- Explicit height: 50pt
- Content requires more (e.g., 120pt)
- Cell expands to fit content (120pt wins over 50pt)

---

#### Test Case 14: Multiple Explicit Heights in Same Row Group

```html
<table style="width: 100%;">
  <tr>
    <td rowspan="3" style="height: 200pt;">
      Rowspan A: 200pt explicit
    </td>
    <td rowspan="2" style="height: 150pt;">
      Rowspan B: 150pt explicit
    </td>
    <td>Cell C</td>
  </tr>
  <tr>
    <td>Cell D</td>
  </tr>
  <tr>
    <td>Cell E</td>
    <td>Cell F</td>
  </tr>
</table>
```

**Expected:**
- Row 0 + Row 1 ≥ 150pt (for cell B)
- Row 0 + Row 1 + Row 2 ≥ 200pt (for cell A)
- Heights distributed to satisfy both constraints

---

#### Test Case 15: Rowspan with Explicit Height at Page Boundary

```html
<table style="width: 100%;">
  <!-- Fill page to 50pt from bottom -->
  <tr><td>Regular rows...</td></tr>

  <!-- Only 50pt space left, but rowspan needs 300pt -->
  <tr>
    <td rowspan="3" style="height: 300pt;">
      Explicit 300pt height - won't fit in 50pt remaining
    </td>
    <td>Cell B - MUST MOVE</td>
  </tr>
  <tr>
    <td>Cell C - MUST MOVE</td>
  </tr>
  <tr>
    <td>Cell D - MUST MOVE</td>
  </tr>
</table>
```

**Expected:**
- 300pt > 50pt available → trigger page break
- Move entire 3-row group to next page
- Allocate full 300pt on new page

---

### Display:none Tests

#### Test Case 16: Display:none Cell in Row with Rowspan

```html
<table style="width: 100%;">
  <tr>
    <td rowspan="3">Rowspan Cell A</td>
    <td style="display: none;">Hidden Cell B</td>
    <td>Visible Cell C</td>
  </tr>
  <tr>
    <td>Visible Cell D</td>
    <td style="display: none;">Hidden Cell E</td>
  </tr>
  <tr>
    <td>Visible Cell F</td>
    <td>Visible Cell G</td>
  </tr>
</table>
```

**Expected:**
- Hidden cells (B, E) skipped during layout
- Rowspan cell A still spans 3 rows
- Visible cells (C, D, F, G) laid out normally
- Column count excludes hidden cells

---

#### Test Case 17: Display:none on Rowspan Cell Itself

```html
<table style="width: 100%;">
  <tr>
    <td rowspan="3" style="display: none;">
      Hidden Rowspan Cell
    </td>
    <td>Visible Cell A</td>
  </tr>
  <tr>
    <td>Visible Cell B</td>
  </tr>
  <tr>
    <td>Visible Cell C</td>
  </tr>
</table>
```

**Expected:**
- Hidden rowspan cell doesn't render
- Space collapses (no empty column)
- Cells A, B, C render in single column

---

#### Test Case 18: Display:none on Some Rows in Rowspan Group

```html
<table style="width: 100%;">
  <tr>
    <td rowspan="4">Rowspan across 4 rows</td>
    <td>Visible Cell A</td>
  </tr>
  <tr style="display: none;">
    <td>Hidden Cell B</td>
  </tr>
  <tr>
    <td>Visible Cell C</td>
  </tr>
  <tr style="display: none;">
    <td>Hidden Cell D</td>
  </tr>
</table>
```

**Expected:**
- Rows 0, 2 visible; rows 1, 3 hidden
- Rowspan cell height = sum of visible row heights only
- Hidden rows contribute 0 to height

---

#### Test Case 19: Display:none with Page Break in Rowspan

```html
<table style="width: 100%;">
  <!-- Fill page -->
  <tr><td>Regular rows...</td></tr>

  <!-- Rowspan at page boundary, some rows hidden -->
  <tr>
    <td rowspan="5">Rowspan Cell (5 rows)</td>
    <td>Visible Cell A</td>
  </tr>
  <tr style="display: none;">
    <td>Hidden Cell B</td>
  </tr>
  <tr>
    <td>Visible Cell C - MUST MOVE</td>
  </tr>
  <tr style="display: none;">
    <td>Hidden Cell D</td>
  </tr>
  <tr>
    <td>Visible Cell E - MUST MOVE</td>
  </tr>
</table>
```

**Expected:**
- Height calculation: only visible rows (A, C, E)
- Hidden rows have height = 0
- If overflow → move all 5 rows (including hidden)
- On new page, render only visible rows

---

## Risk Assessment

### High Risk Areas

1. **Page Break Logic with Rowspan (Phase 5)**
   - Most complex implementation
   - Critical for user requirements
   - Requires coordinating multiple rowspans
   - Edge cases with nested tables

2. **Height Calculations with Mixed Rowspan/Colspan**
   - Complex interactions between vertical and horizontal spanning
   - Distribution of explicit heights

3. **Grid Reference Updates for Multi-Page Tables**
   - Ensuring row group integrity across GridReference boundaries
   - Handling partially rendered tables

### Medium Risk Areas

1. **Style Building Phase (Phase 2)**
   - Marking vertical spans while building row-by-row
   - Requires post-processing step

2. **Nested Table Interactions**
   - Height bubbling from inner to outer tables
   - Context isolation

3. **Display:none with Rowspan**
   - Ensuring hidden rows don't affect layout but maintain row group

### Low Risk Areas

1. **Data Structure Additions (Phase 1)**
   - Simple property additions
   - Similar to existing ColumnSpan

2. **Simple Rowspan without Page Breaks**
   - Straightforward height distribution
   - Follows colspan pattern

---

## Implementation Order

### Recommended Sequence

1. **Phase 1: Data Structures** (1-2 days)
   - Low risk, foundational
   - Can be tested immediately
   - Enables all other work

2. **Phase 2: Style Building** (2-3 days)
   - Detects and marks rowspan cells
   - Includes display:none handling
   - Creates RowSpanInfo tracking

3. **Phase 3: Grid Building** (2-3 days)
   - Creates proper cell references
   - Links spanned cells to spanning cells
   - Tests with simple rowspan (no page breaks)

4. **Phase 4: Height Calculations** (3-4 days)
   - Basic height distribution
   - Explicit height handling
   - Nested table support

5. **Phase 5: Page Break Logic** (5-7 days)
   - Most complex phase
   - Implements atomic row groups
   - Addresses all user requirements
   - Extensive testing needed

6. **Phase 6: Final Rendering** (2-3 days)
   - Layout adjustments
   - Visual output refinement
   - Integration testing

**Total Estimated Time:** 15-22 days

---

## Validation Checklist

### Column Position Tests
- [ ] First column rowspan moves all cells in affected rows
- [ ] Middle column rowspan moves all cells in affected rows
- [ ] Last column rowspan moves all cells in affected rows

### Page Break Tests
- [ ] Simple rowspan at page boundary moves entire group
- [ ] Multiple rowspans find union of all affected rows
- [ ] Rowspan group cannot break across pages
- [ ] Preceding cells move with rowspan group

### Basic Functionality Tests
- [ ] Simple 2-row rowspan renders correctly
- [ ] Rowspan + colspan combination works
- [ ] Multiple independent rowspans in same table
- [ ] Overlapping rowspans (different start rows, overlapping ranges)

### Height Calculation Tests
- [ ] Height distributed across spanned rows
- [ ] Explicit height on rowspan cell honored
- [ ] Content height overrides small explicit height
- [ ] Multiple explicit heights resolved correctly
- [ ] Row explicit heights considered in distribution

### Nested Table Tests
- [ ] Outer rowspan with nested table calculates height correctly
- [ ] Nested table renders completely within outer rowspan cell
- [ ] Inner rowspan doesn't trigger outer table page breaks
- [ ] Nested table with rowspan can move with outer row group
- [ ] Three-level nesting works

### Display:none Tests
- [ ] Hidden cells (display:none) don't occupy space
- [ ] Hidden rowspan cells collapse properly
- [ ] Partial row visibility in rowspan group works
- [ ] Hidden rows excluded from height calculations
- [ ] Page break logic handles hidden rows correctly
- [ ] Visible=false works same as display:none

### Explicit Height Tests
- [ ] Rowspan explicit height larger than content distributes correctly
- [ ] Rowspan explicit height smaller than content expands to fit
- [ ] Multiple explicit heights in same row group resolved correctly
- [ ] Explicit height considered in page break calculations

### Edge Cases
- [ ] Rowspan exceeds remaining table rows (clamped)
- [ ] Empty/invisible cells with rowspan
- [ ] Rowspan = 1 (single row, no special handling)
- [ ] Large rowspan (10+ rows)
- [ ] Entire table fits in one rowspan

### Integration Tests
- [ ] Rowspan in header rows
- [ ] Rowspan in footer rows
- [ ] Rowspan with background colors
- [ ] Rowspan with borders
- [ ] Rowspan with cell padding/margins

---

## Success Criteria

Implementation is complete when:

1. ✅ All 19 test cases pass
2. ✅ All validation checklist items pass
3. ✅ User requirements satisfied:
   - Spanned rows cannot break across pages
   - Page breaks bring all affected cells
   - Following cells respect spacing
4. ✅ No regressions in existing colspan functionality
5. ✅ Performance acceptable (no significant slowdown)
6. ✅ Code follows existing Scryber patterns and conventions

---

## Notes

- Implementation follows existing colspan pattern for consistency
- All three user requirements explicitly addressed in Phase 5
- Comprehensive test coverage for edge cases
- Nested table support included for completeness
- Explicit height handling matches HTML/CSS behavior
- Display:none support ensures proper HTML compatibility

---

**Document Version:** 1.0
**Last Updated:** 2025-11-03
