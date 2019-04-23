using System;

namespace I9.USCIS.Wrapper.Employee {

    [Serializable]
    public abstract class RemoteCase : IRemoteCase {

        #region Members

        public RemoteCase()
        { 
        
        }

        protected string _CaseNbr = null;
        protected string _CurrentState = null;

        //Initial Input
        protected string _InitiCitizenshipDescr = null;
        protected string _InitiAlienNum = null;
        protected string _InitiI94Num = null;
        protected string _InitiLogonId = null;
        protected DateTime _InitiDt;
        protected string _InitiLastNm = null;
        protected string _InitiFirstNm = null;
        protected string _InitiMiddleInitial = null;
        protected string _InitiMaidenNm = null;
        protected string _InitiSsn = null;
        protected DateTime _InitiBirthDt;
        protected DateTime _InitiHireDt;
        protected string _InitiDocType = null;
        protected DateTime _InitiDocExpireDt;
        protected string _InitiUserField = null;

        //Initial Output
        protected string _InitoLastNm = null;
        protected string _InitoFirstNm = null;
        protected string _InitoMessageCode = null;
        protected string _InitoEligibStmt = null;
        protected string _InitoEligibStmtDetails = null;
        protected string[] _LetterTypeCode = null;

        //Secondary (Additional) Input = null;
        protected string _AddiiComments = null;
        protected string _AddiiLogonId = null;
        protected DateTime _AddiiDt;
        protected string _AddiiTransCd = null;

        //Secondary (Additional) Output = null;
        protected string _AddioEligibStmt = null;
        protected string _AddioResolutionCode = null;
        protected DateTime _AddioResolvedDt;

        //Close Case Output
        protected string _ClosureCdDescr = null;
        protected string _ClosedBy = null;
        protected DateTime _ClosedDt;

        //DHS Referral Input
        protected DateTime _InsReferiDt;
        protected string _InsReferiBy = null;

        //DHS Referral Output
        protected string _InsReferoEligibStmt = null;
        protected string _InsReferoResolutionCode = null;
        protected DateTime _InsReferoResolvedDt;

        //SSA Referral Input
        protected DateTime _SsaReferiDt;
        protected string _SsaReferiBy = null;

        //SSA Automated Re-Submittal Input = null;
        protected string _SsaResubmiLastNm = null;
        protected string _SsaResubmiFirstNm = null;
        protected string _SsaResubmiMiddleInitial = null;
        protected string _SsaResubmiMaidenNm = null;
        protected string _SsaResubmiSsn = null;
        protected DateTime _SsaResubmiBirthDt;
        protected string _SsaResubmiLogonId = null;
        protected DateTime _SsaResubmiDt;

        //SSA Referral Response Output
        protected string _SsaResubmoMessageCode = null;
        protected string _SsaResubmoEligibStmt = null;
        protected DateTime _SsaReferralRespDueDate;

        //Misc Output
        protected string _ClientCompanyName = null;
        protected string _PhotoIncluded = null;
        protected byte[] _Photo = null;
        protected int _ReturnStatus = 0;
        protected string _ReturnStatusMsg = null;

        #endregion

        #region Properties - Public

        public string CaseNbr { get { return _CaseNbr; } }
        public string CurrentState { get { return _CurrentState; } }

        #endregion

        #region Properties - Public (Initial Input)

        public string InitiCitizenshipDescr { get { return _InitiCitizenshipDescr; } }
        public string InitiAlienNum { get { return _InitiAlienNum; } }
        public string InitiI94Num { get { return _InitiI94Num; } }
        public string InitiLogonId { get { return _InitiLogonId; } }
        public DateTime InitiDt { get { return _InitiDt; } }
        public string InitiLastNm { get { return _InitiLastNm; } }
        public string InitiFirstNm { get { return _InitiFirstNm; } }
        public string InitiMiddleInitial { get { return _InitiMiddleInitial; } }
        public string InitiMaidenNm { get { return _InitiMaidenNm; } }
        public string InitiSsn { get { return _InitiSsn; } }
        public DateTime InitiBirthDt { get { return _InitiBirthDt; } }
        public DateTime InitiHireDt { get { return _InitiHireDt; } }
        public string InitiDocType { get { return _InitiDocType; } }
        public DateTime InitiDocExpireDt { get { return _InitiDocExpireDt; } }
        public string InitiUserField { get { return _InitiUserField; } }

