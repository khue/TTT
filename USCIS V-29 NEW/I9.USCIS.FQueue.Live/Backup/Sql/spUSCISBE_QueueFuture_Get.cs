using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

using I9.USCIS.Wrapper;

namespace I9.USCIS.FQueue.Live.Sql {

    internal class spUSCISBE_QueueFuture_Get : FdblSql {

        #region Constructors

        public spUSCISBE_QueueFuture_Get(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCISBE_QueueFuture_Get", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISSystemId", SqlDbType.Int);

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(USCISSystemId idSystem) {

            if (idSystem == USCISSystemId.Unknown) throw new ArgumentException("THe uscis system id is invalid");

            ResetCommand();

            SqlCommand.Parameters["@USCISSystemId"].Value = (int)idSystem;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion

    }

}