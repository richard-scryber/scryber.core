# Rowspan Implementation Strategy for Scryber.Core

## Executive Summary

This document outlines the strategy to implement `rowspan` attribute support in the Scryber.Core PDF generation engine. Currently, the table layout engine (`LayoutEngineTable`) fully supports `colspan` but lacks `rowspan` functionality. This implementation will add complete rowspan support with proper handling of complex scenarios including interaction with colspan, page breaks, and varying row heights.

---

## Current State Analysis

### Colspan Implementation (Existing)

**How Colspan Works Currently:**

1. **Property Layer** (`TableCell.cs`)
   - `CellColumnSpan` property reads/writes `StyleKeys.TableCellColumnSpanKey`
   - Default value: 1

2. **Style Building** (`LayoutEngineTable.BuildStyles()`)
   - When processing cells with colspan > 1, the engine adds `null` entries to style arrays
   - This creates "placeholder" entries for spanned columns
   - Example: A cell with colspan=2 adds 1 null entry after itself

3. **Reference Grid** (`LayoutEngineTable.CellReference`)
   - Stores `ColumnSpan` property extracted from style during cell reference creation
   - Used in `DoLayoutRowCells()` to calculate cell width and advance through columns

4. **Layout Processing** (`DoLayoutRowCells()`)
   - Calculates width by summing widths of all spanned columns
   - Calls `MoveToNextRegion()` multiple times based on colspan
   - Indexes advance by colspan amount

### Key Data Structures

```csharp
// In LayoutEngineTable
private Style[,] _cellfullstyles;      // [rowIndex, colIndex] = full style or null
private Style[,] _cellappliedstyles;   // [rowIndex, colIndex] = applied style or null

// In CellReference (inner class)
private int _colspan;                  // Column span count
private int _rowindex;                 // Row index
private int _colindex;                 // Column index
private TableCell _cell;               // Reference to actual cell
```

### Current Gaps for Rowspan

1. **No RowSpan Property** - TableCell lacks `CellRowSpan` property
2. **No RowSpan Tracking** - CellReference doesn't track or store rowspan value
3. **No Grid Reservation** - BuildStyles doesn't reserve cells in subsequent rows for rowspan
4. **No Layout Skipping** - DoLayoutRowCells doesn't skip cells reserved by rowspan from previous rows
5. **No Height Combination** - No mechanism to combine heights across multiple spanned rows
6. **No Page Break Handling** - Rowspan crossing page breaks not handled

---

## Proposed Implementation Strategy

### Phase 1: Core Component Support

#### 1.1 Add RowSpan Property to TableCell

**File:** `Scryber.Components/Components/TableCell.cs`

```csharp
#region public int RowSpan {get;set;}

/// <summary>
/// Gets or sets the row span for this cell.
/// Default is 1 (no spanning).
/// </summary>
[PDFAttribute("row-span")]
[PDFJSConvertor("scryber.studio.design.convertors.integer_attr", JSParams = "\"rowspan\"")]
[PDFDesignable("Row Span", Category = "General", Priority = 2, Type = "Number")]
public virtual int CellRowSpan
{
    get
    {
        if (this.HasStyle)
            return this.Style.GetValue(StyleKeys.TableCellRowSpanKey, 1);
        else
            return 1;
    }
    set
    {
        if (value <= 0)
            this.Style.Table.RemoveCellRowSpan();
        else
            this.Style.Table.CellRowSpan = value;
    }
}

#endregion
```

**Add to StyleKeys:**
- Add `TableCellRowSpanKey` constant to `StyleKeys.cs`
- Add properties to `TableStyleDefinition` or style table extension

#### 1.2 Update CellReference Class

**File:** `Scryber.Components/PDF/Layout/LayoutEngineTable.cs` (CellReference inner class)

```csharp
private int _rowspan;  // NEW

public int RowSpan
{
    get { return _rowspan; }
    set { _rowspan = value; }
}

// In PopulateStyleValues() method, add:
StyleValue<int> rowspan;
if (fullstyle.TryGetValue(StyleKeys.TableCellRowSpanKey, out rowspan))
    _rowspan = rowspan.Value(fullstyle);
else
    _rowspan = 1;
```

### Phase 2: Grid Building and Reservation

#### 2.1 Enhance BuildStyles() Method

**Current Behavior:**
- Builds style arrays accounting only for colspan
- Creates placeholder nulls for colspan

**New Behavior:**
1. First pass: Build initial grid with colspan support (existing)
2. Second pass: Scan for rowspan cells and reserve rows
3. Third pass: Validate grid integrity

**Algorithm:**

```csharp
private void BuildStyles(out int rowcount, out int columncount)
{
    // PHASE 1: Existing colspan handling
    // Build initial arrays with colspan accounted for...
    
    // PHASE 2: NEW - Rowspan Reservation
    // After building initial style arrays, scan for rowspan cells:
    for (int r = 0; r < rowcount; r++)
    {
        for (int c = 0; c < columncount; c++)
        {
            if (_cellfullstyles[r, c] != null)  // This is an actual cell
            {
                // Get rowspan value from this cell
                StyleValue<int> rowspanVal;
                int rowspan = 1;
                if (_cellfullstyles[r, c].TryGetValue(
                    StyleKeys.TableCellRowSpanKey, out rowspanVal))
                {
                    rowspan = rowspanVal.Value(_cellfullstyles[r, c]);
                }
                
                // If rowspan > 1, reserve cells in following rows
                if (rowspan > 1)
                {
                    for (int rs = 1; rs < rowspan; rs++)
                    {
                        int targetRow = r + rs;
                        if (targetRow < rowcount)
                        {
                            // Mark this cell as "spanned by rowspan"
                            // Use existing cell reference but mark as owning row offset
                            _cellfullstyles[targetRow, c] = 
                                new RowspanPlaceholder(_cellfullstyles[r, c], 
                                                       r, c, rs);
                        }
                    }
                }
            }
        }
    }
    
    // PHASE 3: Validate no conflicts
    // Check that colspan + rowspan combinations are valid
}
```

**Important Design Decision**: Use a marker approach where rowspan cells are marked but NOT replaced with nulls. Instead, we'll track rowspan information in a separate data structure to preserve the original cell reference.

#### 2.2 Create RowspanTracking Data Structure

**New Class:** `RowspanCellMarker`

```csharp
private class RowspanCellMarker
{
    public int SourceRowIndex { get; set; }      // Original row
    public int SourceColIndex { get; set; }      // Original col
    public int CurrentRowOffset { get; set; }    // How many rows down (0-based)
    public int TotalRowSpan { get; set; }        // Total rows spanned
    public CellReference SourceCell { get; set; } // Reference to actual cell
    
    public bool IsFirstRow => CurrentRowOffset == 0;
    public bool IsLastRow => CurrentRowOffset == TotalRowSpan - 1;
}
```

**Storage:**
```csharp
// Add to LayoutEngineTable class
private Dictionary<string, RowspanCellMarker> _rowspanMarkers = 
    new Dictionary<string, RowspanCellMarker>();
    // Key: "rowindex_colindex", Value: marker info
```

