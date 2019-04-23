using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_Transaction_Insert : FdblSql {

        #region Constructors

        public spUSCIS_Transaction_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_Transaction_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISSystemId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmployeeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@I9Id", SqlDbType.Int);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idUSCISSystem, int idEmployee, int idI9) {

            if (idUSCISSystem < 1) throw new ArgumentException("system id is invalid");
            if (idEmployee < 1) throw new ArgumentException("employee id is invalid");
            if (idI9 < 1) throw new ArgumentException("i9 id is invalid");

            ResetCommand();

            SqlCommand.Parameters["@USCISSystemId"].Value = idUSCISSystem;
            SqlCommand.Parameters["@EmployeeId"].Value = idEmployee;
            SqlCommand.Parameters["@I9Id"].Value = idI9;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}