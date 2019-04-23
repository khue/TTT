using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace Fdbl.Toolkit.Network {

    #region Win32 Networking Enumerations

    [Flags]
    public enum NetworkShareTypes {
        Disk = 0,
        Printer = 1,
        Device = 2,
        IPC = 3,
        Special = -2147483648
    }

    #endregion

    public class FdblNetwork {

        #region Win32 Networking API

        [StructLayout(LayoutKind.Sequential)]
        private struct NetResource {
            public int Scope;
            public int Type;
            public int DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct UNIVERSAL_NAME_INFO {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string UniversalName;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SHARE_INFO_1 {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string NetName;
            public NetworkShareTypes ShareType;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Remark;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SHARE_INFO_2 {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string NetName;
            public NetworkShareTypes ShareType;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Remark;
            public int Permissions;
            public int MaxUsers;
            public int CurrentUsers;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Path;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Password;
        }

        private const int NO_ERROR = 0;

        private const int CONNECT_UPDATE_PROFILE = 0x00000001;
        private const int CONNECT_INTERACTIVE = 0x00000008;
        private const int CONNECT_PROMPT = 0x00000010;
        private const int CONNECT_REDIRECT = 0x00000080;
        private const int CONNECT_COMMANDLINE = 0x00000800;
        private const int CONNECT_CMD_SAVECRED = 0x00001000;

        private const int RESOURCETYPE_ANY = 0x0;
        private const int RESOURCETYPE_DISK = 0x1;
        private const int RESOURCETYPE_PRINT = 0x2;

        private const int RESOURCE_CONNECTED = 0x1;
        private const int RESOURCE_GLOBALNET = 0x2;
        private const int RESOURCE_REMEMBERED = 0x3;

        private const int RESOURCEDISPLAYTYPE_GENERIC = 0x0;
        private const int RESOURCEDISPLAYTYPE_DOMAIN = 0x1;
        private const int RESOURCEDISPLAYTYPE_SERVER = 0x2;
        private const int RESOURCEDISPLAYTYPE_SHARE = 0x3;

        private const int RESOURCEUSAGE_CONNECTABLE = 0x1;
        private const int RESOURCEUSAGE_CONTAINER = 0x2;

        private const int ERROR_ACCESS_DENIED = 5;
        private const int ERROR_ALREADY_ASSIGNED = 85;
        private const int ERROR_BAD_DEV_TYPE = 66;
        private const int ERROR_BAD_DEVICE = 1200;
        private const int ERROR_BAD_NET_NAME = 67;
        private const int ERROR_BAD_PROFILE = 1206;
        private const int ERROR_BAD_PROVIDER = 1204;
        private const int ERROR_BUSY = 170;
        private const int ERROR_CANCELLED = 1223;
        private const int ERROR_CANNOT_OPEN_PROFILE = 1205;
        private const int ERROR_DEVICE_ALREADY_REMEMBERED = 1202;
        private const int ERROR_EXTENDED_ERROR = 1208;
        private const int ERROR_INVALID_PASSWORD = 86;
        private const int ERROR_MORE_DATA = 234;
        private const int ERROR_NO_NET_OR_BAD_PATH = 1203;
        private const int ERROR_NOT_CONNECTED = 2250;
        private const int ERROR_WRONG_LEVEL = 124;

        private const int MAX_PATH = 260;
        private const int MAX_SI50_ENTRIES = 20;

        private const int UNIVERSAL_NAME_INFO_LEVEL = 1;


        [DllImport("mpr")]
        private static extern int WNetAddConnection2A(ref NetResource NetRes, string Password, string Username, int Flags);

        [DllImport("mpr")]
        private static extern int WNetCancelConnection2A(string Name, int Flags, int Force);

        [DllImport("mpr")]
        private static extern int WNetRestoreConnectionW(int HWnd, string LocalDrive);

        [DllImport("mpr")]
        private static extern int WNetGetUniversalName(string LocalPath, int InfoLevel, ref UNIVERSAL_NAME_INFO Buffer, ref int BufferSize);

        [DllImport("mpr")]
        private static extern int WNetGetUniversalName(string LocalPath, int InfoLevel, IntPtr Buffer, ref int BufferSize);

        [DllImport("netapi32")]
        private static extern int NetShareEnum(string ServerName, int Level, out IntPtr Buffer, int PrefMaxLen, out int EntriesRead, out int TotalEntries, ref int hResume);

        [DllImport("netapi32")]
        private static extern int NetApiBufferFree(IntPtr Buffer);

        #endregion

        #region Methods - Public (Static)

        public static void Connect(FdblNetworkDrive netDrive) {

            if (netDrive == null) throw new ArgumentNullException("network drive is null");

            if (netDrive.RemotePath == null || netDrive.RemotePath.Trim().Length == 0) throw new ArgumentException("network drive is invalid");

            NetResource NetRes = new NetResource();

            NetRes.Scope = RESOURCE_GLOBALNET;
            NetRes.Type = RESOURCETYPE_DISK;
            NetRes.DisplayType = RESOURCEDISPLAYTYPE_SHARE;
            NetRes.Usage = RESOURCEUSAGE_CONNECTABLE;
            NetRes.RemoteName = netDrive.ToString();
            NetRes.LocalName = netDrive.LocalDrive;

            int Flags = 0;

            if (netDrive.SaveCredentials) Flags += CONNECT_CMD_SAVECRED;
            if (netDrive.PersistentConnection) Flags += CONNECT_UPDATE_PROFILE;
            if (netDrive.PromptForCredentials) Flags += (CONNECT_INTERACTIVE + CONNECT_PROMPT);

            if (netDrive.ForceConnection) {
                try {
                    Disconnect(netDrive);
                } catch { }
            }

            int ret = WNetAddConnection2A(ref NetRes, netDrive.AccountPassword, netDrive.UserAccount, Flags);

            if (ret > 0) throw new System.ComponentModel.Win32Exception(ret);

        }

        public static void Disconnect(FdblNetworkDrive netDrive) {

            if (netDrive == null) throw new ArgumentNullException("network drive is null");

            int Flags = 0;

            if (netDrive.PersistentConnection) Flags += CONNECT_UPDATE_PROFILE;

            int ret = WNetCancelConnection2A(netDrive.GetDisconnectDrive(), Flags, Convert.ToInt32(netDrive.ForceDisconnection));

            if (ret > 0) throw new System.ComponentModel.Win32Exception(ret);

        }

        public static ArrayList GetShares(string RemoteServer) {
            ArrayList shares = new ArrayList();
            IntPtr pBuffer = IntPtr.Zero;
            int level = 2;
            int entriesRead = 0;
            int totalEntries = 0;
            int nRet = 0;
            int hResume = 0;

            try {

                nRet = NetShareEnum(RemoteServer, level, out pBuffer, -1, out entriesRead, out totalEntries, ref hResume);

                //Need admin for level 2, drop to level 1
                if (nRet == ERROR_ACCESS_DENIED) {

                    level = 1;
                    nRet = NetShareEnum(RemoteServer, level, out pBuffer, -1, out entriesRead, out totalEntries, ref hResume);

                }

                if (nRet == NO_ERROR && entriesRead > 0) {

                    Type t = (level == 2) ? typeof(SHARE_INFO_2) : typeof(SHARE_INFO_1);
                    int offset = Marshal.SizeOf(t);

                    for (int ndx = 0, lpItem = pBuffer.ToInt32(); ndx < entriesRead; ndx++, lpItem += offset) {

                        IntPtr pItem = new IntPtr(lpItem);

                        if (level == 1) {

                            SHARE_INFO_1 si = (SHARE_INFO_1)Marshal.PtrToStructure(pItem, t);
                            shares.Add(new FdblNetworkShare(RemoteServer, si.NetName, string.Empty, si.Remark, si.ShareType));

                        } else {

                            SHARE_INFO_2 si = (SHARE_INFO_2)Marshal.PtrToStructure(pItem, t);
                            shares.Add(new FdblNetworkShare(RemoteServer, si.NetName, si.Path, si.Remark, si.ShareType));

                        }

                    }

                } else if (nRet > 0) throw new System.ComponentModel.Win32Exception(nRet);

                return shares;

            } finally {

                // Clean up buffer allocated by system
                if (IntPtr.Zero != pBuffer) NetApiBufferFree(pBuffer);

            }
        }

        public static bool IsValidFilePath(string fileName) {

            if (fileName == null) return false;
            if (fileName.Trim().Length == 0) return false;

            char drive = char.ToUpper(fileName[0]);

            if (drive < 'A' || drive > 'Z') return false;
            if (fileName[1] != Path.VolumeSeparatorChar) return false;
            if (fileName[2] != Path.DirectorySeparatorChar) return false;

            return true;

        }

        public static string PathToUNC(string fileName) {

            if (fileName == null) throw new ArgumentNullException("filename is null");

            if (fileName.Trim().Length == 0) throw new ArgumentException("filename is blank");

            fileName = Path.GetFullPath(fileName);

            if (!IsValidFilePath(fileName)) throw new IOException("invalid file path");

            int nRet = 0;
            UNIVERSAL_NAME_INFO rni = new UNIVERSAL_NAME_INFO();
            int bufferSize = Marshal.SizeOf(rni);

            nRet = WNetGetUniversalName(fileName, UNIVERSAL_NAME_INFO_LEVEL, ref rni, ref bufferSize);

            if (nRet == ERROR_MORE_DATA) {

                IntPtr pBuffer = Marshal.AllocHGlobal(bufferSize);

                try {

                    nRet = WNetGetUniversalName(fileName, UNIVERSAL_NAME_INFO_LEVEL, pBuffer, ref bufferSize);
                    if (nRet == NO_ERROR) rni = (UNIVERSAL_NAME_INFO)Marshal.PtrToStructure(pBuffer, typeof(UNIVERSAL_NAME_INFO));

                } finally {

                    Marshal.FreeHGlobal(pBuffer);

                }

            }

            if (nRet == NO_ERROR) return rni.UniversalName;

            return fileName;

        }

        public static void Restore(string LocalDrive) {

            int ret = WNetRestoreConnectionW(0, LocalDrive);

            if (ret > 0) throw new System.ComponentModel.Win32Exception(ret);

        }

        public static void RestoreAll() {
            Restore(null);
        }

        #endregion

        #region Contructors

        private FdblNetwork() { }

        #endregion

    }

}