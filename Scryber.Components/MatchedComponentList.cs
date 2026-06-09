using Scryber.Components;
using System.Collections.Generic;
using Scryber.Styles.Selectors;

namespace Scryber;

public class MatchedComponentList : List<Component>, IMatchedEnumerable
{
    public IMatchedEnumerable Find(string selector)
    {
        if (string.IsNullOrEmpty(selector))
            return new MatchedComponentList();
        else
        {
            var matcher = StyleMatcher.Parse(selector);
            return DoFindMatches(matcher);
        }
        
    }
    
    public MatchedComponentList Find(StyleMatcher matcher)
    {
        return DoFindMatches(matcher);
    }

    protected virtual MatchedComponentList DoFindMatches(StyleMatcher matcher)
    {
        MatchedComponentList all = new MatchedComponentList();
        foreach (var item in this)
        {
            if (item is Component component)
            {
                component.DoFindMatches(all, matcher);
            }
        }
        return all;
    }

    IEnumerator<IComponent> IEnumerable<IComponent>.GetEnumerator()
    {
        return base.GetEnumerator();
    }
}
