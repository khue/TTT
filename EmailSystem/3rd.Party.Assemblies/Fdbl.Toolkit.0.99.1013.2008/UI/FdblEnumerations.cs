using System;

namespace Fdbl.Toolkit.UI {

    #region Enumerations

    [FlagsAttribute]
    public enum FdblDialogPosition : byte {
        CenterScreen,
        CenterParent,
        DefaultLocation
    }

    [FlagsAttribute]
    public enum FdblDialogResult : byte {
        Success,
        Failure,
        Cancelled,
        None
    }

    [FlagsAttribute]
    public enum FdblPasswordEncoding : byte {
        CRC,
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512,
        Plain
    }

    #endregion

}