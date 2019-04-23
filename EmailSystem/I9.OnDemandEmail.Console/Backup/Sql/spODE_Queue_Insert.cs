using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.OnDemandEmail.Console.Sql {

    internal class spODE_Queue_Insert : FdblSql {

        #region Constructors

        internal spODE_Queue_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spODE_Queue_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmailTemplateId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@UserId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@AgentId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmployeeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@I9Id", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@FutureEmployeeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@SentFrom", SqlDbType.VarChar, 128);
            param = SqlCommand.Parameters.Add("@SendTo", SqlDbType.Text);
            param = SqlCommand.Parameters.Add("@CopyTo", SqlDbType.Text);
            param = SqlCommand.Parameters.Add("@BlindCopyTo", SqlDbType.Text);
            param = SqlCommand.Parameters.Add("@Subject", SqlDbType.VarChar, 128);
            param = SqlCommand.Parameters.Add("@MessageText", SqlDbType.Text);
            param = SqlCommand.Parameters.Add("@MessageHtml", SqlDbType.Text);
            param = SqlCommand.Parameters.Add("@Delivered", SqlDbType.Bit);
            param = SqlCommand.Parameters.Add("@HasKeywordIssue", SqlDbType.Bit);
            param = SqlCommand.Parameters.Add("@HasAddressIssue", SqlDbType.Bit);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(Items.EmailRecord er) {

            if (er == null) throw new ArgumentNullException("email record is null");

            ResetCommand();

            SqlCommand.Parameters["@EmailTemplateId"].Value = er.EmailTemplateId;
            SqlCommand.Parameters["@SentFrom"].Value = (!string.IsNullOrEmpty(er.SentFrom) ? er.SentFrom : "Undefined");
            SqlCommand.Parameters["@SendTo"].Value = (!string.IsNullOrEmpty(er.SendTo) ? er.SendTo : "Undefined");
            SqlCommand.Parameters["@Subject"].Value = (!string.IsNullOrEmpty(er.Subject) ? er.Subject : "Undefined");
            SqlCommand.Parameters["@Delivered"].Value = er.Delivered;
            SqlCommand.Parameters["@HasKeywordIssue"].Value = er.IsKeywordIssue;
            SqlCommand.Parameters["@HasAddressIssue"].Value = er.IsAddressIssue;

            if (er.UserId > 0) SqlCommand.Parameters["@UserId"].Value = er.UserId;
            if (er.AgentId > 0) SqlCommand.Parameters["@AgentId"].Value = er.AgentId;
            if (er.EmployeeId > 0) SqlCommand.Parameters["@EmployeeId"].Value = er.EmployeeId;
            if (er.I9Id > 0) SqlCommand.Parameters["@I9Id"].Value = er.I9Id;
            if (er.FutureEmployeeId > 0) SqlCommand.Parameters["@FutureEmployeeId"].Value = er.FutureEmployeeId;
            if (!string.IsNullOrEmpty(er.CopyTo)) SqlCommand.Parameters["@CopyTo"].Value = er.CopyTo;
            if (!string.IsNullOrEmpty(er.BlindCopyTo)) SqlCommand.Parameters["@BlindCopyTo"].Value = er.BlindCopyTo;
            if (!string.IsNullOrEmpty(er.MessageText)) SqlCommand.Parameters["@MessageText"].Value = er.MessageText;
            if (!string.IsNullOrEmpty(er.MessageHtml)) SqlCommand.Parameters["@MessageHtml"].Value = er.MessageHtml;
            
            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}