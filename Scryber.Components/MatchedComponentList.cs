using Scryber.Components;
using System.Collections.Generic;
using Scryber.Styles.Selectors;

namespace Scryber;

public class MatchedComponentList : List<Component>, IMatchedEnumerable
{
    
    public StyleMatcher MatchedTo { get; private set; }

    public MatchedComponentList(StyleMatcher matchedTo)
    {
        MatchedTo = matchedTo;
    }
    
    public MatchedComponentList Find(string selector)
    {
        if (string.IsNullOrEmpty(selector))
            return new MatchedComponentList(null);
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
        MatchedComponentList all = new MatchedComponentList(matcher);
        foreach (var item in this)
        {
            item.DoFindMatches(all, matcher);
        }
        return all;
    }
    
    //
    // explicit IMatchedEnumerable implementation
    //
    
    IMatchedEnumerable IMatchedEnumerable.Find(string selector)
    {
        return this.Find(selector);
    }

    IEnumerator<IComponent> IEnumerable<IComponent>.GetEnumerator()
    {
        return base.GetEnumerator();
    }
    
}
