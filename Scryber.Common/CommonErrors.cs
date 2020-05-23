using System;
namespace Scryber
{
    internal static class CommonErrors
    {

        

        static CommonErrors()
        {
            
        }


        public static string CannotParseNullString { get { return "Cannot parse a nave value from a null or empty string."; } }

        public static string CannotParsePastTheEndOfTheString { get { return "Cannot parse a native value beyond the length of the provided string"; } }

        public static string CannotParseTheDocumentIDs { get { return "The document ID's could nopt be parsed. The expected format is either a single Guid, or a pair of Guid values separated by a comma."; } }

        public static string CannotRecordWebTraceWithoutWebContext { get { return "Trace log messages cannot be recorded to a web tracing log without a current HttpContext. Ensure that there is a current web request or remove the PDFWebTraceLog from the trace."; } }

        public static string CannotWriteToThisStream { get { return "The stream passed to the PDFWriter cannot be written to."; } }

        public static string CouldNotApplyStreamFilters { get { return "The specified stream filters could not be applied to the PDF object {0} : {1}"; } }

        public static string CouldNotCompressStreamFilter { get { return "The compression of the stream failed, check the inner exception for more details."; } }

        public static string CouldNotParseBooleanValue { get { return "The required boolean value could not be parsed from the string literal."; } }

        public static string DocumentIDCannotBeEmpty { get { return "One or more of the specified values for the document id were empty (or an empty Guid). The expected format is a pair of byte arrays (or Guids) "; } }

        public static string FontDefinitionDoesNotHaveFile { get { return "There is no font file associated with the font definition '{0}'"; } }

        public static string InvalidChararcterForStreamUseBytes { get { return "The use of high order characters in a stream is only supported with binary data writing. The character '{0}' could not be written."; } }

        public static string InvalidNullString { get { return "The string value was not recognised as the 'null' string so could not be parsed."; } }

        public static string InvalidObjectReferenceString { get { return "The string expression could not be converted to a valid object reference."; } }

        public static string InvalidPDFName { get { return "The PDF Name '{0}' contains invalid characters. Please use Alpha numeric characters for names."; } }

        public static string InvalidPDFStringEscapeSequence { get { return "The parsed string does not conform to the PDF standards"; } }

        public static string NoDataContextOnTheStack { get { return "There is no current data context to access"; } }

        public static string NoWebContextAvailableForRelativeUrl { get { return "The web application relative url could not be resolved as there is no current HttpContext"; } }

        public static string ParsedValueWasNotAnItegralNumber { get { return "Parsed value was not an itegral number."; } }

        public static string ParsedValueWasNotANumericValue { get { return "The parsed value in the string was not convertable to a numeric value."; } }

        public static string PDFNameDoesNotStartWithSlash { get { return "The PDF name does not start with the required '/' character."; } }

        public static string PDFStringDoesNotStartWithRequiredCharacter { get { return "The PDF String does not start with the required '(' character, nor is it a recognised hexadecimal sequence"; } }

        public static string PDFStringHasUnbalancedParenthese { get { return "The parsed string does not have balanced unescpaed paranthese."; } }

        public static string StreamDoesNotHaveFiltersDefined { get { return "The PDFStream does not have any filters defined. If is invalid to ensure that the filters have been applied"; } }

        public static string TypeStringOnlyNChars { get { return "The PDFType can only be initialized with a ASCII string {1} characters long. The string '{0}' is not the correct length."; } }

        public static string WriteToOnlySupportedForMemoryStreams { get { return "The PDFStream WriteTo method only supports MemoryStreams."; } }

        public static string XPathExpressionCouldNotBeEvaluated { get { return "The XPath expression '{0}' could not be evaluated and returned an error."; } }

        public static string AllDictionaryKeysMustBePDFNames { get { return "The PDFDictionary could not be parsed. All Keys within hte dictionary must be Names (/Name)."; } }

        public static string ArrayDoesNotEndWithRequiredChar { get { return "The PDFArray could not be parsed as it does not end with the required ']' character."; } }

        public static string ArrayDoesNotStartWithRequiredChar { get { return "The PDFArray could not be parsed as it does not start with the required '[' character."; } }

        public static string DictionaryDoesNotEndWithRequiredChar { get { return "The PDFDictionary could not be parsed as it does not end with the required '&gt;&gt;' marker."; } }

        public static string DictionaryDoesNotStartWithRequiredChar { get { return "The PDFDictionary could not be parsed as it does not start with the required '&lt;&lt;' marker."; } }

        public static string XRefTableDoesNotStartWithXRef { get { return "The parsed XRefTable does not start with the required 'xref' marker"; } }

        public static string XRefTableEntryMustBeInCorrectFormat { get { return "Each XRef table entry must consist of a 10 byte digit offset, a 5 byte generation number, and a single character state marker (f,n) with spaces in between and 2 whitespace characters at the end (to make 20 bytes)."; } }

        public static string XRefTableSectionMustBe2Integers { get { return "Each XRefTable section must be a single line of 2 integer values separated by a space. And always have more than 1 entry."; } }

        public static string CouldNotInitializeThePDFReader { get { return "The PDFReader could not initialize based on the referenced file or stream. Check the validity of the file."; } }

        public static string IndirectObjectCannotBeParsed { get { return "The Indirect object could not be parsed"; } }

        public static string AnIndirectObjectWithReferenceCouldNotBeFound { get { return "An indirect object with the referenced id of {0} could not be found in the original file."; } }

        public static string CannotOpenTheFileAtThePath { get { return "The specified file could not be opened or does not exist at the specified path. See the inner exception for more details."; } }

        public static string MarkerNotFoundByReader { get { return "The PDF file marker '{0}' could not be found in the data file by the PDFReader. This marker is required in a PDFFile."; } }

        public static string DatabindingPropertyNotFound { get { return "The property '{0}' could not be found or accessed for binding. Please check the spelling."; } }

        public static string DatabindingSourceNotXPath { get { return "The current databinding source is not an IXpathNavigable object."; } }

        public static string InvalidIndexerExpression { get { return "The binding indexer expression '{0}' could not be evaluated."; } }

    }
}
