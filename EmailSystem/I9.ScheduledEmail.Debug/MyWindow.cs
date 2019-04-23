using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Sql;
using Fdbl.Toolkit.Utils;
using Fdbl.Toolkit.Xml;

namespace I9.ScheduledEmail.Debug {

    public partial class MyWindow : Form {

        #region Members

        private MyConfiguration _MyConfig = null;

        #endregion

        #region Constructors

        public MyWindow() {

            InitializeComponent();

            _MyConfig = new MyConfiguration();

        }

        #endregion

        #region Events - Private (File Menu)

        private void mnuFileExit_Click(object sender, EventArgs e) {

            this.Close();

        }

        #endregion

        #region Events - Private (File Menu)

        private void mnuActionGenerateXml_Click(object sender, EventArgs e) {

            tssProgress.Visible = true;
            tssProgress.Value = 0;

            tssMessage.Text = "Validating form input";

            if (string.IsNullOrEmpty(this.txtEmailTemplateId.Text)) {

                MessageBox.Show("You must enter a value for: Email Template Id", "Fragomen");
                this.txtEmailTemplateId.Focus();

                tssProgress.Visible = false;
                tssMessage.Text = "";

                return;

            }

            if (string.IsNullOrEmpty(this.txtXmlFile.Text)) {

                MessageBox.Show("You must enter a value for: Destination Xml File", "Fragomen");
                this.txtXmlFile.Focus();

                tssProgress.Visible = false;
                tssMessage.Text = "";

                return;

            }

            Sql.DEBUG_spSE_XmlInfo_Build spXmlInfoBuild = null;

            try {

                int idEmailTemplate = int.Parse(this.txtEmailTemplateId.Text.Trim());

                tssProgress.Value = 10;
                tssMessage.Text = "Creating sql connection";
                ssApp.Refresh();

                spXmlInfoBuild = new Sql.DEBUG_spSE_XmlInfo_Build(_MyConfig.SqlFactory.GetConnectionString());

                tssProgress.Value = 20;
                tssMessage.Text = "Executing sql stored procedure";
                ssApp.Refresh();

                int spReturnCode = spXmlInfoBuild.StartDataReader(idEmailTemplate);

                tssProgress.Value = 30;
                tssMessage.Text = "Validating sql return";
                ssApp.Refresh();

                if (spReturnCode != FdblSqlReturnCodes.Success) throw new MyException(string.Format("DEBUG_spSE_XmlInfo_Build returned: {0}", spReturnCode));

                tssProgress.Value = 40;
                tssMessage.Text = "Parsing sql data";
                ssApp.Refresh();

                string xmlData = string.Empty;

                while (spXmlInfoBuild.MoveNextDataReader(true)) {

                    xmlData += Convert.ToString(spXmlInfoBuild.GetDataReaderValue(0, null));

                }

                tssProgress.Value = 50;
                tssMessage.Text = "Creating xml data string";
                ssApp.Refresh();

                xmlData = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Records>" + xmlData + "</Records>";

                tssProgress.Value = 60;
                tssMessage.Text = "Loading xml into parser";
                ssApp.Refresh();

                FdblXmlParser fxp = new FdblXmlParser(xmlData, ParserLoadFrom.Buffer);

                tssProgress.Value = 70;
                tssMessage.Text = "Saving xml data";
                ssApp.Refresh();

                fxp.Document.Save(this.txtXmlFile.Text);

                tssProgress.Value = 80;
                tssMessage.Text = "Xml data has been saved";
                ssApp.Refresh();

            } catch (Exception ex) {

                tssMessage.Text = string.Format("ERROR: {0}", ex.Message);
                ssApp.Refresh();

                MessageBox.Show(FdblExceptions.GetDetails(ex), "Fragomen");

            } finally {

                tssProgress.Visible = false;
                tssProgress.Value = 0;
                ssApp.Refresh();

                if (spXmlInfoBuild != null) spXmlInfoBuild.Dispose();

                ssTimer.Enabled = true;

            }

        }

        private void mnuActionClearForm_Click(object sender, EventArgs e) {

            this.txtEmailTemplateId.Text = "";
            this.txtXmlFile.Text = "";

        }

        #endregion

        #region Events - Private (Form)

        private void lnkXmlFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {

            this.sfd.ShowDialog();

            this.txtXmlFile.Text = this.sfd.FileName;

        }

        private void ssTimer_Tick(object sender, EventArgs e) {

            ssTimer.Enabled = false;
            tssMessage.Text = "";

        }

        #endregion

    }

}
