using System;
using System.Collections.Generic;
using System.Text;

namespace I9.ScheduledEmail.Console.Items {

    internal class AddressRecord {

        #region Members

        private string _FieldType = null;
		private string _ExplicitValue = null;
		private string _FieldName = null;

        #endregion

        #region Constructors

        internal AddressRecord(Sql.spSE_EmailAddresses_Get spEmailAddressesGet) {

            if (spEmailAddressesGet == null) throw new ArgumentNullException("spSE_EmailAddresses_Get is null");

            _FieldType = Convert.ToString(spEmailAddressesGet.GetDataReaderValue(0, null));
            _ExplicitValue = Convert.ToString(spEmailAddressesGet.GetDataReaderValue(1, null));
            _FieldName = Convert.ToString(spEmailAddressesGet.GetDataReaderValue(2, null));

        }

        #endregion

        #region Proeprties - Public

        public string ExplicitValue { get { return _ExplicitValue; } }
        public string FieldName { get { return _FieldName; } }
        public string FieldType { get { return _FieldType; } }

        #endregion

    }

}
