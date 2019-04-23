using System;

using com.vis_dhs.test;

namespace I9.USCIS.Wrapper.Employee {
    public class RemoteCase_Test : RemoteCase {

        #region Constructors


        internal RemoteCase_Test(GetCaseDetailsResult uscisResponse)
        {

            if (uscisResponse == null) throw new ArgumentNullException("invalid uscis response");

            //_CaseNbr = uscisResponse.CaseNumber;
            
            _CurrentState = uscisResponse.CurrentStateCode;

            //Initial Input
            _InitiCitizenshipDescr = uscisResponse.CitizenshipStatusCode;
            _InitiAlienNum = uscisResponse.AlienNumber;
            _InitiI94Num = uscisResponse.I94Number;
            //_InitiLogonId = uscisResponse.InitiLogonId;

            _InitiDt = uscisResponse.CaseCreatedDate;
            _InitiLastNm = uscisResponse.LastName;
            _InitiFirstNm = uscisResponse.FirstName;
            _InitiMiddleInitial = uscisResponse.MiddleInitial;
            _InitiMaidenNm = uscisResponse.OtherNamesUsed;
            _InitiSsn = uscisResponse.Ssn;
            _InitiBirthDt = uscisResponse.BirthDate;
            _InitiHireDt = uscisResponse.HireDate;
            _InitiDocType = Convert.ToString(uscisResponse.DocumentTypeId);
            _InitiDocExpireDt = Convert.ToDateTime(uscisResponse.DocumentExpirationDate);
            //_InitiUserField = uscisResponse.InitiUserField;

            //Initial Output
            _InitoLastNm = uscisResponse.LastName;
            _InitoFirstNm = uscisResponse.FirstName;
            _InitoMessageCode = Convert.ToString(uscisResponse.MessageCode);
            _InitoEligibStmt = uscisResponse.EligibilityStatement;
            _InitoEligibStmtDetails = uscisResponse.EligibilityStatementDetails;
            _LetterTypeCode = uscisResponse.LetterTypeCodeList;

            //Secondary (Additional) Input
            _AddiiComments = uscisResponse.AdditionalComments;
            //_AddiiLogonId = uscisResponse.AddiiLogonId; no longer exist in ev29
            //_AddiiDt = uscisResponse.AddiiDt; no longer exist in in ev29

            //_AddiiTransCd = uscisResponse.AddiiTransCd; No longer exist in ev 29

            //Secondary (Additional) Output
            _AddioEligibStmt = uscisResponse.EligibilityStatement;
            _AddioResolutionCode = uscisResponse.ResolutionCode;
            _AddioResolvedDt = Convert.ToDateTime(uscisResponse.ContactByDate);

            //Close Case Output
            _ClosureCdDescr = uscisResponse.ClosureReasonCode;
            //_ClosedBy = uscisResponse.ClosedBy;
            //_ClosedDt = uscisResponse.ClosedDt;

            //DHS Referral Input
            _InsReferiDt = Convert.ToDateTime(uscisResponse.ReferralDate);
            //_InsReferiBy = uscisResponse.InsReferiBy;

            //DHS Referral Output
            _InsReferoEligibStmt = uscisResponse.EligibilityStatement;
            _InsReferoResolutionCode = uscisResponse.ResolutionCode;
            _InsReferoResolvedDt = Convert.ToDateTime(uscisResponse.ContactByDate);

            //SSA Referral Input
            _SsaReferiDt = Convert.ToDateTime(uscisResponse.ReferralDate);
            //_SsaReferiBy = uscisResponse.SsaReferiBy;

            //SSA Automated Re-Submittal Input
            _SsaResubmiLastNm = uscisResponse.LastName;
            _SsaResubmiFirstNm = uscisResponse.FirstName;
            _SsaResubmiMiddleInitial = uscisResponse.MiddleInitial;
            _SsaResubmiMaidenNm = uscisResponse.OtherNamesUsed;
            _SsaResubmiSsn = uscisResponse.Ssn;
            _SsaResubmiBirthDt = uscisResponse.BirthDate;
            //_SsaResubmiLogonId = uscisResponse.SsaResubmiLogonId;
            //_SsaResubmiDt = uscisResponse.SsaResubmiDt;

            //SSA Referral Response Output
            _SsaResubmoMessageCode = Convert.ToString(uscisResponse.MessageCode);
            _SsaResubmoEligibStmt = uscisResponse.EligibilityStatement;
            _SsaReferralRespDueDate = Convert.ToDateTime(uscisResponse.ContactByDate);
            

            //Misc Output
            _ClientCompanyName = uscisResponse.ClientCompanyName;
            
            //_Photo = uscisResponse.Photo;
            //_ReturnStatus = uscisResponse.ResolutionCode;
            //_ReturnStatusMsg = uscisResponse.ReturnStatusMsg;

        }

        #endregion

    }

}