### Phase 3: Layout Processing

#### 3.1 Modify DoLayoutRowCells()

**Current Logic:**
- Iterates through cells in a row
- Calculates width and height for each cell
- Advances column index by colspan

**New Logic:**
- Before processing a cell in position [row, col]:
  1. Check if this position is marked as "occupied by rowspan"
  2. If occupied, skip to next available column
  3. If this is a cell with rowspan, mark subsequent rows
  4. Track rowspan cells for height calculation

**Implementation Pseudocode:**

```csharp
private Unit DoLayoutRowCells(TableRow row, int rowindex, bool repeating)
{
    Unit offsetX = Unit.Zero;
    int cellcount = _tblRef.AllCells.GetLength(1);
    int cellsprocessed = 0;
    int cellindex = 0;
    List<CellReference> rowspanCellsThisRow = new List<CellReference>();
    
    while (cellsprocessed < cellcount && cellindex < cellcount)
    {
        CellReference cref = _tblRef.AllCells[rowindex, cellindex];
        
        // NEW: Check if this cell is occupied by rowspan from above
        if (IsPositionOccupiedByRowspan(rowindex, cellindex))
        {
            // Mark this position as handled by rowspan cell
            cellindex++;
            cellsprocessed++;
            continue;
        }
        
        if (cref == null)
        {
            // Existing logic
            cellindex++;
            cellsprocessed++;
            continue;
        }
        
        // Existing colspan width calculation...
        Unit w = Unit.Zero;
        for (int i = 0; i < cref.ColumnSpan; i++)
        {
            w += _widths[cellindex + i].Size;
        }
        
        // NEW: Check rowspan for height calculation
        if (cref.RowSpan > 1)
        {
            rowspanCellsThisRow.Add(cref);
            // Don't layout yet - layout when we reach the cell
        }
        
        // Layout the cell (existing)
        if (cref.IsEmpty == false && cref.Cell.Visible)
        {
            this.DoLayoutARowCell(cref, cref.Cell, cellindex, rowindex, repeating);
        }
        
        // Advance by colspan (existing)
        for (int i = 0; i < cref.ColumnSpan; i++)
        {
            this._rowblock.MoveToNextRegion(forced, Unit.Zero, this.Context);
        }
        
        cellindex += cref.ColumnSpan;
        cellsprocessed++;
    }
    
    // Calculate row height considering rowspan
    Unit maxh = this._tblRef.GetMaxCellHeightForRow(rowindex);
    
    // NEW: For cells that started rowspan in previous rows,
    // ensure they're allocated proper total height
    AdjustRowspanCellHeights(rowindex);
    
    this._tblRef.SetCellHeightForRow(rowindex, maxh);
    return maxh;
}

// NEW helper method
private bool IsPositionOccupiedByRowspan(int rowindex, int colindex)
{
    string key = $"{rowindex}_{colindex}";
    return _rowspanMarkers.ContainsKey(key);
}

// NEW helper method
private void AdjustRowspanCellHeights(int rowindex)
{
    // Find all rowspan cells that span past this row
    foreach (var marker in _rowspanMarkers.Values)
    {
        if (marker.SourceRowIndex < rowindex && 
            rowindex < marker.SourceRowIndex + marker.TotalRowSpan)
        {
            // This rowspan cell extends into this row
            // Need to update cumulative height
            marker.SourceCell.SetCumulativeHeightAdjustment(
                this._tblRef.GetMaxCellHeightForRow(rowindex));
        }
    }
}
```

#### 3.2 Height Combination Algorithm

**Challenge:** A cell with rowspan=3 needs to have height equal to the sum of heights in rows 1, 2, and 3 (plus row spacing).

**Solution:** Use a post-layout pass to calculate combined heights:

```csharp
private void CalculateRowspanCellHeights()
{
    foreach (var marker in _rowspanMarkers.Values)
    {
        if (!marker.IsFirstRow) continue;  // Only process from first row
        
        CellReference cell = marker.SourceCell;
        Unit totalHeight = 0;
        
        // Sum heights of all spanned rows
        for (int r = 0; r < marker.TotalRowSpan; r++)
        {
            int targetRow = marker.SourceRowIndex + r;
            totalHeight += this._tblRef.GetMaxCellHeightForRow(targetRow);
        }
        
        // Apply combined height to the cell block
        if (cell.Block != null)
        {
            Rect bounds = cell.Block.TotalBounds;
            bounds.Height = totalHeight;
            cell.Block.TotalBounds = bounds;
        }
    }
}
```

### Phase 4: Edge Cases and Complex Scenarios

#### 4.1 Rowspan + Colspan Interaction

**Scenario:** Cell at (0,0) with colspan=2, rowspan=2

**Expected Grid:**
```
    Col 0    Col 1    Col 2
Row 0: [CELL(0,0) - spans 2x2] [Col2Cell]
Row 1: [occupied by (0,0)]      [occupied by (0,0)] [Col2Cell]
Row 2: [Cell(2,0)]              [Cell(2,1)]         [Cell(2,2)]
```

**Implementation:** In BuildStyles(), when reserving rowspan cells, also account for their colspan:

```csharp
// When rowspan cell spans columns c to c+colspan-1
for (int rs = 1; rs < rowspan; rs++)
{
    for (int cs = 0; cs < colspan; cs++)
    {
        int targetRow = r + rs;
        int targetCol = c + cs;
        _cellfullstyles[targetRow, targetCol] = 
            new RowspanPlaceholder(...);
    }
}
```

#### 4.2 Invalid Rowspan (Extends Past Table End)

**Scenario:** Cell at row 8 with rowspan=5 but table only has 10 total rows

**Handling:**
```csharp
if (r + rowspan > rowcount)
{
    // Truncate to available rows
    rowspan = rowcount - r;
    this.Context.TraceLog.Add(
        TraceLevel.Warning, 
        TableEngineLogCategory,
        $"Cell rowspan extended past table end at row {r}. Truncated to {rowspan}");
}
```

#### 4.3 Rowspan Across Page Breaks and Content Overflow

**Challenge:** When table content overflows to a new page, rowspan cells must be handled properly, especially when cell content exceeds available height.

**Current Page Break Handling:**
- Table creates new GridReference when overflow occurs
- Repeating header rows are handled specially
- Individual cells can overflow based on style settings

**Overflow Behavior Rules:**

1. **When Cell Content Overflows:**
   - If style allows overflow (default): Move entire affected row(s) to new page
   - If style is `overflow: clip`: Truncate content, keep row on current page
   - If style is `overflow: visible`: Content may render beyond page boundary (with warning)
   - If style is `overflow: hidden`: Hide overflow content, keep row on current page

2. **Rowspan Cell Overflow:**
   - When a cell with rowspan content overflows, ALL rows it spans must move to new page
   - This ensures rowspan cell content and occupied rows stay visually aligned
   - Rowspan spanning across page boundary is generally avoided by moving whole row group

