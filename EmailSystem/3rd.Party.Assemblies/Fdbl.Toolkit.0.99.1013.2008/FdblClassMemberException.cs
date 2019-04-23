using System;

namespace Fdbl.Toolkit {

    public class FdblClassMemberException : Exception {

        #region Constructors

        public FdblClassMemberException() : base() { }

        public FdblClassMemberException(string message) : base(message) { }

        public FdblClassMemberException(string message, Exception innerException) : base(message, innerException) { }

        #endregion

    }

}
