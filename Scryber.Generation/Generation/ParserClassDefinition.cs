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

namespace Scryber.Generation
{
    public class ParserClassDefinition
    {
        private string _remoteName;
        private bool _remoteSupported;
        private Type _classtype;
        private Version _min = PDFRequiredFrameworkAttribute.Empty;
        private Version _max = PDFRequiredFrameworkAttribute.Empty;
        private bool _isMinSupported = true;
        private bool _isMaxSupported = true;
        private ParserPropertyDefinition _default;
        private ParserPropertyDefinitionCollection _attributes = new ParserPropertyDefinitionCollection();
        private ParserPropertyDefinitionCollection _elements = new ParserPropertyDefinitionCollection();
        private ParserEventDefinitionCollection _events = new ParserEventDefinitionCollection();

        /// <summary>
        /// Gets the Type of this class defintion
        /// </summary>
        public Type ClassType
        {
            get { return _classtype; }
        }

        public bool IsMinFrameworkSupported
        {
            get { return _isMinSupported; }
        }

        public Version MinRequiredFramework
        {
            get { return _min; }
            set
            {
                _min = value;

                if (value == PDFRequiredFrameworkAttribute.Empty)
                    _isMinSupported = true;
                else
                    _isMinSupported = (value <= Scryber.Utilities.FrameworkHelper.CurrentVersion);
                //value = 0.8.5, current = 0.8.4, value <= current = false
                //value = 0.8.3, current = 0.8.4, value <= current = true
            }
        }

        public bool IsMaxFrameworkSupported
        {
            get { return _isMaxSupported; }
        }

        public Version MaxSupportedFramework
        {
            get { return _max; }
            set
            {
                _max = value;
                if (value == PDFRequiredFrameworkAttribute.Empty)
                    _isMaxSupported = true;
                else
                    _isMaxSupported = (value >= Scryber.Utilities.FrameworkHelper.CurrentVersion);
                //value = 0.8.5, current = 0.8.4, value >= current = true
                //value = 0.8.3, current = 0.8.4, value >= current = false
            }
        }

        public ParserEventDefinitionCollection Events
        {
            get { return _events; }
        }

        /// <summary>
        /// Gets the named attributes that can be parsed in this class
        /// </summary>
        public ParserPropertyDefinitionCollection Attributes
        {
            get { return _attributes; }
        }

        /// <summary>
        /// Gets the named elements that canbe parsed in this class definition
        /// </summary>
        public ParserPropertyDefinitionCollection Elements
        {
            get { return _elements; }
        }

        /// <summary>
        /// Gets or sets the 'default' property definition
        /// </summary>
        public ParserPropertyDefinition DefaultElement
        {
            get { return _default; }
            set { _default = value; }
        }

        /// <summary>
        /// Returns true if this class supports parsing from a remote file
        /// </summary>
        public bool IsRemoteParsable
        {
            get { return this._remoteSupported; }
        }

        /// <summary>
        /// Gets the name of the element that is used when identifying as a remote reference
        /// </summary>
        public string RemoteElementName
        {
            get { return _remoteName; }
        }

        public ParserClassDefinition(Type classtype)
        {
            this._classtype = classtype;
        }


        public override string ToString()
        {
            return "Class '" + this.ClassType.Name + "'";
        }

        internal void SetRemoteParsable(bool isremote, string remoteName)
        {
            this._remoteName = remoteName;
            this._remoteSupported = isremote;

            if (isremote && string.IsNullOrEmpty(remoteName))
                throw new ArgumentNullException("remoteName", "A remote parsable component cannot have an empty remote element name");
        }

    }
}
