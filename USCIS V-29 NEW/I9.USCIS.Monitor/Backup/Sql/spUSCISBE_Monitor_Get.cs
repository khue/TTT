using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;

namespace I9.USCIS.Monitor.Sql {

    internal class spUSCISBE_Monitor_Get : FdblSql {

        #region Constructors

        public spUSCISBE_Monitor_Get(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCISBE_Monitor_Get", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader() {

            ResetCommand();

            return (int)OpenDataReader(true);

        }

        #endregion

    }

}