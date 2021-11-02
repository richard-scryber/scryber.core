using System;
using Scryber.Styles;
using System.Collections.Generic;
using Scryber.PDF.Layout;

namespace Scryber.PDF
{


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
        IPDFLayoutEngine ParentEngine { get; }

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
        bool MoveToNextPage(IComponent initiator, Style initiatorStyle, Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region, ref PDFLayoutBlock block);

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
        ObjectType LayoutType { get; }

        /// <summary>
        /// (Re)Initializes the pooled layout engine so it can lay out a(nother) component
        /// </summary>
        /// <param name="container"></param>
        /// <param name="parent"></param>
        void Init(IComponent container, IPDFLayoutEngine parent);

    }

    #endregion


    #region public interface IPDFViewPortComponent : IPDFComponent

    /// <summary>
    /// Any Component that implements the IPDFViewPortComponent interface has it's own PDF layout engine
    /// to arrange its child contents and return the size
    /// </summary>
    public interface IPDFViewPortComponent : IComponent
    {
        IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDF.PDFLayoutContext context, Style fullstyle);
    }

    #endregion

}
