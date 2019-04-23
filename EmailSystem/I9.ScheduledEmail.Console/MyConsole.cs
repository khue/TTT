using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;
using Fdbl.Toolkit.Xml;

namespace I9.ScheduledEmail.Console {

    internal class MyConsole {

        #region Fields

        internal static readonly int NewEmployee = 5;
        internal static readonly int NewEmployeePassword = 6;

        internal static readonly int IsKeywordIssue = 1;
        internal static readonly int IsAddressIssue = 2;
        internal static readonly int IsSmtpIssue = 3;

        #endregion

        #region Members

        private MyConfiguration _MyConfig = null;

        #endregion

        #region Constructors

        internal MyConsole() {

            FdblConsole.WriteInitialization(System.Reflection.Assembly.GetCallingAssembly());

            _MyConfig = new MyConfiguration();

            FdblConsole.WriteMessage(string.Format("Console started on physical node {0}", Environment.MachineName));

        }

        #endregion

        #region Methods - Public

        public void Run(string timeEmailSchedule) {

            Items.JobDetail jd = new Items.JobDetail();
            Items.JobDetail jdPwd = new Items.JobDetail();

            Sql.spSE_EmailTemplates_Get spEmailTemplatesGet = null;

            try {

                jd.IsActive = true;
                jd.EmailJobId = _JobStart(timeEmailSchedule);

                jdPwd.EmailJobId = jd.EmailJobId;

                spEmailTemplatesGet = new Sql.spSE_EmailTemplates_Get(_MyConfig.SqlFactory.GetConnectionString());

                FdblConsole.WriteMessage(string.Format("Gathering email templates for processing time: {0}", timeEmailSchedule));

                int spReturnCode = spEmailTemplatesGet.StartDataReader(timeEmailSchedule);

                if (spReturnCode == FdblSqlReturnCodes.NoData || spReturnCode == FdblSqlReturnCodes.NoMatch) return;

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_EmailTemplates_Get returned: {0}", spReturnCode));

                while (spEmailTemplatesGet.MoveNextDataReader(true)) {

                    try {

                        jd.DTStart = DateTime.Now;
                        jdPwd.DTStart = DateTime.Now;

                        Items.EmailTemplate et = new Items.EmailTemplate(spEmailTemplatesGet);

                        jdPwd.EmailTemplateId = et.EmailTemplateId;

                        FdblConsole.WriteMessage(string.Format("Processing template : {0}", et.TemplateName));

                        jd.EmailTemplateId = et.EmailTemplateId;

                        if (!et.IsNewEmployeePassword) {

                            FdblConsole.WriteMessage("   Gathering address information", true, false);

                            List<Items.AddressRecord> addressList = _GetAddressList(et);

                            FdblConsole.WriteMessage(string.Format(" ({0} found)", addressList.Count), false, true);

                            FdblConsole.WriteMessage("   Gathering attachments", true, false);

                            List<Items.AttachmentRecord> attachList = _GetAttachmentList(et);

                            jd.Attachments = attachList.Count;

                            FdblConsole.WriteMessage(string.Format(" ({0} found)", attachList.Count), false, true);

                            Items.EmailTemplate etp = null;

                            if (et.IsNewEmployee) {

                                FdblConsole.WriteMessage("   Gathering password information");

                                etp = _GetPasswordEmailTemplate(et);

                            }

                            FdblConsole.WriteMessage("   Gathering client information", true, false);

                            FdblXmlParser fxp = _GetXmlInfo(et);
                            FdblXmlNodeList fxnl = fxp.GetNodeList("Records/Record");

                            if (fxnl == null || fxnl.Count == 0) {

                                FdblConsole.WriteMessage(" (0 found)", false, true);

                            } else  if (fxnl != null && fxnl.Count > 0) {

                                jd.Clients = fxnl.Count;

                                FdblConsole.WriteMessage(string.Format(" ({0} found)", fxnl.Count), false, true);

                                FdblConsole.WriteMessage("   Building and sending emails");

                                for (int i = 0; i < fxnl.Count; i++) {

                                    int idProcessRecord = -1;

                                    try {

                                        _BuildSendEmail(et, addressList, attachList, fxnl.GetNode(i + 1), jd, out idProcessRecord);

                                        if (et.IsNewEmployee && etp != null) {

                                            jdPwd.IsActive = true;

                                            _BuildSendEmail(etp, addressList, attachList, fxnl.GetNode(i + 1), jdPwd, out idProcessRecord);

                                        }

                                    } catch (Exception ex) {

                                        _JobLog(jd, idProcessRecord, ex);

                                    }

                                }

                            }

                        }

                    } catch (Exception ex) {

                        _JobLog(jd, -1, ex);

                    }

                    jd.DTFinish = DateTime.Now;
                    jdPwd.DTFinish = DateTime.Now;

                    if (jd.IsActive) _JobDetail(jd);
                    if (jdPwd.IsActive) _JobDetail(jdPwd);

                    jd.Reset();
                    jdPwd.Reset();

                }

            } catch (Exception ex) {

                FdblSmtpRecord smtp = _MyConfig.FailureEmail;

                smtp.SetMessage(string.Format("{0}\n\n{1}", smtp.Message, FdblExceptions.GetDetails(ex)));

                try { FdblSmtp.Send(smtp); } catch { }

            } finally {

                if (spEmailTemplatesGet != null) spEmailTemplatesGet.Dispose();

                _JobFinish(jd.EmailJobId);

                FdblConsole.WriteMessage("Console is shutting down");

            }

        }

