using System;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.XQueue.Live {

    class Program {

        #region Members (Static)

        internal static readonly string SmtpServer = "smtp-mn.fdbl-int.com";
        internal static readonly string SendFrom = "I9 Postmaster <DoNotReply@Fragomen.com>";
        internal static readonly string SendTo = "khue@fragomen.com";
        internal static readonly string Subject = "Application Issue (I9.USCIS.XQueue)";
        internal static readonly string Message = "There was a problem with the application console.  The excetpion details should be listed below:";

        #endregion

        #region Entry Point

        [STAThread]
        static void Main(string[] args) {

            try {

                MyConsole con = new MyConsole();

                con.Run();

            } catch (Exception ex) {

                FdblSmtpRecord smtp = new FdblSmtpRecord();

                smtp.SmtpServer = Program.SmtpServer;
                smtp.SendFrom = Program.SendFrom;
                smtp.SendTo = Program.SendTo;
                smtp.Subject = Program.Subject;
                smtp.Message = string.Format("{0}\n\n{1}", Program.Message, FdblExceptions.GetDetails(ex));

                try { FdblSmtp.Send(smtp); } catch { }

            }

        }

        #endregion

    }

}
