using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_EmployeeI9_GetDetails : FdblSql {

        #region Constructors

        public spUSCIS_EmployeeI9_GetDetails(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_EmployeeI9_GetDetails", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmployeeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@I9Id", SqlDbType.Int);

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int employeeId, int i9Id) {

            if (employeeId < 1) throw new ArgumentException("invalid employee id");
            if (i9Id < 1) throw new ArgumentException("invalid i9 id");

            ResetCommand();

            SqlCommand.Parameters["@EmployeeId"].Value = employeeId;
            SqlCommand.Parameters["@I9Id"].Value = i9Id;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}