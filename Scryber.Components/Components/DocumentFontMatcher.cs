using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Scryber.Drawing;
using Scryber.PDF.Resources;

namespace Scryber.Components
{
    public partial class Document
    {
        /// <summary>
        /// Inner class for matching fonts and getting the closest match.
        /// </summary>
        /// <remarks>
        /// Uses the exact match for each of the selectors in the font.
        /// Then the nearest weight, then the regular style if italic, and finally courier if still not found.
        /// Stores matches in a collection for future.
        /// </remarks>
        protected class DocumentFontMatcher
        {
            public Document OwnerDocument
            {
                get;
                private set;
            }


            private Dictionary<string, PDFFontResource> _matchedFonts = new Dictionary<string, PDFFontResource>();
            private Dictionary<string, PDFFontResource> _substitutes = new Dictionary<string, PDFFontResource>();

            private Dictionary<string, PDFFontResource> ExactMatched
            {
                get { return _matchedFonts; }
            }

            private Dictionary<string, PDFFontResource> Substitutes
            {
                get { return _substitutes; }
            }
                

            public DocumentFontMatcher(Document document)
            {
                this.OwnerDocument = document ?? throw new ArgumentNullException(nameof(document));
            }

            public PDFFontResource GetFont(PDFFont font, bool create)
            {
                string type = PDFResource.FontDefnResourceType;


                string name;
                PDFFontResource found;
                PDFFontDefinition loaded;

                //Look for an existing exacy match based on the font

                if (this.TryGetExistingExactMatch(font, out found, out name))
                {
                    font.SetResourceFont(name, found);
                    return found;
                }

                if (create)
                {
                    //No existing exact match

                    if (this.TryLoadExactFontDefinition(font, out loaded, out name))
                    {
                        //Found one in the FontFactory
                        var rsrc = this.RegisterNewDocumentFontResource(name, font.FontWeight, font.FontStyle, loaded, false);
                        font.SetResourceFont(name, rsrc);
                        return rsrc;
                    }

                    //No exact match in the factory

                    if(this.OwnerDocument.RenderOptions.UseFontSubstitution)
                    {
                        if(this.TryGetExistingCloseMatch(font, out found, out name))
                        {
                            this.OwnerDocument.TraceLog.Add(TraceLevel.Warning, "Document", "The font '" + font.FullName + "' could not be found, falling back to " + found.Definition.Family + ": " + found.Definition.Weight + (found.Definition.Italic ? " Italic" : "") + " for rendering");
                            font.SetResourceFont(name, found);
                            return found;
                        }

                        //no existing close matches - find a loaded close match

                        if(this.TryGetExistingFontForSubstitution(font, out found, out name))
                        {
                            this.OwnerDocument.TraceLog.Add(TraceLevel.Warning, "Document", "The font '" + font.FullName + "' could not be found, falling back to " + found.Definition.Family + ": " + found.Definition.Weight + (found.Definition.Italic ? " Italic" : "") + " for rendering");
                            this.RegisterFontMatch(found, name, font.FontWeight, font.FontStyle, true);
                            font.SetResourceFont(name, found);

                            return found;
                        }
                    }

                    this.OwnerDocument.TraceLog.Add(TraceLevel.Error, "Document", "The font '" + font.FullName + "' could not be loaded, falling back to Courier for rendering");

                    found = GetCourierSubstitution(font.Selector.FamilyName, font.FontWeight, font.FontStyle);
                    this.RegisterFontMatch(found, font.Selector.FamilyName, font.FontWeight, font.FontStyle, true);
                    font.SetResourceFont(font.Selector.FamilyName, found);
                    return found;

                }

                font.ClearResourceFont();
                return null;
            }


            #region protected bool TryGetExistingExactMatch(PDFFont font, out PDFFontResource match, out string matchedFamily)

            /// <summary>
            /// Tries to get an exactly matching font resource from the existing shared resources.
            /// </summary>
            /// <param name="font"></param>
            /// <param name="match"></param>
            /// <param name="matchedFamily"></param>
            /// <returns></returns>
            protected bool TryGetExistingExactMatch(PDFFont font, out PDFFontResource match, out string matchedFamily)
            {
                var sel = font.Selector;

                while (null != sel)
                {
                    foreach (var rsrc in this.OwnerDocument.SharedResources)
                    {
                        if (rsrc is PDFFontResource frsc)
                        {
                            if (frsc.IsExactMatch(sel.FamilyName, font.FontWeight, font.FontStyle))
                            {
                                matchedFamily = sel.FamilyName;
                                match = frsc;
                                return true;
                            }
                        }
                    }

                    sel = sel.Next;
                }

                match = null;
                matchedFamily = null;
                return false;
            }

            #endregion

            #region protected bool TryLoadExactFontDefinition(PDFFont font, out PDFFontDefinition definition, out string matchedFamily)

