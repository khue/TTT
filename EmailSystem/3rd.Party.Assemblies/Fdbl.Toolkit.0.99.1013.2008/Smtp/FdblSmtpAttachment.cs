using System;
using System.Collections.Generic;
using System.Text;

namespace Fdbl.Toolkit.Smtp {

    public class FdblSmtpAttachment {

        #region Members

        private string _FileName = null;
        private string _FileType = null;
        private string _FileLocation = null;

        #endregion

        #region Constructors

        internal FdblSmtpAttachment() { }

        internal FdblSmtpAttachment(string fileLocation) : this(fileLocation, null, null) { }

        internal FdblSmtpAttachment(string fileLocation, string fileName) : this(fileLocation, fileName, null) { }

        internal FdblSmtpAttachment(string fileLocation, string fileName, string fileType) {

            _FileLocation = fileLocation;
            _FileName = fileName;
            _FileType = fileType;
        
        }

        #endregion

        #region Properties - Public

        public string FileLocation {
            get { return _FileLocation; }
            set { _FileLocation = value; }
        }

        public string FileName {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public string FileType {
            get { return _FileType; }
            set { _FileType = value; }
        }

        #endregion

    }

}