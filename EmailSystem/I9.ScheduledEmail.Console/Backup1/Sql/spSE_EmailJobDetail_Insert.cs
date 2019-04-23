using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.ScheduledEmail.Console.Sql {

    internal class spSE_EmailJobDetail_Insert : FdblSql {

        #region Constructors

        internal spSE_EmailJobDetail_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spSE_EmailJobDetail_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmailJobId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmailTemplateId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@DTStart", SqlDbType.DateTime);
            param = SqlCommand.Parameters.Add("@DTFinish", SqlDbType.DateTime);
            param = SqlCommand.Parameters.Add("@Clients", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@Attachments", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@Delivered", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@Issues", SqlDbType.Int);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(Items.JobDetail jd) {

            if (jd == null) throw new ArgumentNullException("job detail is null");

            ResetCommand();

            SqlCommand.Parameters["@EmailJobId"].Value = jd.EmailJobId;
            SqlCommand.Parameters["@EmailTemplateId"].Value = jd.EmailTemplateId;
            SqlCommand.Parameters["@DTStart"].Value = jd.DTStart;
            SqlCommand.Parameters["@DTFinish"].Value = jd.DTFinish;
            SqlCommand.Parameters["@Clients"].Value = jd.Clients;
            SqlCommand.Parameters["@Attachments"].Value = jd.Attachments;
            SqlCommand.Parameters["@Delivered"].Value = jd.Delivered;
            SqlCommand.Parameters["@Issues"].Value = jd.Issues;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}