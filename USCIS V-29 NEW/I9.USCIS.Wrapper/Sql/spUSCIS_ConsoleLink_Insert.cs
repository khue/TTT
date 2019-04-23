using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_ConsoleLink_Insert : FdblSql {

        #region Constructors

        public spUSCIS_ConsoleLink_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_ConsoleLink_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISConsoleId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISQueueErrorId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISQueueFutureId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISRequestId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISTransactionId", SqlDbType.BigInt);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(WebService.ProcessIds pids) {

            if (pids == null) throw new ArgumentNullException("ProcessIds (pids) is null");

            if (pids.ConsoleId == WebService.Request.NullId && pids.QueueErrorId == WebService.Request.NullId && pids.QueueFutureId == WebService.Request.NullId) throw new ArgumentException("Process id (pid) ConsoleId/QueueErrorId/QueueFutureId is invalid");
            if (pids.RequestId == WebService.Request.NullId) throw new ArgumentException("Process id (pid) RequestId is invalid");
            if (pids.TransactionId == WebService.Request.NullId) throw new ArgumentException("Process id (pid) TransactionId is invalid");

            ResetCommand();

            if (pids.ConsoleId != WebService.Request.NullId) SqlCommand.Parameters["@USCISConsoleId"].Value = pids.ConsoleId;
            if (pids.QueueErrorId != WebService.Request.NullId) SqlCommand.Parameters["@USCISQueueErrorId"].Value = pids.QueueErrorId;
            if (pids.QueueFutureId != WebService.Request.NullId) SqlCommand.Parameters["@USCISQueueFutureId"].Value = pids.QueueFutureId;

            SqlCommand.Parameters["@USCISRequestId"].Value = pids.RequestId;
            SqlCommand.Parameters["@USCISTransactionId"].Value = pids.TransactionId;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}