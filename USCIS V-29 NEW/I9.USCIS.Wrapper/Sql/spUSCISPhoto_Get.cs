using System;
using System.Data.SqlClient;
using System.Data;
using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql
{
    internal class spUSCISPhoto_Get : FdblSql
    {

        #region Constructors

        public spUSCISPhoto_Get(string sqlConnectionInfo) : base(sqlConnectionInfo)
        {
            AutoConnect = true;

            OpenCommand("spUSCISPhoto_Get", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@I9Id", SqlDbType.Int);
            
        }

        #endregion


        #region Methods - Public

        public int StartDataReader(int I9Id)
        {

            if (I9Id < 1) throw new ArgumentException("invalid i9 id");
            ResetCommand();

            SqlCommand.Parameters["@I9Id"].Value = I9Id;
            
            return Convert.ToInt32(OpenDataReader(true));
        }

        #endregion
        
    }
}
