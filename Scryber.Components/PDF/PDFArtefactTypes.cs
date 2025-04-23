/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.PDF
{
    /// <summary>
    /// Defines known types of PDF artefacts and their keys that should be used in an atrefact registration set.
    /// Annotation, Outlines, Names.
    /// </summary>
    public static class PDFArtefactTypes
    {
        public const string Annotations = "Annots";
        public const string Outlines = "Outlines";
        public const string Names = "Names";
        public const string Destinations = "Dests";
        public const string PageLayout = "PageLayout";
        public const string PageMode = "PageMode";
        public const string ViewerPrefs = "ViewerPreferences";
        public const string PageLabels = "PageLabels";
        public const string AcrobatForms = "AcroForm";
        public const string EmbeddedFiles = "EmbeddedFiles";
    }
}
