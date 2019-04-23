using System;
using System.Windows.Forms;

namespace I9.USCIS.DynamicData {

    static class Program {

        #region Entry Point

        [STAThread]
        static void Main() {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WinMain());

        }

        #endregion

    }

}