using System;
namespace Scryber.Generation
{
    public static class Errors
    {
        static Errors()
        {
            //TODO: Load localized resources
        }


        public static string AttributeDefinitionInElementsCollection { get { return "An attribute definition was in the elements collection."; } }

        public static string CannotConvertObjectToType { get { return "An object of type '{0}' cannot be cast to the required '{1}' type."; } }

        public static string CannotCreateInstanceOfType { get { return "An instance of type '{0}' could not be created. {1}"; } }

        public static string CannotSpecifyBindingExpressionsOnEvents { get { return "The value '{0}' is invalid for an event binding on attribute '{1}'. You cannot specify data binding expressions on event attributes."; } }

        public static string CannotUseRemoteTypeReferencesInATypeAttribute { get { return "The type reference '{0}' cannot be used as it resolves to a remote type rather than an actual type. Use the actual type name."; } }

        public static string CanOnlyParseComponentAsElement { get { return "Can only parse a component as an element"; } }

        public static string CouldNotDeterminePropertyType { get { return "Could not determine the type of the reflected property"; } }

        public static string DatabindingIsNotSupportedOnType { get { return "The type '{0}' does not support databinding. To support databinding a type must implement the IPDFBindableComponent interface."; } }

        public static string DuplicateDefaultElementOnClass { get { return "The type '{1}' has multiple default elements defined. The property '{0}' cannot have a PDFElement attribute with a null or empty name because another property has already declared one. Either specify a name, or user the PDFIgnore attribute."; } }

        public static string InvalidXPathExpression { get { return "The XPath expression '{0}' could not be evaluated. Please check the statement."; } }

        public static string NoAddMethodFoundOnCollection { get { return "No 'Add' method was found on the collection type '{0}' accepting a single parameter of type '{1}'. This method is required for a parsed collection, unless the collection implements the IList interface."; } }

        public static string NoContentPropertyDefined { get { return "Could not get the Content property definition from the template type"; } }

        public static string NoPDFComponentDeclaredWithNameInNamespace { get { return "No PDFComponent was found with the declared name of '{0}' in the namespace '{1}'"; } }

        public static string ParsableValueMustHaveParseMethod { get { return "The type '{0}' is declared as parsable with the PDFParsableValueAttribute, but no static method can be found that matched the signature  'Parse(string):T'"; } }

        public static string ParsedTypeDoesNotContainDefinitionFor { get { return "The parsed type '{0}' does not contain a definiton for the {1} with name '{2}', or the {1} could not be assigned."; } }

        public static string ParserAttributeMustBeSimpleOrCustomParsableType { get { return "The type of property '{0}' in class '{1}' must either be of a known simple type  or be declared with the PDFParsableValueAttribute. Attributes cannot contain complex defintion."; } }

        public static string ParserAttributeNameCannotBeEmpty { get { return "The attribute name of a PDFAttribute cannot be null or empty. Please specify a name on property '{0}' of type '{1}' "; } }

        public static string ParserCannotFindAssemblyWithName { get { return "The file parser could not find an assembly with the name '{0}'"; } }

        public static string RequiredAttributeNoFoundOnElement { get { return "The required attribute '{0}' was not found on the component definition '{1}'"; } }

        public static string ReturnTypeOfXPathExpressionCouldNotBeDetermined { get { return "The return type of XPath expression '{0}' could not be determined. Expressions for simple properties must return a simple node set, boolean, integer or string values."; } }

        public static string SourcePathOrTypeMustBeSet { get { return "Neither the source path or source type were set on the remote reference. One of these attributes is required"; } }

        public static string CouldNotSetTextPropertyValue { get { return "The text property '{0}' could not be set on type '{1}'. Check the permissions and scope."; } }

        public static string NoTypeFoundWithPDFComponentNameInNamespace { get { return "No Type could be found in the namespace '{1}' that declares a component name '{0}'. Please check the file and required type."; } }

        public static string TemplateComponentParentMustBeContainer { get { return "The parent Component of a template Component must be an instance of an IPDFComponent"; } }

        public static string TextLiteralTextPropertyNotFound { get { return "The expected {0} property '{1}' was not found on the text literal type '{2}'. Change the generator settings, or define the property."; } }

        public static string TemplateHasNotBeenInitialised { get { return "The template generator has not been initialised. The InitTemplate method must be called before any instaniation."; } }

        public static string ParserDoesNotHaveAssemblyRegisteredForNamespace { get { return "The parser could not find a declared assembly or namespace for the prefix '{0}' used with element '{1}. Ensure the assembly is declared at the top of the file with a unique namespace prefix. e.g. xmlns:my=\"MyAssembly, MyNamespace\""; } }

        public static string RuntimeTypeCouldNotBeDeterminedForReference { get { return "The runtime type required for this property could not be determined for the reference name '{0}'."; } }

        public static string NoAssemblyForXmlNamespace { get { return "The xml namespace '{0}' does not correspond to a known assembly namespace. Please check the namespace spelling, or make sure it is declared in the configuration file against a required runtime assembly."; } }

        public static string ComponentCannotBeUsedBasedOnMaxVersion { get { return "The component '{0}' cannot be used. The maximum supported framework for this component is '{1}', but the current loaded framework is '{2}'. Please upgrade to the latest version of this components library"; } }

        public static string ComponentCannotBeUsedBasedOnMinVersion { get { return "The component '{0}' cannot be used. The minimum supported framework for this component is '{1}', but the current loaded framework is '{2}'. Please upgrade to the latest version of the Scryber framework."; } }

        public static string ReservedAttributeNameCannotBeUsed { get { return "The reserved attribute name '{0}' cannot be used on any parsable properties - {1}"; } }

        public static string NoCurrentDataContextValue { get { return "Binding failed as there is not current data content on the binding stack."; } }

        public static string SqlBindingExpressionsCanOnlyUseDataRecords { get { return "The SQL binding expressions can only use IDataRecords as the current data context value"; } }

        public static string CouldNotParseComponentOfType { get { return "The XML Parser could not parse type '{0}'. {1}"; } }

        public static string BindingIsNotSupportedOnType { get { return "The type '{0}' does not support binding at the stage '{1}. To support binding at this stage it must implement the '{2}' interface."; } }

        public static string BindingPrefixIsNotKnown { get { return "The binding prefix '{0}' in expression '{1}' is not a known or registered binding type."; } }

        public static string ControllerTypeCouldNotBeFound { get { return "The declared controller type '{0}' could not be loaded or found. Type names are case sensitive."; } }

        public static string CouldNotSetOutletPropertyValue { get { return "The value of the PDF Outlet '{0}' could not be set on the controllers. See the inner execption for more details."; } }

        public static string OutletsCannotBeStatic { get { return "PDF Outlets can only be public instance properties or fields that can be assigned a value. The member '{0}' is static or shared."; } }

        public static string OutletsCanOnlyBePropertiesOrFields { get { return "PDF Outlets can only be public properties or fields that can be assigned a value. The member '{0}' is a {1}."; } }

        public static string OutletsMustBeReadWrite { get { return "PDF Outlets can only be public instance properties or fields that can be assigned a value. The member '{0}' is read-only"; } }

        public static string ControllerAlreadyHasActionName { get { return "The controller definition '{0}' already has an action defined with the name '{1}'"; } }

        public static string ControllerAlreadyHasOutletWithID { get { return "The controller definition '{0}' already has an outlet defined with the id '{1}'"; } }

        public static string CouldNotParseSource { get { return "The XmlParser could not parse the source file '{0}'. {1}"; } }

    }
}
