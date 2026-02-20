using System;

namespace Scryber.Options
{
    public class ExpressionOptions
    {

        public const string ExpressionsSection = ScryberOptions.ScryberSectionStub + "Expressions";

        public const bool DefaultUseStdFunctions = true;
        public const bool DefaultCaseSensitive = true;
        public const bool DefaultAllowEval = true;

        public bool UseStandardFunctions { get; set; }

        public bool AllowEval { get; set; }

        public bool IsCaseSensitive { get; set; }
        
        public CustomFunctions[] Register { get; set; }

        public ExpressionOptions()
        {
            UseStandardFunctions = DefaultUseStdFunctions;
            AllowEval = DefaultAllowEval;
            IsCaseSensitive = DefaultCaseSensitive;
        }
    }



    public class CustomFunctions
    {
        public string Name { get; set; }

        public string FactoryType { get; set; }
        
        public bool Enabled { get; set; } = true;

    }



}
