using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace Fdbl.Toolkit.Smtp {

    public class FdblSmtpRecord {

        #region Memebers

        private List<FdblSmtpAttachment> _FileAttachments = new List<FdblSmtpAttachment>();

        private string _SmtpServer = null;
        private string _SendFrom = null;
        private string _SendTo = null;
        private string _CopyTo = null;
        private string _BlindCopyTo = null;
        private string _Subject = null;
        private string _OriginalSubject = null;
        private string _Message = null;
        private string _OriginalMessage = null;
        private string _MessageHtml = null;
        private string _OriginalMessageHtml = null;

        private int _MaxTries = 5;

        #endregion

        #region Properties - Public

        public string BlindCopyTo {
            get { return _BlindCopyTo; }
            set { _BlindCopyTo = (value == null || value.Trim().Length == 0) ? null : value; }
        }

        public string CopyTo {
            get { return _CopyTo; }
            set { _CopyTo = (value == null || value.Trim().Length == 0) ? null : value; }
        }

        public List<FdblSmtpAttachment> FileAttachments {
            get { return _FileAttachments; }
        }

        public bool HaveBlindCopyTo {
            get { return _BlindCopyTo != null && _BlindCopyTo.Trim().Length > 0; }
        }

        public bool HaveCopyTo {
            get { return _CopyTo != null && _CopyTo.Trim().Length > 0; }
        }

        public bool HaveMessage {
            get { return _Message != null && _Message.Trim().Length > 0; }
        }

        public bool HaveMessageHtml {
            get { return _MessageHtml != null && _MessageHtml.Trim().Length > 0; }
        }

        public bool HaveSendTo {
            get { return _SendTo != null && _SendTo.Trim().Length > 0; }
        }

        public bool IsEmpty {
            get { return _SmtpServer.Trim().Length == 0 || (_Subject.Trim().Length == 0 && _Message.Trim().Length == 0); }
        }

        public int MaxTries {
            get { return _MaxTries; }
            set { _MaxTries = value < 1 ? 5 : value; }
        }

        public string Message {
            get { return _Message; }
            set { _Message = (value == null || value.Trim().Length == 0) ? string.Empty : value; }
        }

        public string MessageHtml {
            get { return _MessageHtml; }
            set { _MessageHtml = (value == null || value.Trim().Length == 0) ? string.Empty : value; }
        }

        public string SendFrom {
            get { return _SendFrom; }
            set { _SendFrom = (value == null || value.Trim().Length == 0) ? string.Empty : value; }
        }

        public string SendTo {
            get { return _SendTo; }
            set { _SendTo = (value == null || value.Trim().Length == 0) ? string.Empty : value; }
        }

        public string SmtpServer {
            get { return _SmtpServer; }
            set { _SmtpServer = (value == null || value.Trim().Length == 0) ? string.Empty : value; }
        }

        public string Subject {
            get { return _Subject; }
            set { _Subject = (value == null || value.Trim().Length == 0) ? string.Empty : value; }
        }

        #endregion

        #region Methods - Public

        public void AddFileAttachment(string fileLocation) {

            if (string.IsNullOrEmpty(fileLocation)) return;
            if (!File.Exists(fileLocation)) return;

            _FileAttachments.Add(new FdblSmtpAttachment(fileLocation));

        }

        public void AddFileAttachment(string fileLocation, string fileName) {

            if (string.IsNullOrEmpty(fileLocation)) return;
            if (!File.Exists(fileLocation)) return;

            if (string.IsNullOrEmpty(fileName)) _FileAttachments.Add(new FdblSmtpAttachment(fileLocation));
            else _FileAttachments.Add(new FdblSmtpAttachment(fileLocation, fileName));

        }

        public void AddFileAttachments(string[] fileLocations) {

            if (fileLocations == null || fileLocations.Length == 0) return;

            for (int ndx = 0; ndx < fileLocations.Length; ndx++) {

                if (File.Exists(fileLocations[ndx])) _FileAttachments.Add(new FdblSmtpAttachment(fileLocations[ndx]));

            }

        }

        public void ClearFileAttachments() {

            _FileAttachments.Clear();

        }

        public void ResetMessage() {
            _Message = _OriginalMessage;
            _OriginalMessage = string.Empty;
        }

        public void ResetMessageHtml() {
            _MessageHtml = _OriginalMessageHtml;
            _OriginalMessageHtml = string.Empty;
        }

        public void ResetSubject() {
            _Subject = _OriginalSubject;
            _OriginalSubject = string.Empty;
        }

        public void SetMessage(string message) {
            _OriginalMessage = _Message;
            _Message = (message == null || message.Trim().Length == 0) ? string.Empty : message;
        }

        public void SetMessageHtml(string message) {
            _OriginalMessageHtml = _MessageHtml;
            _MessageHtml = (message == null || message.Trim().Length == 0) ? string.Empty : message;
        }

        public void SetSubject(string subject) {
            _OriginalSubject = _Subject;
            _Subject = (subject == null || subject.Trim().Length == 0) ? string.Empty : subject;
        }

        #endregion

        #region Constructors

        public FdblSmtpRecord() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 5) { }

        public FdblSmtpRecord(string smtpServer, string sendFrom, string sendTo, string subject, string mesg) : this(smtpServer, sendFrom, sendTo, subject, mesg, 5) { }

        public FdblSmtpRecord(string smtpServer, string sendFrom, string sendTo, string subject, string mesg, int maxTries) {

            if (smtpServer == null) throw new ArgumentNullException("smtp server is null");
            if (sendFrom == null) throw new ArgumentNullException("smtp send from is null");
            if (sendTo == null) throw new ArgumentNullException("smtp send to is null");
            if (subject == null) throw new ArgumentNullException("smtp subject is null");
            if (mesg == null) throw new ArgumentNullException("smtp message is null");

            _SmtpServer = smtpServer;
            _SendFrom = sendFrom;
            _SendTo = sendTo;
            _Subject = subject;
            _OriginalSubject = subject;
            _Message = mesg;
            _OriginalMessage = mesg;
            _MaxTries = maxTries < 1 ? 5 : maxTries;

        }

        #endregion

    }

}