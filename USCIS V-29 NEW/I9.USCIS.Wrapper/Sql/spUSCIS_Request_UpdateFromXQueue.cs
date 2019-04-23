using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_Request_UpdateFromXQueue : FdblSql {

        #region Constructors

        public spUSCIS_Request_UpdateFromXQueue(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_Request_UpdateFromXQueue", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISRequestId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISTransactionId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@EmployeeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@I9Id", SqlDbType.Int);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idUSCISRequest, int idUSCISTransaction, int idEmployee, int idI9) {

            if (idUSCISRequest < 1) throw new ArgumentException("uscis request id is invalid");
            if (idUSCISTransaction < 1) throw new ArgumentException("uscis transaction id is invalid");
            if (idEmployee < 1 && idEmployee != -99) throw new ArgumentException("invalid employee id");
            if (idI9 < 1 && idI9 != -99) throw new ArgumentException("invalid i9 id");

            ResetCommand();

            SqlCommand.Parameters["@USCISRequestId"].Value = idUSCISRequest;
            SqlCommand.Parameters["@USCISTransactionId"].Value = idUSCISTransaction;
            SqlCommand.Parameters["@EmployeeId"].Value = idEmployee;
            SqlCommand.Parameters["@I9Id"].Value = idI9;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}