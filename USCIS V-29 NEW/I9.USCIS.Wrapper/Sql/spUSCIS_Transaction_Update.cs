using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_Transaction_Update : FdblSql {

        #region Constructors

        public spUSCIS_Transaction_Update(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_Transaction_Update", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISTransactionId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISRequestId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISResponseId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@USCISCaseStatusId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@USCISProcessStatusId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@USCISClosureId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@I9Id", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@CaseNumber", SqlDbType.VarChar, 15);
            param = SqlCommand.Parameters.Add("@CurrentlyEmployed", SqlDbType.VarChar, 1);
            param = SqlCommand.Parameters.Add("@TNCDueDate", SqlDbType.VarChar, 20);
            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idUSCISTransaction, int idUSCISRequest, int idUSCISResponse, USCISCaseStatus idUSCISCaseStatus, USCISProcessStatus idUSCISProcessStatus, USCISClosureId idClosure, int idI9, string caseNumber, string currentlyEmployed, string TNCDueDate) {

            if (idUSCISTransaction < 1) throw new ArgumentException("uscis transaction id is invalid");

            ResetCommand();
            
            SqlCommand.Parameters["@USCISTransactionId"].Value = idUSCISTransaction;

            if (idUSCISRequest != WebService.Request.NullId) SqlCommand.Parameters["@USCISRequestId"].Value = idUSCISRequest;
            if (idUSCISResponse != WebService.Request.NullId) SqlCommand.Parameters["@USCISResponseId"].Value = idUSCISResponse;
            if (idUSCISCaseStatus != USCISCaseStatus.Unknown) SqlCommand.Parameters["@USCISCaseStatusId"].Value = (int)idUSCISCaseStatus;
            if (idUSCISProcessStatus != USCISProcessStatus.Unknown) SqlCommand.Parameters["@USCISProcessStatusId"].Value = (int)idUSCISProcessStatus;
            if (idClosure != USCISClosureId.None) SqlCommand.Parameters["@USCISClosureId"].Value = (int)idClosure;
            if (idI9 > 0) SqlCommand.Parameters["@I9Id"].Value = idI9;
            if (!string.IsNullOrEmpty(caseNumber)) SqlCommand.Parameters["@CaseNumber"].Value = caseNumber;
            if (!string.IsNullOrEmpty(currentlyEmployed)) SqlCommand.Parameters["@CurrentlyEmployed"].Value = currentlyEmployed;
            if (!string.IsNullOrEmpty(TNCDueDate)) SqlCommand.Parameters["@TNCDueDate"].Value = TNCDueDate;
            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}
