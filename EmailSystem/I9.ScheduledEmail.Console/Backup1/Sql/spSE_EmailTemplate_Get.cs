using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.ScheduledEmail.Console.Sql {

    internal class spSE_EmailTemplate_Get : FdblSql {

        #region Constructors

        internal spSE_EmailTemplate_Get(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spSE_EmailTemplate_Get", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@TemplateTime", SqlDbType.VarChar, 5);

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(string timeEmailSchedule) {

            if (string.IsNullOrEmpty(timeEmailSchedule)) throw new ArgumentNullException("email schedule time is null/blank");

            ResetCommand();

            SqlCommand.Parameters["@TemplateTime"].Value = timeEmailSchedule;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}