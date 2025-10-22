using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Scryber.Generation.Handlebars
{
    public class HBarIf : HBarHelperMapping
    {
        public HBarIf() : base("if", new HandlebarMatchReplacer(ReplaceIf))
        {}

        static string ReplaceIf(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            string result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{#if "))
            {
                var path = "";
                value = value.Substring(6); //remove '{{#if '
                if (value.Length > 2 && value.EndsWith("}}"))
                {
                    value = value.Substring(0, value.Length - 2); //
                    value = value.Trim();
                    path = "data-test=\"{{" + SanitizeBindExpression(value) + "}}\" ";
                    
                    result = "<" + splitter.MappingPrefix + ":choose xmlns:" + splitter.MappingPrefix + "='" +
                             splitter.MappingNamespace + "' >\n\t<" + splitter.MappingPrefix + ":when " + path +
                             ">";
                    tracker.Push(newMatch);
                }
            }
            else if (value.StartsWith("{{/if"))
            {
                var prev = tracker.Pop();

                if (prev.Value.StartsWith("{{else if"))
                {
                    result += "</" + splitter.MappingPrefix + ":when>";
                }
                else if (prev.Value.StartsWith("{{else"))
                {
                    result += "</" + splitter.MappingPrefix + ":otherwise>";
                }
                else if (prev.Value.StartsWith("{{#if"))
                {
                    result += "</" + splitter.MappingPrefix + ":when>";
                }
                else
                {
                    throw new Scryber.PDFParserException(
                        "The handle bar helper sequence is not balanced found a match on '" + prev.Value +
                        "' when expecting to end an 'if' clause at index:" + prev.Index);
                }

                result += "\n</" + splitter.MappingPrefix + ":choose>";
            }

            return result;
        }
    }
}