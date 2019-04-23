using System;

namespace I9.USCIS.Wrapper {

    public enum LogLevel : byte {
        debug,
        warning,
        informational,
        issue,
        error,
        critical
    }

    public enum USCISSystemId : int {
        Unknown = -1,
        Test = 1,
        Live = 2
    }

    public enum USCISCategoryId : int {
        Unknown = -1,
        Internal = 1,
        Standard = 2
    }

    public enum USCISActionId : int {
        AutoProcessInitiateCase = 1,
        InitiateCase = 2,
        CloseCase = 3,
        SubmitSecondVerification = 4,
        ResubmitSSAVerification = 5,
        SubmitSSAReferral = 6,
        SubmitDHSReferral = 7,
        AutoProcessSSAVerification = 8,
        EmpConfirmPhoto = 9,
        EmpUpdateSSALetterReceived = 10,
        EmpUpdateDHSLetterReceived = 11,
        EmpSSAReVerify = 12,
        EmpDHSReVerify = 13,
        EmpCitDHSReVerify = 14,
        EmpRetrievePhoto = 15,
        EmpDMVDHSReVerify = 16,
        EmpRetrieveFAN = 17,
        EmpGetCaseDetails = 19,
        EmpConfirmPhotoNoClose = 20,
        EmpGetDuplicateCaseList = 21,
        EmpDupCaseContinueWithoutChanges = 22,
        EmpDupCaseContinueWithChanges = 23,
        EmpSaveSSATNCNotification = 24,
        EmpSaveDHSTNCNotification = 25
    }

    public enum USCISClosureId : int {
        None = -1,
        IQ = 1,
        EUAUTH = 2,
        SELFT = 3,
        SNTERM = 4,
        EAUTH = 5,
        EELIG = 6,
        EFNC = 7,
        ENOACT = 8,
        EUNCNT = 9,
        TRMFNC = 10,
        EQUIT = 11,
        TERM = 12,
        NOACT = 13,
        UNCNT = 14,
        DUP = 15,
        INCDAT = 16,
        ISDP = 17,
        TECISS = 18,
        EARCLS = 19,
        EARNEW = 20,
        EDEXPD = 21,
        ENCLNT = 22,
        ENEMPD = 23
    }

    public enum USCISMethodId : byte {
        Unknown,
        EmpInitDABPVerif,
        EmpSubmitAdditVerif,
        EmpGetNextResolvedCaseNbrs,
        EmpGetCaseDetails,
        EmpAckReceiptOfResolvedCaseNbr,
        EmpSubSSAReferral,
        EmpSSAResubmittal,
        EmpSubDHSReferral,
        EmpCloseCase,
        EmpGetCitizenshipStatusCodes,
        EmpGetAvailableDocumentTypes,
        EmpGetAllDataFields,
        EmpCpsVerifyConnection,
        SetUserPassword,
        EmpConfirmPhoto,
        EmpUpdateSSALetterReceived,
        EmpUpdateDHSLetterReceived,
        EmpSSAReVerify,
        EmpDHSReVerify,
        EmpCitDHSReVerify,
        EmpRetrievePhoto,
        EmpDMVDHSReVerify,
        EmpRetrieveFAN,
        EmpGetDuplicateCaseList,
        EmpDupCaseContinueWithoutChanges,
        EmpDupCaseContinueWithChanges,
        EmpSaveSSATNCNotification,
        EmpSaveDHSTNCNotification
    }

    public enum USCISCaseStatus : int {
        Unknown = -1,
        Code_005 = 1,
        Code_008 = 2,
        Code_016 = 3,
        Code_027 = 4,
        Code_028 = 5,
        Code_A = 6,
        Code_U = 7,
        Code_C = 8,
        Code_I = 9,
        Code_N = 10,
        Code_O = 11,
        Code_S = 12,
        Code_034 = 13,
        Code_035 = 14,
        Code_D = 15,
        Code_038 = 16,
        Code_036 = 17,
        Code_031 = 18,
        Code_030 = 19,
        Code_029 = 20,
        Code_P = 21,
        Code_X = 22,
        Code_41 = 23,
        Code_42 = 24,
        Code_43 = 25

    }

    public enum USCISProcessStatus : int {
        Unknown = -1,
        AwaitingUSCIS = 1,
        AwaitingClient = 2,
        SSAContested = 3,
        DHSContested = 4,
        Closed = 5,
        SSAResubmittalInProgress = 6,   // front end only
        ManualInterventionRequired = 7, // front end only
        ClosingCase = 8,
        CaseNeedsClose = 9,
        AwaitingPhotoConfirmation = 12
    }

    public enum USCISAcknowledgeCase : int {
        SSAResponse = 1,
        DHSSecondaryResponse = 2,
        DHSThirdStepResponse = 3
    }

    [FlagsAttribute]
    public enum USCISRequestValidation : byte {
        EmployeeId = 1,
        I9Id = 2,
        CaseNumber = 4,
        All = EmployeeId | I9Id | CaseNumber
    }

    public enum OnDemandEmailCodes : int {
        Unknown=-1,
        Code_005=12,
        Code_008=13,
        Code_016=14,
        Code_027=15,
        Code_028=16,
        Code_A=19,
        Code_U=26,
        Code_C=20,
        Code_I=22,
        Code_N=23,
        Code_O=24,
        Code_S=25,
        Code_034=17,
        Code_035=18,
        Code_D=21,
        Code_038=39,
        Code_036=47, 
        Code_031=48,
        Code_030=47,
        Code_029=39,
        Code_P =49, 
        Code_X =50,
        Code_41 =67,
        Code_42 = 70,
        Code_43 = 71
    }

}
