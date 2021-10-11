using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using Scryber.Binding;
using Scryber.Logging;

namespace Scryber.Core.UnitTests.Generation
{
    [TestClass()]
    public class ParserItemBindingPathTests
    {

        public object FakeStringConvertor(string value, Type required)
        {
            if (null == value)
                return "NULL";
            else
                return value;
        }

        public class FakeRecipient
        {
            public string StringProperty { get; set; }

        }

        public class FakeSource
        {
            public string Value { get; set; }

            public FakeSource Child { get; set; }

            public List<FakeSource> Items { get; set; }

            public Dictionary<string, FakeSource> Keys { get; set; }
        }


        /// <summary>
        /// Simple test to assign the string value in the items collection to the recipient
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding Item Expressions")]
        public void BuildSingleExpressionPath()
        {

            //Property to set
            System.Reflection.PropertyInfo prop = typeof(FakeRecipient).GetProperty("StringProperty");

            //expression and binding item
            string expr = "Root";
            BindingItemExpression itembind = BindingItemExpression.Create(expr, prop);

            //Set the items value to be extracted
            DataBindEventArgs args = CreateDataBindArgs();
            string expected = "This is the test";
            args.Context.Items["Root"] = expected;

            //bind the recipient - should have its property set to the item value
            FakeRecipient recip = new FakeRecipient();
            itembind.BindComponent(recip, args);

            string actual = recip.StringProperty;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the item binding when the value is not present in the items collection
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding Item Expressions")]
        public void BuildNotSetExpressionPath()
        {

            //Property to set
            System.Reflection.PropertyInfo prop = typeof(FakeRecipient).GetProperty("StringProperty");


            //expression and binding item
            string expr = "Root";
            BindingItemExpression itembind = BindingItemExpression.Create(expr, prop);

            //DO NOT Set the items value to be extracted
            DataBindEventArgs args = CreateDataBindArgs();
            //string expected = "This is the test";
            //args.Context.Items["Root"] = expected;

            //bind the recipient - should have its property set to the item value
            FakeRecipient recip = new FakeRecipient();
            itembind.BindComponent(recip, args);

            string actual = recip.StringProperty;
            Assert.IsNull(actual);
        }

        /// <summary>
        ///Test to assign the string value from an instance in the items collection to the recipient
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding Item Expressions")]
        public void BuildInnerExpressionPath()
        {

            //Property to set
            System.Reflection.PropertyInfo prop = typeof(FakeRecipient).GetProperty("StringProperty");


            //expression and binding item
            string expr = "Root.Value";
            BindingItemExpression itembind = BindingItemExpression.Create(expr, prop);

            //Set the items value to be extracted as an inner property on the source
            DataBindEventArgs args = CreateDataBindArgs();
            string expected = "This is the test";
            FakeSource root = new FakeSource();
            root.Value = expected;

            args.Context.Items["Root"] = root;

            //bind the recipient - should have its property set to the item value
            FakeRecipient recip = new FakeRecipient();
            itembind.BindComponent(recip, args);

            string actual = recip.StringProperty;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test to assign the string value from an instance within a collection in the items collection to the recipient
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding Item Expressions")]
        public void BuildInnerIndexedExpressionPath()
        {
            //Build the object graph
            string expected = "This is the test";
            FakeSource inner = new FakeSource();
            inner.Value = expected;
            FakeSource root = new FakeSource();
            root.Items = new List<FakeSource>();
            root.Items.Add(null);
            root.Items.Add(inner);

            //expression and binding item
            string expr = "Root.Items[1].Value";

            //Property to set
            System.Reflection.PropertyInfo prop = typeof(FakeRecipient).GetProperty("StringProperty");


            //Create the expression binding
            BindingItemExpression itembind = BindingItemExpression.Create(expr, prop);

            //Set the items value to be extracted as an inner property on the source
            DataBindEventArgs args = CreateDataBindArgs();

            //Set the root entry to the top of the object braph
            args.Context.Items["Root"] = root;

            //bind the recipient - should have its property set to the item value
            FakeRecipient recip = new FakeRecipient();
            itembind.BindComponent(recip, args);

            //String Property should be set to the expected value
            string actual = recip.StringProperty;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test to assign the string value from an instance within a collection in the items collection to the recipient
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding Item Expressions")]
        public void BuildInnerInnerKeyedExpressionPath()
        {
            //Build the object graph
            string expected = "This is the test";
            FakeSource innerinner = new FakeSource();
            innerinner.Value = expected;

            FakeSource inner = new FakeSource();
            inner.Keys = new Dictionary<string, FakeSource>();
            inner.Keys["First"] = innerinner;

            FakeSource root = new FakeSource();
            root.Items = new List<FakeSource>();
            root.Items.Add(null);
            root.Items.Add(inner);

            //expression and binding item
            string expr = "Root.Items[1].Keys['First'].Value";

            //Property to set
            System.Reflection.PropertyInfo prop = typeof(FakeRecipient).GetProperty("StringProperty");


            //Create the expression binding
            BindingItemExpression itembind = BindingItemExpression.Create(expr, prop);

            //Set the items value to be extracted as an inner property on the source
            DataBindEventArgs args = CreateDataBindArgs();

            //Set the root entry to the top of the object braph
            args.Context.Items["Root"] = root;

            //bind the recipient - should have its property set to the item value
            FakeRecipient recip = new FakeRecipient();
            itembind.BindComponent(recip, args);

            //String Property should be set to the expected value
            string actual = recip.StringProperty;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [TestCategory("Binding Item Expressions")]
        public void BuildInnerDynamicExpressionPath()
        {
            string expected = "This is a dynamic property value";
            dynamic data = new ExpandoObject();
            data.MyProperty = expected;

            //expression and binding item
            string expr = "Dynamic.MyProperty";

            //Property to set
            System.Reflection.PropertyInfo prop = typeof(FakeRecipient).GetProperty("StringProperty");


            //Create the expression binding
            BindingItemExpression itembind = BindingItemExpression.Create(expr, prop);

            //Set the items value to be extracted as an inner property on the source
            DataBindEventArgs args = CreateDataBindArgs();

            //Set the root entry to the top of the object braph
            args.Context.Items["Dynamic"] = data;

            //bind the recipient - should have its property set to the item value
            FakeRecipient recip = new FakeRecipient();
            itembind.BindComponent(recip, args);

            //String Property should be set to the expected value
            string actual = recip.StringProperty;
            Assert.AreEqual(expected, actual);

            //
            // with string indexing
            //

            expected = "This is another dynamic property value";
            data.MyProperty = expected;

            //Create the expression binding
            expr = "Dynamic['MyProperty']";
            itembind = BindingItemExpression.Create(expr, prop);

            //Set the items value to be extracted as an inner property on the source
            args = CreateDataBindArgs();

            //Set the root entry to the top of the object braph
            args.Context.Items["Dynamic"] = data;

            //bind the recipient - should have its property set to the item value
            recip = new FakeRecipient();
            itembind.BindComponent(recip, args);

            //String Property should be set to the expected value
            actual = recip.StringProperty;
            Assert.AreEqual(expected, actual);

        }

        private static DataBindEventArgs CreateDataBindArgs()
        {
            var config = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            Assert.IsNotNull(config, "THere is no scryber config service");

            var log = config.TracingOptions.GetTraceLog();

            ItemCollection items = new ItemCollection(null);
            DataContext context = new DataContext(items, log, new PerformanceMonitor(true), null, OutputFormat.PDF);
            DataBindEventArgs args = new DataBindEventArgs(context);
            return args;
        }


       
    }
}
