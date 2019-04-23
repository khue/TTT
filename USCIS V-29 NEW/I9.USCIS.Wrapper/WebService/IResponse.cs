using System;

namespace I9.USCIS.Wrapper.WebService {

    public interface IResponse {

        #region Properties - Public

        string CaseNbr { get; }
        string LastName { get; }
        string FirstName { get; }
        string EligStatementCd { get; }
        string EligStatementTxt { get; }
        string EligStatementDetailsTxt { get; }
        string LetterTypeCode { get; }
        string AdditionalPerfind { get; }
        string PhotoIncluded { get; }
        byte[] Photo { get; }
        string DHSResponse { get; }
        string ResolveDate { get; }
        string UserField { get; }
        string ReturnStatusMsg { get; }
        string ReferralListArray { get; }
        byte[] FAN { get; }

        int USCISRequestId { get; }
        int ProcessStatus { get; }
        int ReturnStatus { get; }
        int HadIssue { get; }
        int NumberOfReferralReasons { get; }

        int NumberOf { get; }

        Employee.IRemoteCase RemoteCase { get; }

        com.vis_dhs.ICaseListArray[] Cases { get; }
        com.vis_dhs.ICitizenshipCodeListArray[] CitizenshipCodes { get; }
        com.vis_dhs.IDocTypeListArray[] DocTypes { get; }
        //com.vis_dhs.IFieldListArray[] Fields { get; }

        #endregion

        #region Properties - Public (Extended)

        int CaseStatus { get; }
        USCISCaseStatus CaseStatusEnum { get; }
        USCISProcessStatus ProcessStatusEnum { get; }

        #endregion

    }

}
