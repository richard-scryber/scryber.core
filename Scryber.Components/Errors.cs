using System;
namespace Scryber
{
    internal static class Errors
    {
        static Errors()
        {
        }



        public static string AssignedDataSourceIsNotIPDFDataSource { get { return "The Component with the id '{0}' does not implement the IPDFDataSource interface. All Components that are to be used as datasources must implement this interface."; } }

        public static string CannotAddCellToIndexAlreadyFull { get { return "Cannot add the cell reference to the column at row {0} because it is already full. Ensure that the cells are added in order to a column."; } }

        public static string CannotCastToStyleItem { get { return "The created style item returned from the Document could not be cast to a {0} item."; } }

        public static string CannotChangeValueWhenLocked { get { return "The value of property '{0}' cannot be changed because the item '{1}' is locked for rendering"; } }

        public static string CannotSetDataSourceAndDataSourceID { get { return "Cannot set both the DataSource value and the DataSourceID value of a binding control. Ensure that the unused property is clear before setting the required property."; } }

        public static string CannotUseFindForComponentNotInDocumentHeirarchy { get { return "Cannot use the FindComponent method when an Component is not contained within the document heirarchy."; } }

        public static string CanOnlyReferenceExternalStyleDefinitionsOfType { get { return "All external defined files must have a base type of '{0}'."; } }

        public static string CouldNotFindControlWithID { get { return "The Component with the id '{0}' could not be found. The search is case sensitive."; } }

        public static string CouldNotParseValue_2 { get { return "The value '{0}' could not be parsed into a {1} instance."; } }

        public static string CouldNotSelectData { get { return "An error occurred whilst trying to extract the data."; } }

        public static string FontNotRegistered { get { return "The font '{0} {1}' has not been registered with the current resource section. Fonts must be registered with the current page or IResourceContainer before use. This is normally done with the PDFGraphics DrawString and DrawTextBlock methods."; } }

        public static string GridIsNullNotMeasured { get { return "The table has not been measured or the referenced grid has been lost."; } }

        public static string InvalidCallToGetGraphicsForStructure { get { return "The hierarchy is not complete. No parent exists that can create a graphics context for the requesting Component."; } }

        public static string InvalidEmptyUnits { get { return "A unit cannot be instantiated with 'Empty', please specify the actual unit of measurement"; } }

        public static string LoadingOfExternalFilesNotSupported { get { return "The loading of an external file '{0}' is not supported in the base document class."; } }

        public static string MeasureNotCalled { get { return "The Measure and Arrange methods must be called on an object before it can be rendered."; } }

        public static string NoOpCountOutOfRange { get { return "The NoOp count on the PDFComponentList has become unbalanced. Please ensure that the count is only incrementd or decrementd evenly."; } }

        public static string NullArgument { get { return "The parameter '{0}' cannot be null."; } }

        public static string PDFTypeStringOnlyNChars { get { return "The PDFType can only be initialized with a ASCII string {1} characters long. The string '{0}' is not the correct length."; } }

        public static string TemplateComponentCannotBeRoot { get { return "A template Component cannot be the root Component. It must be contained within another container."; } }

        public static string UnbalancedDataStack { get { return "The data stack has become unbalanced. Entries have been added or removed out of sequence."; } }

        public static string UnbalancedStyleStack { get { return "The style stack has become unbalanced. Entries have been added or removed out of sequence."; } }

        public static string AssignedDataSourceIsNotNavigable { get { return "The assigned datasource for the binding component does not implement the IXPathNavigable interface. All datasources that are to be used must implement this interface."; } }

        public static string CannotUseBaseTemplateInChoose { get { return "Setting of the 'Template' property is not supported in the PDFChoose Component. Use the Where and Otherwise properties instead."; } }

        public static string CannotAccessStreamWithoutRead { get { return "Cannot access the stream until its initial position has been set. Please use the Read() method before tring to access the stream."; } }

        public static string CannotReadPastTheEOF { get { return "It is an error to attempt to read past the end of the stream. Use the bool Read() return value before accessing the stream values."; } }

        public static string CouldNotFillText { get { return "Could not fill the graphics context with the specific text due to the error : {0}"; } }

        public static string CouldNotParseValue_21 { get { return "The value '{0}' could not be parsed into a {1} instance."; } }

