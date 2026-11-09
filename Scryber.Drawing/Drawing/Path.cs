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

namespace Scryber.Drawing
{

    public class Path
    {
        private List<PathData> _data;


        public List<PathData> Operations
        {
            get { return _data; }
        }

        public Path()
        {
            this._data = new List<PathData>();
        }

        /// <summary>
        /// Adds a new path data operation to this path
        /// </summary>
        /// <param name="data"></param>
        public void Add(PathData data)
        {
            _data.Add(data);
        }

        /// <summary>
        /// Gets the number of path data ops in this path
        /// </summary>
        public int Count
        {
            get { return _data.Count; }
        }

        /// <summary>
        /// Gets the total number of path data ops (including sub paths)
        /// </summary>
        public int TotalCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < this.Count; i++)
                {
                    PathData data = _data[i];
                    if (data.Type == PathDataType.SubPath)
                    {
                        count = ((PathSubPathData)data).InnerPath.TotalCount;
                    }
                    else
                        count++;
                }
                return count;
            }
        }

        /// <summary>
        /// Removes the last path data from this Path
        /// </summary>
        public void Remove()
        {
            this._data.RemoveAt(this._data.Count - 1);
        }

        public virtual Path Clone()
        {
            Path clone = (Path) this.MemberwiseClone();
            clone._data = new List<PathData>(this.Count);
            
            for (int i = 0; i < this.Count; i++)
            {
                PathData data = this._data[i];
                clone._data.Add(data.Clone());
            }
            return clone;
        }

        public void FillAllPoints(List<Point> points)
        {
            foreach (PathData data in this._data)
            {
                data.FillAllPoints(points);
            }
        }
    }

}