3. **Cascade Effect:**
   - If Cell A (rowspan=2) overflows and takes rows 0-1 to new page
   - Even if Cell B (normal cell in row 1) has room, it must also move
   - This maintains table integrity and visual alignment

**Proposed Solution:**

```csharp
/// <summary>
/// Determines if a row group should move to new page based on rowspan overflow
/// </summary>
private bool ShouldMoveRowGroupToNewPage(int startRowIndex, 
    int endRowIndex, Unit availableHeight)
{
    // Check all cells in row range for overflow
    for (int r = startRowIndex; r <= endRowIndex; r++)
    {
        for (int c = 0; c < this.AllCells.TotalColumnCount; c++)
        {
            CellReference cref = this.AllCells.AllCells[r, c];
            if (cref == null || cref.IsEmpty) continue;
            
            // Get actual calculated height
            Unit requiredHeight = GetCellRequiredHeight(cref);
            
            // Check if content overflows available space
            if (requiredHeight > availableHeight)
            {
                // Check overflow style setting
                var overflowStyle = cref.FullStyle.GetValue(
                    StyleKeys.PositionOverflowKey, 
                    OverflowAction.Auto);  // Default allows overflow
                
                if (overflowStyle == OverflowAction.Auto || 
                    overflowStyle == OverflowAction.Visible)
                {
                    return true;  // Move row group to new page
                }
                // If Clip or Hidden, row stays on current page
            }
        }
    }
    return false;
}

/// <summary>
/// Gets the affected row range for a given rowspan cell
/// </summary>
private (int startRow, int endRow) GetRowspanAffectedRange(
    CellReference rowspanCell)
{
    int start = rowspanCell.RowIndex;
    int end = rowspanCell.RowIndex + rowspanCell.RowSpan - 1;
    
    // Also check if other rowspan cells in this range extend it
    for (int r = start; r <= end; r++)
    {
        for (int c = 0; c < this.AllCells.TotalColumnCount; c++)
        {
            CellReference other = this.AllCells.AllCells[r, c];
            if (other != null && other.RowSpan > 1)
            {
                int otherEnd = other.RowIndex + other.RowSpan - 1;
                if (otherEnd > end)
                    end = otherEnd;
            }
        }
    }
    
    return (start, end);
}

/// <summary>
/// Handles page break with rowspan cell overflow
/// </summary>
private void HandleRowspanPageBreakWithOverflow(int breakRow, 
    Unit availableHeight)
{
    // Find all rowspan cells that would be affected
    var affectedRowspanCells = GetRowspanCellsCrossingBreak(breakRow);
    
    foreach (var rowspanCell in affectedRowspanCells)
    {
        var (startRow, endRow) = GetRowspanAffectedRange(rowspanCell);
        
        // Check if this row group should move
        if (ShouldMoveRowGroupToNewPage(startRow, endRow, availableHeight))
        {
            // Move entire row group to new page
            MoveRowGroupToNewPage(startRow, endRow);
            
            this.Context.TraceLog.Add(TraceLevel.Verbose, 
                TableEngineLogCategory,
                $"Moved row group {startRow}-{endRow} to new page due to " +
                $"rowspan cell overflow");
            
            return;  // Page break handled
        }
    }
    
    // If no rowspan overflow detected, use standard page break handling
    HandleStandardPageBreak(breakRow);
}

/// <summary>
/// Clips cell content if overflow style is set to clip
/// </summary>
private void ClipCellContentIfNeeded(CellReference cref, Unit maxHeight)
{
    var overflowStyle = cref.FullStyle.GetValue(
        StyleKeys.PositionOverflowKey, 
        OverflowAction.Auto);
    
    if (overflowStyle == OverflowAction.Clip || 
        overflowStyle == OverflowAction.Hidden)
    {
        if (cref.Block != null)
        {
            // Constrain block height to available space
            Rect bounds = cref.Block.TotalBounds;
            bounds.Height = Unit.Min(bounds.Height, maxHeight);
            cref.Block.TotalBounds = bounds;
            
            // Mark overflow content as hidden
            if (cref.Block.LastOpenBlock() != null)
                cref.Block.LastOpenBlock().Overflow = 
                    overflowStyle == OverflowAction.Hidden ? 
                    OverflowAction.Hidden : OverflowAction.Clip;
        }
    }
}

private List<CellReference> GetRowspanCellsCrossingBreak(int breakRow)
{
    var result = new List<CellReference>();
    
    // Find cells that span across the break row
    for (int r = 0; r < breakRow; r++)
    {
        for (int c = 0; c < this.AllCells.TotalColumnCount; c++)
        {
            CellReference cref = this.AllCells.AllCells[r, c];
            if (cref != null && cref.RowSpan > 1)
            {
                int endRow = cref.RowIndex + cref.RowSpan - 1;
                if (endRow >= breakRow)
                    result.Add(cref);
            }
        }
    }
    
    return result;
}

/// <summary>
/// Expands affected row range accounting for OTHER rowspan cells
/// that span INTO the current affected range from above.
/// This handles the cascade of dependencies upward.
/// </summary>
private (int startRow, int endRow) ExpandRowRangeForOtherRowspans(
    int startRow, int endRow)
{
    bool expanded = true;
    
    // Keep expanding until no more cells span into the range from above
    while (expanded)
    {
        expanded = false;
        
        // Look for any rowspan cells ABOVE startRow that span INTO the range
        for (int r = 0; r < startRow; r++)
        {
            for (int c = 0; c < this.AllCells.TotalColumnCount; c++)
            {
                CellReference other = this.AllCells.AllCells[r, c];
                if (other != null && other.RowSpan > 1)
                {
                    int otherEnd = other.RowIndex + other.RowSpan - 1;
                    
                    // Does this cell span INTO our current range?
                    if (otherEnd >= startRow && other.RowIndex < startRow)
                    {
                        // YES - We must move this cell too!
                        // Expand range to include this cell's start
                        int newStart = other.RowIndex;
                        
                        if (newStart < startRow)
                        {
                            startRow = newStart;
                            expanded = true;
                            
                            this.Context.TraceLog.Add(TraceLevel.Verbose,
                                TableEngineLogCategory,
                                $"Row range expanded upward from {startRow} to " +
                                $"account for rowspan cell at ({other.RowIndex}," +
                                $"{other.ColumnIndex})");
                        }
                    }
                }
            }
        }
    }
    
    return (startRow, endRow);
}

private void MoveRowGroupToNewPage(int startRow, int endRow)
{
    // Create new grid for overflow on new page
    GridReference newGrid = CreateNewPageGrid();
    
    // Copy all rows in affected range to new grid
    for (int r = startRow; r <= endRow; r++)
    {
        RowReference rowRef = this.AllCells.AllRows[r];
        // Re-layout this row on the new page
        newGrid.AddRow(rowRef);
    }
    
    // Update current position
    this.AllCells.SetCurrentGrid(newGrid);
}

private void HandleStandardPageBreak(int breakRow)
{
    // Existing page break logic for non-rowspan scenarios
    GridReference newGrid = CreateNewPageGrid();
    this.AllCells.SetCurrentGrid(newGrid);
}
```

