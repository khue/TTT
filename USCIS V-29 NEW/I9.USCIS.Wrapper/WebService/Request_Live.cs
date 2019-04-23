using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Web.Services.Protocols;

using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;

using com.vis_dhs.www;

namespace I9.USCIS.Wrapper.WebService {

    public class Request_Live : Request, IRequest {

        #region Members

        private EmployerWebServiceV29 _USCISWebService;
        
        private Response_Live _Response;

        #endregion

        #region Constructors

        public Request_Live() : this(Request.BypassId, Request.BypassId) { }

        public Request_Live(int idEmployee, int idI9) {

            Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;

            _Version = string.Format("{0}.{1}.{2} (build {3})", version.Major, version.Minor, version.Revision, version.Build);
            
            _Configuration = new USCISConfiguration(USCISSystemId.Live);

            _Pids = new ProcessIds(_Configuration.Logs.Capture);

            _Pids.CategoryId = USCISCategoryId.Internal;
            _Pids.SystemId = USCISSystemId.Live;
            _Pids.EmployeeId = idEmployee;
            _Pids.I9Id = idI9;
            _Pids.MethodId = USCISMethodId.EmpCpsVerifyConnection;
            _Pids.MethodName = "Constructor";

            _USCISWebService = new EmployerWebServiceV29();
            
            _USCISWebService.Timeout = (_Configuration.CPS.Threshold * 1000);

            _USCISWebService.RequestSoapContext.Security.Timestamp.TtlInSeconds = _Configuration.CPS.Threshold;
            _USCISWebService.RequestSoapContext.Security.Tokens.Add(new UsernameToken(_Configuration.CPS.UserId, _Configuration.CPS.UserPassword, PasswordOption.SendPlainText));
            

            //Response_Live response = null;
            //bool status = false;

            //for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++) {

            //    try {

            //        _Pids.RequestId = base.InsertRequest(i, string.Format("UserId={0};UserPassword=xxxxx", _Configuration.CPS.UserId));

            //        response = new Response_Live(_Pids.RequestId, _USCISWebService.EmpCpsVerifyConnection());

            //        _Pids.ResponseId = base.InsertResponse(response);

            //        if (response.ReturnStatus == 0) status = true;

            //        break;

            //    } catch (Exception ex) {

            //        base.InsertFailedResponse(ex);

            //        Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

            //    }

            //}

            //if (!status) throw new USCISException("Could not initialize the USCIS web service");

            _Pids.CategoryId = USCISCategoryId.Standard;

        }

        #endregion

        #region Properties - Public

        public EmployerWebServiceV29 WebService { get { return _USCISWebService; } }
        public IResponse WebServiceResponse { get { return _Response; } }

        #endregion

        #region Methods - Public

        public void Dispose() {

            if (_USCISWebService != null) _USCISWebService.Dispose();

            System.GC.SuppressFinalize(this);

        }

        #endregion

        #region Methods - Public (Web Service - Gets)

        public bool EmpGetCaseDetails() {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpGetCaseDetails;
            _Pids.MethodName = "EmpGetCaseDetails";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber);

            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

            if (!base.ValidateTransaction(rawData)) return false;

            Response_Live response = null;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++) {

                try {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    GetCaseDetailsRequest r = new GetCaseDetailsRequest();
                    r.CaseNumber = _Pids.CaseNumber;
                    response = new Response_Live(_Pids.RequestId, _USCISWebService.GetCaseDetails(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;


                    if (response  != null)
                    {

                        base.InsertEmpDetails(_Pids.CaseNumber, response.CaseDetails, response.CaseDetailsType);

                    }


                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }
                    
                    break;

                } catch (SoapException ex) {

                    base.InsertFailedResponse(ex);

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            _Response = response;

            return status;

        }

        //10-30-2014 EV 26 get duplicate case list
        public bool EmpGetDuplicateCaseList()
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpGetDuplicateCaseList;
            _Pids.MethodName = "EmpGetDuplicateCaseList";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber);

            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (Exception ex)
            {

                //return base.QueueAsError(USCISActionId.EmpGetDuplicateCaseList, USCISClosureId.None, null, rawData, null, null, ex);

            }

            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

            if (!base.ValidateTransaction(rawData)) return false;

            Response_Live response = null;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    GetDuplicateCaseListRequest r = new GetDuplicateCaseListRequest();
                    r.CaseNumber = _Pids.CaseNumber;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.GetDuplicateCaseList(r));
                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;


                    if (status == true)
                    {

                        base.InsertDuplicateCaseList(_Pids.CaseNumber, response.CaseList, _Pids.I9Id);

                    }

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            _Response = response;

