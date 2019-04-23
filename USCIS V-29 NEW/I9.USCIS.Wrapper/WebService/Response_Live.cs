using System;
using System.Text;

using com.vis_dhs.www;
using System.Xml.Serialization;
using System.IO;

namespace I9.USCIS.Wrapper.WebService {

    internal class Response_Live : Response, IResponse
    {

        #region Members

        private Employee.RemoteCase_Live _RemoteCase = null;
        private GetCaseDetailsResult _CaseDetails = null;
        private System.IO.StringWriter _EmpCaseDetails = null;
        private String _EmpCaseDetailsType = String.Empty;

        //DG private CaseListArray9[] _Cases = null;
        //private Case28[] _Cases = null;
        private com.vis_dhs.ICaseListArray[] _Cases = null;
        private string _CaseList = null;
        private com.vis_dhs.ICitizenshipCodeListArray[] _CitizenshipCodes = null;
        private com.vis_dhs.IDocTypeListArray[] _DocTypes = null;
        //private DocumentField[] _Fields = null;

        #endregion

        #region Constructors

        //parameterless constructor needed for xml serialization
        public Response_Live()
        {


        }

        internal Response_Live(int idUSCISRequest, int hadIssue)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (!(hadIssue == 0 || hadIssue == 1)) throw new ArgumentNullException("invalid issue flag");

            _USCISRequestId = idUSCISRequest;
            _HadIssue = hadIssue;

        }

        internal Response_Live(int idUSCISRequest, SubmitInitialVerificationResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            _CaseNbr = uscisResponse.CaseNumber;
            _LastName = uscisResponse.SystemLastName;
            _FirstName = uscisResponse.SystemFirstName;
            _EligStatementCd = Convert.ToString(uscisResponse.MessageCode);
            _EligStatementTxt = uscisResponse.EligibilityStatement;
            _EligStatementDetailsTxt = uscisResponse.EligibilityStatementDetails;
            if (!(uscisResponse.LetterTypeCodeList == null || uscisResponse.LetterTypeCodeList.Length == 0))
            {
                _LetterCode = String.Join("|", uscisResponse.LetterTypeCodeList);
            }
            //_AdditionalPerfind = uscisResponse.AdditionalPerfind;

            //_PhotoIncluded = uscisResponse.

            //_Photo = uscisResponse.
            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;
            //_NumberOfReferralReasons = uscisResponse.rea

            //if (uscisResponse.ReferralReasonList != null)
            //{

            //    StringBuilder strRsns = new StringBuilder();

            //    foreach (string rsn in uscisResponse.ReferralReasonList)
            //    {

            //        strRsns.Append(rsn);
            //        strRsns.Append(";");
            //    }

            //    _ReferralListArray = strRsns.ToString();
            //}// if uscisresponse.referralreasonlistarray is not null

            string tmpCode = string.Empty;

            if (!string.IsNullOrEmpty(_EligStatementCd)) tmpCode = _EligStatementCd.PadLeft(3, '0');

            if (tmpCode.Equals("005") || tmpCode.Equals("042")) _ProcessStatus = USCISProcessStatus.AwaitingUSCIS;
            else if (tmpCode.Equals("029") || tmpCode.Equals("027") || tmpCode.Equals("038") || tmpCode.Equals("031") || tmpCode.Equals("036") || tmpCode.Equals("030") || tmpCode.Equals("P") || tmpCode.Equals("X") || tmpCode.Equals("041")) _ProcessStatus = USCISProcessStatus.AwaitingClient;
            else if (tmpCode.Equals("008") || tmpCode.Equals("016") || tmpCode.Equals("043")) _ProcessStatus = USCISProcessStatus.CaseNeedsClose;

        }

        //DG 12/17/2013 for retrieving PDF letter
        internal Response_Live(int idUSCISRequest, RetrieveLetterResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_CaseNbr = uscisResponse.CaseNbr;
            //_LetterCode= uscisResponse.LetterTypeCode;


            _FAN = uscisResponse.Letter;
            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

        }

