using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;

using com.vis_dhs.www;

namespace I9.USCIS.DynamicData {

    public partial class WinMain : Form {

        #region Constructors

        public WinMain() {

            InitializeComponent();

        }

        #endregion

        #region Private - Events (Form Buttons)

        private void btnEmpGetCitizenshipStatusCode_Click(object sender, EventArgs e) {

            this.rtEmpGetCitizenshipStatusCodes.Clear();
            this.rtEmpGetCitizenshipStatusCodes.Refresh();

            EmployerWebServiceV18Wse wse = null;

            try {

                _PostMessageT1("Creating ws proxy");

                wse = new EmployerWebServiceV18Wse();

                wse.Timeout = (45 * 1000);

                wse.RequestSoapContext.Security.Timestamp.TtlInSeconds = 45;
                wse.RequestSoapContext.Security.Tokens.Add(new UsernameToken("RGRA9922", "ASty5O18n5xH", PasswordOption.SendPlainText));

                _PostMessageT1("");
                _PostMessageT1("Validating cps connection");

                EmpCpsVerifyConnectionResp respConnection = wse.EmpCpsVerifyConnection();

                _PostMessageT1(string.Format("  Return Status: {0}", respConnection.ReturnStatus));
                _PostMessageT1(string.Format("  Return Message: {0}", respConnection.ReturnStatusMsg));

                _PostMessageT1("");
                _PostMessageT1("Getting citizenship codes");

                EmpGetCitizenshipCodesResp respCitizenship = wse.EmpGetCitizenshipStatusCodes();

                _PostMessageT1(string.Format("  Return Status: {0}", respCitizenship.ReturnStatus));
                _PostMessageT1(string.Format("  Return Message: {0}", respCitizenship.ReturnStatusMsg));

                CitizenshipCodeListArray[] citizenCodes = respCitizenship.CitizenshipCodeArray;

                _PostMessageT1(string.Format("  Number of Codes: {0}", respCitizenship.NumberOfCitizenshipCodes));

                for (int i = 0; i < respCitizenship.NumberOfCitizenshipCodes; i++) {

                    _PostMessageT1("");
                    _PostMessageT1(string.Format("  Citizenship Entry #{0}", i));
                    _PostMessageT1(string.Format("    Code: {0}", citizenCodes[i].CitizenshipCode));
                    _PostMessageT1(string.Format("    Name: {0}", citizenCodes[i].CitizenshipName));
                    _PostMessageT1(string.Format("    Desc: {0}", citizenCodes[i].CitizenshipDescr));

                }

                _PostMessageT1("");
                _PostMessageT1("Finished");

            } catch (Exception ex) {

                _PostMessageT1("Issue while processing");
                _PostMessageT1(_GetDetails(ex));

            } finally {

                if (wse != null) wse.Dispose();

            }

        }

