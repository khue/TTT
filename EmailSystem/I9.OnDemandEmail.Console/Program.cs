using System;

using Fdbl.Toolkit;
using Fdbl.Toolkit.Smtp;
using Fdbl.Toolkit.Utils;

namespace I9.OnDemandEmail.Console {

    /*
     * 4/3/2019 kh - VSTS 10519. Email app has been updated so that if the SendTo address is blank, email will still be sent to cc and bcc. Please be aware that if the email address in sendto is bad (not a valid email), the email will not be sent at all. 
     * However, in the case this happens, there are automated emails sent to the client so they are aware and can fix the bad email address(es).
     * 
     */
    class Program {

        #region Members (Static)

        internal static readonly string _SmtpServer = "mailrelay-int.fragomen.net";
        internal static readonly string _SendFrom = "I9 Postmaster <DoNotReply@Fragoem.com>";
        internal static readonly string _SendTo = "khue@fragomen.com";
        internal static readonly string _Subject = "Application Issue (I9.OnDemandEmail.Console)";
        internal static readonly string _Message = "There was a problem with the application console.  The excetpion details should be listed below:";

        #endregion

        #region Entry Point

        static void Main(string[] args) {

            try {

                MyConsole con = new MyConsole();

                con.Run();

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