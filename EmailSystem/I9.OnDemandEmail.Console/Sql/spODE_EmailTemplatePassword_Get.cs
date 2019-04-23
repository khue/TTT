using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.OnDemandEmail.Console.Sql {

    internal class spODE_EmailTemplatePassword_Get : FdblSql {

        #region Constructors

        internal spODE_EmailTemplatePassword_Get(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spODE_EmailTemplatePassword_Get", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@EmailTemplateTypeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@CompanyId", SqlDbType.Int);

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idEmailTemplateType, int idCompany) {

            if (idEmailTemplateType < 1) throw new ArgumentException("email template type id is invalid");
            if (idCompany < 1) throw new ArgumentException("email template type id is invalid");

            ResetCommand();

            SqlCommand.Parameters["@EmailTemplateTypeId"].Value = idEmailTemplateType;
            SqlCommand.Parameters["@CompanyId"].Value = idCompany;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}