namespace Scryber.Generation.Handlebars
{
    public class HBarHelperMapping
    {
        public string Match { get; set; }
        public HandlebarMatchReplacer Replace { get; set; }

        public HBarHelperMapping(string match, HandlebarMatchReplacer replace)
        {
            this.Match = match;
            this.Replace = replace;
        }

        protected static string SanitizeBindExpression(string expr)
        {
            if (expr.IndexOf('"') >= 0)
                expr =  expr.Replace('\"', '\'');

            if (expr.IndexOf('<') >= 0)
                expr = expr.Replace("<", "&lt;");

            if (expr.IndexOf('&') >= 0)
                expr = expr.Replace("&", "&amp;");

            return expr;
        }
    }
}