namespace Scryber.Styles.Selectors
{
    /// <summary>
    /// Implemented by components that carry a layout page index, allowing PageMatcher
    /// to evaluate @page :first / :left / :right pseudo-classes without depending on
    /// the Scryber.Components assembly.
    /// </summary>
    public interface IPageIndexProvider
    {
        int LayoutPageIndex { get; }
    }
}
