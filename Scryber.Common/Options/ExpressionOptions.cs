using System;
using System.Collections.Generic;

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
        
        public List<CustomFunctionOption> Register { get; set; }

        public ExpressionOptions()
        {
            UseStandardFunctions = DefaultUseStdFunctions;
            AllowEval = DefaultAllowEval;
            IsCaseSensitive = DefaultCaseSensitive;
        }
    }



    public class CustomFunctionOption
    {
        public string Name { get; set; }

        public CustomFunctionType Type { get; set; } = CustomFunctionType.Function;
        
        public bool Override { get; set; } = false;

        public string FunctionType { get; set; }
        
        public string FunctionAssembly { get; set; }
        
        public bool Enabled { get; set; } = true;

        private object _instance;
        
        public object GetInstance()
        {
            //no locking - may be invoked twice, but not an issue
            if(null == this._instance)
                this._instance = Utilities.TypeHelper.GetInstance<object>(FunctionType, FunctionAssembly);
            
            return this._instance;
        }

    }

    public enum CustomFunctionType
    {
        Operator,
        Function
    }



}