        public static string CouldNotParseValue_3 { get { return "The value '{0}' could not be parsed into a {1} instance. A value in the format '{2}' was expected."; } }

        public static string GraphicsOnlySupportsTopDownDrawing { get { return "The PDFGraphics class only supports the TopDown DrawingOrigin"; } }

        public static string MeasureNotCalled1 { get { return "The Measure and Arrange methods must be called on an object before it can be rendered."; } }

        public static string NullArgument1 { get { return "The parameter '{0}' cannot be null."; } }

        public static string ComponentIsNotKnownToLayoutEngine { get { return "The Component with type '{0}' is not a known Component for the layout engine. Either change its type, make the type implement the IPDFComponentViewPort interface, or sets its type to NoOp."; } }

        public static string LayoutEngineHasNotSetArrangement { get { return "The PDFComponentArangement has not been set on this Component before attempting to render. All Components must have their arrangement set before rendering."; } }

        public static string CouldNotBuildTheTableGrid { get { return "The TableGrid could not be built for the table '{0}'. See the inner exception for more details"; } }

        public static string CouldNotLayoutGridCells { get { return "Could not layout the cells of the table grid with id '{0}'. {1}"; } }

        public static string CannotAccessGridOffsetInTable { get { return "Cannot access the offset {0} in the table with a {1} count of {2}"; } }

        public static string CannotModifyTableGridSizes { get { return "This group of table grid sizes has been locked and cannot be modified, only read."; } }

        public static string GridNotClosed { get { return "The CloseGrid method has not been called after adding cells to the table grid. "; } }

        public static string CannotLoadFileWithRelativePath { get { return "The method must be called with an absolute path (not relative). Use the MapPath method to attempt conversion from a relative to an absolute path."; } }

        public static string FileNotFound { get { return "The requested file could not be found at the specified path. Or the file is not a valid XML document."; } }

        public static string CannotConvertObjectToType { get { return "An object of type '{0}' cannot be cast to the required '{1}' type."; } }

        public static string CircularReferenceToPath { get { return "The parsed documents have a circular dependancy reference to the path '{0}'. Please check the references."; } }

        public static string NoTypeFoundWithPDFComponentNameInNamespace { get { return "No Type could be found in the namespace '{1}' that declares a component name '{0}'. Please check the file and required type."; } }

        public static string PathStackIsUnbalanced { get { return "The path resolution stack has become unbalanced and parsing cannot continue."; } }

        public static string CannotCreateInstanceOfType { get { return "An instance of type '{0}' could not be created. {1}"; } }

        public static string LayoutEngineIsAlreadyWorking { get { return "The layout engine is already positioning a component. You cannot use the same engine to run more than one component at a time"; } }

        public static string LayoutFailedForComponent { get { return "The layout of component '{0}' failed. {1}"; } }

        public static string PageBreaksAreOnlySupportedAtTheRootSection { get { return "Page break components are only supported at the root content level of a section."; } }

        public static string ParentDocumentCannotBeNull { get { return "The parent document cannot be null."; } }

        public static string RequiredChildElementNotFountOnElement { get { return "The required child element '{0}' was not found on the component definition '{1}'"; } }

        public static string ReturnTypeOfXPathExpressionCannotBeNavigatorForPropertyEvaluation { get { return "The return type of expression '{0}' is a new XPath navigator. This cannot be assigned to a simple property."; } }

        public static string UnbalancedOutlineStack { get { return "The outline stack has become unbalanced. Entries have been added or removed out of sequence."; } }

        public static string NoValidLicenceFoundForOperation { get { return "A valid, sourced licence was not found that supports the restricted operation '{0}'. Please check your license agreement to ensure you are up to date."; } }

        public static string ResourceContainerOfComponnetNotFound { get { return "The resource container for the {0} component '{1}' was not found. This component can only be rendered as child of a resource container such as a Page or Section."; } }

        public static string AFontWithNameMustBeDeclaredInConfig { get { return "The font with name '{0}' is not a standard font, This font must be referenced in the configuration file or have the file path specified. "; } }

        public static string ColorValueIsNotCurrentlySupported { get { return "The color space '{0}' is not supported. The PDFX graphics system currently only supports RGB and Gray color spaces, other values of the enumeration are there for future use."; } }

