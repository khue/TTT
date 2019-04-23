using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Fdbl.Toolkit.Utils {

    public class FdblConsole {

        #region Constants

        protected const int StandardInputHandle = -10;
        protected const int StandardOutputHandle = -11;
        protected const int StandardErrorHandle = -12;

        protected const int EnableLineInput = 0x0002;
        protected const int EnableEchoInput = 0x0004;

        #endregion

        #region Structures

        [StructLayout(LayoutKind.Sequential)]
        protected struct ConsoleCursorInfo {
            internal int Size;
            internal bool Visible;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct ConsoleScreenBufferInfo {
            internal Coord Size;
            internal Coord CursorPosition;
            internal short Attribute;
            internal Rect Window;
            internal Coord MaxSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct Coord {
            internal short x;
            internal short y;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct Rect {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }

        #endregion

        #region Win32 APIs

        [DllImport("msvcrt")]
        protected static extern Int32 _getch();

        [DllImport("msvcrt")]
        protected static extern Int32 _kbhit();

        [DllImport("kernel32")]
        protected static extern bool FillConsoleOutputCharacter(IntPtr hConsoleOutput, short cCharacter, int nLength, Coord WriteCoord, out int lpNumberOfCharsWritten);

        //[DllImport("kernel32")]
        //protected extern static bool GetConsoleCursorInfo(IntPtr Handle, out ConsoleCursorInfo lpConsoleCursorInfo);

        //[DllImport("kernel32")]
        //protected static extern bool GetConsoleCursorPosition(IntPtr hConsoleOutput, out Coord lpConsoleCursorInfo);

        //[DllImport("kernel32")]
        //protected static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);

        [DllImport("kernel32")]
        protected static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out ConsoleScreenBufferInfo lpConsoleScreenBufferInfo);

        [DllImport("kernel32")]
        protected static extern IntPtr GetStdHandle(int nStdHandle);

        //[DllImport("kernel32")]
        //protected extern static bool SetConsoleCursorInfo(IntPtr Handle, ref ConsoleCursorInfo lpConsoleCursorInfo);

        [DllImport("kernel32")]
        protected static extern bool SetConsoleCursorPosition(IntPtr hConsoleOutput, Coord lpConsoleCursorInfo);

        //[DllImport("kernel32")]
        //protected static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

        #endregion

        #region Members

        static readonly object ClassLock = new object();

        #endregion

        #region Methods - Public (Static)

        public static void ClearCurrentConsoleLine() {

            lock (ClassLock) {

                ConsoleScreenBufferInfo csbi;
                Coord rowStart;
                int wr;
                IntPtr screen = GetStdHandle(StandardOutputHandle);

                GetConsoleScreenBufferInfo(screen, out csbi);

                rowStart.x = 0;
                rowStart.y = csbi.CursorPosition.y;

                FillConsoleOutputCharacter(screen, (short)' ', csbi.CursorPosition.x, rowStart, out wr);
                SetConsoleCursorPosition(screen, rowStart);

            }

        }

        public static string GetDTStamp() {

            return GetDTStamp(FdblFormats.DateFormat, FdblFormats.TimeFormat12);

        }

        public static string GetDTStamp(string dateFormat, string timeFormat) {

            return DateTime.Now.ToString(string.Format("{0} {1}  ", dateFormat, timeFormat));

        }

        public static int GetKey() {

            int key = -1;

            if (_kbhit() != 0) {

                key = _getch();

                if (key == 0 && _kbhit() != 0)
                    key = 1000 + _getch(); //F1 to F10 keys
                else if (key == 224 && _kbhit() != 0)
                    key = 2000 + _getch(); //Cursor-keys, F11 and F12

            }

            if (key == 13) return -2;

            if ((key >= 65 && key <= 90) || (key >= 97 && key <= 122) || (key >= 48 && key <= 57) || key == 8 || key == 27 || key == 32) return key;

            return -1;

        }

        public static void WriteException(Exception ex) {

            if (ex == null) return;

            lock (ClassLock) {
                Console.Write(string.Format("{0}{1}{0}", FdblFormats.CrLf, FdblExceptions.GetDetails(ex)));
            }

        }

        public static void WriteInitialization(Assembly assembly) {

            if (assembly == null) return;

            Version version = assembly.GetName().Version;

            string softwareTitle = ((AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute))).Title;
            string softwareCompany = ((AssemblyCompanyAttribute)AssemblyCompanyAttribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute))).Company;
            string releaseCopyright = ((AssemblyCopyrightAttribute)AssemblyCopyrightAttribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute))).Copyright;
            string releaseVersion = string.Format("{0}.{1} (build {2}.{3})", version.Major, version.Minor, version.Revision, version.Build);
            string releaseDate = File.GetCreationTime(assembly.Location).ToString("D");

            lock (ClassLock) {

                Console.WriteLine(string.Format("{0}", softwareTitle));
                Console.WriteLine(string.Format("Release {0}, {1}", releaseVersion, releaseDate));
                Console.WriteLine(string.Format("Copyright c {0} {1}. All Rights Reserved.", releaseCopyright, softwareCompany));
                Console.WriteLine(string.Empty);
                Console.WriteLine(string.Format("{0}Initializing console", GetDTStamp()));

            }

        }

        public static void WriteMessage(string message) {

            WriteMessage(message, true, true);

        }

        public static void WriteMessage(string message, bool includeDTStamp) {

            WriteMessage(message, includeDTStamp, true);

        }

        public static void WriteMessage(string message, bool includeDTStamp, bool appendNewLine) {

            lock (ClassLock) {

                string dtStamp = includeDTStamp ? GetDTStamp() : string.Empty;
                string conLine = string.Format("{0}{1}", dtStamp, message);

                if (appendNewLine) Console.WriteLine(conLine);
                else Console.Write(conLine);

            }

        }

        #endregion

        #region Constructors

        private FdblConsole() { }

        #endregion

    }

}