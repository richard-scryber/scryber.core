using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// Tests for date/time expression functions
    /// Phase 2: addDays(), addMonths(), addYears(), addHours(), addMinutes(), addSeconds(), addMilliseconds(),
    ///          yearOf(), monthOfYear(), dayOfMonth(), dayOfWeek(), dayOfYear(), hourOf(), minuteOf()
    /// Phase 4: daysBetween(), hoursBetween(), minutesBetween(), secondsBetween(), secondOf(), millisecondOf()
    /// </summary>
    [TestClass()]
    public class DateTimeFunctions_Tests
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

        #endregion

        #region addDays() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddDays_PositiveNumber_AddsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "startDate", new DateTime(2024, 1, 15) }
            };
            var result = EvaluateExpression<DateTime>("addDays(startDate, 10)", vars);
            Assert.AreEqual(new DateTime(2024, 1, 25), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddDays_NegativeNumber_SubtractsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 2, 15) }
            };
            var result = EvaluateExpression<DateTime>("addDays(date, -10)", vars);
            Assert.AreEqual(new DateTime(2024, 2, 5), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddDays_CrossMonthBoundary_HandlesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 1, 30) }
            };
            var result = EvaluateExpression<DateTime>("addDays(date, 5)", vars);
            Assert.AreEqual(new DateTime(2024, 2, 4), result);
        }

        #endregion

        #region addMonths() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddMonths_PositiveNumber_AddsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 1, 15) }
            };
            var result = EvaluateExpression<DateTime>("addMonths(date, 3)", vars);
            Assert.AreEqual(new DateTime(2024, 4, 15), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddMonths_CrossYearBoundary_HandlesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 11, 15) }
            };
            var result = EvaluateExpression<DateTime>("addMonths(date, 3)", vars);
            Assert.AreEqual(new DateTime(2025, 2, 15), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddMonths_NegativeNumber_SubtractsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 6, 15) }
            };
            var result = EvaluateExpression<DateTime>("addMonths(date, -2)", vars);
            Assert.AreEqual(new DateTime(2024, 4, 15), result);
        }

        #endregion

        #region addYears() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddYears_PositiveNumber_AddsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 1, 15) }
            };
            var result = EvaluateExpression<DateTime>("addYears(date, 5)", vars);
            Assert.AreEqual(new DateTime(2029, 1, 15), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddYears_NegativeNumber_SubtractsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 1, 15) }
            };
            var result = EvaluateExpression<DateTime>("addYears(date, -10)", vars);
            Assert.AreEqual(new DateTime(2014, 1, 15), result);
        }

        #endregion

        #region addHours() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddHours_PositiveNumber_AddsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 10, 0, 0) }
            };
            var result = EvaluateExpression<DateTime>("addHours(time, 5)", vars);
            Assert.AreEqual(new DateTime(2024, 1, 15, 15, 0, 0), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddHours_CrossDayBoundary_HandlesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 22, 0, 0) }
            };
            var result = EvaluateExpression<DateTime>("addHours(time, 4)", vars);
            Assert.AreEqual(new DateTime(2024, 1, 16, 2, 0, 0), result);
        }

        #endregion

        #region addMinutes() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddMinutes_PositiveNumber_AddsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 10, 30, 0) }
            };
            var result = EvaluateExpression<DateTime>("addMinutes(time, 45)", vars);
            Assert.AreEqual(new DateTime(2024, 1, 15, 11, 15, 0), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddMinutes_CrossHourBoundary_HandlesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 10, 50, 0) }
            };
            var result = EvaluateExpression<DateTime>("addMinutes(time, 20)", vars);
            Assert.AreEqual(new DateTime(2024, 1, 15, 11, 10, 0), result);
        }

        #endregion

        #region addSeconds() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddSeconds_PositiveNumber_AddsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 10, 30, 15) }
            };
            var result = EvaluateExpression<DateTime>("addSeconds(time, 30)", vars);
            Assert.AreEqual(new DateTime(2024, 1, 15, 10, 30, 45), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddSeconds_CrossMinuteBoundary_HandlesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 10, 30, 50) }
            };
            var result = EvaluateExpression<DateTime>("addSeconds(time, 20)", vars);
            Assert.AreEqual(new DateTime(2024, 1, 15, 10, 31, 10), result);
        }

        #endregion

        #region addMilliseconds() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddMilliseconds_PositiveNumber_AddsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 10, 30, 15, 500) }
            };
            var result = EvaluateExpression<DateTime>("addMilliseconds(time, 250)", vars);
            Assert.AreEqual(new DateTime(2024, 1, 15, 10, 30, 15, 750), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void AddMilliseconds_CrossSecondBoundary_HandlesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 10, 30, 15, 900) }
            };
            var result = EvaluateExpression<DateTime>("addMilliseconds(time, 200)", vars);
            Assert.AreEqual(new DateTime(2024, 1, 15, 10, 30, 16, 100), result);
        }

        #endregion

        #region yearOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void YearOf_ValidDate_ReturnsYear()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 6, 15) }
            };
            var result = EvaluateExpression<int>("yearOf(date)", vars);
            Assert.AreEqual(2024, result);
        }

        #endregion

        #region monthOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MonthOf_ValidDate_ReturnsMonth()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 6, 15) }
            };
            var result = EvaluateExpression<int>("monthOf(date)", vars);
            Assert.AreEqual(6, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MonthOf_January_ReturnsOne()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 1, 1) }
            };
            var result = EvaluateExpression<int>("monthOf(date)", vars);
            Assert.AreEqual(1, result);
        }

        #endregion

        #region dayOfMonth() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void DayOfMonth_ValidDate_ReturnsDay()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 6, 15) }
            };
            var result = EvaluateExpression<int>("dayOfMonth(date)", vars);
            Assert.AreEqual(15, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void DayOfMonth_FirstOfMonth_ReturnsOne()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 3, 1) }
            };
            var result = EvaluateExpression<int>("dayOfMonth(date)", vars);
            Assert.AreEqual(1, result);
        }

        #endregion

        #region dayOfWeek() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void DayOfWeek_ValidDate_ReturnsDayNumber()
        {
            var vars = new Dictionary<string, object>
            {
                // January 15, 2024 is a Monday
                { "date", new DateTime(2024, 1, 15) }
            };
            var result = EvaluateExpression<int>("dayOfWeek(date)", vars);
            // DayOfWeek enum: Sunday=0, Monday=1, Tuesday=2, etc.
            Assert.AreEqual((int)DayOfWeek.Monday, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void DayOfWeek_Sunday_ReturnsZero()
        {
            var vars = new Dictionary<string, object>
            {
                // January 14, 2024 is a Sunday
                { "date", new DateTime(2024, 1, 14) }
            };
            var result = EvaluateExpression<int>("dayOfWeek(date)", vars);
            Assert.AreEqual((int)DayOfWeek.Sunday, result);
        }

        #endregion

        #region dayOfYear() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void DayOfYear_January1_ReturnsOne()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 1, 1) }
            };
            var result = EvaluateExpression<int>("dayOfYear(date)", vars);
            Assert.AreEqual(1, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void DayOfYear_February1_Returns32()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 2, 1) }
            };
            var result = EvaluateExpression<int>("dayOfYear(date)", vars);
            Assert.AreEqual(32, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void DayOfYear_December31NonLeap_Returns365()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2023, 12, 31) }
            };
            var result = EvaluateExpression<int>("dayOfYear(date)", vars);
            Assert.AreEqual(365, result);
        }

        #endregion

        #region hourOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void HourOf_ValidTime_ReturnsHour()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 14, 30, 0) }
            };
            var result = EvaluateExpression<int>("hourOf(time)", vars);
            Assert.AreEqual(14, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void HourOf_Midnight_ReturnsZero()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 0, 0, 0) }
            };
            var result = EvaluateExpression<int>("hourOf(time)", vars);
            Assert.AreEqual(0, result);
        }

        #endregion

        #region minuteOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MinuteOf_ValidTime_ReturnsMinute()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 14, 45, 30) }
            };
            var result = EvaluateExpression<int>("minuteOf(time)", vars);
            Assert.AreEqual(45, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MinuteOf_ZeroMinutes_ReturnsZero()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 14, 0, 30) }
            };
            var result = EvaluateExpression<int>("minuteOf(time)", vars);
            Assert.AreEqual(0, result);
        }

        #endregion

        #region daysBetween() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void DaysBetween_TwoDates_ReturnsCorrectDays()
        {
            var vars = new Dictionary<string, object>
            {
                { "startDate", new DateTime(2024, 1, 1) },
                { "endDate", new DateTime(2024, 1, 10) }
            };
            var result = EvaluateExpression<int>("daysBetween(startDate, endDate)", vars);
            Assert.AreEqual(9, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void DaysBetween_ReversedDates_ReturnsNegative()
        {
            var vars = new Dictionary<string, object>
            {
                { "startDate", new DateTime(2024, 1, 10) },
                { "endDate", new DateTime(2024, 1, 1) }
            };
            var result = EvaluateExpression<int>("daysBetween(startDate, endDate)", vars);
            Assert.AreEqual(-9, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void DaysBetween_SameDate_ReturnsZero()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 1, 15) }
            };
            var result = EvaluateExpression<int>("daysBetween(date, date)", vars);
            Assert.AreEqual(0, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void DaysBetween_CrossMonthBoundary_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "startDate", new DateTime(2024, 1, 25) },
                { "endDate", new DateTime(2024, 2, 5) }
            };
            var result = EvaluateExpression<int>("daysBetween(startDate, endDate)", vars);
            Assert.AreEqual(11, result);
        }

        #endregion

        #region hoursBetween() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void HoursBetween_TwoTimes_ReturnsCorrectHours()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 10, 0, 0) },
                { "endTime", new DateTime(2024, 1, 15, 15, 0, 0) }
            };
            var result = EvaluateExpression<int>("hoursBetween(startTime, endTime)", vars);
            Assert.AreEqual(5, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void HoursBetween_CrossDayBoundary_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 22, 0, 0) },
                { "endTime", new DateTime(2024, 1, 16, 2, 0, 0) }
            };
            var result = EvaluateExpression<int>("hoursBetween(startTime, endTime)", vars);
            Assert.AreEqual(4, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void HoursBetween_FullDay_Returns24()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 0, 0, 0) },
                { "endTime", new DateTime(2024, 1, 16, 0, 0, 0) }
            };
            var result = EvaluateExpression<int>("hoursBetween(startTime, endTime)", vars);
            Assert.AreEqual(24, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void HoursBetween_ReversedTimes_ReturnsNegative()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 15, 0, 0) },
                { "endTime", new DateTime(2024, 1, 15, 10, 0, 0) }
            };
            var result = EvaluateExpression<int>("hoursBetween(startTime, endTime)", vars);
            Assert.AreEqual(-5, result);
        }

        #endregion

        #region minutesBetween() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MinutesBetween_TwoTimes_ReturnsCorrectMinutes()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 10, 30, 0) },
                { "endTime", new DateTime(2024, 1, 15, 11, 15, 0) }
            };
            var result = EvaluateExpression<int>("minutesBetween(startTime, endTime)", vars);
            Assert.AreEqual(45, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MinutesBetween_CrossHourBoundary_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 10, 50, 0) },
                { "endTime", new DateTime(2024, 1, 15, 11, 10, 0) }
            };
            var result = EvaluateExpression<int>("minutesBetween(startTime, endTime)", vars);
            Assert.AreEqual(20, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MinutesBetween_FullHour_Returns60()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 10, 0, 0) },
                { "endTime", new DateTime(2024, 1, 15, 11, 0, 0) }
            };
            var result = EvaluateExpression<int>("minutesBetween(startTime, endTime)", vars);
            Assert.AreEqual(60, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MinutesBetween_ReversedTimes_ReturnsNegative()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 11, 15, 0) },
                { "endTime", new DateTime(2024, 1, 15, 10, 30, 0) }
            };
            var result = EvaluateExpression<int>("minutesBetween(startTime, endTime)", vars);
            Assert.AreEqual(-45, result);
        }

        #endregion

        #region secondsBetween() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void SecondsBetween_TwoTimes_ReturnsCorrectSeconds()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 10, 30, 15) },
                { "endTime", new DateTime(2024, 1, 15, 10, 30, 45) }
            };
            var result = EvaluateExpression<int>("secondsBetween(startTime, endTime)", vars);
            Assert.AreEqual(30, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void SecondsBetween_CrossMinuteBoundary_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 10, 30, 50) },
                { "endTime", new DateTime(2024, 1, 15, 10, 31, 10) }
            };
            var result = EvaluateExpression<int>("secondsBetween(startTime, endTime)", vars);
            Assert.AreEqual(20, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void SecondsBetween_FullMinute_Returns60()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 10, 30, 0) },
                { "endTime", new DateTime(2024, 1, 15, 10, 31, 0) }
            };
            var result = EvaluateExpression<int>("secondsBetween(startTime, endTime)", vars);
            Assert.AreEqual(60, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void SecondsBetween_ReversedTimes_ReturnsNegative()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 10, 30, 45) },
                { "endTime", new DateTime(2024, 1, 15, 10, 30, 15) }
            };
            var result = EvaluateExpression<int>("secondsBetween(startTime, endTime)", vars);
            Assert.AreEqual(-30, result);
        }

        #endregion

        #region millisecondsBetween() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MillisecondsBetween_TwoTimes_ReturnsCorrectMilliseconds()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 10, 30, 15, 100) },
                { "endTime", new DateTime(2024, 1, 15, 10, 30, 15, 350) }
            };
            var result = EvaluateExpression<int>("millisecondsBetween(startTime, endTime)", vars);
            Assert.AreEqual(250, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MillisecondsBetween_CrossSecondBoundary_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 10, 30, 15, 900) },
                { "endTime", new DateTime(2024, 1, 15, 10, 30, 16, 100) }
            };
            var result = EvaluateExpression<int>("millisecondsBetween(startTime, endTime)", vars);
            Assert.AreEqual(200, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MillisecondsBetween_FullSecond_Returns1000()
        {
            var vars = new Dictionary<string, object>
            {
                { "startTime", new DateTime(2024, 1, 15, 10, 30, 15, 0) },
                { "endTime", new DateTime(2024, 1, 15, 10, 30, 16, 0) }
            };
            var result = EvaluateExpression<int>("millisecondsBetween(startTime, endTime)", vars);
            Assert.AreEqual(1000, result);
        }

        #endregion

        #region secondOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void SecondOf_ValidTime_ReturnsSecond()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 14, 45, 37) }
            };
            var result = EvaluateExpression<int>("secondOf(time)", vars);
            Assert.AreEqual(37, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void SecondOf_ZeroSeconds_ReturnsZero()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 14, 45, 0) }
            };
            var result = EvaluateExpression<int>("secondOf(time)", vars);
            Assert.AreEqual(0, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void SecondOf_WithMilliseconds_ReturnsOnlySeconds()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 14, 45, 30, 500) }
            };
            var result = EvaluateExpression<int>("secondOf(time)", vars);
            Assert.AreEqual(30, result);
        }

        #endregion

        #region millisecondOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MillisecondOf_ValidTime_ReturnsMillisecond()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 14, 45, 30, 250) }
            };
            var result = EvaluateExpression<int>("millisecondOf(time)", vars);
            Assert.AreEqual(250, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MillisecondOf_ZeroMilliseconds_ReturnsZero()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 14, 45, 30, 0) }
            };
            var result = EvaluateExpression<int>("millisecondOf(time)", vars);
            Assert.AreEqual(0, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("DateTime")]
        public void MillisecondOf_MaxMilliseconds_Returns999()
        {
            var vars = new Dictionary<string, object>
            {
                { "time", new DateTime(2024, 1, 15, 14, 45, 30, 999) }
            };
            var result = EvaluateExpression<int>("millisecondOf(time)", vars);
            Assert.AreEqual(999, result);
        }

        #endregion
    }
}
