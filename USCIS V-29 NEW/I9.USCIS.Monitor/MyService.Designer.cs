namespace I9.USCIS.Monitor {
    partial class MyService {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this._ServiceLog = new System.Diagnostics.EventLog();
            this._InitTimer = new System.Timers.Timer();
            this._ServiceTimer = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this._ServiceLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._InitTimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ServiceTimer)).BeginInit();
            // 
            // _ServiceLog
            // 
            this._ServiceLog.Log = "I9.USCIS";
            this._ServiceLog.Source = "I9.USCIS.Log";
            this._ServiceLog.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(this._ServiceLog_EntryWritten);
            // 
            // _InitTimer
            // 
            this._InitTimer.AutoReset = false;
            this._InitTimer.Interval = 15000D;
            this._InitTimer.Elapsed += new System.Timers.ElapsedEventHandler(this._InitTimer_Elapsed);
            // 
            // _ServiceTimer
            // 
            this._ServiceTimer.Interval = 30000D;
            this._ServiceTimer.Elapsed += new System.Timers.ElapsedEventHandler(this._ServiceTimer_Elapsed);
            // 
            // MyService
            // 
            this.ServiceName = "I9.USCIS.Monitor";
            ((System.ComponentModel.ISupportInitialize)(this._ServiceLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._InitTimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ServiceTimer)).EndInit();

        }

        #endregion

        private System.Timers.Timer _ServiceTimer;
        private System.Timers.Timer _InitTimer;
        private System.Diagnostics.EventLog _ServiceLog;
    }
}
