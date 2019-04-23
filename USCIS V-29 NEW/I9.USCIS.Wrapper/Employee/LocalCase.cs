using System;
using System.IO;
using System.Drawing;
using Fdbl.Toolkit;
using Fdbl.Toolkit.Utils;
using System.Text;



namespace I9.USCIS.Wrapper.Employee {

    public class LocalCase {

        #region Members

        private int _USCISCompanyId = -1;

        private string _LastName = null;
        private string _FirstName = null;
        private string _MiddleInitial = null;
        private string _MaidenName = null;
        private string _SSN = null;
        private string _AlienNumber = null;
        private string _I94Number = null;
        private string _CardNumber = null;
        private string _PassportNumber = null;
        private string _VisaNumber = null;
        private string _UserField = null;
        private string _EmailAddress = null;

        private string _DateOfDocumentExpiration;
        private string _DateOfBirth;
        private string _DateOfHire;
        
    
        private int _CitizenshipStatus = 0;
        private int _DocumentType = 0;

        private bool _UsesFutureHires = false;

        //kevin hue 8/31/2010 - new fields for version 21
        private string _OverDueVerifyReason = null;
        private string _OverDueVerifyReasonOther = null;
        private string _UploadDoc = null;
        private byte[] _DocGIF = null;
        private string _PhotoConfirmation = null;
        private string _CurrentlyEmployed = null;

        //kevin hue 9/28/2011 - new fields for versino 23
        private int _DocumentBType = 0;
        private int _DocumentCType = 0;
        private int _SupportingDocumentID = 0;
        private string _StateIssuingAuthority = null;
        private string _ListBDocumentNumber = null;
        private string _ListBExpirationFlag = null;

        //new fields for ev version 24 3-5-2013 KH
        private string _NoForeignPassport = null;
        private string _CountryOfIssuance = null;

       //10-31-2014 new fields for EV version 26
        private string _DupReason = null;
        private string _DupReasonOther = null;

        #endregion

        #region Constructors

