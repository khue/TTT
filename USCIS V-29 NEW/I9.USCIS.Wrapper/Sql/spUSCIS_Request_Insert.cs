using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_Request_Insert : FdblSql {

        #region Constructors

        public spUSCIS_Request_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_Request_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISTransactionId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISCategoryId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@USCISSystemId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@Attempt", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmployeeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@I9Id", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@MethodName", SqlDbType.VarChar, 255);
            param = SqlCommand.Parameters.Add("@RawData", SqlDbType.VarChar, 6144);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idUSCISTransaction, int idUSCISCategory, int idUSCISSystem, int idEmployee, int idI9, int attempt, string methodName, string rawData) {

            if (idUSCISSystem < 1) throw new ArgumentException("system id is invalid");
            if (idEmployee < 1 && idEmployee != -99) throw new ArgumentException("invalid employee id");
            if (idI9 < 1 && idI9 != -99) throw new ArgumentException("invalid i9 id");
            if (++attempt < 1 ) throw new ArgumentException("invalid attempt");

            if (methodName == null) throw new ArgumentNullException("request method is null");
            if (methodName.Trim().Length == 0) throw new ArgumentException("request method is blank");

            if (rawData == null) throw new ArgumentNullException("raw data is null");
            if (rawData.Trim().Length == 0) throw new ArgumentException("raw data is blank");

            ResetCommand();

            if (idUSCISTransaction != -1) SqlCommand.Parameters["@USCISTransactionId"].Value = idUSCISTransaction;
            if (idUSCISCategory != -1) SqlCommand.Parameters["@USCISCategoryId"].Value = idUSCISCategory;
            if (idUSCISSystem != -1) SqlCommand.Parameters["@USCISSystemId"].Value = idUSCISSystem;
            SqlCommand.Parameters["@Attempt"].Value = attempt;
            SqlCommand.Parameters["@EmployeeId"].Value = idEmployee;
            SqlCommand.Parameters["@I9Id"].Value = idI9;
            SqlCommand.Parameters["@MethodName"].Value = FdblStrings.Left(methodName, 255);
            SqlCommand.Parameters["@RawData"].Value = FdblStrings.Left(rawData, 6144);

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}