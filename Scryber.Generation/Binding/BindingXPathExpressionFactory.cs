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
using Scryber.Generation;

namespace Scryber.Binding
{
    public class BindingXPathExpressionFactory : IPDFBindingExpressionFactory
    {
        public string BindingKey { get { return "xpath"; } }

        public DocumentGenerationStage BindingStage
        {
            get { return DocumentGenerationStage.Bound; }
        }

        public InitializedEventHandler GetInitBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty)
        {
            throw new NotSupportedException("XPath Binding is not supported on any other document lifecycle stage than the databinding");
        }

        public LoadedEventHandler GetLoadBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty)
        {
            throw new NotSupportedException("XPath Binding is not supported on any other document lifecycle stage than the databinding");
        }

        public DataBindEventHandler GetDataBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty)
        {
            ValueConverter valConv;

            if(ParserDefintionFactory.IsSimpleObjectType(forProperty.PropertyType, out valConv))
            {
                BindingXPathExpression expr = BindingXPathExpression.Create(expressionvalue, valConv, forProperty);
                return new DataBindEventHandler(expr.BindComponent);
            }
            else if(ParserDefintionFactory.IsCustomParsableObjectType(forProperty.PropertyType, out valConv))
            {
                BindingXPathExpression expr = BindingXPathExpression.Create(expressionvalue, valConv, forProperty);
                return new DataBindEventHandler(expr.BindComponent);
            }
            else if(forProperty.PropertyType == typeof(Object))
            {
                valConv = null;

                var expr = BindingXPathExpression.Create(expressionvalue, valConv, forProperty);
                return new DataBindEventHandler(expr.BindComponent);
            }
            else
                throw new PDFParserException(string.Format(Errors.ParserAttributeMustBeSimpleOrCustomParsableType, forProperty.Name, forProperty.PropertyType));
        }

    }

}
