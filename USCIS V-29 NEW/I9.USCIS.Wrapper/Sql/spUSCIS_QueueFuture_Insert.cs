using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_QueueFuture_Insert : FdblSql {

        #region Constructors

        public spUSCIS_QueueFuture_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_QueueFuture_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISSystemId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@USCISTransactionId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@EmployeeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@I9Id", SqlDbType.Int);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(WebService.ProcessIds pids) {

            if (pids.SystemId == USCISSystemId.Unknown) throw new ArgumentException("invalid system id");
            if (pids.TransactionId == WebService.Request.NullId) throw new ArgumentException("invalid transaction id");

            if (pids.EmployeeId == WebService.Request.NullId) throw new ArgumentException("invalid employee id");
            if (pids.I9Id == WebService.Request.NullId) throw new ArgumentException("invalid i9 id");

            ResetCommand();

            SqlCommand.Parameters["@USCISSystemId"].Value = pids.SystemId;
            SqlCommand.Parameters["@USCISTransactionId"].Value = pids.TransactionId;
            SqlCommand.Parameters["@EmployeeId"].Value = pids.EmployeeId;
            SqlCommand.Parameters["@I9Id"].Value = pids.I9Id;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}