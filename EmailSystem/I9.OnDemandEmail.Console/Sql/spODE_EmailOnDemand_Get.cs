using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.OnDemandEmail.Console.Sql {

    internal class spODE_EmailOnDemand_Get : FdblSql {

        #region Constructors

        internal spODE_EmailOnDemand_Get(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spODE_EmailOnDemand_Get", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader() {

            ResetCommand();

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}