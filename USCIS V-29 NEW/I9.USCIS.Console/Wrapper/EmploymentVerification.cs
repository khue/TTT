using System;

using I9.USCIS.Wrapper;
using I9.USCIS.Wrapper.WebService;

namespace I9.USCIS.Console.Wrapper {

    internal class EmploymentVerification {

        #region Members

        private IRequest _Request;

        #endregion

        #region Constructors

        internal EmploymentVerification(int idConsole, USCISSystemId idSystem, int idEmployee, int idI9) {

            if (idSystem == USCISSystemId.Live) _Request = USCISFactory.GetLiveRequest(idEmployee, idI9);
            else if (idSystem == USCISSystemId.Test) _Request = USCISFactory.GetTestRequest(idEmployee, idI9);
            else throw new ArgumentException("The given uscis system id is not defined");

            _Request.Pids.ConsoleId = idConsole;

        }

        #endregion

        #region Methods - Public

        public void Dispose() {
      
            if (_Request != null) _Request.Dispose();

            System.GC.SuppressFinalize(this);

        }

        #endregion

        #region Methods - Public (Web Service - Server Processes)

        public bool AutoProcessInitiateCase() {

            if (!_Request.InitiateCase()) return false;

            if (_Request.WebServiceResponse == null) return true;

            _Request.InsertLink();

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Unknown) return false;
            if (cs == USCISCaseStatus.Code_005) return true;
            if (cs == USCISCaseStatus.Code_027) return true;
            if (cs == USCISCaseStatus.Code_028) return true;
            if (cs == USCISCaseStatus.Code_029) return true;
            if (cs == USCISCaseStatus.Code_038) return true;
            if (cs == USCISCaseStatus.Code_031) return true;
            if (cs == USCISCaseStatus.Code_030) return true;
            if (cs == USCISCaseStatus.Code_036) return true;
            if (cs == USCISCaseStatus.Code_42) return true;
            if (cs == USCISCaseStatus.Code_U) return true;
            if (cs == USCISCaseStatus.Code_C) return true;
            if (cs == USCISCaseStatus.Code_I) return true;
            if (cs == USCISCaseStatus.Code_N) return true;
            if (cs == USCISCaseStatus.Code_S) return true;
            if (cs == USCISCaseStatus.Code_P) return true;
            if (cs == USCISCaseStatus.Code_X) return true;

            //11-3-2014 kevin EV 26 if duplicate case then grab the duplicate case list
            if (cs == USCISCaseStatus.Code_41)
            {
                _Request.EmpGetDuplicateCaseList();
                return true;
            }

            //2-28-2018 if photo match then pull photo. new in ev29
            if (cs == USCISCaseStatus.Code_031)
                {
                    if (!_Request.EmpRetrievePhoto()) return false;
                }

           

            _Request.CaseNumber = _Request.WebServiceResponse.CaseNbr;

            if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;

            _Request.InsertLink();

            return true;

        }

        public bool AutoProcessSSAVerification(string caseNumber) {

            _Request.CaseNumber = caseNumber;

            if (!_Request.ResubmitSSAVerification()) return false;

            if (_Request.WebServiceResponse == null) return true;

            _Request.InsertLink();

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Unknown) return false;
            if (cs == USCISCaseStatus.Code_005) return true;
            if (cs == USCISCaseStatus.Code_027) return true;
            if (cs == USCISCaseStatus.Code_028) return true;
            if (cs == USCISCaseStatus.Code_029) return true;
            if (cs == USCISCaseStatus.Code_038) return true;
            if (cs == USCISCaseStatus.Code_031) return true;
            if (cs == USCISCaseStatus.Code_030) return true;
            if (cs == USCISCaseStatus.Code_036) return true;
            if (cs == USCISCaseStatus.Code_42) return true;
            if (cs == USCISCaseStatus.Code_U) return true;
            if (cs == USCISCaseStatus.Code_C) return true;
            if (cs == USCISCaseStatus.Code_I) return true;
            if (cs == USCISCaseStatus.Code_N) return true;
            if (cs == USCISCaseStatus.Code_S) return true;
            if (cs == USCISCaseStatus.Code_P) return true;
            if (cs == USCISCaseStatus.Code_X) return true;

            //11-3-2014 kevin EV 26 if duplicate case then grab the duplicate case list
            if (cs == USCISCaseStatus.Code_41)
            {
                _Request.EmpGetDuplicateCaseList();
                return true;
            }

            if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;

            _Request.InsertLink();