        public static string ParentDocumentMustBeTemplateParser { get { return "The parent document of a template must implment the IPDFTemplateParser interface."; } }

        public static string InvalidXPathExpression { get { return "The XPath expression '{0}' could not be evaluated. Please check the statement."; } }

        public static string TemplateComponentParentMustBeContainer { get { return "The parent Component of a template Component must be an instance of an IPDFContainerComponent"; } }

        public static string TemplateHasNotBeenInitialised { get { return "The template generator has not been initialised. The InitTemplate method must be called before any instaniation."; } }

        public static string CannotSetBaseTextOfPageNumber { get { return "The base text value of the PageNumberLabel cannot be set explicity. This is derived at render time from the format value and current page numbers"; } }

        public static string ComponentNamerAlreadyRegistered { get { return "A component with the name '{0}' has already been registered in this document. It is an error to have 2 components in the same name in the same document."; } }

        public static string LinkToDestinationCouldNotBeMade { get { return "No matching destination to '{0}' could be found. The link cannot be assigned."; } }

        public static string NoCurrentComponentInLayoutEngine { get { return "There is no current component in this Layout Engine. Ensure the engine is in the process of laying out a container before calling this method."; } }

        public static string CannotEndAComponentLayoutIfNotCurrent { get { return "It is an error to attempt to end the layout of a component that is not the current component."; } }

        public static string CannotSetViewPortAsInline { get { return "A viewport component cannot be set with a layout mode of inline."; } }

        public static string NoTextBlockAsCurrentComponentIsNotTextComponent { get { return "The current text block cannot be retrieved as the current component does not implement the IPDFTextComponent interface."; } }

        public static string NoWebContextAvailable { get { return "There is no current web context available. This provider requires a current web context inorder to function."; } }

        public static string CannotLayoutABlockThatHasBeenClosed { get { return "Cannot alter the layout of a block that has been closed. Check the IsClosed property before trying to append"; } }

        public static string CannotLayoutAPageThatHasBeenClosed { get { return "Cannot alter the layout of a page that has been closed."; } }

        public static string NoOpenBlocksToAppendTo { get { return "The layout does not have any open blocks on the page."; } }

        public static string LayoutContainerHasExistingOpenItem { get { return "Cannot perform this action, as the current item in this container has not been closed. Close the current item first, then perform the action."; } }

        public static string LayoutItemCouldNotBeClosed { get { return "The layout item could not be closed. "; } }

        public static string LayoutItemIsClosed { get { return "This layout item is closed and cannot be appended to"; } }

        public static string LayoutNoCurrentLineToClose { get { return "The layout item does not have a currently open line available to close. Check the HasOpenLine property first."; } }

        public static string AlreadyAHeaderDefinedOnPage { get { return "There is already a header layout defined on this layout page"; } }

        public static string NoFooterBlockToClose { get { return "There is no current footer block on this layout page to close. "; } }

        public static string NoHeaderBlockToClose { get { return "There is no current header block on this layout page to close"; } }

        public static string TextOptionTypeIsNotSupported { get { return "The text operation flag '{0}' is not supported. Use a full parse to generate spans, divs, and text formats."; } }

        public static string AbsolutePositioningBlocksRegisterWithTheLayoutPage { get { return "Blocks that are absolute positioned must be registered with the page rather than on an inner block."; } }

        public static string CannotBeginABlockThatIsInline { get { return "Inline components do not have surrounding blocks."; } }

        public static string TextBlockIsNotSetOnComponent { get { return "The TextBlock has not been assigned to the current text component."; } }

        public static string ThisBlockDoesNotHaveAnyPostionOptions { get { return "The layout block to be rendered does not have any postion infromation associated with it. The position must be set on the block before it can be rendered."; } }

        public static string ThisBlockDoesNotHaveAnyStyle { get { return "The layout block to be rendered does not have any style information associated with it. The FulStyle must be set before it can be rendered."; } }

        public static string NoTableRowToAddCellTo { get { return "The current table grid layout does not have any open rows to add layout cells to."; } }

        public static string NoCurrentOpenTableCell { get { return "There is no currently open table layout cell on the row. "; } }

        public static string DocumentCannotBeBoundAtThisStage { get { return "The document component cannot be bound at this stage. It has already been laid out."; } }

