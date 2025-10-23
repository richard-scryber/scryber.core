using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scryber.Generation.Handlebars
{
    public class HBarEach : HBarHelperMapping
    {
        public HBarEach() : base("each", new HandlebarMatchReplacer(ReplaceWith))
        {}

        static string ReplaceWith(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            var result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{#each"))
            {
                if (value.StartsWith("{{#each "))
                {
                    var path = "";
                    value = value.Substring(7); //remove '{{#each '
                    if (value.Length > 2 && value.EndsWith("}}"))
                    {
                        value = value.Substring(0, value.Length - 2); //remove the '}}' at the end.
                        value = value.Trim();
                        path = "data-bind=\"{{" + SanitizeBindExpression(value) + "}}\" ";


                        result = "<" + splitter.MappingPrefix + ":each xmlns:" + splitter.MappingPrefix + "='" +
                                 splitter.MappingNamespace + "' " + path +
                                 ">";
                        tracker.Push(newMatch);
                    }
                }
                else
                {
                    result = "<" + splitter.MappingPrefix + ":each xmlns:" + splitter.MappingPrefix + "='" + splitter.MappingNamespace + "' data-bind='{{.}}' >";
                    tracker.Push(newMatch);
                }
            }
            else if (value.StartsWith("{{/each"))
            {
                if (tracker.Count <= 0)
                    throw new PDFParserException(
                        "The handlebars helper stack is un-balanced. Expecting an existing {{#each to have been previously processed.");
                
                var prev = tracker.Pop();
                if (prev.Value.StartsWith("{{#each"))
                    result = "</" + splitter.MappingPrefix + ":each>";
                else
                {
                    throw new PDFParserException("The '" + prev.Value +
                                                 "' does not match the end /each statement in the content.");
                }
            }

            return result;
        }
    }
}