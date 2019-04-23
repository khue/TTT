using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.EQueue.Test.Sql {

    internal class spUSCISBE_QueueError_Reset : FdblSql {

        #region Constructors

        public spUSCISBE_QueueError_Reset(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCISBE_QueueError_Reset", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISQueueErrorId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@Win32Handle", SqlDbType.BigInt);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(int idQueueError, int idWin32) {

            if (idQueueError == MyConsole.NullId) throw new ArgumentException("The uscis error queue id is invalid");
            if (idWin32 == MyConsole.NullId) throw new ArgumentException("The win32 id is invalid");

            ResetCommand();

            SqlCommand.Parameters["@USCISQueueErrorId"].Value = idQueueError;
            SqlCommand.Parameters["@Win32Handle"].Value = idWin32;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}