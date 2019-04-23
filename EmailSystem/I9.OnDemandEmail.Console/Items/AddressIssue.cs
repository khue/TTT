using System;
using System.Collections.Generic;
using System.Text;

namespace I9.OnDemandEmail.Console.Items {

    internal class AddressIssue {

        #region Members

        private string _FieldName = null;
        private string _Message = null;
        private string _Details = null;

        #endregion

        #region Constructors

        internal AddressIssue() { }

        internal AddressIssue(string fieldName) {

            _FieldName = fieldName;

        }

        internal AddressIssue(string fieldName, string details) {

            _FieldName = fieldName;
            _Details = details;

        }

        #endregion

        #region Properties - Public

        public string FieldName {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        public string Message {
            get { return _Message; }
            set { _Message = value; }
        }

        public string Details {
            get { return _Details; }
            set { _Details = value; }
        }

        #endregion

    }

}
