using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scryber.Generation.Handlebars
{
    public class HBarUsing : HBarHelperMapping
    {
        public HBarUsing() : base("using", new HandlebarMatchReplacer(ReplaceUsing))
        {}

        static string ReplaceUsing(HBarHelperSplitter splitter, Stack<Match> tracker, Match newMatch)
        {
            var result = "";
            var value = newMatch.Value.Trim();
            if (value.StartsWith("{{#using "))
            {
                var path = "";
                value = value.Substring(9); //remove '{{#using '
                if (value.Length > 2 && value.EndsWith("}}"))
                {
                    value = value.Substring(0, value.Length - 2); //remove the '}}' at the end.
                    value = value.Trim();
                    path = "data-bind='{{" + SanitizeBindExpression(value) + "}}'";
                }
                else
                {
                    path = "data-bind='{{.}}'";
                }

                result = "<" + splitter.MappingPrefix + ":using xmlns:" + splitter.MappingPrefix + "='" +
                         splitter.MappingNamespace + "' " + path +
                         " >";
                tracker.Push(newMatch);
            }
            else if (value.StartsWith("{{/using"))
            {
                var prev = tracker.Pop();
                if (prev.Value.StartsWith("{{#using"))
                    result = "</" + splitter.MappingPrefix + ":using>";
                else
                {
                    throw new InvalidOperationException("The '" + prev.Value +
                                                        "' does not match the end /using statement in the content.");
                }
            }
            else
            {
                throw new InvalidOperationException("The Using matcher only works for {{#using and {{/using - does not match the allowed statements in the content.");
            }
            
            return result;
        }
    }
}