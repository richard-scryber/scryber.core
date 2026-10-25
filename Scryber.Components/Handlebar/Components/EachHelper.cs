using System.Collections;

namespace Scryber.Handlebar.Components;

/// <summary>
/// Inherits from the looping template component with an &lt;each&gt; element. When the parser pre-filters any handlebar {{#each .. }} tags,
/// they will be replaced with the 'each' element and namespace that maps to this component. Secondary parsing will then pick up the 'each' element and create an instance of this class.
/// </summary>
[PDFParsableComponent("each")]
public class EachHelper : Scryber.Html.Components.HTMLTemplate
{
    protected override void OnDataBinding(DataContext context)
    {
        base.OnDataBinding(context);
    }

    /// <summary>
    /// Overrides the default for the handlebars each helper, so that dictionaries and json objects are iterated over their entries (with the key available as @key),
    /// rather than being bound as a single item (as with the template and ForEach components).
    /// </summary>
    protected override Scryber.Data.Enumerators.IBindingEnumerator GetBehaviourEnumerator(DataBindingBehaviour behaviour, object value, IDataSource source, DataContext context)
    {
        if (behaviour.EnumerateThoughData)
        {
            if (value is IDictionary dict)
            {
                return new Scryber.Data.Enumerators.DictionaryBindingEnumerator(dict, source, behaviour.IncrementContextIndex,
                    behaviour.SetContextData, context);
            }
            else if (value is Newtonsoft.Json.Linq.JObject jobj)
            {
                return new Scryber.Data.Enumerators.JObjectEnumerator(jobj, source, behaviour.IncrementContextIndex,
                    behaviour.SetContextData, context);
            }
#if NET6_0_OR_GREATER
            else if (value is System.Text.Json.JsonElement jele && jele.ValueKind == System.Text.Json.JsonValueKind.Object)
            {
                var ordered = new System.Collections.Specialized.OrderedDictionary();
                foreach (var prop in jele.EnumerateObject())
                    ordered[prop.Name] = prop.Value;

                return new Scryber.Data.Enumerators.DictionaryBindingEnumerator(ordered, source, behaviour.IncrementContextIndex,
                    behaviour.SetContextData, context);
            }
#endif
        }

        return base.GetBehaviourEnumerator(behaviour, value, source, context);
    }
}