**Height Calculation with Overflow:**

When a rowspan cell has content that overflows, its height must accommodate all spanned rows:

```csharp
private Unit GetCellRequiredHeight(CellReference cref)
{
    if (cref.Block == null)
        return Unit.Zero;
    
    // For normal cells, return block height
    if (cref.RowSpan == 1)
        return cref.Block.Height;
    
    // For rowspan cells, return the combined height of all spanned rows
    Unit totalHeight = Unit.Zero;
    for (int r = 0; r < cref.RowSpan; r++)
    {
        int targetRow = cref.RowIndex + r;
        totalHeight += this._tblRef.GetMaxCellHeightForRow(targetRow);
    }
    
    // Add spacing between rows (if any)
    if (cref.RowSpan > 1)
        totalHeight += GetRowSpacingTotal(cref.RowIndex, cref.RowSpan);
    
    return totalHeight;
}
```

#### 4.4 Percentage-Based Heights with Rowspan

**Challenge:** If rows have percentage-based heights and a cell spans rows with different percentages

**Solution:**
```csharp
// When calculating combined height for rowspan cells:
// If any row has percentage height, calculate based on:
// 1. Explicit heights from spanned cells
// 2. Content-based minimum height
// 3. Distribute percentage appropriately
```

---

## Implementation Phases and Milestones

### Phase 1: Foundation (Days 1-2)
- [x] Add CellRowSpan property to TableCell
- [x] Add RowSpan tracking to CellReference
- [x] Add StyleKeys.TableCellRowSpanKey
- [ ] Unit tests for property setting/getting

### Phase 2: Grid Building (Days 2-3)
- [ ] Implement RowspanCellMarker class
- [ ] Update BuildStyles() for rowspan reservation
- [ ] Create rowspan marker tracking dictionary
- [ ] Unit tests for grid building with rowspan

### Phase 3: Layout Processing (Days 3-4)
- [ ] Implement IsPositionOccupiedByRowspan()
- [ ] Update DoLayoutRowCells() to skip rowspan positions
- [ ] Implement height combination algorithm
- [ ] Unit tests for basic rowspan layout

### Phase 4: Edge Cases (Days 4-5)
- [ ] Handle colspan + rowspan interaction
- [ ] Implement invalid rowspan truncation
- [ ] Handle page break scenarios with overflow detection
- [ ] Implement cascade effect (when one cell overflows, move all affected rows)
- [ ] Implement upward cascade (other rowspan cells spanning INTO affected rows must move too)
- [ ] Implement clip/hidden overflow handling
- [ ] Handle percentage heights with rowspan
- [ ] Unit tests for all edge cases (now 12 page break tests instead of 4)

### Phase 5: Integration & Polish (Days 5-6)
- [ ] Integration tests with real documents
- [ ] Performance validation
- [ ] Documentation updates
- [ ] Sample templates
- [ ] Final validation tests

---

## Test Strategy

### Unit Test Structure

All tests in: `Scryber.UnitLayouts/TableRowspan_Tests.cs` (new file)

### Test Categories

#### A. Property Tests (5 tests)

```csharp
[TestClass]
public class TableRowspan_Tests
{
    [TestMethod]
    public void TableCell_RowSpan_DefaultValue()
    {
        var cell = new TableCell();
        Assert.AreEqual(1, cell.CellRowSpan);
    }
    
    [TestMethod]
    public void TableCell_RowSpan_SetAndGet()
    {
        var cell = new TableCell();
        cell.CellRowSpan = 3;
        Assert.AreEqual(3, cell.CellRowSpan);
    }
    
    [TestMethod]
    public void TableCell_RowSpan_InvalidValue()
    {
        var cell = new TableCell();
        // Should either ignore or throw
        cell.CellRowSpan = 0;
        Assert.AreEqual(1, cell.CellRowSpan);  // Reverts to default
    }
    
    [TestMethod]
    public void TableCell_RowSpan_NegativeValue()
    {
        var cell = new TableCell();
        cell.CellRowSpan = -5;
        Assert.AreEqual(1, cell.CellRowSpan);  // Reverts to default
    }
    
    [TestMethod]
    public void TableCell_RowSpan_LargeValue()
    {
        var cell = new TableCell();
        cell.CellRowSpan = 100;
        Assert.AreEqual(100, cell.CellRowSpan);
    }
}
```

#### B. Simple Rowspan Tests (8 tests)

Basic rowspan with 2-3 rows, uniform column widths, no colspan:

```csharp
    [TestMethod]
    public void TableRowspan_SimpleRowspan2Rows()
    {
        // Table: 3 rows, 3 columns
        // Cell (0,0) has rowspan=2
        // Expected: Cell height = height of rows 0 and 1 combined
    }
    
    [TestMethod]
    public void TableRowspan_SimpleRowspan3Rows()
    {
        // Table: 4 rows, 2 columns
        // Cell (0,0) has rowspan=3
        // Expected: Cell height = height of rows 0, 1, 2 combined
    }
    
    [TestMethod]
    public void TableRowspan_RowspanMultipleCells()
    {
        // Table: 3 rows, 4 columns
        // Cell (0,0) rowspan=2, Cell (0,2) rowspan=2
        // Expected: Two independent rowspan cells, proper layout
    }
    
    [TestMethod]
    public void TableRowspan_RowspanWithVariableRowHeights()
    {
        // Table: 3 rows with different content heights
        // Cell (0,0) rowspan=2
        // Expected: Cell height accommodates both rows
    }
    
    [TestMethod]
    public void TableRowspan_RowspanVerticalAlignment()
    {
        // Cell with rowspan uses vertical-align style
        // Expected: Content properly aligned within combined height
    }
    
    [TestMethod]
    public void TableRowspan_RowspanWithBorders()
    {
        // Cell with rowspan and borders defined
        // Expected: Border properly spans combined height
    }
    
    [TestMethod]
    public void TableRowspan_RowspanWithBackgroundColor()
    {
        // Cell with rowspan and background color
        // Expected: Background fills combined height
    }
    
    [TestMethod]
    public void TableRowspan_RowspanWithPadding()
    {
        // Cell with rowspan and padding
        // Expected: Padding applied correctly
    }
```

#### C. Colspan + Rowspan Interaction Tests (6 tests)

```csharp
    [TestMethod]
    public void TableRowspan_ColspanAndRowpanSimple()
    {
        // Table: 3 rows, 3 columns
        // Cell (0,0) colspan=2, rowspan=2
        // Expected: Cell occupies 2x2 area, rows 1 cols 0-1 are occupied
    }
    
    [TestMethod]
    public void TableRowspan_MultipleSpanningCells()
    {
        // Cell (0,0): colspan=2, rowspan=2
        // Cell (0,2): colspan=1, rowspan=3  
        // Expected: Proper grid layout with all spans honored
    }
    
    [TestMethod]
    public void TableRowspan_SpanningCellsAdjacent()
    {
        // Multiple adjacent cells with colspan and rowspan
        // Expected: No overlap, proper padding between
    }
    
    [TestMethod]
    public void TableRowspan_WidthCalcWithColspanRowspan()
    {
        // Table with columns of different widths
        // Cell with both colspan and rowspan
        // Expected: Width calculated correctly across columns
    }
    
    [TestMethod]
    public void TableRowspan_HeightCalcWithColspanRowspan()
    {
        // Table with rows of different heights
        // Cell with both colspan and rowspan
        // Expected: Height calculated correctly across rows
    }
    
    [TestMethod]
    public void TableRowspan_NestedTableWithRowspan()
    {
        // Cell with rowspan contains nested table
        // Expected: Nested table fits in combined cell height
    }
```