            /// <summary>
            /// Tries to load an exactly matching font from the font factory
            /// </summary>
            /// <param name="font">The font to attempt to load</param>
            /// <param name="definition">If found, set to the loaded definition</param>
            /// <param name="matchedFamily">Set to the actual family in the font selector, that matched</param>
            /// <returns>True if a family, style and weight were exactly matched, else false</returns>
            protected bool TryLoadExactFontDefinition(PDFFont font, out PDFFontDefinition definition, out string matchedFamily)
            {
                var sel = font.Selector;
                while (null != sel)
                {
                    definition = PDFFontFactory.GetFontDefinition(sel.FamilyName, font.FontStyle, font.FontWeight, false);
                    if (null != definition)
                    {
                        matchedFamily = sel.FamilyName;
                        return true;
                    }

                    sel = sel.Next;
                }

                definition = null;
                matchedFamily = null;
                return false;
            }

            #endregion

            #region protected bool TryGetExistingCloseMatch(PDFFont font, out PDFFontResource match, out string matchedFamily)

            /// <summary>
            /// Looks in the current document resources for a font that has previously been matched against a selector.
            /// </summary>
            /// <param name="font">The font to match against</param>
            /// <param name="match">Set to the found resource, or null</param>
            /// <param name="matchedFamily">Set to the name of the matched family</param>
            /// <returns>True if an existing match is found, otherwise false</returns>
            protected bool TryGetExistingCloseMatch(PDFFont font, out PDFFontResource match, out string matchedFamily)
            {
                var sel = font.Selector;

                while (null != sel)
                {
                    foreach (var rsrc in this.OwnerDocument.SharedResources)
                    {
                        if (rsrc is PDFFontResource frsc)
                        {
                            if (frsc.IsSubstitutionMatch(sel.FamilyName, font.FontWeight, font.FontStyle))
                            {
                                match = frsc;
                                matchedFamily = sel.FamilyName;
                                return true;
                            }
                        }
                    }

                    sel = sel.Next;
                }

                match = null;
                matchedFamily = null;
                return false;
            }


            #endregion

            #region private bool TryGetExistingFontForSubstitution(PDFFont forFont, out PDFFontResource match, out string matchedFamily)

            //We use a single look up buffer in the document
            private List<PDFFontResource> _lookupBuffer = new List<PDFFontResource>();

            private bool TryGetExistingFontForSubstitution(PDFFont forFont, out PDFFontResource match, out string matchedFamily)
            {
                int bestProximity = int.MaxValue;
                var sel = forFont.Selector;

                match = null;
                matchedFamily = null;

                while (null != sel)
                {
                    int currProximity;
                    PDFFontResource currResource;

                    if (this.TryGetExistingFontForSubstitution(sel.FamilyName, forFont.FontWeight, forFont.FontStyle, out currProximity, out currResource)
                        && currProximity < bestProximity)
                    {
                        match = currResource;
                        matchedFamily = sel.FamilyName;
                        bestProximity = currProximity;
                    }

                    sel = sel.Next;
                }

                sel = forFont.Selector;
                PDFFontDefinition matchedDefn = null;

                while(null != sel)
                {
                    int currProximity = bestProximity;
                    PDFFontDefinition currDefinition;

                    if(this.TryGetDefinitionForSubstitution(sel.FamilyName, forFont.FontWeight, forFont.FontStyle, ref currProximity, out currDefinition)
                        && currProximity < bestProximity)
                    {
                        matchedDefn = currDefinition;
                        matchedFamily = sel.FamilyName;
                        bestProximity = currProximity;
                    }

                    sel = sel.Next;
                }

                if(null != matchedDefn)
                {
                    match = this.RegisterNewDocumentFontResource(matchedFamily, forFont.FontWeight, forFont.FontStyle, matchedDefn, bestProximity > 0);
                }

                return null != match;
            }

            #endregion

            #region private bool TryGetDefinitionForSubstitution(string familyName, int fontWeight, FontStyle fontStyle, ref int proximity, out PDFFontDefinition bestDefn)

            private bool TryGetDefinitionForSubstitution(string familyName, int fontWeight, FontStyle fontStyle, ref int proximity, out PDFFontDefinition bestDefn)
            {
                
                PDFFontFactory.FontReference foundRef = null;

                foreach (var reference in PDFFontFactory.GetAllFontsForFamilyAndStyle(familyName, fontStyle))
                {
                    
                    int newProx = Math.Abs(fontWeight - reference.Weight);

                    //if we don't have a closest match
                    //or this resource more closely matches
                    if (newProx < proximity)
                    {
                        proximity = newProx;
                        foundRef = reference;
                    }
                }

                //We found a reference with a higher proximity.
                if(null != foundRef)
                {
                    bestDefn = PDFFontFactory.GetFontDefinition(foundRef.FamilyName, foundRef.Style, foundRef.Weight);
                    return true;
                }
                else if(fontStyle != FontStyle.Regular && proximity == int.MaxValue)
                {
                    //We have not found one with the right style, and nothing has been found in the current resources
                    //So lets fall back to regular and see what we can find.
                    return this.TryGetDefinitionForSubstitution(familyName, fontWeight, FontStyle.Regular, ref proximity, out bestDefn);
                }
                else
                {
                    bestDefn = null;
                    return false;
                }



            }

