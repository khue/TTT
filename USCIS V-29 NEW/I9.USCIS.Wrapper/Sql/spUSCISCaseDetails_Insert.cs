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
    internal class spUSCISCaseDetails_Insert : FdblSql
    {

        #region Constructors

        public spUSCISCaseDetails_Insert(string sqlConnectionInfo)
            : base(sqlConnectionInfo)
        {
            AutoConnect = true;

            OpenCommand("spUSCISCaseDetails_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;


            param = SqlCommand.Parameters.Add("@Type", SqlDbType.NVarChar, 50);
            param = SqlCommand.Parameters.Add("@CaseNumber", SqlDbType.VarChar, 50);
            param = SqlCommand.Parameters.Add("@XMLObject", SqlDbType.NVarChar,-1);
            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion


        #region Methods - Public

        public int StartDataReader(String CaseNbr,StringWriter swCaseDetails, String caseDetailsType)
        {
            ResetCommand();


            if (!string.IsNullOrEmpty(caseDetailsType)) SqlCommand.Parameters["@Type"].Value = caseDetailsType;
            if (!string.IsNullOrEmpty(CaseNbr)) SqlCommand.Parameters["@CaseNumber"].Value = CaseNbr;
            if (!string.IsNullOrEmpty(swCaseDetails.ToString())) SqlCommand.Parameters["@XMLObject"].Value = swCaseDetails.ToString();

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));
           // return SqlCommand.ExecuteNonQuery();
        }

        #endregion
        
    }
}