        #endregion

        #region Methods - Private (Process Email)

        private void _BuildSendEmail(Items.EmailTemplate et, List<Items.AddressRecord> addressList, List<Items.AttachmentRecord> attachList, XmlElement xe, Items.JobDetail jd, out int idProcessRecord) {

            idProcessRecord = -1;

            if (et == null) throw new ArgumentNullException("The email template is null");
            if (addressList == null) throw new ArgumentNullException("The email template address list is null");
            if (xe == null) throw new ArgumentNullException("The email info data node is null");

            if (addressList.Count == 0) throw new ArgumentException("address list is empty");

            Sql.spSE_Queue_Insert spQueueInsert = null;
            Sql.spSE_QueueAttachment_Insert spQueueAttachmentInsert = null;
            Sql.spSE_QueueLog_Insert spQueueLogInsert = null;

            Exception lex = null;

            try {

                spQueueInsert = new Sql.spSE_Queue_Insert(_MyConfig.SqlFactory.GetConnectionString());
                spQueueAttachmentInsert = new Sql.spSE_QueueAttachment_Insert(_MyConfig.SqlFactory.GetConnectionString());
                spQueueLogInsert = new Sql.spSE_QueueLog_Insert(_MyConfig.SqlFactory.GetConnectionString());

                Items.EmailRecord er = new Items.EmailRecord(et, addressList, xe);
                
                FdblConsole.WriteMessage(string.Format("      Processing {0} {1} (id: {2})", er.FirstName, er.LastName, er.GetId()));

                idProcessRecord = er.EmployeeId;

                if (er.CanDeliver) {

                    string usrPassword = null;

                    if (et.IsNewEmployeePassword) {

                        usrPassword = _GetAccountPassword(er);

                    }

                    FdblSmtpRecord fsr = new FdblSmtpRecord();

                    fsr.SmtpServer = _MyConfig.FailureEmail.SmtpServer;
                    fsr.SendFrom = er.SentFrom;
                    fsr.Sender = er.SentSender;
                    fsr.ReplyTo = er.SentReplyTo;
                    fsr.SendTo = er.SendTo;
                    fsr.CopyTo = er.CopyTo;
                    fsr.BlindCopyTo = er.BlindCopyTo;
                    fsr.Subject = er.Subject;

                    if (!string.IsNullOrEmpty(er.SendTo)) FdblConsole.WriteMessage(string.Format("         to  : {0}", er.SendTo));
                    if (!string.IsNullOrEmpty(er.CopyTo)) FdblConsole.WriteMessage(string.Format("         cc  : {0}", er.CopyTo));
                    if (!string.IsNullOrEmpty(er.BlindCopyTo)) FdblConsole.WriteMessage(string.Format("         bcc : {0}", er.BlindCopyTo));

                    if (!string.IsNullOrEmpty(er.MessageText)) {

                        if (et.IsNewEmployeePassword) fsr.Message = er.MessageText.Replace("{Password}", usrPassword);
                        else fsr.Message = er.MessageText;

                    }

                    if (!string.IsNullOrEmpty(er.MessageHtml)) {

                        if (et.IsNewEmployeePassword) fsr.Message = er.MessageHtml.Replace("{Password}", usrPassword);
                        else fsr.MessageHtml = er.MessageHtml;

                    }

                    if (!et.IsNewEmployeePassword && attachList != null) {

                        for (int j = 0; j < attachList.Count; j++) {

                            if (!string.IsNullOrEmpty(attachList[j].AttachmentName)) fsr.AddFileAttachment(attachList[j].AttachmentPath, attachList[j].AttachmentName);
                            else fsr.AddFileAttachment(attachList[j].AttachmentPath);

                        }

                    }

                    try {

                        FdblSmtp.Send(fsr);

                        er.Delivered = true;

                    } catch (Exception ex) {

                        lex = ex;
                        er.Delivered = false;

                    }

                }

                if (er.Delivered) jd.Delivered++;
                if (er.IsAddressIssue || er.IsKeywordIssue) jd.Issues++;

                int spReturnCode = spQueueInsert.StartDataReader(er);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_Queue_Insert returned: {0}", spReturnCode));

                if (!spQueueInsert.MoveNextDataReader(true)) throw new MyException("spSE_Queue_Insert could not advance cursor");

                int idEmailQueue = Convert.ToInt32(spQueueInsert.GetDataReaderValue(0, -1));

                if (idEmailQueue != -1) {

                    if (!et.IsNewEmployeePassword && attachList != null) {

                        for (int j = 0; j < attachList.Count; j++) {

                            spQueueAttachmentInsert.StartDataReader(idEmailQueue, attachList[j].EmailTemplateAttachmentId);

                        }

                    }

                    if (!er.CanDeliver || !er.Delivered || er.IsKeywordIssue) {

                        if (!er.CanDeliver || er.IsKeywordIssue) {

                            if (er.IsKeywordIssue) {

                                spReturnCode = spQueueLogInsert.StartDataReader(idEmailQueue, MyConsole.IsKeywordIssue, null, "There was at least one keyword field left in the email", null);

                                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_QueueLog_Insert returned: {0}", spReturnCode));

                            }


                            //if (er.IsAddressIssue) {

                            //    for (int i = 0; i < er.AddressIssues.Count; i++) {

                            //        spReturnCode = spQueueLogInsert.StartDataReader(idEmailQueue, MyConsole.IsAddressIssue, er.AddressIssues[i].FieldName, null, er.AddressIssues[i].Details);

                            //        if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_QueueLog_Insert returned: {0}", spReturnCode));

                            //    }

                            //}

                        }

                        if (!er.Delivered && lex != null) {

                            spReturnCode = spQueueLogInsert.StartDataReader(idEmailQueue, MyConsole.IsSmtpIssue, lex);

                            if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_QueueLog_Insert returned: {0}", spReturnCode));

                        }

                    }

                }

            } finally {

                if (spQueueLogInsert != null) spQueueLogInsert.Dispose();
                if (spQueueAttachmentInsert != null) spQueueAttachmentInsert.Dispose();
                if (spQueueInsert != null) spQueueInsert.Dispose();

            }

        }

