using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.XQueue.Live.Sql {

    internal class spUSCIS_ResolvedCase_Insert : FdblSql {

        #region Constructors

        public spUSCIS_ResolvedCase_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_ResolvedCase_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISRequestId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISTransactionId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@Processed", SqlDbType.Bit);
            param = SqlCommand.Parameters.Add("@CaseNumber", SqlDbType.Char, 15);
            param = SqlCommand.Parameters.Add("@TypeOfCase", SqlDbType.Char, 1);
            param = SqlCommand.Parameters.Add("@ResolvedDate", SqlDbType.VarChar, 32);
            param = SqlCommand.Parameters.Add("@ResponseCode", SqlDbType.VarChar, 3);
            param = SqlCommand.Parameters.Add("@ResponseStatement", SqlDbType.VarChar, 64);
            param = SqlCommand.Parameters.Add("@UserField", SqlDbType.VarChar, 40);
            param = SqlCommand.Parameters.Add("@Issue", SqlDbType.VarChar, 1024);
            param = SqlCommand.Parameters.Add("@Details", SqlDbType.Text);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(Items.CaseInfo ci) {

            if (ci == null) throw new ArgumentNullException("CaseInfo is null");

            if (ci.RequestId == MyConsole.NullId) throw new ArgumentException("USCIS case id is invalid");

            ResetCommand();

            SqlCommand.Parameters["@USCISRequestId"].Value = ci.RequestId;
            SqlCommand.Parameters["@Processed"].Value = (ci.Processed ? 1 : 0);
            SqlCommand.Parameters["@ResolvedDate"].Value = ci.ResolveDate;

            if (ci.TransactionId != MyConsole.NullId) SqlCommand.Parameters["@USCISTransactionId"].Value = ci.TransactionId;
            if (!string.IsNullOrEmpty(ci.CaseNumber)) SqlCommand.Parameters["@CaseNumber"].Value = ci.CaseNumber;
            if (!string.IsNullOrEmpty(ci.TypeOfCase)) SqlCommand.Parameters["@TypeOfCase"].Value = ci.TypeOfCase;
            if (!string.IsNullOrEmpty(ci.ResponseCode)) SqlCommand.Parameters["@ResponseCode"].Value = ci.ResponseCode;
            if (!string.IsNullOrEmpty(ci.ResponseStatement)) SqlCommand.Parameters["@ResponseStatement"].Value = ci.ResponseStatement;
            if (!string.IsNullOrEmpty(ci.UserField)) SqlCommand.Parameters["@UserField"].Value = ci.UserField;
            if (!string.IsNullOrEmpty(ci.Issue)) SqlCommand.Parameters["@Issue"].Value = ci.Issue;
            if (!string.IsNullOrEmpty(ci.Details)) SqlCommand.Parameters["@Details"].Value = ci.Details;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}