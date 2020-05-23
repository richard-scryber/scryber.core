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
using Scryber.Native;
using Scryber.Resources;

namespace Scryber.Drawing
{
    public class PDFExternalGraphicsState
    {

        #region private class StateInfo

        /// <summary>
        /// Encapsulates teh current external graphics state info.
        /// </summary>
        private class StateInfo : IEquatable<StateInfo>
        {
            public double Stroke;
            public double Fill;
            public bool Registered;
            public PDFExtGSState ExtState;

            public StateInfo(double fill, double stroke)
            {
                this.Fill = fill;
                this.Stroke = stroke;
                this.Registered = false;
            }

            public StateInfo(StateInfo previous)
                : this(previous.Fill, previous.Stroke)
            {
            }

            public override bool Equals(object obj)
            {
                return this.Equals((StateInfo)obj);
            }

            public bool Equals(StateInfo info)
            {
                if (null == info)
                    return false;
                else
                    return this.Fill == info.Fill && this.Stroke == info.Stroke;
            }

            public override int GetHashCode()
            {
                return (int)(this.Fill * 1000000.0) + (int)(this.Stroke * 1000.0);
            }
        }

        #endregion

        private Stack<StateInfo> _states;
        private StateInfo _identity;
        private IPDFResourceContainer _container;
        private PDFWriter _writer;

        private StateInfo CurrentState
        {
            get
            {
                if (this._states.Count == 0)
                    return null;
                else
                    return _states.Peek();
            }

        }

        public PDFExternalGraphicsState(IPDFResourceContainer container, PDFWriter writer)
        {
            _states = new Stack<StateInfo>();
            _identity = new StateInfo(1.0, 1.0);
            _container = container;
            _writer = writer;
        }


        public void SaveState()
        {
            StateInfo tocopy = this.CurrentState;
            if (null == tocopy)
                tocopy = this._identity;

            _states.Push(new StateInfo(tocopy));
        }

        public void RestoreState()
        {
            if (this._states.Count > 0)
                this._states.Pop();
        }

        public void EnsureState()
        {

        }

        public void SetStrokeOpacity(PDFReal opacity)
        {
            
            StateInfo info = this.CurrentState;
            if(null == info)
                throw new NullReferenceException("No current state to modify");

            if (info.Stroke != opacity.Value)
            {
                if (info.Registered == false)
                {
                    PDFExtGSState state = new PDFExtGSState();
                    _container.Register(state);
                    state.Name.WriteData(_writer);
                    _writer.WriteOpCodeS(PDFOpCode.GraphApplyState);
                    info.ExtState = state;
                    info.Registered = true;
                }
                info.ExtState.States[Resources.PDFExtGSState.ColorStrokeOpacity] = opacity;
                info.Stroke = opacity.Value;
            }
        }

        public void SetFillOpacity(PDFReal opacity)
        {
            StateInfo info = this.CurrentState;
            if (null == info)
                throw new NullReferenceException("No current state to modify");

            if (info.Fill != opacity.Value)
            {
                if (info.Registered == false)
                {
                    PDFExtGSState state = new PDFExtGSState();
                    _container.Register(state);
                    state.Name.WriteData(_writer);
                    _writer.WriteOpCodeS(PDFOpCode.GraphApplyState);
                    info.ExtState = state;
                    info.Registered = true;
                }
                info.ExtState.States[Resources.PDFExtGSState.ColorFillOpactity] = opacity;
                info.Fill = opacity.Value;
            }
        }

    }
}
