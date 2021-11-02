using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Scryber.Data
{
    /// <summary>
    /// Static utility class that converts teh DataSet structure into an XPath DataSchema
    /// </summary>
    public static class DataSetSchemaGenerator
    {

        public static DataSchema CreateSchemaFromSet(System.Data.DataSet dataSet, DataContext context)
        {
            return DoPopulateDataSchema(dataSet, context);
        }


        #region private static PDFDataSchema DoPopulateDataSchema(DataSet dataset, PDFDataContext context)

        private static DataSchema DoPopulateDataSchema(DataSet dataset, DataContext context)
        {
            string dsName = dataset.DataSetName;
            string dsXmlName = System.Xml.XmlConvert.EncodeLocalName(dsName);
            System.Data.DataTable[] tables = GetTopLevelTables(dataset);
            List<DataItem> topItems = new List<DataItem>();

            foreach (System.Data.DataTable table in tables)
            {
                string tableName = table.TableName;
                string tableXmlName = System.Xml.XmlConvert.EncodeLocalName(tableName);
                string path = DataSchema.CombineElementNames(dsXmlName, tableXmlName);
                DataItemCollection columns = new DataItemCollection(GetTableColumnSchema(path, table));
                DataItem topTable = new DataItem(path, tableName, tableName, columns);
                topItems.Add(topTable);

                //TODO: Load child schemas
            }
            DataSchema schema = new DataSchema(dsName, new DataItemCollection(topItems));
            return schema;
        }

        #endregion

        #region private static IEnumerable<PDFDataItem> GetTableColumnSchema(string path, System.Data.DataTable table)

        /// <summary>
        /// Extracts a bunch of data items for the schema from the table columns
        /// </summary>
        /// <param name="path">the current path to the table</param>
        /// <param name="table">The data table to extract the columns for</param>
        /// <returns></returns>
        private static IEnumerable<DataItem> GetTableColumnSchema(string path, System.Data.DataTable table)
        {
            List<DataItem> all = new List<DataItem>();
            foreach (System.Data.DataColumn col in table.Columns)
            {
                if (col.ColumnMapping == System.Data.MappingType.Hidden)
                {
                    //Do Nothing
                }
                else
                {
                    DataItem item = GetColumnDataItem(path, col);
                    if (null != item)
                        all.Add(item);
                }
            }
            return all;
        }

        #endregion

        #region private static PDFDataItem GetColumnDataItem(string path, System.Data.DataColumn col)

        /// <summary>
        /// Extracts a single schema data item for the provided data column
        /// </summary>
        /// <param name="path"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private static DataItem GetColumnDataItem(string path, System.Data.DataColumn col)
        {
            string name = col.ColumnName;
            string relativePath = System.Xml.XmlConvert.EncodeLocalName(name);
            string title = string.IsNullOrEmpty(col.Caption) ? name : col.Caption;

            System.Xml.XmlNodeType nodetype;
            if (col.ColumnMapping == System.Data.MappingType.Hidden)
                return null;
            else if (col.ColumnMapping == System.Data.MappingType.Attribute)
            {
                relativePath = "@" + relativePath;
                nodetype = System.Xml.XmlNodeType.Attribute;
            }
            else if (col.ColumnMapping == System.Data.MappingType.SimpleContent)
            {
                relativePath = "text()";
                nodetype = System.Xml.XmlNodeType.Text;
            }
            else if (col.ColumnMapping == System.Data.MappingType.Element)
            {
                nodetype = System.Xml.XmlNodeType.Element;
                relativePath += "/text()";
            }
            else
                throw new ArgumentOutOfRangeException("col.ColumnMapping");

            string fullpath = DataSchema.CombineElementNames(path, relativePath);
            
            DataItem item = new DataItem(fullpath, relativePath, name, title, nodetype, GetDataTypeFromSystemType(col));
            return item;

        }

        #endregion

        private static DataType GetDataTypeFromSystemType(DataColumn column)
        {
            if (column is IPDFDataColumn)
                return ((IPDFDataColumn)column).DataType;
            else
            {
                Type type = column.DataType;
                DataType dtype;
                switch (System.Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        dtype = DataType.Boolean;
                        break;

                    case TypeCode.Char:
                    case TypeCode.DBNull:
                    case TypeCode.String:
                        dtype = DataType.String;
                        break;

                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                    case TypeCode.SByte:
                        dtype = DataType.Integer;
                        break;

                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Single:
                        dtype = DataType.Double;
                        break;

                    case TypeCode.DateTime:
                        dtype = DataType.DateTime;
                        break;

                    case TypeCode.Object:
                        if (type == typeof(byte[]))
                            dtype = DataType.Image;
                        else if (type == typeof(Uri))
                            dtype = DataType.Url;
                        else if (typeof(System.Collections.ICollection).IsAssignableFrom(type))
                            dtype = DataType.Array;
                        else
                            dtype = DataType.Unknown;
                        break;

                    default:
                        dtype = DataType.Unknown;
                        break;
                }

                return dtype;
            }
        }

        #region private System.Data.DataTable[] GetTopLevelTables(System.Data.DataSet dataset)

        /// <summary>
        /// Gets all the tables in the dataset that do not have a parent relation (i.e. they are at the top of the set).
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        private static System.Data.DataTable[] GetTopLevelTables(System.Data.DataSet dataset)
        {
            List<System.Data.DataTable> found = new List<System.Data.DataTable>();
            foreach (System.Data.DataTable dt in dataset.Tables)
            {
                if (dt.ParentRelations == null || dt.ParentRelations.Count == 0)
                    found.Add(dt);
                else
                {
                    //We have parent relation, but if they are to this table then we are still top level.
                    bool selfOnly = true;
                    foreach(System.Data.DataRelation rel in dt.ParentRelations)
                    {
                        if (rel.ParentTable != dt)
                            selfOnly = false;
                    }

                    if (selfOnly)
                        found.Add(dt);
                }
            }
            return found.ToArray();
        }

        #endregion
    }
}
