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
using System.Xml.XPath;
using System.Data;
using System.Data.Common;

namespace Scryber.Data
{
    /// <summary>
    /// Encapsulates a SQL command to a database.
    /// </summary>
    [PDFRequiredFramework("0.9.0")]
    [PDFParsableComponent("SqlCommand")]
    [PDFJSConvertor("scryber.studio.design.sql.convertors.sql_SQLCommand")]
    public class SqlProviderCommand : DataTableProviderCommandBase, IPDFDataSetProviderCommand
    {
        //the category name of the sql command
        internal const string SqlCommandLog = "Sql Command"; 
        
        #region public string Statement {get;set;}

        private string _statement;
        /// <summary>
        /// Gets or sets the SQL statement to be executed
        /// </summary>
        [PDFAttribute("statement")]
        [PDFElement("Statement")]
        public string Statement
        {
            get { return _statement; }
            set
            {
                _statement = value;
            }
        }

        #endregion

        #region public CommandType CommandType {get;set;}

        private CommandType _type = CommandType.Text;

        /// <summary>
        /// Gets or sets the type of SQL command to be executed
        /// </summary>
        [PDFAttribute("command-type")]
        public CommandType CommandType
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion

        #region public string ConnectionName {get;set;}

        private string _conname;

        /// <summary>
        /// Gets or sets the name of the connection string to use in the config file.
        /// </summary>
        [PDFAttribute("connection-name")]
        [PDFDesignable(Ignore =true)]
        public string ConnectionName
        {
            get { return _conname; }
            set { _conname = value; }
        }

        #endregion

        #region public string ConnectionString {get;set;}

        private string _constring;

        /// <summary>
        /// Gets or sets the explict connection string to a sql data source. 
        /// This will be used as a higher priority than the connection name
        /// </summary>
        [PDFAttribute("connection-string")]
        public string ConnectionString
        {
            get { return _constring; }
            set { _constring = value; }
        }

        #endregion

        #region public string ConnectionProvider {get;set;}

        private string _prov = "System.Data.SqlClient";

        /// <summary>
        /// Gets or sets the provider name for an explicit connection 
        /// to a sql data source. It is a required value if the connection string is set, but defaulted to SQLClient provider
        /// </summary>
        [PDFAttribute("connection-provider")]
        public string ConnectionProvider
        {
            get { return _prov; }
            set { _prov = value; }
        }

        #endregion

        #region public int CommandTimeOut {get;set;}

        private int _timeout = -1;
        /// <summary>
        /// Gets or sets the timeout for the command execution
        /// </summary>
        [PDFAttribute("command-timeout")]
        public int CommandTimeOut
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        #endregion

        //
        // ctor
        //

        #region public PDFSqlCommand()

        public SqlProviderCommand()
            : this(PDFObjectTypes.SqlCommandType)
        {
        }

        #endregion

        #region protected PDFSqlCommand(PDFObjectType type)

        protected SqlProviderCommand(PDFObjectType type)
            : base(type)
        {
        }

        #endregion

        //
        // override methods
        //

        #region public override object GetNullValue()

        /// <summary>
        /// Gets the native null or empty value for the commands source
        /// </summary>
        /// <returns></returns>
        public override object GetNullValue(System.Data.DbType type)
        {
            return System.DBNull.Value;
        }

        #endregion

        

        #region protected override void DoDataBind(PDFDataContext context)

        /// <summary>
        /// Overrides the base implementation to ensure that the parameters and relations are databound
        /// </summary>
        /// <param name="context"></param>
        protected override void DoDataBind(PDFDataContext context, bool includeChildren)
        {
            base.DoDataBind(context, includeChildren);
            if (this.HasParameters)
                this.Parameters.DataBind(context);
            if (this.HasRelations)
                this.Relations.DataBind(context);
        }

        #endregion

        //
        // support methods
        //


        #region public void FillData(DataSet ds, PDFProviderParameterValueCollection values)

