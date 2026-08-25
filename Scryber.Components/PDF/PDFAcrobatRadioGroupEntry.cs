using System.Collections.Generic;
using Scryber.PDF.Native;

namespace Scryber.PDF
{
    /// <summary>
    /// The canonical PDF structure for a group of radio buttons sharing one field name: a parent
    /// node declaring /T (the shared name), /FT /Btn, /Ff (the Radio bit) and /V (the currently
    /// selected on-value) once, with each radio as a /Kids entry that carries only /AS (its own
    /// current state) and /Parent back to this node - rather than every radio in the group flatly
    /// duplicating /T/FT/Ff/V the way an ungrouped field does. Distinguishing "radio vs checkbox"
    /// by this structure (not just the /Ff Radio bit) is apparently what some readers use for
    /// their own visual rendering, so getting the grouping right matters beyond just correctness.
    /// </summary>
    public class PDFAcrobatRadioGroupEntry : PDFAcrobatFormEntry
    {
        public FormFieldOptions FieldOptions { get; set; }

        private PDFObjectRef _groupRef;
        private List<PDFObjectRef> _kidRefs;

        /// <summary>
        /// True once this group's own indirect object has been opened (by whichever of a
        /// triggering kid or the normal OutputToPDF path got here first) - lets a kid distinguish
        /// "I'm the one that needs to open/complete the group" from "I'm just being rendered as
        /// one of the group's other members, nested inside someone else's trigger".
        /// </summary>
        internal bool IsOpen => null != _groupRef;

        public PDFAcrobatRadioGroupEntry(string name, FormFieldOptions options) : base(name)
        {
            this.FieldOptions = options;
        }

        /// <summary>
        /// Opens this group's own indirect object (a no-op if already open) and sets /Parent on
        /// every kid immediately, so whichever kid is asked to render first - normally one of
        /// this group's own widgets, triggered from the page's own /Annots pass, which runs
        /// before /AcroForm - can write a valid /Parent on its very own first (and, per
        /// PDFAnnotationEntry's caching, only) render. Uses the writer's existing nested-indirect-
        /// object support (the same pattern already used to collect and write a nested /Catalog
        /// reference) rather than needing any "reserve a number, write content later" writer
        /// change - the group's object simply stays open across every kid's own nested
        /// begin/end, and only gets its own dictionary content (and closes) once every kid,
        /// including whichever one triggered this, has actually rendered - see RenderRemaining/
        /// Complete.
        /// </summary>
        private PDFObjectRef Open(PDFWriter writer)
        {
            if (null != _groupRef)
                return _groupRef;

            _groupRef = writer.BeginObject();
            _kidRefs = new List<PDFObjectRef>();

            foreach (var fld in this.Fields)
            {
                if (fld is PDFAcrobatFormCheckWidget check)
                    check.Parent = _groupRef;
            }

            return _groupRef;
        }

        /// <summary>
        /// Renders every kid that hasn't already rendered itself (skipping <paramref name="exclude"/>,
        /// whose own DoOutputToPDF triggered this and is already mid-flight - it supplies its own
        /// ref directly to Complete once it finishes). Safe to call even if some kids already have
        /// a cached ref (PDFAnnotationEntry.OutputToPDF just returns it without re-rendering).
        /// </summary>
        private void RenderRemaining(PDFRenderContext context, PDFWriter writer, PDFAcrobatFormCheckWidget exclude)
        {
            foreach (var fld in this.Fields)
            {
                if (ReferenceEquals(fld, exclude))
                    continue;

                var child = fld.OutputToPDF(context, writer);
                if (null != child)
                {
                    foreach (var oref in child)
                        _kidRefs.Add(oref);
                }
            }
        }

        /// <summary>
        /// Writes this group's own dictionary (/T /FT /Ff /V /Kids) and closes its indirect
        /// object - called once every kid's ref is known, whether that's from the normal
        /// OutputToPDF path (every kid rendered here, in order) or a kid-triggered Open/
        /// RenderRemaining/Complete sequence (the triggering kid's own ref passed in directly,
        /// since it's still rendering itself when this needs to run).
        /// </summary>
        private PDFObjectRef Complete(PDFObjectRef selfRef = null)
        {
            if (null != selfRef)
                _kidRefs.Add(selfRef);

            string selectedValue = null;
            foreach (var fld in this.Fields)
            {
                if (fld is PDFAcrobatFormCheckWidget check && check.IsChecked)
                    selectedValue = string.IsNullOrEmpty(check.OnStateName) ? "on" : check.OnStateName;
            }

            var writer = _writer;
            writer.BeginDictionary();
            writer.WriteDictionaryStringEntry("T", this.Name);
            writer.WriteDictionaryNumberEntry("Ff", (int)this.FieldOptions);
            writer.WriteDictionaryNameEntry("FT", "Btn");
            if (!string.IsNullOrEmpty(selectedValue))
                writer.WriteDictionaryNameEntry("V", selectedValue);

            writer.BeginDictionaryEntry("Kids");
            writer.WriteArrayRefEntries(true, _kidRefs.ToArray());
            writer.EndDictionaryEntry();
            writer.EndDictionary();
            writer.EndObject();

            return _groupRef;
        }

        private PDFWriter _writer;

        /// <summary>
        /// Called by a member widget (from within its own DoOutputToPDF, before it writes its own
        /// object) when it's the one being asked to render first. Opens the group and renders
        /// every other kid nested within it; the caller is then expected to render itself
        /// normally (also nested, since the group is still open) and call CompleteFromKid with
        /// its own resulting ref once done.
        /// </summary>
        internal void BeginFromKid(PDFRenderContext context, PDFWriter writer, PDFAcrobatFormCheckWidget triggeringKid)
        {
            _writer = writer;
            Open(writer);
            RenderRemaining(context, writer, triggeringKid);
        }

        /// <summary>
        /// Called by the triggering kid once it has finished rendering itself - supplies its own
        /// ref so the group's /Kids array is complete, then writes the group's dictionary and
        /// closes it. After this, the group is fully rendered and cached; the normal
        /// OutputToPDF path (from the /AcroForm pass) will just return the cached ref.
        /// </summary>
        internal void CompleteFromKid(PDFObjectRef triggeringKidRef)
        {
            Complete(triggeringKidRef);
        }

        public override IEnumerable<PDFObjectRef> OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (this.Fields.Count == 0)
                return null;

            if (null != _groupRef)
                //Already fully rendered - a kid was asked first (the usual order: page /Annots
                //runs before /AcroForm) and triggered Open/RenderRemaining/Complete already.
                return new PDFObjectRef[] { _groupRef };

            //Nothing triggered this early (no kid's own /Annots entry ran first) - fall back to
            //doing the whole thing directly, in the original order.
            _writer = writer;
            Open(writer);
            RenderRemaining(context, writer, exclude: null);
            var oref = Complete();

            return new PDFObjectRef[] { oref };
        }
    }
}
