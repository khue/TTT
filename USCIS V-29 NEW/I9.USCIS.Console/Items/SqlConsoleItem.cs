using System;

using I9.USCIS.Wrapper;

namespace I9.USCIS.Console.Items {

    internal class SqlConsoleItem {

        #region Members

        private USCISSystemId _USCISSystemId;
        private USCISActionId _USCISActionId;

        private int _ConsoleId;

        private int _SystemId;
        private int _ActionId;
        private int _ClosureId;

        private int _EmployeeId;
        private int _I9Id;

        private string _CaseNumber;
        private string _Comments;
        private string _CurrentlyEmployed;
        private string _LetterTypeCode;

        #endregion

        #region Constructors

        public SqlConsoleItem(Sql.spUSCISBE_Console_Begin sp) {

            if (sp == null) throw new MyException("spUSCIS_Console_Begin is null");

            _SystemId = Convert.ToInt32(sp.GetDataReaderValue(0, MyConsole.NullId));
            _ActionId = Convert.ToInt32(sp.GetDataReaderValue(1, MyConsole.NullId));
            _ClosureId = Convert.ToInt32(sp.GetDataReaderValue(2, MyConsole.NullId));
            _EmployeeId = Convert.ToInt32(sp.GetDataReaderValue(3, MyConsole.NullId));
            _I9Id = Convert.ToInt32(sp.GetDataReaderValue(4, MyConsole.NullId));
            _CaseNumber = Convert.ToString(sp.GetDataReaderValue(5, string.Empty));
            _Comments = Convert.ToString(sp.GetDataReaderValue(6, string.Empty));
            _CurrentlyEmployed = Convert.ToString(sp.GetDataReaderValue(7, string.Empty));
            _LetterTypeCode = Convert.ToString(sp.GetDataReaderValue(8, string.Empty));

            if (_SystemId == (int)USCISSystemId.Live) _USCISSystemId = USCISSystemId.Live;
            else if (_SystemId == (int)USCISSystemId.Test) _USCISSystemId = USCISSystemId.Test;
            else _USCISSystemId = USCISSystemId.Unknown;

            if (_ActionId == (int)USCISActionId.AutoProcessInitiateCase) _USCISActionId = USCISActionId.AutoProcessInitiateCase;
            else if (_ActionId == (int)USCISActionId.AutoProcessSSAVerification) _USCISActionId = USCISActionId.AutoProcessSSAVerification;
            else if (_ActionId == (int)USCISActionId.InitiateCase) _USCISActionId = USCISActionId.InitiateCase;
            else if (_ActionId == (int)USCISActionId.SubmitSSAReferral) _USCISActionId = USCISActionId.SubmitSSAReferral;
            else if (_ActionId == (int)USCISActionId.ResubmitSSAVerification) _USCISActionId = USCISActionId.ResubmitSSAVerification;
            else if (_ActionId == (int)USCISActionId.SubmitSecondVerification) _USCISActionId = USCISActionId.SubmitSecondVerification;
            else if (_ActionId == (int)USCISActionId.SubmitDHSReferral) _USCISActionId = USCISActionId.SubmitDHSReferral;
            else if (_ActionId == (int)USCISActionId.CloseCase) _USCISActionId = USCISActionId.CloseCase;
            else if (_ActionId == (int)USCISActionId.EmpConfirmPhoto) _USCISActionId = USCISActionId.EmpConfirmPhoto;
            else if (_ActionId == (int)USCISActionId.EmpConfirmPhotoNoClose) _USCISActionId = USCISActionId.EmpConfirmPhotoNoClose;
            else if (_ActionId == (int)USCISActionId.EmpUpdateSSALetterReceived) _USCISActionId = USCISActionId.EmpUpdateSSALetterReceived;
            else if (_ActionId == (int)USCISActionId.EmpUpdateDHSLetterReceived) _USCISActionId = USCISActionId.EmpUpdateDHSLetterReceived;
            else if (_ActionId == (int)USCISActionId.EmpSSAReVerify) _USCISActionId = USCISActionId.EmpSSAReVerify;
            else if (_ActionId == (int)USCISActionId.EmpDHSReVerify) _USCISActionId = USCISActionId.EmpDHSReVerify;
            else if (_ActionId == (int)USCISActionId.EmpCitDHSReVerify) _USCISActionId = USCISActionId.EmpCitDHSReVerify;
            else if (_ActionId == (int)USCISActionId.EmpRetrievePhoto) _USCISActionId = USCISActionId.EmpRetrievePhoto;
            else if (_ActionId == (int)USCISActionId.EmpDMVDHSReVerify) _USCISActionId = USCISActionId.EmpDMVDHSReVerify;
            else if (_ActionId == (int)USCISActionId.EmpRetrieveFAN) _USCISActionId = USCISActionId.EmpRetrieveFAN;
            else if (_ActionId == (int)USCISActionId.EmpGetCaseDetails) _USCISActionId = USCISActionId.EmpGetCaseDetails;
            else if (_ActionId == (int)USCISActionId.EmpGetDuplicateCaseList) _USCISActionId = USCISActionId.EmpGetDuplicateCaseList;
            else if (_ActionId == (int)USCISActionId.EmpDupCaseContinueWithoutChanges) _USCISActionId = USCISActionId.EmpDupCaseContinueWithoutChanges;
            else if (_ActionId == (int)USCISActionId.EmpDupCaseContinueWithChanges) _USCISActionId = USCISActionId.EmpDupCaseContinueWithChanges;
            else if (_ActionId == (int)USCISActionId.EmpSaveSSATNCNotification) _USCISActionId = USCISActionId.EmpSaveSSATNCNotification;
            else if (_ActionId == (int)USCISActionId.EmpSaveDHSTNCNotification) _USCISActionId = USCISActionId.EmpSaveDHSTNCNotification;

        }

        #endregion

        #region Properties - Public

        public int ConsoleId { 
            get { return _ConsoleId; } 
            set { _ConsoleId = value; } 
        }

        public USCISSystemId SystemIdEnum { get { return _USCISSystemId; } }
        public USCISActionId ActionIdEnum { get { return _USCISActionId; } }

        public int SystemId { get { return _SystemId; } }
        public int ActionId { get { return _ActionId; } }
        public int ClosureId { get { return _ClosureId; } }
        
        public int EmployeeId { get { return _EmployeeId; } }
        public int I9Id { get { return _I9Id; } }

        public string CaseNumber { get { return _CaseNumber; } }
        public string Comments { get { return _Comments; } }
        public string CurrentlyEmployed { get { return _CurrentlyEmployed; } }
        public string LetterTypeCode { get { return _LetterTypeCode; } }

        #endregion

    }

}