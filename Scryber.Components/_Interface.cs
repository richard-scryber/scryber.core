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

using Scryber.PDF.Native;
using Scryber.Drawing;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF.Resources;
using Scryber.Styles;
using Scryber.PDF;

namespace Scryber
{

    //
    // PDFComponent interfaces and related contracts
    //

    #region public interface ITemplateParser : IComponent

    /// <summary>
    /// Interface that all components must implement if they need to parse template files.
    /// </summary>
    public interface ITemplateParser : IComponent
    {
        IComponent ParseTemplate(IRemoteComponent comp, System.IO.TextReader reader);
    }

    #endregion

    #region public interface IContainerComponent : IComponent

    /// <summary>
    /// Interface that identifies a Page Component as a container for multiple child Components
    /// </summary>
    public interface IContainerComponent : IComponent
    {
        bool HasContent { get; }
        /// <summary>
        /// Gets a list of IPDFComponents that are children of this Component
        /// </summary>
        ComponentList Content { get; }
    }

    #endregion

    /// <summary>
    /// Base interface that all document layouts should implement if they are passed as an layout.
    /// </summary>
    public interface IDocumentLayout
    {
        IDocument Owner { get; }

        OutputFormat Format { get; }
    }

    #region public interface ITextComponent : IComponent

    /// <summary>
    /// Interface for any text based Component (has visual content displayed as text on a page) 
    /// </summary>
    public interface ITextComponent : IComponent
    {

        Text.PDFTextReader CreateReader(ContextBase context, Style fullstyle);
    }

    #endregion

    #region public interface IGraphicPathComponent : IComponent

    /// <summary>
    /// Interface for any Component that is displayed as a shape or path
    /// </summary>
    public interface IGraphicPathComponent : IComponent
    {
        GraphicsPath CreatePath(Size avail, Styles.Style fullstyle);

        /// <summary>
        /// Gets or sets the path generated with the CreatePath method
        /// </summary>
        Drawing.GraphicsPath Path { get; set; }
    }

    #endregion

    #region public interface IPDFImageComponent : IComponent

    public interface IPDFImageComponent : IComponent, ILayoutComponent
    {
        /// <summary>
        /// Gets the image resource data associated with this image. 
        /// Returns null if there is no image.
        /// </summary>
        /// <returns></returns>
        PDFImageXObject GetImageObject(ContextBase context, Style imagestyle);

    }

    #endregion
    
    #region public interface ITopAndTailedComponent : IContainerComponent

    /// <summary>
    /// Interface that extends the container Component to include a header and a footer
    /// </summary>
    public interface ITopAndTailedComponent : IContainerComponent
    {

        /// <summary>
        /// Gets or sets the list of Components in the header of this Component
        /// </summary>
        ITemplate Header { get; set; }

        /// <summary>
        /// Gets or sets the list of Components in the footer of this Component
        /// </summary>
        ITemplate Footer { get; set; }
        
    }

    #endregion

    public interface ITopAndTailedContinuationComponent : ITopAndTailedComponent
    {
        /// <summary>
        /// Gets or sets the list of Components in the header on ALL PAGES AFTER of the parent component extends too,
        /// except the first. If not defined, then the the standard footer will be used (if defined)
        /// </summary>
        ITemplate ContinuationHeader { get; set; }

        /// <summary>
        /// Gets or sets the list of Components in the footer on
        /// ALL PAGES PRE-CEEDING of this Component if defined, other-wise the standard footer will be used (if defined).
        /// </summary>
        ITemplate ContinuationFooter { get; set; }
    }
    
    
    
    #region public interface IPDFRenderable
    
