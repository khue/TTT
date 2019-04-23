﻿using System;
using System.Data;
using System.Data.SqlClient;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.XQueue.Test.Sql
{
    internal class spUSCIS_OnDemandEmail_Insert : FdblSql {

        #region Constructors

        public spUSCIS_OnDemandEmail_Insert(string sqlConnectionInfo) : base(sqlConnectionInfo)
        {

            AutoConnect = true;

            OpenCommand("spUSCIS_OnDemandEmail_Insert", CommandType.StoredProcedure, true);

            SqlParameter param = SqlCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            param.Direction = ParameterDirection.ReturnValue;
                        
            param = SqlCommand.Parameters.Add("@EmailTemplateTypeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@EmployeeId", SqlDbType.Int);
            param = SqlCommand.Parameters.Add("@i9Id", SqlDbType.Int);
            
            param = SqlCommand.Parameters.Add("@SqlErrorCode", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

        }

        #endregion


        #region Methods - Public

        public int StartDataReader(int idEmailTemplateType, int idEmployee, int idI9)
        {
           
            if ((idEmailTemplateType < 8 || idEmailTemplateType > 55) && idEmailTemplateType != -1) throw new ArgumentException("Email Template Type is invalid. Type: " + idEmailTemplateType);
            if (idEmployee < 1) throw new ArgumentException("Employee ID is invalid");
            if (idI9 < 1) throw new ArgumentException("I9 ID is invalid");

            /*
             * Added by Kevin Hue 7/17/09 - Do not log if emailtemplatetype is -1
             */
            if (idEmailTemplateType == -1)
            {
                return FdblSqlReturnCodes.Success;
            }
            else
            {
                ResetCommand();
                
                SqlCommand.Parameters["@EmailTemplateTypeId"].Value = idEmailTemplateType;
                SqlCommand.Parameters["@EmployeeId"].Value = idEmployee;
                SqlCommand.Parameters["@i9Id"].Value = idI9;

                SqlCommand.Parameters["@SqlErrorCode"].Value = 0;

                return Convert.ToInt32(OpenDataReader(true));
            }
        }

        #endregion

    }
} //end namespace
