using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_Log_Insert : FdblSql {

        #region Constructors

        public spUSCIS_Log_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_Log_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISTransactionId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISRequestId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISResponseId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISQueueErrorId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISQueueFutureId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISConsoleId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISCategoryId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@USCISSystemId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@LogLevel", SqlDbType.VarChar, 25);
            param = SqlCommand.Parameters.Add("@ClassModule", SqlDbType.VarChar, 512);
            param = SqlCommand.Parameters.Add("@FriendlyMessage", SqlDbType.VarChar, 1024);
            param = SqlCommand.Parameters.Add("@Message", SqlDbType.VarChar, 4096);
            param = SqlCommand.Parameters.Add("@Details", SqlDbType.Text);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(WebService.ProcessIds pids, LogLevel logLevel, string classModule, string friendlyMessage, string message, string details) {

            ResetCommand();

            if (pids != null) {

                if (pids.TransactionId != WebService.Request.NullId) SqlCommand.Parameters["@USCISTransactionId"].Value = pids.TransactionId;
                if (pids.RequestId != WebService.Request.NullId) SqlCommand.Parameters["@USCISRequestId"].Value = pids.RequestId;
                if (pids.ResponseId != WebService.Request.NullId) SqlCommand.Parameters["@USCISResponseId"].Value = pids.ResponseId;
                if (pids.QueueErrorId != WebService.Request.NullId) SqlCommand.Parameters["@USCISQueueErrorId"].Value = pids.QueueErrorId;
                if (pids.QueueFutureId != WebService.Request.NullId) SqlCommand.Parameters["@USCISQueueFutureId"].Value = pids.QueueFutureId;
                if (pids.ConsoleId != WebService.Request.NullId) SqlCommand.Parameters["@USCISConsoleId"].Value = pids.ConsoleId;
                if (pids.CategoryId != USCISCategoryId.Unknown) SqlCommand.Parameters["@USCISCategoryId"].Value = (int)pids.CategoryId;
                if (pids.SystemId != USCISSystemId.Unknown) SqlCommand.Parameters["@USCISSystemId"].Value = (int)pids.SystemId;

            }

            SqlCommand.Parameters["@LogLevel"].Value = Convert.ToString(logLevel).ToLower();
            if (!string.IsNullOrEmpty(classModule)) SqlCommand.Parameters["@ClassModule"].Value = classModule;
            if (!string.IsNullOrEmpty(friendlyMessage)) SqlCommand.Parameters["@FriendlyMessage"].Value = friendlyMessage;
            SqlCommand.Parameters["@Message"].Value = message;
            if (!string.IsNullOrEmpty(details)) SqlCommand.Parameters["@Details"].Value = details;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}