using System;
using System.Text;
using System.Web.Services.Protocols;
using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Wrapper.WebService {

    public abstract class Request {

        #region Fields

        public static readonly int BypassId = -99;
        public static readonly int NullId = -1;

        #endregion

        #region Members

        protected USCISConfiguration _Configuration = null;
        protected ProcessIds _Pids = null;
        protected string _Version = null;

        #endregion

        #region Properties - Public

        public USCISConfiguration Configuration { get { return _Configuration; } }
        public ProcessIds Pids { get { return _Pids; } }
        public string Version { get { return _Version; } }

        public int EmployeeId {
            get { return _Pids.EmployeeId; }
            set { _Pids.EmployeeId = value; }
        }

        public int I9Id {
            get { return _Pids.I9Id; }
            set { _Pids.I9Id = value; }
        }

        public int QueueErrorId {
            get { return _Pids.QueueErrorId; }
            set { _Pids.QueueErrorId = value; }
        }

        public int QueueFutureId {
            get { return _Pids.QueueFutureId; }
            set { _Pids.QueueFutureId = value; }
        }

        public int TransactionId {
            get { return _Pids.TransactionId; }
            set { _Pids.TransactionId = value; }
        }

        public string CaseNumber {
            get { return _Pids.CaseNumber; }
            set { _Pids.CaseNumber = value; }
        }

        #endregion

        #region Methods - Public (Static - File Logging)

        public static string GetRequestXmlLog(ProcessIds pids, string rawData) {

            return string.Format("<Request DateTime=\"{0}\" EmployeeId=\"{1}\" I9Id=\"{2}\" MethodName=\"{3}\">{4}</Request>", DateTime.Now, pids.EmployeeId, pids.I9Id, Convert.ToString(pids.MethodId), rawData);

        }

        public static string GetResponseXmlLog(Response response) {

            if (response == null) return string.Format("<Response DateTime=\"{0}\" CaseNbr=\"\" LastName=\"\" FirstName=\"\" EligStatementCd=\"\" EligStatementTxt=\"\" EligStatementDetailsTxt=\"\" AdditionalPerfind=\"\" ReturnStatus=\"\" ReturnStatusMsg=\"\" DHSResponse=\"\" ResolveDate=\"\" UserField=\"\" HadIssue=\"1\"/>", DateTime.Now);

            return string.Format("<Response DateTime=\"{0}\" CaseNbr=\"{1}\" LastName=\"{2}\" FirstName=\"{3}\" EligStatementCd=\"{4}\" EligStatementTxt=\"{5}\" EligStatementDetailsTxt=\"{6}\" AdditionalPerfind=\"{7}\" ReturnStatus=\"{8}\" ReturnStatusMsg=\"{9}\" DHSResponse=\"{10}\" ResolveDate=\"{11}\" UserField=\"{12}\" HadIssue=\"{13}\"/>", DateTime.Now, response.CaseNbr, response.LastName, response.FirstName, response.EligStatementCd, response.EligStatementTxt, response.EligStatementDetailsTxt, response.AdditionalPerfind, response.ReturnStatus, response.ReturnStatusMsg, response.DHSResponse, response.ResolveDate, response.UserField, response.HadIssue);

        }

        #endregion

        #region Methods - Public (USCISLog)

        public void Capture(LogLevel logLevel, string message) {

            Capture(logLevel, null, null, message, null);

        }

        public void Capture(LogLevel logLevel, string message, Exception ex) {

            Capture(logLevel, null, null, message, ex);

        }

        public void Capture(LogLevel logLevel, string classModule, string message) {

            Capture(logLevel, classModule, null, message, null);

        }

        public void Capture(LogLevel logLevel, string classModule, string message, Exception ex) {

            Capture(logLevel, null, null, message, ex);

        }

        public void Capture(LogLevel logLevel, string classModule, string friendlyMessage, string message, Exception ex) {

            Sql.spUSCIS_Log_Insert spLogInsert = new Sql.spUSCIS_Log_Insert(_Configuration.SqlFactory.GetConnectionString());

            try {

                string details = (ex != null) ? FdblExceptions.GetDetails(ex) : null;

                int spReturnCode = spLogInsert.StartDataReader(_Pids, logLevel, classModule, friendlyMessage, message, details);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Log_Insert returned: {0} (Sql returned: {1})", spReturnCode, spLogInsert.SqlErrorCode));

            } catch (Exception iex) {

                Xml.LogWriter.WriteEntry(_Pids, logLevel, classModule, friendlyMessage, message, ex, iex, _Configuration.Logs.Capture);

            } finally {

                if (spLogInsert != null) spLogInsert.Dispose();

            }

        }

        #endregion

        #region Methods - Public (Gets)

        public string GetClosureCode(USCISClosureId idClosure) {

            Sql.spUSCIS_Closure_Get spClosureGet = new Sql.spUSCIS_Closure_Get(_Configuration.SqlFactory.GetConnectionString());

            try {

                int spReturnCode = spClosureGet.StartDataReader((int)idClosure);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Closure_Get: {0} (Sql returned: {1})", spReturnCode, spClosureGet.SqlErrorCode));

                if (!spClosureGet.MoveNextDataReader(true)) throw new USCISException("spUSCIS_Closure_Get could not advance cursor");

                string tmp = Convert.ToString(spClosureGet.GetDataReaderValue(0, string.Empty));

                if (FdblStrings.IsBlank(tmp)) throw new USCISException("spUSCIS_Closure_Get could not determine the closure code for the given id");

                return tmp;

            } finally {

                if (spClosureGet != null) spClosureGet.Dispose();

            }

        }

        
        public Employee.LocalCase GetLocalEmployeeCase() {

            Sql.spUSCIS_EmployeeI9_GetDetails spEmployeeI9GetDetails = new Sql.spUSCIS_EmployeeI9_GetDetails(_Configuration.SqlFactory.GetConnectionString());

            try {

                int spReturnCode = spEmployeeI9GetDetails.StartDataReader(_Pids.EmployeeId, _Pids.I9Id);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_EmployeeI9_GetDetails returned: {0} (Sql returned: {1})", spReturnCode, spEmployeeI9GetDetails.SqlErrorCode));

                if (!spEmployeeI9GetDetails.MoveNextDataReader(true)) throw new USCISException("spUSCIS_EmployeeI9_GetDetails could not advance cursor");

                return new Employee.LocalCase(spEmployeeI9GetDetails);

            } finally {

                if (spEmployeeI9GetDetails != null) spEmployeeI9GetDetails.Dispose();

            }

        }

        public Employee.UploadedPhoto GetPhoto()
        {

            Sql.spUSCISPhoto_Get spPhotoGet = new Sql.spUSCISPhoto_Get(_Configuration.SqlFactoryAlt.GetConnectionString());

            try
            {
                int spReturnCode = spPhotoGet.StartDataReader(_Pids.I9Id);
                
                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCISPhoto_Get returned: {0} (Sql returned: {1})", spReturnCode, spPhotoGet.SqlErrorCode));

                if (!spPhotoGet.MoveNextDataReader(true)) throw new USCISException("spUSCISPhoto_Get could not advance cursor");

                return new Employee.UploadedPhoto(spPhotoGet);

            }
            finally
            {
                if (spPhotoGet != null) spPhotoGet.Dispose();
            }

        }// end GetPhoto()

        public bool GetSystemIdentities() {

            _Pids.EmployeeId = Request.NullId;
            _Pids.I9Id = Request.NullId;

            Sql.spUSCIS_EmployeeI9_GetIdentities spEmployeeI9GetIdentities = new Sql.spUSCIS_EmployeeI9_GetIdentities(_Configuration.SqlFactory.GetConnectionString());

            try {

                int spReturnCode = spEmployeeI9GetIdentities.StartDataReader(_Pids.CaseNumber);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_EmployeeI9_GetIdentities: {0}", spReturnCode));

                if (!spEmployeeI9GetIdentities.MoveNextDataReader(true)) throw new USCISException("spUSCIS_EmployeeI9_GetIdentities could not advance cursor");

                _Pids.TransactionId = Convert.ToInt32(spEmployeeI9GetIdentities.GetDataReaderValue(0, Request.NullId));
                _Pids.EmployeeId = Convert.ToInt32(spEmployeeI9GetIdentities.GetDataReaderValue(1, Request.NullId));
                _Pids.I9Id = Convert.ToInt32(spEmployeeI9GetIdentities.GetDataReaderValue(2, Request.NullId));

                if (_Pids.EmployeeId == Request.NullId) throw new USCISException("spUSCIS_EmployeeI9_GetIdentities could not determine the sql identity of the employee");
                if (_Pids.I9Id == Request.NullId) throw new USCISException("spUSCIS_EmployeeI9_GetIdentities could not determine the sql identity of the I9");

                return true;

            } catch (Exception ex) {

                _Pids.NextCategoryCallIsInternal = true;

                Capture(LogLevel.error, "Request::GetSystemIdentities", null, ex.Message, ex);

                _Pids.EmployeeId = Request.BypassId;
                _Pids.I9Id = Request.BypassId;

                return false;

            } finally {

                if (spEmployeeI9GetIdentities != null) spEmployeeI9GetIdentities.Dispose();

            }

        }

        public int GetTransactionId() {

            if (_Pids.MethodName.Equals("InitiateCase", StringComparison.OrdinalIgnoreCase)) {

                try {

                    return InsertTransaction();

                } catch { }

            } else {

                if (string.IsNullOrEmpty(_Pids.CaseNumber)) throw new USCISException("The case number is null/blank");

                try {

                    return _GetTransactionIdByCase(_Pids.CaseNumber);

                } catch (Exception ex) {

                    Capture(LogLevel.error, string.Format("Request::{0}", _Pids.MethodName), null, "transaction id (by case number)", ex);

                }

            }

            return Request.NullId;

        }

        #endregion

        #region Methods - Public (Link)

        public void InsertLink() {

            Sql.spUSCIS_ConsoleLink_Insert spConsoleLinkInsert = new Sql.spUSCIS_ConsoleLink_Insert(_Configuration.SqlFactory.GetConnectionString());

            try {

                int spReturnCode = spConsoleLinkInsert.StartDataReader(_Pids);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_ConsoleLink_Insert returned: {0} (Sql returned: {1})", spReturnCode, spConsoleLinkInsert.SqlErrorCode));

            } finally {

                if (spConsoleLinkInsert != null) spConsoleLinkInsert.Dispose();

            }

        }

        #endregion

        #region Methods - Protected (Queue)

        protected bool QueueAsError(USCISActionId idAction, USCISClosureId idClosure, string comments, string rawData, string idErrorRequests, Response resp, Exception ex) {

            if (ex == null) Capture(LogLevel.error, string.Format("Request::{0}", _Pids.MethodName), null, "Unable to perform requested action", null);
            else Capture(LogLevel.error, string.Format("Request::{0}", _Pids.MethodName), null, ex.Message, ex);

            /*
             *Modified by Kevin Hue 
             *Modified Date : 6/10/2009
             *Summary : Updated to set CanBeProcessed column in USCISQueueError table to true based
             *on response status from Everify. Currently, added only status : -1001, -1033, -1062
             * */
            bool CanBeProcessed;

            try
            {

                switch (resp.ReturnStatus)
                {
                    //System error submitting initial verification    
                    case -1001:
                        CanBeProcessed = false;
                        break;

                    //Hiredate is null
                    case -1033:
                        CanBeProcessed = true;
                        break;

                    //Hiredate must be between 11/1/1997 and current date
                    case -1062:
                        CanBeProcessed = true;
                        break;

                    default:
                        CanBeProcessed = false;
                        break;
                }// end switch
            }// end try
            catch (Exception e)
            {

                Capture(LogLevel.error, "Request::QueueAsError ", null, e.Message, e);
                return false;
            }// end catch

            if (!_QueueAsError(idAction, idClosure, comments, CanBeProcessed)) {

                Xml.ProcessWriter.WriteEntry(_Pids.TransactionId, GetRequestXmlLog(_Pids, rawData), GetResponseXmlLog(resp), _Configuration.Logs.Process);

            }

            return true;

        }

        protected bool QueueAsFuture(string rawData) {

            Capture(LogLevel.error, string.Format("Request::{0}", _Pids.MethodName), null, "This is a future hire and is being queued until the appropriate date", null);

            if (!_QueueAsFuture()) {

                Xml.ProcessWriter.WriteEntry(_Pids.TransactionId, GetRequestXmlLog(_Pids, rawData), GetResponseXmlLog(null), _Configuration.Logs.Process);

            }

            return true;

        }

        #endregion

        #region Methods - Protected (EmailOnDemand)
        /*
         4/29/09 Kevin Hue - Method to use spUSCIS_OnDemandEmail_Insert class for inserting record into 
         * EmailOnDemand table. Email will be sent by I9.OnDemandEmail.Console.
         * Purpose is to notify recipients based on everify response.
         */
        protected bool CheckAndSendEmail(int idEmailTemplateType, int idEmployee, int idI9)
        {
            Sql.spUSCIS_OnDemandEmail_Insert spOnDemandEmailInsert = new Sql.spUSCIS_OnDemandEmail_Insert(_Configuration.SqlFactory.GetConnectionString());

            try
            {
                int spReturnCode = spOnDemandEmailInsert.StartDataReader(idEmailTemplateType, idEmployee, idI9);
                //10-30-2014 KH - removed logging for this to minimize logging. this inserts a log for all clients who do not have emails setup and bloats uscislog, burying meaningful log entries
                //if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_OnDemandEmail_Insert: {0} (Sql returned: {1}) ", spReturnCode, spOnDemandEmailInsert.SqlErrorCode));
                                
                return true;

            }// end try

            catch (Exception ex)
            {

                Capture(LogLevel.error, "Request::EmailOnDemand", null, ex.Message, ex);
                return false;
            }// end catch

            finally
            {

                if (spOnDemandEmailInsert != null) spOnDemandEmailInsert.Dispose();

            }// end finally
                        
        }// end CheckAndSendEmail

        #endregion

        #region Methods - Protected (Validation)

        protected bool ValidateParameters(USCISRequestValidation rv) {

            try {

                if (((rv & USCISRequestValidation.EmployeeId) == USCISRequestValidation.EmployeeId) && (_Pids.EmployeeId == Request.NullId)) throw new ArgumentException("invalid employee id");
                if (((rv & USCISRequestValidation.I9Id) == USCISRequestValidation.I9Id) && (_Pids.I9Id == Request.NullId)) throw new ArgumentException("invalid i9 id");
                if (((rv & USCISRequestValidation.CaseNumber) == USCISRequestValidation.CaseNumber) && (string.IsNullOrEmpty(_Pids.CaseNumber))) throw new ArgumentException("case number is null/blank");

                return true;

            } catch (Exception ex) {

                _Pids.NextCategoryCallIsInternal = true;

                Capture(LogLevel.error, string.Format("Request::{0}", _Pids.MethodName), null, ex.Message, ex);

                return false;

            }

        }

        protected bool ValidateTransaction(string rawData) {

            if (_Pids.TransactionId != Request.NullId) return true;

            Xml.ProcessWriter.WriteEntry(_Pids.TransactionId, GetRequestXmlLog(_Pids, rawData), GetResponseXmlLog(null), _Configuration.Logs.Process);

            return false;

        }

        protected bool ValidateTransaction(USCISActionId idAction, USCISClosureId idClosure, string comments, string rawData) {

            if (_Pids.TransactionId != Request.NullId) return true;

            if (!_QueueAsError(idAction, idClosure, comments, false)) {

                Xml.ProcessWriter.WriteEntry(_Pids.TransactionId, GetRequestXmlLog(_Pids, rawData), GetResponseXmlLog(null), _Configuration.Logs.Process);

            }

            return false;

        }

        #endregion

        #region Methods - Protected (Request/Response/Transaction)

        protected int InsertRequest(int attempt, string rawData) {

            Sql.spUSCIS_Request_Insert spRequestInsert = new Sql.spUSCIS_Request_Insert(_Configuration.SqlFactory.GetConnectionString());

            try {

                int spReturnCode = spRequestInsert.StartDataReader(_Pids.TransactionId, (int)_Pids.CategoryId, (int)_Pids.SystemId, _Pids.EmployeeId, _Pids.I9Id, attempt, Convert.ToString(_Pids.MethodId), rawData);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Request_Insert returned: {0} (Sql returned: {1})", spReturnCode, spRequestInsert.SqlErrorCode));

                if (!spRequestInsert.MoveNextDataReader(true)) throw new USCISException("spUSCIS_Request_Insert could not advance cursor");

                int id = Convert.ToInt32(spRequestInsert.GetDataReaderValue(0, -1));

                if (id == -1) throw new USCISException("spUSCIS_Request_Insert could not determine the sql identity of the new record");

                return id;

            } finally {

                if (spRequestInsert != null) spRequestInsert.Dispose();

            }

        }

        protected int InsertResponse(Response response) {

            Sql.spUSCIS_Response_Insert spResponseInsert = new Sql.spUSCIS_Response_Insert(_Configuration.SqlFactory.GetConnectionString());

            try {

                int spReturnCode = spResponseInsert.StartDataReader(response);
                
                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Response_Insert returned: {0} (Sql returned: {1})", spReturnCode, spResponseInsert.SqlErrorCode));

                if (!spResponseInsert.MoveNextDataReader(true)) throw new USCISException("spUSCIS_Response_Insert could not advance cursor");

                int id = Convert.ToInt32(spResponseInsert.GetDataReaderValue(0, -1));

                if (id == -1) throw new USCISException("spUSCIS_Response_Insert could not determine the sql identity of the new record");
                            
                return id;

            } finally {

                if (spResponseInsert != null) spResponseInsert.Dispose();

            }
                     
        }

        protected int InsertResponse(Response response, string CaseNbr)
        {

            Sql.spUSCIS_Response_Insert spResponseInsert = new Sql.spUSCIS_Response_Insert(_Configuration.SqlFactory.GetConnectionString());

            try
            {

                int spReturnCode = spResponseInsert.StartDataReader(response, CaseNbr);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Response_Insert returned: {0} (Sql returned: {1})", spReturnCode, spResponseInsert.SqlErrorCode));

                if (!spResponseInsert.MoveNextDataReader(true)) throw new USCISException("spUSCIS_Response_Insert could not advance cursor");

                int id = Convert.ToInt32(spResponseInsert.GetDataReaderValue(0, -1));

                if (id == -1) throw new USCISException("spUSCIS_Response_Insert could not determine the sql identity of the new record");

                return id;

            }
            finally
            {

                if (spResponseInsert != null) spResponseInsert.Dispose();

            }

        }

        protected int InsertResponse(Response response, SoapException ex)
        {

            Sql.spUSCIS_Response_Insert spResponseInsert = new Sql.spUSCIS_Response_Insert(_Configuration.SqlFactory.GetConnectionString());

            try
            {

                int spReturnCode = spResponseInsert.StartDataReader(response, ex);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Response_Insert returned: {0} (Sql returned: {1})", spReturnCode, spResponseInsert.SqlErrorCode));

                if (!spResponseInsert.MoveNextDataReader(true)) throw new USCISException("spUSCIS_Response_Insert could not advance cursor");

                int id = Convert.ToInt32(spResponseInsert.GetDataReaderValue(0, -1));

                if (id == -1) throw new USCISException("spUSCIS_Response_Insert could not determine the sql identity of the new record");

                return id;

            }
            finally
            {

                if (spResponseInsert != null) spResponseInsert.Dispose();

            }

        }

        protected void InsertPhoto(Response response)
        {

            Sql.spUSCISPhoto_Insert spPhotoInsert = new Sql.spUSCISPhoto_Insert(_Configuration.SqlFactoryAlt.GetConnectionString());

            try
            {
                int spReturnCode = spPhotoInsert.StartDataReader(response);
                
                
                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCISPhoto_Insert returned: {0} (Sql returned: {1})", spReturnCode, spPhotoInsert.SqlErrorCode));
                               

            }
            finally
            {
                if (spPhotoInsert != null) spPhotoInsert.Dispose();
            }

            
        }//end insert photo

        protected void InsertPhoto(Response response, string CaseNbr)
        {

            Sql.spUSCISPhoto_Insert spPhotoInsert = new Sql.spUSCISPhoto_Insert(_Configuration.SqlFactoryAlt.GetConnectionString());

            try
            {
                int spReturnCode = spPhotoInsert.StartDataReader(response, CaseNbr);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCISPhoto_Insert returned: {0} (Sql returned: {1})", spReturnCode, spPhotoInsert.SqlErrorCode));


            }
            finally
            {
                if (spPhotoInsert != null) spPhotoInsert.Dispose();
            }


        }//end insert photo


        protected void InsertPDFLetter(Response response)
        {

            Sql.spUSCISPDFLetter_Insert spPDFLetterInsert = new Sql.spUSCISPDFLetter_Insert(_Configuration.SqlFactoryAlt.GetConnectionString());

            try
            {
                int spReturnCode = spPDFLetterInsert.StartDataReader(response);


                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCISPDFLetter_Insert returned: {0} (Sql returned: {1})", spReturnCode, spPDFLetterInsert.SqlErrorCode));


            }
            finally
            {
                if (spPDFLetterInsert != null) spPDFLetterInsert.Dispose();
            }


        }//end PDF letter

        protected void InsertPDFLetter(Response response, string CaseNbr, string LetterCode)
        {

            Sql.spUSCISPDFLetter_Insert spPDFLetterInsert = new Sql.spUSCISPDFLetter_Insert(_Configuration.SqlFactoryAlt.GetConnectionString());

            try
            {
                int spReturnCode = spPDFLetterInsert.StartDataReader(response, CaseNbr, LetterCode);


                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCISPDFLetter_Insert returned: {0} (Sql returned: {1})", spReturnCode, spPDFLetterInsert.SqlErrorCode));


            }
            finally
            {
                if (spPDFLetterInsert != null) spPDFLetterInsert.Dispose();
            }


        }//end PDF letter


        protected void InsertEmpDetails(String CaseNumber, System.IO.StringWriter swCaseDetails, string caseType)
        {

            Sql.spUSCISCaseDetails_Insert spPDFCaseDetailsInsert = new Sql.spUSCISCaseDetails_Insert(_Configuration.SqlFactory.GetConnectionString());

            try
            {
                int spReturnCode = spPDFCaseDetailsInsert.StartDataReader(CaseNumber, swCaseDetails, caseType);


                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCISCaseDetails_Insert returned: {0} (Sql returned: {1})", spReturnCode, spPDFCaseDetailsInsert.SqlErrorCode));


            }
            finally
            {
                if (spPDFCaseDetailsInsert != null) spPDFCaseDetailsInsert.Dispose();
            }


        }//end insert EmpDetails

        //10-28-2014 KH - insert into duplicate case list into uscisduplicatecases table
        protected void InsertDuplicateCaseList(String CaseNumber, string CaseList, int I9Id)
        {
            Sql.spUSCISDuplicateCases_Insert spUSCISDuplicateCasesInsert = new Sql.spUSCISDuplicateCases_Insert(_Configuration.SqlFactory.GetConnectionString());

            try
            {
                int spReturnCode = spUSCISDuplicateCasesInsert.StartDataReader(CaseNumber, CaseList, I9Id);


                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCISDuplicateCasesInsert returned: {0} (Sql returned: {1})", spReturnCode, spUSCISDuplicateCasesInsert.SqlErrorCode));


            }
            finally
            {
                if (spUSCISDuplicateCasesInsert != null) spUSCISDuplicateCasesInsert.Dispose();
            }

        }



        protected void InsertFailedResponse(SoapException ex) {

            if (_Pids.RequestId != -1 && _Pids.ResponseId == -1) {

                try {

                    if (_Pids.SystemId == USCISSystemId.Test) InsertResponse(new Response_Test(_Pids.RequestId, 1), ex);
                    else if (_Pids.SystemId == USCISSystemId.Live) InsertResponse(new Response_Live(_Pids.RequestId, 1), ex);

                } catch { }

            }

            Capture(LogLevel.error, string.Format("Request::{0}", _Pids.MethodName), null, ex.Message, ex);

        }

        protected int InsertTransaction() {

            Sql.spUSCIS_Transaction_Insert spTransactionInsert = new Sql.spUSCIS_Transaction_Insert(_Configuration.SqlFactory.GetConnectionString());

            try {

                int spReturnCode = spTransactionInsert.StartDataReader((int)_Pids.SystemId, _Pids.EmployeeId, _Pids.I9Id);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Transaction_Insert returned: {0} (Sql returned: {1})", spReturnCode, spTransactionInsert.SqlErrorCode));

                if (!spTransactionInsert.MoveNextDataReader(true)) throw new USCISException("spUSCIS_Transaction_Insert could not advance cursor");

                int id = Convert.ToInt32(spTransactionInsert.GetDataReaderValue(0, -1));

                if (id == -1) throw new USCISException("spUSCIS_Transaction_Insert could not determine the sql identity of the new record");

                return id;

            } finally {

                if (spTransactionInsert != null) spTransactionInsert.Dispose();

            }

        }

        protected void UpdateRequest(int idUSCISRequest, int idUSCISTransaction, int idEmployee, int idI9) {

            Sql.spUSCIS_Request_UpdateFromXQueue spRequestUpdate = new Sql.spUSCIS_Request_UpdateFromXQueue(_Configuration.SqlFactory.GetConnectionString());

            try {

                int spReturnCode = spRequestUpdate.StartDataReader(idUSCISRequest, idUSCISTransaction, idEmployee, idI9);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Request_UpdateFromXQueue returned: {0} (Sql returned: {1})", spReturnCode, spRequestUpdate.SqlErrorCode));

            } finally {

                if (spRequestUpdate != null) spRequestUpdate.Dispose();

            }

        }

        protected void UpdateTransaction(USCISCaseStatus idUSCISCaseStatus, USCISProcessStatus idUSCISProcessStatus, string TNCDueDate) {

            Sql.spUSCIS_Transaction_Update spTransactionUpdate = new Sql.spUSCIS_Transaction_Update(_Configuration.SqlFactory.GetConnectionString());

            try {

                int spReturnCode = spTransactionUpdate.StartDataReader(_Pids.TransactionId, _Pids.RequestId, _Pids.ResponseId, idUSCISCaseStatus, idUSCISProcessStatus, USCISClosureId.None, _Pids.I9Id, _Pids.CaseNumber, null, TNCDueDate);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Transaction_Update returned: {0} (Sql returned: {1})", spReturnCode, spTransactionUpdate.SqlErrorCode));

            } finally {

                if (spTransactionUpdate != null) spTransactionUpdate.Dispose();

            }

        }

        protected void UpdateTransaction(USCISCaseStatus idUSCISCaseStatus, USCISProcessStatus idUSCISProcessStatus)
        {

            Sql.spUSCIS_Transaction_Update spTransactionUpdate = new Sql.spUSCIS_Transaction_Update(_Configuration.SqlFactory.GetConnectionString());

            try
            {

                int spReturnCode = spTransactionUpdate.StartDataReader(_Pids.TransactionId, _Pids.RequestId, _Pids.ResponseId, idUSCISCaseStatus, idUSCISProcessStatus, USCISClosureId.None, _Pids.I9Id, _Pids.CaseNumber, null, null);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Transaction_Update returned: {0} (Sql returned: {1})", spReturnCode, spTransactionUpdate.SqlErrorCode));

            }
            finally
            {

                if (spTransactionUpdate != null) spTransactionUpdate.Dispose();

            }

        }

        protected void UpdateTransaction(USCISCaseStatus idUSCISCaseStatus, USCISProcessStatus idUSCISProcessStatus, USCISClosureId idClosure, string currentlyEmployed, string TNCDueDate) {

            Sql.spUSCIS_Transaction_Update spTransactionUpdate = new Sql.spUSCIS_Transaction_Update(_Configuration.SqlFactory.GetConnectionString());

            try {

                int spReturnCode = spTransactionUpdate.StartDataReader(_Pids.TransactionId, _Pids.RequestId, _Pids.ResponseId, idUSCISCaseStatus, idUSCISProcessStatus, idClosure, _Pids.I9Id, _Pids.CaseNumber, currentlyEmployed, TNCDueDate);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Transaction_Update returned: {0} (Sql returned: {1})", spReturnCode, spTransactionUpdate.SqlErrorCode));

            } finally {

                if (spTransactionUpdate != null) spTransactionUpdate.Dispose();

            }

        }

        #endregion

        #region Methods - Private (Transaction)

        private int _GetTransactionIdByCase(string caseNumber) {

            Sql.spUSCIS_Transaction_GetByCase spTransactionGetByCase = new Sql.spUSCIS_Transaction_GetByCase(_Configuration.SqlFactory.GetConnectionString());

            try {

                int spReturnCode = spTransactionGetByCase.StartDataReader(caseNumber);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_Transaction_GetByCase: {0} (Sql returned: {1})", spReturnCode, spTransactionGetByCase.SqlErrorCode));

                if (!spTransactionGetByCase.MoveNextDataReader(true)) throw new USCISException("spUSCIS_Transaction_GetByCase could not advance cursor");

                int id = Convert.ToInt32(spTransactionGetByCase.GetDataReaderValue(0, Request.NullId));

                if (id == Request.NullId) throw new USCISException("spUSCIS_Transaction_GetByCase could not determine the transaction id for the case number");

                return id;

            } finally {

                if (spTransactionGetByCase != null) spTransactionGetByCase.Dispose();

            }

        }

        #endregion
          
        #region Methods - Private (Queue)

        private bool _QueueAsError(USCISActionId idAction, USCISClosureId idClosure, string comments, bool canBeProcessed) {

            try {

                if (_Pids.EmployeeId == Request.NullId) throw new ArgumentException("invalid employee id");
                if (_Pids.I9Id == Request.NullId) throw new ArgumentException("invalid i9 id");

                Sql.spUSCIS_QueueError_Insert spQueueErrorInsert = new Sql.spUSCIS_QueueError_Insert(_Configuration.SqlFactory.GetConnectionString());

                
                try {

                    int spReturnCode = spQueueErrorInsert.StartDataReader(_Pids, idAction, idClosure, comments, canBeProcessed);

                    if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_QueueError_Insert returned: {0} (Sql returned: {1})", spReturnCode, spQueueErrorInsert.SqlErrorCode));

                } finally {

                    if (spQueueErrorInsert != null) spQueueErrorInsert.Dispose();

                }

                return true;

            } catch (Exception ex) {

                Capture(LogLevel.error, "Request::QueueAsError", null, ex.Message, ex);

                return false;

            }

        }

        private bool _QueueAsFuture() {

            try {

                if (_Pids.TransactionId == Request.NullId) throw new ArgumentException("invalid transaction id");
                if (_Pids.EmployeeId == Request.NullId) throw new ArgumentException("invalid employee id");
                if (_Pids.I9Id == Request.NullId) throw new ArgumentException("invalid i9 id");

                Sql.spUSCIS_QueueFuture_Insert spQueueFutureInsert = new Sql.spUSCIS_QueueFuture_Insert(_Configuration.SqlFactory.GetConnectionString());

                try {

                    int spReturnCode = spQueueFutureInsert.StartDataReader(_Pids);

                    if (spReturnCode != FdblSqlReturnCodes.Success) throw new USCISException(string.Format("spUSCIS_QueueFuture_Insert returned: {0} (Sql returned: {1})", spReturnCode, spQueueFutureInsert.SqlErrorCode));

                } finally {

                    if (spQueueFutureInsert != null) spQueueFutureInsert.Dispose();

                }

                return true;

            } catch (Exception ex) {

                Capture(LogLevel.error, "Request::QueueAsFuture", null, ex.Message, ex);

                return false;

            }

        }

        #endregion

    }

}