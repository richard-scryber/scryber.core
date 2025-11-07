using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scryber.Generation.Handlebars
{
    public class HBarElseIf : HBarHelperMapping
    {
        public HBarElseIf(): base("else if", new HandlebarMatchReplacer(ReplaceElseIf))
        {}

        static string ReplaceElseIf(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            string result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{else if "))
            {
                var path = "";
                value = value.Substring(10); //remove '{{else if '
                if (value.Length > 2 && value.EndsWith("}}"))
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
                    
                    value = value.Substring(0, value.Length - 2); //
                    value = value.Trim();
                    path = "data-test=\"{{" + SanitizeBindExpression(value) + "}}\" ";
                    
                    result += "<" + splitter.MappingPrefix + ":when " + path +
                              ">";
                    tracker.Push(newMatch);
                }
            }
            else
                ;//else if does not have an end token
            return result;
        }
    }
}