        //new response method for receiving us passport photo. kh 2/15/2011
        internal Response_Live(int idUSCISRequest, RetrieveDocumentPhotoResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_CaseNbr = uscisResponse.CaseNbr;
            //_LastName = uscisResponse.LastName;
            //_FirstName = uscisResponse.FirstName;
            //_EligStatementCd = uscisResponse.EligStatementCd;
            //_EligStatementTxt = uscisResponse.EligStatementTxt;
            //_EligStatementDetailsTxt = uscisResponse.EligStatementDetailsTxt;
            //_PhotoIncluded = uscisResponse.PhotoIncluded;
            _Photo = uscisResponse.Photo;
            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;


            string tmpCode = string.Empty;

            if (!string.IsNullOrEmpty(_EligStatementCd)) tmpCode = _EligStatementCd.PadLeft(3, '0');

            if (tmpCode.Equals("005") || tmpCode.Equals("042")) _ProcessStatus = USCISProcessStatus.AwaitingUSCIS;
            else if (tmpCode.Equals("027") || tmpCode.Equals("038") || tmpCode.Equals("031") || tmpCode.Equals("036") || tmpCode.Equals("030")) _ProcessStatus = USCISProcessStatus.AwaitingClient;
            else if (tmpCode.Equals("008") || tmpCode.Equals("016") || tmpCode.Equals("043")) _ProcessStatus = USCISProcessStatus.CaseNeedsClose;

        }

        internal Response_Live(int idUSCISRequest, SubmitAdditionalVerificationResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;
            _EligStatementTxt = uscisResponse.EligibilityStatement;
            //may need to insert current status code here if we create a field to store it.
            _ProcessStatus = USCISProcessStatus.AwaitingUSCIS;

        }


        internal Response_Live(int idUSCISRequest, GetResolvedCasesResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            _Cases = uscisResponse.CaseList;
            _NumberOf = uscisResponse.CaseList.Length;
            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

            if (_NumberOf > 0 && _Cases != null)
            {

                _CaseNbr = _Cases[0].GetCaseNumber();
                _EligStatementCd = Convert.ToString(_Cases[0].GetMessageCode());
                _EligStatementTxt = _Cases[0].GetEligibility();
                _ResolveDate = Convert.ToString(_Cases[0].GetResolveDate());

                string rawData = string.Format("TypeOfCase={0}", _Cases[0].GetTypeOfCase());

                if (!string.IsNullOrEmpty(_ReturnStatusMsg)) rawData = string.Format("{0} ({1})", _EligStatementCd, rawData);

                _ReturnStatusMsg = rawData;

            }

        }

        internal Response_Live(int idUSCISRequest, GetCaseDetailsResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

            _RemoteCase = new Employee.RemoteCase_Live(uscisResponse);

            //serialize the EmpGtCseDetailsResp25 object for storing later
            var xs = new XmlSerializer(uscisResponse.GetType());
            _EmpCaseDetailsType = uscisResponse.GetType().Name;
            _EmpCaseDetails = new StringWriter();
            xs.Serialize(_EmpCaseDetails, uscisResponse);

        }

        internal Response_Live(int idUSCISRequest, ConfirmResolvedCasesReceivedResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

        }

        internal Response_Live(int idUSCISRequest, SubmitSsaReferralResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

            _ResolveDate = Convert.ToString(uscisResponse.ContactByDate);

            if (!(uscisResponse.LetterTypeCodeList == null || uscisResponse.LetterTypeCodeList.Length == 0))
            {
                _LetterCode = String.Join("|", uscisResponse.LetterTypeCodeList);
            }

            if (_LetterCode == null) { _LetterCode = "SSA_FAN"; }

            _ProcessStatus = USCISProcessStatus.SSAContested;

        }

        internal Response_Live(int idUSCISRequest, SubmitSsaReverifyResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

            //may need to store currentstatecode 

            _EligStatementCd = Convert.ToString(uscisResponse.MessageCode);
            _EligStatementTxt = uscisResponse.EligibilityStatement;
            _EligStatementDetailsTxt = uscisResponse.EligibilityStatementDetails;

            if (!(uscisResponse.LetterTypeCodeList == null || uscisResponse.LetterTypeCodeList.Length == 0))
            {
                _LetterCode = String.Join("|", uscisResponse.LetterTypeCodeList);
            }

            if (_LetterCode == null) { _LetterCode = "SSA_FAN"; }

            _ProcessStatus = USCISProcessStatus.AwaitingClient;

        }

        internal Response_Live(int idUSCISRequest, SubmitDhsReverifyResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

            //may need to store currentstatecode 
            _EligStatementCd = Convert.ToString(uscisResponse.MessageCode);
            _EligStatementTxt = uscisResponse.EligibilityStatement;
            _LastName = uscisResponse.SystemLastName;
            _FirstName = uscisResponse.SystemFirstName;

            if (!(uscisResponse.LetterTypeCodeList == null || uscisResponse.LetterTypeCodeList.Length == 0))
            {
                _LetterCode = String.Join("|", uscisResponse.LetterTypeCodeList);
            }
            if (_LetterCode == null) { _LetterCode = "DHS_FAN"; }


            _ProcessStatus = USCISProcessStatus.AwaitingClient;


        }

