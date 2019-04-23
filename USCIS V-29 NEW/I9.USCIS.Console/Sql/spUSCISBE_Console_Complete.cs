using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Console.Sql {

    internal class spUSCISBE_Console_Complete : FdblSql {

        #region Constructors

        public spUSCISBE_Console_Complete(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCISBE_Console_Complete", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISConsoleId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@Win32Handle", SqlDbType.BigInt);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idConsole, int idWin32) {

            if (idConsole == MyConsole.NullId) throw new ArgumentException("The uscis console id is invalid");
            if (idWin32 == MyConsole.NullId) throw new ArgumentException("The win32 id is invalid");

            ResetCommand();

            SqlCommand.Parameters["@USCISConsoleId"].Value = idConsole;
            SqlCommand.Parameters["@Win32Handle"].Value = idWin32;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}