        #endregion

        #region Properties - Public (Initial Output)

        public string InitoLastNm { get { return _InitoLastNm; } }
        public string InitoFirstNm { get { return _InitoFirstNm; } }
        public string InitoMessageCode { get { return _InitoMessageCode; } }
        public string InitoEligibStmt { get { return _InitoEligibStmt; } }
        public string InitoEligibStmtDetails { get { return _InitoEligibStmtDetails; } }
        public string[] LetterTypeCode { get { return _LetterTypeCode; } }

        #endregion

        #region Properties - Public (Secondary [Additional] Input)

        public string AddiiComments { get { return _AddiiComments; } }
        public string AddiiLogonId { get { return _AddiiLogonId; } }
        public DateTime AddiiDt { get { return _AddiiDt; } }
        public string AddiiTransCd { get { return _AddiiTransCd; } }

        #endregion

        #region Properties - Public (Secondary [Additional] Output)

        public string AddioEligibStmt { get { return _AddioEligibStmt; } }
        public string AddioResolutionCode { get { return _AddioResolutionCode; } }
        public DateTime AddioResolvedDt { get { return _AddioResolvedDt; } }

        #endregion

        #region Properties - Public (Close Case Output)

        public string ClosureCdDescr { get { return _ClosureCdDescr; } }
        public string ClosedBy { get { return _ClosedBy; } }
        public DateTime ClosedDt { get { return _ClosedDt; } }

        #endregion

        #region Properties - Public (DHS Referral Input)

        public DateTime InsReferiDt { get { return _InsReferiDt; } }
        public string InsReferiBy { get { return _InsReferiBy; } }

        #endregion

        #region Properties - Public (DHS Referral Output)

        public string InsReferoEligibStmt { get { return _InsReferoEligibStmt; } }
        public string InsReferoResolutionCode { get { return _InsReferoResolutionCode; } }
        public DateTime InsReferoResolvedDt { get { return _InsReferoResolvedDt; } }

        #endregion

        #region Properties - Public (SSA Referral Input)

        public DateTime SsaReferiDt { get { return _SsaReferiDt; } }
        public string SsaReferiBy { get { return _SsaReferiBy; } }

        #endregion

        #region Properties - Public (SSA Automated Re-Submittal Input)

        public string SsaResubmiLastNm { get { return _SsaResubmiLastNm; } }
        public string SsaResubmiFirstNm { get { return _SsaResubmiFirstNm; } }
        public string SsaResubmiMiddleInitial { get { return _SsaResubmiMiddleInitial; } }
        public string SsaResubmiMaidenNm { get { return _SsaResubmiMaidenNm; } }
        public string SsaResubmiSsn { get { return _SsaResubmiSsn; } }
        public DateTime SsaResubmiBirthDt { get { return _SsaResubmiBirthDt; } }
        public string SsaResubmiLogonId { get { return _SsaResubmiLogonId; } }
        public DateTime SsaResubmiDt { get { return _SsaResubmiDt; } }

        #endregion

        #region Properties - Public (SSA Referral Response Output)

        public string SsaResubmoMessageCode { get { return _SsaResubmoMessageCode; } }
        public string SsaResubmoEligibStmt { get { return _SsaResubmoEligibStmt; } }
        public DateTime SsaReferralRespDueDate { get { return _SsaReferralRespDueDate; } }

        #endregion

        #region Properties - Public (Misc Output)

        public string ClientCompanyName { get { return _ClientCompanyName; } }
        public string PhotoIncluded { get { return _PhotoIncluded; } }
        public byte[] Photo { get { return _Photo; } }
        public int ReturnStatus { get { return _ReturnStatus; } }
        public string ReturnStatusMsg { get { return _ReturnStatusMsg; } }

        #endregion

    }

}
