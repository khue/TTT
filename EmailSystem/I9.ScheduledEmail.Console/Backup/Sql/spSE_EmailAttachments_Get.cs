using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.ScheduledEmail.Console.Sql {

    internal class spSE_EmailAttachments_Get : FdblSql {

        #region Constructors

        internal spSE_EmailAttachments_Get(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spSE_EmailAttachments_Get", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmailTemplateId", SqlDbType.Int);

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idEmailTemplate) {

            if (idEmailTemplate < 1) throw new ArgumentException("email template id is invalid");

            ResetCommand();

            SqlCommand.Parameters["@EmailTemplateId"].Value = idEmailTemplate;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}