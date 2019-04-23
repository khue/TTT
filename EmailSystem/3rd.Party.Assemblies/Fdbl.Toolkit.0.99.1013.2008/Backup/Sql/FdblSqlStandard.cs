using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace Fdbl.Toolkit.Sql {

    public class FdblSqlStandard : FdblSql {

        #region Methods - Public

        public long Execute(string statement, FdblExecuteAction action) {

            if (statement == null) throw new ArgumentNullException("sql statement is null");
            if (statement.Trim().Length == 0) throw new ArgumentException("sql statement is blank");

            SqlCommand cmd = null;
            long retId = 0;

            OpenConnection(true);

            if (action == FdblExecuteAction.Add) {

                cmd = new SqlCommand(string.Format("SET NOCOUNT ON; {0}; SELECT @@IDENTITY;", statement), SqlConnection);
                object id = cmd.ExecuteScalar();
                retId = (id == null || id.ToString().Trim().Length == 0) ? -1 : Convert.ToInt64(id);

            } else {

                cmd = new SqlCommand(statement, SqlConnection);
                retId = (long)cmd.ExecuteNonQuery();

            }

            cmd.Dispose();
            CloseConnection();

            return retId;

        }

        public int ExecuteNonQuery(string statement) {

            if (statement == null) throw new ArgumentNullException("sql statement is null");
            if (statement.Trim().Length == 0) throw new ArgumentException("sql statement is blank");

            OpenConnection(true);

            SqlCommand cmd = new SqlCommand(statement, SqlConnection);
            int retId = cmd.ExecuteNonQuery();

            cmd.Dispose();
            CloseConnection();

            return retId;

        }

        public object ExecuteScalar(string statement) {

            if (statement == null) throw new ArgumentNullException("sql statement is null");
            if (statement.Trim().Length == 0) throw new ArgumentException("sql statement is blank");

            OpenConnection(true);

            SqlCommand cmd = new SqlCommand(statement, SqlConnection);
            object retData = cmd.ExecuteScalar();

            cmd.Dispose();
            CloseConnection();

            return retData;

        }

        public object Query(string statement, FdblQueryResult result) {

            if (statement == null) throw new ArgumentNullException("sql statement is null");
            if (statement.Trim().Length == 0) throw new ArgumentException("sql statement is blank");

            OpenConnection(false);

            SqlCommand cmd = new SqlCommand(statement, SqlConnection);
            SqlDataReader sdr = cmd.ExecuteReader();

            Array sqlColumns = Array.CreateInstance(typeof(string), sdr.FieldCount);
            for (int pos = 0; pos < sdr.FieldCount; pos++) {
                sqlColumns.SetValue(sdr.GetName(pos), pos);
            }

            Collections.FdblCollection cols = null;
            XmlDocument xml = null;

            if (result == FdblQueryResult.Collection) {

                cols = new Collections.FdblCollection();

                while (sdr.Read()) {

                    using (Collections.FdblCollection row = new Collections.FdblCollection()) {

                        for (int pos = 0; pos < sdr.FieldCount; pos++) {
                            row.Add(sdr[pos], (string)sqlColumns.GetValue(pos));
                        }

                        cols.Add(row.Clone());

                    }

                }

            } else if (result == FdblQueryResult.Xml) {

                xml = new XmlDocument();

                int cnt = 0;

                xml.LoadXml("<VirtualData/>");
                XmlElement root = (XmlElement)xml.GetElementsByTagName("VirtualData").Item(0);
                XmlElement resultNode = (XmlElement)xml.CreateNode(XmlNodeType.Element, "ResultSet", string.Empty);

                while (sdr.Read()) {

                    cnt++;
                    XmlElement rowNode = (XmlElement)xml.CreateNode(XmlNodeType.Element, "Row", string.Empty);

                    for (int pos = 0; pos < sdr.FieldCount; pos++) {

                        XmlElement columnNode = (XmlElement)xml.CreateNode(XmlNodeType.Element, "Column", string.Empty);

                        columnNode.SetAttribute("Id", Convert.ToString(pos + 1));
                        columnNode.SetAttribute("Name", (string)sqlColumns.GetValue(pos));
                        columnNode.SetAttribute("Data", Convert.IsDBNull(sdr[pos]) ? string.Empty : sdr.GetValue(pos).ToString());

                        rowNode.AppendChild(columnNode);

                    }

                    resultNode.AppendChild(rowNode);

                }

                resultNode.SetAttribute("Rows", Convert.ToString(cnt));
                resultNode.SetAttribute("Columns", Convert.ToString(sdr.FieldCount));
                root.AppendChild(resultNode);

            }

            sdr.Close();
            cmd.Dispose();

            CloseConnection();

            if (result == FdblQueryResult.Collection) return cols;
            if (result == FdblQueryResult.Xml) return xml;

            return null;

        }

        #endregion

        #region Constructors

        public FdblSqlStandard(string sqlConnectionInfo) : base(sqlConnectionInfo) { }

        #endregion

    }

}