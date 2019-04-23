using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace Fdbl.Toolkit.Smtp {

    public class FdblSmtp {

        #region Methods - Public (Static)

        public static void Send(string smtpServer, string sendFrom, string sendTo, string subject, string mesg) {
            Send(new FdblSmtpRecord(smtpServer, sendFrom, sendTo, subject, mesg));
        }

        public static void Send(string smtpServer, string sendFrom, string sendTo, string subject, string mesg, int maxTries) {
            Send(new FdblSmtpRecord(smtpServer, sendFrom, sendTo, subject, mesg, maxTries));
        }

        public static void Send(string smtpServer, string sendFrom, string sendTo, string subject, string mesg, string fileAttachment) {
            FdblSmtpRecord rec = new FdblSmtpRecord(smtpServer, sendFrom, sendTo, subject, mesg);
            rec.AddFileAttachment(fileAttachment);
            Send(rec);
        }

        public static void Send(string smtpServer, string sendFrom, string sendTo, string subject, string mesg, string fileAttachment, int maxTries) {
            FdblSmtpRecord rec = new FdblSmtpRecord(smtpServer, sendFrom, sendTo, subject, mesg, maxTries);
            rec.AddFileAttachment(fileAttachment);
            Send(rec);
        }

        public static void Send(string smtpServer, string sendFrom, string sendTo, string subject, string mesg, string[] fileAttachments) {
            FdblSmtpRecord rec = new FdblSmtpRecord(smtpServer, sendFrom, sendTo, subject, mesg);
            rec.AddFileAttachments(fileAttachments);
            Send(rec);
        }

        public static void Send(string smtpServer, string sendFrom, string sendTo, string subject, string mesg, string[] fileAttachments, int maxTries) {
            FdblSmtpRecord rec = new FdblSmtpRecord(smtpServer, sendFrom, sendTo, subject, mesg, maxTries);
            rec.AddFileAttachments(fileAttachments);
            Send(rec);
        }

        public static void Send(FdblSmtpRecord smtpRecord) {

            if (smtpRecord == null) throw new ArgumentNullException("smtp record is null");

            if (smtpRecord.IsEmpty) throw new ArgumentException("smtp record is blank");

            SmtpClient smtp = new SmtpClient(smtpRecord.SmtpServer);
            MailMessage mail = new MailMessage();

            try {

                mail.Sender = new MailAddress(smtpRecord.SendFrom);
                mail.From = new MailAddress(smtpRecord.SendFrom);

                _LoadSendToAddress(mail, smtpRecord);
                _LoadCopyToAddress(mail, smtpRecord);
                _LoadBlindCopyToAddress(mail, smtpRecord);

                mail.Subject = smtpRecord.Subject;

                if (smtpRecord.HaveMessage) mail.Body = smtpRecord.Message;

                if (smtpRecord.HaveMessageHtml) {

                    AlternateView av = AlternateView.CreateAlternateViewFromString(smtpRecord.MessageHtml, null, MediaTypeNames.Text.Html);
                    mail.AlternateViews.Add(av);

                }

                if (smtpRecord.FileAttachments.Count > 0) mail.Body += string.Format("{0}{0}{0}", Utils.FdblFormats.CrLf);

                for (int ndx = 0; ndx < smtpRecord.FileAttachments.Count; ndx++) {

                    FdblSmtpAttachment fsa = smtpRecord.FileAttachments[ndx];

                    if (File.Exists(fsa.FileLocation)) {

                        Attachment attach = new Attachment(fsa.FileLocation);

                        if (!string.IsNullOrEmpty(fsa.FileName)) attach.Name = fsa.FileName;

                        mail.Attachments.Add(attach);

                    }

                }

                Exception iex = null;

                for (int ndx = 0; ndx < smtpRecord.MaxTries; ndx++) {

                    try {
                        smtp.Send(mail);
                        break;
                    } catch (Exception ex) {
                        iex = new Exception(ex.Message, iex);
                        if (ndx == smtpRecord.MaxTries) throw iex;
                    }

                }

            } finally {

                mail.Dispose();

            }

        }

        #endregion

        #region Contructors

        private FdblSmtp() { }

        #endregion

        #region Methods - Private

        private static void _LoadBlindCopyToAddress(MailMessage mail, FdblSmtpRecord smtpRecord) {

            if (!smtpRecord.HaveBlindCopyTo) return;

            string[] names = smtpRecord.BlindCopyTo.Split(";,".ToCharArray());

            for (int i = 0; i < names.Length; i++) {

                mail.Bcc.Add(names[i]);

            }

        }

        private static void _LoadCopyToAddress(MailMessage mail, FdblSmtpRecord smtpRecord) {

            if (!smtpRecord.HaveCopyTo) return;

            string[] names = smtpRecord.CopyTo.Split(";,".ToCharArray());

            for (int i = 0; i < names.Length; i++) {

                mail.CC.Add(names[i]);

            }

        }

        private static void _LoadSendToAddress(MailMessage mail, FdblSmtpRecord smtpRecord) {

            if (!smtpRecord.HaveSendTo) return;

            string[] names = smtpRecord.SendTo.Split(";,".ToCharArray());

            for (int i = 0; i < names.Length; i++) {

                mail.To.Add(names[i]);

            }

        }

        #endregion
    }

}