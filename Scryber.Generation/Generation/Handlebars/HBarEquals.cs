using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scryber.Generation.Handlebars
{
    public class HBarEquals : HBarHelperMapping
    {

        public HBarEquals() : base("equals", new HandlebarMatchReplacer(ReplaceEquals))
        {
        }

        static string ReplaceEquals(HBarHelperSplitter splitter, Stack<Match> matches, Match newMatch)
        {
            return "";
        }
    }
}