using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// Tests for collection manipulation expression functions: collect(), selectWhere(), sortBy(), reverse(), firstWhere()
    /// </summary>
    [TestClass()]
    public class CoalesceFunctions_Tests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Helper Methods

        private BindingCalcExpressionFactory CreateFactory()
        {
            return new BindingCalcExpressionFactory();
        }

        private T EvaluateExpression<T>(string expression, Dictionary<string, object> vars = null)
        {
            var factory = CreateFactory();
            var expr = factory.CreateExpression(expression);
            return expr.Evaluate<T>(vars ?? new Dictionary<string, object>());
        }

        private object EvaluateExpression(string expression, Dictionary<string, object> vars = null)
        {
            var factory = CreateFactory();
            var expr = factory.CreateExpression(expression);
            return expr.Evaluate(vars ?? new Dictionary<string, object>());
        }

        #endregion

        #region collect() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Collect_Collection_ReturnsArray()
        {
            var vars = new Dictionary<string, object>
            {
                { "nums", new string[] { "one", "two", "three" } }
            };
            var result = EvaluateExpression("collect(nums)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(3, resultArray.Length);
            Assert.AreEqual("one", resultArray[0]);
            Assert.AreEqual("two", resultArray[1]);
            Assert.AreEqual("three", resultArray[2]);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Collect_2Collections_ReturnsArray()
        {
            var vars = new Dictionary<string, object>
            {
                { "nums", new string[] { "one", "two", "three" } },
                { "nums2", new string[] { "four", "five", "six" } }
            };
            var result = EvaluateExpression("collect(nums, nums2)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(6, resultArray.Length);
            Assert.AreEqual("one", resultArray[0]);
            Assert.AreEqual("two", resultArray[1]);
            Assert.AreEqual("three", resultArray[2]);
            Assert.AreEqual("four", resultArray[3]);
            Assert.AreEqual("five", resultArray[4]);
            Assert.AreEqual("six", resultArray[5]);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Collect_EmptyCollection_ReturnsEmptyArray()
        {
            var vars = new Dictionary<string, object>
            {
                { "empty", new string[] { } }
            };
            var result = EvaluateExpression("collect(empty)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(0, resultArray.Length);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Collect_EmptyAndOneCollection_ReturnsOneArray()
        {
            var vars = new Dictionary<string, object>
            {
                { "empty", new string[] { } }
            };
            var result = EvaluateExpression("collect(empty, 'one')", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(1, resultArray.Length);
            Assert.AreEqual("one", resultArray[0]);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Collect_OneAndEmptyCollection_ReturnsOneArray()
        {
            var vars = new Dictionary<string, object>
            {
                { "empty", new string[] { } }
            };
            var result = EvaluateExpression("collect('one', empty)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(1, resultArray.Length);
            Assert.AreEqual("one", resultArray[0]);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Collect_CollectionsAndItems_ReturnsArray()
        {
            var vars = new Dictionary<string, object>
            {
                { "nums", new string[] { "one", "two" } },
                { "nums2", new string[] { "four", "five" } }
            };
            var result = EvaluateExpression("collect(nums, 'three', nums2, 'six')", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(6, resultArray.Length);
            Assert.AreEqual("one", resultArray[0]);
            Assert.AreEqual("two", resultArray[1]);
            Assert.AreEqual("three", resultArray[2]);
            Assert.AreEqual("four", resultArray[3]);
            Assert.AreEqual("five", resultArray[4]);
            Assert.AreEqual("six", resultArray[5]);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Collect_CollectionsItemsEnded_ReturnsArray()
        {
            var vars = new Dictionary<string, object>
            {
                { "nums", new string[] { "two", "three" } },
                { "nums2", new string[] { "four", "five" } }
            };
            var result = EvaluateExpression("collect('one', nums, nums2, 'six')", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(6, resultArray.Length);
            Assert.AreEqual("one", resultArray[0]);
            Assert.AreEqual("two", resultArray[1]);
            Assert.AreEqual("three", resultArray[2]);
            Assert.AreEqual("four", resultArray[3]);
            Assert.AreEqual("five", resultArray[4]);
            Assert.AreEqual("six", resultArray[5]);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Collect_CollectionsItemsTyped_ReturnsArray()
        {
            var vars = new Dictionary<string, object>
            {
                { "nums", new string[] { "two", "three" } },
                { "nums2", new string[] { "four", "five" } }
            };
            var result = EvaluateExpression("collect(1, nums, date(), nums2, bool(1))", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(7, resultArray.Length);
            
            Assert.AreEqual(1, resultArray[0]);
            
            Assert.AreEqual("two", resultArray[1]);
            Assert.AreEqual("three", resultArray[2]);
            
            var dt = (DateTime)resultArray[3];
            Assert.AreEqual(DateTime.Now.Year, dt.Year);
            Assert.AreEqual(DateTime.Now.Month, dt.Month);
            Assert.AreEqual(DateTime.Now.Day, dt.Day);
            
            Assert.AreEqual("four", resultArray[4]);
            Assert.AreEqual("five", resultArray[5]);
            
            Assert.AreEqual(true, resultArray[6]);
        }

        
        #endregion

        #region selectWhere() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SelectWhere_StringProperty_FiltersCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { name = "Item A", status = "active" },
                        new { name = "Item B", status = "inactive" },
                        new { name = "Item C", status = "active" }
                    }
                }
            };
            var result = EvaluateExpression("selectWhere(items, .status == 'active')", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(2, resultArray.Length);
            Assert.AreEqual(((dynamic)resultArray[0]).name,  "Item A");
            Assert.AreEqual(((dynamic)resultArray[1]).name,  "Item C");
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SelectWhere_NumericProperty_FiltersCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "products", new[]
                    {
                        new { name = "Product A", quantity = 10 },
                        new { name = "Product B", quantity = 0 },
                        new { name = "Product C", quantity = 5 }
                    }
                }
            };
            var result = EvaluateExpression("selectWhere(products, .quantity == 0)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(1, resultArray.Length);
            var one =  (dynamic)resultArray[0];
            Assert.AreEqual("Product B", one.name);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SelectWhere_NoMatches_ReturnsEmptyArray()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { name = "Item A", type = "A" },
                        new { name = "Item B", type = "B" }
                    }
                }
            };
            var result = EvaluateExpression("selectWhere(items, .type == 'C')", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(0, resultArray.Length);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SelectWhere_BooleanProperty_FiltersCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "tasks", new[]
                    {
                        new { title = "Task 1", completed = true },
                        new { title = "Task 2", completed = false },
                        new { title = "Task 3", completed = true },
                        new { title = "Task 4", completed = false }
                    }
                }
            };
            var result = EvaluateExpression("selectWhere(tasks, .completed == true)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(2, resultArray.Length);
            Assert.AreEqual(((dynamic)resultArray[0]).title,  "Task 1");
            Assert.AreEqual(((dynamic)resultArray[1]).title,  "Task 3");
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SelectWhere_AllMatch_ReturnsFullArray()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { category = "electronics" },
                        new { category = "electronics" }
                    }
                }
            };
            var result = EvaluateExpression("selectWhere(items, .category == 'electronics')", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(2, resultArray.Length);
        }

        #endregion

        #region sortBy() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SortBy_StringProperty_SortsAlphabetically()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { name = "Charlie" },
                        new { name = "Alice" },
                        new { name = "Bob" }
                    }
                }
            };
            var result = EvaluateExpression("sortBy(items, .name)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(3, resultArray.Length);

            // Check ordering by accessing dynamic properties
            dynamic first = resultArray[0];
            Assert.AreEqual("Alice", first.name);
            
            first = resultArray[1];
            Assert.AreEqual("Bob", first.name);
            
            first = resultArray[2];
            Assert.AreEqual("Charlie", first.name);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SortBy_StringProperty_SortsDescending()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { name = "Charlie" },
                        new { name = "Alice" },
                        new { name = "Bob" }
                    }
                }
            };
            var result = EvaluateExpression("sortBy(items, .name, 'desc')", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(3, resultArray.Length);

            // Check ordering by accessing dynamic properties
            dynamic first = resultArray[0];
            Assert.AreEqual("Charlie", first.name);
            
            first = resultArray[1];
            Assert.AreEqual("Bob", first.name);
            
            first = resultArray[2];
            Assert.AreEqual("Alice", first.name);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SortBy_StringProperty_SortsCaseSensitive()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { name = "alice" },
                        new { name = "Alice" },
                        new { name = "Bob" },
                        new { name = "bob" }
                    }
                }
            };
            var result = EvaluateExpression("sortBy(items, .name, 'asc')", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(4, resultArray.Length);

            // Check ordering by accessing dynamic properties
            dynamic first = resultArray[0];
            Assert.AreEqual("Alice", first.name);
            
            first = resultArray[1];
            Assert.AreEqual("Bob", first.name);
            
            first = resultArray[2];
            Assert.AreEqual("alice", first.name);
            
            first = resultArray[3];
            Assert.AreEqual("bob", first.name);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SortBy_NumericProperty_SortsAscending()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { value = 30 },
                        new { value = 10 },
                        new { value = 20 }
                    }
                }
            };
            var result = EvaluateExpression("sortBy(items, .value)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(3, resultArray.Length);

            dynamic item = resultArray[0];
            Assert.AreEqual(10, item.value);
            
            item = resultArray[1];
            Assert.AreEqual(20, item.value);
            
            item = resultArray[2];
            Assert.AreEqual(30, item.value);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SortBy_AlreadySorted_RemainsInOrder()
        {
            var vars = new Dictionary<string, object>
            {
                { "sorted", new[]
                    {
                        new { id = 1 },
                        new { id = 2 },
                        new { id = 3 }
                    }
                }
            };
            var result = EvaluateExpression("sortBy(sorted, .id)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(3, resultArray.Length);

            dynamic item = resultArray[0];
            Assert.AreEqual(1, item.id);
            
            item = resultArray[1];
            Assert.AreEqual(2, item.id);
            
            item = resultArray[2];
            Assert.AreEqual(3, item.id);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SortBy_AlreadySorted_Descending()
        {
            var vars = new Dictionary<string, object>
            {
                { "sorted", new[]
                    {
                        new { id = 1 },
                        new { id = 2 },
                        new { id = 3 }
                    }
                }
            };
            var result = EvaluateExpression("sortBy(sorted, .id, 'desc')", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(3, resultArray.Length);

            dynamic item = resultArray[0];
            Assert.AreEqual(3, item.id);
            
            item = resultArray[1];
            Assert.AreEqual(2, item.id);
            
            item = resultArray[2];
            Assert.AreEqual(1, item.id);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SortBy_SingleItem_ReturnsUnchanged()
        {
            var vars = new Dictionary<string, object>
            {
                { "single", new[] { new { name = "Only" } } }
            };
            var result = EvaluateExpression("sortBy(single, .name)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(1, resultArray.Length);
            
            dynamic item = resultArray[0];
            Assert.AreEqual("Only", item.name);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void SortBy_EmptyCollection_ReturnsEmpty()
        {
            var vars = new Dictionary<string, object>
            {
                { "empty", new object[] { } }
            };
            var result = EvaluateExpression("sortBy(empty, .name)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(0, resultArray.Length);
        }

        #endregion

        #region reverse() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Reverse_NumericArray_ReversesOrder()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 1, 2, 3, 4, 5 } }
            };
            var result = EvaluateExpression("reverse(numbers)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(5, resultArray.Length);
            Assert.AreEqual(5, resultArray[0]);
            Assert.AreEqual(1, resultArray[4]);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Reverse_StringArray_ReversesOrder()
        {
            var vars = new Dictionary<string, object>
            {
                { "words", new[] { "first", "second", "third" } }
            };
            var result = EvaluateExpression("reverse(words)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(3, resultArray.Length);
            Assert.AreEqual("third", resultArray[0]);
            Assert.AreEqual("second", resultArray[1]);
            Assert.AreEqual("first", resultArray[2]);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Reverse_ObjectArray_ReversesOrder()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { id = 1, name = "First" },
                        new { id = 2, name = "Second" },
                        new { id = 3, name = "Third" }
                    }
                }
            };
            var result = EvaluateExpression("reverse(items)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(3, resultArray.Length);

            dynamic item = resultArray[0];
            Assert.AreEqual(3, item.id);
            item = resultArray[1];
            Assert.AreEqual(2, item.id);
            item = resultArray[2];
            Assert.AreEqual(1, item.id);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Reverse_SingleItem_ReturnsUnchanged()
        {
            var vars = new Dictionary<string, object>
            {
                { "single", new[] { 42 } }
            };
            var result = EvaluateExpression("reverse(single)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(1, resultArray.Length);
            Assert.AreEqual(42, resultArray[0]);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Reverse_EmptyArray_ReturnsEmpty()
        {
            var vars = new Dictionary<string, object>
            {
                { "empty", new int[] { } }
            };
            var result = EvaluateExpression("reverse(empty)", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(0, resultArray.Length);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Reverse_StringArray_IndividualItems()
        {
            var vars = new Dictionary<string, object>
            {
            };
            var result = EvaluateExpression("reverse('first', 'second', 'third', 'fourth', 'fifth')", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(5, resultArray.Length);
            Assert.AreEqual("fifth", resultArray[0]);
            Assert.AreEqual("fourth", resultArray[1]);
            Assert.AreEqual("third", resultArray[2]);
            Assert.AreEqual("second", resultArray[3]);
            Assert.AreEqual("first", resultArray[4]);
            
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void Reverse_StringArray_MixedCollection()
        {
            var vars = new Dictionary<string, object>
            {
                { "words", new[] { "second", "third", "fourth" } }
            };
            var result = EvaluateExpression("reverse('first', words, 'fifth')", vars);
            var resultArray = result as object[];

            Assert.IsNotNull(resultArray);
            Assert.AreEqual(5, resultArray.Length);
            Assert.AreEqual("fifth", resultArray[0]);
            Assert.AreEqual("fourth", resultArray[1]);
            Assert.AreEqual("third", resultArray[2]);
            Assert.AreEqual("second", resultArray[3]);
            Assert.AreEqual("first", resultArray[4]);
            
        }

        #endregion

        #region firstWhere() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void FirstWhere_MatchFound_ReturnsFirstMatch()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { id = 1, status = "pending" },
                        new { id = 2, status = "active" },
                        new { id = 3, status = "active" }
                    }
                }
            };
            var result = EvaluateExpression("firstWhere(items, .status == 'active')", vars);

            Assert.IsNotNull(result);
            dynamic item = result;
            Assert.AreEqual(2, item.id);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void FirstWhere_NumericMatch_ReturnsCorrectItem()
        {
            var vars = new Dictionary<string, object>
            {
                { "products", new[]
                    {
                        new { sku = "ABC", quantity = 10 },
                        new { sku = "DEF", quantity = 0 },
                        new { sku = "GHI", quantity = 5 }
                    }
                }
            };
            var result = EvaluateExpression("firstWhere(products, .quantity == 0)", vars);

            Assert.IsNotNull(result);
            dynamic product = result;
            Assert.AreEqual("DEF", product.sku);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void FirstWhere_NoMatch_ReturnsNull()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { type = "A" },
                        new { type = "B" }
                    }
                }
            };
            var result = EvaluateExpression("firstWhere(items, .type == 'C')", vars);

            Assert.IsNull(result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void FirstWhere_MultipleMatches_ReturnsFirst()
        {
            var vars = new Dictionary<string, object>
            {
                { "users", new[]
                    {
                        new { name = "Alice", role = "admin" },
                        new { name = "Bob", role = "user" },
                        new { name = "Charlie", role = "admin" }
                    }
                }
            };
            var result = EvaluateExpression("firstWhere(users, .role == 'admin')", vars);

            Assert.IsNotNull(result);
            dynamic user = result;
            Assert.AreEqual("Alice", user.name);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Collection")]
        public void FirstWhere_BooleanProperty_FindsMatch()
        {
            var vars = new Dictionary<string, object>
            {
                { "tasks", new[]
                    {
                        new { title = "Task 1", urgent = false },
                        new { title = "Task 2", urgent = true },
                        new { title = "Task 3", urgent = false }
                    }
                }
            };
            var result = EvaluateExpression("firstWhere(tasks, .urgent = true)", vars);

            Assert.IsNotNull(result);
            dynamic task = result;
            Assert.AreEqual("Task 2", task.title);
        }

        #endregion
    }
}