        internal LocalCase(Sql.spUSCIS_EmployeeI9_GetDetails sp) {

            if (sp == null) throw new ArgumentNullException("spUSCIS_EmployeeI9_GetDetails is null");

            int tmpCitizenshipStatus = 0;
            int tmpListADocumentId = 0;

            string tmpAlienI94Num = null;
            string tmpListADocument1Num = null;
            string tmpListAExpiration1Date = null;
            string tmpListADocument2Num = null;
            string tmpListAExpiration2Date = null;
            string tmpListBExpirationDate = null;
            string tmpDocumentAType = null;
            string tmpDocumentBType = null;
            string tmpUSCISIdCompany = null;
            string tmpUSCISIdDivision = null;
            string tmpDMVListBExpirationDate = null;
            string tmpFPPNumber = null;
            
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            _LastName = Convert.ToString(sp.GetDataReaderValue(0, string.Empty));
            _FirstName = Convert.ToString(sp.GetDataReaderValue(1, string.Empty));
            _MiddleInitial = Convert.ToString(sp.GetDataReaderValue(2, string.Empty));
            _MaidenName = Convert.ToString(sp.GetDataReaderValue(3, string.Empty));
            _DateOfBirth = Convert.ToDateTime(sp.GetDataReaderValue(4, "01/01/0001")).ToString("yyyy-MM-dd");
            _SSN = Convert.ToString(sp.GetDataReaderValue(5, string.Empty));
            tmpCitizenshipStatus = Convert.ToInt32(sp.GetDataReaderValue(6, -1));
            _CitizenshipStatus = Convert.ToInt32(sp.GetDataReaderValue(7, -1));
            tmpAlienI94Num = Convert.ToString(sp.GetDataReaderValue(8, string.Empty));
            tmpListBExpirationDate = Convert.ToDateTime(sp.GetDataReaderValue(9, "01/01/0001")).ToString("yyyy/MM/dd");
            _DateOfHire = Convert.ToString(sp.GetDataReaderValue(10, "01/01/0001"));
            tmpListADocumentId = Convert.ToInt32(sp.GetDataReaderValue(11, -1));
            tmpListADocument1Num = Convert.ToString(sp.GetDataReaderValue(12, string.Empty));
            tmpListAExpiration1Date = Convert.ToDateTime(sp.GetDataReaderValue(13, "01/01/0001")).ToString("yyyy/MM/dd");
            tmpListADocument2Num = Convert.ToString(sp.GetDataReaderValue(14, string.Empty));
            tmpListAExpiration2Date = Convert.ToDateTime(sp.GetDataReaderValue(15, "01/01/0001")).ToString("yyyy/MM/dd");
            tmpDocumentAType = Convert.ToString(sp.GetDataReaderValue(16, string.Empty));
            tmpDocumentBType = Convert.ToString(sp.GetDataReaderValue(17, string.Empty));
            _UserField = Convert.ToString(sp.GetDataReaderValue(18, string.Empty));
            tmpUSCISIdCompany = Convert.ToString(sp.GetDataReaderValue(19, string.Empty));
            tmpUSCISIdDivision = Convert.ToString(sp.GetDataReaderValue(20, string.Empty));
            _UsesFutureHires = Convert.ToBoolean(sp.GetDataReaderValue(21, false));
            _OverDueVerifyReason = Convert.ToString(sp.GetDataReaderValue(22, string.Empty));
            _OverDueVerifyReasonOther = Convert.ToString(sp.GetDataReaderValue(23, string.Empty));
            _UploadDoc = Convert.ToString(sp.GetDataReaderValue(24, "N"));
            if (Convert.ToString(sp.GetDataReaderValue(25, "")).Length > 0) { _DocGIF = (byte[])sp.GetDataReaderValue(25, ""); }
            _PhotoConfirmation = (Convert.ToString(sp.GetDataReaderValue(26, "")));
            _CurrentlyEmployed = (Convert.ToString(sp.GetDataReaderValue(27, "")));
            _DocumentCType = Convert.ToInt32(sp.GetDataReaderValue(28, -1));
            _SupportingDocumentID = Convert.ToInt32(sp.GetDataReaderValue(29, -1));
            _StateIssuingAuthority = (Convert.ToString(sp.GetDataReaderValue(30, string.Empty)));
            _ListBDocumentNumber = (Convert.ToString(sp.GetDataReaderValue(31, string.Empty)));
            tmpDMVListBExpirationDate = Convert.ToDateTime(sp.GetDataReaderValue(32, "01/01/0001")).ToString("yyyy/MM/dd");
            _ListBExpirationFlag = (Convert.ToString(sp.GetDataReaderValue(33, string.Empty)));
            _NoForeignPassport = (Convert.ToString(sp.GetDataReaderValue(34, string.Empty)));
            _CountryOfIssuance = (Convert.ToString(sp.GetDataReaderValue(35, string.Empty)));
            _EmailAddress = (Convert.ToString(sp.GetDataReaderValue(36, string.Empty)));
            _DupReason = (Convert.ToString(sp.GetDataReaderValue(37, string.Empty)));
            _DupReasonOther = (Convert.ToString(sp.GetDataReaderValue(38, string.Empty)));
            tmpFPPNumber = (Convert.ToString(sp.GetDataReaderValue(39, string.Empty)));

            if (!string.IsNullOrEmpty(tmpDocumentBType))
            {
            _DocumentBType = Convert.ToInt32(tmpDocumentBType);  
            }

 
            //clear out variable if listbdoc type is not == 1 (ID)
            if (_DocumentBType != 1)
            {
                _StateIssuingAuthority = string.Empty;
            }


            if (!int.TryParse(tmpUSCISIdDivision, out _USCISCompanyId)) int.TryParse(tmpUSCISIdCompany, out _USCISCompanyId);

            if (!string.IsNullOrEmpty(tmpDocumentAType)) {

                try { _DocumentType = int.Parse(tmpDocumentAType); } catch { _DocumentType = 0; }

            } else if (!string.IsNullOrEmpty(tmpDocumentBType)) {

                try { _DocumentType = 28; }
                catch { _DocumentType = 0; }

            } else _DocumentType = 0;

          

            //if (_CitizenshipStatus == 3 && _DocumentType == 24) _VisaNumber = "12345678";
            if (!string.IsNullOrEmpty(tmpListADocument1Num)) _PassportNumber = tmpListADocument1Num;


            //add by kevin - do not send passport number if lpr checkbox
            if (tmpCitizenshipStatus == 2 && Convert.ToInt32(_DocumentType) != 25) _PassportNumber = string.Empty;

            //this line is for production. The documentid are productionids. uncomment this when deploy into production. Recommend and use uat documentids 
            //if (tmpCitizenshipStatus == 3 && tmpListADocumentId != 104 && tmpListADocumentId != 98 && tmpListADocumentId != 103 && tmpListADocumentId != 137 && tmpListADocumentId != 62) _PassportNumber = string.Empty;

            //test this code in UAT first. If AA citizenship, then passport should always be blank.
            if (tmpCitizenshipStatus == 3) _PassportNumber = string.Empty;


            //version 21 change. if citizenshipstatus =6 then cardnumber cannot be null
            if ((tmpCitizenshipStatus == 2 && _DocumentType == 13) || (tmpCitizenshipStatus == 3 && _DocumentType == 17))
            {
                _CardNumber = tmpListADocument1Num;
                if (Convert.ToInt32(tmpDocumentAType) == 13) { tmpListAExpiration1Date = "01/01/0001"; }
                
            }
            
            // Used for data scrubbing

            string[] alpha = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string[] replaceWith = { string.Empty };
            string[] lookFor;
            string nonCharacters = string.Empty;
            int ndx;

            ndx = 0;
            nonCharacters = FdblStrings.Trim(FdblStrings.ReplaceSubstring(_MaidenName, alpha, replaceWith, false));
            lookFor = new string[nonCharacters.Length + 1];

            for (ndx = 0; ndx < nonCharacters.Length; ndx++)
            {

                lookFor[ndx] = nonCharacters.Substring(ndx, 1);

            }

            lookFor[ndx] = " ";

            _MaidenName = FdblStrings.ReplaceSubstring(_MaidenName, lookFor, replaceWith);

            // Fix document types based on USCIS standards

            if ((tmpListADocumentId == -1 || _DocumentType == 28) && string.IsNullOrEmpty(tmpDocumentBType))
            {

                if (tmpCitizenshipStatus == 3) {

                    if (tmpAlienI94Num.Length < 10) {

                        _AlienNumber = tmpAlienI94Num;
                        _I94Number = string.Empty;

                    } else {

                        _AlienNumber = string.Empty;
                        _I94Number = tmpAlienI94Num;

                    }

                    _DateOfDocumentExpiration = "01/01/0001";
                    
                } else if (tmpCitizenshipStatus == 2) {

                    _AlienNumber = tmpAlienI94Num;
                    _I94Number = string.Empty;
                    _DateOfDocumentExpiration = "01/01/0001";

                } else {

                    _AlienNumber = string.Empty;
                    _I94Number = string.Empty;
                    _DateOfDocumentExpiration = "01/01/0001";

                }

            }
            else if (Convert.ToInt32(_DocumentType) == 24)
            {

                _AlienNumber = string.Empty;
                _I94Number = tmpListADocument2Num;
                //_PassportNumber = tmpListADocument2Num;
                _PassportNumber = tmpListADocument1Num;
                _DateOfDocumentExpiration = tmpListAExpiration2Date;

            }
            else if (Convert.ToInt32(_DocumentType) == 25)
            {

                _AlienNumber = tmpListADocument2Num;
                _I94Number = string.Empty;
                _DateOfDocumentExpiration = tmpListAExpiration2Date;

            }
            //new statements for version 23 kevin hue 10/4/2011
            else if (!string.IsNullOrEmpty(tmpDocumentBType))
            {
             
                //_DateOfDocumentExpiration = "01/01/0001";
                _DateOfDocumentExpiration = tmpDMVListBExpirationDate;
                _AlienNumber = tmpAlienI94Num;
                _I94Number = string.Empty;
                _PassportNumber = tmpFPPNumber;

              if (tmpCitizenshipStatus == 3) {

                    if (tmpAlienI94Num.Length < 10) {

                        _AlienNumber = tmpAlienI94Num;
                        _I94Number = string.Empty;

                    } else {

                        _AlienNumber = string.Empty;
                        _I94Number = tmpAlienI94Num;

                    }

                    
                    
                } else if (tmpCitizenshipStatus == 2) {

                    _AlienNumber = tmpAlienI94Num;
                    _I94Number = string.Empty;
                    

                } else {

                    _AlienNumber = string.Empty;
                    _I94Number = string.Empty;
                    

                }

            }
               
        
            else {

                //_AlienNumber = tmpAlienI94Num;
                //_I94Number = string.Empty;
                _DateOfDocumentExpiration = tmpListAExpiration1Date;

                if (tmpAlienI94Num.Length < 10)
                {

                    _AlienNumber = tmpAlienI94Num;
                    _I94Number = string.Empty;

                }
                else
                {

                    _AlienNumber = string.Empty;
                    _I94Number = tmpAlienI94Num;

                }
                
            }

            string[] nums = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            // Fix the AlienNumber based on USCIS standards
            if (_AlienNumber.Trim().Length > 0) {

                string nonNumbers = FdblStrings.Trim(FdblStrings.ReplaceSubstring(_AlienNumber, nums, replaceWith, false));

                lookFor = new string[nonNumbers.Length + 1];

                for (ndx = 0; ndx < nonNumbers.Length; ndx++) {

                    lookFor[ndx] = nonNumbers.Substring(ndx, 1);

                }

                lookFor[ndx] = " ";

                _AlienNumber = FdblStrings.ReplaceSubstring(_AlienNumber, lookFor, replaceWith).PadLeft(9, '0');

                if (_AlienNumber.Equals("000000000")) _AlienNumber = string.Empty;

            }

            // Fix the I94Number based on USCIS standards
            if (_I94Number.Trim().Length > 0) {

                string nonNumbers = FdblStrings.Trim(FdblStrings.ReplaceSubstring(_I94Number, nums, replaceWith, false));

                lookFor = new string[nonNumbers.Length + 1];

                for (ndx = 0; ndx < nonNumbers.Length; ndx++) {

                    lookFor[ndx] = nonNumbers.Substring(ndx, 1);

                }

                lookFor[ndx] = " ";

                _I94Number = FdblStrings.ReplaceSubstring(_I94Number, lookFor, replaceWith).PadLeft(11, '0');

                if (_I94Number.Equals("00000000000")) _I94Number = string.Empty;

            }

            // Fix the SSN based on USCIS standards
            if (_SSN.Trim().Length > 0) {

                string nonNumbers = FdblStrings.Trim(FdblStrings.ReplaceSubstring(_SSN, nums, replaceWith, false));

                lookFor = new string[nonNumbers.Length + 1];

                for (ndx = 0; ndx < nonNumbers.Length; ndx++) {

                    lookFor[ndx] = nonNumbers.Substring(ndx, 1);

                }

                lookFor[ndx] = " ";

                _SSN = FdblStrings.ReplaceSubstring(_SSN, lookFor, replaceWith).PadLeft(9, '0');

                if (_SSN.Equals("000000000")) _SSN = string.Empty;

            }

            _LastName = FdblStrings.Left(_LastName, 40);
            _FirstName = FdblStrings.Left(_FirstName, 25);
            _MiddleInitial = FdblStrings.Left(_MiddleInitial, 1);
            _MaidenName = FdblStrings.Left(_MaidenName, 40);
            _UserField = FdblStrings.Left(_UserField, 40);

        }

