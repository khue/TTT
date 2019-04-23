using System;
using System.Data.SqlClient;
using System.Data;
using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql
{
    internal class spUSCISPDFLetter_Insert : FdblSql
    {

        #region Constructors

        public spUSCISPDFLetter_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo)
        {
            AutoConnect = true;

            OpenCommand("spUSCISPDFLetter_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@PDF", SqlDbType.Image);
            param = SqlCommand.Parameters.Add("@CaseNumber", SqlDbType.VarChar, 30);
            param = SqlCommand.Parameters.Add("@LetterTypeCode", SqlDbType.VarChar, 20);
            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion


        #region Methods - Public

        public int StartDataReader(WebService.Response response)
        {
            ResetCommand();

            if (!string.IsNullOrEmpty(response.FAN.ToString())) SqlCommand.Parameters["@PDF"].Value = response.FAN;
            if (!string.IsNullOrEmpty(response.CaseNbr)) SqlCommand.Parameters["@CaseNumber"].Value = response.CaseNbr;
            if (!string.IsNullOrEmpty(response.LetterTypeCode)) SqlCommand.Parameters["@LetterTypeCode"].Value = response.LetterTypeCode;


            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));
           // return SqlCommand.ExecuteNonQuery();
        }

        public int StartDataReader(WebService.Response response, string CaseNumber, string LetterTypeCode)
        {
            ResetCommand();

            if (!string.IsNullOrEmpty(response.FAN.ToString())) SqlCommand.Parameters["@PDF"].Value = response.FAN;
            if (!string.IsNullOrEmpty(CaseNumber)) SqlCommand.Parameters["@CaseNumber"].Value = CaseNumber;
            if (!string.IsNullOrEmpty(LetterTypeCode)) SqlCommand.Parameters["@LetterTypeCode"].Value = LetterTypeCode;


            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));
            // return SqlCommand.ExecuteNonQuery();
        }

        #endregion

    }
}
