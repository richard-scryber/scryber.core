/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Native;
using System.Drawing;
using Scryber.Drawing;
using Scryber.Components;
using Scryber.Layout;
using Scryber.Styles;

namespace Scryber
{

    //
    // PDFComponent interfaces and related contracts
    //

    #region public interface IPDFTemplateParser : IPDFComponent

    /// <summary>
    /// Interface that all components must implement if they need to parse template files.
    /// </summary>
    public interface IPDFTemplateParser : IPDFComponent
    {
        IPDFComponent ParseTemplate(IPDFRemoteComponent comp, System.IO.TextReader reader);
    }

    #endregion

    #region public interface IPDFContainerComponent : IPDFComponent

    /// <summary>
    /// Interface that identifies a Page Component as a container for multiple child Components
    /// </summary>
    public interface IPDFContainerComponent : IPDFComponent
    {
        bool HasContent { get; }
        /// <summary>
        /// Gets a list of IPDFComponents that are children of this Component
        /// </summary>
        ComponentList Content { get; }
    }

    #endregion

    #region public interface IPDFTextComponent : IPDFComponent

    /// <summary>
    /// Interface for any text based Component (has visual content displayed as text on a page) 
    /// </summary>
    public interface IPDFTextComponent : IPDFComponent
    {
        //Text.PDFTextLayout TextLayout { get; set; }

        Text.PDFTextReader CreateReader(PDFLayoutContext context, Style fullstyle);

        //void ResetTextBlock();
    }

    #endregion

    #region public interface IPDFGraphicPathComponent : IPDFComponent

    /// <summary>
    /// Interface for any Component that is displayed as a shape or path
    /// </summary>
    public interface IPDFGraphicPathComponent : IPDFComponent, IPDFRenderComponent
    {
        PDFGraphicsPath CreatePath(PDFSize avail, Styles.Style fullstyle);

        /// <summary>
        /// Gets or sets the path generated with the CreatePath method
        /// </summary>
        Drawing.PDFGraphicsPath Path { get; set; }
    }

    #endregion

    #region public interface IPDFImageComponent : IPDFComponent

    public interface IPDFImageComponent : IPDFComponent, IPDFVisualRenderComponent
    {
        /// <summary>
        /// Gets the image resource data associated with this image. 
        /// Returns null if there is no image.
        /// </summary>
        /// <returns></returns>
        Scryber.Resources.PDFImageXObject GetImageObject(PDFContextBase context, Style imagestyle);

    }

    #endregion
    
    #region public interface IPDFTopAndTailedComponent : IPDFContainerComponent

    /// <summary>
    /// Interface that extends the container Component to include a header and a footer
    /// </summary>
    public interface IPDFTopAndTailedComponent : IPDFContainerComponent
    {

        /// <summary>
        /// Gets or sets the list of Components in the header of this Component
        /// </summary>
        IPDFTemplate Header { get; set; }

        /// <summary>
        /// Gets or sets the list of Components in the footer of this Component
        /// </summary>
        IPDFTemplate Footer { get; set; }
    }

    #endregion

    #region public interface IPDFRenderComponent : IPDFComponent

    /// <summary>
    /// A PDF Component that supports rendering
    /// </summary>
    public interface IPDFRenderComponent : IPDFComponent
    {
        /// <summary>
        /// Event that is raised before the Component is rendered to the document
        /// </summary>
        event PDFRenderEventHandler PreRender;
        /// <summary>
        /// Event that is raised after the Component has been rendered
        /// </summary>
        event PDFRenderEventHandler PostRender;

