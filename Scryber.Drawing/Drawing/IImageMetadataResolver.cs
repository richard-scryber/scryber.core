namespace Scryber.Drawing
{
    /// <summary>
    /// Implemented by whatever can resolve a loaded image's EXIF metadata by its source path -
    /// in practice, the Document (via its SharedResources). Exists so the meta() expression
    /// function (in Scryber.Expressive, which sits below Scryber.Components in the project
    /// dependency graph and can't reference Document/SharedResources directly) can still reach
    /// this capability: it's threaded through as a reserved variable value rather than a direct
    /// reference. See ImageMetaVars in Scryber.Generation for how it gets there.
    /// </summary>
    public interface IImageMetadataResolver
    {
        /// <summary>
        /// Resolves the EXIF metadata for the image at the given path (as it would be resolved
        /// for an &lt;img src="..."&gt; from the same context) - false if the image hasn't been
        /// loaded/registered, or has no EXIF data.
        /// </summary>
        bool TryGetImageMetadata(string path, out ImageEXIFMap metadata);
    }
}