#### D. Grid Building and Validation Tests (5 tests)

```csharp
    [TestMethod]
    public void TableRowspan_GridBuilding_BasicGrid()
    {
        // Build grid with simple rowspan
        // Verify internal grid structure is correct
    }
    
    [TestMethod]
    public void TableRowspan_GridBuilding_ComplexGrid()
    {
        // Complex grid with multiple spanning cells
        // Verify all positions properly mapped
    }
    
    [TestMethod]
    public void TableRowspan_GridBuilding_EdgeCase_Truncation()
    {
        // Cell has rowspan=5 but table only 3 rows
        // Expected: Rowspan truncated, no exception
    }
    
    [TestMethod]
    public void TableRowspan_GridBuilding_InvalidRowspan()
    {
        // Cell with rowspan=0 or negative
        // Expected: Treated as rowspan=1
    }
    
    [TestMethod]
    public void TableRowspan_GridBuilding_LargeRowspan()
    {
        // Cell with rowspan=50 in small table
        // Expected: Properly handled without performance issues
    }
```

#### E. Layout Position Tests (5 tests)

```csharp
    [TestMethod]
    public void TableRowspan_LayoutPositions_SkipsOccupiedCells()
    {
        // Verify layout engine skips cells occupied by rowspan
        // Check that cell indices advance correctly
    }
    
    [TestMethod]
    public void TableRowspan_LayoutPositions_RowspanStartCell()
    {
        // Cell that starts rowspan is positioned correctly
    }
    
    [TestMethod]
    public void TableRowspan_LayoutPositions_RowspanContinuationRows()
    {
        // Rows that are covered by rowspan are positioned correctly
    }
    
    [TestMethod]
    public void TableRowspan_LayoutPositions_MixedRowspanCells()
    {
        // Multiple rowspan cells at different positions
        // All positioned correctly relative to each other
    }
    
    [TestMethod]
    public void TableRowspan_LayoutPositions_RowspanWithEmptyCells()
    {
        // Some cells empty, some with rowspan
        // Positions adjust correctly for both
    }
```

#### F. Visual Rendering Tests (6 tests)

```csharp
    [TestMethod]
    public void TableRowspan_Rendering_SimpleRowspanPDF()
    {
        // Generate PDF with simple rowspan
        // Visually validate layout matches expected
    }
    
    [TestMethod]
    public void TableRowspan_Rendering_ComplexRowspanPDF()
    {
        // Generate PDF with complex spanning
        // Visually validate all cells properly positioned
    }
    
    [TestMethod]
    public void TableRowspan_Rendering_RowspanWithStyling()
    {
        // Generate PDF with styled rowspan cells
        // Verify borders, colors, fonts render correctly
    }
    
    [TestMethod]
    public void TableRowspan_Rendering_RowspanWithContent()
    {
        // Generate PDF with rowspan cells containing rich content
        // Verify content flows properly
    }
    
    [TestMethod]
    public void TableRowspan_Rendering_RowspanMultiplePages()
    {
        // Generate PDF where table spans multiple pages
        // Verify rowspan handling across page breaks (if implemented)
    }
    
    [TestMethod]
    public void TableRowspan_Rendering_RowspanComparisonWithHTML()
    {
        // Generate same table from HTML with rowspan
        // Compare PDF output with expected browser rendering
    }
```

#### G. Page Break Handling Tests (9 tests)

```csharp
    [TestMethod]
    public void TableRowspan_PageBreak_RowspanEndsBeforeBreak()
    {
        // Cell with rowspan ends before page break
        // Expected: Clean layout on both pages
    }
    
    [TestMethod]
    public void TableRowspan_PageBreak_RowspanStartsBeforeBreak()
    {
        // Cell with rowspan spans past page break
        // Expected: Row group moves to new page to maintain rowspan integrity
    }
    
    [TestMethod]
    public void TableRowspan_PageBreak_MultipleRowspanCellsAcrossBreak()
    {
        // Multiple cells with rowspan across page boundary
        // Expected: Proper continuation on new page, all affected rows move together
    }
    
    [TestMethod]
    public void TableRowspan_PageBreak_RowspanWithRepeatingHeaders()
    {
        // Table with repeating headers and rowspan cells
        // Expected: Headers repeat, rowspan persists across pages
    }
    
    [TestMethod]
    public void TableRowspan_PageBreak_RowspanContentOverflow_AllowOverflow()
    {
        // Cell with rowspan has content that overflows available page height
        // Overflow style: auto (default - allows overflow)
        // Expected: Entire affected row group moves to new page
    }
    
    [TestMethod]
    public void TableRowspan_PageBreak_RowspanContentOverflow_Clip()
    {
        // Cell with rowspan content overflows, but overflow: clip is set
        // Expected: Content clipped on current page, row stays in place
    }
    
    [TestMethod]
    public void TableRowspan_PageBreak_RowspanContentOverflow_Hidden()
    {
        // Cell with rowspan content overflows, overflow: hidden
        // Expected: Overflow hidden, row stays on current page
    }
    
    [TestMethod]
    public void TableRowspan_PageBreak_CascadeOverflow_OneRowspanTriggersMove()
    {
        // Multiple cells: A (rowspan=2), B (normal in row 1)
        // A's content overflows
        // Expected: Both rows move to new page, B moves with them for alignment
    }
    
    [TestMethod]
    public void TableRowspan_PageBreak_RowspanHeightCalculationWithOverflow()
    {
        // Cell with rowspan across multiple pages
        // Expected: Height correctly calculated combining all spanned rows
        // Content sized appropriately for combined height
    }
    
    [TestMethod]
    public void TableRowspan_PageBreak_NestedRowspanCells_UpwardCascade()
    {
        // Complex scenario: 
        // Cell A (row 0): rowspan=5, has content overflow (70pt required)
        // Cell B (row 2): rowspan=3 (contained within A's span)
        // Cell C (row 4): normal cell
        // Available space: 60pt
        // Expected: Because A overflows, rows 0-5 move together
        //           Even though B starts in row 2, it must move with its parent context
        //           Row 4 moves with them for alignment
    }
    
    [TestMethod]
    public void TableRowspan_PageBreak_MultipleIndependentRowspans_UpwardCascade()
    {
        // Scenario:
        // Cell A (row 0): rowspan=2, content=65pt (overflows 60pt available)
        // Cell D (row 1): normal, within A span
        // Cell B (row 3): rowspan=2, content=40pt (no overflow)
        // Cell E (row 4): normal, within B span
        // Expected: A overflows → rows 0-1 move
        //           B doesn't overflow but... Check if B needs to move
        //           (B stays on page since no overflow cascade)
    }
    
    [TestMethod]
    public void TableRowspan_PageBreak_RowspanStaircasePattern()
    {
        // Staircase of rowspan cells:
        // Row 0: Cell A (rowspan=3), Column 0
        // Row 1: Cell B (rowspan=2), Column 1
        // Row 2: Cell C (rowspan=1), Column 2
        // Row 3: Empty
        // If A overflows → rows 0-2 move
        // If B overflows (but A doesn't) → rows 1-2 move, 
        //    but does it force A to move too?
        // Expected: B overflowing doesn't force A to move
        //           (A already spanned but not the cause)
    }


#### H. Data Binding Tests (3 tests)

```csharp
    [TestMethod]
    public void TableRowspan_DataBinding_DynamicRowspan()
    {
        // Table with rowspan using data binding {{model.rowSpanValue}}
        // Expected: Rowspan value populated from data
    }
    
    [TestMethod]
    public void TableRowspan_DataBinding_ConditionalRowspan()
    {
        // Rowspan value conditional based on data
        // Expected: Correct rowspan applied based on condition
    }
    
    [TestMethod]
    public void TableRowspan_DataBinding_RowspanInDataBoundTable()
    {
        // Table generated via {{#each}} with rowspan cells
        // Expected: Rowspan properly applied in looped rows
    }
