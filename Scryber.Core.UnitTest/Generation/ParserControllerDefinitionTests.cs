using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Generation;
using Scryber.Components;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.Core.UnitTests.Generation
{

    #region Test Mock Classes

    /// <summary>
    /// Adds the properties, methods and attributes for testing the ParsedDefinitionController.
    /// </summary>
    public class TestParsedController : TestParsedControllerBase
    {
        [PDFOutlet]
        public PDFLabel LabelField;

        [PDFOutlet]
        public PDFPlaceHolder PlaceholderProperty { get; set; }

        [PDFOutlet]
        public static PDFImage InvalidStaticField;

        [PDFOutlet]
        public static PDFImage InvalidStaticProperty { get; set; }

        [PDFOutlet]
        protected PDFImage InvalidProtectedField;

        [PDFOutlet]
        protected PDFImage InvalidProtectedProperty { get; set; }

        [PDFOutlet("mylabel")]
        public PDFLabel LabelFieldOtherName { get; set; }

        [PDFOutlet("otherlabel", Required = true)]
        public PDFLabel RequiredLabelOtherName { get; set; }

        [PDFOutlet(IsOutlet=false)]
        public PDFLabel ExplicitExcludedLabel { get; set; }

        /// <summary>
        /// No Declaration - rely on base implementation
        /// </summary>
        public override PDFLabel OverridenBaseClassLabel
        {
            get
            {
                return base.OverridenBaseClassLabel;
            }
            set
            {
                base.OverridenBaseClassLabel = value;
            }
        }

        [PDFOutlet("renamedbaselabel")]
        public override PDFLabel RenamedBaseClassLabel
        {
            get
            {
                return base.RenamedBaseClassLabel;
            }
            set
            {
                base.RenamedBaseClassLabel = value;
            }
        }

        [PDFOutlet(IsOutlet=false)]
        public override PDFLabel ExcludedInSubClass
        {
            get
            {
                return base.ExcludedInSubClass;
            }
            set
            {
                base.ExcludedInSubClass = value;
            }
        }


        

        [PDFAction()]
        public void SimpleAction() { }

        [PDFAction("otheraction")]
        public void SimpleActionOtherName() { }

        [PDFAction(IsAction=false)]
        public void ExcludedAction()
        { }

        public void NotAnAction()
        {
        }

        [PDFAction()]
        public static void StaticNotAnAction()
        {

        }

        [PDFAction()]
        protected void ProtectedNotAnAction()
        {

        }

        public override void OverridenBaseAction()
        {
            base.OverridenBaseAction();
        }

        [PDFAction("newActionName")]
        public override void RenamedBaseAction()
        {
            base.RenamedBaseAction();
        }

        [PDFAction(IsAction=false)]
        public override void ExcludedBaseAction()
        {
            base.ExcludedBaseAction();
        }

    }

    /// <summary>
    /// Base class for the TestParsedController so we can override properites and test inheritance
    /// </summary>
    public class TestParsedControllerBase
    {
        [PDFOutlet]
        public PDFLabel BaseClassLabel { get; set; }

        [PDFOutlet]
        public virtual PDFLabel OverridenBaseClassLabel { get; set; }

        [PDFOutlet]
        public virtual PDFLabel RenamedBaseClassLabel { get; set; }

        [PDFOutlet]
        public virtual PDFLabel ExcludedInSubClass { get; set; }

        [PDFAction()]
        public void SimpleBaseAction()
        {
        }

        [PDFAction()]
        public virtual void OverridenBaseAction()
        {
        }

        public virtual void RenamedBaseAction()
        { }

        public virtual void ExcludedBaseAction()
        {

        }
    }

    #endregion


    [TestClass()]
    public class ParserControllerDefinitionTests
    {
        const string TypeName = "Scryber.Core.UnitTests.Generation.TestParsedController, Scryber.Core.UnitTests";

        #region public void TestParserControllerDefinition_Loading()

        [TestMethod()]
        [TestCategory("ParserControllerDefinition")]
        public void TestParserControllerDefinition_Loading()
        {
            //Check the factory loads the controller definition
            
            ParserControllerDefinition defn = ParserDefintionFactory.GetControllerDefinition(TypeName);
            Assert.IsNotNull(defn);

            Assert.AreEqual(TypeName, defn.ControllerTypeName);
            Assert.IsNotNull(defn.ControllerType);
            Assert.AreEqual(typeof(TestParsedController), defn.ControllerType);
            Assert.IsNotNull(defn.Outlets);
            Assert.IsNotNull(defn.Actions);
        }

        #endregion

        #region public void TestParserControllerDefinition_Outlets()

        [TestMethod()]
        [TestCategory("ParserControllerDefinition")]
        public void TestParserControllerDefinition_Outlets()
        {
            //Check the reported properties and fields on the definition itself

            ParserControllerDefinition defn = ParserDefintionFactory.GetControllerDefinition(TypeName);
            Assert.IsNotNull(defn);

            ParserControllerOutlet outlet;

            bool expected = true; //Field
            bool actual = defn.Outlets.TryGetOutlet("LabelField", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(outlet);
            Assert.AreEqual("LabelField", outlet.ID);
            Assert.IsNotNull(outlet.OutletMember);
            Assert.IsFalse(outlet.Required);
            Assert.AreEqual(MemberTypes.Field, outlet.OutletMember.MemberType);
            Assert.AreEqual("LabelField", outlet.OutletMember.Name);

            expected = true; //Property
            actual = defn.Outlets.TryGetOutlet("PlaceholderProperty", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(outlet);
            Assert.AreEqual("PlaceholderProperty", outlet.ID);
            Assert.IsNotNull(outlet.OutletMember);
            Assert.IsFalse(outlet.Required);
            Assert.AreEqual(MemberTypes.Property, outlet.OutletMember.MemberType);
            Assert.AreEqual("PlaceholderProperty", outlet.OutletMember.Name);

            expected = false; //should not be there.
            actual = defn.Outlets.TryGetOutlet("InvalidStaticField", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(outlet);

            expected = false; //should not be there.
            actual = defn.Outlets.TryGetOutlet("InvalidStaticProperty", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(outlet);

            expected = false; //should not be there.
            actual = defn.Outlets.TryGetOutlet("InvalidProtectedField", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(outlet);

            expected = false; //should not be there.
            actual = defn.Outlets.TryGetOutlet("InvalidProtectedProperty", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(outlet);

            expected = true; //Renamed
            actual = defn.Outlets.TryGetOutlet("mylabel", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(outlet);
            Assert.AreEqual("mylabel", outlet.ID);
            Assert.IsFalse(outlet.Required);
            Assert.IsNotNull(outlet.OutletMember);
            Assert.AreEqual(MemberTypes.Property, outlet.OutletMember.MemberType);
            Assert.AreEqual("LabelFieldOtherName", outlet.OutletMember.Name);

            expected = true; //Renamed and required
            actual = defn.Outlets.TryGetOutlet("otherlabel", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(outlet);
            Assert.AreEqual("otherlabel", outlet.ID);
            Assert.IsTrue(outlet.Required);
            Assert.IsNotNull(outlet.OutletMember);
            Assert.AreEqual(MemberTypes.Property, outlet.OutletMember.MemberType);
            Assert.AreEqual("RequiredLabelOtherName", outlet.OutletMember.Name);

            expected = false; //Explicit Exclusion
            actual = defn.Outlets.TryGetOutlet("ExplicitExcludedLabel", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(outlet);


            expected = true; //Property declared on base class
            actual = defn.Outlets.TryGetOutlet("BaseClassLabel", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(outlet);
            Assert.AreEqual("BaseClassLabel", outlet.ID);
            Assert.IsNotNull(outlet.OutletMember);
            Assert.IsFalse(outlet.Required);
            Assert.AreEqual(MemberTypes.Property, outlet.OutletMember.MemberType);
            Assert.AreEqual("BaseClassLabel", outlet.OutletMember.Name);


            expected = true; //Property declared on base class overriden on sub class - no attribute on override
            actual = defn.Outlets.TryGetOutlet("OverridenBaseClassLabel", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(outlet);
            Assert.AreEqual("OverridenBaseClassLabel", outlet.ID);
            Assert.IsNotNull(outlet.OutletMember);
            Assert.IsFalse(outlet.Required);
            Assert.AreEqual(MemberTypes.Property, outlet.OutletMember.MemberType);
            Assert.AreEqual("OverridenBaseClassLabel", outlet.OutletMember.Name);

            expected = true; //Property declared on base class, overriden on subclass - renamed
            actual = defn.Outlets.TryGetOutlet("renamedbaselabel", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(outlet);
            Assert.AreEqual("renamedbaselabel", outlet.ID);
            Assert.IsNotNull(outlet.OutletMember);
            Assert.IsFalse(outlet.Required);
            Assert.AreEqual(MemberTypes.Property, outlet.OutletMember.MemberType);
            Assert.AreEqual("RenamedBaseClassLabel", outlet.OutletMember.Name);


            expected = false; //Property declared on base class overriden on sub class - attribute IsOutlet=false
            actual = defn.Outlets.TryGetOutlet("ExcludedInSubClass", out outlet);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(outlet);
        }

        #endregion


        [TestMethod()]
        [TestCategory("ParserControllerDefinition")]
        public void TestParserControllerDefinition_Actions()
        {
            ParserControllerDefinition defn = ParserDefintionFactory.GetControllerDefinition(TypeName);
            Assert.IsNotNull(defn);

            ParserControllerAction action;

            bool expected = true; //Simple declared method
            bool actual = defn.Actions.TryGetAction("SimpleAction", out action);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(action);
            Assert.AreEqual("SimpleAction", action.Name);
            Assert.IsNotNull(action.ActionMethod);
            Assert.AreEqual(MemberTypes.Method, action.ActionMethod.MemberType);
            Assert.AreEqual("SimpleAction", action.ActionMethod.Name);


            expected = true; //Renamed method - SimpleActionOtherName
            actual = defn.Actions.TryGetAction("otheraction", out action);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(action);
            Assert.AreEqual("otheraction", action.Name);
            Assert.IsNotNull(action.ActionMethod);
            Assert.AreEqual(MemberTypes.Method, action.ActionMethod.MemberType);
            Assert.AreEqual("SimpleActionOtherName", action.ActionMethod.Name);


            expected = false; //Explicit exclusion method - ExcludedAction
            actual = defn.Actions.TryGetAction("ExcludedAction", out action);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(action);

            expected = false; //No attribute - NotAnAction
            actual = defn.Actions.TryGetAction("NotAnAction", out action);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(action);

            expected = false; //static method - StaticNotAnAction
            actual = defn.Actions.TryGetAction("StaticNotAnAction", out action);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(action);

            expected = false; //static method - ProtectedNotAnAction
            actual = defn.Actions.TryGetAction("ProtectedNotAnAction", out action);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(action);

            expected = true; //overriden method no attribute on override - OverridenBaseAction
            actual = defn.Actions.TryGetAction("OverridenBaseAction", out action);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(action);
            Assert.AreEqual("OverridenBaseAction", action.Name);
            Assert.IsNotNull(action.ActionMethod);
            Assert.AreEqual(MemberTypes.Method, action.ActionMethod.MemberType);
            Assert.AreEqual("OverridenBaseAction", action.ActionMethod.Name);

            expected = true; //Renamed overriden method - RenamedBaseAction
            actual = defn.Actions.TryGetAction("newActionName", out action);
            Assert.AreEqual(expected, actual);
            Assert.IsNotNull(action);
            Assert.AreEqual("newActionName", action.Name);
            Assert.IsNotNull(action.ActionMethod);
            Assert.AreEqual(MemberTypes.Method, action.ActionMethod.MemberType);
            Assert.AreEqual("RenamedBaseAction", action.ActionMethod.Name);

            expected = false; //overriden method attribute with IsAction=false - ExcludedBaseAction
            actual = defn.Actions.TryGetAction("ExcludedBaseAction", out action);
            Assert.AreEqual(expected, actual);
            Assert.IsNull(action);

        }
    }


    
}
