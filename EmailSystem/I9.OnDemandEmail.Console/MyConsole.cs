using System;
using System.Collections;
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

namespace I9.OnDemandEmail.Console {

    internal class MyConsole {

        #region Fields

        internal static readonly int NullId = -1;

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

        public void Run() {

            if (_AnotherInstanceRunning()) return;

            Hashtable htTemplates = new Hashtable();
            Hashtable htAddress = new Hashtable();
            Hashtable htAttachment = new Hashtable();
            Hashtable htPasswords = new Hashtable();

            Sql.spODE_EmailOnDemand_Get spEmailOnDemandGet = null;

            try {

                spEmailOnDemandGet = new Sql.spODE_EmailOnDemand_Get(_MyConfig.SqlFactory.GetConnectionString());

                FdblConsole.WriteMessage("Gathering on-demand emails for processing");

                int spReturnCode = spEmailOnDemandGet.StartDataReader();

                if (spReturnCode == FdblSqlReturnCodes.NoData || spReturnCode == FdblSqlReturnCodes.NoMatch) return;

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spODE_EmailOnDemand_Get returned: {0}", spReturnCode));

                List<Exception> listExceptions = new List<Exception>();

                while (spEmailOnDemandGet.MoveNextDataReader(true)) {

                    Items.EmailTemplate et = null;
                    Items.EmailTemplate etp = null;

                    Items.EmailOnDemand eod = new Items.EmailOnDemand(spEmailOnDemandGet);

                    FdblConsole.WriteMessage(string.Format("Processing on-demand email id: {0}", eod.EmailOnDemandId));

                    FdblConsole.WriteMessage(string.Format("   Fetching email template id: {0}", eod.EmailTemplateId));

                    try {

                        if (!htTemplates.ContainsKey(eod.EmailTemplateId)) et = _GetEmailTemplate(eod);
                        else et = (Items.EmailTemplate)htTemplates[eod.EmailTemplateId];

                    } catch (Exception ex) {

                        listExceptions.Add(ex);

                    } finally {

                        if (!htTemplates.ContainsKey(eod.EmailTemplateId)) htTemplates.Add(eod.EmailTemplateId, et);

                    }

                    if (et != null && !et.IsNewEmployeePassword) {

                        List<Items.AddressRecord> listAddress = null;
                        List<Items.AttachmentRecord> listAttachment = null;

                        Items.EmailOnDemandLog eodl = new Items.EmailOnDemandLog();
                        Items.EmailOnDemandLog eodlPwd = new Items.EmailOnDemandLog();

                        eodl.EmailOnDemandId = eod.EmailOnDemandId;
                        eodlPwd.EmailOnDemandId = eod.EmailOnDemandId;

                        FdblConsole.WriteMessage("   Gathering address information", true, false);

                        try {

                            if (!htAddress.ContainsKey(eod.EmailOnDemandId)) listAddress = _GetAddressList(et);
                            else listAddress = (List<Items.AddressRecord>)htAddress[eod.EmailOnDemandId];

                        } catch (Exception ex) {

                            listExceptions.Add(ex);

                        } finally {

                            if (!htAddress.ContainsKey(eod.EmailOnDemandId)) htAddress.Add(eod.EmailOnDemandId, listAddress);

                            FdblConsole.WriteMessage(string.Format(" ({0} found)", (listAddress == null  ? 0 : listAddress.Count)), false, true);

                        }

                        if (listAddress != null && listAddress.Count > 0) {

                            FdblConsole.WriteMessage("   Gathering attachments", true, false);

                            try {

                                if (!htAttachment.ContainsKey(eod.EmailOnDemandId)) listAttachment = _GetAttachmentList(et);
                                else listAttachment = (List<Items.AttachmentRecord>)htAttachment[eod.EmailOnDemandId];

                            } catch (Exception ex) {

                                listExceptions.Add(ex);

                            } finally {

                                if (!htAttachment.ContainsKey(eod.EmailOnDemandId)) htAttachment.Add(eod.EmailOnDemandId, listAttachment);

                                FdblConsole.WriteMessage(string.Format(" ({0} found)", (listAttachment == null ? 0 : listAttachment.Count)), false, true);

                            }

                            if (et.IsNewEmployeeAgentUser) {

                                FdblConsole.WriteMessage("   Gathering password information", true, false);

                                try {

                                    if (!htPasswords.ContainsKey(eod.EmailOnDemandId)) etp = _GetPasswordEmailTemplate(et);
                                    else etp = (Items.EmailTemplate)htPasswords[eod.EmailOnDemandId];

                                } catch (Exception ex) {

                                    listExceptions.Add(ex);

                                } finally {

                                    if (!htPasswords.ContainsKey(eod.EmailOnDemandId)) htPasswords.Add(eod.EmailOnDemandId, etp);

                                    FdblConsole.WriteMessage(" (1 found)", false, true);

                                }

                            }

                            FdblConsole.WriteMessage("   Gathering client information", true, false);

                            FdblXmlParser fxp = _GetXmlInfo(eod, et);
                            FdblXmlNodeList fxnl = fxp.GetNodeList("Records/Record");

                            if (fxnl == null || fxnl.Count == 0) {

                                FdblConsole.WriteMessage(" (0 found)", false, true);

                            } else if (fxnl != null && fxnl.Count == 1) {

                                FdblConsole.WriteMessage(" (1 found)", false, true);

                                FdblConsole.WriteMessage("   Building and sending email");

                                for (int i = 0; i < fxnl.Count; i++) {

                                    _BuildSendEmail(et, listAddress, listAttachment, fxnl.GetNode(1), eodl);
                                    _EmailOnDemandLog(eodl);

                                    if (!eodl.CatchException && et.IsNewEmployeeAgentUser && etp != null) {

                                        _BuildSendEmail(etp, listAddress, listAttachment, fxnl.GetNode(1), eodlPwd);
                                        _EmailOnDemandLog(eodlPwd);

                                    }

                                }

                            }

                        }

                        _EmailOnDemandUpdate(eod.EmailOnDemandId);

                    }

                }

                if (listExceptions.Count > 0) {

                    FdblConsole.WriteMessage("Sending administration notifications for internal exceptions");

                    FdblSmtpRecord smtp = _MyConfig.IssueEmail;

                    for (int i = 0; i < listExceptions.Count; i++) {

                        smtp.SetMessage(string.Format("{0}\n\n{1}", smtp.Message, FdblExceptions.GetDetails(listExceptions[i])));

                        try { FdblSmtp.Send(smtp); } catch { }

                        smtp.ResetMessage();

                    }

                }

            } catch (Exception ex) {

                FdblSmtpRecord smtp = _MyConfig.FailureEmail;

                smtp.SetMessage(string.Format("{0}\n\n{1}", smtp.Message, FdblExceptions.GetDetails(ex)));
                

                try { FdblSmtp.Send(smtp); } catch { }

            } finally {

                if (spEmailOnDemandGet != null) spEmailOnDemandGet.Dispose();

                FdblConsole.WriteMessage("Console is shutting down");

            }

        }

