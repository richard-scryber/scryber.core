using System.Collections.Generic;
using Scryber.Drawing;
using Scryber.Expressive.Exceptions;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.Image
{
    /// <summary>
    /// meta(path) returns the list of EXIF metadata keys available for the image at that path (as
    /// it would be resolved for an &lt;img src="..."&gt; from the same document), or an empty list
    /// if the image has no EXIF data / couldn't be resolved. meta(path, key) returns the string
    /// form of that one value, or null if the key isn't present. Never throws for a missing image
    /// or absent EXIF data - both silently resolve to an empty/null result, matching the rest of
    /// Scryber's lax-by-default template evaluation.
    ///
    /// The image lookup itself happens via IImageMetadataResolver, threaded through under the
    /// reserved ImageMetaVars.ResolverVar variable name (see BindingCalcExpression.BindComponent)
    /// rather than referenced directly - Scryber.Expressive sits below Scryber.Components in the
    /// project dependency graph and can't see Document/SharedResources.
    /// </summary>
    public class MetaFunction : FunctionBase
    {
        public override string Name => "meta";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
        {
            this.ValidateParameterCount(parameters, -1, 1);
            if (parameters.Length > 2)
                throw new ParameterCountMismatchException($"{this.Name}() takes at most 2 arguments");

            var path = parameters[0].Evaluate(variables) as string;
            if (string.IsNullOrEmpty(path))
                return parameters.Length == 1 ? (object)new List<string>() : null;

            IImageMetadataResolver resolver = null;
            if (variables.TryGetValue(ImageMetaVars.ResolverVar, out var resolverObj))
                resolver = resolverObj as IImageMetadataResolver;

            ImageEXIFMap map = null;
            resolver?.TryGetImageMetadata(path, out map);

            if (parameters.Length == 1)
            {
                var keys = new List<string>();
                if (null != map)
                    keys.AddRange(map.Keys);
                return keys;
            }
            else
            {
                var key = parameters[1].Evaluate(variables) as string;
                if (string.IsNullOrEmpty(key) || null == map)
                    return null;

                var value = map.Get(key);
                return value?.ToString();
            }
        }
    }
}
