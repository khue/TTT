using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_QueueError_Insert : FdblSql {

        #region Constructors

        public spUSCIS_QueueError_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_QueueError_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISSystemId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@USCISTransactionId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISRequestId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISActionId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@USCISClosureId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmployeeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@I9Id", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@CaseNumber", SqlDbType.VarChar, 15);
            param = SqlCommand.Parameters.Add("@Comments", SqlDbType.VarChar, 150);
            param = SqlCommand.Parameters.Add("@CanBeProcessed", SqlDbType.Bit);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(WebService.ProcessIds pids, USCISActionId idAction, USCISClosureId idClosure, string comments, bool canBeProcessed) {

            if (pids.SystemId == USCISSystemId.Unknown) throw new ArgumentException("invalid system id");

            if (pids.EmployeeId == WebService.Request.NullId) throw new ArgumentException("invalid employee id");
            if (pids.I9Id == WebService.Request.NullId) throw new ArgumentException("invalid i9 id");

            ResetCommand();

            SqlCommand.Parameters["@USCISSystemId"].Value = pids.SystemId;
            SqlCommand.Parameters["@USCISActionId"].Value = (int)idAction;
            SqlCommand.Parameters["@EmployeeId"].Value = pids.EmployeeId;
            SqlCommand.Parameters["@I9Id"].Value = pids.I9Id;
            SqlCommand.Parameters["@CanBeProcessed"].Value = (canBeProcessed ? 1 : 0);

            if (pids.TransactionId == WebService.Request.NullId) SqlCommand.Parameters["@USCISTransactionId"].Value = pids.TransactionId;
            if (pids.RequestId != WebService.Request.NullId) SqlCommand.Parameters["@USCISRequestId"].Value = pids.RequestId;
            if (idClosure != USCISClosureId.None) SqlCommand.Parameters["@USCISClosureId"].Value = (int)idClosure;
            if (!string.IsNullOrEmpty(pids.CaseNumber)) SqlCommand.Parameters["@CaseNumber"].Value = pids.CaseNumber;
            if (!string.IsNullOrEmpty(comments)) SqlCommand.Parameters["@Comments"].Value = comments;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}