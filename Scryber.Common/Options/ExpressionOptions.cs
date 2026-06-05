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

        public CustomFunctionOption()
        {}

        public CustomFunctionOption(string name, CustomFunctionType type, string typeName, string assemblyName,
            bool forceOverride = false, bool enabled = true)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
            
            FunctionType = typeName ?? throw new ArgumentNullException(nameof(typeName));
            FunctionAssembly = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            
            Override = forceOverride;
            Enabled = enabled;
        }
        
        public CustomFunctionOption(string name, CustomFunctionType type, object functionInstance,
            bool forceOverride = false, bool enabled = true)
        {
            if (null == functionInstance)
                throw new ArgumentNullException(nameof(functionInstance));

            _instance = functionInstance;
            
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
            
            FunctionType = functionInstance.GetType().FullName;
            FunctionAssembly = functionInstance.GetType().Assembly.FullName;
            
            Override = forceOverride;
            Enabled = enabled;
        }

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
