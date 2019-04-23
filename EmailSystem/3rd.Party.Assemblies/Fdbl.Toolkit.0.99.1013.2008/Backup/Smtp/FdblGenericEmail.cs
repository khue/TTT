using System;
using System.Reflection;

namespace Fdbl.Toolkit.Smtp {

    public class FdblGenericEmail {

        #region Method - Public (Static)

        public static FdblSmtpRecord GetSmtpRecord(string jid, Exception ex) {

            Assembly assembly = System.Reflection.Assembly.GetCallingAssembly();
            string aTitle = ((AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute))).Title;

            FdblSmtpRecord smtp = new FdblSmtpRecord();

            smtp.SmtpServer = "smtp-mn.fdbl-int.com";
            smtp.SendFrom = string.Format("{0} <DoNotReply@Fragomen.com>", aTitle.Replace(".", " "));
            smtp.SendTo = "kcarlisle@fragomen.com,failure@fragomen.com";
            smtp.Subject = string.Format("Application Issue ({0})", aTitle);

            if (jid == null) smtp.Message = string.Format("There was a problem with the application console.  The excetpion details should be listed below:{1}{1}{2}", Utils.FdblFormats.CrLf, Utils.FdblExceptions.GetDetails(ex));
            else smtp.Message = string.Format("There was a problem with the application console (JobId: {0}).  The excetpion details should be listed below:{1}{1}{2}", jid, Utils.FdblFormats.CrLf, Utils.FdblExceptions.GetDetails(ex));

            return smtp;

        }

        public static void SendEmail(Exception ex) {

            try {


                FdblSmtp.Send(FdblGenericEmail.GetSmtpRecord(null, ex));

            } catch { }

        }

        public static void SendEmail(string jid, Exception ex) {

            try {

                if (jid == null || jid.Trim().Length == 0) jid = "xx";

                FdblSmtp.Send(FdblGenericEmail.GetSmtpRecord(jid, ex));

            } catch { }

        }

        #endregion

        #region Constructors

        private FdblGenericEmail() { }

        #endregion

    }

}
