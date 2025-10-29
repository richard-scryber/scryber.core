using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Scryber.Generation.Handlebars;

namespace Scryber.Generation
{
    public class HBarHelperSplitter
    {
        

        
        private static readonly Regex _helper_old = new Regex("{{#([a-z]+)\\s*(.*)*}}|{{\\/([a-z]*)}}|{{else}}|{{else\\s+if\\s(.*)}}|{{(log)\\s+(.*)}}");

        private static readonly Regex _helper =
            new Regex("{{#([a-z]+)\\s*([^{])*}}|{{\\/([a-z]*)}}|{{(else)}}|{{(else\\s+if)\\s(.*)}}|{{(log)\\s+(.*)}}");

        private static readonly Dictionary<string, HBarHelperMapping> _knownMappings =
            new Dictionary<string, HBarHelperMapping>()
            {
                { "using", new HBarUsing() },
                { "with", new HBarWith() },
                { "equals", new HBarEquals()},
                { "if", new HBarIf()},
                {"each", new HBarEach()},
                {"else", new HBarElse()},
                {"else if", new HBarElseIf()},
                {"log", new HBarLog()}
            };
        
        private string _mappingNamespace;
        private string _mappingPrefix;

        public string MappingNamespace
        {
            get => _mappingNamespace;
            set => _mappingNamespace = value;
        }

        public string MappingPrefix
        {
            get => _mappingPrefix;
            set => _mappingPrefix = value;
        }
        
        private Dictionary<string, HBarHelperMapping> _keyedHelpers;

        public HBarHelperSplitter() : this(XHtmlHandleHelperReader.HandlebarNamespace,
            XHtmlHandleHelperReader.HandlebarPrefix)
        {
        }

        public HBarHelperSplitter(string mappingNamespace, string mappingPrefix) : this(mappingNamespace, mappingPrefix, _knownMappings)
        {}

        
        public HBarHelperSplitter(string mappingNamespace, string mappingPrefix,
            params HBarHelperMapping[] mappingHelpers)
        {
            this._mappingNamespace = mappingNamespace;
            this._mappingPrefix = mappingPrefix;
            this._keyedHelpers = new Dictionary<string, HBarHelperMapping>();
            
            foreach (var helper in mappingHelpers)
            {
                this._keyedHelpers[helper.Match] = helper;
            }
        }

        private HBarHelperSplitter(string mappingNamespace, string mappingPrefix,
            Dictionary<string, HBarHelperMapping> mappings)
        {
            this._mappingNamespace = mappingNamespace;
            this._mappingPrefix = mappingPrefix;
            
            this._keyedHelpers = mappings;
        }
        
        public string ReplaceAll(string input)
        {
            Stack<Match> looping = new Stack<Match>();
            const int GlobalMatchIndex = 0;
            const int HelperStartIndex = 1;
            const int HelperEndIndex = 3;
            const int HelperElseIndex = 4;
            const int HelperElseIfIndex = 5;
            const int HelperLogIndex = 7;
            
            var updated = _helper.Replace(input, (match) =>
            {
                var name = match.Groups[HelperStartIndex].Value;
                if (this._keyedHelpers.TryGetValue(name, out var starthelper))
                    return starthelper.Replace(this, looping, match);
                
                name = match.Groups[HelperEndIndex].Value;
                if (this._keyedHelpers.TryGetValue(name, out var endhelper))
                    return endhelper.Replace(this, looping, match);

                name = match.Groups[GlobalMatchIndex].Value;
                if (this._keyedHelpers.TryGetValue(name, out var globalHelper))
                    return globalHelper.Replace(this, looping, match);

                name = match.Groups[HelperElseIndex].Value;
                if (this._keyedHelpers.TryGetValue(name, out var elseHelper))
                    return elseHelper.Replace(this, looping, match);
                
                name = match.Groups[HelperElseIfIndex].Value;
                if (this._keyedHelpers.TryGetValue(name, out var elseifHelper))
                    return elseifHelper.Replace(this, looping, match);

                name = match.Groups[HelperLogIndex].Value;
                if (this._keyedHelpers.TryGetValue(name, out var logHelper))
                    return logHelper.Replace(this, looping, match);
                
                return "";

            });


            return updated;
        }

    }
}