        #endregion

        #region Methods - Private (Various GETS)

        private string _GetAccountPassword(Items.EmailRecord er) {

            Sql.spSE_EmailPassword_Update spEmailPasswordUpdate = null;

            try {

                string password = Common.Crypto.GetPasswordString(8);
                string passwordSalt = Common.Crypto.GetSaltString(10);
                string passwordHash = Common.Crypto.ComputeSHA1Hash(password, passwordSalt);

                spEmailPasswordUpdate = new Sql.spSE_EmailPassword_Update(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailPasswordUpdate.StartDataReader(er.AgentId, er.EmployeeId, er.UserId, passwordHash, passwordSalt);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_EmailPassword_Update returned: {0} (Sql returned: {1}", spReturnCode, spEmailPasswordUpdate.SqlErrorCode));

                return password;

            } finally {

                if (spEmailPasswordUpdate != null) spEmailPasswordUpdate.Dispose();

            }

        }

        private List<Items.AddressRecord> _GetAddressList(Items.EmailTemplate et) {

            if (et == null) throw new ArgumentNullException("The email template is null");

            Sql.spSE_EmailAddresses_Get spEmailAddressesGet = null;

            try {

                spEmailAddressesGet = new Sql.spSE_EmailAddresses_Get(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailAddressesGet.StartDataReader(et.EmailTemplateId);

                if (spReturnCode == FdblSqlReturnCodes.NoData || spReturnCode == FdblSqlReturnCodes.NoMatch) throw new MyException("No email addresses have been defined for this template");

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_EmailAddresses_Get returned: {0}", spReturnCode));

                List<Items.AddressRecord> addressList = new List<Items.AddressRecord>();

                while (spEmailAddressesGet.MoveNextDataReader(true)) {

                    addressList.Add(new Items.AddressRecord(spEmailAddressesGet));

                }

                if (addressList.Count == 0) throw new MyException(string.Format("No addresses loaded for email template: {0}", et.TemplateName));

                if (et.IsNewEmployee) {

                    int i = 0;

                    while (i < addressList.Count) {

                        if (!string.IsNullOrEmpty(addressList[i].ExplicitValue)) addressList.RemoveAt(i);
                        //3/31/2014 kevin hue - removed this because tech group decided that we do want to send new employee emails, along with passwords to addresses other than EmployeeEmail. Some clients use personal email that's stored on flex fields.
                        //else if (!addressList[i].FieldName.Equals("EmployeeEmail", StringComparison.OrdinalIgnoreCase)) addressList.RemoveAt(i);
                        else i++;

                    }

                }

                return addressList;

            } finally {

                if (spEmailAddressesGet != null) spEmailAddressesGet.Dispose();

            }

        }

        private List<Items.AttachmentRecord> _GetAttachmentList(Items.EmailTemplate et) {

            if (et == null) throw new ArgumentNullException("The email template is null");

            Sql.spSE_EmailAttachments_Get spEmailAttachmentsGet = null;

            try {

                spEmailAttachmentsGet = new Sql.spSE_EmailAttachments_Get(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailAttachmentsGet.StartDataReader(et.EmailTemplateId);

                List<Items.AttachmentRecord> attachList = new List<Items.AttachmentRecord>();

                if (!(spReturnCode == FdblSqlReturnCodes.NoData || spReturnCode == FdblSqlReturnCodes.NoMatch)) {

                    if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_EmailAttachments_Get returned: {0}", spReturnCode));

                    while (spEmailAttachmentsGet.MoveNextDataReader(true)) {

                        Items.AttachmentRecord ar = new Items.AttachmentRecord(spEmailAttachmentsGet);

                        if (!File.Exists(ar.AttachmentPath)) throw new FileNotFoundException(string.Format("Could not locate template ({0}) attachment: {1}", et.TemplateName, ar.AttachmentPath));

                        attachList.Add(ar);

                    }

                }

                return attachList;

            } finally {

                if (spEmailAttachmentsGet != null) spEmailAttachmentsGet.Dispose();

            }
        }

        private Items.EmailTemplate _GetPasswordEmailTemplate(Items.EmailTemplate et) {

            if (et == null) throw new ArgumentNullException("The email template is null");

            Sql.spSE_EmailTemplatesPassword_Get spEmailTemplatesPasswordGet = null;

            try {

                spEmailTemplatesPasswordGet = new Sql.spSE_EmailTemplatesPassword_Get(_MyConfig.SqlFactory.GetConnectionString());

                //int spReturnCode = spEmailTemplatesPasswordGet.StartDataReader(et.EmailTemplateId, et.CompanyId);
                int spReturnCode = spEmailTemplatesPasswordGet.StartDataReader(MyConsole.NewEmployeePassword, et.CompanyId);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_EmailTemplatesPassword_Get returned: {0}", spReturnCode));

                if (!spEmailTemplatesPasswordGet.MoveNextDataReader(true)) throw new MyException("spSE_EmailTemplatesPassword_Get could not advance cursor");

                return new Items.EmailTemplate(spEmailTemplatesPasswordGet);

            } finally {

                if (spEmailTemplatesPasswordGet != null) spEmailTemplatesPasswordGet.Dispose();

            }

        }

        private FdblXmlParser _GetXmlInfo(Items.EmailTemplate et) {

            if (et == null) throw new ArgumentNullException("The email template is null");

            Sql.spSE_XmlInfo_Build spXmlInfoBuild = null;

            try {

                spXmlInfoBuild = new Sql.spSE_XmlInfo_Build(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spXmlInfoBuild.StartDataReader(et.EmailTemplateId);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_XmlInfo_Build returned: {0}", spReturnCode));

                string xmlData = string.Empty;

                while (spXmlInfoBuild.MoveNextDataReader(true)) {

                    xmlData += Convert.ToString(spXmlInfoBuild.GetDataReaderValue(0, null));

                }

                xmlData = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Records>" + xmlData + "</Records>";

                return new FdblXmlParser(xmlData, ParserLoadFrom.Buffer);

            } finally {

                if (spXmlInfoBuild != null) spXmlInfoBuild.Dispose();

            }

        }

        #endregion

        #region Methods - Private (Job Status/Details/Logs)

        private void _JobLog(Items.JobDetail jd, int idProcessRecord, Exception ex) {

            if (jd == null) throw new ArgumentNullException("job detail is null");

            Sql.spSE_EmailJobLog_Insert spEmailJobLogInsert = null;

            try {

                spEmailJobLogInsert = new Sql.spSE_EmailJobLog_Insert(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailJobLogInsert.StartDataReader(jd.EmailJobId, jd.EmailTemplateId, idProcessRecord, ex);

            } catch {

                // Ignore failed inserts (for now)

            } finally {

                if (spEmailJobLogInsert != null) spEmailJobLogInsert.Dispose();

            }

        }

        private void _JobDetail(Items.JobDetail jd) {

            if (jd == null) throw new ArgumentNullException("job detail is null");

            Sql.spSE_EmailJobDetail_Insert spEmailJobDetailInsert = null;

            try {

                spEmailJobDetailInsert = new Sql.spSE_EmailJobDetail_Insert(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailJobDetailInsert.StartDataReader(jd);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_EmailJobDetail_Insert returned: {0} (Sql returned: {1}", spReturnCode, spEmailJobDetailInsert.SqlErrorCode));

            } catch (Exception ex) {

                _JobLog(jd, -1, ex);

            } finally {

                if (spEmailJobDetailInsert != null) spEmailJobDetailInsert.Dispose();

            }

        }

        private int _JobStart(string timeEmailSchedule) {

            Sql.spSE_EmailJob_Insert spEmailJobInsert = null;

            try {

                spEmailJobInsert = new Sql.spSE_EmailJob_Insert(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailJobInsert.StartDataReader(timeEmailSchedule);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_EmailJob_Insert returned: {0} (Sql returned: {1}", spReturnCode, spEmailJobInsert.SqlErrorCode));

                if (!spEmailJobInsert.MoveNextDataReader(true)) throw new MyException("spSE_EmailJob_Insert could not advance cursor");

                return Convert.ToInt32(spEmailJobInsert.GetDataReaderValue(0, -1));

            } finally {

                if (spEmailJobInsert != null) spEmailJobInsert.Dispose();

            }

        }

        private void _JobFinish(int idJob) {

            Sql.spSE_EmailJob_Update spEmailJobUpdate = null;

            try {

                spEmailJobUpdate = new Sql.spSE_EmailJob_Update(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailJobUpdate.StartDataReader(idJob);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spSE_EmailJob_Update returned: {0} (Sql returned: {1}", spReturnCode, spEmailJobUpdate.SqlErrorCode));

            }  catch (Exception ex) {

                // Ignore failed inserts (for now)

            } finally {

                if (spEmailJobUpdate != null) spEmailJobUpdate.Dispose();

            }

        }

        #endregion

    }

}
