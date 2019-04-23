using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_EmployeeI9_GetIdentities : FdblSql {

        #region Constructors

        public spUSCIS_EmployeeI9_GetIdentities(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_EmployeeI9_GetIdentities", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@CaseNumber", SqlDbType.VarChar, 15);

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(string caseNumber) {

            if (string.IsNullOrEmpty(caseNumber)) throw new ArgumentException("case number is null/blank");

            ResetCommand();

            SqlCommand.Parameters["@CaseNumber"].Value = caseNumber;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}