using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Scryber.Generation
{
    /// <summary>
    /// Encapsulates a method and name that can be used as a PDF Action on a controller instance
    /// </summary>
    public class ParserControllerAction
    {

        #region public MethodInfo ActionMethod { get; private set; }

        /// <summary>
        /// Gets the Action method associated with this controller action
        /// </summary>
        public MethodInfo ActionMethod { get; private set; }

        #endregion

        #region public string Name { get; private set; }

        /// <summary>
        /// Gets the name of the method associated with this controller action.
        /// </summary>
        public string Name { get; private set; }

        #endregion

        //
        // .ctor
        //

        #region public ParserControllerAction(MethodInfo action, string name)

        /// <summary>
        /// Creates a new controller action that references a method that can be invoked from a template.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="name"></param>
        public ParserControllerAction(MethodInfo action, string name)
        {
            if (null == action)
                throw new ArgumentNullException("action");

            if (string.IsNullOrEmpty(name))
                name = action.Name;

            this.ActionMethod = action;
            this.Name = name;

        }

        #endregion
    }


    /// <summary>
    /// A list of controller actions that can be accessed by index or name (case sensitive)
    /// </summary>
    public class ParserControllerActionList : System.Collections.ObjectModel.KeyedCollection<string, ParserControllerAction>
    {

        #region protected override string GetKeyForItem(ParserControllerAction item)

        /// <summary>
        /// Overrides the base abstract method to return the name of the Action as a key
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override string GetKeyForItem(ParserControllerAction item)
        {
            return item.Name;
        }

        #endregion

        #region public bool TryGetAction(string name, out ParserControllerAction action)

        /// <summary>
        /// Tries to retrieve a specific action with the specified name setting the action value, and true as the result. Otherwise null and returning false.
        /// </summary>
        /// <param name="name">the name of hte action to find</param>
        /// <param name="action">Set to any found action, or null</param>
        /// <returns>True if a matching action was found, otherwise false</returns>
        public bool TryGetAction(string name, out ParserControllerAction action)
        {
            if (this.Count == 0)
            {
                action = null;
                return false;
            }
            else
                return this.Dictionary.TryGetValue(name, out action);
        }

        #endregion
    }
}
