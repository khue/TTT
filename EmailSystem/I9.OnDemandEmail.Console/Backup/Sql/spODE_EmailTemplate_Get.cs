using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.OnDemandEmail.Console.Sql {

    internal class spODE_EmailTemplate_Get : FdblSql {

        #region Constructors

        internal spODE_EmailTemplate_Get(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spODE_EmailTemplate_Get", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmailTemplateId", SqlDbType.Int);

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idEmailTemplate) {

            if (idEmailTemplate == MyConsole.NullId) throw new ArgumentNullException("email template id is invalid");

            ResetCommand();

            SqlCommand.Parameters["@EmailTemplateId"].Value = idEmailTemplate;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}