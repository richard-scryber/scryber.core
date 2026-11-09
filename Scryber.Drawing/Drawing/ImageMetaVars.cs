namespace Scryber.Drawing
{
    /// <summary>
    /// Reserved variable name used to thread an IImageMetadataResolver through to the meta()
    /// expression function, following the exact same pattern UnitRelativeVars.RelativeCallbackVar
    /// already uses to carry a non-data capability (a delegate, there) through IVariableProvider -
    /// see ItemVariableProvider in Scryber.Generation for where this gets set and read.
    /// </summary>
    public static class ImageMetaVars
    {
        public const string ResolverVar = UnitRelativeVars.RelativeVarPrefix + "ImageMetadataResolver";
    }
}