        private void btnEmpGetAvailableDocumentTypes_Click(object sender, EventArgs e) {

            this.rtEmpGetAvailableDocumentTypes.Clear();
            this.rtEmpGetAvailableDocumentTypes.Refresh();

            if (string.IsNullOrEmpty(this.txtCitizenshipCodeT2.Text)) {

                MessageBox.Show("You must enter a citizenship code");
                this.txtCitizenshipCodeT2.Focus();
                return;

            }

            EmployerWebServiceV18Wse wse = null;

            try {

                _PostMessageT2("Creating ws proxy");

                wse = new EmployerWebServiceV18Wse();

                wse.Timeout = (45 * 1000);

                wse.RequestSoapContext.Security.Timestamp.TtlInSeconds = 45;
                wse.RequestSoapContext.Security.Tokens.Add(new UsernameToken("RGRA9922", "ASty5O18n5xH", PasswordOption.SendPlainText));

                _PostMessageT2("");
                _PostMessageT2("Validating cps connection");

                EmpCpsVerifyConnectionResp respConnection = wse.EmpCpsVerifyConnection();

                _PostMessageT2(string.Format("  Return Status: {0}", respConnection.ReturnStatus));
                _PostMessageT2(string.Format("  Return Message: {0}", respConnection.ReturnStatusMsg));

                _PostMessageT2("");
                _PostMessageT2(string.Format("Getting available documentent types for citizenship code: {0}", this.txtCitizenshipCodeT2.Text));

                EmpGetAvailableDocumentTypesResp respDocuments = wse.EmpGetAvailableDocumentTypes(this.txtCitizenshipCodeT2.Text);

                _PostMessageT2(string.Format("  Return Status: {0}", respDocuments.ReturnStatus));
                _PostMessageT2(string.Format("  Return Message: {0}", respDocuments.ReturnStatusMsg));

                DocTypeListArray[] documentCodes = respDocuments.DocTypeArray;

                _PostMessageT2(string.Format("  Number of Codes: {0}", respDocuments.NumberOfDocTypes));

                for (int i = 0; i < respDocuments.NumberOfDocTypes; i++) {

                    _PostMessageT2("");
                    _PostMessageT2(string.Format("  Document Entry #{0}", i));
                    _PostMessageT2(string.Format("    Id:   {0}", documentCodes[i].DocumentId));
                    _PostMessageT2(string.Format("    Type: {0}", documentCodes[i].DocumentType));
                    _PostMessageT2(string.Format("    Code: {0}", documentCodes[i].CitizenshipCode));
                    _PostMessageT2(string.Format("    Name: {0}", documentCodes[i].CitizenshipName));

                }

                _PostMessageT2("");
                _PostMessageT2("Finished");

            } catch (Exception ex) {

                _PostMessageT2("Issue while processing");
                _PostMessageT2(_GetDetails(ex));

            } finally {

                if (wse != null) wse.Dispose();

            }

        }

        private void btnEmpGetAllDataFields_Click(object sender, EventArgs e) {

            this.rtEmpGetAllDataFields.Clear();
            this.rtEmpGetAllDataFields.Refresh();

            if (string.IsNullOrEmpty(this.txtCitizenshipCodeT3.Text)) {

                MessageBox.Show("You must enter a citizenship code");
                this.txtCitizenshipCodeT3.Focus();
                return;

            }

            if (string.IsNullOrEmpty(this.txtDocumentIdT3.Text)) {

                MessageBox.Show("You must enter a document id");
                this.txtDocumentIdT3.Focus();
                return;

            }

            EmployerWebServiceV18Wse wse = null;

            try {

                _PostMessageT3("Creating ws proxy");

                wse = new EmployerWebServiceV18Wse();

                wse.Timeout = (45 * 1000);

                wse.RequestSoapContext.Security.Timestamp.TtlInSeconds = 45;
                wse.RequestSoapContext.Security.Tokens.Add(new UsernameToken("RGRA9922", "ASty5O18n5xH", PasswordOption.SendPlainText));

                _PostMessageT3("");
                _PostMessageT3("Validating cps connection");

                EmpCpsVerifyConnectionResp respConnection = wse.EmpCpsVerifyConnection();

                _PostMessageT3(string.Format("  Return Status: {0}", respConnection.ReturnStatus));
                _PostMessageT3(string.Format("  Return Message: {0}", respConnection.ReturnStatusMsg));

                _PostMessageT3("");
                _PostMessageT3(string.Format("Getting available field codes for documentent type ({0}) for citizenship code ({1})", this.txtDocumentIdT3.Text, this.txtCitizenshipCodeT3.Text));

                EmpGetAllDataFieldsResp respFields = wse.EmpGetAllDataFields(this.txtCitizenshipCodeT3.Text, this.txtDocumentIdT3.Text);

                _PostMessageT3(string.Format("  Return Status: {0}", respFields.ReturnStatus));
                _PostMessageT3(string.Format("  Return Message: {0}", respFields.ReturnStatusMsg));

                FieldListArray[] fieldCodes = respFields.FieldArray;

                _PostMessageT3(string.Format("  Number of Codes: {0}", respFields.NumberOfFieldRecords));

                for (int i = 0; i < respFields.NumberOfFieldRecords; i++) {

                    _PostMessageT3("");
                    _PostMessageT3(string.Format("  Field Entry #{0}", i));
                    _PostMessageT3(string.Format("    Name: {0}", fieldCodes[i].DataElementName));
                    _PostMessageT3(string.Format("    Req:  {0}", fieldCodes[i].RequiredInd));
                    _PostMessageT3(string.Format("    Id:   {0}", fieldCodes[i].DocumentId));
                    _PostMessageT3(string.Format("    Type: {0}", fieldCodes[i].DocumentType));
                    _PostMessageT3(string.Format("    Code: {0}", fieldCodes[i].CitizenshipCode));
                    _PostMessageT3(string.Format("    Name: {0}", fieldCodes[i].CitizenshipName));

                }

                _PostMessageT3("");
                _PostMessageT3("Finished");

            } catch (Exception ex) {

                _PostMessageT3("Issue while processing");
                _PostMessageT3(_GetDetails(ex));

            } finally {

                if (wse != null) wse.Dispose();

            }

        }

