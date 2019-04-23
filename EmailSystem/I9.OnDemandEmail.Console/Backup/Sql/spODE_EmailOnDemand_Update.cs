using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.OnDemandEmail.Console.Sql {

    internal class spODE_EmailOnDemand_Update : FdblSql {

        #region Constructors

        internal spODE_EmailOnDemand_Update(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spODE_EmailOnDemand_Update", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmailOnDemandId", SqlDbType.Int);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idEmailOnDemad) {

            if (idEmailOnDemad == MyConsole.NullId) throw new ArgumentException("email on-demand id is invalid");

            ResetCommand();

            SqlCommand.Parameters["@EmailOnDemandId"].Value = idEmailOnDemad;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}