        public static string DocumentHasAlreadyBeenInitialized { get { return "This document has already been initialized. It cannot be initialized more than once."; } }

        public static string DocumentHasAlreadyBeenLoaded { get { return "This document has already been loaded. It cannot be loaded more than once."; } }

        public static string DocumentHasBeenDisposed { get { return "This document has already been disposed. It cannot take part in any activities once it has been disposed."; } }

        public static string DocumentHasNotBeenInitialized { get { return "This document has not been initialised. It cannot perform further actions until it has been initiaized."; } }

        public static string DocumentHasNotBeenLaidout { get { return "This PDFDocument has not been laidout, and cannot perfrom further generation stages out of sequence."; } }

        public static string DocumentHasNotBeenLoaded { get { return "This PDFDocument has not been loaded, and cannot perfrom further generation stages out of sequence."; } }

        public static string TryingToOutputADifferentDocumentLayout { get { return "The document layout does not correspond to this PDFDocument and you cannot output a layout from a different document component."; } }

        public static string CouldNotFindControlWithName { get { return "A component with name '{0}' could not be found. The search is case sensitive. To look up an ID, prefix with #"; } }

        public static string CouldNotLoadTheDataFromTheSourcePath { get { return "The {0} data source could not be retrieved from the requested path. See the inner exception for more details."; } }

        public static string NoSourcePathDefinedOnXmlDataSource { get { return "The XmlDatasource {0} does not has a source file set to load data from and the value of the XmlDocument has not been set."; } }

        public static string AssembyTokensDoNotMatchOnLicense { get { return "The strong named assembly token does not match the licensed component"; } }

        public static string CannotDeserializePDFLicenceFromData { get { return "Could not deserialize the license data for component {0}. See the inner exception for more details."; } }

        public static string CannotSerializePDFLicenceToStream { get { return "The license could not be serialized to the provided stream. See te inner exception for more details."; } }

        public static string CouldNotDeserializeTheLicenseTerms { get { return "Could not deserialize the licence terms. An error occurred whilst reading the fields. See the inner exception for more details."; } }

        public static string CouldNotLoadLicenceKey { get { return "An error occurred while trying to load the license key for component {0}. See the inner exception for more details."; } }

        public static string LicenseSignatureCouldNotBeValidated { get { return "The license signature is invalid for the specified terms."; } }

        public static string TypeDoesNotHavePublicKeyForLicence { get { return "The component {0} is registered as a licensed component, but does not have a public key assigned on it's attribute"; } }

        public static string TypeIsNotRegisteredWithLicensedComponentAttribute { get { return "The component {0} is not registered with a license key attribute."; } }

        public static string CouldNotTransformInputData { get { return "The transformation of the data failed. See the inner exception for more details."; } }

        public static string XSLTCouldNotBeLoadedFromPath { get { return "The XSLT transformation file could not be loaded from the mapped path '{0}'"; } }

        public static string XSLTPathOrTransformerNotSetOnInstance { get { return "The path to an XSLT file or an XSLCompiled transform has not been set on the Transform extension for the datasource {0}."; } }

        public static string UnknownDocumentFormat { get { return "The document format '{0}' is not a known or supported format. Rendering cannot contine."; } }

        public static string NoFileLinkSpecifiedOnUriAction { get { return "A link of type Uri was specified, but no value was specified for the file to navigate to. Please specify a File value."; } }

        public static string LayoutManagerAlreadyHasAnOpenItem { get { return "The layout manager already has an open {0}. Please close the current {0} before starting a new one."; } }

        public static string DataSchemaExtractionIsNotSupported { get { return "This data source does not support extraction of the data schema. Always ceck the Suports property before calling this method."; } }

        public static string FontDefinitionDoesNotHaveFile { get { return "There is no font file associated with the font definition '{0}'"; } }

        public static string CouldNotDownloadBarcodeData { get { return "The barcode image could not be downloaded from the specified url: {0}"; } }

        public static string CouldNotInitializeTheImageForComponent { get { return "The initialization and image load for component {0} failed. See inner exception for more details"; } }

        public static string CannotDeclareNamespaceWithoutPrefix { get { return "All namespaces must have a prefix explictly set for all elements that are in a namespace. The namespace '{0}' does not have a prefix assigned."; } }

