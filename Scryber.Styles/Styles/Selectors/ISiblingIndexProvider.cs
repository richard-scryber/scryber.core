namespace Scryber.Styles.Selectors
{
    /// <summary>
    /// Implemented by components that can report their position among their element siblings,
    /// allowing StyleSelector to evaluate structural pseudo-classes (:nth-child, :first-child,
    /// :nth-of-type, etc.) without Scryber.Styles depending on the Scryber.Components assembly.
    /// Follows the same bridging pattern as IPageIndexProvider.
    /// </summary>
    public interface ISiblingIndexProvider
    {
        /// <summary>
        /// The 1-based position of this component among its parent's element children
        /// (text/whitespace nodes are not counted).
        /// </summary>
        int SiblingIndex { get; }

        /// <summary>
        /// The total number of element children of this component's parent.
        /// </summary>
        int SiblingCount { get; }

        /// <summary>
        /// The 1-based position of this component among its parent's element children that
        /// share the same element name as this component.
        /// </summary>
        int SiblingOfTypeIndex { get; }

        /// <summary>
        /// The total number of this component's parent's element children that share the
        /// same element name as this component.
        /// </summary>
        int SiblingOfTypeCount { get; }
    }
}
