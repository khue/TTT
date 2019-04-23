using System;
using System.Collections.Generic;
using System.Text;

namespace I9.USCIS.Wrapper.Employee {

    public interface IRemoteCase {

        #region Properties - Public

        string CaseNbr { get; }
        string CurrentState { get; }

        #endregion

        #region Properties - (Initial Input)

        string InitiCitizenshipDescr { get; }
        string InitiAlienNum { get; }
        string InitiI94Num { get; }
        string InitiLogonId { get; }
        DateTime InitiDt { get; }
        string InitiLastNm { get; }
        string InitiFirstNm { get; }
        string InitiMiddleInitial { get; }
        string InitiMaidenNm { get; }
        string InitiSsn { get; }
        DateTime InitiBirthDt { get; }
        DateTime InitiHireDt { get; }
        string InitiDocType { get; }
        DateTime InitiDocExpireDt { get; }
        string InitiUserField { get; }

        #endregion

        #region Properties - (Initial Output)

        string InitoLastNm { get; }
        string InitoFirstNm { get; }
        string InitoMessageCode { get; }
        string InitoEligibStmt { get; }
        string InitoEligibStmtDetails { get; }
        string[] LetterTypeCode { get; }

        #endregion

        #region Properties - (Secondary [Additional] Input)

        string AddiiComments { get; }
        string AddiiLogonId { get; }
        DateTime AddiiDt { get; }
        string AddiiTransCd { get; }

        #endregion

        #region Properties - (Secondary [Additional] Output)

        string AddioEligibStmt { get; }
        string AddioResolutionCode { get; }
        DateTime AddioResolvedDt { get; }

        #endregion

        #region Properties - (Close Case Output)

        string ClosureCdDescr { get; }
        string ClosedBy { get; }
        DateTime ClosedDt { get; }

        #endregion

        #region Properties - (DHS Referral Input)

        DateTime InsReferiDt { get; }
        string InsReferiBy { get; }

        #endregion

        #region Properties - (DHS Referral Output)

        string InsReferoEligibStmt { get; }
        string InsReferoResolutionCode { get; }
        DateTime InsReferoResolvedDt { get; }

        #endregion

        #region Properties - (SSA Referral Input)

        DateTime SsaReferiDt { get; }
        string SsaReferiBy { get; }

        #endregion

        #region Properties - (SSA Automated Re-Submittal Input)

        string SsaResubmiLastNm { get; }
        string SsaResubmiFirstNm { get; }
        string SsaResubmiMiddleInitial { get; }
        string SsaResubmiMaidenNm { get; }
        string SsaResubmiSsn { get; }
        DateTime SsaResubmiBirthDt { get; }
        string SsaResubmiLogonId { get; }
        DateTime SsaResubmiDt { get; }

        #endregion

        #region Properties - (SSA Referral Response Output)

        string SsaResubmoMessageCode { get; }
        string SsaResubmoEligibStmt { get; }
        DateTime SsaReferralRespDueDate { get; }

        #endregion

        #region Properties - (Misc Output)

        string ClientCompanyName { get; }
        string PhotoIncluded { get; }
        byte[] Photo { get; }
        int ReturnStatus { get; }
        string ReturnStatusMsg { get; }

        #endregion

    }

}