        public static string NoCurrentNumberGroup { get { return "There is no current number group in the list numbering to action. Groups must be opended before they can be used/"; } }

        public static string IndirectObjectHasAlreadyBeenWritten { get { return "This indirect object has already been written to the underlying stream. Cannot write the same object twice"; } }

        public static string CannotCreatePolygonWithLessThan3Sides { get { return "Cannot create a regular ploygon shape with less than 3 sides (it's not a shape)."; } }

        public static string CannotCreatePolygramWithLessThan5Sides { get { return "Cannot create a regular ploygram shape with less than 5 sides (it's not a shape)."; } }

        public static string CouldNotParseTheValue_3 { get { return "The value '{0}' could not be parsed into a {1} instance. A value in the format '{2}'"; } }

        public static string StepCountCannotBeGreaterThanVertexCount { get { return "The step count cannot be greater than the vertex count."; } }

        public static string PathMustHave3PointsForTriangle { get { return "The path data for a triangle must have 3 and only 3 points."; } }

        public static string OrigSourceIsRequiredForUpdateDocument { get { return "When using the UpdateDocument either the BaseFile must be set, or the orig source path to a valid PDF file must be set."; } }

        public static string PageContentsCanOnlyByReferencesOrArraysOfReferences { get { return "The parsed page content streams can only be object references or arrays of bject references"; } }

        public static string PageMediaboxMustBeAnArray { get { return "The MediaBox value for a page must be an array of numbers (or an indirect reference to an array of numbers."; } }

        public static string PageToUpdateWasNotFound { get { return "The page at (zero based) index {0} was not found in the file '{1}'."; } }

        public static string ModificationStartIndexIsLongerThanDocument { get { return "The specified start index for a page modification is longer than the current base file."; } }

        public static string ModificationStartIndexPlusCountIsLongerThanDocument { get { return "The specified count for page modifications goes beyond the current base file page count, starting at the specified index."; } }

        public static string PageOwnerOfAModifyLayoutMustBeAModifyPage { get { return "The Page owner of a PDFModifyLayoutPage must be a PDFModifyPageBase"; } }

        public static string CouldNotLoadImageFromPath { get { return "The image path could not be resolved, or the image could not be loaded from path '{0}'"; } }

        public static string CouldNotLoadTheMissingImage { get { return "The fall back missing image could not be found, so cannot replace the not found image."; } }

        public static string NoRemoteDestinationSpecifiedOnLink { get { return "The link to a remote destination could not be created as the path to the file has not been set (or is empty)"; } }

        public static string CouldNotLoadTheDataFromProvider { get { return "The source data could not be loaded from the data provider with id '{0}'. See the inner exception for more details."; } }

        public static string CannotChangeTheContentsOfAPlaceHolderOnceParsed { get { return "This placeholder has already parsed it's contents, and can no longer be modifed."; } }

        public static string CannotGetSchemaForTransformedSource { get { return "A transformed SQL data source no longer support schema extraction."; } }

        public static string CouldNotBindDataComponent { get { return "Building and binding the required components in the data list to the current data source failed. See the inner exception for more details."; } }

        public static string CouldNotConvertObjectToXml { get { return "The object could not be converted to Xml. Please see the inner exception for more details."; } }

        public static string CouldNotLoadAssembly { get { return "The requested assembly '{0}' could not be found."; } }

        public static string CouldNotLoadType { get { return "The requested type '{0}' could not be loaded from the requested assembly."; } }

        public static string CouldNotTransformResult { get { return "The loaded data could not be transformed by the specified file. See the inner exception for more details."; } }

        public static string CountNoConvertBinaryDataToBitmap { get { return "The provided binary data could not be converted to a required Bitmap instance. Ensure the data is a bitmap."; } }

        public static string DataLoadFailedForCommand { get { return "The command '{0}' on data source '{1}' failed to load the data. See the inner exception for more details."; } }

        public static string DataSchemaItemNotFoundAtPath { get { return "No Data schema item was found at the path '{0}'."; } }

        public static string FailedToLoadDataForObjectSource { get { return "The object datasource '{0}' could not load the data because : {1}"; } }

        public static string MethodParameterNotDefinedWithName { get { return "The registered method has parameter '{0}' which has not been specified. All parameters must be declared on the command."; } }

