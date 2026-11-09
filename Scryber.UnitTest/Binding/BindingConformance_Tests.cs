using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Logging;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Binding
{
    /// <summary>
    /// The agreed behaviour of every binding / css / template error, in both strict and lax conformance modes.
    ///
    ///  - Invalid syntax (template, attribute, css or inside a repeating template) is invalid completely - it always throws,
    ///    when it is parsed, or if it is parsed as part of binding (a template or an evaluation) when it is bound.
    ///  - A root variable that has not been set or is null (e.g. 'model' in model.missing.value) is logged as an error, in both modes.
    ///  - A null part way along, or at the end of, a path (e.g. 'missing' or 'value') is not an error - the value is just null.
    ///  - var(name, default) uses the default if there is no value.
    /// </summary>
    [TestClass()]
    public class BindingConformance_Tests
    {
        private enum Stage { Parse, Bind, Ok }

        private class Outcome
        {
            public Stage Stage;
            public Exception Exception;
            public List<CollectorTraceLogEntry> Errors = new List<CollectorTraceLogEntry>();
            public List<CollectorTraceLogEntry> Warnings = new List<CollectorTraceLogEntry>();
            public Color BackgroundColor = Color.Transparent;
        }

        private static readonly object NoModel = new object();

        private static string Build(string head, string body, bool strict)
        {
            return "<?xml version='1.0' encoding='utf-8'?>\n" +
                   (strict ? "<?scryber parser-mode='strict' ?>\n" : "<?scryber parser-mode='lax' ?>\n") +
                   "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>" + head + "</style></head><body>" + body + "</body></html>";
        }

        private static Outcome Run(string head, string body, object model, bool strict)
        {
            var outcome = new Outcome();
            Document doc;

            try
            {
                using (var sr = new StringReader(Build(head, body, strict)))
                    doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            }
            catch (Exception ex)
            {
                outcome.Stage = Stage.Parse;
                outcome.Exception = ex;
                return outcome;
            }

            doc.AppendTraceLog = true;
            var collector = (doc.TraceLog.GetLogWithName(TraceLog.ScryberAppendTraceLogName) as CollectorTraceLog)
                            ?? (doc.TraceLog as CollectorTraceLog);

            if (!ReferenceEquals(model, NoModel))
                doc.Params["model"] = model;

            try
            {
                using (var ms = new MemoryStream())
                    doc.SaveAsPDF(ms);
            }
            catch (Exception ex)
            {
                outcome.Stage = Stage.Bind;
                outcome.Exception = ex;
                return outcome;
            }

            outcome.Stage = Stage.Ok;
            outcome.Errors = collector.Where(e => e.Level == TraceLevel.Error).ToList();
            outcome.Warnings = collector.Where(e => e.Level == TraceLevel.Warning).ToList();

            if (doc.FindAComponentById("p1") is VisualComponent p)
            {
                var style = p.GetAppliedStyle();
                if (style.TryGetValue(StyleKeys.BgColorKey, out var found))
                    outcome.BackgroundColor = found.Value(style);
            }

            return outcome;
        }

        /// <summary>Runs in strict and lax and asserts the same stage in each.</summary>
        private static void AssertBoth(string head, string body, object model, Stage expected, Action<Outcome, string> more = null)
        {
            foreach (var strict in new[] { true, false })
            {
                var mode = strict ? "strict" : "lax";
                var outcome = Run(head, body, model, strict);
                Assert.AreEqual(expected, outcome.Stage, "Unexpected stage in " + mode + " mode: " + outcome.Exception?.Message);
                more?.Invoke(outcome, mode);
            }
        }

        private static void AssertSingleError(Outcome outcome, string mode)
        {
            Assert.AreEqual(1, outcome.Errors.Count, "Expected exactly one logged error in " + mode + " mode");
            Assert.AreEqual(0, outcome.Warnings.Count, "Expected no logged warnings in " + mode + " mode");
        }

        private static void AssertSilent(Outcome outcome, string mode)
        {
            Assert.AreEqual(0, outcome.Errors.Count, "Expected no logged errors in " + mode + " mode");
            Assert.AreEqual(0, outcome.Warnings.Count, "Expected no logged warnings in " + mode + " mode");
        }

        private const string Text = "<p id='p1'>Value: {{model.missing.value}}</p>";
        private const string InlineVarDefault = "<p id='p1' style='background-color: var(model.missing.value, red);'>Text</p>";
        private const string InlineVarNoDefault = "<p id='p1' style='background-color: var(model.missing.value);'>Text</p>";
        private const string SheetBody = "<p id='p1' class='c1'>Text</p>";
        private const string SheetVarDefault = ".c1 { background-color: var(model.missing.value, red); }";
        private const string SheetVarNoDefault = ".c1 { background-color: var(model.missing.value); }";

        private static readonly object ModelNoMissing = new { other = 1 };
        private static readonly object ModelMissingNoValue = new { missing = new { other = "x" } };
        private static readonly object ModelNullValue = new { missing = new { value = (string)null } };

        // ---------------------------------------------------------------------------------------
        // Invalid syntax
        // ---------------------------------------------------------------------------------------

        [TestMethod()]
        public void InvalidExpression_InMainTemplate_ThrowsOnParse()
        {
            AssertBoth("", "<p>{{concat(model.name, 'x'}}</p>", new { name = "a" }, Stage.Parse);
            AssertBoth("", "<p class=\"{{concat(model.name, 'x'}}\">Text</p>", new { name = "a" }, Stage.Parse);
        }

        [TestMethod()]
        public void InvalidExpression_MissingOperand_InMainTemplate_ThrowsOnBind()
        {
            //These compile but cannot be evaluated - still invalid syntax, so strict and lax both throw.
            var model = new { missing = new { value = "v" }, items = new[] { "a" } };
            AssertBoth("", "<p>{{model.missing.[}}</p>", model, Stage.Bind);
            AssertBoth("", "<p>{{model.items[0}}</p>", model, Stage.Bind);
            AssertBoth("", "<p>{{model.missing..value}}</p>", model, Stage.Bind);
        }

        [TestMethod()]
        public void InvalidExpression_WrongParameterCount_ThrowsOnBind()
        {
            AssertBoth("", "<p>{{abs(1, 2)}}</p>", NoModel, Stage.Bind);
            AssertBoth("", "<p>{{concat()}}</p>", NoModel, Stage.Bind);
        }

        [TestMethod()]
        public void EvalOfInvalidExpressionSuppliedAsData_IsAnEvaluationError_NotTemplateSyntax()
        {
            //The expression string is data, so strict throws as for any evaluation failure, and lax logs a warning and continues.
            var model = new { expr = "unknownfunction(1)" };
            var body = "<p id='p1'>Value: {{eval(model.expr)}}</p>";

            var strict = Run("", body, model, true);
            Assert.AreEqual(Stage.Bind, strict.Stage, "Strict should throw: " + strict.Exception?.Message);

            var lax = Run("", body, model, false);
            Assert.AreEqual(Stage.Ok, lax.Stage, "Lax should continue: " + lax.Exception?.Message);
            Assert.AreEqual(0, lax.Errors.Count);
            Assert.AreEqual(1, lax.Warnings.Count, "Lax should log a single warning");
        }

        [TestMethod()]
        public void InvalidExpression_InRepeatingLoop_ThrowsOnBind()
        {
            AssertBoth("", "{{#each model.items}}<p>{{concat(this.name, 'x'}}</p>{{/each}}",
                new { items = new[] { new { name = "a" }, new { name = "b" } } }, Stage.Bind);
        }

        [TestMethod()]
        public void InvalidXml_InMainTemplate_ThrowsOnParse()
        {
            AssertBoth("", "<p>Unclosed <b>bold</p>", NoModel, Stage.Parse);
        }

        [TestMethod()]
        public void InvalidXml_InRepeatingLoop_ThrowsOnParse()
        {
            //The content of the loop is read as part of the document xml, so it fails when the document is parsed.
            AssertBoth("", "{{#each model.items}}<p>Unclosed <b>bold</p>{{/each}}",
                new { items = new[] { new { name = "a" } } }, Stage.Parse);
        }

        // ---------------------------------------------------------------------------------------
        // Missing values in an expression
        // ---------------------------------------------------------------------------------------

        [TestMethod()]
        public void MissingValue_NoModel_LogsAnError()
        {
            AssertBoth("", Text, NoModel, Stage.Ok, AssertSingleError);
            AssertBoth("", Text, null, Stage.Ok, AssertSingleError);
        }

        [TestMethod()]
        public void MissingValue_NoMissing_IsSilent()
        {
            AssertBoth("", Text, ModelNoMissing, Stage.Ok, AssertSilent);
            AssertBoth("", Text, new { missing = (object)null }, Stage.Ok, AssertSilent);
        }

        [TestMethod()]
        public void MissingValue_NoValue_IsSilent()
        {
            AssertBoth("", Text, ModelMissingNoValue, Stage.Ok, AssertSilent);
            AssertBoth("", Text, ModelNullValue, Stage.Ok, AssertSilent);
        }

        // ---------------------------------------------------------------------------------------
        // css var() with a default
        // ---------------------------------------------------------------------------------------

        private static void AssertDefaultUsed(Outcome outcome, string mode)
        {
            AssertSilent(outcome, mode);
            Assert.AreEqual(StandardColors.Red, outcome.BackgroundColor, "The default should be used in " + mode + " mode");
        }

        [TestMethod()]
        public void CssVarWithDefault_NoModel_UsesDefault()
        {
            AssertBoth("", InlineVarDefault, NoModel, Stage.Ok, AssertDefaultUsed);
            AssertBoth(SheetVarDefault, SheetBody, NoModel, Stage.Ok, AssertDefaultUsed);
        }

        [TestMethod()]
        public void CssVarWithDefault_NoMissing_UsesDefault()
        {
            AssertBoth("", InlineVarDefault, ModelNoMissing, Stage.Ok, AssertDefaultUsed);
            AssertBoth(SheetVarDefault, SheetBody, ModelNoMissing, Stage.Ok, AssertDefaultUsed);
        }

        [TestMethod()]
        public void CssVarWithDefault_NoValue_UsesDefault()
        {
            AssertBoth("", InlineVarDefault, ModelMissingNoValue, Stage.Ok, AssertDefaultUsed);
            AssertBoth(SheetVarDefault, SheetBody, ModelMissingNoValue, Stage.Ok, AssertDefaultUsed);
        }

        // ---------------------------------------------------------------------------------------
        // css var() without a default
        // ---------------------------------------------------------------------------------------

        [TestMethod()]
        public void CssVarNoDefault_NoModel_LogsAnError()
        {
            Action<Outcome, string> check = (outcome, mode) =>
            {
                AssertSingleError(outcome, mode);
                Assert.AreEqual(Color.Transparent, outcome.BackgroundColor, "The value should be left unset in " + mode + " mode");
            };

            AssertBoth("", InlineVarNoDefault, NoModel, Stage.Ok, check);
            AssertBoth(SheetVarNoDefault, SheetBody, NoModel, Stage.Ok, check);
        }

        [TestMethod()]
        public void CssVarNoDefault_NoMissingOrNoValue_IsSilent()
        {
            Action<Outcome, string> check = (outcome, mode) =>
            {
                AssertSilent(outcome, mode);
                Assert.AreEqual(Color.Transparent, outcome.BackgroundColor, "The value should be left unset in " + mode + " mode");
            };

            AssertBoth("", InlineVarNoDefault, ModelNoMissing, Stage.Ok, check);
            AssertBoth("", InlineVarNoDefault, ModelMissingNoValue, Stage.Ok, check);
        }

        // ---------------------------------------------------------------------------------------
        // Invalid css expression syntax
        // ---------------------------------------------------------------------------------------

        [TestMethod()]
        public void CssInvalidExpression_UnclosedBracket_ThrowsWhenParsed()
        {
            //inline style attributes are parsed with the document
            AssertBoth("", "<p id='p1' style='background-color: var(model.value, red;'>T</p>", new { value = "red" }, Stage.Parse);
        }

        [TestMethod()]
        public void CssInvalidExpression_InStylesheet_Throws()
        {
            //Stylesheets are parsed as the document is initialized, so this is raised during the save.
            AssertBoth(":root { --x: var(model.value, red; } .c1 { background-color: var(--x); }", SheetBody, new { value = "red" }, Stage.Bind);
        }

        [TestMethod()]
        public void CssInvalidExpression_MissingOperand_Throws()
        {
            AssertBoth("", "<p id='p1' style='background-color: var(model.missing.[, red);'>T</p>", new { missing = new { value = "red" } }, Stage.Bind);
            AssertBoth(".c1 { width: calc(1 + ); }", SheetBody, NoModel, Stage.Bind);
        }
    }
}