        /// <summary>
        /// Outputs the compenent content to the pdf writer
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer);
    }

    #endregion

    #region public interface IPDFVisualRenderComponent : IPDFRenderComponent

    /// <summary>
    /// Interface for components to implement that render their own contents within the content of a page, but don't need to implement a layout engine.
    /// </summary>
    public interface IPDFVisualRenderComponent : IPDFRenderComponent
    {
        /// <summary>
        /// Returns the required size to be made available within the layout for the component to render into.
        /// </summary>
        /// <param name="available">The current available size</param>
        /// <param name="context">The current layout context</param>
        /// <param name="appliedstyle">The style applied to the component</param>
        /// <returns>The required size of the component content (excluding any padding or margins)</returns>
        PDFSize GetRequiredSizeForLayout(PDFSize available, PDFLayoutContext context, Style appliedstyle);

        /// <summary>
        /// Applies the final render size(s) back to the visual render component.
        /// </summary>
        /// <param name="content">The inner content rectangle</param>
        /// <param name="border">The border rectangle (content + padding)</param>
        /// <param name="total">The total bounds (content + padding + margins)</param>
        /// <param name="style">The full style</param>
        void SetRenderSizes(PDFRect content, PDFRect border, PDFRect total, Style style);
    }

    #endregion

    #region public interface IPDFVisualComponent : IPDFStyledComponent

    /// <summary>
    /// A PDF Visual Component that has a physical dimension and content
    /// </summary>
    public interface IPDFVisualComponent : IPDFStyledComponent
    {

        /// <summary>
        /// Gets or sets the X position of this component
        /// </summary>
        PDFUnit X { get; set; }

        /// <summary>
        /// Gets or sets the Y position of this component
        /// </summary>
        PDFUnit Y { get; set; }

        /// <summary>
        /// Gets or sets the explcit Width of this component
        /// </summary>
        PDFUnit Width { get; set; }

        /// <summary>
        /// Gets or sets the explicit Height of this component
        /// </summary>
        PDFUnit Height { get; set; }

        /// <summary>
        /// Gets the page that contains this Component
        /// </summary>
        Page Page { get; }

        

    }

    #endregion

    #region public interface IPDFDataStyledComponent

    /// <summary>
    /// All instances that implement can have a specific key to their
    /// style so this can be stored repeatably (for multiple components with the same identifier),
    /// rather than dynamically built each time.
    /// </summary>
    public interface IPDFDataStyledComponent
    {
        string DataStyleIdentifier { get; set; }
    }

    #endregion

    #region public interface IPDFViewPortComponent : IPDFComponent

    /// <summary>
    /// Any Component that implements the IPDFViewPortComponent interface has it's own layout engine
    /// to arrange its child contents and return the size
    /// </summary>
    public interface IPDFViewPortComponent : IPDFComponent
    {
        IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style fullstyle);
    }

    #endregion

    #region public interface IPDFInvisibleContainer : IPDFContainerComponent

    /// <summary>
    /// Placeholder for a container that does not affect rendering, 
    /// but it's children are laid out directly in the engine as if the components contents
    /// were part of the parent collection
    /// </summary>
    public interface IPDFInvisibleContainer : IPDFContainerComponent
    {
    }

    #endregion

    #region public interface IPDFDataSetProviderCommand

    /// <summary>
    /// Specific interface for a provider command that will populate a dataset table with the data retrieved from a command
    /// </summary>
    public interface IPDFDataSetProviderCommand : IPDFComponent
    {
        string GetDataTableName(System.Data.DataSet dataSet);

        object GetNullValue(System.Data.DbType type);

        void FillData(System.Data.DataSet dataset, Scryber.Data.XPathDataSourceBase source, IPDFDataSetProviderCommand parent, PDFDataContext context);
    }

    #endregion

    //
    // layout breaks
    //

    #region IPDFLayoutBreak : IPDFComponent

    /// <summary>
    /// Base interface for a break in the layout
    /// </summary>
    public interface IPDFLayoutBreak : IPDFComponent
    {
        LayoutBreakType BreakType { get; }
    }

    #endregion


    //
    //Support interfaces
    //

    #region  public interface IPDFLayoutEngine

    /// <summary>
    /// Defines the interface for a layout engine. 
    /// This is the actual class that implements the layout of individual Components
    /// </summary>
    public interface IPDFLayoutEngine : IDisposable
    {
        /// <summary>
        /// Gets the parent layout engine that invoked this engine
        /// </summary>
        public IPDFLayoutEngine ParentEngine { get; }

        /// <summary>
        /// Gets or sets the flag for the engine to continue on and layout more components
        /// </summary>
        bool ContinueLayout { get; set; }

        /// <summary>
        /// Gets the current layout context in the engine
        /// </summary>
        PDFLayoutContext Context { get; }

        /// <summary>
        /// Main method to layout items
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        void Layout(PDFLayoutContext context, Style fullstyle);

        /// <summary>
        /// Moves the provided block and region to a new page along with the provided stack of blocks
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="region"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        bool MoveToNextPage(IPDFComponent initiator, Style initiatorStyle, Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region, ref PDFLayoutBlock block);

        /// <summary>
        /// Request to the engine to close the block and begin in a new region
        /// </summary>
        /// <param name="blockToClose"></param>
        /// <param name="joinToRegion"></param>
        /// <returns></returns>
        PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion);
    }

    #endregion

    #region public interface IPDFPooledLayoutEngine : IPDFLayoutEngine

    /// <summary>
    /// If a layout engine can be pooled against a particular object type, 
    /// then this interface dictactes the type and re-initialize method.
    /// </summary>
    public interface IPDFPooledLayoutEngine : IPDFLayoutEngine
    {
        /// <summary>
        /// Gets the type of component this pooled engine can layout
        /// </summary>
        PDFObjectType LayoutType { get; }

        /// <summary>
        /// (Re)Initializes the pooled layout engine so it can lay out a(nother) component
        /// </summary>
        /// <param name="container"></param>
        /// <param name="parent"></param>
        void Init(IPDFComponent container, IPDFLayoutEngine parent);

    }

    #endregion

    

    //
    // Data Interfaces
    //

    #region public interface IPDFDataProvider

    /// <summary>
    /// Interface for helpers to implement that allows components to make requests for specific data
    /// </summary>
    public interface IPDFDataProvider : IPDFDataComponent
    {
        //string ID { get; }

        string ProviderKey { get; }

        string DomainRegEx { get; }

        bool IsValid(out string error);

        object GetResponse(string resource, object args, PDFContextBase context);
    }

    #endregion

    /// <summary>
    /// An extension to the standard .Net data column interface
    /// </summary>
    public interface IPDFDataColumn
    {
        DataType DataType { get; }
    }

    //
    // Artifact interfaces
    //

    #region public interface IArtefactEntry

    /// <summary>
    /// Placeholder interface for an entry in an Artifact collection
    /// </summary>
    public interface IArtefactEntry
    {
    }

    #endregion

    #region public interface ICategorisedArtefactNamesEntry : IArtefactEntry

    /// <summary>
    /// Extended artefact interface to support the category and full name so that the 
    /// can be added to a category NameTree in the layouts documents Names entry
    /// </summary>
    public interface ICategorisedArtefactNamesEntry : IArtefactEntry
    {
        /// <summary>
        /// Gets the category this Names artefact should be registed under.
        /// </summary>
        string NamesCategory { get; }

        /// <summary>
        /// Gets the full idenitifying name this artefact is known by. Duplicates are equivalent
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Outputs the entry to the writer and returns an indirect object that represents the entry data.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        Scryber.Native.PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer);
    }

    #endregion

    #region public interface IArtefactCollection

    /// <summary>
    /// A contract for any collection of Atrifacts registered on a resource container
    /// </summary>
    public interface IArtefactCollection
    {
        string CollectionName { get; }

        //int Count { get; }

        object Register(IArtefactEntry catalogobject);

        void Close(object registration);

        PDFObjectRef[] OutputContentsToPDF(PDFRenderContext context, PDFWriter writer);

        PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer);
    }

    #endregion

    #region public interface IPDFFormField : IPDFComponent

    /// <summary>
    /// Defines the contract for form fields
    /// </summary>
    public interface IPDFFormField : IPDFComponent
    {
        object GetFieldEntry(PDFContextBase context);
    }

    #endregion


    #region public interface IPDFSignatureFormField : IPDFFormField

    /// <summary>
    /// Defines the contract for Signature Fields in a document
    /// </summary>
    public interface IPDFSignatureFormField : IPDFFormField
    {

    }

    #endregion

    

    //
    // Other interfaces
    //

    #region public interface IPDFXObjectComponent : IPDFContainerComponent

    /// <summary>
    /// Interface for XObjects that are rendered into a stream independent of the main page stream of the document.
    /// </summary>
    public interface IPDFXObjectComponent : IPDFContainerComponent, IPDFResourceContainer, IPDFRenderComponent
    {
        IPDFResourceContainer Resources { get; }
    }

    #endregion
}