        public static string NoConnectionDefined { get { return "There is no connection defined on this command '{0}' in datasource '{1}'."; } }

        public static string NoConnectionSettingFor { get { return "The configuration file does not contain a connection setting with the name '{0}'."; } }

        public static string NoMethodFoundOnTheTypeWithName { get { return "No required method could be found on the type {1} with the name '{0}' or the specified parameters."; } }

        public static string NoPredefinedCommandWithKey { get { return "The data source '{1}' does not contain a command with the specified key '{0}'."; } }

        public static string NoProviderFactoryWithName { get { return "There is no configured .net data provider with the name '{0}'."; } }

        public static string NullChildColumnForRelationToTable { get { return "The relation could not be created in command {1} as the child column name has not been set for table {0}"; } }

        public static string NullParentColumnForRelationToTable { get { return "The relation could not be created in command {1} as the parent column name has not been set for table {0}"; } }

        public static string ObjectDataSourceCanOnlyUseObjectCommands { get { return "The object datasource {1} can only use and execute PDFObjectCommands to load the data. The command '{0}' is not an object command."; } }

        public static string ParentDocumentCannotBeNull1 { get { return "The component must be within the document hierarchy to perform this action. Currently the parent document is null."; } }

        public static string ResourceContainerOfComponentNotFound { get { return "The required resource container for the {0} component with ID '{1}' cound not be found."; } }

        public static string SqlDataSourceCanOnlyUseSqlCommands { get { return "The SqlDataSource '{1}' can only use SqlCommands to load data. The command '{0}' is not an SqlCommand."; } }

        public static string TableDoesNotContainColumn { get { return "The  relation could not be created in command {1} as the table {0} does not contain the column {2}."; } }

        public static string CannotCreateInstanceOfTypeOnCommand { get { return "An instance of the type '{0}' required by the object command '{1}' could not be created. A public parameterless constructor is needed on the declaring type for instance methods to be used."; } }

        public static string CommandForRelatedDataMustMatchType { get { return "The releated data command for type {0} must be of type {1}"; } }

        public static string CommandWithNameCannotBeFound { get { return "A Provider command with the name '{0}' could not be found in the source '{1}'. Names are case-sensitive."; } }

        public static string MatchingMethodSignatureCouldNotBeFoundForObjectData { get { return "The method '{0}' found on type {1}does not match the signature declared by the object command '{2}' in the data source {3}. Ensure all required methods are publicly accessible."; } }

        public static string MatchingMethodSignatureDoesNotHaveAReturnTypeObjectData { get { return "The method '{0}' found on type {1}does not have a return type, so cannot be used as declared by the object command '{2}' in the data source {3}. Ensure all required methods are publicly accessible."; } }

        public static string MethodCouldNotBeFoundForObjectData { get { return "The method '{0}' could not be found on type {1} required by object command '{2}' in the data source {3}. Ensure all required methods are publicly accessible."; } }

        public static string MethodNameNotSetOnObjectDataSource { get { return "The Method name to call for the Object Command '{0}' has not been set in the data source '{1}'"; } }

        public static string TypeCouldNotBeFoundForObjectData { get { return "The type '{0}' could not be found or loaded on object command '{1}' in the data source {2}. Are you missing an assembly reference."; } }

        public static string TypeNameNotSetOnObjectDataSource { get { return "The Type name of the class containing the method to call for the Object Command '{0}' has not been set in the data source '{1}'"; } }

        public static string NoDefaultValueSetOnEmptyValueTypeParameter { get { return "No default value has been set on the parameter '{0}' which has a value type '{1}'. Cannot pass null to this parameter, so a default value is required."; } }

        public static string CannotReferenceExternalFilesFromSourcesWithoutARoot { get { return "The original parsed source does not have a root location. Cannot parse referenced files from an un-rooted source unless they are full / absolute paths."; } }

        public static string UnknownArtefactForNamesDictionary { get { return "Unknown artefact for the names dictionary. All Artefacts must implement the ICategorisedArtefactNamesEntry interface if they are to be stored in the Names dictionary of the documents catalog."; } }

        public static string CanOnlyTransformBlockComponents { get { return "The contents of a positioned region we expected to be a single layout block. Transformations are only supported on the standard block elements such as panels and divs."; } }

    }
}
