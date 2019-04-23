namespace I9.ScheduledEmail.Debug {
    partial class MyWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyWindow));
            this.label1 = new System.Windows.Forms.Label();
            this.txtEmailTemplateId = new System.Windows.Forms.MaskedTextBox();
            this.txtXmlFile = new System.Windows.Forms.TextBox();
            this.mnuApp = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAction = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuActionGenerateXml = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuActionClearForm = new System.Windows.Forms.ToolStripMenuItem();
            this.lnkXmlFile = new System.Windows.Forms.LinkLabel();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.ssApp = new System.Windows.Forms.StatusStrip();
            this.tssMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.ssTimer = new System.Windows.Forms.Timer(this.components);
            this.mnuApp.SuspendLayout();
            this.ssApp.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Email Template Id:";
            // 
            // txtEmailTemplateId
            // 
            this.txtEmailTemplateId.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmailTemplateId.Location = new System.Drawing.Point(122, 36);
            this.txtEmailTemplateId.Mask = "00000000";
            this.txtEmailTemplateId.Name = "txtEmailTemplateId";
            this.txtEmailTemplateId.Size = new System.Drawing.Size(63, 21);
            this.txtEmailTemplateId.TabIndex = 1;
            // 
            // txtXmlFile
            // 
            this.txtXmlFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtXmlFile.Location = new System.Drawing.Point(122, 70);
            this.txtXmlFile.Name = "txtXmlFile";
            this.txtXmlFile.Size = new System.Drawing.Size(330, 21);
            this.txtXmlFile.TabIndex = 3;
            // 
            // mnuApp
            // 
            this.mnuApp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuAction});
            this.mnuApp.Location = new System.Drawing.Point(0, 0);
            this.mnuApp.Name = "mnuApp";
            this.mnuApp.Size = new System.Drawing.Size(464, 24);
            this.mnuApp.TabIndex = 4;
            this.mnuApp.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(35, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(103, 22);
            this.mnuFileExit.Text = "Exit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuAction
            // 
            this.mnuAction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuActionGenerateXml,
            this.toolStripSeparator1,
            this.mnuActionClearForm});
            this.mnuAction.Name = "mnuAction";
            this.mnuAction.Size = new System.Drawing.Size(54, 20);
            this.mnuAction.Text = "Actions";
            // 
            // mnuActionGenerateXml
            // 
            this.mnuActionGenerateXml.Name = "mnuActionGenerateXml";
            this.mnuActionGenerateXml.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.mnuActionGenerateXml.Size = new System.Drawing.Size(188, 22);
            this.mnuActionGenerateXml.Text = "Generate Xml";
            this.mnuActionGenerateXml.Click += new System.EventHandler(this.mnuActionGenerateXml_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // mnuActionClearForm
            // 
            this.mnuActionClearForm.Name = "mnuActionClearForm";
            this.mnuActionClearForm.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.mnuActionClearForm.Size = new System.Drawing.Size(188, 22);
            this.mnuActionClearForm.Text = "Clear Form";
            this.mnuActionClearForm.Click += new System.EventHandler(this.mnuActionClearForm_Click);
            // 
            // lnkXmlFile
            // 
            this.lnkXmlFile.ActiveLinkColor = System.Drawing.Color.Blue;
            this.lnkXmlFile.AutoSize = true;
            this.lnkXmlFile.Location = new System.Drawing.Point(12, 73);
            this.lnkXmlFile.Name = "lnkXmlFile";
            this.lnkXmlFile.Size = new System.Drawing.Size(102, 13);
            this.lnkXmlFile.TabIndex = 5;
            this.lnkXmlFile.TabStop = true;
            this.lnkXmlFile.Text = "Destination Xml File:";
            this.lnkXmlFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkXmlFile_LinkClicked);
            // 
            // sfd
            // 
            this.sfd.DefaultExt = "xml";
            this.sfd.Filter = "Xml Files|*.xml";
            this.sfd.Title = "Destination Xml File";
            // 
            // ssApp
            // 
            this.ssApp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssMessage,
            this.tssProgress});
            this.ssApp.Location = new System.Drawing.Point(0, 103);
            this.ssApp.Name = "ssApp";
            this.ssApp.Size = new System.Drawing.Size(464, 22);
            this.ssApp.SizingGrip = false;
            this.ssApp.TabIndex = 7;
            // 
            // tssMessage
            // 
            this.tssMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssMessage.Name = "tssMessage";
            this.tssMessage.Size = new System.Drawing.Size(449, 17);
            this.tssMessage.Spring = true;
            this.tssMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tssProgress
            // 
            this.tssProgress.Maximum = 80;
            this.tssProgress.Name = "tssProgress";
            this.tssProgress.Size = new System.Drawing.Size(100, 16);
            this.tssProgress.Visible = false;
            // 
            // ssTimer
            // 
            this.ssTimer.Interval = 5000;
            this.ssTimer.Tick += new System.EventHandler(this.ssTimer_Tick);
            // 
            // MyWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 125);
            this.Controls.Add(this.ssApp);
            this.Controls.Add(this.lnkXmlFile);
            this.Controls.Add(this.txtXmlFile);
            this.Controls.Add(this.txtEmailTemplateId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mnuApp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuApp;
            this.MaximizeBox = false;
            this.Name = "MyWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "I9 Scheduled Email Debug Xml";
            this.mnuApp.ResumeLayout(false);
            this.mnuApp.PerformLayout();
            this.ssApp.ResumeLayout(false);
            this.ssApp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox txtEmailTemplateId;
        private System.Windows.Forms.TextBox txtXmlFile;
        private System.Windows.Forms.MenuStrip mnuApp;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.LinkLabel lnkXmlFile;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.StatusStrip ssApp;
        private System.Windows.Forms.ToolStripStatusLabel tssMessage;
        private System.Windows.Forms.ToolStripProgressBar tssProgress;
        private System.Windows.Forms.ToolStripMenuItem mnuAction;
        private System.Windows.Forms.ToolStripMenuItem mnuActionGenerateXml;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuActionClearForm;
        private System.Windows.Forms.Timer ssTimer;
    }
}

