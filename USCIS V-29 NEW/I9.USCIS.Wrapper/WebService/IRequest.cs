using System;

namespace I9.USCIS.Wrapper.WebService {

    public interface IRequest : IDisposable {

        #region Properties - Public

        USCISConfiguration Configuration { get; }
        ProcessIds Pids { get; }
        string Version { get; }

        int EmployeeId { get; set; }
        int I9Id { get; set; }
        int QueueErrorId { get; set; }
        int QueueFutureId { get; set; }
        int TransactionId { get; set; }

        string CaseNumber { get; set; }

        IResponse WebServiceResponse { get; }

        #endregion

        #region Methods - Public (USCISLog)

        void Capture(LogLevel logLevel, string message);
        void Capture(LogLevel logLevel, string message, Exception ex);
        void Capture(LogLevel logLevel, string classModule, string message);
        void Capture(LogLevel logLevel, string classModule, string message, Exception ex);
        void Capture(LogLevel logLevel, string classModule, string friendlyMessage, string message, Exception ex);

        #endregion

        #region Methods - Public (Gets)

        string GetClosureCode(USCISClosureId idClosure);
        Employee.LocalCase GetLocalEmployeeCase();
        bool GetSystemIdentities();
        int GetTransactionId();

        #endregion

        #region Methods - Public (Link)

        void InsertLink();

        #endregion

        #region Methods - Public (Web Service - Gets)

        bool GetNextCase();

        #endregion

        #region Methods - Public (Web Service - General Updates)

        bool AcknowledgeReceiptOfCaseNumber(USCISAcknowledgeCase ac);
        bool AcknowledgeReceiptOfCaseNumber(string ac, string strTypeOfCase);
        //bool SetAccountPassword(string newPassword);

        #endregion

        #region Methods - Public (Web Service - Push)

        bool InitiateCase();
        bool SubmitSSAReferral();
        bool ResubmitSSAVerification();
        bool SubmitDHSReferral();
        bool SubmitSecondVerification(string comments);
        bool CloseCase(USCISClosureId idClosure, string currentlyEmployed);
        bool EmpConfirmPhoto();
        //bool EmpUpdateSSALetterReceived(string LetterTypeCode);
        //bool EmpUpdateDHSLetterReceived(string LetterTypeCode);
        bool EmpSSAReVerify();
        bool EmpDHSReVerify();
        bool EmpCitDHSReVerify();
        bool EmpRetrievePhoto();
        bool EmpDMVDHSReVerify();
        bool EmpRetrieveFAN(string LetterTypeCode);
        bool EmpGetCaseDetails();
        
        bool EmpGetDuplicateCaseList();
        bool EmpDupCaseContinueWithChanges();
        bool EmpDupCaseContinueWithoutChanges();
        bool EmpSaveSSATNCNotification();
        bool EmpSaveDHSTNCNotification();

        #endregion
                
        #region Methods - Public (Local Updates)

        void UpdateRequest(int idUSCISRequest, int idUSCISTransaction, int idEmployee, int idI9);
        void UpdateTransaction(USCISCaseStatus idUSCISCaseStatus, USCISProcessStatus idUSCISProcessStatus);

        #endregion

    }

}