        internal Response_Live(int idUSCISRequest, ContinueDuplicateCaseWithChangeResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

            //may need to store currentstatecode 
            _EligStatementCd = Convert.ToString(uscisResponse.MessageCode);
            _EligStatementTxt = uscisResponse.EligibilityStatement;
            _EligStatementDetailsTxt = uscisResponse.EligibilityStatementDetails;
            _LastName = uscisResponse.SystemLastName;
            _FirstName = uscisResponse.SystemFirstName;

            if (!(uscisResponse.LetterTypeCodeList == null || uscisResponse.LetterTypeCodeList.Length == 0))
            {
                _LetterCode = String.Join("|", uscisResponse.LetterTypeCodeList);
            }

            _ProcessStatus = USCISProcessStatus.AwaitingClient;

        }

        internal Response_Live(int idUSCISRequest, ContinueDuplicateCaseResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

            //may need to store currentstatecode 
            _EligStatementCd = Convert.ToString(uscisResponse.MessageCode);
            _EligStatementTxt = uscisResponse.EligibilityStatement;
            _EligStatementDetailsTxt = uscisResponse.EligibilityStatementDetails;
            _LastName = uscisResponse.SystemLastName;
            _FirstName = uscisResponse.SystemFirstName;

            if (!(uscisResponse.LetterTypeCodeList == null || uscisResponse.LetterTypeCodeList.Length == 0))
            {
                _LetterCode = String.Join("|", uscisResponse.LetterTypeCodeList);
            }

            _ProcessStatus = USCISProcessStatus.AwaitingClient;

        }

        internal Response_Live(int idUSCISRequest, SubmitSsaResubmittalResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            _LastName = uscisResponse.SystemLastName;
            _FirstName = uscisResponse.SystemFirstName;
            _EligStatementCd = Convert.ToString(uscisResponse.MessageCode);
            _EligStatementTxt = uscisResponse.EligibilityStatement;
            //_AdditionalPerfind = uscisResponse.AdditionalPerfind;
            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

            string tmpCode = string.Empty;

            if (!string.IsNullOrEmpty(_EligStatementCd)) tmpCode = _EligStatementCd.PadLeft(3, '0');

            if (tmpCode.Equals("005") || tmpCode.Equals("038") || tmpCode.Equals("031") || tmpCode.Equals("036") || tmpCode.Equals("030") || tmpCode.Equals("P") || tmpCode.Equals("X") || tmpCode.Equals("042")) _ProcessStatus = USCISProcessStatus.AwaitingUSCIS;
            else if (tmpCode.Equals("008") || tmpCode.Equals("016") || tmpCode.Equals("043")) _ProcessStatus = USCISProcessStatus.ClosingCase;

        }

