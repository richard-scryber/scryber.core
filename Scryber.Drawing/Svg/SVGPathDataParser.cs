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
using System.Text.RegularExpressions;
using Scryber.Drawing;
using Scryber.Logging;

namespace Scryber.Svg
{
    public class SVGPathDataParser
    {
        Regex operators = new Regex(@"(?=[MZLHVCSQTAmzlhvcsqta])");
        Regex arguments = new Regex(@"[\s,]|(?=-)");
        private bool _strict;
        private TraceLog _log;


        /// <summary>
        /// Creates a new instance of the SVG Path parser, that reads path data and converts to a path data commands
        /// </summary>
        /// <param name="strict">If true then exceptions will be thrown if commands cannot be parsed or are not supported.
        /// If false then errors will be written to the log instead, and execution continues</param>
        /// <param name="log">The log (if any) to write to</param>
        public SVGPathDataParser(bool strict, TraceLog log)
        {
            this._strict = strict;
            this._log = log == null ? new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Off) : log;
        }

        public void ParseSVG(GraphicsPath path, string data)
        {
            string[] tokens = operators.Split(data);

            foreach (string match in tokens)
            {
                if (!string.IsNullOrEmpty(match))
                    ParseSVGCommand(path, match);
            }
        }

        /// <summary>
        /// Parses a single svg path command - first character is the command character, and the numeric arguments follow after.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="command"></param>
        public void ParseSVGCommand(GraphicsPath path, string command)
        {
            char cmd = command[0];
            string[] args;

            if (command.Length > 0)
                args = arguments.Split(command.Substring(1).Trim());
            else
                args = null;

            ParseSVGCommand(path, cmd, args);
        }


        private void ParseSVGCommand(GraphicsPath path, char cmd, string[] args)
        {
            switch (cmd)
            {
                case('M'):
                    ParseSVGMoveCommand(path, cmd, true, args);
                    break;
                case('m'):
                    ParseSVGMoveCommand(path, cmd, false, args);
                    break;
                case('Z'):
                    ParseSVGCloseCommand(path, args);
                    break;
                case('z'):
                    ParseSVGCloseCommand(path, args);
                    break;
                case('L'):
                    ParseSVGLineCommand(path, cmd, true, args);
                    break;
                case('l'):
                    ParseSVGLineCommand(path, cmd, false, args);
                    break;
                case('H'):
                    ParseSVGHorizontalCommand(path, cmd, true, args);
                    break;
                case('h'):
                    ParseSVGHorizontalCommand(path, cmd, false, args);
                    break;
                case('V'):
                    ParseSVGVerticalCommand(path, cmd, true, args);
                    break;
                case('v'):
                    ParseSVGVerticalCommand(path, cmd, false, args);
                    break;
                case('C'):
                    ParseSVGCubicCommand(path, cmd, true, args);
                    break;
                case('c'):
                    ParseSVGCubicCommand(path, cmd, false, args);
                    break;
                case('S'):
                    ParseSVGSmoothCubicCommand(path, cmd, true, args);
                    break;
                case('s'):
                    ParseSVGSmoothCubicCommand(path, cmd, false, args);
                    break;
                case('Q'):
                    ParseSVGQuadraticCommand(path, cmd, true, args);
                    break;
                case('q'):
                    ParseSVGQuadraticCommand(path, cmd, false, args);
                    break;
                case('T'):
                    ParseSVGSmoothQuadraticCommand(path, cmd, true, args);
                    break;
                case('t'):
                    ParseSVGSmoothQuadraticCommand(path, cmd, false, args);
                    break;
                case('A'):
                    ParseSVGArcCommand(path, cmd, true, args);
                    break;
                case('a'):
                    ParseSVGArcCommand(path, cmd, false, args);
                    break;
                default:
                    break;
            }
        }

        private void ParseSVGMoveCommand(GraphicsPath path, char cmd, bool absolute, string[] args)
        {
            int index = 0;
            Unit x, y;

            while (index < args.Length)
            {
                //must be at least an x and y, but can optionally be more x and y's
                if (index == 0 || !string.IsNullOrEmpty(args[index]))
                {
                    if (AssertParseUnit(args, ref index, cmd, out x) && AssertParseUnit(args, ref index, cmd, out y))
                    {
                        if (absolute)
                            path.MoveTo(new Point(x, y));
                        else
                            path.MoveBy(new Point(x, y));
                    }
                }
                else if (string.IsNullOrEmpty(args[index]))
                    index++;
            }
        }