        /// <summary>
        /// Loads all the required data from this commands Sql source into the dataset using the
        /// Sql statement and the provided parameter values that have ben explicitly set
        /// </summary>
        /// <param name="ds">the DataSet to populate</param>
        /// <param name="index">The index of the command</param>
        public override void FillData(DataSet ds, XPathDataSourceBase source, IPDFDataSetProviderCommand parent, PDFDataContext context)
        {
            bool logDebug = context.TraceLog.ShouldLog(TraceLevel.Debug);

            if (logDebug)
                context.TraceLog.Begin(TraceLevel.Debug, SqlCommandLog, string.Format("Starting load of data for sqlCommand '{0}'", this.ID));

            DbConnection con = null;
            DbCommand cmd = null;

            try
            {
                con = this.CreateConnection(source, parent as SqlProviderCommand);//parent could be null but handled in the code

                if (logDebug)
                    context.TraceLog.Add(TraceLevel.Debug, SqlCommandLog, "Created db connection to sql database");
                
                cmd = this.CreateCommand(con, this.CommandType, this.Statement);
                if (logDebug)
                    context.TraceLog.Add(TraceLevel.Debug, SqlCommandLog, "Created db command for statement");
                
                this.PopulateParameters(cmd, context);
                
                if (logDebug)
                    context.TraceLog.Add(TraceLevel.Debug, SqlCommandLog, "Added " + cmd.Parameters.Count + " parameters to db command");
                
                con.Open();
                this.PopulateData(ds, cmd);

                if (!string.IsNullOrEmpty(this.AsAttributes))
                    ApplyAttributes(ds, this.AsAttributes, context);

                if (logDebug)
                    context.TraceLog.Add(TraceLevel.Debug, SqlCommandLog, "Dataset populated");
                
            }
            finally
            {
                if (null != cmd)
                    cmd.Dispose();
                if (null != con)
                    con.Dispose();

                if (logDebug)
                    context.TraceLog.Add(TraceLevel.Debug, SqlCommandLog, string.Format("Disposed of command and connection"));

            }

            if (logDebug)
                context.TraceLog.End(TraceLevel.Debug, SqlCommandLog, string.Format("Completed load of data for sqlCommand '{0}'", this.ID));

        }

        #endregion

        
        
        //
        // SQL DB Command Methods
        //

        #region private void PopulateData(DataSet ds, int index, DbCommand cmd)

        /// <summary>
        /// Fills a the dataset with the data as a new datatable after executing the command
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private void PopulateData(DataSet ds, DbCommand cmd)
        {
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                string tname = GetDataTableName(ds);
                DataTable dt = new DataTable(tname);

                if (!string.IsNullOrEmpty(this.ElementNamespaceUri))
                    dt.Namespace = this.ElementNamespaceUri;

                dt.Load(reader, LoadOption.OverwriteChanges);
                ds.Tables.Add(dt);
                dt.AcceptChanges();
            }
        }

        #endregion

        #region private void PopulateParameters(DbCommand cmd, PDFProviderParameterValueCollection values)

        /// <summary>
        /// Adds the parameters defined on this PDFSqlCommand to the DbCommand setting their properties and value
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="values"></param>
        private void PopulateParameters(DbCommand cmd, PDFDataContext context)
        {
            if (this.HasParameters)
            {
                foreach (DataParameter param in this.Parameters)
                {
                    DbParameter dbparm = cmd.CreateParameter();
                    dbparm.DbType = param.DataType;
                    if (param.Size > 0)
                        dbparm.Size = param.Size;
                    dbparm.ParameterName = param.ParameterName;
                    dbparm.Value = param.GetParameterValue(this, context);
                    cmd.Parameters.Add(dbparm);
                }
            }
        }

        #endregion

        #region private DbCommand CreateCommand(DbConnection con, System.Data.CommandType commandType, string statement)

        /// <summary>
        /// Creates a new DbCommand with the connection, type and Sql statement.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandType"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        private DbCommand CreateCommand(DbConnection con, System.Data.CommandType commandType, string statement)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Connection = con;
            cmd.CommandType = commandType;
            cmd.CommandText = statement;

            if (this.CommandTimeOut > 0)
                cmd.CommandTimeout = this.CommandTimeOut;

            return cmd;
        }

        #endregion

        #region private DbConnection CreateConnection(PDFXPathDataSourceBase source, PDFSqlProviderCommand parent)

        /// <summary>
        /// Creates a new connection to the Sql database. Returns connection closed.
        /// </summary>
        /// <returns></returns>
        private DbConnection CreateConnection(XPathDataSourceBase source, SqlProviderCommand parent)
        {
            if (null != parent)
            {
                if (string.IsNullOrEmpty(this.ConnectionName))
                    this.ConnectionName = parent.ConnectionName;

                if (string.IsNullOrEmpty(this.ConnectionString))
                {
                    this.ConnectionString = parent.ConnectionString;
                    this.ConnectionProvider = parent.ConnectionProvider;
                }
            }

            string constr;
            string provider;

            if (!string.IsNullOrEmpty(this.ConnectionString))
            {
                constr = this.ConnectionString;
                provider = this.ConnectionProvider;
            }
            else if (!string.IsNullOrEmpty(this.ConnectionName))
            {
                System.Configuration.ConnectionStringSettings consettings = System.Configuration.ConfigurationManager.ConnectionStrings[this.ConnectionName];
                if (null == consettings)
                    throw new NullReferenceException(string.Format(Errors.NoConnectionSettingFor, this.ConnectionName));
                constr = consettings.ConnectionString;
                provider = consettings.ProviderName;
            }
            else
                throw new NullReferenceException(string.Format(Errors.NoConnectionDefined, this.ID, source.ID));

            DbProviderFactory fact = DbProviderFactories.GetFactory(provider);
            if (null == fact)
                throw new NullReferenceException(string.Format(Errors.NoProviderFactoryWithName, provider));
            
            DbConnection con = fact.CreateConnection();
            con.ConnectionString = constr;
            return con;
        }

        #endregion

    }
}
