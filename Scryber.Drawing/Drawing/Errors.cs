using System;
namespace Scryber.Drawing
{
    internal static class Errors
    {
        static Errors()
        {
        }

        public static string CannotAccessStreamWithoutRead { get { return "Cannot access the stream until its initial position has been set. Please use the Read() method before tring to access the stream."; } }

        public static string CannotCastObjectToType { get { return "The object '{0}' could not be converted or cast to the required '{1}' type."; } }

        public static string CannotReadPastTheEOF { get { return "It is an error to attempt to read past the end of the stream. Use the bool Read() return value before accessing the stream values."; } }

        public static string ColorComponentMustBeBetweenZeroAndOne { get { return "The color component value for a pdf color (Red, Green, Blue, etc) must be a floating point number between 0 and 1. The value '{0}' is outside this range."; } }

        public static string ColorValueIsNotCurrentlySupported { get { return "The color space '{0}' is not supported. The PDFX graphics system currently only supports RGB and Gray color spaces, other values of the enumeration are there for future use."; } }

        public static string CouldNotBuildPathFromArc { get { return "The PDF Graphics engine could not build a valid set of bezier curves describing the required arc. See inner exception for more details."; } }

        public static string CouldNotBuildPathFromQuadratic { get { return "The PDF Graphics engine could not build a valid set of bezier curves describing the required quadratic curve. See inner exception for more details."; } }

        public static string CouldNotCastObjectToType { get { return "The object of type '{1}' could not be cast to a '{0}'"; } }

        public static string CouldNotFillText { get { return "Could not fill the graphics context with the specific text due to the error : {0}"; } }

        public static string CouldNotInitializeTheFonts { get { return "The initialization of the fonts failed. {0}"; } }

        public static string CouldNotLoadImagingFactory { get { return "The configured imaging factory type '{0}' could not be loaded."; } }

        public static string CouldNotLoadTheFontFile { get { return "The specified font file at path '{0}' could not be loaded. {1}"; } }

        public static string CouldNotLoadTheFontResource { get { return "The font resource with name '{0}' could not be loaded from the resource manager '{1}'"; } }

        public static string CouldNotLoadTheResourceManagerForBase { get { return "The resource manager could not be loaded for the name '{0}'. The name must be in the format 'basename [,assemblyname]' where assembly name is optional and if it is not provided the currently executing assembly will be used"; } }

        public static string CouldNotParseTheImageAtPath { get { return "The image parser could not parse the image at path '{0}'. {1}"; } }

        public static string CouldNotParseThePathExpectedInstruction { get { return "The path data '{0}' could not be parsed into a graphics path. Expected an instruction character at index '{1}', but found '{2}'."; } }

        public static string CouldNotParseThePathInstructionNotRecognised { get { return "The path data '{0}' could not be parsed into a graphics path. The instruction '{2}' at index {1} was not a known or supported instruction."; } }

        public static string CouldNotParseValue_3 { get { return "The value '{0}' could not be parsed into a {1} instance. A value in the format '{2}' was expected."; } }

        public static string FontDefinitionDoesNotHaveFile { get { return "There is no font file associated with the font definition '{0}'"; } }

        public static string FontMappingMustHaveFilePathOrResourceName { get { return "A configuration must have either a 'font-file' or 'rsrc-name' attribute declared so that the font definiton can be found. The font '{0}' has neither."; } }

        public static string FontNotFound { get { return "The font '{0}' could not be found. Please check the name and style."; } }

        public static string FontNotFoundOrEnableSystem { get { return "The font '{0}' could not be found. Please check the name and style. The use of system font files is currently blocked by the configuration settings."; } }

        public static string GraphicsOnlySupportsTopDownDrawing { get { return "The PDFGraphics class only supports the TopDown DrawingOrigin"; } }

        public static string NoLinesInParagraph { get { return "There are no lines in this paragraph"; } }

        public static string NoParagraphsInBlock { get { return "There are no paragraphs in this block"; } }

        public static string NullSystemFont { get { return "The system font, or font family cound not be found for the PDFFont '{0}'. Please check the name and any required font mappings in the config file."; } }

        public static string TextLayoutFailed { get { return "The layout of the text block could not be created. See inner exception for more details."; } }

        public static string UnbalancedTextStyle { get { return "The text style stack has become unbalanced and style end markers cannot be removed from an empty stack."; } }

        public static string UnknownPageUnits { get { return "The units '{0}' is not a known unit of measurement. Expected values are mm, inch and point pt."; } }

        public static string WriteToOnlySupportedForMemoryStreams { get { return "The WriteTo method is only supported on Memory Streams"; } }

    }
}
