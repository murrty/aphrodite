using System;
using System.IO;

namespace aphrodite {

    public enum DownloadType : int {
        None = -1,
        Tags = 0,
        Pools = 1,
        Images = 2
    }

    public static class JsonDownloadInfo {
        public static readonly string postJsonBase = "https://e621.net/posts/{0}.json";
    }

    public class TagDownloadInfo {
        public bool Valid;

        public string Tags;
        public bool FromUrl;
        public string PageUrl;
        public string PageNumber;

        public string[] Graylist;
        public string[] Blacklist;
        public string UndesiredTags;

        public string DownloadPath;
        public bool SaveBlacklistedFiles;
        public bool IgnoreFinish;
        public bool SaveInfo;

        public bool UseMinimumScore;
        public bool MinimumScoreAsTag;
        public int MinimumScore;
        public int ImageLimit;
        public int PageLimit;
        public bool SaveExplicit;
        public bool SaveQuestionable;
        public bool SaveSafe;
        public bool SeparateRatings;
        public bool SeparateNonImages;
        public bool SkipExistingFiles;
        public string FileNameSchema;

        /// <summary>
        /// Initializes new TagDownloadInfo for downloading specified tags.
        /// </summary>
        /// <param name="InputTags"></param>
        public TagDownloadInfo(string InputTags) {
            if (InputTags == null) {
                Valid = false;
                return;
            }

            Tags = InputTags;
            PageUrl = null;
            FromUrl = false;

            DownloadPath = Environment.CurrentDirectory;

            if (Config.Settings.General.saveLocation != string.Empty) {
                DownloadPath = Config.Settings.General.saveLocation;
            }

            Graylist = Config.Settings.General.blacklist.Split(' ');
            Blacklist = Config.Settings.General.zeroToleranceBlacklist.Split(' ');
            UndesiredTags = Config.Settings.General.undesiredTags;

            SaveBlacklistedFiles = Config.Settings.General.saveBlacklisted;
            IgnoreFinish = Config.Settings.General.ignoreFinish;
            SaveInfo = Config.Settings.General.saveInfo;

            UseMinimumScore = aphrodite.Config.Settings.Tags.enableScoreMin;
            if (UseMinimumScore) {
                MinimumScoreAsTag = aphrodite.Config.Settings.Tags.scoreAsTag;
                MinimumScore = aphrodite.Config.Settings.Tags.scoreMin;
            }

            if (aphrodite.Config.Settings.Tags.imageLimit > 0) {
                ImageLimit = aphrodite.Config.Settings.Tags.imageLimit;
            }

            if (PageLimit > 0) {
                PageLimit = aphrodite.Config.Settings.Tags.pageLimit;
            }

            SeparateRatings = aphrodite.Config.Settings.Tags.separateRatings;
            SeparateNonImages = aphrodite.Config.Settings.Tags.separateNonImages;
            SaveExplicit = aphrodite.Config.Settings.Tags.Explicit;
            SaveQuestionable = aphrodite.Config.Settings.Tags.Questionable;
            SaveSafe = aphrodite.Config.Settings.Tags.Safe;

            FileNameSchema = apiTools.ReplaceIllegalCharacters(aphrodite.Config.Settings.Tags.fileNameSchema.ToLower());
        }
        /// <summary>
        /// Initializes new TagDownloadInfo for downloading a page.
        /// </summary>
        /// <param name="InputPage"></param>
        /// <param name="IsPage"></param>
        public TagDownloadInfo(bool IsPage, string InputPage) {
            if (IsPage && InputPage == null || !apiTools.IsValidPageLink(InputPage)) {
                Valid = false;
                return;
            }

            FromUrl = IsPage;
            PageUrl = InputPage;

            if (InputPage.IndexOf("?tags=") > -1) {
                Tags = InputPage.Split('?')[1].Split('&')[0].Substring(5);
            }
            else if (InputPage.IndexOf("&tags=") > -1) {
                Tags = InputPage.Substring(InputPage.IndexOf("&tags=")).Split('&')[0].Substring(6);
            }
            else {
                Tags = "No tags";
            }

            if (InputPage.IndexOf("?page=") > -1) {
                PageNumber = InputPage.Split('?')[1].Split('&')[0].Substring(5);
            }
            else if (InputPage.IndexOf("&page=") > -1) {
                PageNumber = InputPage.Substring(InputPage.IndexOf("&page=")).Split('&')[0].Substring(6);
            }
            else {
                PageNumber = "1";
            }

            DownloadPath = Environment.CurrentDirectory;

            if (Config.Settings.General.saveLocation != string.Empty) {
                DownloadPath = Config.Settings.General.saveLocation;
            }

            Graylist = Config.Settings.General.blacklist.Split(' ');
            Blacklist = Config.Settings.General.zeroToleranceBlacklist.Split(' ');
            UndesiredTags = Config.Settings.General.undesiredTags;

            SaveBlacklistedFiles = Config.Settings.General.saveBlacklisted;
            IgnoreFinish = Config.Settings.General.ignoreFinish;
            SaveInfo = Config.Settings.General.saveInfo;

            UseMinimumScore = aphrodite.Config.Settings.Tags.enableScoreMin;
            if (UseMinimumScore) {
                MinimumScoreAsTag = aphrodite.Config.Settings.Tags.scoreAsTag;
                MinimumScore = aphrodite.Config.Settings.Tags.scoreMin;
            }

            if (aphrodite.Config.Settings.Tags.imageLimit > 0) {
                ImageLimit = aphrodite.Config.Settings.Tags.imageLimit;
            }

            if (PageLimit > 0) {
                PageLimit = aphrodite.Config.Settings.Tags.pageLimit;
            }

            SeparateRatings = aphrodite.Config.Settings.Tags.separateRatings;
            SeparateNonImages = aphrodite.Config.Settings.Tags.separateNonImages;
            SaveExplicit = aphrodite.Config.Settings.Tags.Explicit;
            SaveQuestionable = aphrodite.Config.Settings.Tags.Questionable;
            SaveSafe = aphrodite.Config.Settings.Tags.Safe;

            FileNameSchema = apiTools.ReplaceIllegalCharacters(aphrodite.Config.Settings.Tags.fileNameSchema.ToLower());
        }
    }
    public class PoolDownloadInfo {
        public bool Valid;

