using System;
namespace Scryber.Styles
{
    public class Errors
    {
        public Errors()
        {
        }

        public static string CanOnlyMergeItemsOfSameType { get { return "The style list can only merge '{0}' style items with '{0}' style items. The merging item '{1}' is not of the same type."; } }

        public static string CanOnlyReferenceExternalStyleDefinitionsOfType { get { return "All external defined files must have a base type of '{0}'."; } }

        public static string CouldNotCreateTheGraphicObjectFromTheStyle { get { return "Could not create the {0} from the style : {1}"; } }

        public static string LoadingOfExternalFilesNotSupported { get { return "The loading of an external file '{0}' is not supported in the base document class."; } }

        public static string NoCurrentComponentInLayoutEngine { get { return "There is no current component in this Layout Engine. Ensure the engine is in the process of laying out a container before calling this method."; } }

        public static string StyleItemNotFound { get { return "The style key '{0}' is not a recognised style key"; } }

    }
}