            return true;

        }

        #endregion

        #region Methods - Public (Web Service - Manual Processes)

        public bool InitiateCase() {

            if (!_Request.InitiateCase()) return false;

           

            _Request.InsertLink();

            return true;

        }

        public bool SubmitSSAReferral(string caseNumber) {

            _Request.CaseNumber = caseNumber;

            if (!_Request.SubmitSSAReferral()) return false;

            _Request.InsertLink();

            return true;

        }

        public bool EmpUpdateSSALetterReceived(string caseNumber, string LetterTypeCode)
        {

            _Request.CaseNumber = caseNumber;

           // if (!_Request.EmpUpdateSSALetterReceived(LetterTypeCode)) return false;

            _Request.InsertLink();

            return true;

        }


        public bool EmpUpdateDHSLetterReceived(string caseNumber, string LetterTypeCode)
        {

            _Request.CaseNumber = caseNumber;

           //if (!_Request.EmpUpdateDHSLetterReceived(LetterTypeCode)) return false;

            _Request.InsertLink();

            return true;

        }

        public bool ResubmitSSAVerification(string caseNumber) {

            _Request.CaseNumber = caseNumber;

            if (!_Request.ResubmitSSAVerification()) return false;

            _Request.InsertLink();

            return true;

        }

        public bool SubmitDHSReferral(string caseNumber) {

            _Request.CaseNumber = caseNumber;

            if (!_Request.SubmitDHSReferral()) return false;

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Code_031)
            {
                if (!_Request.EmpRetrievePhoto()) return false;
            }

            _Request.InsertLink();

            return true;

        }

        public bool SubmitSecondVerification(string caseNumber, string comments) {

            _Request.CaseNumber = caseNumber;

            if (!_Request.SubmitSecondVerification(comments)) return false;

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Code_008 || cs == USCISCaseStatus.Code_016 || cs == USCISCaseStatus.Code_A || cs == USCISCaseStatus.Code_O)
            {
                if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;
            }

            _Request.InsertLink();

            return true;

        }

        public bool CloseCase(int idClosure, string caseNumber, string currentlyEmployed) {

            _Request.CaseNumber = caseNumber;

            if (!_Request.CloseCase(_ConvertToUSCISClosureId(idClosure), currentlyEmployed)) return false;

            _Request.InsertLink();

            return true;

        }

        public bool EmpConfirmPhoto(string caseNumber, Boolean autoclose)
        {
            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpConfirmPhoto()) return false;
                        
            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (autoclose) {
                if (cs == USCISCaseStatus.Code_008 || cs == USCISCaseStatus.Code_016 || cs == USCISCaseStatus.Code_A || cs == USCISCaseStatus.Code_O)
                {
                    if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;
                }
            }

            _Request.InsertLink();

            return true;

        }

        public bool EmpSSAReVerify(string caseNumber)
        {
            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpSSAReVerify()) return false;

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Code_008 || cs == USCISCaseStatus.Code_016 || cs == USCISCaseStatus.Code_A || cs == USCISCaseStatus.Code_O)
            {
                if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;
            }

            if (cs == USCISCaseStatus.Code_031)
            {
                if (!_Request.EmpRetrievePhoto()) return false;
            }

            _Request.InsertLink();

            return true;

        }

        public bool EmpDHSReVerify(string caseNumber)
        {
            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpDHSReVerify()) return false;

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Code_031)
            {
                if (!_Request.EmpRetrievePhoto()) return false;
            }

            if (cs == USCISCaseStatus.Code_008 || cs == USCISCaseStatus.Code_016 || cs == USCISCaseStatus.Code_A || cs == USCISCaseStatus.Code_O)
            {
                if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;
            }

            _Request.InsertLink();

            return true;

        }

        public bool EmpCitDHSReVerify(string caseNumber)
        {
            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpCitDHSReVerify()) return false;

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Code_008 || cs == USCISCaseStatus.Code_016 || cs == USCISCaseStatus.Code_A || cs == USCISCaseStatus.Code_O)
            {
                if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;
            }

            if (cs == USCISCaseStatus.Code_031)
            {
                if (!_Request.EmpRetrievePhoto()) return false;
            }

            _Request.InsertLink();

            return true;

        }

        public bool EmpDMVDHSReVerify(string caseNumber)
        {
            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpDMVDHSReVerify()) return false;

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Code_008 || cs == USCISCaseStatus.Code_016 || cs == USCISCaseStatus.Code_A || cs == USCISCaseStatus.Code_O)
            {
                if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;
            }

            if (cs == USCISCaseStatus.Code_031)
            {
                if (!_Request.EmpRetrievePhoto()) return false;
            }

            _Request.InsertLink();

            return true;

        }

        
        public bool EmpRetrievePhoto(string caseNumber)
        {
            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpRetrievePhoto()) return false;

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Code_008 || cs == USCISCaseStatus.Code_016 || cs == USCISCaseStatus.Code_A || cs == USCISCaseStatus.Code_O)
            {
                if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;
            }

            _Request.InsertLink();

            return true;

        }


        public bool EmpRetrieveFAN(string caseNumber, string LetterTypeCode)
        {
            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpRetrieveFAN(LetterTypeCode)) return false;

            _Request.InsertLink();

            return true;

        }

        public bool EmpGetCaseDetails(string caseNumber)
        {
            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpGetCaseDetails()) return false;

            _Request.InsertLink();

            return true;

        }

        //11-3-2014 EV 26 Get dup case listing
        public bool EmpGetDuplicateCaseList(string caseNumber)
        {
            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpGetDuplicateCaseList()) return false;

            _Request.InsertLink();

            return true;
        }

        public bool EmpDupCaseContinueWithoutChanges(string caseNumber)
        {
            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpDupCaseContinueWithoutChanges()) return false;

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Code_008 || cs == USCISCaseStatus.Code_016 || cs == USCISCaseStatus.Code_A || cs == USCISCaseStatus.Code_O)
            {
                if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;
            }

            if (cs == USCISCaseStatus.Code_031)
            {
                if (!_Request.EmpRetrievePhoto()) return false;
            }

            _Request.InsertLink();

            return true;
        }

        public bool EmpDupCaseContinueWithChanges(string caseNumber)
        {
            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpDupCaseContinueWithChanges()) return false;

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            if (cs == USCISCaseStatus.Code_008 || cs == USCISCaseStatus.Code_016 || cs == USCISCaseStatus.Code_A || cs == USCISCaseStatus.Code_O)
            {
                if (!_Request.CloseCase(USCISClosureId.EELIG, "Y")) return false;
            }

            if (cs == USCISCaseStatus.Code_031)
            {
                if (!_Request.EmpRetrievePhoto()) return false;
            }

            _Request.InsertLink();

            return true;
        }

        public bool EmpSaveSSATNCNotification(string caseNumber)
        {

            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpSaveSSATNCNotification()) return false;

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            _Request.InsertLink();

            return true;
        }

        public bool EmpSaveDHSTNCNotification(string caseNumber)
        {

            _Request.CaseNumber = caseNumber;

            if (!_Request.EmpSaveDHSTNCNotification()) return false;

            USCISCaseStatus cs = _Request.WebServiceResponse.CaseStatusEnum;

            _Request.InsertLink();

            return true;
        }
        #endregion

        #region Methods - Private

        private USCISClosureId _ConvertToUSCISClosureId(int idClosure) {

            if (idClosure == (int)USCISClosureId.EAUTH) return USCISClosureId.EAUTH;
            else if (idClosure == (int)USCISClosureId.EUAUTH) return USCISClosureId.EUAUTH;
            else if (idClosure == (int)USCISClosureId.IQ) return USCISClosureId.IQ;
            else if (idClosure == (int)USCISClosureId.SELFT) return USCISClosureId.SELFT;
            else if (idClosure == (int)USCISClosureId.SNTERM) return USCISClosureId.SNTERM;
            else if (idClosure == (int)USCISClosureId.EELIG) return USCISClosureId.EELIG;
            else if (idClosure == (int)USCISClosureId.EFNC) return USCISClosureId.EFNC;
            else if (idClosure == (int)USCISClosureId.ENOACT) return USCISClosureId.ENOACT;
            else if (idClosure == (int)USCISClosureId.EUNCNT) return USCISClosureId.EUNCNT;
            else if (idClosure == (int)USCISClosureId.TRMFNC) return USCISClosureId.TRMFNC;
            else if (idClosure == (int)USCISClosureId.EQUIT) return USCISClosureId.EQUIT;
            else if (idClosure == (int)USCISClosureId.TERM) return USCISClosureId.TERM;
            else if (idClosure == (int)USCISClosureId.NOACT) return USCISClosureId.NOACT;
            else if (idClosure == (int)USCISClosureId.UNCNT) return USCISClosureId.UNCNT;
            else if (idClosure == (int)USCISClosureId.DUP) return USCISClosureId.DUP;
            else if (idClosure == (int)USCISClosureId.INCDAT) return USCISClosureId.INCDAT;
            else if (idClosure == (int)USCISClosureId.ISDP) return USCISClosureId.ISDP;
            else if (idClosure == (int)USCISClosureId.TECISS) return USCISClosureId.TECISS;
            else if (idClosure == (int)USCISClosureId.EARCLS) return USCISClosureId.EARCLS;
            else if (idClosure == (int)USCISClosureId.EARNEW) return USCISClosureId.EARNEW;
            else if (idClosure == (int)USCISClosureId.EDEXPD) return USCISClosureId.EDEXPD;
            else if (idClosure == (int)USCISClosureId.ENCLNT) return USCISClosureId.ENCLNT;
            else if (idClosure == (int)USCISClosureId.ENEMPD) return USCISClosureId.ENEMPD;
            else return USCISClosureId.None;
        }

        #endregion

    }

}