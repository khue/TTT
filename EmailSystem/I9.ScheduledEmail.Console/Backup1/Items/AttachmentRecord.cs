using System;
using System.Collections.Generic;
using System.Text;

namespace I9.ScheduledEmail.Console.Items {

    internal class AttachmentRecord {

        #region Members

        private int _EmailTemplateAttachmentId = -1;

        private string _AttachmentPath = null;
        private string _AttachmentName = null;

        #endregion

        #region Constructors

        internal AttachmentRecord(Sql.spSE_EmailAttachments_Get spEmailAttachmentsGet) {

            if (spEmailAttachmentsGet == null) throw new ArgumentNullException("spSE_EmailAttachments_Get is null");

            _EmailTemplateAttachmentId = Convert.ToInt32(spEmailAttachmentsGet.GetDataReaderValue(0, -1));
            _AttachmentPath = Convert.ToString(spEmailAttachmentsGet.GetDataReaderValue(1, null));
            _AttachmentName = Convert.ToString(spEmailAttachmentsGet.GetDataReaderValue(2, null));

        }

        #endregion

        #region Proeprties - Public

        public int EmailTemplateAttachmentId { get { return _EmailTemplateAttachmentId; } }

        public string AttachmentName { get { return _AttachmentName; } }
        public string AttachmentPath { get { return _AttachmentPath; } }

        #endregion

    }

}
