using System;
using System.ServiceProcess;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Utils;

namespace I9.USCIS.Monitor {

    static class Program {

        #region Members (Static)

        internal static readonly string SmtpServer = "smtp-mn.fdbl-int.com";
        internal static readonly string SendFrom = "I9 Postmaster <DoNotReply@Fragoem.com>";
        internal static readonly string SendTo = "kcarlisle@fragomen.com";
        internal static readonly string Subject = "Service Issue (I9.USCIS.Monitor)";
        internal static readonly string Message = "There was a problem with the service.  The excetpion details should be listed below:";

        #endregion

        #region Entry Point

        static void Main() {

            try {

                ServiceBase[] ServicesToRun = new ServiceBase[] { new MyService() };
                ServiceBase.Run(ServicesToRun);

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