        public string PoolId;

        public string[] Graylist;
        public string[] Blacklist;
        public string UndesiredTags;

        public string DownloadPath;
        public bool SaveInfo;
        public bool IgnoreFinish;
        public bool SaveBlacklistedFiles;

        public bool MergeBlacklisted;
        public bool OpenAfter;
        public string FileNameSchema;

        public PoolDownloadInfo(string RequestedPool) {
            if (RequestedPool == null) {
                Valid = false;
                return;
            }

            PoolId = RequestedPool;
            DownloadPath = Environment.CurrentDirectory;

            if (Config.Settings.General.saveLocation != string.Empty) {
                DownloadPath = Config.Settings.General.saveLocation;
            }

            Graylist = Config.Settings.General.blacklist.Split(' ');
            Blacklist = Config.Settings.General.zeroToleranceBlacklist.Split(' ');
            UndesiredTags = Config.Settings.General.undesiredTags;

            SaveInfo = Config.Settings.General.saveInfo;
            IgnoreFinish = Config.Settings.General.ignoreFinish;
            SaveBlacklistedFiles = Config.Settings.General.saveBlacklisted;

            MergeBlacklisted = Config.Settings.Pools.mergeBlacklisted;
            OpenAfter = Config.Settings.Pools.openAfter;
            FileNameSchema = apiTools.ReplaceIllegalCharacters(Config.Settings.Pools.fileNameSchema);
        }
    }
    public class ImageDownloadInfo {
        public bool Valid;
        public bool UseForm;

        public string ImageUrl;
        public string PostId;

        public string[] Graylist;
        public string[] Blacklist;
        public string UndesiredTags;

        public string DownloadPath;
        public bool SaveInfo;
        public bool IgnoreFinish;

        public bool SeparateRatings;
        public bool SeparateBlacklisted;
        public bool SeparateNonImages;
        public bool SeparateArtists;
        public string FileNameSchema;

        public ImageDownloadInfo(string Image) {
            if (Image == null) {
                Valid = false;
                return;
            }

            DownloadPath = Environment.CurrentDirectory;

            if (Config.Settings.General.saveLocation != string.Empty) {
                DownloadPath = Config.Settings.General.saveLocation;
            }

            Graylist = Config.Settings.General.blacklist.Split(' ');
            Blacklist = Config.Settings.General.zeroToleranceBlacklist.Split(' ');
            UndesiredTags = Config.Settings.General.undesiredTags;

            SaveInfo = Config.Settings.General.saveInfo;
            IgnoreFinish = Config.Settings.General.ignoreFinish;

            FileNameSchema = apiTools.ReplaceIllegalCharacters(Config.Settings.Images.fileNameSchema);
            SeparateRatings = Config.Settings.Images.separateRatings;
            SeparateBlacklisted = Config.Settings.Images.separateBlacklisted;
            SeparateNonImages = Config.Settings.Images.separateNonImages;
            SeparateArtists = Config.Settings.Images.separateArtists;

            UseForm = Config.Settings.Images.useForm;
        }
    }

    class Downloader {
        /// <summary>
        /// Uses the program's internal settings for settings
        /// </summary>
        public static class Arguments {
            public static bool DownloadTags(string Tags) {
                try {
                    frmTagDownloader tagDL = new frmTagDownloader();
                    tagDL.DownloadInfo = new TagDownloadInfo(Tags);
                    tagDL.ShowDialog();
                    tagDL.Dispose();
                    return true;
                }
                catch {
                    throw;
                }
            }
            public static bool DownloadPage(string PageUrl) {
                try {
                    frmTagDownloader tagDL = new frmTagDownloader();
                    tagDL.DownloadInfo = new TagDownloadInfo(true, PageUrl);
                    tagDL.ShowDialog();
                    tagDL.Dispose();
                    return true;
                }
                catch {
                    throw;
                }
            }
            public static bool DownloadPool(string PoolId) {
                try {
                    frmPoolDownloader PoolDL = new frmPoolDownloader();
                    PoolDL.DownloadInfo = new PoolDownloadInfo(PoolId);
                    PoolDL.ShowDialog();
                    PoolDL.Dispose();
                    return true;
                }
                catch {
                    throw;
                }
            }
            public static bool DownloadImage(string ImageId) {
            try {
                ImageDownloadInfo NewInfo = new ImageDownloadInfo(ImageId);
                if (NewInfo.UseForm) {
                    frmImageDownloader imageDL = new frmImageDownloader();
                    imageDL.DownloadInfo = NewInfo;
                    imageDL.ShowDialog();
                    imageDL.Dispose();
                }
                else {
                    ImageDownloader imageDL = new ImageDownloader(NewInfo);
                }
                return true;
            }
            catch {
                throw;
            }
        }
        }
    }
}
