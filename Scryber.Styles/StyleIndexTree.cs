using System;
using System.Collections.Generic;
using Scryber.Styles;
using Scryber.Styles.Selectors;

namespace Scryber
{
	/// <summary>
	/// Base class to Index of a list of style definitions that can be enumerator over for matching
	/// </summary>
    /// <remarks>The index is built from the last style selector based</remarks>
	public abstract class StyleIndexTree
	{
        private List<StyleDefn> _directs;

		public StyleIndexTree()
		{
		}

        public virtual bool DoAddDefinition(string id, string eleName, StyleClassSelector cls, StyleDefn defn)
        {
            if(null == _directs)
            {
                _directs = new List<StyleDefn>();
            }
            _directs.Add(defn);
            return true;
        }

        /// <summary>
        /// Returns all the style definitions that match the last selector in the styles that have been added.
        /// </summary>
        /// <param name="id">The </param>
        /// <param name="elementName"></param>
        /// <param name="classes"></param>
        /// <param name="state"></param>
        /// <param name="component"></param>
        /// <returns></returns>
		public virtual IEnumerable<StyleDefn> DoGetMatched(string id, string elementName, string[] classes, IComponent component)
        {
            if (null != this._directs)
            {
                foreach (var found in this._directs)
                {
                    yield return found;
                }
            }
        }
	}

    public class StyleClassIndexTree : StyleIndexTree
    {
        public string ClassName { get; private set; }
        private Dictionary<string, StyleClassIndexTree> _byClass;

        public StyleClassIndexTree(string name)
        {
            this.ClassName = name;
        }


        public override IEnumerable<StyleDefn> DoGetMatched(string id, string elementName, string[] classes, IComponent component)
        {
            if (null != classes && classes.Length > 0)
            {
                if (null != this._byClass)
                {
                    foreach (var cls in classes)
                    {
                        if (this._byClass.TryGetValue(cls, out var tree))
                        {
                            foreach (var found in tree.DoGetMatched(id, elementName, classes, component))
                            {
                                yield return found;
                            }
                        }
                    }
                }
            }

            foreach (var found in base.DoGetMatched(id, elementName, classes, component))
            {
                yield return found;
            }

        }

        public override bool DoAddDefinition(string id, string eleName, StyleClassSelector cls, StyleDefn defn)
        {
            if (null != cls)
            {
                if (null == _byClass)
                {
                    _byClass = new Dictionary<string, StyleClassIndexTree>();
                }

                if (!_byClass.TryGetValue(cls.ClassName, out var tree))
                {
                    tree = new StyleClassIndexTree(cls.ClassName);
                    _byClass.Add(cls.ClassName, tree);
                }

                return tree.DoAddDefinition(id, eleName, cls.AndClass, defn);
            }
            else
            {
                return base.DoAddDefinition(id, eleName, cls, defn);
            }
        }

    }

    public class StyleElementIndexTree : StyleIndexTree
    {
        public string ElementName { get; private set; }
        private Dictionary<string, StyleClassIndexTree> _byClass;

        public StyleElementIndexTree(string name)
        {
            this.ElementName = name;
        }

        public override IEnumerable<StyleDefn> DoGetMatched(string id, string elementName, string[] classes, IComponent component)
        {
            if (null != classes && classes.Length > 0)
            {
                if (null != this._byClass)
                {
                    foreach (var cls in classes)
                    {
                        if (this._byClass.TryGetValue(cls, out var tree))
                        {
                            foreach (var found in tree.DoGetMatched(id, elementName, classes, component))
                            {
                                yield return found;
                            }
                        }
                    }
                }
            }

            foreach (var found in base.DoGetMatched(id, elementName, classes, component))
            {
                yield return found;
            }
        }

        public override bool DoAddDefinition(string id, string eleName, StyleClassSelector cls, StyleDefn defn)
        {
            if (null != cls)
            {
                if (null == _byClass)
                {
                    _byClass = new Dictionary<string, StyleClassIndexTree>();
                }

                if (!_byClass.TryGetValue(cls.ClassName, out var tree))
                {
                    tree = new StyleClassIndexTree(cls.ClassName);
                    _byClass.Add(cls.ClassName, tree);
                }

                return tree.DoAddDefinition(id, eleName, cls.AndClass, defn);
            }
            else
            {
                return base.DoAddDefinition(id, eleName, cls, defn);
            }
        }
    }

