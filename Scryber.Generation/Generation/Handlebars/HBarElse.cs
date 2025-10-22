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
                value = value.Substring(6); //remove '{{#if '
                if (value.Length >= 2 && value.EndsWith("}}"))
                {
                    var prev = tracker.Pop();
                    if (prev.Value.StartsWith("{{#if") || prev.Value.StartsWith("{{else if"))
                        result = "</" + splitter.MappingPrefix + ":when>\n\t";
                    else
                    {
                        throw new Scryber.PDFParserException(
                            "The handle bar helper sequence is not balanced found a match on '" + prev.Value +
                            "' when expecting to end an 'if' or 'else if' clause at index:" + prev.Index);
                    }
                    result += "<" + splitter.MappingPrefix + ":otherwise>";
                    tracker.Push(newMatch);
                }
            }
            else
                ;//else does not have an end token
            
            return result;
        }
    }
}