            return status;

        }

        

        public bool GetNextCase() {

            _Response = null;

            _Pids.EmployeeId = Request.BypassId;
            _Pids.I9Id = Request.BypassId;

            _Pids.MethodId = USCISMethodId.EmpGetNextResolvedCaseNbrs;
            _Pids.MethodName = "GetNextCase";

            Response_Live response = null;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++) {

                try {

                    _Pids.RequestId = base.InsertRequest(i, "fetching (no data sent)");

                    //response = new Response_Live(_Pids.RequestId, _USCISWebService.EmpGetNextResolvedCaseNbrs(_Version, "1", "Y"));
                    GetResolvedCasesRequest r = new GetResolvedCasesRequest();
                    r.NumberOfRecords = 1;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.GetResolvedCases(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                   if (response.Photo != null && _Pids.CaseNumber != null)
                   {
                       base.InsertPhoto(response, _Pids.CaseNumber);
                   }
                    
                    break;

                } catch (SoapException ex) {

                    base.InsertFailedResponse(ex);

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            _Response = response;

            return status;

        }

        #endregion

        #region Methods - Public (Web Service - General Updates)

        public bool AcknowledgeReceiptOfCaseNumber(USCISAcknowledgeCase ac) {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpAckReceiptOfResolvedCaseNbr;
            _Pids.MethodName = "AcknowledgeReceiptOfCaseNumber";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber);

            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

            if (!base.ValidateTransaction(rawData)) return false;

            Response_Live response = null;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++) {

                try {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    ConfirmResolvedCasesReceivedRequest r = new ConfirmResolvedCasesReceivedRequest();
                    ConfirmResolvedCaseListRecord nbr = new ConfirmResolvedCaseListRecord();
                    nbr.CaseNumber = _Pids.CaseNumber;

                    r.CaseList[0] = nbr;
                    if (response == null) response = new Response_Live(_Pids.RequestId, _USCISWebService.ConfirmResolvedCasesReceived(r));
                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    break;

                } catch (SoapException ex) {

                    base.InsertFailedResponse(ex);

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            _Response = response;
            return status;

        }

        public bool AcknowledgeReceiptOfCaseNumber(string ac, string strTypeOfCase)
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpAckReceiptOfResolvedCaseNbr;
            _Pids.MethodName = "AcknowledgeReceiptOfCaseNumber";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber);

            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

            //if (!base.ValidateTransaction(rawData)) return false;

            Response_Live response = null;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    ConfirmResolvedCasesReceivedRequest r = new ConfirmResolvedCasesReceivedRequest();
                    ConfirmResolvedCaseListRecord nbr = new ConfirmResolvedCaseListRecord();
                    r.CaseList = new ConfirmResolvedCaseListRecord[1];

                    r.CaseList[0] = nbr;
                    r.CaseList[0].CaseNumber = _Pids.CaseNumber;

                    if (strTypeOfCase == "Initial")
                    { r.CaseList[0].VerificationStep = VerificationStepType.Initial; }
                    else if (strTypeOfCase == "Second")
                    { r.CaseList[0].VerificationStep = VerificationStepType.Second; }
                    else if (strTypeOfCase == "Third")
                    { r.CaseList[0].VerificationStep = VerificationStepType.Third; }

                    if (response == null) response = new Response_Live(_Pids.RequestId, _USCISWebService.ConfirmResolvedCasesReceived(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            _Response = response;
            return status;

        }
      
        #endregion

        #region Methods - Public (Web Service - Push)

        public bool InitiateCase() {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpInitDABPVerif;
            _Pids.MethodName = "InitiateCase";

            if (!base.ValidateParameters(USCISRequestValidation.EmployeeId | USCISRequestValidation.I9Id)) return false;
            
            string rawData = string.Format("EmployeeId={0};I9Id={1}", _Pids.EmployeeId, _Pids.I9Id);

            Employee.LocalCase emp = null;

            try {

                emp = base.GetLocalEmployeeCase();

            } catch (SoapException ex) {

                //return base.QueueAsError(USCISActionId.InitiateCase, USCISClosureId.None, null, rawData, null, null, ex);

            }

            rawData = string.Format("EmployeeId={0};I9Id={1};LastName={2};FirstName={3};MiddleInitial={4};MaidenName={5};SSN={6};DateOfBirth={7};DateOfHire={8};CitizenshipStatus={9};AlienNumber={10};I94Number={11};CardNumber={12};PassportNumber={13};VisaNumber={14};DocumentType={15};DateOfDocumentExpiration={16};SubmiterName={17};SubmiterPhone={18};UserField={19};OverDueReasonCode={20};ListBDocumentID={21};ListCDocumentID={22};SupportingDocID={23};StateIssuingAuthority={24};ListBDocumentNumber={25};NoForeignPassport={26};CountryOfIssuance={27};ListBExpirationFlag={28}", _Pids.EmployeeId, _Pids.I9Id, emp.LastName, emp.FirstName, emp.MiddleInitial, emp.MaidenName, emp.SSN, emp.DateOfBirth.ToString("MM/dd/yyyy"), emp.DateOfHire.ToString("MM/dd/yyyy"), emp.CitizenshipStatus, emp.AlienNumber, emp.I94Number, emp.CardNumber, emp.PassportNumber, emp.VisaNumber, emp.DocumentType, emp.DateOfDocumentExpiration.ToString("MM/dd/yyyy"), _Configuration.CPS.SubmiterName, _Configuration.CPS.SubmiterPhone, emp.UserField, emp.OverDueVerifyReason, emp.ListBDocumentType, emp.ListCDocumentType, emp.SupportingDocumentID, emp.StateIssuingAuthority, emp.ListBDocumentNumber, emp.NoForeignPassport, emp.CountryOfIssuance, emp.ListBExpirationFlag);

            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

            if (!base.ValidateTransaction(USCISActionId.InitiateCase, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++) {

                try {

                    _Pids.CaseNumber = null;
                    _Pids.RequestId = base.InsertRequest(i, rawData);

                    SubmitInitialVerificationRequest r = new SubmitInitialVerificationRequest();
                    r.LastName = emp.LastName;
                    r.FirstName = emp.FirstName;
                    r.MiddleInitial = emp.MiddleInitial;
                    r.OtherNamesUsed = emp.MaidenName;
                    r.Ssn = emp.SSN;
                    r.BirthDate = emp.DateOfBirth;
                    r.AlienNumber = emp.AlienNumber;
                    r.I94Number = emp.I94Number;
                    r.IssuingAuthorityCode = emp.StateIssuingAuthority;
                    r.CountryOfIssuanceCode = emp.CountryOfIssuance;
                    r.CitizenshipStatusCode = Convert.ToString(emp.CitizenshipStatus);
                    r.PassportNumber = emp.PassportNumber;
                    r.NoForeignPassportIndicator = false; //always send false per documentation. Excerpt from ICA 29.3 documentation: "This field must contain a value of “false” due to Form I-9 changes."
                    r.SubmittingOfficialName = null;
                    r.SubmittingOfficialPhoneExtension = null;
                    r.SubmittingOfficialPhoneNumber = null;
                    r.EmployerCaseId = Convert.ToString(_Pids.TransactionId);
                    r.DocumentExpirationDate = emp.DateOfDocumentExpiration;
                    r.ListBCDocumentNumber = emp.ListBDocumentNumber;
                    r.SupportingDocumentTypeId = emp.SupportingDocumentID;
                    r.ListCDocumentTypeId = emp.ListCDocumentType;
                    r.ListBDocumentTypeId = emp.ListBDocumentType;
                    r.DocumentTypeId = emp.DocumentType;
                    r.VisaNumber = emp.VisaNumber;
                    r.CardNumber = emp.CardNumber;
                    r.LateVerificationReasonCode = emp.OverDueVerifyReason;
                    r.LateVerificationOtherReasonText = emp.OverDueVerifyReasonOther;
                    r.HireDate = emp.DateOfHire;
                    r.EmailAddress = emp.EmailAddress;
                    r.ClientCompanyId = emp.USCISCompanyId;
                    r.ClientSoftwareVersion = _Version;

                    if (emp.ListBExpirationFlag == "Y")
                    {
                        r.NoDocumentExpirationDateIndicator = true;
                    }
                    else
                    {
                        r.NoDocumentExpirationDateIndicator = null;
                    }

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.SubmitInitialVerification(r));
                    _Pids.CaseNumber = response.CaseNbr;
                    _Pids.ResponseId = base.InsertResponse(response);

                    status = true;

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }

                    break;

                } catch (SoapException ex) {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId) {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }
                              
            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.ManualInterventionRequired);
             }

            _Response = response;

            return status;

        }

        public bool SubmitSSAReferral() {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpSubSSAReferral;
            _Pids.MethodName = "SubmitSSAReferral";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber);
            
            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

            if (!base.ValidateTransaction(USCISActionId.SubmitSSAReferral, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++) {

                try {

                    _Pids.RequestId = base.InsertRequest(i, rawData);

                    SubmitSsaReferralRequest r = new SubmitSsaReferralRequest();
                    r.CaseNumber = _Pids.CaseNumber;
                    r.ClientSoftwareVersion = _Version;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.SubmitSsaReferral(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }

                    break;

                } catch (SoapException ex) {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId) {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum, response.ResolveDate);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.ManualInterventionRequired);

            }

            _Response = response;

            return status;

        }

        public bool EmpRetrieveFAN(string LetterTypeCode)
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpRetrieveFAN;
            _Pids.MethodName = "EmpRetrieveFAN";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};LetterTypeCode={3}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, LetterTypeCode);

            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (Exception ex)
            {

                //return base.QueueAsError(USCISActionId.EmpRetrieveFAN, USCISClosureId.None, null, rawData, null, null, ex);

            }


            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

            if (!base.ValidateTransaction(USCISActionId.EmpRetrieveFAN, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);

                    RetrieveLetterRequest r = new RetrieveLetterRequest();
                    r.CaseNumber = _Pids.CaseNumber;
                    r.ClientSoftwareVersion = _Version;
                    r.Language = LanguageType.ENGLISH;

                    switch (LetterTypeCode)
                    {
                        case "SSA_FAN":
                            r.LetterTypeCode = LetterTypeCodeType.SSA_FAN;
                            break;
                        case "SSA_NATZ_FAN":
                            r.LetterTypeCode = LetterTypeCodeType.SSA_NATZ_FAN;
                            break;
                        case "SSA_FA_FAN":
                            r.LetterTypeCode = LetterTypeCodeType.SSA_FA_FAN;
                            break;
                        case "DHS_FAN":
                            r.LetterTypeCode = LetterTypeCodeType.DHS_FAN;
                            break;
                        case "SSA_FNC":
                            r.LetterTypeCode = LetterTypeCodeType.SSA_FNC;
                            break;
                        case "DHS_FNC":
                            r.LetterTypeCode = LetterTypeCodeType.DHS_FNC;
                            break;
                        case "SSA_RDC":
                            r.LetterTypeCode = LetterTypeCodeType.SSA_RDC;
                            break;
                        case "DHS_RDC":
                            r.LetterTypeCode = LetterTypeCodeType.DHS_RDC;
                            break;
                    }

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.RetrieveLetter(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    if (_Pids.CaseNumber != null)
                    {

                        base.InsertPDFLetter(response, _Pids.CaseNumber, LetterTypeCode);
                    }

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId)
                    {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                //base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.ManualInterventionRequired);
            }

            _Response = response;

            return status;

        }

        public bool ResubmitSSAVerification() {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpSSAResubmittal;
            _Pids.MethodName = "ResubmitSSAVerification";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber);

            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (SoapException ex)
            {

                //return base.QueueAsError(USCISActionId.ResubmitSSAVerification, USCISClosureId.None, null, rawData, null, null, ex);

            }

            rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};LastName={3};FirstName={4};MiddleInitial={5};MaidenName={6};SSN={7};DateOfBirth={8}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, emp.LastName, emp.FirstName, emp.MiddleInitial, emp.MaidenName, emp.SSN, emp.DateOfBirth.ToString("MM/dd/yyyy"));

            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

            if (!base.ValidateTransaction(USCISActionId.ResubmitSSAVerification, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++) {

                try {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    SubmitSsaResubmittalRequest r = new SubmitSsaResubmittalRequest();
                    r.CaseNumber = _Pids.CaseNumber;
                    r.ClientSoftwareVersion = _Version;
                    r.BirthDate = emp.DateOfBirth;
                    r.FirstName = emp.FirstName;
                    r.LastName = emp.LastName;
                    r.MiddleInitial = emp.MiddleInitial;
                    r.OtherNamesUsed = emp.MaidenName;
                    r.Ssn = emp.SSN;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.SubmitSsaResubmittal(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }

                    break;

                } catch (SoapException ex) {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId) {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.ManualInterventionRequired);

            }

            _Response = response;

            return status;

        }

        public bool SubmitDHSReferral() {

            _Response = null;
            
            Employee.LocalCase emp = null;
            Employee.UploadedPhoto uploadedphoto = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (Exception ex)
            {

                //return base.QueueAsError(USCISActionId.SubmitDHSReferral, USCISClosureId.None, null, "GetLocalEmployeeCase()", null, null, ex);

            }

            //run only if there is a photo found
            if (emp.UploadDoc == "Y") {
                
                try
                {
                    uploadedphoto = base.GetPhoto();
                }
                catch (Exception ex)
                {

                    //return base.QueueAsError(USCISActionId.SubmitDHSReferral, USCISClosureId.None, null, "GetPhoto()", null, null, ex);
                }

            }// end if uploaddoc==y


            _Pids.MethodId = USCISMethodId.EmpSubDHSReferral;
            _Pids.MethodName = "SubmitDHSReferral";

            string rawData = null;

            if (emp.UploadDoc == "Y")
            {
                rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};Filename={3}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, uploadedphoto.FName);
            }
            else
            {
                rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};Filename={3}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, "NA");
            }

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;
            
            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

            if (!base.ValidateTransaction(USCISActionId.SubmitDHSReferral, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++) {

                try {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    SubmitDhsReferralRequest r = new SubmitDhsReferralRequest();
                    r.CaseNumber = _Pids.CaseNumber;
                    r.ClientSoftwareVersion = _Version;
                    
                    if (uploadedphoto != null)
                    {
                        r.PhotoDocument = uploadedphoto.GIFToSend;
                    }

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.SubmitDhsReferral(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;
                                      
                    break;

                } catch (SoapException ex) {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId) {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum, response.ResolveDate);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.ManualInterventionRequired);
            }

            _Response = response;

            return status;

        }

        public bool SubmitSecondVerification(string comments) {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpSubmitAdditVerif;
            _Pids.MethodName = "SubmitSecondVerification";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;

            if (comments == null) comments = string.Empty;

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};SubmiterName={3};SubmiterPhone={4};Comments={5}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, _Configuration.CPS.SubmiterName, _Configuration.CPS.SubmiterPhone, comments);

            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

            if (!base.ValidateTransaction(USCISActionId.SubmitSecondVerification, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++) {

                try {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    SubmitAdditionalVerificationRequest r = new SubmitAdditionalVerificationRequest();
                    r.ClientSoftwareVersion = _Version;
                    r.CaseNumber = _Pids.CaseNumber;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.SubmitAdditionalVerification(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }

                    status = true;

                    break;

                } catch (SoapException ex) {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId) {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.ManualInterventionRequired);
            }

            _Response = response;

            return status;

        }

        public bool CloseCase(USCISClosureId idClosure, string currentlyEmployed) {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpCloseCase;
            _Pids.MethodName = "CloseCase";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;
                      
            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};ClosureId={3};CurrentlyEmployed={4}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, idClosure, currentlyEmployed);

            string codeClosure = null;

            try {

                codeClosure = base.GetClosureCode(idClosure);

            } catch (Exception ex) {

                //return base.QueueAsError(USCISActionId.CloseCase, idClosure, null, rawData, null, null, ex);

            }

            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

            if (!base.ValidateTransaction(USCISActionId.CloseCase, idClosure, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++) {

                try {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    CloseCaseRequest r = new CloseCaseRequest();
                    r.CaseNumber = _Pids.CaseNumber;
                    r.ClientSoftwareVersion = _Version;
                    r.ClosureReasonCode = codeClosure;

                    if (currentlyEmployed == "Y")
                    {
                        r.CurrentlyEmployed = true;
                    }
                    else if (currentlyEmployed == "N")
                    {
                        r.CurrentlyEmployed = false;
                    }
                    else
                    {
                        r.CurrentlyEmployed = null;
                    }

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.CloseCase(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    break;

                } catch (SoapException ex) {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId) {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum, idClosure, currentlyEmployed, null);
               // base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.CaseNeedsClose);
            }

            _Response = response;

            return status;

        }

        //empphotoconfirm version 21 method
        public bool EmpConfirmPhoto()
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpConfirmPhoto;
            _Pids.MethodName = "EmpConfirmPhoto";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;

                       
            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();

           
            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (SoapException ex)
            {

                //return base.QueueAsError(USCISActionId.EmpConfirmPhoto, USCISClosureId.None, null, "", null, null, ex);

            }

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};PhotoConfirmation={3}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, emp.PhotoConfirmation);

            if (!base.ValidateTransaction(USCISActionId.EmpConfirmPhoto, USCISClosureId.None, null, rawData)) return false;
            
            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {
                    
                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    ConfirmDocumentPhotoRequest r = new ConfirmDocumentPhotoRequest();
                    r.CaseNumber = _Pids.CaseNumber;
                    r.ClientSoftwareVersion = _Version;

                    if (emp.PhotoConfirmation == "Y")
                    {
                        r.PhotoConfirmedIndicator = true;
                    }
                    else
                    {
                        r.PhotoConfirmedIndicator = false;
                    }

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.ConfirmDocumentPhoto(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId)
                    {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.CaseNeedsClose);
            }

            _Response = response;

            return status;

        }

        //EmpSSAReverify version 21
        public bool EmpSSAReVerify()
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpSSAReVerify;
            _Pids.MethodName = "EmpSSAReVerify";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;


            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();


            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (SoapException ex)
            {

                //return base.QueueAsError(USCISActionId.EmpSSAReVerify, USCISClosureId.None, null, "", null, null, ex);

            }

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};Lastname={3};Firstname={4};MiddleInitial={5};Maidenname={6};SSN={7};DateofBirth={8}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, emp.LastName, emp.FirstName, emp.MiddleInitial, emp.MaidenName, emp.SSN, emp.DateOfBirth);

            if (!base.ValidateTransaction(USCISActionId.EmpSSAReVerify, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    SubmitSsaReverifyRequest r = new SubmitSsaReverifyRequest();
                    r.ClientSoftwareVersion = _Version;
                    r.CaseNumber = _Pids.CaseNumber;
                    r.BirthDate = Convert.ToDateTime(emp.DateOfBirth.ToString("yyyy-MM-dd"));
                    r.FirstName = emp.FirstName;
                    r.LastName = emp.LastName;
                    r.MiddleInitial = emp.MiddleInitial;
                    r.Ssn = emp.SSN;
                    r.OtherNamesUsed = emp.MaidenName;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.SubmitSsaReverify(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId)
                    {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.CaseNeedsClose);
            }

            _Response = response;

            return status;

        }

        //EmpDHSReverify version 21
        public bool EmpDHSReVerify()
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpDHSReVerify;
            _Pids.MethodName = "EmpDHSReVerify";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;


            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();


            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (Exception ex)
            {

                //return base.QueueAsError(USCISActionId.EmpDHSReVerify, USCISClosureId.None, null, "", null, null, ex);

            }

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};AlienNumber={3};I94={4};CardNumber={5};Passport={6};Visa={7};NoForeignPassport={8};CountryOfIssuance={9}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, emp.AlienNumber, emp.I94Number,emp.CardNumber,emp.PassportNumber, emp.VisaNumber, emp.NoForeignPassport, emp.CountryOfIssuance);

            if (!base.ValidateTransaction(USCISActionId.EmpDHSReVerify, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    SubmitDhsReverifyRequest r = new SubmitDhsReverifyRequest();
                    r.ClientSoftwareVersion = _Version;
                    r.CaseNumber = _Pids.CaseNumber;
                    r.AlienNumber = emp.AlienNumber;
                    r.BirthDate = emp.DateOfBirth;
                    r.CardNumber = emp.CardNumber;
                    r.CountryOfIssuanceCode = emp.CountryOfIssuance;
                    //r.DocumentExpirationDate = emp.DateOfDocumentExpiration;
                    r.I94Number = emp.I94Number;
                    r.ListBCDocumentNumber = emp.ListBDocumentNumber;
                    r.NoForeignPassportIndicator = false; //always send false per documentation. Excerpt from ICA 29.3 documentation: "This field must contain a value of “false” due to Form I-9 changes."
                    r.PassportNumber = emp.PassportNumber;
                    r.VisaNumber = emp.VisaNumber;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.SubmitDhsReverify(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId)
                    {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.CaseNeedsClose);
            }

            _Response = response;

            return status;

        }

        //EmpCitDHSReverify version 21
        public bool EmpCitDHSReVerify()
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpCitDHSReVerify;
            _Pids.MethodName = "EmpCitDHSReVerify";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;


            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();


            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (Exception ex)
            {

                //return base.QueueAsError(USCISActionId.EmpCitDHSReVerify, USCISClosureId.None, null, "", null, null, ex);

            }

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};Passport={3};DateofBirth={4}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, emp.PassportNumber, emp.DateOfBirth);

            if (!base.ValidateTransaction(USCISActionId.EmpCitDHSReVerify, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    SubmitDhsReverifyRequest r = new SubmitDhsReverifyRequest();
                    r.ClientSoftwareVersion = _Version;
                    r.CaseNumber = _Pids.CaseNumber;
                    r.AlienNumber = emp.AlienNumber;
                    r.BirthDate = emp.DateOfBirth;
                    r.CardNumber = emp.CardNumber;
                    r.CountryOfIssuanceCode = emp.CountryOfIssuance;
                    //r.DocumentExpirationDate = emp.DateOfDocumentExpiration;
                    r.I94Number = emp.I94Number;
                    r.ListBCDocumentNumber = emp.ListBDocumentNumber;
                    r.NoForeignPassportIndicator = false; //always send false per documentation. Excerpt from ICA 29.3 documentation: "This field must contain a value of “false” due to Form I-9 changes."
                    r.PassportNumber = emp.PassportNumber;
                    r.VisaNumber = emp.VisaNumber;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.SubmitDhsReverify(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId)
                    {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.CaseNeedsClose);
            }

            _Response = response;

            return status;

        }


        //EmpDMVDHSReverify version 23
        public bool EmpDMVDHSReVerify()
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpDMVDHSReVerify;
            _Pids.MethodName = "EmpDMVDHSReVerify";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;


            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();


            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (Exception ex)
            {

                //return base.QueueAsError(USCISActionId.EmpDMVDHSReVerify, USCISClosureId.None, null, "", null, null, ex);

            }

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};AlienNbr={3};I94Nbr={4};DMVDocNbr={5};DateofBirth={6};NoForeignPassport={7};CountryOfIssuance={8}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, emp.AlienNumber, emp.I94Number, emp.ListBDocumentNumber, emp.DateOfBirth, emp.NoForeignPassport, emp.CountryOfIssuance);

            if (!base.ValidateTransaction(USCISActionId.EmpDMVDHSReVerify, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    SubmitDhsReverifyRequest r = new SubmitDhsReverifyRequest();
                    r.ClientSoftwareVersion = _Version;
                    r.CaseNumber = _Pids.CaseNumber;
                    r.AlienNumber = emp.AlienNumber;
                    r.BirthDate = emp.DateOfBirth;
                    r.CardNumber = emp.CardNumber;
                    r.CountryOfIssuanceCode = emp.CountryOfIssuance;
                    //r.DocumentExpirationDate = emp.DateOfDocumentExpiration;
                    r.I94Number = emp.I94Number;
                    r.ListBCDocumentNumber = emp.ListBDocumentNumber;
                    r.NoForeignPassportIndicator = false; //always send false per documentation. Excerpt from ICA 29.3 documentation: "This field must contain a value of “false” due to Form I-9 changes."
                    r.PassportNumber = emp.PassportNumber;
                    r.VisaNumber = emp.VisaNumber;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.SubmitDhsReverify(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId)
                    {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.CaseNeedsClose);
            }

            _Response = response;

            return status;

        }

        //new request object to retrieve US passport photo kh 2/15/2011
        public bool EmpRetrievePhoto()
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpRetrievePhoto;
            _Pids.MethodName = "EmpRetrievePhoto";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;


            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();


            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (Exception ex)
            {

                //return base.QueueAsError(USCISActionId.EmpRetrievePhoto, USCISClosureId.None, null, "", null, null, ex);

            }

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber);

            if (!base.ValidateTransaction(USCISActionId.EmpRetrievePhoto, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    RetrieveDocumentPhotoRequest r = new RetrieveDocumentPhotoRequest();
                    r.ClientSoftwareVersion = _Version;
                    r.CaseNumber = _Pids.CaseNumber;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.RetrieveDocumentPhoto(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId)
                    {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                //do no send email for this because everify returns code P twice, once when it's a dhs referred, photo match required and again when we retrieve the photo. The return cd doesn't change during photo retrival.
                //base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.CaseNeedsClose);
            }

            _Response = response;

            return status;

        }

        
        //EmpDupCaseContinueWithChanges 10-31-2014 EV 26 
        public bool EmpDupCaseContinueWithChanges()
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpDupCaseContinueWithChanges;
            _Pids.MethodName = "EmpDupCaseContinueWithChanges";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;


            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();


            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (SoapException ex)
            {

                //return base.QueueAsError(USCISActionId.EmpDupCaseContinueWithChanges, USCISClosureId.None, null, "", null, null, ex);

            }

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};Lastname={3};Firstname={4};MiddleInitial={5};Maidenname={6};SSN={7};DateofBirth={8}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, emp.LastName, emp.FirstName, emp.MiddleInitial, emp.MaidenName, emp.SSN, emp.DateOfBirth);

            if (!base.ValidateTransaction(USCISActionId.EmpDupCaseContinueWithChanges, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    ContinueDuplicateCaseWithChangeRequest r = new ContinueDuplicateCaseWithChangeRequest();
                    r.ClientSoftwareVersion = _Version;
                    r.CaseNumber = _Pids.CaseNumber;
                    r.BirthDate = emp.DateOfBirth;
                    r.FirstName = emp.FirstName;
                    r.LastName = emp.LastName;
                    r.MiddleInitial = emp.MiddleInitial;
                    r.OtherNamesUsed = emp.MaidenName;
                    r.Ssn = emp.SSN;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.ContinueDuplicateCaseWithChange(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }


                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId)
                    {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.CaseNeedsClose);
            }

            _Response = response;

            return status;

        }

        //EmpDupCaseContinueWithoutChanges version 26
        public bool EmpDupCaseContinueWithoutChanges()
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpDupCaseContinueWithoutChanges;
            _Pids.MethodName = "EmpDupCaseContinueWithoutChanges";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;


            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();


            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (Exception ex)
            {

                //return base.QueueAsError(USCISActionId.EmpDupCaseContinueWithoutChanges, USCISClosureId.None, null, "", null, null, ex);

            }

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};DupReason={3};DupReasonOther={4}", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber, emp.DupReason, emp.DupReasonOther);

            if (!base.ValidateTransaction(USCISActionId.EmpDupCaseContinueWithoutChanges, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    ContinueDuplicateCaseRequest r = new ContinueDuplicateCaseRequest();
                    r.ClientSoftwareVersion = _Version;
                    r.CaseNumber = _Pids.CaseNumber;
                    r.ReasonCode = emp.DupReason;
                    r.OtherReasonText = emp.DupReasonOther;

                    response = new Response_Live(_Pids.RequestId, _USCISWebService.ContinueDuplicateCase(r));

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    if (response.Photo != null && _Pids.CaseNumber != null)
                    {
                        base.InsertPhoto(response, _Pids.CaseNumber);
                    }

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId)
                    {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.CaseNeedsClose);
            }

            _Response = response;

            return status;

        }

        //4/3/2015 EV version 27 update
        public bool EmpSaveSSATNCNotification()
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpSaveSSATNCNotification;
            _Pids.MethodName = "EmpSaveSSATNCNotification";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;


            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();


            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (Exception ex)
            {

                //return base.QueueAsError(USCISActionId.EmpSaveSSATNCNotification, USCISClosureId.None, null, "", null, null, ex);

            }

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber);

            if (!base.ValidateTransaction(USCISActionId.EmpSaveSSATNCNotification, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    SetSsaTncNotificationRequest r = new SetSsaTncNotificationRequest();
                    r.ClientSoftwareVersion = _Version;
                    r.CaseNumber = _Pids.CaseNumber;
                    r.EmployeeNotifiedIndicator = true;

                    //response = new Response_Test(_Pids.RequestId, _USCISWebService.SetSsaTncNotification(r)); //this request has no response.
                    _USCISWebService.SetSsaTncNotification(r);
                    response = new Response_Live(_Pids.RequestId, 0);

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId)
                    {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                //base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.CaseNeedsClose);
            }

            _Response = response;

            return status;

        }

        //4/3/2015 EV version 27 update
        public bool EmpSaveDHSTNCNotification()
        {

            _Response = null;

            _Pids.MethodId = USCISMethodId.EmpSaveDHSTNCNotification;
            _Pids.MethodName = "EmpSaveDHSTNCNotification";

            if (!base.ValidateParameters(USCISRequestValidation.All)) return false;


            if (_Pids.TransactionId == Request.NullId) _Pids.TransactionId = base.GetTransactionId();


            Employee.LocalCase emp = null;

            try
            {

                emp = base.GetLocalEmployeeCase();

            }
            catch (Exception ex)
            {

                //return base.QueueAsError(USCISActionId.EmpSaveDHSTNCNotification, USCISClosureId.None, null, "", null, null, ex);

            }

            string rawData = string.Format("EmployeeId={0};I9Id={1};CaseNumber={2};", _Pids.EmployeeId, _Pids.I9Id, _Pids.CaseNumber);

            if (!base.ValidateTransaction(USCISActionId.EmpSaveDHSTNCNotification, USCISClosureId.None, null, rawData)) return false;

            Response_Live response = null;
            string idErrorRequests = string.Empty;
            bool status = false;

            for (int i = 0; i < _Configuration.CPS.AttemptMaxRetry; i++)
            {

                try
                {

                    _Pids.RequestId = base.InsertRequest(i, rawData);
                    SetDhsTncNotificationRequest r = new SetDhsTncNotificationRequest();
                    r.ClientSoftwareVersion = _Version;
                    r.CaseNumber = _Pids.CaseNumber;
                    r.EmployeeNotifiedIndicator = true;

                    //response = new Response_Test(_Pids.RequestId, _USCISWebService.EmpSaveDHSTNCNotification(_Pids.CaseNumber, 'Y'.ToString()));
                    _USCISWebService.SetDhsTncNotification(r);

                    response = new Response_Live(_Pids.RequestId, 0);

                    _Pids.ResponseId = base.InsertResponse(response, _Pids.CaseNumber);

                    status = true;

                    break;

                }
                catch (SoapException ex)
                {

                    base.InsertFailedResponse(ex);

                    if (_Pids.RequestId != Request.NullId)
                    {

                        if (idErrorRequests.Length > 0) idErrorRequests += ", ";
                        idErrorRequests += Convert.ToString(_Pids.RequestId);

                    }

                    Thread.Sleep(_Configuration.CPS.AttemptWaitTime);

                }

            }

            if (status)
            {
                base.UpdateTransaction(response.CaseStatusEnum, response.ProcessStatusEnum);
                //base.CheckAndSendEmail((int)response.EmailEnum, _Pids.EmployeeId, _Pids.I9Id);
            }
            else
            {

                base.UpdateTransaction((response == null ? USCISCaseStatus.Unknown : response.CaseStatusEnum), USCISProcessStatus.CaseNeedsClose);
            }

            _Response = response;

            return status;

        }

        #endregion

        #region Methods - Public (Local Updates)

        public void UpdateRequest(int idUSCISRequest, int idUSCISTransaction, int idEmployee, int idI9) {

            base.UpdateRequest(idUSCISRequest, idUSCISTransaction, idEmployee, idI9);

        }

        public void UpdateTransaction(USCISCaseStatus idUSCISCaseStatus, USCISProcessStatus idUSCISProcessStatus) {

            base.UpdateTransaction(idUSCISCaseStatus, idUSCISProcessStatus);

        }

        #endregion
                        
    }

}