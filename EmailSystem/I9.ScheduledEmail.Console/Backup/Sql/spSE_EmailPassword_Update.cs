using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.ScheduledEmail.Console.Sql {

    internal class spSE_EmailPassword_Update : FdblSql {

        #region Constructors

        internal spSE_EmailPassword_Update(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spSE_EmailPassword_Update", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@AgentId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmployeeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@UserId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@Password", SqlDbType.VarChar, 40);
            param = SqlCommand.Parameters.Add("@PasswordSalt", SqlDbType.VarChar, 10);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idAgent, int idEmployee, int idUser, string password, string passwordSalt) {

            int ids = 0;

            if (idAgent > 0) ids++;
            if (idEmployee > 0) ids++;
            if (idUser > 0) ids++;

            if (ids == 0) throw new ArgumentNullException("At least one agent/employee/user id must be supplied");
            if (ids > 1) throw new ArgumentException("Only one agent/employee/user id can be supplied");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password is null/blank");
            if (string.IsNullOrEmpty(passwordSalt)) throw new ArgumentNullException("password salt is null/blank");

            ResetCommand();

            if (idAgent > 0) SqlCommand.Parameters["@AgentId"].Value = idAgent;
            if (idEmployee > 0) SqlCommand.Parameters["@EmployeeId"].Value = idEmployee;
            if (idUser > 0) SqlCommand.Parameters["@UserId"].Value = idUser;
            SqlCommand.Parameters["@Password"].Value = password;
            SqlCommand.Parameters["@PasswordSalt"].Value = passwordSalt;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}