using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.OnDemandEmail.Console.Sql {

    internal class spODE_XmlInfo_Build : FdblSql {

        #region Constructors

        internal spODE_XmlInfo_Build(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spODE_XmlInfo_Build", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmailOnDemandId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmailTemplateId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmailTemplateTypeId", SqlDbType.Int);

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idOnDemandEmail, int idEmailTemplate, int idEmailTemplateType) {

            if (idOnDemandEmail < 1) throw new ArgumentException("email on-demand id is invalid");
            if (idEmailTemplate < 1) throw new ArgumentException("email template id is invalid");
            if (idEmailTemplateType < 1) throw new ArgumentException("email template type id is invalid");

            ResetCommand();

            SqlCommand.Parameters["@EmailOnDemandId"].Value = idOnDemandEmail;
            SqlCommand.Parameters["@EmailTemplateId"].Value = idEmailTemplate;
            SqlCommand.Parameters["@EmailTemplateTypeId"].Value = idEmailTemplateType;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}