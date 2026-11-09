namespace Scryber.Components
{
    /// <summary>
    /// A synthetic Panel wrapper used by layout engines (currently LayoutEngineFlexBox, for
    /// row-direction flex items that aren't already a Panel - e.g. a bare text node sitting
    /// directly in a flex container) to give a non-container child a real block-layout box
    /// without it becoming a genuine, permanent level in the document's structural hierarchy.
    ///
    /// It gets both things a wrapper like this needs, for two independent reasons:
    /// - A real layout box: Panel already implements IPDFViewPortComponent, which
    ///   LayoutEngineBase's child-type dispatch checks before IInvisibleContainer, so
    ///   DoLayoutViewPortComponent runs as normal and IInvisibleContainer's own "don't create a
    ///   box, just flatten my children into the caller's context" layout behaviour never
    ///   triggers for this class.
    /// - Transparency to structural CSS selectors: Component.PopulateSiblingPosition (backing
    ///   :nth-child/:nth-of-type) and ComponentWrappingList&lt;T&gt;.BuildAllItems both check
    ///   IInvisibleContainer directly and recurse into its content instead of counting/collecting
    ///   the wrapper itself - independent of the layout dispatch above, so both apply here too.
    ///
    /// Wrapping a child still reparents it for real (Contents.Add sets child.Parent to this
    /// wrapper) - the transparency comes from downstream code recognising IInvisibleContainer,
    /// not from avoiding that reparent.
    /// </summary>
    public class InvisibleFlexContainer : Panel, IInvisibleContainer
    {
    }
}
