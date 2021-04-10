using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aphrodite {

    public enum DownloadStatus {
        Waiting,
        ReadyToDownload,
        Finished,
        Errored,
        Aborted,
        FormWasDisposed,
        FileAlreadyExists,
        NothingToDownload,
        PostOrPoolWasDeleted,
        ApiReturnedNullOrEmpty,
        FileWasNullAfterBypassingBlacklist,
    }

    public enum DownloadType : int {
        None = -1,
        Tags = 0,
        Pools = 1,
        Images = 2
    }

    public enum ConfigType : int {
        None = -1,
        All = 0,
        Initialization = 1,
        FormSettings = 2,
        General = 3,
        Tags = 4,
        Pools = 5,
        Images = 6
    }

}
