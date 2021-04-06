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

            switch (Program.UseIni) {
                case true:
                    if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg"))) {
                        Graylist = File.ReadAllLines(Environment.CurrentDirectory + "\\graylist.cfg");
                    }
                    else {
                        Graylist = new string[] { };
                    }
                    if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg")) {
                        Blacklist = File.ReadAllLines(Environment.CurrentDirectory + "\\blacklist.cfg");
                    }
                    else {
                        Blacklist = new string[] { };
                    }
                    if (Program.Ini.KeyExists("UndesiredTags", "Global")) {
                        UndesiredTags = Program.Ini.ReadString("UndesiredTags", "Global");
                    }
                    else {
                        UndesiredTags = string.Empty;
                    }

                    if (Program.Ini.KeyExists("saveBlacklisted", "Global")) {
                        SaveBlacklistedFiles = Program.Ini.ReadBool("saveBlacklisted", "Global");
                    }
                    else {
                        SaveBlacklistedFiles = true;
                    }
                    if (Program.Ini.KeyExists("saveInfo", "Global")) {
                        SaveInfo = Program.Ini.ReadBool("saveInfo", "Global");
                    }
                    else {
                        SaveInfo = true;
                    }
                    if (Program.Ini.KeyExists("ignoreFinish", "Global")) {
                        IgnoreFinish = Program.Ini.ReadBool("ignoreFinish", "Global");
                    }
                    else {
                        IgnoreFinish = false;
                    }


                    if (Program.Ini.KeyExists("useMinimumScore", "Tags")) {
                        UseMinimumScore = Program.Ini.ReadBool("useMinimumScore", "Tags");
                    }
                    else {
                        UseMinimumScore = false;
                    }
                    if (UseMinimumScore) {
                        MinimumScoreAsTag = Program.Ini.ReadBool("scoreAsTag", "Tags");
                        MinimumScore = Program.Ini.ReadInt("scoreMin", "Tags");
                    }
                    if (Program.Ini.KeyExists("imageLimit", "Tags")) {
                        if (Program.Ini.ReadInt("imageLimit", "Tags") > 0) {
                            ImageLimit = Program.Ini.ReadInt("imageLimit", "Tags");
                        }
                        else {
                            ImageLimit = 0;
                        }
                    }
                    if (Program.Ini.KeyExists("pageLimit", "Tags")) {
                        if (Program.Ini.ReadInt("pageLimit", "Tags") > 0) {
                            PageLimit = Program.Ini.ReadInt("pageLimit", "Tags");
                        }
                    }
                    if (Program.Ini.KeyExists("separateRatings", "Tags")) {
                        SeparateRatings = Program.Ini.ReadBool("separateRatings", "Tags");
                    }
                    else {
                        SeparateRatings = true;
                    }
                    if (Program.Ini.KeyExists("separateNonImages", "Tags")) {
                        SeparateNonImages = Program.Ini.ReadBool("separateNonImages", "Tags");
                    }
                    else {
                        SeparateNonImages = true;
                    }
                    if (Program.Ini.KeyExists("Explicit", "Tags")) {
                        SaveExplicit = Program.Ini.ReadBool("Explicit", "Tags");
                    }
                    else {
                        SaveExplicit = true;
                    }
                    if (Program.Ini.KeyExists("Questionable", "Tags")) {
                        SaveQuestionable = Program.Ini.ReadBool("Questionable", "Tags");
                    }
                    else {
                        SaveQuestionable = true;
                    }
                    if (Program.Ini.KeyExists("Safe", "Tags")) {
                        SaveSafe = Program.Ini.ReadBool("Safe", "Tags");
                    }
                    else {
                        SaveSafe = true;
                    }
                    if (Program.Ini.KeyExists("fileNameSchema", "Tags")) {
                        FileNameSchema = apiTools.ReplaceIllegalCharacters(Program.Ini.ReadString("fileNameSchema", "Tags").ToLower());
                    }
                    else {
                        FileNameSchema = "%md5%";
                    }
                    break;

                default:
                    if (General.Default.saveLocation != string.Empty) {
                        DownloadPath = General.Default.saveLocation;
                    }

                    Graylist = General.Default.blacklist.Split(' ');
                    Blacklist = General.Default.zeroToleranceBlacklist.Split(' ');
                    UndesiredTags = General.Default.undesiredTags;

                    SaveBlacklistedFiles = General.Default.saveBlacklisted;
                    IgnoreFinish = General.Default.ignoreFinish;
                    SaveInfo = General.Default.saveInfo;

                    UseMinimumScore = aphrodite.Tags.Default.enableScoreMin;
                    if (UseMinimumScore) {
                        MinimumScoreAsTag = aphrodite.Tags.Default.scoreAsTag;
                        MinimumScore = aphrodite.Tags.Default.scoreMin;
                    }

                    if (aphrodite.Tags.Default.imageLimit > 0) {
                        ImageLimit = aphrodite.Tags.Default.imageLimit;
                    }

                    if (PageLimit > 0) {
                        PageLimit = aphrodite.Tags.Default.pageLimit;
                    }

                    SeparateRatings = aphrodite.Tags.Default.separateRatings;
                    SeparateNonImages = aphrodite.Tags.Default.separateNonImages;
                    SaveExplicit = aphrodite.Tags.Default.Explicit;
                    SaveQuestionable = aphrodite.Tags.Default.Questionable;
                    SaveSafe = aphrodite.Tags.Default.Safe;

                    FileNameSchema = apiTools.ReplaceIllegalCharacters(aphrodite.Tags.Default.fileNameSchema.ToLower());
                    break;
            }
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

            switch (Program.UseIni) {
                case true:
                    if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg"))) {
                        Graylist = File.ReadAllLines(Environment.CurrentDirectory + "\\graylist.cfg");
                    }
                    else {
                        Graylist = new string[] { };
                    }
                    if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg")) {
                        Blacklist = File.ReadAllLines(Environment.CurrentDirectory + "\\blacklist.cfg");
                    }
                    else {
                        Blacklist = new string[] { };
                    }
                    if (Program.Ini.KeyExists("UndesiredTags", "Global")) {
                        UndesiredTags = Program.Ini.ReadString("UndesiredTags", "Global");
                    }
                    else {
                        UndesiredTags = string.Empty;
                    }

                    if (Program.Ini.KeyExists("saveBlacklisted", "Global")) {
                        SaveBlacklistedFiles = Program.Ini.ReadBool("saveBlacklisted", "Global");
                    }
                    else {
                        SaveBlacklistedFiles = true;
                    }
                    if (Program.Ini.KeyExists("saveInfo", "Global")) {
                        SaveInfo = Program.Ini.ReadBool("saveInfo", "Global");
                    }
                    else {
                        SaveInfo = true;
                    }
                    if (Program.Ini.KeyExists("ignoreFinish", "Global")) {
                        IgnoreFinish = Program.Ini.ReadBool("ignoreFinish", "Global");
                    }
                    else {
                        IgnoreFinish = false;
                    }

                    if (Program.Ini.KeyExists("useMinimumScore", "Tags")) {
                        UseMinimumScore = Program.Ini.ReadBool("useMinimumScore", "Tags");
                    }
                    else {
                        UseMinimumScore = false;
                    }
                    if (UseMinimumScore) {
                        MinimumScoreAsTag = Program.Ini.ReadBool("scoreAsTag", "Tags");
                        MinimumScore = Program.Ini.ReadInt("scoreMin", "Tags");
                    }
                    if (Program.Ini.KeyExists("imageLimit", "Tags")) {
                        if (Program.Ini.ReadInt("imageLimit", "Tags") > 0) {
                            ImageLimit = Program.Ini.ReadInt("imageLimit", "Tags");
                        }
                        else {
                            ImageLimit = 0;
                        }
                    }
                    if (Program.Ini.KeyExists("pageLimit", "Tags")) {
                        if (Program.Ini.ReadInt("pageLimit", "Tags") > 0) {
                            PageLimit = Program.Ini.ReadInt("pageLimit", "Tags");
                        }
                    }
                    if (Program.Ini.KeyExists("separateRatings", "Tags")) {
                        SeparateRatings = Program.Ini.ReadBool("separateRatings", "Tags");
                    }
                    else {
                        SeparateRatings = true;
                    }
                    if (Program.Ini.KeyExists("separateNonImages", "Tags")) {
                        SeparateNonImages = Program.Ini.ReadBool("separateNonImages", "Tags");
                    }
                    else {
                        SeparateNonImages = true;
                    }
                    if (Program.Ini.KeyExists("Explicit", "Tags")) {
                        SaveExplicit = Program.Ini.ReadBool("Explicit", "Tags");
                    }
                    else {
                        SaveExplicit = true;
                    }
                    if (Program.Ini.KeyExists("Questionable", "Tags")) {
                        SaveQuestionable = Program.Ini.ReadBool("Questionable", "Tags");
                    }
                    else {
                        SaveQuestionable = true;
                    }
                    if (Program.Ini.KeyExists("Safe", "Tags")) {
                        SaveSafe = Program.Ini.ReadBool("Safe", "Tags");
                    }
                    else {
                        SaveSafe = true;
                    }
                    if (Program.Ini.KeyExists("fileNameSchema", "Tags")) {
                        FileNameSchema = apiTools.ReplaceIllegalCharacters(Program.Ini.ReadString("fileNameSchema", "Tags").ToLower());
                    }
                    else {
                        FileNameSchema = "%md5%";
                    }
                    break;

                default:
                    if (General.Default.saveLocation != string.Empty) {
                        DownloadPath = General.Default.saveLocation;
                    }

                    Graylist = General.Default.blacklist.Split(' ');
                    Blacklist = General.Default.zeroToleranceBlacklist.Split(' ');
                    UndesiredTags = General.Default.undesiredTags;

                    SaveBlacklistedFiles = General.Default.saveBlacklisted;
                    IgnoreFinish = General.Default.ignoreFinish;
                    SaveInfo = General.Default.saveInfo;

                    UseMinimumScore = aphrodite.Tags.Default.enableScoreMin;
                    if (UseMinimumScore) {
                        MinimumScoreAsTag = aphrodite.Tags.Default.scoreAsTag;
                        MinimumScore = aphrodite.Tags.Default.scoreMin;
                    }

                    if (aphrodite.Tags.Default.imageLimit > 0) {
                        ImageLimit = aphrodite.Tags.Default.imageLimit;
                    }

                    if (PageLimit > 0) {
                        PageLimit = aphrodite.Tags.Default.pageLimit;
                    }

                    SeparateRatings = aphrodite.Tags.Default.separateRatings;
                    SeparateNonImages = aphrodite.Tags.Default.separateNonImages;
                    SaveExplicit = aphrodite.Tags.Default.Explicit;
                    SaveQuestionable = aphrodite.Tags.Default.Questionable;
                    SaveSafe = aphrodite.Tags.Default.Safe;

                    FileNameSchema = apiTools.ReplaceIllegalCharacters(aphrodite.Tags.Default.fileNameSchema.ToLower());
                    break;
            }
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

            switch (Program.UseIni) {
                case true:
                    if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg"))) {
                        Graylist = File.ReadAllLines(Environment.CurrentDirectory + "\\graylist.cfg");
                    }
                    else {
                        Graylist = new string[] { };
                    }
                    if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg")) {
                        Blacklist = File.ReadAllLines(Environment.CurrentDirectory + "\\blacklist.cfg");
                    }
                    else {
                        Blacklist = new string[] { };
                    }
                    if (Program.Ini.KeyExists("UndesiredTags", "Global")) {
                        UndesiredTags = Program.Ini.ReadString("UndesiredTags", "Global");
                    }
                    else {
                        UndesiredTags = string.Empty;
                    }

                    if (Program.Ini.KeyExists("saveInfo", "Global")) {
                        SaveInfo = Program.Ini.ReadBool("saveInfo", "Global");
                    }
                    else {
                        SaveInfo = true;
                    }
                    if (Program.Ini.KeyExists("ignoreFinish", "Global")) {
                        IgnoreFinish = Program.Ini.ReadBool("ignoreFinish", "Global");
                    }
                    else {
                        IgnoreFinish = false;
                    }
                    if (Program.Ini.KeyExists("saveBlacklisted", "Global")) {
                        SaveBlacklistedFiles = Program.Ini.ReadBool("saveBlacklisted", "Global");
                    }
                    else {
                        SaveBlacklistedFiles = true;
                    }

                    if (Program.Ini.KeyExists("mergeBlacklisted", "Pools")) {
                        MergeBlacklisted = Program.Ini.ReadBool("mergeBlacklisted", "Pools");
                    }
                    else {
                        MergeBlacklisted = true;
                    }
                    if (Program.Ini.KeyExists("openAfter", "Pools")) {
                        OpenAfter = Program.Ini.ReadBool("openAfter", "Pools");
                    }
                    else {
                        OpenAfter = false;
                    }
                    if (Program.Ini.KeyExists("fileNameSchema", "Pools")) {
                        FileNameSchema = apiTools.ReplaceIllegalCharacters(Program.Ini.ReadString("fileNameSchema", "Pools").ToLower());
                    }
                    else {
                        FileNameSchema = Properties.Settings.Default.Properties["fileNameSchema"].DefaultValue as string;
                    }
                    break;

                default:
                    if (General.Default.saveLocation != string.Empty) {
                        DownloadPath = General.Default.saveLocation;
                    }

                    Graylist = General.Default.blacklist.Split(' ');
                    Blacklist = General.Default.zeroToleranceBlacklist.Split(' ');
                    UndesiredTags = General.Default.undesiredTags;

                    SaveInfo = General.Default.saveInfo;
                    IgnoreFinish = General.Default.ignoreFinish;
                    SaveBlacklistedFiles = General.Default.saveBlacklisted;

                    MergeBlacklisted = Pools.Default.mergeBlacklisted;
                    OpenAfter = Pools.Default.openAfter;
                    FileNameSchema = apiTools.ReplaceIllegalCharacters(Pools.Default.fileNameSchema);
                    break;
            }
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

            switch (Program.UseIni) {
                case true:
                    if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg"))) {
                        Graylist = File.ReadAllLines(Environment.CurrentDirectory + "\\graylist.cfg");
                    }
                    else {
                        Graylist = new string[] { };
                    }

                    if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg")) {
                        Blacklist = File.ReadAllLines(Environment.CurrentDirectory + "\\blacklist.cfg");
                    }
                    else {
                        Blacklist = new string[] { };
                    }
                    if (Program.Ini.KeyExists("UndesiredTags", "Global")) {
                        UndesiredTags = Program.Ini.ReadString("UndesiredTags", "Global");
                    }
                    else {
                        UndesiredTags = string.Empty;
                    }


                    if (Program.Ini.KeyExists("saveInfo", "Global")) {
                        SaveInfo = Program.Ini.ReadBool("saveInfo", "Global");
                    }
                    else {
                        SaveInfo = true;
                    }

                    if (Program.Ini.KeyExists("ignoreFinish", "Global")) {
                        IgnoreFinish = Program.Ini.ReadBool("ignoreFinish", "Global");
                    }
                    else {
                        IgnoreFinish = false;
                    }


                    if (Program.Ini.KeyExists("fileNameSchema", "Images")) {
                        FileNameSchema = apiTools.ReplaceIllegalCharacters(Program.Ini.ReadString("fileNameSchema", "Images").ToLower());
                    }
                    else {
                        FileNameSchema = "%artist%_%md5%";
                    }

                    if (Program.Ini.KeyExists("separateRatings", "Images")) {
                        SeparateRatings = Program.Ini.ReadBool("separateRatings", "Images");
                    }
                    else {
                        SeparateRatings = true;
                    }

                    if (Program.Ini.KeyExists("separateBlacklisted", "Images")) {
                        SeparateBlacklisted = Program.Ini.ReadBool("separateBlacklisted", "Images");
                    }
                    else {
                        SeparateBlacklisted = true;
                    }

                    if (Program.Ini.KeyExists("separateNonImages", "Images")) {
                        SeparateNonImages = Program.Ini.ReadBool("separateNonImages", "Images");
                    }
                    else {
                        SeparateNonImages = false;
                    }

                    if (Program.Ini.KeyExists("separateArtists", "Images")) {
                        SeparateArtists = Program.Ini.ReadBool("separateArtists", "Images");
                    }
                    else {
                        SeparateArtists = false;
                    }

                    if (Program.Ini.KeyExists("useForm", "Images")) {
                        if (Program.Ini.ReadBool("useForm", "Images")) {
                            UseForm = true;
                        }
                    }
                    break;

                default:
                    if (General.Default.saveLocation != string.Empty) {
                        DownloadPath = General.Default.saveLocation;
                    }

                    Graylist = General.Default.blacklist.Split(' ');
                    Blacklist = General.Default.zeroToleranceBlacklist.Split(' ');
                    UndesiredTags = General.Default.undesiredTags;

                    SaveInfo = General.Default.saveInfo;
                    IgnoreFinish = General.Default.ignoreFinish;

                    FileNameSchema = apiTools.ReplaceIllegalCharacters(Images.Default.fileNameSchema);
                    SeparateRatings = Images.Default.separateRatings;
                    SeparateBlacklisted = Images.Default.separateBlacklisted;
                    SeparateNonImages = Images.Default.separateNonImages;
                    SeparateArtists = Images.Default.separateArtists;

                    UseForm = Images.Default.useForm;
                    break;
            }
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
                    ImageDownloader imageDL = new ImageDownloader();
                    imageDL.DownloadInfo = NewInfo;
                    imageDL.downloadImage();
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
