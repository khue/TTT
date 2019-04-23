using System;

namespace Fdbl.Toolkit {

    public class FdblClassMemberNullException : Exception {

        #region Constructors

        public FdblClassMemberNullException() : base() { }

        public FdblClassMemberNullException(string message) : base(message) { }

        public FdblClassMemberNullException(string message, Exception innerException) : base(message, innerException) { }

        #endregion

    }

}
