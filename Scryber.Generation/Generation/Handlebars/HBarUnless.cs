using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Scryber.Generation.Handlebars
{
    public class HBarUnless : HBarHelperMapping
    {
        public HBarUnless() : base("unless", new HandlebarMatchReplacer(ReplaceIf))
        {}

        static string ReplaceIf(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            string result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{#unless "))
            {
                var path = "";
                value = value.Substring(10); //remove '{{#unless '
                if (value.Length > 2 && value.EndsWith("}}"))
                {
                    value = value.Substring(0, value.Length - 2); //
                    value = value.Trim();
                    path = "data-test=\"{{" + SanitizeBindExpression(value) + "}}\" ";
                    
                    result = "<" + splitter.MappingPrefix + ":choose xmlns:" + splitter.MappingPrefix + "='" +
                             splitter.MappingNamespace + "' >\n\t<" + splitter.MappingPrefix + ":whenNot " + path +
                             ">"; //This is When Not for Unless
                    tracker.Push(newMatch);
                }
            }
            else if (value.StartsWith("{{/unless"))
            {
                if (tracker.Count <= 0)
                    throw new PDFParserException(
                        "The Handlebars stack is un-balanced. Expecting an existing {{#with to have been previously processed.");
                
                var prev = tracker.Pop();

                if (prev.Value.StartsWith("{{else"))
                {
                    result += "</" + splitter.MappingPrefix + ":otherwise>";
                }
                else if (prev.Value.StartsWith("{{#unless"))
                {
                    result += "</" + splitter.MappingPrefix + ":whenNot>";
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