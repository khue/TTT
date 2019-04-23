using System;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Utils;

namespace I9.ScheduledEmail.Console {

    class Program {

        #region Members (Static)

        internal static readonly string _SmtpServer = "smtp-mn.fdbl-int.com";
        internal static readonly string _SendFrom = "I9 Scheduled Emails <DoNotReply@Fragomen.com>";
        internal static readonly string _SendTo = "khue@fragomen.com";
        internal static readonly string _Subject = "Application Issue (I9.ScheduledEmail.Console)";
        internal static readonly string _Message = "There was a problem with the application console.  The excetpion details should be listed below:";

        #endregion

        #region Entry Point

        static void Main(string[] args) {

            try {

                
                if (args.Length != 1) throw new MyException("Incorrect number of application arguments.  There should be only one argument and that is the template time to process.");

                string timeEmailSchedule = args[0];
                
                MyConsole con = new MyConsole();

                con.Run(timeEmailSchedule);

            } catch (Exception ex) {

                FdblSmtpRecord smtp = new FdblSmtpRecord();

                smtp.SmtpServer = _SmtpServer;
                smtp.SendFrom = _SendFrom;
                smtp.SendTo = _SendTo;
                smtp.Subject = _Subject;
                smtp.Message = string.Format("{0}\n\n{1}", _Message, FdblExceptions.GetDetails(ex));

                try { FdblSmtp.Send(smtp); } catch { }

            }
        }

        #endregion

    }

}