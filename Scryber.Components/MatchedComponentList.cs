using Scryber.Components;
using System.Collections.Generic;
using Scryber.Styles.Selectors;

namespace Scryber;

public class MatchedComponentList : List<Component>, IMatchedEnumerable
{
    
    public MatchedComponentList Previous { get; private set; }
    
    public StyleMatcher MatchedTo { get; private set; }
    

    public MatchedComponentList(StyleMatcher matchedTo, MatchedComponentList previous)
    {
        MatchedTo = matchedTo;
        Previous = previous;
    }
    
    public MatchedComponentList FindMatches(string selector)
    {
        if (string.IsNullOrEmpty(selector))
            return new MatchedComponentList(null, this);
        else
        {
            var matcher = StyleMatcher.Parse(selector);
            return DoFindMatches(matcher);
        }
        
    }
    
    public MatchedComponentList FindMatches(StyleMatcher matcher)
    {
        return DoFindMatches(matcher);
    }

    protected virtual MatchedComponentList DoFindMatches(StyleMatcher matcher)
    {
        MatchedComponentList all = new MatchedComponentList(matcher, this);
        foreach (var item in this)
        {
            item.DoFindMatches(all, matcher);
        }
        return all;
    }
    
    //
    // explicit IMatchedEnumerable implementation
    //

    IComponent IMatchedEnumerable.this[int index] => this[index];

    IMatchedEnumerable IMatchedEnumerable.FindMatches(string selector)
    {
        return this.FindMatches(selector);
    }

    IEnumerator<IComponent> IEnumerable<IComponent>.GetEnumerator()
    {
        return base.GetEnumerator();
    }
    
}