```

#### I. Performance and Stress Tests (3 tests)

```csharp
    [TestMethod]
    public void TableRowspan_Performance_LargeTable()
    {
        // 50 rows x 20 columns with multiple rowspan cells
        // Measure layout time
        // Expected: Completes in < 1 second
    }
    
    [TestMethod]
    public void TableRowspan_Performance_ComplexSpanning()
    {
        // Table with dense colspan + rowspan combinations
        // Measure layout time
        // Expected: No performance degradation
    }
    
    [TestMethod]
    public void TableRowspan_Performance_ManyRowspanCells()
    {
        // Table with 100+ cells having rowspan
        // Expected: Efficient marker tracking
    }
```

#### J. Error Handling and Validation Tests (4 tests)

```csharp
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TableRowspan_Validation_RowspanZero()
    {
        // Verify rowspan=0 produces appropriate error or warning
    }
    
    [TestMethod]
    public void TableRowspan_Validation_RowspanExceedsTable()
    {
        // Cell rowspan exceeds table row count
        // Expected: Truncated with warning, no error
    }
    
    [TestMethod]
    public void TableRowspan_Validation_RowspanWithMissingCells()
    {
        // Row has fewer cells than expected due to spanning
        // Expected: Properly handled
    }
    
    [TestMethod]
    public void TableRowspan_Validation_RowspanWithEmptyRows()
    {
        // Some rows completely spanned (no non-spanning cells)
        // Expected: Proper layout without gaps
    }
```

#### K. HTML Parser Tests (2 tests)

```csharp
    [TestMethod]
    public void TableRowspan_Parser_HTMLRowspanAttribute()
    {
        // Parse HTML: <td rowspan="3">Content</td>
        // Expected: TableCell.CellRowSpan = 3
    }
    
    [TestMethod]
    public void TableRowspan_Parser_HTMLRowspanWithColspan()
    {
        // Parse HTML: <td colspan="2" rowspan="2">Content</td>
        // Expected: Both attributes parsed correctly
    }
```

---

## Test Data Scenarios

### Scenario 1: Simple 2x2 Rowspan
```
┌─────┬─────┐
│     │ B   │
│  A  ├─────┤
│ (2x1)│ C   │
└─────┴─────┘
```
- Cell A: rowspan=2
- Cell B: normal
- Cell C: normal

### Scenario 2: Complex Multi-Spanning
```
┌─────┬─────┬─────┐
│  A  │  B (colspan=2)│
│(1x2)├─────┼─────┤
│     │  C  │  D  │
└─────┴─────┴─────┘
```
- Cell A: rowspan=2, colspan=1
- Cell B: colspan=2
- Cells C, D: normal

### Scenario 3: Dense Grid
```
┌─────┬─────┬─────┬─────┐
│  A  │  B  │ C (colspan=2) │
│(2x1)├─────┼─────┬─────┤
│     │  D  │  E  │  F  │
├─────┼─────┼─────┼─────┤
│  G  │  H  │  I  │  J  │
└─────┴─────┴─────┴─────┘
```

### Scenario 4: Page Break with Rowspan Overflow (New)
```
Page 1:
┌─────┬─────┐
│  A  │  B  │
│(2x1)│     │  <- Available space: 50pt
│     ├─────┤    Cell A content: 80pt
└─────┴─────┘    ** OVERFLOW **

Expected Behavior (overflow: auto):
Page 1: [Empty or partial]
Page 2:
┌─────┬─────┐
│  A  │  B  │  <- Both rows moved
│     ├─────┤
│     │  C  │
└─────┴─────┘
```

### Scenario 5: Cascade Overflow Effect (New)
```
┌─────┬─────┬─────┐
│  A  │  B  │  C  │
│(2x1)│     │     │  <- A overflows
│     ├─────┼─────┤     B and C have room
│     │  D  │  E  │

Expected Behavior:
All three rows (0, 1, 2) move to new page because A's overflow
requires row 1, which in turn requires row 0 for visual alignment
```

### Scenario 6: Clip Vs. Overflow (New)
```
Same layout as Scenario 4, but:

overflow: clip
Result: Content clipped at 50pt, rows stay on page 1

overflow: hidden  
Result: Content hidden, rows stay on page 1

overflow: auto (default)
Result: Rows move to page 2
```

### Scenario 7: Edge Cases
- All cells in column have rowspan
- Single row table with rowspan (should be no-op)
- Rowspan=1 (default, should behave like normal)
- Very large rowspan (100+)
- Rowspan content larger than entire page height (overflow handling)

### Scenario 8: Upward Cascade - Nested Rowspan Dependencies (New)
```
Page Break Cascading Effect:

