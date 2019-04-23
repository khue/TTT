namespace I9.USCIS.DynamicData {
    partial class WinMain {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinMain));
            this.rtEmpGetCitizenshipStatusCodes = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnEmpGetCitizenshipStatusCode = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnEmpGetAvailableDocumentTypes = new System.Windows.Forms.Button();
            this.rtEmpGetAvailableDocumentTypes = new System.Windows.Forms.RichTextBox();
            this.txtCitizenshipCodeT2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnEmpGetAllDataFields = new System.Windows.Forms.Button();
            this.rtEmpGetAllDataFields = new System.Windows.Forms.RichTextBox();
            this.txtDocumentIdT3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCitizenshipCodeT3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnGenerateHierarchical = new System.Windows.Forms.Button();
            this.rtGenerateXml = new System.Windows.Forms.RichTextBox();
            this.btnGenerateFlat = new System.Windows.Forms.Button();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtEmpGetCitizenshipStatusCodes
            // 
            this.rtEmpGetCitizenshipStatusCodes.BackColor = System.Drawing.SystemColors.Info;
            this.rtEmpGetCitizenshipStatusCodes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtEmpGetCitizenshipStatusCodes.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtEmpGetCitizenshipStatusCodes.Location = new System.Drawing.Point(3, 35);
            this.rtEmpGetCitizenshipStatusCodes.Name = "rtEmpGetCitizenshipStatusCodes";
            this.rtEmpGetCitizenshipStatusCodes.Size = new System.Drawing.Size(678, 563);
            this.rtEmpGetCitizenshipStatusCodes.TabIndex = 1;
            this.rtEmpGetCitizenshipStatusCodes.Text = "";
            this.rtEmpGetCitizenshipStatusCodes.WordWrap = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(692, 627);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnEmpGetCitizenshipStatusCode);
            this.tabPage1.Controls.Add(this.rtEmpGetCitizenshipStatusCodes);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(684, 601);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "EmpGetCitizenshipStatusCodes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnEmpGetCitizenshipStatusCode
            // 
            this.btnEmpGetCitizenshipStatusCode.Location = new System.Drawing.Point(578, 6);
            this.btnEmpGetCitizenshipStatusCode.Name = "btnEmpGetCitizenshipStatusCode";
            this.btnEmpGetCitizenshipStatusCode.Size = new System.Drawing.Size(98, 23);
            this.btnEmpGetCitizenshipStatusCode.TabIndex = 2;
            this.btnEmpGetCitizenshipStatusCode.Text = "Fetch Data";
            this.btnEmpGetCitizenshipStatusCode.UseVisualStyleBackColor = true;
            this.btnEmpGetCitizenshipStatusCode.Click += new System.EventHandler(this.btnEmpGetCitizenshipStatusCode_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnEmpGetAvailableDocumentTypes);
            this.tabPage2.Controls.Add(this.rtEmpGetAvailableDocumentTypes);
            this.tabPage2.Controls.Add(this.txtCitizenshipCodeT2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(684, 601);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "EmpGetAvailableDocumentTypes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnEmpGetAvailableDocumentTypes
            // 
            this.btnEmpGetAvailableDocumentTypes.Location = new System.Drawing.Point(578, 6);
            this.btnEmpGetAvailableDocumentTypes.Name = "btnEmpGetAvailableDocumentTypes";
            this.btnEmpGetAvailableDocumentTypes.Size = new System.Drawing.Size(98, 23);
            this.btnEmpGetAvailableDocumentTypes.TabIndex = 5;
            this.btnEmpGetAvailableDocumentTypes.Text = "Fetch Data";
            this.btnEmpGetAvailableDocumentTypes.UseVisualStyleBackColor = true;
            this.btnEmpGetAvailableDocumentTypes.Click += new System.EventHandler(this.btnEmpGetAvailableDocumentTypes_Click);
            // 
            // rtEmpGetAvailableDocumentTypes
            // 
            this.rtEmpGetAvailableDocumentTypes.BackColor = System.Drawing.SystemColors.Info;
            this.rtEmpGetAvailableDocumentTypes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtEmpGetAvailableDocumentTypes.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtEmpGetAvailableDocumentTypes.Location = new System.Drawing.Point(3, 35);
            this.rtEmpGetAvailableDocumentTypes.Name = "rtEmpGetAvailableDocumentTypes";
            this.rtEmpGetAvailableDocumentTypes.Size = new System.Drawing.Size(678, 563);
            this.rtEmpGetAvailableDocumentTypes.TabIndex = 4;
            this.rtEmpGetAvailableDocumentTypes.Text = "";
            this.rtEmpGetAvailableDocumentTypes.WordWrap = false;
            // 
            // txtCitizenshipCodeT2
            // 
            this.txtCitizenshipCodeT2.Location = new System.Drawing.Point(102, 6);
            this.txtCitizenshipCodeT2.Name = "txtCitizenshipCodeT2";
            this.txtCitizenshipCodeT2.Size = new System.Drawing.Size(274, 20);
            this.txtCitizenshipCodeT2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Citizenship Code:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnEmpGetAllDataFields);
            this.tabPage3.Controls.Add(this.rtEmpGetAllDataFields);
            this.tabPage3.Controls.Add(this.txtDocumentIdT3);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.txtCitizenshipCodeT3);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(684, 601);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "EmpGetAllDataFields";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnEmpGetAllDataFields
            // 
            this.btnEmpGetAllDataFields.Location = new System.Drawing.Point(578, 6);
            this.btnEmpGetAllDataFields.Name = "btnEmpGetAllDataFields";
            this.btnEmpGetAllDataFields.Size = new System.Drawing.Size(98, 23);
            this.btnEmpGetAllDataFields.TabIndex = 5;
            this.btnEmpGetAllDataFields.Text = "Fetch Data";
            this.btnEmpGetAllDataFields.UseVisualStyleBackColor = true;
            this.btnEmpGetAllDataFields.Click += new System.EventHandler(this.btnEmpGetAllDataFields_Click);
            // 
            // rtEmpGetAllDataFields
            // 
            this.rtEmpGetAllDataFields.BackColor = System.Drawing.SystemColors.Info;
            this.rtEmpGetAllDataFields.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtEmpGetAllDataFields.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtEmpGetAllDataFields.Location = new System.Drawing.Point(0, 60);
            this.rtEmpGetAllDataFields.Name = "rtEmpGetAllDataFields";
            this.rtEmpGetAllDataFields.Size = new System.Drawing.Size(684, 541);
            this.rtEmpGetAllDataFields.TabIndex = 4;
            this.rtEmpGetAllDataFields.Text = "";
            this.rtEmpGetAllDataFields.WordWrap = false;
            // 
            // txtDocumentIdT3
            // 
            this.txtDocumentIdT3.Location = new System.Drawing.Point(102, 34);
            this.txtDocumentIdT3.Name = "txtDocumentIdT3";
            this.txtDocumentIdT3.Size = new System.Drawing.Size(242, 20);
            this.txtDocumentIdT3.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Document Id:";
            // 
            // txtCitizenshipCodeT3
            // 
            this.txtCitizenshipCodeT3.Location = new System.Drawing.Point(102, 8);
            this.txtCitizenshipCodeT3.Name = "txtCitizenshipCodeT3";
            this.txtCitizenshipCodeT3.Size = new System.Drawing.Size(242, 20);
            this.txtCitizenshipCodeT3.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Citizenship Code:";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnGenerateHierarchical);
            this.tabPage4.Controls.Add(this.rtGenerateXml);
            this.tabPage4.Controls.Add(this.btnGenerateFlat);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(684, 601);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Generate Xml";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnGenerateHierarchical
            // 
            this.btnGenerateHierarchical.Location = new System.Drawing.Point(420, 6);
            this.btnGenerateHierarchical.Name = "btnGenerateHierarchical";
            this.btnGenerateHierarchical.Size = new System.Drawing.Size(125, 23);
            this.btnGenerateHierarchical.TabIndex = 2;
            this.btnGenerateHierarchical.Text = "Generate Hierarchical";
            this.btnGenerateHierarchical.UseVisualStyleBackColor = true;
            this.btnGenerateHierarchical.Click += new System.EventHandler(this.btnGenerateHierarchical_Click);
            // 
            // rtGenerateXml
            // 
            this.rtGenerateXml.BackColor = System.Drawing.SystemColors.Info;
            this.rtGenerateXml.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtGenerateXml.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtGenerateXml.Location = new System.Drawing.Point(0, 35);
            this.rtGenerateXml.Name = "rtGenerateXml";
            this.rtGenerateXml.Size = new System.Drawing.Size(684, 566);
            this.rtGenerateXml.TabIndex = 1;
            this.rtGenerateXml.Text = "";
            this.rtGenerateXml.WordWrap = false;
            // 
            // btnGenerateFlat
            // 
            this.btnGenerateFlat.Location = new System.Drawing.Point(551, 6);
            this.btnGenerateFlat.Name = "btnGenerateFlat";
            this.btnGenerateFlat.Size = new System.Drawing.Size(125, 23);
            this.btnGenerateFlat.TabIndex = 0;
            this.btnGenerateFlat.Text = "Generate Flat";
            this.btnGenerateFlat.UseVisualStyleBackColor = true;
            this.btnGenerateFlat.Click += new System.EventHandler(this.btnGenerateFlat_Click);
            // 
            // sfd
            // 
            this.sfd.Filter = "Xml files (*.xml)|*.xml";
            // 
            // WinMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 627);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WinMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get Dynamic Data";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtEmpGetCitizenshipStatusCodes;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnEmpGetAvailableDocumentTypes;
        private System.Windows.Forms.RichTextBox rtEmpGetAvailableDocumentTypes;
        private System.Windows.Forms.TextBox txtCitizenshipCodeT2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnEmpGetAllDataFields;
        private System.Windows.Forms.RichTextBox rtEmpGetAllDataFields;
        private System.Windows.Forms.TextBox txtDocumentIdT3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCitizenshipCodeT3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnEmpGetCitizenshipStatusCode;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.RichTextBox rtGenerateXml;
        private System.Windows.Forms.Button btnGenerateFlat;
        private System.Windows.Forms.Button btnGenerateHierarchical;
    }
}

