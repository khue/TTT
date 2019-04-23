using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.ScheduledEmail.Debug.Sql {

    internal class DEBUG_spSE_XmlInfo_Build : FdblSql {

        #region Constructors

        internal DEBUG_spSE_XmlInfo_Build(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            //OpenCommand("DEBUG_spSE_XmlInfo_Build", CommandType.StoredProcedure, true);
            OpenCommand("spSE_XmlInfo_Build", CommandType.StoredProcedure, true);
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