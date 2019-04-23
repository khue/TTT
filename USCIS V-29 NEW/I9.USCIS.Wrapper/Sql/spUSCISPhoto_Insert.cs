using System;
using System.Data.SqlClient;
using System.Data;
using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql
{
    internal class spUSCISPhoto_Insert : FdblSql
    {

        #region Constructors

        public spUSCISPhoto_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo)
        {
            AutoConnect = true;

            OpenCommand("spUSCISPhoto_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@Photo", SqlDbType.Image);
            param = SqlCommand.Parameters.Add("@CaseNumber", SqlDbType.VarChar, 30);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion


        #region Methods - Public

        public int StartDataReader(WebService.Response response)
        {
            ResetCommand();

            if (!string.IsNullOrEmpty(response.Photo.ToString())) SqlCommand.Parameters["@Photo"].Value = response.Photo;
            if (!string.IsNullOrEmpty(response.CaseNbr)) SqlCommand.Parameters["@CaseNumber"].Value = response.CaseNbr;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));
           // return SqlCommand.ExecuteNonQuery();
        }

        public int StartDataReader(WebService.Response response, string CaseNbr)
        {
            ResetCommand();

            if (!string.IsNullOrEmpty(response.Photo.ToString())) SqlCommand.Parameters["@Photo"].Value = response.Photo;
            if (!string.IsNullOrEmpty(CaseNbr)) SqlCommand.Parameters["@CaseNumber"].Value = CaseNbr;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));
            // return SqlCommand.ExecuteNonQuery();
        }

        #endregion

    }
}
