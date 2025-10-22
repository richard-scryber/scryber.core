using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Scryber.Generation.Handlebars
{
    public class HBarWith : HBarHelperMapping
    {
        public HBarWith() : base("with", new HandlebarMatchReplacer(ReplaceWith))
        {}

        static string ReplaceWith(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            
            
            return "";
        }
    }
}