    public class StyleIDIndexTree : StyleIndexTree
    {
        public string ID { get; private set; }

        private Dictionary<string, StyleElementIndexTree> _byName;
        private Dictionary<string, StyleClassIndexTree> _byClass;
        private List<StyleDefn> _directs;

        public StyleIDIndexTree(string id)
        {
            this.ID = id;
        }

        public override bool DoAddDefinition(string id, string eleName, StyleClassSelector cls, StyleDefn defn)
        {
            if (!string.IsNullOrEmpty(eleName))
            {
                if (null == _byName)
                {
                    _byName = new Dictionary<string, StyleElementIndexTree>();
                }

                if (!_byName.TryGetValue(eleName, out var tree))
                {
                    tree = new StyleElementIndexTree(eleName);
                    _byName.Add(eleName, tree);
                }

                return tree.DoAddDefinition(id, string.Empty, cls, defn);
            }
            else if (null != cls)
            {
                if (null == _byClass)
                {
                    _byClass = new Dictionary<string, StyleClassIndexTree>();
                }

                if (!_byClass.TryGetValue(cls.ClassName, out var tree))
                {
                    tree = new StyleClassIndexTree(cls.ClassName);
                    _byClass.Add(cls.ClassName, tree);
                }

                return tree.DoAddDefinition(id, eleName, cls.AndClass, defn);
            }
            else
            {
                return base.DoAddDefinition(id, eleName, cls, defn);
            }
        }

        public override IEnumerable<StyleDefn> DoGetMatched(string id, string elementName, string[] classes, IComponent component)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                if (null != this._byName && this._byName.TryGetValue(elementName, out var tree))
                {
                    foreach (var found in tree.DoGetMatched(id, elementName, classes, component))
                    {
                        yield return found;
                    }
                }
            }
            if (null != classes && classes.Length > 0)
            {
                if (null != this._byClass)
                {
                    foreach (var cls in classes)
                    {
                        if (this._byClass.TryGetValue(cls, out var tree))
                        {
                            foreach (var found in tree.DoGetMatched(id, elementName, classes, component))
                            {
                                yield return found;
                            }
                        }
                    }
                }
            }

