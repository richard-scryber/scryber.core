using System;
using Scryber.Components;

namespace Scryber.Html.Components
{

    /// <summary>
    /// Wraps a fragment of html that can be built. The instance itself is ignored and the contents output as part of the document, as it implements the IInvisibleContainer
    /// </summary>
    /// <remarks>
    /// This class is used by the HtmlFragmentParser to wrap any individual content. But the <fragment xmlns=... can be used to include it in any content
    /// </remarks>
    [PDFParsableComponent("fragment")]
	public class HTMLFragmentWrapper : ContainerComponent, INamingContainer, IInvisibleContainer
	{
		public HTMLFragmentWrapper()
			: base(ObjectTypes.Template)
		{
		}

        [PDFArray(typeof(Component))]
        [PDFElement("")]
        public ComponentList Content
        {
            get
            {
                return base.InnerContent;
            }

        }

        // INamingContianer interface

        IComponent INamingContainer.Owner
        {
            get { return this.Parent; }
            set { this.Parent = (Component)value; }
        }
    }
}

