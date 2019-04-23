using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.ScheduledEmail.Console.Sql {

    internal class spSE_EmailJob_Insert : FdblSql {

        #region Constructors

        internal spSE_EmailJob_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spSE_EmailJob_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@TemplateTime", SqlDbType.Char, 5);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(string timeEmailTemplateSchedule) {

            if (string.IsNullOrEmpty(timeEmailTemplateSchedule)) throw new ArgumentNullException("email template schedule time is null/blank");

            ResetCommand();

            SqlCommand.Parameters["@TemplateTime"].Value = timeEmailTemplateSchedule;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}