using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scryber.Generation.Handlebars
{
    public class HBarElse : HBarHelperMapping
    {
        public HBarElse() : base("else", new HandlebarMatchReplacer(ReplaceElse))
        {}

        static string ReplaceElse(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            string result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{else"))
            {
                var path = "";
                value = value.Substring(6); //remove '{{else '
                if (value.Length >= 2 && value.EndsWith("}}"))
                {
                    var prev = tracker.Pop();
                    if (prev.Value.StartsWith("{{#if") || prev.Value.StartsWith("{{else if"))
                    {
                        //we are in an if ... else if ... else block.
                        result = "</" + splitter.MappingPrefix + ":when>\n\t";
                        result += "<" + splitter.MappingPrefix + ":otherwise>";
                    }
                    else if (prev.Value.StartsWith("{{#with "))
                    {
                        result = "</Template></" + splitter.MappingPrefix + ":withContent>\n\t";
                        result += "<" + splitter.MappingPrefix + ":elseContent><Template>";
                    }
                    else
                    {
                        throw new Scryber.PDFParserException(
                            "The handle bar helper sequence is not balanced found a match on '" + prev.Value +
                            "' when expecting to end an 'if' or 'else if' clause at index:" + prev.Index);
                    }
                    
                    tracker.Push(newMatch);
                }
            }
            else
                ;//else does not have an end token
            
            return result;
        }
    }
}