        internal Response_Live(int uscisRequestId, SubmitDhsReferralResult uscisResponse)
        {

            if (uscisRequestId < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = uscisRequestId;

            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;
            //add field to store contactbydate
            _ResolveDate = Convert.ToString(uscisResponse.ContactByDate);

            if (!(uscisResponse.LetterTypeCodeList == null || uscisResponse.LetterTypeCodeList.Length == 0))
            {
                _LetterCode = String.Join("|", uscisResponse.LetterTypeCodeList);
            }

            _ProcessStatus = USCISProcessStatus.DHSContested;

        }

        internal Response_Live(int idUSCISRequest, CloseCaseResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;
            //_UserField = uscisResponse.userField;
            //add currentstatecode to store
            _ProcessStatus = USCISProcessStatus.Closed;

        }

        internal Response_Live(int idUSCISRequest, GetCitizenshipStatusesResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            _CitizenshipCodes = uscisResponse.CitizenshipStatusList;
            _NumberOf = uscisResponse.CitizenshipStatusList.Length;
            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

        }

        internal Response_Live(int idUSCISRequest, GetAvailableDocumentTypesResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            _DocTypes = uscisResponse.DocumentTypeList;
            _NumberOf = uscisResponse.DocumentTypeList.Length;
            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

        }

        //internal Response_Live(int idUSCISRequest, EmpGetAllDataFieldsResp uscisResponse) {

        //    if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
        //    if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

        //    _USCISRequestId = idUSCISRequest;

        //    _Fields = uscisResponse.FieldArray;
        //    _NumberOf = uscisResponse.NumberOfFieldRecords;
        //    _ReturnStatus = uscisResponse.ReturnStatus;
        //    _ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

        //}

        internal Response_Live(int idUSCISRequest, ConfirmDocumentPhotoResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_CaseNbr = uscisResponse.CaseNbr;
            _EligStatementCd = Convert.ToString(uscisResponse.MessageCode);
            _EligStatementTxt = uscisResponse.EligibilityStatement;
            //_LetterCode = String.Join("|", uscisResponse.LetterTypeCodeList);
            if (!(uscisResponse.LetterTypeCodeList == null || uscisResponse.LetterTypeCodeList.Length == 0))
            {
                _LetterCode = String.Join("|", uscisResponse.LetterTypeCodeList);
            }
            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

            string tmpCode = string.Empty;

            if (!string.IsNullOrEmpty(_EligStatementCd)) tmpCode = _EligStatementCd.PadLeft(3, '0');

            if (tmpCode.Equals("029") || tmpCode.Equals("U") || tmpCode.Equals("038")) { if (_LetterCode == null) { _LetterCode = "DHS_FAN"; } }

            if (tmpCode.Equals("005") || tmpCode.Equals("042")) _ProcessStatus = USCISProcessStatus.AwaitingUSCIS;
            else if (tmpCode.Equals("029") || tmpCode.Equals("027") || tmpCode.Equals("038") || tmpCode.Equals("031") || tmpCode.Equals("036") || tmpCode.Equals("030") || tmpCode.Equals("P") || tmpCode.Equals("X")) _ProcessStatus = USCISProcessStatus.AwaitingClient;
            else if (tmpCode.Equals("008") || tmpCode.Equals("016") || tmpCode.Equals("043")) _ProcessStatus = USCISProcessStatus.ClosingCase;
        }

        //8-10-2015 kh deprecated in V28. 
        //internal Response_Live(int idUSCISRequest, SetUserPasswordResp uscisResponse) {

        //    if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
        //    if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

        //    _USCISRequestId = idUSCISRequest;

        //    _ReturnStatus = uscisResponse.ReturnStatus;
        //    _ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

        //}


        //internal Response_Live(int idUSCISRequest, EmpCpsVerifyConnectionResp uscisResponse) {

        //    if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
        //    if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

        //    _USCISRequestId = idUSCISRequest;

        //    _ReturnStatus = uscisResponse.ReturnStatus;
        //    _ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

        //}

        //10-28-2014 KH - EV 26 retrieve duplicate case list if response code = 41
        internal Response_Live(int idUSCISRequest, GetDuplicateCaseListResult uscisResponse)
        {

            if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            _USCISRequestId = idUSCISRequest;

            //_CaseList = uscisResponse.DuplicateCaseList;           

            if (uscisResponse.DuplicateCaseList != null)
            {

                StringBuilder strCase = new StringBuilder();

                foreach (DuplicateCaseRecord cs in uscisResponse.DuplicateCaseList)
                {

                    strCase.Append(cs.CaseNumber);
                    strCase.Append("|");
                }

                _CaseList = strCase.ToString();
            }// if uscisresponse.referralreasonlistarray is not null

            //_ReturnStatus = uscisResponse.ReturnStatus;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

            _ProcessStatus = USCISProcessStatus.AwaitingClient;

        }

        //4-3-2015 kevin hue new EV 27 response object
        //internal Response_Live(int idUSCISRequest, EmpSaveTNCNotificationResp uscisResponse)
        //{
        //    if (idUSCISRequest < 1) throw new ArgumentException("invalid uscis request id");
        //    if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

        //    _USCISRequestId = idUSCISRequest;

        //    _ReturnStatus = uscisResponse.ReturnStatus;
        //    _ReturnStatusMsg = uscisResponse.ReturnStatusMsg;
        //}



        #endregion

        #region Properties - Public

        public Employee.IRemoteCase RemoteCase { get { return _RemoteCase; } }

        //public EmpGtCseDetailsResp25 CaseDetails { get { return _CaseDetails; } }
        public StringWriter CaseDetails { get { return _EmpCaseDetails; } }
        public String CaseDetailsType { get { return _EmpCaseDetailsType; } }

        public com.vis_dhs.ICaseListArray[] Cases { get { return _Cases; } }
        public com.vis_dhs.ICitizenshipCodeListArray[] CitizenshipCodes { get { return _CitizenshipCodes; } }
        public com.vis_dhs.IDocTypeListArray[] DocTypes { get { return _DocTypes; } }
        //public com.vis_dhs.IFieldListArray[] Fields { get { return _Fields; } }
        public string CaseList { get { return _CaseList; } }

        #endregion
    }

}