using System;
using System.Data.SqlClient;
using System.Data;
using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;
using System.Xml.Serialization;
using System.IO;
using I9.USCIS.Wrapper.WebService;

namespace I9.USCIS.Wrapper.Sql
{
    internal class spUSCISDuplicateCases_Insert : FdblSql
    {

        #region Constructors

        public spUSCISDuplicateCases_Insert(string sqlConnectionInfo)
            : base(sqlConnectionInfo)
        {
            AutoConnect = true;

            OpenCommand("spUSCISDuplicateCases_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;


            param = SqlCommand.Parameters.Add("@CaseNumber", SqlDbType.VarChar, 50);
            param = SqlCommand.Parameters.Add("@CaseList", SqlDbType.NVarChar,-1);
            param = SqlCommand.Parameters.Add("@I9ID", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion


        #region Methods - Public

        public int StartDataReader(String CaseNbr,string swCaseDetails, int I9Id)
        {
            ResetCommand();

            if (!string.IsNullOrEmpty(CaseNbr)) SqlCommand.Parameters["@CaseNumber"].Value = CaseNbr;
            if (!string.IsNullOrEmpty(swCaseDetails.ToString())) SqlCommand.Parameters["@CaseList"].Value = swCaseDetails.ToString();
            if (!string.IsNullOrEmpty(I9Id.ToString())) SqlCommand.Parameters["@I9ID"].Value = I9Id;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));
           // return SqlCommand.ExecuteNonQuery();
        }

        #endregion
        
    }
}