            #endregion

            #region private bool TryGetExistingFontForSubstitution(string familyName, int fontWeight, FontStyle fontStyle, out int proximity, out PDFFontResource bestMatch)

            private bool TryGetExistingFontForSubstitution(string familyName, int fontWeight, FontStyle fontStyle, out int proximity, out PDFFontResource bestMatch)
            {
                proximity = int.MaxValue;

                if (this.OwnerDocument.SharedResources.Count > 0)
                {
                    _lookupBuffer.Clear();

                    foreach (var rsrc in this.OwnerDocument.SharedResources)
                    {
                        if (rsrc is PDFFontResource fnt)
                        {
                            if (fnt.Definition.Family == familyName)
                            {
                                if (fontStyle == Drawing.FontStyle.Italic && fnt.Definition.Italic)
                                    _lookupBuffer.Add(fnt);
                                else if (fontStyle == Drawing.FontStyle.Regular && !fnt.Definition.Italic)
                                    _lookupBuffer.Add(fnt);
                                else
                                {
                                    //Styles don't match so don't add.
                                }
                            }
                        }
                    }


                    if (_lookupBuffer.Count > 0)
                    {
                        //We have matching font resources.
                        //so find the closest match

                        bestMatch = null;
                        

                        foreach (var fnt in _lookupBuffer)
                        {
                            int newProx = 0;
                            if (fontStyle == Drawing.FontStyle.Italic)
                                newProx = fnt.Definition.Italic ? 0 : 1000; //Prefer a style rather than a weight.
                            else
                                newProx = fnt.Definition.Italic ? 1000 : 0; //reverse check we don't really want to use italic unless absolutely nescessary

                            newProx += Math.Abs(fontWeight - fnt.Definition.Weight);

                            //if we don't have a closest match
                            //or this resource more closely matches
                            if (bestMatch == null || newProx < proximity)
                            {
                                proximity = newProx;
                                bestMatch = fnt;
                            }

                        }

                        return null != bestMatch;
                        

                    }
                }

                bestMatch = null;
                return false;
            }

            #endregion

            #region protected PDFFontResource GetCourier(string failed, int weight, FontStyle style)

            protected PDFFontResource GetCourierSubstitution(string failed, int weight, FontStyle style)
            {

                string defaultFont = "Courier";
                int defWeight = FontWeights.Regular;
                Drawing.FontStyle defStyle = Drawing.FontStyle.Regular;
                string rsrcName = defaultFont;

                if (weight >= FontWeights.SemiBold)
                {
                    rsrcName += ", Bold";
                    defWeight = FontWeights.Bold;
                    if (style != Drawing.FontStyle.Regular)
                    {
                        defStyle = Drawing.FontStyle.Italic;
                        rsrcName += " Italic";
                    }
                }
                else if (style != Drawing.FontStyle.Regular)
                {
                    defStyle = Drawing.FontStyle.Italic;
                    rsrcName += ", Italic";
                }

                var mono = this.OwnerDocument.SharedResources.GetResource(PDFResource.FontDefnResourceType, rsrcName) as PDFFontResource;
                if (null == mono)
                {
                    var monoDefn = PDFFontFactory.GetFontDefinition(defaultFont, defStyle, defWeight, true);
                    var fullname = PDFFont.GetFullName(failed, weight, style);
                    mono = this.OwnerDocument.RegisterFontResource(fullname, this.OwnerDocument, monoDefn) ;
                    mono.RegisterSubstitution(failed, weight, style, isSubstitution: true);
                }

                return mono;
            }

            #endregion


            /// <summary>
            /// Registers a new PDFFontResource into the document from the font definition.
            /// </summary>
            /// <param name="font">The font that was matched</param>
            /// <param name="matchedName">The family name that was matched against</param>
            /// <param name="definition">The definition that was used</param>
            /// <returns>The created font resource</returns>
            protected PDFFontResource RegisterNewDocumentFontResource(string matchedName, int weight, FontStyle style, PDFFontDefinition definition, bool isSubstitution)
            {
                var fullname = PDFFont.GetFullName(matchedName, weight, style);
                var rsrc = this.OwnerDocument.RegisterFontResource(fullname, this.OwnerDocument, definition);

                this.RegisterFontMatch(rsrc, matchedName, weight, style, isSubstitution);

                return rsrc;
            }

            protected void RegisterFontMatch(PDFFontResource resource, string matchedName, int fontWeight, FontStyle style, bool isSubstitution)
            {
                resource.RegisterSubstitution(matchedName, fontWeight, style, isSubstitution);
            }
        }
    }
}
