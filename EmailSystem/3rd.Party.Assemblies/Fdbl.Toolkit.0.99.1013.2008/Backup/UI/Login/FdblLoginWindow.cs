using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Utils;

namespace Fdbl.Toolkit.UI.Login {

    public class FdblLoginWindow : System.Windows.Forms.Form {

        #region Members

        private int _Attempts;
        private bool _WasCancelled;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLoginInfo;
        private System.Windows.Forms.Button btnOk;

        private System.ComponentModel.Container components = null;

        #endregion

        #region Properties - Public

        public int Attempts {
            get { return _Attempts; }
        }

        public string Canonical {
            get { return string.Format("{0}:{1}", txtUserName.Text, txtPassword.Text); }
        }

        public string Password {
            get { return txtPassword.Text; }
        }

        public string UserName {
            get { return txtUserName.Text; }
        }

        public bool WasCancelled {
            get { return _WasCancelled; }
        }

        #endregion

        #region Methods - Public

        public void ResetPasswordField() {

            txtPassword.Text = "";
            txtPassword.Focus();

        }

        #endregion

        #region Constructors

        internal FdblLoginWindow() : this(null) { }

        internal FdblLoginWindow(string loginTitle) {

            InitializeComponent();

            if (Environment.OSVersion.Platform.Equals(PlatformID.Win32NT)) txtPassword.PasswordChar = (char)9679;
            else txtPassword.PasswordChar = (char)176;

            if (loginTitle != null) lblLoginInfo.Text = loginTitle;

            _Attempts = 0;
            _WasCancelled = false;

        }

        #endregion

        #region Methods - Protected

        protected override void Dispose(bool disposing) {

            if (disposing) {

                if (components != null) {
                    components.Dispose();
                }

            }

            base.Dispose(disposing);

        }

        #endregion

        #region Methods - Private


        private void btnOk_Click(object sender, System.EventArgs e) {

            _WasCancelled = false;
            _Attempts++;
            Close();

        }

        private void btnCancel_Click(object sender, System.EventArgs e) {

            _WasCancelled = true;
            Close();

        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent() {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FdblLoginWindow));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblLoginInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lblLoginInfo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(359, 80);
            this.panel1.TabIndex = 0;
            // 
            // lblLoginInfo
            // 
            this.lblLoginInfo.Location = new System.Drawing.Point(65, 15);
            this.lblLoginInfo.Name = "lblLoginInfo";
            this.lblLoginInfo.Size = new System.Drawing.Size(195, 20);
            this.lblLoginInfo.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Please enter your user name and password:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Login";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(290, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Username:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(15, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Password:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(90, 95);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(250, 22);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.Text = "";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(90, 130);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(250, 22);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.Text = "";
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOk.Location = new System.Drawing.Point(120, 170);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(105, 25);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Location = new System.Drawing.Point(235, 170);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FdblLoginWindow
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(359, 208);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FdblLoginWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Login";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

    }

}