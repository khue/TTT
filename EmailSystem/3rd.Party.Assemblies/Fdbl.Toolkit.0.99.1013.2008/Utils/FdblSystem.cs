using System;
using System.Diagnostics;
using System.IO;

namespace Fdbl.Toolkit.Utils {

    #region Enumerations

    [FlagsAttribute]
    public enum FdblLaunchType : byte {
        NoWait,
        WaitForExit,
        WaitForIdle
    }

    #endregion

    public class FdblSystem {

        #region Methods - Public (Static)

        public static bool ApplicationAlreadyExecuting(string appName) {

            if (appName == null) throw new ArgumentNullException("application name is null");

            if (appName.Trim().Length == 0) throw new ArgumentException("application name is blank");

            string fLocation = appName;
            string sName = appName.ToLower();

            Process[] apps = Process.GetProcessesByName(sName);

            if (apps.Length == 0) {

                sName = Path.GetFileNameWithoutExtension(sName);
                apps = Process.GetProcessesByName(sName);

            }

            if (apps.Length == 0) return false;

            int cId = Process.GetCurrentProcess().Id;
            bool found = false;

            for (int ndx = 0; ndx < apps.Length; ndx++) {

                //if (cId != apps[ndx].Id && apps[ndx].MainModule.FileName == fLocation) {
                if (cId != apps[ndx].Id) {

                    found = true;
                    break;

                }

            }

            return found;
        }

        public static bool ApplicationExceeds(string appName, int appMaxCount) {

            if (appName == null) throw new ArgumentNullException("application name is null");

            if (appName.Trim().Length == 0) throw new ArgumentException("application name is blank");

            //return GetApplicationCount(appName) >= appMaxCount;
            return GetApplicationCount(appName) > appMaxCount;

        }

        public static int GetApplicationCount(string appName) {

            if (appName == null) throw new ArgumentNullException("application name is null");

            if (appName.Trim().Length == 0) throw new ArgumentException("application name is blank");

            appName = appName.ToLower();

            if (appName.EndsWith(".exe") || appName.EndsWith(".com") || appName.EndsWith(".bat")) appName = Path.GetFileNameWithoutExtension(appName);

            int cnt = 0;

            try {

                Process[] p = Process.GetProcessesByName(appName);
                if (p != null) cnt = p.Length;

            } catch { }

            return cnt;

        }

        public static string GetExecutingPath() {

            return FdblFormats.ForOS(Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location));

        }

        public static bool IsApplicationRunning(int appHandle) {

            if (appHandle < 1) throw new ArgumentOutOfRangeException("application handle is out of range");

            Process p = null;

            try {

                p = Process.GetProcessById(appHandle);

            } catch {

                return false;

            }

            if (p == null) return false;

            return true;

        }

        public static int Launch(string app, string args, FdblLaunchType launchType, int waitTime, bool hasWindow) {

            if (app == null) throw new ArgumentNullException("application name is null");

            if (app.Trim().Length == 0) throw new ArgumentException("application name is blank");

            if (args == null) args = string.Empty;

            Process myProcess = new Process();

            myProcess.StartInfo.FileName = app;
            myProcess.StartInfo.Arguments = args;
            if (!hasWindow) {
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }

            myProcess.Start();

            switch (launchType) {

                case FdblLaunchType.WaitForExit:
                    switch (waitTime < 1) {
                        case true:
                            myProcess.WaitForExit();
                            break;
                        case false:
                            myProcess.WaitForExit(waitTime);
                            break;
                    }
                    break;

                case FdblLaunchType.WaitForIdle:
                    switch (waitTime < 1) {
                        case true:
                            myProcess.WaitForInputIdle();
                            break;
                        case false:
                            myProcess.WaitForInputIdle(waitTime);
                            break;
                    }
                    break;

            }

            return myProcess.HasExited ? myProcess.ExitCode : -1;

        }

        #endregion

        #region Contructors

        private FdblSystem() { }

        #endregion

    }

}