        #endregion

        #region Methods - Private (Process Email)

        private void _BuildSendEmail(Items.EmailTemplate et, List<Items.AddressRecord> addressList, List<Items.AttachmentRecord> attachList, XmlElement xe, Items.EmailOnDemandLog eodl) {

            if (et == null) throw new ArgumentNullException("The email template is null");
            if (addressList == null) throw new ArgumentNullException("The email template address list is null");
            if (xe == null) throw new ArgumentNullException("The email info data node is null");
            if (eodl == null) throw new ArgumentNullException("The email on-demand log is null");

            if (addressList.Count == 0) throw new ArgumentException("address list is empty");

            Sql.spODE_Queue_Insert spQueueInsert = null;
            Sql.spODE_QueueAttachment_Insert spQueueAttachmentInsert = null;
            Sql.spODE_QueueLog_Insert spQueueLogInsert = null;

            Exception lex = null;

            try {

                spQueueInsert = new Sql.spODE_Queue_Insert(_MyConfig.SqlFactory.GetConnectionString());
                spQueueAttachmentInsert = new Sql.spODE_QueueAttachment_Insert(_MyConfig.SqlFactory.GetConnectionString());
                spQueueLogInsert = new Sql.spODE_QueueLog_Insert(_MyConfig.SqlFactory.GetConnectionString());

                Items.EmailRecord er = new Items.EmailRecord(et, addressList, xe);

                FdblConsole.WriteMessage(string.Format("      Processing {0} {1} (id: {2})", er.FirstName, er.LastName, er.GetId()));

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

                if (er.Delivered) eodl.Delivered = true;
                if (er.IsAddressIssue || er.IsKeywordIssue) eodl.Issue = true;

                int spReturnCode = spQueueInsert.StartDataReader(er);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spODE_Queue_Insert returned: {0}", spReturnCode));

                if (!spQueueInsert.MoveNextDataReader(true)) throw new MyException("spODE_Queue_Insert could not advance cursor");

                int idEmailQueue = Convert.ToInt32(spQueueInsert.GetDataReaderValue(0, -1));

                if (idEmailQueue != -1) {

                    eodl.EmailQueueId = idEmailQueue;

                    if (!et.IsNewEmployeePassword && attachList != null) {

                        for (int j = 0; j < attachList.Count; j++) {

                            spQueueAttachmentInsert.StartDataReader(idEmailQueue, attachList[j].EmailTemplateAttachmentId);

                        }

                    }

                    if (!er.CanDeliver || !er.Delivered || er.IsKeywordIssue)
                    {

                        eodl.Message = "There were issues with the email.  Please check the EmailQueueLog table for more details";

                        if (!er.CanDeliver || er.IsKeywordIssue)
                        {

                            if (er.IsKeywordIssue) {

                                spReturnCode = spQueueLogInsert.StartDataReader(idEmailQueue, MyConsole.IsKeywordIssue, null, "There was at least one keyword field left in the email", null);

                                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spODE_QueueLog_Insert returned: {0}", spReturnCode));

                            }


                            if (er.IsAddressIssue) {

                                for (int i = 0; i < er.AddressIssues.Count; i++) {

                                    spReturnCode = spQueueLogInsert.StartDataReader(idEmailQueue, MyConsole.IsAddressIssue, er.AddressIssues[i].FieldName, null, er.AddressIssues[i].Details);

                                    if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spODE_QueueLog_Insert returned: {0}", spReturnCode));

                                }

                            }

                        }

                        if (!er.Delivered && lex != null) {

                            spReturnCode = spQueueLogInsert.StartDataReader(idEmailQueue, MyConsole.IsSmtpIssue, lex);

                            if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spODE_QueueLog_Insert returned: {0}", spReturnCode));

                        }

                    }

                }

            } catch (Exception ex) {

                eodl.CatchException = true;
                eodl.Message = ex.Message;
                eodl.Details = FdblExceptions.GetDetails(ex);

            } finally {

                if (spQueueLogInsert != null) spQueueLogInsert.Dispose();
                if (spQueueAttachmentInsert != null) spQueueAttachmentInsert.Dispose();
                if (spQueueInsert != null) spQueueInsert.Dispose();

            }

        }

