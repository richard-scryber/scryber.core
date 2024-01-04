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
using Scryber.Logging;

namespace Scryber.Generation
{
    /// <summary>
    /// Defines the key setings to use by parsers in the Scryber 
    /// library when generating content from files or streams
    /// </summary>
    public class ParserSettings
    {
        //
        // attribute names for parsing the processing instructions
        //

        public const string ParserConformanceMode = "parser-mode";
        public const string ParserShouldLog = "parser-log";
        public const string ParserCulture = "parser-culture";

        public const string AppendLogToDocument = "append-log";
        public const string ComponentLogLevel = "log-level";
        public const string ComponentController = "controller";


        #region ivars

        private Type _templateinstanceType;
        private Type _tempateGenType;
        private Type _textLiteralType;
        private Type _whitespaceType;
        private TraceLog _log;
        private bool _logParserOutput;
        private ParserConformanceMode _conformance;
        private ParserLoadType _loadtype;
        private PDFReferenceResolver _resolver;
        private PerformanceMonitor _perfmon;
        private bool _appendLog;
        private Type _controllerType;
        private object _controllerInstance;
        private System.Globalization.CultureInfo _specificCulture;
        private ParserReferenceMissingAction _missingRefAction;

        #endregion

        /// <summary>
        /// Gets the type to be used for instances of a template
        /// </summary>
        public Type TemplateInstanceType
        {
            get { return _templateinstanceType; }
        }

        /// <summary>
        /// Gets the type to be used for template references
        /// </summary>
        public Type TempateGeneratorType
        {
            get { return _tempateGenType; }
        }

        /// <summary>
        /// Gets the type to be used for any text literals
        /// </summary>
        public Type TextLiteralType
        {
            get { return _textLiteralType; }
        }

        public Type WhitespaceType
        {
            get { return _whitespaceType; }
        }

        /// <summary>
        /// Gets or sets the conformance mode for the parser
        /// </summary>
        public ParserConformanceMode ConformanceMode
        {
            get { return _conformance; }
            set { _conformance = value; }
        }

        /// <summary>
        /// Gets the reference resolver
        /// </summary>
        public PDFReferenceResolver Resolver
        {
            get { return _resolver; }
        }

        /// <summary>
        /// Gets the load type for this parser
        /// </summary>
        public ParserLoadType LoadType
        {
            get { return _loadtype; }
        }

        /// <summary>
        /// Gets or sets the current log
        /// </summary>
        public TraceLog TraceLog
        {
            get { return _log; }
            set { _log = value; }
        }

        /// <summary>
        /// Gets or sets the Performance monitor for the document
        /// </summary>
        public PerformanceMonitor PerformanceMonitor
        {
            get { return _perfmon; }
            set { _perfmon = value; }
        }

        /// <summary>
        /// Gets or sets the flag that says the parser will append to the current log
        /// </summary>
        public bool LogParserOutput
        {
            get { return _logParserOutput; }
            set { _logParserOutput = value; }
        }

        /// <summary>
        /// Gets or sets the flag that means the log output will be appended to the final document
        /// </summary>
        public bool AppendLog
        {
            get { return _appendLog; }
            set { _appendLog = value; }
        }

        /// <summary>
        /// Gets or sets the controller object for the parsed template
        /// </summary>
        public Type ControllerType
        {
            get { return _controllerType; }
            set
            {
                _controllerType = value;
                _controllerInstance = null;
            }
        }

        public object Controller
        {
            get { return this._controllerInstance; }
            set
            {
                this._controllerInstance = value;
                if (null == _controllerInstance)
                    this._controllerType = null;
                else
                    this._controllerType = this._controllerInstance.GetType();
            }
        }

        public System.Globalization.CultureInfo SpecificCulture
        {
            get { return _specificCulture; }
            set { _specificCulture = value; }
        }

        public ParserReferenceMissingAction MissingReferenceAction
        {
            get { return _missingRefAction; }
            set { _missingRefAction = value; }
        }

        public bool HasSpecificCulture
        {
            get { return _specificCulture != null; }
        }

