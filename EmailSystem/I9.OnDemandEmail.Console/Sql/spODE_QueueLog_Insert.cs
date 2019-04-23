using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.OnDemandEmail.Console.Sql {

    internal class spODE_QueueLog_Insert : FdblSql {

        #region Constructors

        internal spODE_QueueLog_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spODE_QueueLog_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmailQueueId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmailLogTypeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@FieldName", SqlDbType.VarChar, 256);
            param = SqlCommand.Parameters.Add("@Message", SqlDbType.VarChar, 1024);
            param = SqlCommand.Parameters.Add("@Details", SqlDbType.Text);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idEmailQueue, int idEmailLogType, Exception ex) {

            if (idEmailQueue < 1) throw new ArgumentException("email queue id is invalid");
            if (idEmailLogType < 1) throw new ArgumentException("email log type id is invalid");
            if (ex == null) throw new ArgumentNullException("exception is null");

            return StartDataReader(idEmailQueue, idEmailLogType, null, ex.Message, FdblExceptions.GetDetails(ex));

        }

        public int StartDataReader(int idEmailQueue, int idEmailLogType, string fieldName, string message, string details) {

            if (idEmailQueue < 1) throw new ArgumentException("email queue id is invalid");
            if (idEmailLogType < 1) throw new ArgumentException("email log type id is invalid");
            if (string.IsNullOrEmpty(fieldName) && string.IsNullOrEmpty(message)) throw new ArgumentException("field name and message are both null/blank");

            ResetCommand();

            SqlCommand.Parameters["@EmailQueueId"].Value = idEmailQueue;
            SqlCommand.Parameters["@EmailLogTypeId"].Value = idEmailLogType;

            if (!string.IsNullOrEmpty(fieldName)) SqlCommand.Parameters["@FieldName"].Value = fieldName;
            if (!string.IsNullOrEmpty(message)) SqlCommand.Parameters["@Message"].Value = message;
            if (!string.IsNullOrEmpty(details)) SqlCommand.Parameters["@Details"].Value = details;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}