┌─────┬─────┬─────┐
│  A  │  B  │  C  │  <- Row 0 (start of A's rowspan)
│(3x1)├─────┼─────┤     Available: 60pt
│     │  D  │     │  <- Row 1
│(2x1)├─────┼─────┤     Cell A content: 70pt → OVERFLOWS
│  E  │  F  │     │  <- Row 2 (start of B's rowspan)
│     ├─────┼─────┤     Cell B content: 40pt (no overflow)
│     │  G  │     │  <- Row 3
└─────┴─────┴─────┘

Cascade Decision Process:
1. Cell A content=70pt > 60pt available → OVERFLOW
2. Cell A rowspan=3 → affects rows [0, 1, 2]
3. Check for OTHER rowspan cells IN rows [0, 1, 2]:
   ✓ Cell B is in row 1, rowspan=2 → spans [1, 2]
4. Cell B is CONTAINED within A's span, but:
   - B doesn't overflow → no upward cascade from B
   - A's overflow is sufficient to move rows [0, 1, 2]
5. Result: Rows [0, 1, 2] move to new page

BUT - Check for rowspan cells ABOVE that span INTO [0, 1, 2]:
   None exist (A starts at row 0)
   
Conclusion: Move rows 0-2 to new page
```

### Scenario 9: Complex Upward Cascade (New)
```
Problematic Pattern:

┌─────┬─────┐
│  A  │  B  │  <- Row 0
│(2x1)│     │     A spans [0, 1]
│     ├─────┤     A content: 30pt (fits in 60pt)
│     │  C  │  <- Row 1
├─────┼─────┤
│  D  │  E  │  <- Row 2
│(2x1)│     │     D spans [2, 3]
│     ├─────┤     D content: 70pt → OVERFLOWS
│     │  F  │  <- Row 3
└─────┴─────┘

Step 1: Cell D overflows → rows [2, 3] must move
Step 2: Check for rowspan cells ABOVE row 2 that span INTO rows [2, 3]:
        Cell A is at rows [0, 1]
        Cell A's span END (1) does NOT reach row 2
        → Cell A does NOT span into [2, 3]
        → Do NOT force A to move

Result: Move only rows [2, 3] to new page
         Rows [0, 1] stay on page 1
         Page 1 has rows [0, 1], Page 2 has rows [2, 3]
         CLEAN BREAK - no dependencies
```

### Scenario 10: TRUE Upward Cascade Dependency (New - Critical!)
```
Critical Case Where Upper Rowspan MUST Move:

┌─────┬─────┐
│  A  │  B  │  <- Row 0
│(3x1)│     │     A spans [0, 1, 2]
│     ├─────┤     A content: 30pt (fits in 60pt)
│     │  C  │  <- Row 1
├─────┼─────┤
│  D  │  E  │  <- Row 2 ← A's range ENDS here
│(1x1)│     │     D content: 70pt → OVERFLOWS
├─────┼─────┤
│  F  │  G  │  <- Row 3
└─────┴─────┘

Step 1: Cell D overflows → rows [2] must move
Step 2: Check for rowspan cells ABOVE row 2 that span INTO row 2:
        Cell A at rows [0, 1, 2]
        Cell A's span END (2) == row 2
        → Cell A SPANS INTO row [2]
        → IT MUST MOVE TOO!
        
Step 3: Expand affected range upward:
        New range = [0, 1, 2] (includes A's entire span)
        
Step 4: Check again for cells above row 0 spanning into [0, 1, 2]:
        None exist
        
Result: Move rows [0, 1, 2] to new page
        Why? Row 2 must move (D overflows)
             Row 2 is part of A's rowspan
             A cannot be split across pages
             Therefore A's entire span [0, 1, 2] moves together
```

This is the **critical edge case**: Other rowspan cells that span INTO the affected rows must move too, even if they don't overflow themselves.
```
```

---

## Success Criteria

### Functional Requirements
- ✓ `CellRowSpan` property accessible and settable
- ✓ Rowspan value properly extracted from styles
- ✓ Grid building accounts for rowspan cells
- ✓ Cells in spanned rows are skipped during layout
- ✓ Combined heights calculated correctly
- ✓ Rowspan + colspan work together
- ✓ Invalid rowspan values handled gracefully
- ✓ HTML rowspan attributes parsed
- ✓ Cell content overflow detected and handled
- ✓ Overflow style respected (auto/clip/hidden)
- ✓ Cascade effect: one cell's overflow moves all affected rows
- ✓ Upward cascade: rowspan cells spanning INTO affected range also move
- ✓ Row groups move to new page when rowspan content overflows available height
- ✓ Affected rows stay together (no split across pages)
- ✓ Other rowspan cells above don't force unnecessary moves

### Performance Requirements
- ✓ Layout time < 1s for 50x20 table with rowspan
- ✓ No memory leaks with large rowspan grids
- ✓ Marker tracking efficient (< 1MB for typical tables)
- ✓ Overflow detection doesn't add > 5% to layout time

### Code Quality Requirements
- ✓ All new code follows Scryber style guidelines
- ✓ Comprehensive inline documentation
- ✓ No breaking changes to existing colspan behavior
- ✓ All tests passing (25+ unit tests, including 9 page break tests)
- ✓ Code reviewed and approved

### Documentation Requirements
- ✓ Update [attr_colspan_rowspan.md](../docs/reference/htmlattributes/attributes/attr_colspan_rowspan.md)
- ✓ Add rowspan examples to table documentation
- ✓ Document overflow behavior with rowspan
- ✓ Update API documentation (docstrings)
- ✓ Add troubleshooting guide for rowspan issues including overflow handling

---

## Risks and Mitigations

| Risk | Impact | Mitigation |
|------|--------|-----------|
| Overflow detection complexity | High | Implement overflow detection as separate subsystem, reuse existing page break framework |
| Cascade effect correctness | High | Comprehensive tests for various overflow scenarios, trace logging for debugging |
| Page break handling complexity | High | Phased approach: implement basic rowspan first, overflow support in Phase 4 |
| All affected rows moving together | Medium | Validate row group calculation in unit tests before layout |
| Performance with dense spanning | Medium | Efficient marker tracking using Dictionary, early testing with stress loads |
| Interaction with colspan | High | Comprehensive edge case tests before final release |
| Backward compatibility | High | Verify colspan tests still pass, no changes to existing colspan logic |
| Complex grid calculations | Medium | Use existing grid framework, minimal new math required |

---

## Appendix A: Overflow Behavior Detail

### Overflow Style Handling with Rowspan

When cell content exceeds available space on a page, the layout engine respects the CSS `overflow` property:

#### Auto (Default) Behavior
```
overflow: auto (or not specified)

Position | Behavior
---------|----------
Middle of table | Move affected row group to new page
Near page bottom | Check if row group fits; if not, move to new page
At exact boundary | Move to new page to avoid split

Cascade Effect: If Cell A (rowspan=2) overflows:
- Both rows containing Cell A move to new page
- All other cells in those rows also move (even if they fit)
- This maintains visual alignment and table structure
```

#### Clip Behavior
```
overflow: clip

Result: Content is truncated at cell boundary
- Row stays on current page
- Overflow content is not rendered
- No warning (clips silently as per CSS spec)
- Rowspan cell's height is limited to available space
```

#### Hidden Behavior  
```
overflow: hidden

Result: Content is completely hidden
- Row stays on current page
- Overflow content exists but is not visible
- Rowspan cell's height is limited to available space
- Similar to clip but content still occupies memory
```

### Algorithm: Detect and Handle Rowspan Overflow

**Input:** Current page layout state with available height, rowspan cell, row group
**Output:** Rows remain on page OR row group moves to new page

```
Step 1: Measure Cell Requirements
├─ For each cell in row group:
│  ├─ Calculate required height including margins/padding
│  └─ Compare to available page height
└─ Determine if any cell overflows

Step 2: Check Overflow Style
├─ If overflow = clip OR hidden:
│  └─ Clip cell content, keep row on page
└─ Else (auto/visible):
   └─ Continue to Step 3

Step 3: Identify Affected Rows
├─ Start with overflow cell's row
├─ Expand to include all rows spanned by that cell
├─ Check for other rowspan cells in those rows
└─ Expand if those cells span beyond current range

Step 4: Calculate Total Row Group Height
├─ Sum all row heights in affected range
├─ Add row spacing
└─ Compare to remaining page height

Step 5: Move or Clip Decision
├─ If total height < remaining height:
│  └─ Keep rows on current page (content fits)
└─ Else:
   ├─ Expand affected row range:
   │  └─ Call ExpandRowRangeForOtherRowspans()
   │     └─ Find any rowspan cells ABOVE that span INTO the range
   │        └─ If found, expand range upward to include them
   │           (These must move too - can't be split)
   └─ Move entire expanded row group to new page
      ├─ Create new page if needed
      ├─ Move all rows in group
      └─ Immediately start laying out on new page

Step 6: Update Layout State
├─ Adjust current position
├─ Update row/cell references
└─ Log action (verbose mode)
```

### Example Walkthrough

**Scenario:** Cell A (rowspan=2) has 80pt content, available space is 50pt

```
Before Layout:
Page 1 - Available Height: 50pt

┌─────┬─────┐
│  A  │  B  │  <- Row 0
│  ?  │  ?  │
│     ├─────┤
│?????│  C  │  <- Row 1
│     │  ?  │
└─────┴─────┘

Step 1 & 2: Cell A requires 80pt, overflow=auto (default)
Step 3: Affected rows: [0, 1] (rowspan=2)
Step 4: Total height = Row0_height + Row1_height + spacing
       (Need minimum 80pt for Cell A content)
Step 5: 80pt > 50pt available → MOVE TO NEW PAGE

After Layout:
Page 1: [Empty or partial content before table]

Page 2 - New Page Created:
┌─────┬─────┐
│  A  │  B  │  <- Row 0 (moved)
│  80 │content│
│     ├─────┤
│     │  C  │  <- Row 1 (moved with A)
│ pt  │content│
└─────┴─────┘
```

### Cascade Effect Example

**Scenario:** Three cells, one with rowspan overflows

```
Original Layout (all on page):
┌─────┬─────┬─────┐
│  A  │  B  │  C  │  <- Row 0: Space available = 60pt
│(2x1)│     │     │  <- Cell A content = 70pt
│     ├─────┼─────┤     Cell B & C = 40pt each
│     │  D  │  E  │  <- Row 1: Space available = 100pt
└─────┴─────┴─────┘

Decision Process:
- Cell A overflows (70pt > 60pt available in Row 0)
- Cell A has rowspan=2 → affects rows [0, 1]
- Cascade: Must move rows 0 AND 1 together
- Result: Although rows 0 & 1 have total 160pt space,
          they move to new page to stay with Cell A

After Move to New Page:
Page 1: [Previous content]

Page 2:
┌─────┬─────┬─────┐
│  A  │  B  │  C  │  <- All three rows stay together
│(2x1)│     │     │  <- Maintains table visual integrity
│     ├─────┼─────┤
│     │  D  │  E  │
└─────┴─────┴─────┘
```

This cascade effect is CRUCIAL because:
1. **Visual Alignment:** Rowspan cells span multiple rows; splitting rows breaks alignment
2. **Logical Integrity:** A table split across pages shouldn't have a rowspan cell only partially shown
3. **User Expectation:** Tables at page breaks are expected to move together

---

## Appendix B: Implementation Checklist

### Code Changes Required

**File: `Scryber.Components/Components/TableCell.cs`**
- [ ] Add `CellRowSpan` property
- [ ] Add attribute metadata (`[PDFAttribute]`)
- [ ] Add designer metadata (`[PDFDesignable]`)
- [ ] Add JS convertor metadata

**File: `Scryber.Styles/StyleKeys.cs`**
- [ ] Add `TableCellRowSpanKey` constant
- [ ] Add to appropriate style category

**File: `Scryber.Components/PDF/Layout/LayoutEngineTable.cs`**
- [ ] Add `RowspanCellMarker` inner class
- [ ] Add rowspan tracking dictionary to `LayoutEngineTable`
- [ ] Update `CellReference` class: add `_rowspan` field and property
- [ ] Update `CellReference.PopulateStyleValues()`: extract rowspan
- [ ] Update `BuildStyles()`: second pass for rowspan reservation
- [ ] Update `DoLayoutRowCells()`: check for occupied cells, skip them
- [ ] Add `IsPositionOccupiedByRowspan()` method
- [ ] Add `GetRowspanAffectedRange()` method
- [ ] Add `ExpandRowRangeForOtherRowspans()` method - **NEW: Handles upward cascade**
- [ ] Add `CalculateRowspanCellHeights()` method
- [ ] Add `HandleRowspanPageBreakWithOverflow()` method
- [ ] Add `GetCellRequiredHeight()` method
- [ ] Add `ClipCellContentIfNeeded()` method
- [ ] Add `ShouldMoveRowGroupToNewPage()` method
- [ ] Add `MoveRowGroupToNewPage()` method
- [ ] Add `GetRowspanCellsCrossingBreak()` method
- [ ] Update `DoLayoutComponent()`: call overflow detection

**File: `Scryber.UnitLayouts/TableRowspan_Tests.cs` (New)**
- [ ] Create test class with 25+ unit tests
- [ ] Organize into test categories (A-K)
- [ ] Include overflow scenarios

**Files: Documentation**
- [ ] Update `docs/reference/htmlattributes/attributes/attr_colspan_rowspan.md`
- [ ] Add rowspan examples
- [ ] Document overflow behavior
- [ ] Add troubleshooting section

---

### Related Issues
- Rowspan mentioned in TEST_COVERAGE_ANALYSIS.md as missing
- Documentation already exists for colspan attribute
- attr_colspan_rowspan.md references both features

### Related Files
- `Scryber.Components/PDF/Layout/LayoutEngineTable.cs` - Main implementation
- `Scryber.Components/Components/TableCell.cs` - Property definition
- `Scryber.Styles/StyleKeys.cs` - Style key constants
- `Scryber.UnitLayouts/Table_Tests.cs` - Existing table tests
- `docs/reference/htmlattributes/attributes/attr_colspan_rowspan.md` - Documentation

### External References
- [HTML colspan/rowspan spec](https://html.spec.whatwg.org/multipage/tables.html)
- [CSS Table Layout spec](https://www.w3.org/TR/CSS2/tables.html)
- [PDF 1.7 Table Support](https://www.adobe.io/content/dam/udp/assets/open/pdf/spec/PDF32000_2008.pdf)

---

## Conclusion

The rowspan implementation is well-scoped and builds directly on existing colspan infrastructure. With a phased approach and comprehensive testing, this feature can be delivered in 5-6 days with high quality and minimal risk to the existing codebase.

The proposed test strategy (20+ tests covering properties, layout, rendering, edge cases, and integration) provides confidence that the implementation meets requirements and handles complex scenarios gracefully.
