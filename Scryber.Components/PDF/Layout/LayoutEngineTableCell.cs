using Scryber.Components;

namespace Scryber.PDF.Layout;

/// <summary>
/// Engine for laying out a Table Cell. Defaults to main LayoutEnginePanel behaviour, 
/// </summary>
public class LayoutEngineTableCell : LayoutEnginePanel
{

    public LayoutEngineTableCell(ContainerComponent container, IPDFLayoutEngine parent) : base(container, parent)
    {
        
    }
    
}