        private void btnGenerateFlat_Click(object sender, EventArgs e) {

            this.rtGenerateXml.Clear();
            this.rtGenerateXml.Refresh();

            XmlTextWriter xtw = null;
            EmployerWebServiceV18Wse wse = null;

            try {


                _PostMessageT4("Getting xml file name from user");

                if (sfd.ShowDialog() == DialogResult.Cancel) {

                    _PostMessageT4("Operation cancelled by user");
                    return;

                }

                _PostMessageT4("");
                _PostMessageT4("Creating xml writer");

                xtw = new XmlTextWriter(sfd.FileName, Encoding.UTF8);

                xtw.Formatting = Formatting.Indented;

                xtw.WriteStartDocument();

                xtw.WriteStartElement("USCIS");

                _PostMessageT4("");
                _PostMessageT4("Creating ws proxy");

                wse = new EmployerWebServiceV18Wse();

                wse.Timeout = (45 * 1000);

                wse.RequestSoapContext.Security.Timestamp.TtlInSeconds = 45;
                wse.RequestSoapContext.Security.Tokens.Add(new UsernameToken("RGRA9922", "ASty5O18n5xH", PasswordOption.SendPlainText));

                _PostMessageT4("");
                _PostMessageT4("Validating cps connection");

                EmpCpsVerifyConnectionResp respConnection = wse.EmpCpsVerifyConnection();

                _PostMessageT4(string.Format("  Return Status: {0}", respConnection.ReturnStatus));
                _PostMessageT4(string.Format("  Return Message: {0}", respConnection.ReturnStatusMsg));

                _PostMessageT4("");
                _PostMessageT4("Getting citizenship codes");

                EmpGetCitizenshipCodesResp respCitizenship = wse.EmpGetCitizenshipStatusCodes();

                _PostMessageT4(string.Format("  Return Status: {0}", respCitizenship.ReturnStatus));
                _PostMessageT4(string.Format("  Return Message: {0}", respCitizenship.ReturnStatusMsg));
                _PostMessageT4(string.Format("  Number of Codes: {0}", respCitizenship.NumberOfCitizenshipCodes));

                CitizenshipCodeListArray[] citizenCodes = respCitizenship.CitizenshipCodeArray;

                _PostMessageT4("");
                _PostMessageT4("Writing citizenship codes as xml");

                xtw.WriteStartElement("EmpGetCitizenshipStatusCodes");

                for (int i = 0; i < respCitizenship.NumberOfCitizenshipCodes; i++) {

                    xtw.WriteStartElement("Citizenship");
                    xtw.WriteAttributeString("Code", citizenCodes[i].CitizenshipCode);
                    xtw.WriteAttributeString("Name", citizenCodes[i].CitizenshipName);
                    xtw.WriteString(citizenCodes[i].CitizenshipDescr);
                    xtw.WriteEndElement();

                }

                xtw.WriteEndElement();

                xtw.WriteStartElement("EmpGetAvailableDocumentTypes");

                Hashtable ht = new Hashtable();

                for (int i = 0; i < respCitizenship.NumberOfCitizenshipCodes; i++) {

                    _PostMessageT4("");
                    _PostMessageT4(string.Format("Getting available documentent types for citizenship code: {0}", citizenCodes[i].CitizenshipCode));

                    EmpGetAvailableDocumentTypesResp respDocuments = wse.EmpGetAvailableDocumentTypes(citizenCodes[i].CitizenshipCode);

                    _PostMessageT4(string.Format("  Return Status: {0}", respDocuments.ReturnStatus));
                    _PostMessageT4(string.Format("  Return Message: {0}", respDocuments.ReturnStatusMsg));
                    _PostMessageT4(string.Format("  Number of Codes: {0}", respDocuments.NumberOfDocTypes));

                    DocTypeListArray[] documentCodes = respDocuments.DocTypeArray;

                    _PostMessageT4("");
                    _PostMessageT4("Writing document codes as xml");

                    for (int j = 0; j < respDocuments.NumberOfDocTypes; j++) {

                        xtw.WriteStartElement("Document");
                        xtw.WriteAttributeString("Id", documentCodes[j].DocumentId);
                        xtw.WriteAttributeString("Type", documentCodes[j].DocumentType);
                        xtw.WriteAttributeString("CitCode", documentCodes[j].CitizenshipCode);
                        xtw.WriteAttributeString("CitName", documentCodes[j].CitizenshipName);
                        xtw.WriteString(citizenCodes[i].CitizenshipDescr);
                        xtw.WriteEndElement();

                    }

                    ht.Add(i, documentCodes);

                }

                xtw.WriteEndElement();

                xtw.WriteStartElement("EmpGetAllDataFields");

                for (int i = 0; i < respCitizenship.NumberOfCitizenshipCodes; i++) {

                    DocTypeListArray[] documentCodes = (DocTypeListArray[])ht[i];

                    for (int j = 0; j < documentCodes.Length; j++) {

                        _PostMessageT4("");
                        _PostMessageT4(string.Format("Getting available field codes for documentent type ({0}) for citizenship code ({1})", documentCodes[j].DocumentId, documentCodes[j].CitizenshipCode));

                        EmpGetAllDataFieldsResp respFields = wse.EmpGetAllDataFields(documentCodes[j].CitizenshipCode, documentCodes[j].DocumentId);

                        _PostMessageT4(string.Format("  Return Status: {0}", respFields.ReturnStatus));
                        _PostMessageT4(string.Format("  Return Message: {0}", respFields.ReturnStatusMsg));
                        _PostMessageT4(string.Format("  Number of Codes: {0}", respFields.NumberOfFieldRecords));

                        FieldListArray[] fieldCodes = respFields.FieldArray;

                        _PostMessageT4("");
                        _PostMessageT4("Writing field codes as xml");

                        for (int k = 0; k < respFields.NumberOfFieldRecords; k++) {

                            xtw.WriteStartElement("Field");
                            xtw.WriteAttributeString("Name", fieldCodes[k].DataElementName);
                            xtw.WriteAttributeString("Required", fieldCodes[k].RequiredInd);
                            xtw.WriteAttributeString("DocId", fieldCodes[k].DocumentId);
                            xtw.WriteAttributeString("DocType", fieldCodes[k].DocumentType);
                            xtw.WriteAttributeString("CitCode", fieldCodes[k].CitizenshipCode);
                            xtw.WriteAttributeString("CitName", fieldCodes[k].CitizenshipName);
                            xtw.WriteString(citizenCodes[i].CitizenshipDescr);
                            xtw.WriteEndElement();

                        }

                    }

                }

                xtw.WriteEndElement();

                xtw.WriteEndElement();

                xtw.WriteEndDocument();

                _PostMessageT4("");
                _PostMessageT4("Finished generating xml file");

            } catch (Exception ex) {

                _PostMessageT4("Issue while processing");
                _PostMessageT4(_GetDetails(ex));

            } finally {

                if (xtw != null) {

                    xtw.Flush();
                    xtw.Close();

                }

                if (wse != null) wse.Dispose();

            }

        }

