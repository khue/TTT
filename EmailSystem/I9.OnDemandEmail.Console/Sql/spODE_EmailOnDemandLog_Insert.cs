using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.OnDemandEmail.Console.Sql {

    internal class spODE_EmailOnDemandLog_Insert : FdblSql {

        #region Constructors

        internal spODE_EmailOnDemandLog_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spODE_EmailOnDemandLog_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmailOnDemandId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmailQueueId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@Delivered", SqlDbType.Bit);
            param = SqlCommand.Parameters.Add("@Issue", SqlDbType.Bit);
            param = SqlCommand.Parameters.Add("@Message", SqlDbType.VarChar, 1024);
            param = SqlCommand.Parameters.Add("@Details", SqlDbType.Text);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(Items.EmailOnDemandLog eodl) {

            if (eodl == null) throw new ArgumentNullException("email on-demand log is null");

            ResetCommand();

            SqlCommand.Parameters["@EmailOnDemandId"].Value = eodl.EmailOnDemandId;
            SqlCommand.Parameters["@Delivered"].Value = (eodl.Delivered ? 1 : 0);
            SqlCommand.Parameters["@Issue"].Value = (eodl.Issue ? 1 : 0);

            if (eodl.EmailQueueId != MyConsole.NullId) SqlCommand.Parameters["@EmailQueueId"].Value = eodl.EmailQueueId;
            if (!string.IsNullOrEmpty(eodl.Message)) SqlCommand.Parameters["@Message"].Value = eodl.Message;
            if (!string.IsNullOrEmpty(eodl.Details)) SqlCommand.Parameters["@Details"].Value = eodl.Details;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}