        public ParserSettings(Type literaltype, Type whitespaceType, Type templategenerator, Type templateinstance, 
                                PDFReferenceResolver resolver, ParserConformanceMode conformance, ParserLoadType loadtype,
                                TraceLog log, PerformanceMonitor perfmon, object controllerInstance)
        {
            this._textLiteralType = literaltype;
            this._whitespaceType = whitespaceType;
            this._tempateGenType = templategenerator;
            this._templateinstanceType = templateinstance;
            this._resolver = resolver;
            this._conformance = conformance;
            this._loadtype = loadtype;
            this._log = log;
            this._perfmon = perfmon;
            this._controllerInstance = controllerInstance;
            this._controllerType = (null == controllerInstance) ? null : controllerInstance.GetType();

            //Get the default culture from the config - can be overridden in the processing instructions, or at generation time in code
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            var parserOptions = config.ParsingOptions;
            this.MissingReferenceAction = parserOptions.MissingReferenceAction;
            this.SpecificCulture = parserOptions.GetDefaultCulture();
        }

        /// <summary>
        /// Creates a full clone of these settings
        /// </summary>
        /// <returns></returns>
        public ParserSettings Clone()
        {
            ParserSettings clone = this.MemberwiseClone() as ParserSettings;
            return clone;
        }

        /// <summary>
        /// Based on this settings instance, reads any processing instructions from the value 
        /// and updates this instances values
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void ReadProcessingInstructions(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                //Conformance Mode

                string mode = GetProcessingValue(ParserConformanceMode, value);
                if (!string.IsNullOrEmpty(mode))
                {
                    if (string.Equals("strict", mode, StringComparison.OrdinalIgnoreCase))
                        this.ConformanceMode = Scryber.ParserConformanceMode.Strict;
                    else if (string.Equals("lax", mode, StringComparison.OrdinalIgnoreCase))
                        this.ConformanceMode = Scryber.ParserConformanceMode.Lax;
                    else
                        throw new PDFException("The parser conformance mode could not be understood :" + mode +
                                                     ", allowed values are 'strict' or 'lax' (case insensitive)");
                    
                }

                //Parser Logging

                string log = GetProcessingValue(ParserShouldLog, value);
                bool dolog;
                if (!string.IsNullOrEmpty(log) && bool.TryParse(log, out dolog))
                {
                    this.LogParserOutput = dolog;
                }

                //Append Log

                string append = GetProcessingValue(AppendLogToDocument, value);
                bool doappend = false;

                if (!string.IsNullOrEmpty(append) && bool.TryParse(append, out doappend))
                {
                    this.AppendLog = doappend;
                }

                //Parser Culture

                string culture = GetProcessingValue(ParserCulture, value);
                if(!string.IsNullOrEmpty(culture))
                {
                    System.Globalization.CultureInfo found = System.Globalization.CultureInfo.GetCultureInfo(culture);
                    this.SpecificCulture = found;
                }

                //Log Level

                string level = GetProcessingValue(ComponentLogLevel, value);
                TraceRecordLevel loglevel = this.TraceLog.RecordLevel;

                if (!string.IsNullOrEmpty(level))
                {
                    TraceRecordLevel parsed;
                    if (Enum.TryParse<TraceRecordLevel>(level, true, out parsed))
                    {
                        this.TraceLog.SetRecordLevel(parsed);
                        this.PerformanceMonitor.RecordMeasurements = (parsed <= TraceRecordLevel.Verbose);
                        loglevel = parsed;
                    }
                    else
                        this.TraceLog.Add(TraceLevel.Error,"XMLParser", "The log-level processing instruction was a value that is not defined '" + level + "'");
                }

                //Component controller

                string controllername = GetProcessingValue(ComponentController, value);
                if (!string.IsNullOrEmpty(controllername))
                {
                    Type found = Type.GetType(controllername, true);
                    this.ControllerType = found;
                }


            }

        }


        #region private string GetProcessingValue(string attributename, string full)

        /// <summary>
        /// Extracts the processing instruction value with the specified name or string.Empty if it is not defined.
        /// </summary>
        /// <param name="attributename"></param>
        /// <param name="full"></param>
        /// <returns></returns>
        private string GetProcessingValue(string attributename, string full)
        {
            if (string.IsNullOrEmpty(attributename))
                return string.Empty;

            int index = full.IndexOf(attributename + "=");

            if (index == 0 || (index > 0 && full[index - 1] == ' '))//make sure we are at a boundary
            {
                index += attributename.Length + 1;

                char separator = full[index];
                int start, end;

                if (separator == '\'' || separator == '"')
                {
                    start = index + 1;
                    end = full.IndexOf(separator, start);
                }
                else
                {
                    start = index;
                    end = full.IndexOf(' ', start);
                    if (end < start)
                        end = full.Length;
                }

                return full.Substring(start, end - start);
            }
            return string.Empty;
        }

        #endregion

    }

}
