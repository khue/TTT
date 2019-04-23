using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_Closure_Get : FdblSql {

        #region Constructors

        public spUSCIS_Closure_Get(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_Closure_Get", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@ClosureId", SqlDbType.Int);

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idClosure) {

            if (idClosure < 1) throw new ArgumentException("invalid closure id");

            ResetCommand();

            SqlCommand.Parameters["@ClosureId"].Value = idClosure;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}