        private void btnGenerateHierarchical_Click(object sender, EventArgs e) {

            this.rtGenerateXml.Clear();
            this.rtGenerateXml.Refresh();

            XmlTextWriter xtw = null;
            EmployerWebServiceV18Wse wse = null;

            try {


                _PostMessageT4("Getting xml file name from user");

                if (sfd.ShowDialog() == DialogResult.Cancel) {

                    _PostMessageT4("Operation cancelled by user");
                    return;

                }

                _PostMessageT4("");
                _PostMessageT4("Creating xml writer");

                xtw = new XmlTextWriter(sfd.FileName, Encoding.UTF8);

                xtw.Formatting = Formatting.Indented;

                xtw.WriteStartDocument();

                xtw.WriteStartElement("USCIS");

                _PostMessageT4("");
                _PostMessageT4("Creating ws proxy");

                wse = new EmployerWebServiceV18Wse();

                wse.Timeout = (45 * 1000);

                wse.RequestSoapContext.Security.Timestamp.TtlInSeconds = 45;
                wse.RequestSoapContext.Security.Tokens.Add(new UsernameToken("RGRA9922", "ASty5O18n5xH", PasswordOption.SendPlainText));

                _PostMessageT4("");
                _PostMessageT4("Validating cps connection");

                EmpCpsVerifyConnectionResp respConnection = wse.EmpCpsVerifyConnection();

                _PostMessageT4(string.Format("  Return Status: {0}", respConnection.ReturnStatus));
                _PostMessageT4(string.Format("  Return Message: {0}", respConnection.ReturnStatusMsg));

                _PostMessageT4("");
                _PostMessageT4("Getting citizenship codes");

                EmpGetCitizenshipCodesResp respCitizenship = wse.EmpGetCitizenshipStatusCodes();

                _PostMessageT4(string.Format("  Return Status: {0}", respCitizenship.ReturnStatus));
                _PostMessageT4(string.Format("  Return Message: {0}", respCitizenship.ReturnStatusMsg));
                _PostMessageT4(string.Format("  Number of Codes: {0}", respCitizenship.NumberOfCitizenshipCodes));

                CitizenshipCodeListArray[] citizenCodes = respCitizenship.CitizenshipCodeArray;

                for (int i = 0; i < respCitizenship.NumberOfCitizenshipCodes; i++) {

                    _PostMessageT4("");
                    _PostMessageT4("Writing citizenship codes as xml");

                    xtw.WriteStartElement("Citizenship");
                    xtw.WriteAttributeString("Code", citizenCodes[i].CitizenshipCode);
                    xtw.WriteElementString("Name", citizenCodes[i].CitizenshipName);
                    xtw.WriteElementString("Description", citizenCodes[i].CitizenshipDescr);

                    _PostMessageT4("");
                    _PostMessageT4(string.Format("Getting available documentent types for citizenship code: {0}", citizenCodes[i].CitizenshipCode));

                    EmpGetAvailableDocumentTypesResp respDocuments = wse.EmpGetAvailableDocumentTypes(citizenCodes[i].CitizenshipCode);

                    _PostMessageT4(string.Format("  Return Status: {0}", respDocuments.ReturnStatus));
                    _PostMessageT4(string.Format("  Return Message: {0}", respDocuments.ReturnStatusMsg));
                    _PostMessageT4(string.Format("  Number of Codes: {0}", respDocuments.NumberOfDocTypes));

                    DocTypeListArray[] documentCodes = respDocuments.DocTypeArray;

                    for (int j = 0; j < respDocuments.NumberOfDocTypes; j++) {

                        _PostMessageT4("");
                        _PostMessageT4("Writing document codes as xml");

                        xtw.WriteStartElement("Document");
                        xtw.WriteAttributeString("Id", documentCodes[j].DocumentId);
                        xtw.WriteAttributeString("Type", documentCodes[j].DocumentType);

                        _PostMessageT4("");
                        _PostMessageT4(string.Format("Getting available field codes for documentent type ({0}) for citizenship code ({1})", documentCodes[j].DocumentId, documentCodes[j].CitizenshipCode));

                        EmpGetAllDataFieldsResp respFields = wse.EmpGetAllDataFields(documentCodes[j].CitizenshipCode, documentCodes[j].DocumentId);

                        _PostMessageT4(string.Format("  Return Status: {0}", respFields.ReturnStatus));
                        _PostMessageT4(string.Format("  Return Message: {0}", respFields.ReturnStatusMsg));
                        _PostMessageT4(string.Format("  Number of Codes: {0}", respFields.NumberOfFieldRecords));

                        FieldListArray[] fieldCodes = respFields.FieldArray;

                        _PostMessageT4("");
                        _PostMessageT4("Writing field codes as xml");

                        for (int k = 0; k < respFields.NumberOfFieldRecords; k++) {

                            xtw.WriteStartElement("Field");
                            xtw.WriteAttributeString("Name", fieldCodes[k].DataElementName);
                            xtw.WriteAttributeString("Required", fieldCodes[k].RequiredInd);
                            xtw.WriteEndElement();

                        }

                        xtw.WriteEndElement();

                    }
                    
                    xtw.WriteEndElement();

                }

                xtw.WriteEndElement();

                xtw.WriteEndDocument();

                _PostMessageT4("");
                _PostMessageT4("Finished generating xml file");

            } catch (Exception ex) {

                _PostMessageT4("Issue while processing");
                _PostMessageT4(_GetDetails(ex));

            } finally {

                if (xtw != null) {

                    xtw.Flush();
                    xtw.Close();

                }

                if (wse != null) wse.Dispose();

            }

        }

