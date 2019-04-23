using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.ScheduledEmail.Console.Sql {

    internal class spSE_EmailJobLog_Insert : FdblSql {

        #region Constructors

        internal spSE_EmailJobLog_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spSE_EmailJobLog_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmailJobId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmailTemplateId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@ProcessRecordId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@Message", SqlDbType.VarChar, 1024);
            param = SqlCommand.Parameters.Add("@Details", SqlDbType.Text);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idEmailJob, int idEmailTemplate, int idProcessRecord, Exception ex) {

            if (ex == null) throw new ArgumentNullException("exception is null");

            return StartDataReader(idEmailJob, idEmailTemplate, idProcessRecord, ex.Message, FdblExceptions.GetDetails(ex));

        }

        public int StartDataReader(int idEmailJob, int idEmailTemplate, int idProcessRecord, string message, string details) {

            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException("message is null/blank");

            ResetCommand();

            if (idEmailJob > 0) SqlCommand.Parameters["@EmailJobId"].Value = idEmailJob;
            if (idEmailTemplate > 0) SqlCommand.Parameters["@EmailTemplateId"].Value = idEmailTemplate;
            if (idProcessRecord > 0) SqlCommand.Parameters["@ProcessRecordId"].Value = idProcessRecord;
            SqlCommand.Parameters["@Message"].Value = message;
            if (!string.IsNullOrEmpty(details)) SqlCommand.Parameters["@Details"].Value = details;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}