            foreach (var found in base.DoGetMatched(id, elementName, classes, component))
            {
                yield return found;
            }
        }
    }


    /// <summary>
    /// Top level style collection index - has an inner group of trees by ID, element name, and class
    /// </summary>
    public class StyleRootIndexTree : StyleIndexTree
	{
		private Dictionary<string, StyleIDIndexTree> _byID;
		private Dictionary<string, StyleElementIndexTree> _byName;
		private Dictionary<string, StyleClassIndexTree> _byClass;
        private List<StyleDefn> _roots;
        private List<StyleDefn> _catchAlls;

        /// <summary>
        /// Returns an enumerable set of style definitions that match the last (top) selector only of the definition against the component
        /// </summary>
        /// <param name="component"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IEnumerable<StyleDefn> GetTopMatched(IComponent component)
        {
            var id = component.ID;
            var ele = component.ElementName;
            string classes = null;
            string[] allClasses = null;

            if (component is IStyledComponent styledComponent)
                classes = styledComponent.StyleClass;
            if (!string.IsNullOrEmpty(classes))
            {
                classes = classes.Trim();
                if (classes.IndexOf(" ") >= 0)
                    allClasses = classes.Split(' ');
                else
                    allClasses = new string[] { classes };

            }
            return DoGetMatched(id, ele, allClasses, component);
        }

        public bool AddDefinition(StyleDefn defn)
        {
            var match = defn.Match;
            var sel = match.Selector;
            int count = 0;

            if (match is StyleRootMatcher)
            {
                if(null == _roots) { _roots = new List<StyleDefn>(); }
                _roots.Add(defn);

                count = 1;
            }
            else if(match is StyleCatchAllMatcher)
            {
                if(null == _catchAlls) { _catchAlls = new List<StyleDefn>(); }
                _catchAlls.Add(defn);
            }
            else
            {
                while (null != sel)
                {
                    var id = sel.AppliedID;
                    var ele = sel.AppliedElement;
                    var cls = sel.AppliedClass;
                    if(ele == "*" || ele == "*|*")
                    {
                        if (string.IsNullOrEmpty(id) && null == cls)
                        {
                            if(null == _catchAlls) { _catchAlls = new List<StyleDefn>(); }
                            _catchAlls.Add(defn);
                            count++;
                            sel = null;

                            if(match is StyleMultipleMatcher multiple2)
                            {
                                match = multiple2.Next;
                                if (null != match)
                                    sel = match.Selector;
                            }
                            continue;
                        }
                        else
                            ele = "";
                    }

                    if (DoAddDefinition(id, ele, cls, defn))
                        count++;

                    sel = null;

                    if (match is Scryber.Styles.Selectors.StyleMultipleMatcher multiple)
                    {
                        match = multiple.Next;
                        if (null != match)
                            sel = match.Selector;
                    }

                }
            }

            return count > 0;
            
        }

        public override bool DoAddDefinition(string id, string eleName, StyleClassSelector cls, StyleDefn defn)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if(null == _byID) {
                    _byID = new Dictionary<string, StyleIDIndexTree>();
                }

                if(!_byID.TryGetValue(id, out var tree)){
                    tree = new StyleIDIndexTree(id);
                    _byID.Add(id, tree);
                };

                return tree.DoAddDefinition(string.Empty, eleName, cls, defn);
            }
            else if (!string.IsNullOrEmpty(eleName))
            {
                if(null== _byName){
                    _byName = new Dictionary<string, StyleElementIndexTree>();
                }

                if(!_byName.TryGetValue(eleName, out var tree)){
                    tree = new StyleElementIndexTree(eleName);
                    _byName.Add(eleName, tree);
                }

                return tree.DoAddDefinition(id, string.Empty, cls, defn);
            }
            else if(null != cls)
            {
                if(null == _byClass){
                    _byClass = new Dictionary<string, StyleClassIndexTree>();
                }

                if(!_byClass.TryGetValue(cls.ClassName, out var tree)){
                    tree = new StyleClassIndexTree(cls.ClassName);
                    _byClass.Add(cls.ClassName, tree);
                }

                return tree.DoAddDefinition(id, eleName, cls.AndClass, defn);
            }
            else
            {
                return base.DoAddDefinition(id, eleName, cls, defn);
            }
        }

        public override IEnumerable<StyleDefn> DoGetMatched(string id, string elementName, string[] classes, IComponent component)
        {
            if(null != _catchAlls && _catchAlls.Count > 0)
            {
                //catch alls have the lowest priority - so good to release each in turn

                foreach(var ca in _catchAlls)
                {
                    yield return ca;
                }
            }

            if(null != _roots && _roots.Count > 0 && component is IDocument)
            {
                foreach(var root in _roots)
                {
                    yield return root;
                }
            }
            if(!string.IsNullOrEmpty(id))
            {
                if(null != this._byID && this._byID.TryGetValue(id, out var tree))
                {
                    foreach(var found in tree.DoGetMatched(id, elementName, classes, component))
                    {
                        yield return found;
                    }
                }
            }
            if(!string.IsNullOrEmpty(elementName))
            {
                if(null != this._byName && this._byName.TryGetValue(elementName, out var tree))
                {
                    foreach (var found in tree.DoGetMatched(id, elementName, classes, component))
                    {
                        yield return found;
                    }
                }
            }
            if(null != classes && classes.Length > 0)
            {
                if(null != this._byClass)
                {
                    foreach (var cls in classes)
                    {
                        if(this._byClass.TryGetValue(cls, out var tree))
                        {
                            foreach(var found in tree.DoGetMatched(id, elementName, classes, component))
                            {
                                yield return found;
                            }
                        }
                    }
                }
            }

            foreach (var found in base.DoGetMatched(id, elementName, classes, component))
            {
                yield return found;
            }
        }
    }


	

	


	
}