        private void ParseSVGLineCommand(GraphicsPath path, char cmd, bool absolute, string[] args)
        {
            Unit x,y;
            int index = 0;
            while (index < args.Length)
            {
                //must be at least an x and y, but can optionally be more x and y's
                if (index == 0 || !string.IsNullOrEmpty(args[index]))
                {
                    if (!AssertParseUnit(args, ref index, cmd, out x))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out y))
                        return;

                    if (absolute)
                        path.LineTo(new Point(x, y));
                    else
                        path.LineFor(new Point(x, y));
                }
                else if (string.IsNullOrEmpty(args[index]))
                    index++;
            }
        }

        private void ParseSVGVerticalCommand(GraphicsPath path, char cmd, bool absolute, string[] args)
        {
            Unit v;
            int index = 0;
            while (index < args.Length)
            {
                //must be at least one, but can optionally be more
                if (index == 0 || !string.IsNullOrEmpty(args[index]))
                {
                    if (AssertParseUnit(args, ref index, cmd, out v))
                    {
                        if (absolute)
                            path.VerticalLineTo(v);
                        else
                            path.VerticalLineFor(v);
                    }
                }
                else if (string.IsNullOrEmpty(args[index]))
                    index++;
            }
        }

        private void ParseSVGHorizontalCommand(GraphicsPath path, char cmd, bool absolute, string[] args)
        {
            Unit h;
            int index = 0;

            
            while (index < args.Length)
            {
                //must be at least one, but can optionally be more
                if (index == 0 || !string.IsNullOrEmpty(args[index]))
                {
                    if (AssertParseUnit(args, ref index, cmd, out h))
                    {
                        if (absolute)
                            path.HorizontalLineTo(h);
                        else
                            path.HorizontalLineFor(h);
                    }
                }
                else if (string.IsNullOrEmpty(args[index]))
                    index++;
            }
        }

        private void ParseSVGCloseCommand(GraphicsPath path, string[] args)
        {
            path.ClosePath(false);
        }

        private void ParseSVGCubicCommand(GraphicsPath path, char cmd, bool absolute, string[] args)
        {
            Unit startHandleX, startHandleY, endHandleX, endHandleY, endPtX, endPtY;
            int index = 0;

            while (index < args.Length)
            {
                if (index == 0 || !string.IsNullOrEmpty(args[index]))
                {

                    if (!AssertParseUnit(args, ref index, cmd, out startHandleX))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out startHandleY))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out endHandleX))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out endHandleY))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out endPtX))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out endPtY))
                        return;

                    if (absolute)
                        path.CubicCurveTo(new Point(endPtX, endPtY), new Point(startHandleX, startHandleY), new Point(endHandleX, endHandleY));
                    else
                        path.CubicCurveFor(new Point(endPtX, endPtY), new Point(startHandleX, startHandleY), new Point(endHandleX, endHandleY));
                }
                else if (string.IsNullOrEmpty(args[index]))
                    index++;
            }
        }

        private void ParseSVGQuadraticCommand(GraphicsPath path, char cmd, bool absolute, string[] args)
        {
            Unit handleX, handleY, endPtX, endPtY;
            int index = 0;

            while (index < args.Length)
            {
                if (index == 0 || !string.IsNullOrEmpty(args[index]))
                {

                    if (!AssertParseUnit(args, ref index, cmd, out handleX))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out handleY))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out endPtX))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out endPtY))
                        return;

                    if (absolute)
                        path.QuadraticCurveTo(new Point(endPtX, endPtY), new Point(handleX, handleY));
                    else
                        path.QuadraticCurveFor(new Point(endPtX, endPtY), new Point(handleX, handleY));
                }
                else if (string.IsNullOrEmpty(args[index]))
                    index++;
            }
        }

        #region private void ParseSVGSmoothCubicCommand(PDFGraphicsPath path, char cmd, bool absolute, string[] args)

        /// <summary>
        /// Parses one or more smooth Cubic path commands in the format endHandleX,endHandeY endX,endY
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cmd"></param>
        /// <param name="absolute"></param>
        /// <param name="args"></param>
        /// <remarks>The curve is a cubic bezier path that starts from the current cursor position.
        /// The start handle is inferred as a reflection of the previous handle point</remarks>
        private void ParseSVGSmoothCubicCommand(GraphicsPath path, char cmd, bool absolute, string[] args)
        {
            Unit endHandleX, endHandleY, endPtX, endPtY;
            int index = 0;

            while (index < args.Length)
            {
                if (index == 0 || !string.IsNullOrEmpty(args[index]))
                {
                    if (!AssertParseUnit(args, ref index, cmd, out endHandleX))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out endHandleY))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out endPtX))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out endPtY))
                        return;

                    if (absolute)
                        path.SmoothCubicCurveTo(new Point(endPtX, endPtY), new Point(endHandleX, endHandleY));
                    else
                        path.SmoothCubicCurveFor(new Point(endPtX, endPtY), new Point(endHandleX, endHandleY));
                }
                else if (string.IsNullOrEmpty(args[index]))
                    index++;
            }
        }

        #endregion

        #region private void ParseSVGSmoothQuadraticCommand(PDFGraphicsPath path, char cmd, bool absolute, string[] args)

        /// <summary>
        /// Parses one or more smooth quadratic path commands in the format endx,endy
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cmd"></param>
        /// <param name="absolute"></param>
        /// <param name="args"></param>
        /// <remarks>The handle is inferred as a reflection of the previous handle</remarks>
        private void ParseSVGSmoothQuadraticCommand(GraphicsPath path, char cmd, bool absolute, string[] args)
        {
            Unit endPtX, endPtY;
            int index = 0;

            while (index < args.Length)
            {
                if (index == 0 || !string.IsNullOrEmpty(args[index]))
                {

                    if (!AssertParseUnit(args, ref index, cmd, out endPtX))
                        return;
                    if (!AssertParseUnit(args, ref index, cmd, out endPtY))
                        return;

                    if (absolute)
                        path.SmoothQuadraticCurveTo(new Point(endPtX, endPtY));
                    else
                        path.SmoothQuadraticCurveFor(new Point(endPtX, endPtY));
                }
                else if (string.IsNullOrEmpty(args[index]))
                    index++;
            }
        }

        #endregion

        #region private void ParseSVGArcCommand(PDFGraphicsPath path, char cmd, bool absolute, string[] args)

        /// <summary>
        /// Parses one or more arc commands in the format rx ry ang, large-flag, sweep-flag endx endy.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cmd"></param>
        /// <param name="absolute"></param>
        /// <param name="args"></param>
        /// <remarks>
        /// The arc is a segment along an ellipse that has 2 radii with the arc 
        /// extending from the current cursor position to the end point.
        /// 
        /// rx = Radius X of the constructed ellipse
        /// ry = Radius Y of the constructed ellipse
        /// ang = The rotational angle in degrees of the constructed ellipse
        /// large-flag = 1 to use the longest path around the ellipse, 0 to use the shortest path
        /// sweep-flag = 1 to sweep in a positive direction, 0 to sweep in a negative direction.
        /// endx = The X co-ordinate of the end point of the arc
        /// endy = The Y co-ordinate of the end point of the arc
        /// </remarks>
        private void ParseSVGArcCommand(GraphicsPath path, char cmd, bool absolute, string[] args)
        {
            Unit rx, ry, endx, endy;
            double ang;
            bool large, sweep;
            int index = 0;

            while (index < args.Length)
            {
                if (index == 0 || !string.IsNullOrEmpty(args[index]))
                {

                    if (!AssertParseUnit(args, ref index, cmd, out rx))
                        return;

                    if (!AssertParseUnit(args, ref index, cmd, out ry))
                        return;

                    if (!AssertParseDouble(args, ref index, cmd, out ang))
                        return;

                    if (!AssertParseBoolInt(args, ref index, cmd, out large))
                        return;

                    if (!AssertParseBoolInt(args, ref index, cmd, out sweep))
                        return;

                    if (!AssertParseUnit(args, ref index, cmd, out endx))
                        return;

                    if (!AssertParseUnit(args, ref index, cmd, out endy))
                        return;

                    if (absolute)
                        path.ArcTo(rx, ry, ang, large ? PathArcSize.Large : PathArcSize.Small, sweep ? PathArcSweep.Positive : PathArcSweep.Negative, new Point(endx, endy));
                    else
                        path.ArcFor(rx, ry, ang, large ? PathArcSize.Large : PathArcSize.Small, sweep ? PathArcSweep.Positive : PathArcSweep.Negative, new Point(endx, endy));
                }
                else if (string.IsNullOrEmpty(args[index]))
                    index++;

            }
        }

        #endregion

        //
        // helper methods
        //

        #region protected bool AssertParseBoolInt(string[] args, ref int arrayIndex, char cmd, out bool parsed)

        /// <summary>
        /// Parses the next argument in the array into a boolean value 1 = true, 0 = false, incrementing the index as part of the process.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="arrayIndex"></param>
        /// <param name="cmd"></param>
        /// <param name="parsed"></param>
        /// <returns></returns>
        protected bool AssertParseBoolInt(string[] args, ref int arrayIndex, char cmd, out bool parsed)
        {
            int value;
            while (arrayIndex < args.Length && string.IsNullOrEmpty(args[arrayIndex])) arrayIndex++;

            if (arrayIndex >= args.Length)
            {
                parsed = false;
                RaiseError("No required argument found for pdf unit in '" + cmd + "' command");
                return false;
            }
            else if (int.TryParse(args[arrayIndex], out value) == false)
            {
                RaiseError("Could not parse the string '" + args[arrayIndex] + "' into a valid unit value");
                parsed = false;
                return false;
            }
            else if (value > 1 || value < 0)
            {
                RaiseError("Could not parse the string '" + args[arrayIndex] + "' into a valid boolean flag value");
                parsed = false;
                return false;
            }
            else if (value == 1)
                parsed = true;
            else
                parsed = false;

            arrayIndex++;
            return true;
        }

        #endregion

        #region protected bool AssertParseInt(string[] args, ref int arrayIndex, char cmd, out int parsed)

        /// <summary>
        /// Parses the next argument in the array into an integer value, incrementing the index as part of the process.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="arrayIndex"></param>
        /// <param name="cmd"></param>
        /// <param name="parsed"></param>
        /// <returns></returns>
        /// <remarks>If the argument is not present or cannot be parsed then either an exception will be thrown, or an error raised on the trace log, depending on the Strict switch</remarks>
        protected bool AssertParseInt(string[] args, ref int arrayIndex, char cmd, out int parsed)
        {
            while (arrayIndex < args.Length && string.IsNullOrEmpty(args[arrayIndex])) arrayIndex++;

            if (arrayIndex >= args.Length)
            {
                parsed = 0;
                RaiseError("No required argument found for pdf unit in '" + cmd + "' command");
                return false;
            }
            else if (int.TryParse(args[arrayIndex], out parsed) == false)
            {
                RaiseError("Could not parse the string '" + args[arrayIndex] + "' into a valid unit value");
                return false;
            }
            arrayIndex++;
            return true;
        }

        #endregion

        #region protected bool AssertParseDouble(string[] args, ref int arrayIndex, char cmd, out double parsed)

        /// <summary>
        /// Parses the next argument in the array into an double value, incrementing the index as part of the process.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="arrayIndex"></param>
        /// <param name="cmd"></param>
        /// <param name="parsed"></param>
        /// <returns></returns>
        /// <remarks>If the argument is not present or cannot be parsed then either an exception will be thrown, or an error raised on the trace log, depending on the Strict switch</remarks>
        protected bool AssertParseDouble(string[] args, ref int arrayIndex, char cmd, out double parsed)
        {
            while (arrayIndex < args.Length && string.IsNullOrEmpty(args[arrayIndex])) arrayIndex++;

            if (arrayIndex >= args.Length)
            {
                parsed = 0;
                RaiseError("No required argument found for pdf unit in '" + cmd + "' command");
                return false;
            }
            else if (double.TryParse(args[arrayIndex], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out parsed) == false)
            {
                RaiseError("Could not parse the string '" + args[arrayIndex] + "' into a valid unit value");
                return false;
            }
            arrayIndex++;
            return true;
        }

        #endregion

        #region protected bool AssertParseUnit(string[] args, ref int arrayIndex, char cmd, out PDFUnit parsed)

        /// <summary>
        /// Parses the next argument in the array into a PDFUnit value, incrementing the index as part of the process.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="arrayIndex"></param>
        /// <param name="cmd"></param>
        /// <param name="parsed"></param>
        /// <returns></returns>
        /// <remarks>If the argument is not present or cannot be parsed then either an exception will be thrown, or an error raised on the trace log, depending on the Strict switch</remarks>
        protected bool AssertParseUnit(string[] args, ref int arrayIndex, char cmd, out Unit parsed)
        {
            while(arrayIndex < args.Length && string.IsNullOrEmpty(args[arrayIndex])) arrayIndex++;

            if (arrayIndex >= args.Length)
            {
                parsed = Unit.Empty;
                RaiseError("No required argument found for pdf unit in '" + cmd + "' command");
                return false;
            }
            else if (Unit.TryParse(args[arrayIndex], out parsed) == false)
            {
                RaiseError("Could not parse the string '" + args[arrayIndex] + "' into a valid unit value");
                return false;
            }
            arrayIndex++;
            return true;
        }

        #endregion

        #region protected void RaiseError(string message)

        /// <summary>
        /// Raises an error by either throwing an exception, or added an error to the event log - depends upon the Strict flag 
        /// </summary>
        /// <param name="message"></param>
        protected void RaiseError(string message)
        {
            if (this._strict)
                throw new PDFException(message);
            else
                _log.Add(TraceLevel.Warning, "PDF Path parser", message);
        }

        #endregion

    }
}
