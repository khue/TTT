using System;
using System.Windows.Forms;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Utils;

namespace Fdbl.Toolkit.UI.Password {

    public class FdblPasswordFactory {

        #region Methods - Public

        public static FdblPasswordWindow GetPasswordWindow() {

            return new FdblPasswordWindow();

        }

        public static FdblPasswordWindow GetPasswordWindow(string pwdTitle) {

            return new FdblPasswordWindow(pwdTitle);

        }

        public static FdblDialogResult RunPasswordWindow(string password, int maxAttempts, FdblPasswordEncoding pwdEncode) {

            return RunPasswordWindow(null, password, maxAttempts, pwdEncode, FdblDialogPosition.CenterScreen, null);

        }

        public static FdblDialogResult RunPasswordWindow(string password, int maxAttempts, FdblPasswordEncoding pwdEncode, IWin32Window owner) {

            return RunPasswordWindow(null, password, maxAttempts, pwdEncode, FdblDialogPosition.CenterScreen, owner);

        }

        public static FdblDialogResult RunPasswordWindow(string password, int maxAttempts, FdblPasswordEncoding pwdEncode, FdblDialogPosition winPosition) {

            return RunPasswordWindow(null, password, maxAttempts, pwdEncode, winPosition, null);

        }

        public static FdblDialogResult RunPasswordWindow(string password, int maxAttempts, FdblPasswordEncoding pwdEncode, FdblDialogPosition winPosition, IWin32Window owner) {

            return RunPasswordWindow(null, password, maxAttempts, pwdEncode, winPosition, owner);

        }

        public static FdblDialogResult RunPasswordWindow(string pwdTitle, string password, int maxAttempts, FdblPasswordEncoding pwdEncode) {

            return RunPasswordWindow(pwdTitle, password, maxAttempts, pwdEncode, FdblDialogPosition.CenterScreen, null);

        }

        public static FdblDialogResult RunPasswordWindow(string pwdTitle, string password, int maxAttempts, FdblPasswordEncoding pwdEncode, IWin32Window owner) {

            return RunPasswordWindow(pwdTitle, password, maxAttempts, pwdEncode, FdblDialogPosition.CenterScreen, owner);

        }

        public static FdblDialogResult RunPasswordWindow(string pwdTitle, string password, int maxAttempts, FdblPasswordEncoding pwdEncode, FdblDialogPosition winPosition) {

            return RunPasswordWindow(pwdTitle, password, maxAttempts, pwdEncode, FdblDialogPosition.CenterScreen, null);

        }

        public static FdblDialogResult RunPasswordWindow(string pwdTitle, string password, int maxAttempts, FdblPasswordEncoding pwdEncode, FdblDialogPosition winPosition, IWin32Window owner) {

            FdblPasswordWindow win = new FdblPasswordWindow(pwdTitle);

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

                    if (password.Equals(pwd)) return FdblDialogResult.Success;

                    MessageBox.Show("Incorrect password information.", "Incorrect Password");

                    win.ResetPasswordField();

                }

                MessageBox.Show("Too many failed password attempts.", "Incorrect Password");

                return FdblDialogResult.Failure;

            } finally {

                if (win != null) win.Dispose();

            }

        }

        #endregion

        #region Constructors

        private FdblPasswordFactory() { }

        #endregion

    }

}