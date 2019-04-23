using System;

namespace I9.USCIS.Wrapper.WebService {

    public abstract class Response {

        #region Members

        protected string _CaseNbr = null;
        protected string _LastName = null;
        protected string _FirstName = null;
        protected string _EligStatementCd = null;
        protected string _EligStatementTxt = null;
        protected string _EligStatementDetailsTxt = null;
        //protected string[] _LetterTypeCode = null;
        protected string _LetterCode = null;
        protected string _AdditionalPerfind = null;
        protected string _PhotoIncluded = null;
        protected byte[] _Photo = null;
        protected string _DHSResponse = null;
        protected string _ResolveDate = null;
        protected string _UserField = null;
        protected string _ReturnStatusMsg = null;
        protected int _NumberOfReferralReasons = 0;
        protected string _ReferralListArray = null;
        protected byte[] _FAN = null;

        protected USCISProcessStatus _ProcessStatus = USCISProcessStatus.Unknown;

        protected int _USCISRequestId = -1;
        protected int _ReturnStatus = -1;
        protected int _HadIssue = 0;

        protected int _NumberOf = 0;

        #endregion

        #region Properties - Public

        public string CaseNbr { get { return _CaseNbr; } }
        public string LastName { get { return _LastName; } }
        public string FirstName { get { return _FirstName; } }
        public string EligStatementCd { get { return _EligStatementCd; } }
        public string EligStatementTxt { get { return _EligStatementTxt; } }
        public string EligStatementDetailsTxt { get { return _EligStatementDetailsTxt; } }
        //public string[] LetterTypeCode { get { return _LetterTypeCode; } }
        public string LetterTypeCode { get { return _LetterCode; } }
        public string AdditionalPerfind { get { return _AdditionalPerfind; } }
        public string PhotoIncluded { get { return _PhotoIncluded; } }
        public byte[] Photo { get { return _Photo; } }
        public string DHSResponse { get { return _DHSResponse; } }
        public string ResolveDate { get { return _ResolveDate; } }
        public string UserField { get { return _UserField; } }
        public string ReturnStatusMsg { get { return _ReturnStatusMsg; } }
        public int NumberOfReferralReasons { get { return _NumberOfReferralReasons; } }
        public string ReferralListArray { get { return _ReferralListArray; } }
        public byte[] FAN { get { return _FAN; } }
        
        public int USCISRequestId { get { return _USCISRequestId; } }
        public int ProcessStatus { get { return (int)_ProcessStatus; } }
        public int ReturnStatus { get { return _ReturnStatus; } }
        public int HadIssue { get { return _HadIssue; } }

        public int NumberOf { get { return _NumberOf; } }
        
        #endregion

        #region Properties - Public (Extended)

        public int CaseStatus {

            get {

                string tmpCode1 = string.Empty;
                string tmpCode2 = string.Empty;

                if (!string.IsNullOrEmpty(_EligStatementCd)) tmpCode1 = _EligStatementCd.PadLeft(3, '0');
                if (!string.IsNullOrEmpty(_DHSResponse)) tmpCode2 = _DHSResponse.ToLower();

                if (tmpCode1.Equals("005")) return (int)USCISCaseStatus.Code_005;
                else if (tmpCode1.Equals("008")) return (int)USCISCaseStatus.Code_008;
                else if (tmpCode1.Equals("016")) return (int)USCISCaseStatus.Code_016;
                else if (tmpCode1.Equals("027")) return (int)USCISCaseStatus.Code_027;
                else if (tmpCode1.Equals("028")) return (int)USCISCaseStatus.Code_028;
                else if (tmpCode1.Equals("029")) return (int)USCISCaseStatus.Code_029;
                else if (tmpCode1.Equals("038")) return (int)USCISCaseStatus.Code_038;
                else if (tmpCode1.Equals("036")) return (int)USCISCaseStatus.Code_036;
                else if (tmpCode1.Equals("031")) return (int)USCISCaseStatus.Code_031;
                else if (tmpCode1.Equals("030")) return (int)USCISCaseStatus.Code_030;
                else if (tmpCode1.Equals("041")) return (int)USCISCaseStatus.Code_41;
                else if (tmpCode1.Equals("042")) return (int)USCISCaseStatus.Code_42;
                else if (tmpCode1.Equals("043")) return (int)USCISCaseStatus.Code_43;
                else if (tmpCode2.Equals("a")) return (int)USCISCaseStatus.Code_A;
                else if (tmpCode2.Equals("u")) return (int)USCISCaseStatus.Code_U;
                else if (tmpCode2.Equals("c")) return (int)USCISCaseStatus.Code_C;
                else if (tmpCode2.Equals("i")) return (int)USCISCaseStatus.Code_I;
                else if (tmpCode2.Equals("n")) return (int)USCISCaseStatus.Code_N;
                else if (tmpCode2.Equals("o")) return (int)USCISCaseStatus.Code_O;
                else if (tmpCode2.Equals("s")) return (int)USCISCaseStatus.Code_S;
                else if (tmpCode2.Equals("p")) return (int)USCISCaseStatus.Code_P;
                else if (tmpCode2.Equals("x")) return (int)USCISCaseStatus.Code_X;
                else return -1;

            }

        }

