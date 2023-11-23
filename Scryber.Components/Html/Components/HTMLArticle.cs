
namespace Scryber.Html.Components
{
    [PDFParsableComponent("article")]
    public class HTMLArticle : HTMLHeadFootContainer
    {
        public HTMLArticle()
        : this(HTMLObjectTypes.Article)
        {
        }

        protected HTMLArticle(ObjectType type)
            : base(type)
        {
            
        }
    }
}