        #endregion

        #region Methods - Private (Various GETS)

        private string _GetAccountPassword(Items.EmailRecord er) {

            Sql.spODE_EmailPassword_Update spEmailPasswordUpdate = null;

            try {

                string password = Common.Crypto.GetPasswordString(8);
                string passwordSalt = Common.Crypto.GetSaltString(10);
                string passwordHash = Common.Crypto.ComputeSHA1Hash(password, passwordSalt);

                spEmailPasswordUpdate = new Sql.spODE_EmailPassword_Update(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailPasswordUpdate.StartDataReader(er.AgentId, er.EmployeeId, er.UserId, passwordHash, passwordSalt);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spODE_EmailPassword_Update returned: {0} (Sql returned: {1}", spReturnCode, spEmailPasswordUpdate.SqlErrorCode));

                return password;

            } finally {

                if (spEmailPasswordUpdate != null) spEmailPasswordUpdate.Dispose();

            }

        }

        private List<Items.AddressRecord> _GetAddressList(Items.EmailTemplate et) {

            if (et == null) throw new ArgumentNullException("The email template is null");

            Sql.spODE_EmailAddresses_Get spEmailAddressesGet = null;

            try {

                spEmailAddressesGet = new Sql.spODE_EmailAddresses_Get(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailAddressesGet.StartDataReader(et.EmailTemplateId);

                if (spReturnCode == FdblSqlReturnCodes.NoData || spReturnCode == FdblSqlReturnCodes.NoMatch) throw new MyException("No email addresses have been defined for this template");

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spODE_EmailAddresses_Get returned: {0}", spReturnCode));

                List<Items.AddressRecord> addressList = new List<Items.AddressRecord>();

                while (spEmailAddressesGet.MoveNextDataReader(true)) {

                    addressList.Add(new Items.AddressRecord(spEmailAddressesGet));

                }

                if (addressList.Count == 0) throw new MyException(string.Format("No addresses loaded for email template: {0}", et.TemplateName));

                if (et.IsNewEmployeeAgentUser) {

                    int i = 0;

                    while (i < addressList.Count) {
                        
                        if (!string.IsNullOrEmpty(addressList[i].ExplicitValue)) addressList.RemoveAt(i);
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

            Sql.spODE_EmailAttachments_Get spEmailAttachmentsGet = null;

            try {

                spEmailAttachmentsGet = new Sql.spODE_EmailAttachments_Get(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailAttachmentsGet.StartDataReader(et.EmailTemplateId);

                List<Items.AttachmentRecord> attachList = new List<Items.AttachmentRecord>();

                if (!(spReturnCode == FdblSqlReturnCodes.NoData || spReturnCode == FdblSqlReturnCodes.NoMatch)) {

                    if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spODE_EmailAttachments_Get returned: {0}", spReturnCode));

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

        private Items.EmailTemplate _GetEmailTemplate(Items.EmailOnDemand eod) {

            Sql.spODE_EmailTemplate_Get spEmailTemplateGet = null;

            try {

                spEmailTemplateGet = new Sql.spODE_EmailTemplate_Get(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailTemplateGet.StartDataReader(eod.EmailTemplateId);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spODE_EmailTemplate_Get returned: {0}", spReturnCode));

                if (!spEmailTemplateGet.MoveNextDataReader(true)) throw new MyException("spODE_EmailTemplate_Get could not advance cursor");

                return new Items.EmailTemplate(spEmailTemplateGet);

            } finally {

                if (spEmailTemplateGet != null) spEmailTemplateGet.Dispose();

            }

        }

        private Items.EmailTemplate _GetPasswordEmailTemplate(Items.EmailTemplate et) {

            if (et == null) throw new ArgumentNullException("The email template is null");

            Sql.spODE_EmailTemplatePassword_Get spEmailTemplatesPasswordGet = null;

            try {

                spEmailTemplatesPasswordGet = new Sql.spODE_EmailTemplatePassword_Get(_MyConfig.SqlFactory.GetConnectionString());

                //int spReturnCode = spEmailTemplatesPasswordGet.StartDataReader(et.EmailTemplateId, et.CompanyId);
                int spReturnCode = spEmailTemplatesPasswordGet.StartDataReader(MyConsole.NewEmployeePassword, et.CompanyId);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spODE_EmailTemplatesPassword_Get returned: {0}", spReturnCode));

                if (!spEmailTemplatesPasswordGet.MoveNextDataReader(true)) throw new MyException("spODE_EmailTemplatesPassword_Get could not advance cursor");

                return new Items.EmailTemplate(spEmailTemplatesPasswordGet);

            } finally {

                if (spEmailTemplatesPasswordGet != null) spEmailTemplatesPasswordGet.Dispose();

            }

        }

        private FdblXmlParser _GetXmlInfo(Items.EmailOnDemand eod, Items.EmailTemplate et) {

            if (et == null) throw new ArgumentNullException("The email template is null");

            Sql.spODE_XmlInfo_Build spXmlInfoBuild = null;

            try {

                spXmlInfoBuild = new Sql.spODE_XmlInfo_Build(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spXmlInfoBuild.StartDataReader(eod.EmailOnDemandId, et.EmailTemplateId, et.EmailTemplateTypeId);

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("spODE_XmlInfo_Build returned: {0}", spReturnCode));

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

        #region Methods - Private (Logs/Queue)

        private void _EmailOnDemandLog(Items.EmailOnDemandLog eodl) {

            if (eodl == null) throw new ArgumentNullException("email on-demand log is null");

            Sql.spODE_EmailOnDemandLog_Insert spEmailOnDemandLogInsert = null;

            try {

                spEmailOnDemandLogInsert = new Sql.spODE_EmailOnDemandLog_Insert(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailOnDemandLogInsert.StartDataReader(eodl);

            } catch {

                // Ignore failed inserts (for now)

            } finally {

                if (spEmailOnDemandLogInsert != null) spEmailOnDemandLogInsert.Dispose();

            }

        }

        private void _EmailOnDemandUpdate(int idEmailOnDemand) {

            Sql.spODE_EmailOnDemand_Update spEmailOnDemandUpdate = null;

            try {

                spEmailOnDemandUpdate = new Sql.spODE_EmailOnDemand_Update(_MyConfig.SqlFactory.GetConnectionString());

                int spReturnCode = spEmailOnDemandUpdate.StartDataReader(idEmailOnDemand);

            } catch {

                // Ignore failed inserts (for now)

            } finally {

                if (spEmailOnDemandUpdate != null) spEmailOnDemandUpdate.Dispose();

            }

        }

        #endregion

        #region Methods - Private (Misc)

        private bool _AnotherInstanceRunning() {

            string fn = Path.GetFileName(Assembly.GetEntryAssembly().Location);

            FdblConsole.WriteMessage(string.Format("Checking for other instances of: {0}", fn));

            if (!FdblSystem.ApplicationAlreadyExecuting(Path.GetFileNameWithoutExtension(fn))) return false;

            FdblConsole.WriteMessage("Another console instance is running");
            FdblConsole.WriteMessage("Console is shutting down");

            return true;

        }

        #endregion


    }

}