        public USCISCaseStatus CaseStatusEnum {

            get {

                string tmpCode1 = string.Empty;
                string tmpCode2 = string.Empty;

                if (!string.IsNullOrEmpty(_EligStatementCd)) tmpCode1 = _EligStatementCd.PadLeft(3, '0');
                if (!string.IsNullOrEmpty(_DHSResponse)) tmpCode2 = _DHSResponse.ToLower();

                if (tmpCode1.Equals("005")) return USCISCaseStatus.Code_005;
                else if (tmpCode1.Equals("008")) return USCISCaseStatus.Code_008;
                else if (tmpCode1.Equals("016")) return USCISCaseStatus.Code_016;
                else if (tmpCode1.Equals("027")) return USCISCaseStatus.Code_027;
                else if (tmpCode1.Equals("028")) return USCISCaseStatus.Code_028;
                else if (tmpCode1.Equals("029")) return USCISCaseStatus.Code_029;
                else if (tmpCode1.Equals("038")) return USCISCaseStatus.Code_038;
                else if (tmpCode1.Equals("036")) return USCISCaseStatus.Code_036;
                else if (tmpCode1.Equals("031")) return USCISCaseStatus.Code_031;
                else if (tmpCode1.Equals("030")) return USCISCaseStatus.Code_030;
                else if (tmpCode1.Equals("041")) return USCISCaseStatus.Code_41;
                else if (tmpCode1.Equals("042")) return USCISCaseStatus.Code_42;
                else if (tmpCode1.Equals("043")) return USCISCaseStatus.Code_43;
                else if (tmpCode2.Equals("a")) return USCISCaseStatus.Code_A;
                else if (tmpCode2.Equals("u")) return USCISCaseStatus.Code_U;
                else if (tmpCode2.Equals("c")) return USCISCaseStatus.Code_C;
                else if (tmpCode2.Equals("i")) return USCISCaseStatus.Code_I;
                else if (tmpCode2.Equals("n")) return USCISCaseStatus.Code_N;
                else if (tmpCode2.Equals("o")) return USCISCaseStatus.Code_O;
                else if (tmpCode2.Equals("s")) return USCISCaseStatus.Code_S;
                else if (tmpCode2.Equals("p")) return USCISCaseStatus.Code_P;
                else if (tmpCode2.Equals("x")) return USCISCaseStatus.Code_X;
                else return USCISCaseStatus.Unknown;

            }

        }

        public OnDemandEmailCodes EmailEnum {

            get
            {

                string tmpCode1 = string.Empty;
                string tmpCode2 = string.Empty;

                if (!string.IsNullOrEmpty(_EligStatementCd)) tmpCode1 = _EligStatementCd.PadLeft(3, '0');
                if (!string.IsNullOrEmpty(_DHSResponse)) tmpCode2 = _DHSResponse.ToLower();

                if (tmpCode1.Equals("005")) return OnDemandEmailCodes.Code_005;
                else if (tmpCode1.Equals("008")) return OnDemandEmailCodes.Code_008;
                else if (tmpCode1.Equals("016")) return OnDemandEmailCodes.Code_016;
                else if (tmpCode1.Equals("027")) return OnDemandEmailCodes.Code_027;
                else if (tmpCode1.Equals("028")) return OnDemandEmailCodes.Code_028;
                else if (tmpCode1.Equals("029")) return OnDemandEmailCodes.Code_U;
                else if (tmpCode1.Equals("038")) return OnDemandEmailCodes.Code_038;
                else if (tmpCode1.Equals("036")) return OnDemandEmailCodes.Code_036;
                else if (tmpCode1.Equals("031")) return OnDemandEmailCodes.Code_031;
                else if (tmpCode1.Equals("030")) return OnDemandEmailCodes.Code_030;
                else if (tmpCode1.Equals("041")) return OnDemandEmailCodes.Code_41;
                else if (tmpCode1.Equals("042")) return OnDemandEmailCodes.Code_42;
                else if (tmpCode1.Equals("043")) return OnDemandEmailCodes.Code_43;
                else if (tmpCode1.Equals("p")) return OnDemandEmailCodes.Code_P;
                else if (tmpCode1.Equals("x")) return OnDemandEmailCodes.Code_X;
                else if (tmpCode2.Equals("a")) return OnDemandEmailCodes.Code_A;
                else if (tmpCode2.Equals("u")) return OnDemandEmailCodes.Code_U;
                else if (tmpCode2.Equals("c")) return OnDemandEmailCodes.Code_C;
                else if (tmpCode2.Equals("i")) return OnDemandEmailCodes.Code_I;
                else if (tmpCode2.Equals("n")) return OnDemandEmailCodes.Code_N;
                else if (tmpCode2.Equals("o")) return OnDemandEmailCodes.Code_O;
                else if (tmpCode2.Equals("s")) return OnDemandEmailCodes.Code_S;
                else if (tmpCode2.Equals("p")) return OnDemandEmailCodes.Code_P;
                else if (tmpCode2.Equals("x")) return OnDemandEmailCodes.Code_X;
                else return OnDemandEmailCodes.Unknown;

            }

        }

        public USCISProcessStatus ProcessStatusEnum { get { return _ProcessStatus; } }

        #endregion

    }

}
