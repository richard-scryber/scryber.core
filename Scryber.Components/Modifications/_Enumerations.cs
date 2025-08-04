namespace Scryber.Modifications;

public enum FrameFileType
{
    DirectPDF,
    ReferencedTemplate,
    ContainedTemplate
}

public enum FrameFileStatus
{
    NotLoaded,
    Loading,
    Ready,
    Invalid
}