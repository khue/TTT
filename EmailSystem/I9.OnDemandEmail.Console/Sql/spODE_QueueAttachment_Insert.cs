using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.OnDemandEmail.Console.Sql {

    internal class spODE_QueueAttachment_Insert : FdblSql {

        #region Constructors

        internal spODE_QueueAttachment_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spODE_QueueAttachment_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmailQueueId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmailTemplateAttachmentId", SqlDbType.Int);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idEmailQueue, int idEmailTemplateAttachment) {

            if (idEmailQueue < 1) throw new ArgumentException("email queue id is invalid");
            if (idEmailTemplateAttachment < 1) throw new ArgumentException("email template attachment id is invalid");

            ResetCommand();

            SqlCommand.Parameters["@EmailQueueId"].Value = idEmailQueue;
            SqlCommand.Parameters["@EmailTemplateAttachmentId"].Value = idEmailTemplateAttachment;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}