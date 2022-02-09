namespace aphrodite {
    /// <summary>
    /// What the pushed arguments requested. Only deals with one type per runtime.
    /// </summary>
    public enum ArgumentType {
        /// <summary>
        /// No arguments were passed.
        /// </summary>
        NoArguments,

        /// <summary>
        /// The arguments form was cancelled, so the program will exit.
        /// </summary>
        CancelledArgumentsDownload,

        /// <summary>
        /// Updates the "aphrodite:" protocol.
        /// </summary>
        UpdateProtocol,

        /// <summary>
        /// Downloads tags with ArgumentData.
        /// </summary>
        DownloadTags,
        /// <summary>
        /// Downloads page with ArgumentData.
        /// </summary>
        DownloadPage,
        /// <summary>
        /// Downloads pool with ArgumentData.
        /// </summary>
        DownloadPools,
        /// <summary>
        /// Downloads image with ArgumentData.
        /// </summary>
        DownloadImages,
        /// <summary>
        /// Downloads furrybooru with ArgumentData.
        /// </summary>
        DownloadFurryBooru,
        /// <summary>
        /// Downloads inkbunny with ArgumentData.
        /// </summary>
        DownloadInkBunny,
        /// <summary>
        /// Downloads imgur album with ArgumentData.
        /// </summary>
        DownloadImgur,

        /// <summary>
        /// Pushes the tags in ArgumentData to the main form.
        /// </summary>
        PushTags,
        /// <summary>
        /// Pushes the pool ArgumentData to the main form.
        /// </summary>
        PushPools,
        /// <summary>
        /// Pushes the image in ArgumentData to the main form.
        /// </summary>
        PushImages,
        /// <summary>
        /// Pushes the argument in ArgumentData to the furrybooru form.
        /// </summary>
        PushFurryBooru,
        /// <summary>
        /// Pushes the argument in ArgumentData to the inkbunny form.
        /// </summary>
        PushInkBunny,
        /// <summary>
        /// Pushes the argument in ArgumentData to the imgur form.
        /// </summary>
        PushImgur,

        /// <summary>
        /// Adds the pool in ArgumentData to the pool wishlist.
        /// </summary>
        AddToPoolWishlist,

        /// <summary>
        /// Shows the Redownloader form.
        /// </summary>
        ShowRedownloader,
        /// <summary>
        /// Shows the Blacklist form.
        /// </summary>
        ShowBlacklist,
        /// <summary>
        /// Shows the Pool Wishlist form.
        /// </summary>
        ShowPoolWishlist,
        /// <summary>
        /// Shows the FurryBooru form.
        /// </summary>
        ShowFurryBooru,
        /// <summary>
        /// Shows the InkBunny form.
        /// </summary>
        ShowInkBunny,
        /// <summary>
        /// Shows the Imgur form.
        /// </summary>
        ShowImgur,

        /// <summary>
        /// Configure program settings.
        /// </summary>
        ConfigureSettings,
        /// <summary>
        /// Configure program settings, starts in the Tags tab.
        /// </summary>
        ConfigureTagsSettings,
        /// <summary>
        /// Configure program settings, starts in the Pools tab.
        /// </summary>
        ConfigurePoolsSettings,
        /// <summary>
        /// Configure program settings, starts in the Images tab.
        /// </summary>
        ConfigureImagesSettings,
        /// <summary>
        /// Configure the program settings, starts in the Misc tab.
        /// </summary>
        ConfigureMiscSettings,
        /// <summary>
        /// Configure program settings, starts in the Protocol tab.
        /// </summary>
        ConfigureProtocolSettings,
        /// <summary>
        /// Configure program settings, starts in the Schema tab.
        /// </summary>
        ConfigureSchemaSettings,
        /// <summary>
        /// Configure the program settings, starts in the Import/Export tab.
        /// </summary>
        ConfigureImportExportSettings,
        /// <summary>
        /// Configure program settings, starts in the Portable tab.
        /// </summary>
        ConfigurePortableSettings,
    }

    /// <summary>
    /// The status of the current download.
    /// </summary>
    public enum DownloadStatus {
        /// <summary>
        /// The download thread is waiting to start.
        /// </summary>
        Waiting,
        /// <summary>
        /// The download is being parsed.
        /// </summary>
        Parsing,
        /// <summary>
        /// The download is ready to download, pre-download logic should be processing with this status.
        /// </summary>
        ReadyToDownload,
        /// <summary>
        /// The download is currently processing.
        /// </summary>
        Downloading,
        /// <summary>
        /// The download has finished.
        /// </summary>
        Finished,
        /// <summary>
        /// The download encountered an error.
        /// </summary>
        Errored,
        /// <summary>
        /// The download was aborted.
        /// </summary>
        Aborted,
        /// <summary>
        /// The download client was forbidden from accessing the content.
        /// </summary>
        Forbidden,
        /// <summary>
        /// The download is already being downloaded in another form.
        /// </summary>
        AlreadyBeingDownloaded,
        /// <summary>
        /// The download form was disposed and cannot be accessed.
        /// </summary>
        FormWasDisposed,
        /// <summary>
        /// The file has already been downloaded and cannot be redownloaded.
        /// </summary>
        FileAlreadyExists,
        /// <summary>
        /// All available files have already been downloaded.
        /// </summary>
        NothingToDownload,
        /// <summary>
        /// The post or pool was deleted, and cannot be downloaded.
        /// </summary>
        PostOrPoolWasDeleted,
        /// <summary>
        /// The api returned null or empty, and cannot be parsed.
        /// </summary>
        ApiReturnedNullOrEmpty,
        /// <summary>
        /// The image url was still null after attempting to bypass the blacklist.
        /// </summary>
        FileWasNullAfterBypassingBlacklist,
    }

    /// <summary>
    /// Enumeration of what download type is requested.
    /// </summary>
    public enum DownloadType {
        /// <summary>
        /// No type was selected.
        /// </summary>
        None,
        /// <summary>
        /// Tags was selected.
        /// </summary>
        Tags,
        /// <summary>
        /// Page was selected.
        /// </summary>
        Page,
        /// <summary>
        /// Pools was selected.
        /// </summary>
        Pools,
        /// <summary>
        /// Images was selected.
        /// </summary>
        Images,
        /// <summary>
        /// FurryBooru was selected.
        /// </summary>
        FurryBooru,
        /// <summary>
        /// InkBunny was selected.
        /// </summary>
        InkBunny,
        /// <summary>
        /// InkBunny Keywords was selected.
        /// </summary>
        InkBunnyKeywords,
        /// <summary>
        /// InkBunny Artists' Gallery was selected.
        /// </summary>
        InkBunnyArtistGallery,
        /// <summary>
        /// InkBunny Users' Favorites was selected.
        /// </summary>
        InkBunnyUsersFavorites,
        /// <summary>
        /// Imgur was selected.
        /// </summary>
        Imgur
    }

    /// <summary>
    /// What site was requested to be downloaded from.
    /// </summary>
    public enum DownloadSite {
        /// <summary>
        /// No site.
        /// </summary>
        None,
        /// <summary>
        /// e621.net
        /// </summary>
        e621,
        /// <summary>
        /// furry.booru.org
        /// </summary>
        FurryBooru,
        /// <summary>
        /// imgur.com
        /// </summary>
        Imgur,
        /// <summary>
        /// inkbunny.net
        /// </summary>
        InkBunny
    }
}
