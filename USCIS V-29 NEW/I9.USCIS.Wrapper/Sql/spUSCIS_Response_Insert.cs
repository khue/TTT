using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.Sql {

    internal class spUSCIS_Response_Insert : FdblSql {

        #region Constructors

        public spUSCIS_Response_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo) {

            AutoConnect = true;

            OpenCommand("spUSCIS_Response_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;

            param = SqlCommand.Parameters.Add("@USCISRequestId", SqlDbType.BigInt);
            param = SqlCommand.Parameters.Add("@CaseNbr", SqlDbType.VarChar, 15);
            param = SqlCommand.Parameters.Add("@LastName", SqlDbType.VarChar, 40);
            param = SqlCommand.Parameters.Add("@FirstName", SqlDbType.VarChar, 25);
            param = SqlCommand.Parameters.Add("@EligStatementCd", SqlDbType.VarChar, 3);

            param = SqlCommand.Parameters.Add("@EligStatementTxt", SqlDbType.VarChar, 100);
            param = SqlCommand.Parameters.Add("@EligStatementDetailsTxt", SqlDbType.VarChar, 100);
            param = SqlCommand.Parameters.Add("@LetterTypeCode", SqlDbType.VarChar, 20);
            param = SqlCommand.Parameters.Add("@AdditionalPerfind", SqlDbType.Char, 1);
            param = SqlCommand.Parameters.Add("@ReturnStatus", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@ReturnStatusMsg", SqlDbType.VarChar, 4096);
            param = SqlCommand.Parameters.Add("@DHSResponse", SqlDbType.Char, 1);
            param = SqlCommand.Parameters.Add("@ResolveDate", SqlDbType.DateTime);
            param = SqlCommand.Parameters.Add("@UserField", SqlDbType.VarChar, 40);
            param = SqlCommand.Parameters.Add("@HadIssue", SqlDbType.Bit);
            param = SqlCommand.Parameters.Add("@PhotoIncluded", SqlDbType.VarChar, 1);
            param = SqlCommand.Parameters.Add("@NumberOfReferralReasons", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@ReferralReasons", SqlDbType.VarChar, 1000);

            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion

        #region Methods - Public

        public int StartDataReader(WebService.Response response) {

            ResetCommand();

            SqlCommand.Parameters["@USCISRequestId"].Value = response.USCISRequestId;

            if (!string.IsNullOrEmpty(response.CaseNbr)) SqlCommand.Parameters["@CaseNbr"].Value = response.CaseNbr;
            if (!string.IsNullOrEmpty(response.LastName)) SqlCommand.Parameters["@LastName"].Value = response.LastName;
            if (!string.IsNullOrEmpty(response.FirstName)) SqlCommand.Parameters["@FirstName"].Value = response.FirstName;
            if (!string.IsNullOrEmpty(response.EligStatementCd)) SqlCommand.Parameters["@EligStatementCd"].Value = response.EligStatementCd;
            if (!string.IsNullOrEmpty(response.EligStatementTxt)) SqlCommand.Parameters["@EligStatementTxt"].Value = response.EligStatementTxt;
            if (!string.IsNullOrEmpty(response.EligStatementDetailsTxt)) SqlCommand.Parameters["@EligStatementDetailsTxt"].Value = response.EligStatementDetailsTxt;                      
            if(!(response.LetterTypeCode==null || response.LetterTypeCode.Length==0)) {
                if (!string.IsNullOrEmpty(response.LetterTypeCode)) SqlCommand.Parameters["@LetterTypeCode"].Value = response.LetterTypeCode;
            }
            if (!string.IsNullOrEmpty(response.AdditionalPerfind)) SqlCommand.Parameters["@AdditionalPerfind"].Value = response.AdditionalPerfind;
            SqlCommand.Parameters["@ReturnStatus"].Value = 0;
            SqlCommand.Parameters["@ReturnStatusMsg"].Value = "SUCCCESSFUL";
            if (!string.IsNullOrEmpty(response.DHSResponse)) SqlCommand.Parameters["@DHSResponse"].Value = response.DHSResponse;


             //1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM. 


            if (IsValidSqlDateTimeNative(response.ResolveDate)) SqlCommand.Parameters["@ResolveDate"].Value = Convert.ToDateTime(response.ResolveDate);
            if (!string.IsNullOrEmpty(response.UserField)) SqlCommand.Parameters["@UserField"].Value = response.UserField;
            SqlCommand.Parameters["@HadIssue"].Value = response.HadIssue;
            if (!string.IsNullOrEmpty(response.PhotoIncluded)) SqlCommand.Parameters["@PhotoIncluded"].Value = response.PhotoIncluded;
            if (!string.IsNullOrEmpty(response.NumberOfReferralReasons.ToString())) SqlCommand.Parameters["@NumberOfReferralReasons"].Value = response.NumberOfReferralReasons;
            if (!string.IsNullOrEmpty(response.ReferralListArray)) SqlCommand.Parameters["@ReferralReasons"].Value = response.ReferralListArray;
            
            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        public int StartDataReader(WebService.Response response, System.Web.Services.Protocols.SoapException ex)
        {

            ResetCommand();

            SqlCommand.Parameters["@USCISRequestId"].Value = response.USCISRequestId;

            if (!string.IsNullOrEmpty(response.CaseNbr)) SqlCommand.Parameters["@CaseNbr"].Value = response.CaseNbr;
            if (!string.IsNullOrEmpty(response.LastName)) SqlCommand.Parameters["@LastName"].Value = response.LastName;
            if (!string.IsNullOrEmpty(response.FirstName)) SqlCommand.Parameters["@FirstName"].Value = response.FirstName;
            if (!string.IsNullOrEmpty(response.EligStatementCd)) SqlCommand.Parameters["@EligStatementCd"].Value = response.EligStatementCd;
            if (!string.IsNullOrEmpty(response.EligStatementTxt)) SqlCommand.Parameters["@EligStatementTxt"].Value = response.EligStatementTxt;
            if (!string.IsNullOrEmpty(response.EligStatementDetailsTxt)) SqlCommand.Parameters["@EligStatementDetailsTxt"].Value = response.EligStatementDetailsTxt;
            if (!(response.LetterTypeCode == null || response.LetterTypeCode.Length == 0))
            {
                if (!string.IsNullOrEmpty(response.LetterTypeCode)) SqlCommand.Parameters["@LetterTypeCode"].Value = response.LetterTypeCode;
            }
            if (!string.IsNullOrEmpty(response.AdditionalPerfind)) SqlCommand.Parameters["@AdditionalPerfind"].Value = response.AdditionalPerfind;
            if (!string.IsNullOrEmpty(ex.Code.Name)) SqlCommand.Parameters["@ReturnStatus"].Value = Convert.ToInt32(ex.Code.Name.Replace("-", ""));
            if (!string.IsNullOrEmpty(ex.Message)) SqlCommand.Parameters["@ReturnStatusMsg"].Value = ex.Message;
            if (!string.IsNullOrEmpty(response.DHSResponse)) SqlCommand.Parameters["@DHSResponse"].Value = response.DHSResponse;


            //1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM. 


            if (IsValidSqlDateTimeNative(response.ResolveDate)) SqlCommand.Parameters["@ResolveDate"].Value = Convert.ToDateTime(response.ResolveDate);
            if (!string.IsNullOrEmpty(response.UserField)) SqlCommand.Parameters["@UserField"].Value = response.UserField;
            SqlCommand.Parameters["@HadIssue"].Value = response.HadIssue;
            if (!string.IsNullOrEmpty(response.PhotoIncluded)) SqlCommand.Parameters["@PhotoIncluded"].Value = response.PhotoIncluded;
            if (!string.IsNullOrEmpty(response.NumberOfReferralReasons.ToString())) SqlCommand.Parameters["@NumberOfReferralReasons"].Value = response.NumberOfReferralReasons;
            if (!string.IsNullOrEmpty(response.ReferralListArray)) SqlCommand.Parameters["@ReferralReasons"].Value = response.ReferralListArray;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        public int StartDataReader(WebService.Response response, string CaseNbr)
        {

            ResetCommand();

            SqlCommand.Parameters["@USCISRequestId"].Value = response.USCISRequestId;

            if (!string.IsNullOrEmpty(CaseNbr)) SqlCommand.Parameters["@CaseNbr"].Value = CaseNbr;
            if (!string.IsNullOrEmpty(response.LastName)) SqlCommand.Parameters["@LastName"].Value = response.LastName;
            if (!string.IsNullOrEmpty(response.FirstName)) SqlCommand.Parameters["@FirstName"].Value = response.FirstName;
            if (!string.IsNullOrEmpty(response.EligStatementCd)) SqlCommand.Parameters["@EligStatementCd"].Value = response.EligStatementCd;
            if (!string.IsNullOrEmpty(response.EligStatementTxt)) SqlCommand.Parameters["@EligStatementTxt"].Value = response.EligStatementTxt;
            if (!string.IsNullOrEmpty(response.EligStatementDetailsTxt)) SqlCommand.Parameters["@EligStatementDetailsTxt"].Value = response.EligStatementDetailsTxt;
            if (!(response.LetterTypeCode == null || response.LetterTypeCode.Length == 0))
            {
                if (!string.IsNullOrEmpty(response.LetterTypeCode)) SqlCommand.Parameters["@LetterTypeCode"].Value = response.LetterTypeCode;
            }
            if (!string.IsNullOrEmpty(response.AdditionalPerfind)) SqlCommand.Parameters["@AdditionalPerfind"].Value = response.AdditionalPerfind;
            SqlCommand.Parameters["@ReturnStatus"].Value = 0;
            SqlCommand.Parameters["@ReturnStatusMsg"].Value = "SUCCCESSFUL";
            if (!string.IsNullOrEmpty(response.DHSResponse)) SqlCommand.Parameters["@DHSResponse"].Value = response.DHSResponse;


            //1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM. 


            if (IsValidSqlDateTimeNative(response.ResolveDate)) SqlCommand.Parameters["@ResolveDate"].Value = Convert.ToDateTime(response.ResolveDate);
            if (!string.IsNullOrEmpty(response.UserField)) SqlCommand.Parameters["@UserField"].Value = response.UserField;
            SqlCommand.Parameters["@HadIssue"].Value = response.HadIssue;
            if (!string.IsNullOrEmpty(response.PhotoIncluded)) SqlCommand.Parameters["@PhotoIncluded"].Value = response.PhotoIncluded;
            if (!string.IsNullOrEmpty(response.NumberOfReferralReasons.ToString())) SqlCommand.Parameters["@NumberOfReferralReasons"].Value = response.NumberOfReferralReasons;
            if (!string.IsNullOrEmpty(response.ReferralListArray)) SqlCommand.Parameters["@ReferralReasons"].Value = response.ReferralListArray;

            SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

            return Convert.ToInt32(OpenDataReader(true));

        }

        #endregion


        /// <summary>
        /// An better method to verify whether a value is 
        /// kosher for SQL Server datetime. This uses the native library
        /// for checking range values
        /// </summary>
        /// <param name="someval">A date string that may parse</param>
        /// <returns>true if the parameter is valid for SQL Sever datetime</returns>
        static bool IsValidSqlDateTimeNative(string someval)
        {
            bool valid = false;
            DateTime testDate = DateTime.MinValue;
            System.Data.SqlTypes.SqlDateTime sdt;
            if (DateTime.TryParse(someval, out testDate))
            {
                try
                {
                    // take advantage of the native conversion
                    sdt = new System.Data.SqlTypes.SqlDateTime(testDate);
                    valid = true;
                }
                catch (System.Data.SqlTypes.SqlTypeException ex)
                {

                    // no need to do anything, this is the expected out of range error
                }
            }

            return valid;
        }

    }

}