using System;
using System.Windows.Forms;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Utils;

namespace Fdbl.Toolkit.UI.Login {

    public class FdblLoginFactory {

        #region Methods - Public


        public static FdblLoginWindow GetLoginWindow() {

            return new FdblLoginWindow();

        }

        public static FdblLoginWindow GetLoginWindow(string loginTitle) {

            return new FdblLoginWindow(loginTitle);

        }

        public static FdblDialogResult RunLoginWindow(string userName, string password, int maxAttempts, FdblPasswordEncoding pwdEncode) {

            return RunLoginWindow(null, userName, password, maxAttempts, pwdEncode, FdblDialogPosition.CenterScreen);

        }

        public static FdblDialogResult RunLoginWindow(string userName, string password, int maxAttempts, FdblPasswordEncoding pwdEncode, IWin32Window owner) {

            return RunLoginWindow(null, userName, password, maxAttempts, pwdEncode, FdblDialogPosition.CenterScreen);

        }

        public static FdblDialogResult RunLoginWindow(string userName, string password, int maxAttempts, FdblPasswordEncoding pwdEncode, FdblDialogPosition winPosition) {

            return RunLoginWindow(null, userName, password, maxAttempts, pwdEncode, winPosition);

        }

        public static FdblDialogResult RunLoginWindow(string userName, string password, int maxAttempts, FdblPasswordEncoding pwdEncode, FdblDialogPosition winPosition, IWin32Window owner) {

            return RunLoginWindow(null, userName, password, maxAttempts, pwdEncode, winPosition);

        }

        public static FdblDialogResult RunLoginWindow(string loginTitle, string userName, string password, int maxAttempts, FdblPasswordEncoding pwdEncode) {

            return RunLoginWindow(loginTitle, userName, password, maxAttempts, pwdEncode, FdblDialogPosition.CenterScreen);

        }

        public static FdblDialogResult RunLoginWindow(string loginTitle, string userName, string password, int maxAttempts, FdblPasswordEncoding pwdEncode, IWin32Window owner) {

            return RunLoginWindow(loginTitle, userName, password, maxAttempts, pwdEncode, FdblDialogPosition.CenterScreen);

        }

        public static FdblDialogResult RunLoginWindow(string loginTitle, string userName, string password, int maxAttempts, FdblPasswordEncoding pwdEncode, FdblDialogPosition winPosition) {

            return RunLoginWindow(loginTitle, userName, password, maxAttempts, pwdEncode, FdblDialogPosition.CenterScreen);

        }

        public static FdblDialogResult RunLoginWindow(string loginTitle, string userName, string password, int maxAttempts, FdblPasswordEncoding pwdEncode, FdblDialogPosition winPosition, IWin32Window owner) {

            FdblLoginWindow win = new FdblLoginWindow(loginTitle);

            if (winPosition == FdblDialogPosition.CenterScreen) win.StartPosition = FormStartPosition.CenterScreen;
            else if (winPosition == FdblDialogPosition.CenterParent) win.StartPosition = FormStartPosition.CenterParent;
            else win.StartPosition = FormStartPosition.WindowsDefaultBounds;

            try {

                string pwd = null;

                while (win.Attempts < maxAttempts) {

                    if (owner == null) win.ShowDialog();
                    else win.ShowDialog(owner);

                    if (win.WasCancelled) return FdblDialogResult.Cancelled;

                    if (pwdEncode == FdblPasswordEncoding.CRC) pwd = Convert.ToString(Crypto.FdblHash.CRCString(win.Password));
                    if (pwdEncode == FdblPasswordEncoding.MD5) pwd = Crypto.FdblHash.HashString(win.Password, Crypto.FdblHashType.MD5);
                    if (pwdEncode == FdblPasswordEncoding.SHA1) pwd = Crypto.FdblHash.HashString(win.Password, Crypto.FdblHashType.SHA1);
                    if (pwdEncode == FdblPasswordEncoding.SHA256) pwd = Crypto.FdblHash.HashString(win.Password, Crypto.FdblHashType.SHA256);
                    if (pwdEncode == FdblPasswordEncoding.SHA384) pwd = Crypto.FdblHash.HashString(win.Password, Crypto.FdblHashType.SHA384);
                    if (pwdEncode == FdblPasswordEncoding.SHA512) pwd = Crypto.FdblHash.HashString(win.Password, Crypto.FdblHashType.SHA512);
                    if (pwdEncode == FdblPasswordEncoding.Plain) pwd = win.Password;

                    if (userName.Equals(win.UserName) && password.Equals(pwd)) return FdblDialogResult.Success;

                    MessageBox.Show("Incorrect login information.", "Incorrect Login");

                    win.ResetPasswordField();

                }

                MessageBox.Show("Too many failed login attempts.", "Incorrect Login");

                return FdblDialogResult.Failure;

            } finally {

                if (win != null) win.Dispose();

            }

        }

        #endregion

        #region Constructors

        private FdblLoginFactory() { }

        #endregion

    }

}