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
using System.Linq;
using System.Text;
using System.Configuration;

namespace Scryber.Configuration
{
    /// <summary>
    /// The configuration section for the PDF rendering options. Doefines the default values for output rendering.
    /// </summary>
    [Obsolete("Use the IScryberCOnfigurationSerive, from the ServiceProvider", true)]
    public class RenderOptionsConfigurationSection : ConfigurationSection
    {

        #region public OutputCompressionType CompressionType {get;set;}

        private const string CompressionAttrKey = "compression-type";
        private const OutputCompressionType DefaultCompression = OutputCompressionType.FlateDecode;

        /// <summary>
        /// Defines the standard compression option for all PDF rendered documents
        /// </summary>
        [ConfigurationProperty(CompressionAttrKey, IsRequired=false, IsKey=false, DefaultValue=DefaultCompression)]
        public OutputCompressionType CompressionType
        {
            get
            {
                object value = this[CompressionAttrKey];
                if (null == value)
                    value = DefaultCompression;

                return (OutputCompressionType)value;
            }
            set
            {
                this[CompressionAttrKey] = value;
            }
        }

        #endregion


        #region public string PDFVersion {get;set;}

        private const string VersionAttrKey = "pdf-version";
        private const string DefaultPDFVersion = "1.5";

        /// <summary>
        /// Defines the standard version for all PDF rendered documents
        /// </summary>
        [ConfigurationProperty(VersionAttrKey, IsRequired = false, IsKey = false, DefaultValue = DefaultPDFVersion)]
        public string PDFVersion
        {
            get
            {
                object value = this[VersionAttrKey];
                if (null == value || string.IsNullOrEmpty(value.ToString()))
                    value = DefaultPDFVersion;

                return value.ToString();
            }
            set
            {
                this[VersionAttrKey] = value;
            }
        }

        #endregion

        #region public Version GetPDFVersion()

        /// <summary>
        /// Returns a parsed Version instance from the PDFVersion value
        /// </summary>
        /// <returns></returns>
        public Version GetPDFVersion()
        {
            string vers = this.PDFVersion;
            Version parsed;
            if (Version.TryParse(vers, out parsed))
                return parsed;
            else
                throw new ArgumentException("Could not parse the pdf version number for output : " + vers, "this.PDFVersion");
        }

        #endregion


        #region  public string OutputCompliance {get;set;}

        public const string ComplianceAttrKey = "output-compliance";
        public const string DefaultCompliance = "None";

        /// <summary>
        /// Gets the rendering output Compliance mode - currently only None is supported, but will be extended to PDF/A etc.
        /// </summary>
        [ConfigurationProperty(ComplianceAttrKey, IsRequired = false, IsKey = false, DefaultValue = DefaultCompliance)]
        public string OutputCompliance
        {
            get
            {
                object value = this[ComplianceAttrKey];
                if (null == value || string.IsNullOrEmpty(value.ToString()))
                    value = DefaultCompliance;
                return value.ToString();
            }
            set
            {
                this[ComplianceAttrKey] = value;
            }
        }

        #endregion

        #region public OutputCompliance GetOutputCompliance()

        /// <summary>
        /// Gets the parsed output Compliance value of this configuration section. 
        /// If the configured value is not one of the known types, then Other is returned.
        /// </summary>
        /// <returns></returns>
        public OutputCompliance GetOutputCompliance()
        {
            OutputCompliance result;
            string value = this.OutputCompliance;
            if (Enum.TryParse<OutputCompliance>(value, out result))
                return result;
            else
                return Scryber.OutputCompliance.Other;
        }

        #endregion

        #region  public OutputStringType StringOutput {get;set;}

        public const string StringAttrKey = "string-output";
        public const OutputStringType DefaultStringOutput = OutputStringType.Hex;

        /// <summary>
        /// Gets the string output type of the PDFWriter. Options are Text - where the characters will be written as characters, or Hex where the characters will be Hexadecimal encoded
        /// </summary>
        [ConfigurationProperty(StringAttrKey, IsRequired = false, IsKey = false, DefaultValue = DefaultStringOutput)]
        public OutputStringType StringOutput
        {
            get
            {
                object value = this[StringAttrKey];
                if (null == value)
                    value = DefaultStringOutput;
                return (OutputStringType)value;
            }
            set
            {
                this[ComplianceAttrKey] = value;
            }
        }

        #endregion

        #region public ComponentNameOutput NameOutput {get;set;}

        public const string NameOutputAttrKey = "component-name-output";
        public const ComponentNameOutput DefaultNameOutput = ComponentNameOutput.ExplicitOnly;

        /// <summary>
        /// Gets the name dictionary component output type
        /// </summary>
        [ConfigurationProperty(NameOutputAttrKey, IsRequired=false, IsKey=false, DefaultValue = DefaultNameOutput)]
        public ComponentNameOutput NameOutput
        {
            get
            {
                object value = this[NameOutputAttrKey];
                if (null == value)
                    value = DefaultNameOutput;
                return (ComponentNameOutput)value;
            }
            set
            {
                this[NameOutputAttrKey] = value;
            }
        }

        #endregion

    }
}
