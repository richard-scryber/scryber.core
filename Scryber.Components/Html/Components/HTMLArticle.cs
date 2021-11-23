
namespace Scryber.Html.Components
{
    [PDFParsableComponent("article")]
    public class HTMLArticle : HTMLHeadFootContainer
    {
        public HTMLArticle()
        : this((ObjectType)"htAr")
        {
        }

        protected HTMLArticle(ObjectType type)
            : base(type)
        {
            
        }
    }
}
