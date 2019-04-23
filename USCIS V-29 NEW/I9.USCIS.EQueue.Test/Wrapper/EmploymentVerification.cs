using System;

using I9.USCIS.Wrapper;
using I9.USCIS.Wrapper.WebService;

namespace I9.USCIS.EQueue.Test.Wrapper {

    internal class EmploymentVerification {

        #region Members

        private IRequest _Request;

        #endregion

        #region Constructors

        internal EmploymentVerification(USCISSystemId idSystem) {

            if (idSystem == USCISSystemId.Live) _Request = USCISFactory.GetLiveRequest();
            else if (idSystem == USCISSystemId.Test) _Request = USCISFactory.GetTestRequest();
            else throw new ArgumentException("The given uscis system id is not defined");

        }

        #endregion

        #region Methods - Public

        public void Dispose() {
      
            if (_Request != null) _Request.Dispose();

            System.GC.SuppressFinalize(this);

        }

        public void SetEmployee(int idQueueError, int idTransaction, int idEmployee, int idI9) {

            if (idQueueError != MyConsole.NullId) _Request.QueueErrorId = idQueueError;
            if (idTransaction != MyConsole.NullId) _Request.TransactionId = idTransaction;
            _Request.EmployeeId = idEmployee;
            _Request.I9Id = idI9;

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
            if (cs == USCISCaseStatus.Code_U) return true;
            if (cs == USCISCaseStatus.Code_C) return true;
            if (cs == USCISCaseStatus.Code_I) return true;
            if (cs == USCISCaseStatus.Code_N) return true;
            if (cs == USCISCaseStatus.Code_S) return true;

            _Request.CaseNumber = _Request.WebServiceResponse.CaseNbr;

            if (!_Request.CloseCase(USCISClosureId.EAUTH,"Y")) return false;

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
            if (cs == USCISCaseStatus.Code_U) return true;
            if (cs == USCISCaseStatus.Code_C) return true;
            if (cs == USCISCaseStatus.Code_I) return true;
            if (cs == USCISCaseStatus.Code_N) return true;
            if (cs == USCISCaseStatus.Code_S) return true;

            if (!_Request.CloseCase(USCISClosureId.EAUTH,"Y")) return false;

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

        public bool ResubmitSSAVerification(string caseNumber) {

            _Request.CaseNumber = caseNumber;

            if (!_Request.ResubmitSSAVerification()) return false;

            _Request.InsertLink();

            return true;

        }

        public bool SubmitDHSReferral(string caseNumber) {

            _Request.CaseNumber = caseNumber;

            if (!_Request.SubmitDHSReferral()) return false;

            _Request.InsertLink();

            return true;

        }

        public bool SubmitSecondVerification(string caseNumber, string comments) {

            _Request.CaseNumber = caseNumber;

            if (!_Request.SubmitSecondVerification(comments)) return false;

            _Request.InsertLink();

            return true;

        }

        public bool CloseCase(int idClosure, string caseNumber) {

            _Request.CaseNumber = caseNumber;

            if (!_Request.CloseCase(_ConvertToUSCISClosureId(idClosure),"Y")) return false;

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
            else return USCISClosureId.None;

        }

        #endregion

    }

}