    public interface IPDFRenderable
    {
        /// <summary>
        /// Outputs the compenent content to the pdf writer
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer);
    }
    
    #endregion

    #region public interface IPDFRenderComponent : IComponent

    /// <summary>
    /// A Component that supports rendering to a PDF writer
    /// </summary>
    public interface IPDFRenderComponent : IComponent, IPDFRenderable
    {
        /// <summary>
        /// Event that is raised before the Component is rendered to the document
        /// </summary>
        event RenderEventHandler PreRender;
        /// <summary>
        /// Event that is raised after the Component has been rendered
        /// </summary>
        event RenderEventHandler PostRender;

        
    }

    #endregion

    #region public interface IPDFLayoutComponent : IRenderComponent

    /// <summary>
    /// Interface for components to implement that layout their own contents within the content of a PDF page, but don't need to implement a layout engine.
    /// </summary>
    public interface ILayoutComponent : IComponent
    {
        /// <summary>
        /// Returns the required size to be made available within the layout for the component to render into.
        /// </summary>
        /// <param name="available">The current available size</param>
        /// <param name="context">The current layout context</param>
        /// <param name="appliedstyle">The style applied to the component</param>
        /// <returns>The required size of the component content (excluding any padding or margins)</returns>
        Size GetRequiredSizeForLayout(Size available, LayoutContext context, Style appliedstyle);

        /// <summary>
        /// Applies the final render size(s) back to the visual render component.
        /// </summary>
        /// <param name="content">The inner content rectangle</param>
        /// <param name="border">The border rectangle (content + padding)</param>
        /// <param name="total">The total bounds (content + padding + margins)</param>
        /// <param name="style">The full style</param>
        void SetRenderSizes(Rect content, Rect border, Rect total, Style style);
    }

    #endregion

    #region public interface IVisualComponent : IStyledComponent

    /// <summary>
    /// A Visual Component that has a physical dimension and content
    /// </summary>
    public interface IVisualComponent : IStyledComponent
    {

        /// <summary>
        /// Gets or sets the X position of this component
        /// </summary>
        Unit X { get; set; }

        /// <summary>
        /// Gets or sets the Y position of this component
        /// </summary>
        Unit Y { get; set; }

        /// <summary>
        /// Gets or sets the explcit Width of this component
        /// </summary>
        Unit Width { get; set; }

        /// <summary>
        /// Gets or sets the explicit Height of this component
        /// </summary>
        Unit Height { get; set; }

        /// <summary>
        /// Gets the page that contains this Component
        /// </summary>
        Page Page { get; }

        

    }

    #endregion

    #region public interface IDataStyledComponent

    /// <summary>
    /// All instances that implement can have a specific key to their
    /// style so this can be stored repeatably (for multiple components with the same identifier),
    /// rather than dynamically built each time.
    /// </summary>
    public interface IDataStyledComponent
    {
        string DataStyleIdentifier { get; set; }
    }

    #endregion

    #region public interface IInvisibleContainer : IPDFContainerComponent

    /// <summary>
    /// Placeholder for a container that does not affect rendering, 
    /// but it's children are laid out directly in the engine as if the components contents
    /// were part of the parent collection
    /// </summary>
    public interface IInvisibleContainer : IContainerComponent
    {
    }

    #endregion

    #region public interface IPassThroughStyleContainer : IInvisibleContainer
    
    /// <summary>
    /// A special interface that allows the styles declared on an invisible component to 'pass through' (be applied to another component or set of components).
    /// </summary>
    public interface IPassThroughStyleContainer : IInvisibleContainer
    {
        void ApplyStylesToChildren(IPDFLayoutEngine engine, PDFLayoutContext context, Style toPass);
    }
    
    #endregion

    #region public interface IDataSetProviderCommand

    /// <summary>
    /// Specific interface for a provider command that will populate a dataset table with the data retrieved from a command
    /// </summary>
    public interface IDataSetProviderCommand : IComponent
    {
        string GetDataTableName(System.Data.DataSet dataSet);

        object GetNullValue(System.Data.DbType type);

        void FillData(System.Data.DataSet dataset, Scryber.Data.XPathDataSourceBase source, IDataSetProviderCommand parent, DataContext context);
    }

    #endregion

    //
    // layout breaks
    //

    #region ILayoutBreak : IComponent

    /// <summary>
    /// Base interface for a break in the layout
    /// </summary>
    public interface ILayoutBreak : IComponent
    {
        LayoutBreakType BreakType { get; }
    }

    #endregion


    //
    //Support interfaces
    //

    

    //
    // Data Interfaces
    //

    #region public interface IDataProvider

    /// <summary>
    /// Interface for helpers to implement that allows components to make requests for specific data
    /// </summary>
    public interface IDataProvider : IComponent
    {
        //string ID { get; }

        string ProviderKey { get; }

        string DomainRegEx { get; }

        bool IsValid(out string error);

        object GetResponse(string resource, object args, ContextBase context);
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
        PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer);
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
    public interface IPDFFormField : IComponent
    {
        object GetFieldEntry(ContextBase context);
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
    public interface IPDFXObjectComponent : IContainerComponent, IResourceContainer, IPDFRenderComponent
    {
    }

    #endregion
}