        #endregion

        #region Private - Methods

        private string _GetDetails(Exception ex) {

            if (ex == null) return string.Empty;

            if (ex.Message.ToLower().Equals("rethrow")) ex = ex.InnerException;

            string details = string.Format("{0}{1}{1}Exception Message Trace{1}", ex.Message, Environment.NewLine);

            Exception iex = ex.InnerException;
            while (iex != null) {
                details += string.Format("   {0}{1}", iex.Message, Environment.NewLine);
                iex = iex.InnerException;
            }

            details += string.Format("{0}Stack Trace{0}{1}{0}{0}Root Stack Trace{0}", Environment.NewLine, ex.StackTrace);

            iex = ex.InnerException;
            while (iex != null) {
                details += iex.StackTrace;
                iex = iex.InnerException;
                if (iex != null) details += Environment.NewLine;
            }
            details += Environment.NewLine;

            return details;
        }

        private void _PostMessageT1(string mesg) {

            this.rtEmpGetCitizenshipStatusCodes.AppendText(mesg + Environment.NewLine);
            this.rtEmpGetCitizenshipStatusCodes.Refresh();
            this.rtGenerateXml.ScrollToCaret();

        }

        private void _PostMessageT2(string mesg) {

            this.rtEmpGetAvailableDocumentTypes.AppendText(mesg + Environment.NewLine);
            this.rtEmpGetAvailableDocumentTypes.Refresh();
            this.rtGenerateXml.ScrollToCaret();

        }

        private void _PostMessageT3(string mesg) {

            this.rtEmpGetAllDataFields.AppendText(mesg + Environment.NewLine);
            this.rtEmpGetAllDataFields.Refresh();
            this.rtGenerateXml.ScrollToCaret();

        }

        private void _PostMessageT4(string mesg) {

            this.rtGenerateXml.AppendText(mesg + Environment.NewLine);
            this.rtGenerateXml.Refresh();
            this.rtGenerateXml.ScrollToCaret();

        }

        #endregion

    }

}