        #endregion

     
        #region Properties - Public

        public string AlienNumber { get { return _AlienNumber; } }
        public string CardNumber { get { return _CardNumber; } }
        public string FirstName { get { return _FirstName; } }
        public string I94Number { get { return _I94Number; } }
        public string LastName { get { return _LastName; } }
        public string MaidenName { get { return _MaidenName; } }
        public string MiddleInitial { get { return _MiddleInitial; } }
        public string PassportNumber { get { return _PassportNumber; } }
        public string SSN { get { return _SSN; } }
        public string UserField { get { return _UserField; } }
        public string VisaNumber { get { return _VisaNumber; } }
        public string EmailAddress { get { return _EmailAddress; } }
        public string DupReason { get { return _DupReason; } }
        public string DupReasonOther { get { return _DupReasonOther; } }

        public int CitizenshipStatus { get { return _CitizenshipStatus; } }
        public int DocumentType { get { return _DocumentType; } }
        public int USCISCompanyId { get { return _USCISCompanyId; } }

        public DateTime DateOfDocumentExpiration { get { return Convert.ToDateTime(_DateOfDocumentExpiration); } }
        public DateTime DateOfBirth { get { return Convert.ToDateTime(_DateOfBirth); } }
        public DateTime DateOfHire { get { return Convert.ToDateTime(_DateOfHire); } }

        public bool UsesFutureHires { get { return _UsesFutureHires; } }

        public string OverDueVerifyReason { get { return _OverDueVerifyReason; } }
        public string OverDueVerifyReasonOther { get { return _OverDueVerifyReasonOther; } }
        public string UploadDoc { get { return _UploadDoc; } }
        public byte[] DocGIF { get { return _DocGIF; } }
        public string PhotoConfirmation { get { return _PhotoConfirmation; } }
        public string CurrentlyEmployed { get { return _CurrentlyEmployed; } }
        public int ListBDocumentType { get { return _DocumentBType; } }
        public int ListCDocumentType { get { return _DocumentCType; } }
        public int SupportingDocumentID { get { return _SupportingDocumentID; } }
        public string StateIssuingAuthority { get { return _StateIssuingAuthority; } }
        public string ListBDocumentNumber { get { return _ListBDocumentNumber; } }
        public string ListBExpirationFlag { get { return _ListBExpirationFlag; } }
        public string NoForeignPassport { get { return _NoForeignPassport; } }
        public string CountryOfIssuance { get { return _CountryOfIssuance; } }

        #endregion

        #region Properties - Public (Virtual)


        #endregion

    }

}