using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aphrodite {
    class Downloader {
        static IniFile ini = new IniFile();

        /// <summary>
        /// Uses method-defined settings for most of everything (if applicable)
        /// </summary>
        public static class MainForm {
            public static void downloadTags(string tags, int pageLimit, bool useScoreLimit, bool scoreLimitAsTag, int scoreLimit, int imageLimit, string[] ratings, bool separateRatings, bool useIni = false) {
                frmTagDownloader tagDL = new frmTagDownloader();

                tagDL.webHeader = Program.UserAgent;
                tagDL.openAfter = false;
                if (useIni) {
                    tagDL.saveTo = Environment.CurrentDirectory;
                    tagDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                    tagDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                    tagDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                    tagDL.saveBlacklistedFiles = ini.ReadBool("saveBlacklisted", "Global");
                    tagDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                    tagDL.fileNameSchema = ini.ReadString("fileNameSchema", "Tags");
                }
                else {
                    General.Default.Reload();
                    if (General.Default.saveLocation != string.Empty)
                        tagDL.saveTo = General.Default.saveLocation;
                    else
                        tagDL.saveTo = Environment.CurrentDirectory;
                    tagDL.graylist = General.Default.blacklist;
                    tagDL.blacklist = General.Default.zeroToleranceBlacklist;
                    tagDL.saveInfo = General.Default.saveInfo;
                    tagDL.saveBlacklistedFiles = General.Default.saveBlacklisted;
                    tagDL.ignoreFinish = General.Default.ignoreFinish;
                    tagDL.fileNameSchema = Tags.Default.fileNameSchema;
                }

                tagDL.tags = tags;
                tagDL.useMinimumScore = useScoreLimit;
                if (useScoreLimit) {
                    tagDL.scoreAsTag = scoreLimitAsTag;
                    tagDL.minimumScore = scoreLimit;
                }
                tagDL.imageLimit = imageLimit;
                tagDL.pageLimit = pageLimit;
                tagDL.separateRatings = separateRatings;
                tagDL.ratings = ratings;

                tagDL.Show();
            }
            public static void downloadPool(string poolId, bool openAfterDownload, bool mergeBlacklisted, bool useIni = false) {
                frmPoolDownloader PoolDl = new frmPoolDownloader();
                PoolDl.poolID = poolId;
                PoolDl.header = Program.UserAgent;

                if (useIni) {
                    PoolDl.saveTo = Environment.CurrentDirectory;
                    PoolDl.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                    PoolDl.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");

                    PoolDl.saveInfo = ini.ReadBool("saveInfo", "Global");
                    PoolDl.saveBlacklisted = ini.ReadBool("saveBlacklisted", "Global");
                    PoolDl.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");

                    PoolDl.saveInfo = ini.ReadBool("saveMetadata", "Global");
                    PoolDl.saveBlacklisted = ini.ReadBool("saveArtistMetadata", "Global");
                    PoolDl.ignoreFinish = ini.ReadBool("saveTagMetadata", "Global");

                    PoolDl.fileNameSchema = ini.ReadString("fileNameSchema", "Pools");
                }
                else {
                    General.Default.Reload();
                    if (General.Default.saveLocation != string.Empty)
                        PoolDl.saveTo = General.Default.saveLocation;
                    else
                        PoolDl.saveTo = Environment.CurrentDirectory;
                    PoolDl.graylist = General.Default.blacklist;
                    PoolDl.blacklist = General.Default.zeroToleranceBlacklist;

                    PoolDl.saveInfo = General.Default.saveInfo;
                    PoolDl.saveBlacklisted = General.Default.saveBlacklisted;
                    PoolDl.ignoreFinish = General.Default.ignoreFinish;

                    PoolDl.saveMetadata = General.Default.saveMetadata;
                    PoolDl.saveArtistMetadata = General.Default.saveArtistMetadata;
                    PoolDl.saveTagMetadata = General.Default.saveTagMetadata;

                    PoolDl.fileNameSchema = Pools.Default.fileNameSchema;
                }

                PoolDl.mergeBlacklisted = mergeBlacklisted;
                PoolDl.openAfter = openAfterDownload;

                PoolDl.Show();
            }
            public static void downloadImage(string imageId, bool separateRatings, bool separateBlacklisted, bool separateArtists, bool useForm, bool useIni = false) {
                if (useForm) {
                    frmImageDownloader imgDl = new frmImageDownloader();
                    imgDl.header = Program.UserAgent;

                    if (apiTools.IsValidImageLink(imageId)) {
                        imgDl.url = imageId;
                    }
                    else {
                        imgDl.postID = imageId;
                    }

                    imgDl.separateRatings = separateRatings;
                    imgDl.separateBlacklisted = separateBlacklisted;
                    imgDl.separateArtists = separateArtists;

                    if (useIni) {
                        imgDl.saveTo = Environment.CurrentDirectory;
                        imgDl.staticSaveTo = Environment.CurrentDirectory;

                        if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg")))
                            imgDl.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                        else
                            imgDl.graylist = string.Empty;

                        if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg"))
                            imgDl.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                        else
                            imgDl.blacklist = string.Empty;


                        if (ini.KeyExists("saveInfo", "Global"))
                            imgDl.saveInfo = ini.ReadBool("saveInfo", "Global");
                        else
                            imgDl.saveInfo = true;

                        if (ini.KeyExists("ignoreFinish", "Global"))
                            imgDl.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                        else
                            imgDl.ignoreFinish = false;


                        if (ini.KeyExists("fileNameSchema", "Images"))
                            imgDl.fileNameSchema = apiTools.ReplaceIllegalCharacters(ini.ReadString("fileNameSchema", "Images").ToLower());
                        else
                            imgDl.fileNameSchema = "%artist%_%md5%";

                    }
                    else {
                        if (General.Default.saveLocation != string.Empty)
                            imgDl.saveTo = General.Default.saveLocation;
                        else
                            imgDl.saveTo = Environment.CurrentDirectory;
                        imgDl.staticSaveTo = General.Default.saveLocation;
                        imgDl.graylist = General.Default.blacklist;
                        imgDl.blacklist = General.Default.zeroToleranceBlacklist;
                        imgDl.saveInfo = General.Default.saveInfo;
                        imgDl.ignoreFinish = General.Default.ignoreFinish;
                        imgDl.fileNameSchema = Images.Default.fileNameSchema;
                    }

                    imgDl.Show();
                }
                else {
                    ImageDownloader imgDl = new ImageDownloader();
                    imgDl.header = Program.UserAgent;

                    if (apiTools.IsValidImageLink(imageId)) {
                        imgDl.url = imageId;
                    }
                    else {
                        imgDl.postID = imageId;
                    }

                    imgDl.separateRatings = separateRatings;
                    imgDl.separateBlacklisted = separateBlacklisted;
                    imgDl.separateArtists = separateArtists;

                    if (useIni) {
                        imgDl.saveTo = Environment.CurrentDirectory;
                        imgDl.staticSaveTo = Environment.CurrentDirectory;

                        if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg")))
                            imgDl.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                        else
                            imgDl.graylist = string.Empty;

                        if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg"))
                            imgDl.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                        else
                            imgDl.blacklist = string.Empty;


                        if (ini.KeyExists("saveInfo", "Global"))
                            imgDl.saveInfo = ini.ReadBool("saveInfo", "Global");
                        else
                            imgDl.saveInfo = true;

                        if (ini.KeyExists("ignoreFinish", "Global"))
                            imgDl.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                        else
                            imgDl.ignoreFinish = false;


                        if (ini.KeyExists("fileNameSchema", "Images"))
                            imgDl.fileNameSchema = apiTools.ReplaceIllegalCharacters(ini.ReadString("fileNameSchema", "Images").ToLower());
                        else
                            imgDl.fileNameSchema = "%artist%_%md5%";

                    }
                    else {
                        imgDl.saveTo = General.Default.saveLocation;
                        imgDl.staticSaveTo = General.Default.saveLocation;
                        imgDl.graylist = General.Default.blacklist;
                        imgDl.blacklist = General.Default.zeroToleranceBlacklist;
                        imgDl.saveInfo = General.Default.saveInfo;
                        imgDl.ignoreFinish = General.Default.ignoreFinish;
                        imgDl.fileNameSchema = Images.Default.fileNameSchema;
                    }

                    imgDl.downloadImage();
                }
            }
        }

        /// <summary>
        /// Uses the program's internal settings for settings
        /// </summary>
        public static class Arguments {
            public static bool downloadPool(string ID, bool useIni = false) {
                try {
                    frmPoolDownloader poolDL = new frmPoolDownloader();
                    poolDL.header = Program.UserAgent;
                    poolDL.poolID = ID;

                    if (useIni) {
                        poolDL.saveTo = Environment.CurrentDirectory;

                        if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg")))
                            poolDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                        else
                            poolDL.graylist = string.Empty;

                        if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg"))
                            poolDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                        else
                            poolDL.blacklist = string.Empty;


                        if (ini.KeyExists("saveInfo", "Global"))
                            poolDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                        else
                            poolDL.saveInfo = true;

                        if (ini.KeyExists("openAfter", "Global"))
                            poolDL.openAfter = false; //ini.ReadBool("openAfter", "Global");
                        else
                            poolDL.openAfter = false;

                        if (ini.KeyExists("saveBlacklisted", "Global"))
                            poolDL.saveBlacklisted = ini.ReadBool("saveBlacklisted", "Global");
                        else
                            poolDL.saveBlacklisted = true;

                        if (ini.KeyExists("ignoreFinish", "Global"))
                            poolDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                        else
                            poolDL.ignoreFinish = false;


                        //if (ini.KeyExists("saveMetadata", "Global"))
                        //    poolDL.saveMetadata = ini.ReadBool("saveMetadata", "Global");
                        //else
                        //    poolDL.saveMetadata = false;

                        //if (ini.KeyExists("saveArtistMetadata", "Global"))
                        //    poolDL.saveArtistMetadata = ini.ReadBool("saveArtistMetadata", "Global");
                        //else
                        //    poolDL.saveArtistMetadata = false;

                        //if (ini.KeyExists("saveTagMetadata", "Global"))
                        //    poolDL.saveTagMetadata = ini.ReadBool("saveTagMetadata", "Global");
                        //else
                        //    poolDL.saveTagMetadata = false;


                        if (ini.KeyExists("fileNameSchema", "Pools"))
                            poolDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(ini.ReadString("fileNameSchema", "Pools").ToLower());
                        else
                            poolDL.fileNameSchema = "%poolname%_%page%";

                        if (ini.KeyExists("mergeBlacklisted", "Pools"))
                            poolDL.mergeBlacklisted = ini.ReadBool("mergeBlacklisted", "Pools");
                        else
                            poolDL.mergeBlacklisted = true;

                        if (ini.KeyExists("openAfter", "Pools"))
                            poolDL.openAfter = ini.ReadBool("openAfter", "Pools");
                        else
                            poolDL.openAfter = false;
                    }
                    else {
                        General.Default.Reload();
                        Pools.Default.Reload();

                        if (General.Default.saveLocation != string.Empty)
                            poolDL.saveTo = General.Default.saveLocation;
                        else
                            poolDL.saveTo = Environment.CurrentDirectory;
                        poolDL.graylist = General.Default.blacklist;
                        poolDL.blacklist = General.Default.zeroToleranceBlacklist;
                        //poolDL.saveMetadata = Settings.Default.saveMetadata;
                        //poolDL.saveArtistMetadata = Settings.Default.saveArtistMetadata;
                        //poolDL.saveTagMetadata = Settings.Default.saveTagMetadata;

                        poolDL.saveInfo = General.Default.saveInfo;
                        poolDL.ignoreFinish = General.Default.ignoreFinish;
                        poolDL.saveBlacklisted = General.Default.saveBlacklisted;

                        poolDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(Pools.Default.fileNameSchema);
                        poolDL.mergeBlacklisted = Pools.Default.mergeBlacklisted;
                        poolDL.openAfter = Pools.Default.openAfter;
                    }

                    poolDL.Show();

                    return true;
                }
                catch {
                    return false;
                }
            }
            public static bool downloadTags(string tags, bool useIni = false) {
                try {
                    frmTagDownloader tagDL = new frmTagDownloader();
                    tagDL.webHeader = Program.UserAgent;
                    tagDL.tags = tags;
                    string ratings = string.Empty;

                    if (useIni) {
                        tagDL.saveTo = Environment.CurrentDirectory;

                        if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg")))
                            tagDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                        else
                            tagDL.graylist = string.Empty;

                        if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg"))
                            tagDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                        else
                            tagDL.blacklist = string.Empty;


                        if (ini.KeyExists("saveInfo", "Global"))
                            tagDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                        else
                            tagDL.saveInfo = true;

                        if (ini.KeyExists("openAfter", "Global"))
                            tagDL.openAfter = false; //ini.ReadBool("openAfter", "Global");
                        else
                            tagDL.openAfter = false;

                        if (ini.KeyExists("saveBlacklisted", "Global"))
                            tagDL.saveBlacklistedFiles = ini.ReadBool("saveBlacklisted", "Global");
                        else
                            tagDL.saveBlacklistedFiles = true;

                        if (ini.KeyExists("ignoreFinish", "Global"))
                            tagDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                        else
                            tagDL.ignoreFinish = false;


                        if (ini.KeyExists("useMinimumScore", "Tags"))
                            tagDL.useMinimumScore = ini.ReadBool("useMinimumScore", "Tags");
                        else
                            tagDL.useMinimumScore = false;

                        if (tagDL.useMinimumScore) {
                            tagDL.scoreAsTag = ini.ReadBool("scoreAsTag", "Tags");
                            tagDL.minimumScore = ini.ReadInt("scoreMin", "Tags");
                        }

                        if (ini.KeyExists("imageLimit", "Tags"))
                            if (ini.ReadInt("imageLimit", "Tags") > 0)
                                tagDL.imageLimit = ini.ReadInt("imageLimit", "Tags");
                            else
                                tagDL.imageLimit = 0;

                        if (ini.KeyExists("pageLimit", "Tags"))
                            if (ini.ReadInt("pageLimit", "Tags") > 0)
                                tagDL.pageLimit = ini.ReadInt("pageLimit", "Tags");

                        if (ini.KeyExists("separateRatings", "Tags"))
                            tagDL.separateRatings = ini.ReadBool("separateRatings", "Tags");
                        else
                            tagDL.separateRatings = true;

                        if (ini.KeyExists("Explicit", "Tags"))
                            if (ini.ReadBool("Explicit", "Tags"))
                                ratings += "e ";
                        if (ini.KeyExists("Questionable", "Tags"))
                            if (ini.ReadBool("Questionable", "Tags"))
                                ratings += "q ";
                        if (ini.KeyExists("Safe", "Tags"))
                            if (ini.ReadBool("Safe", "Tags"))
                                ratings += "s";
                        ratings = ratings.TrimEnd(' ');

                        if (tagDL.separateRatings)
                            tagDL.ratings = ratings.Split(' ');

                        if (ini.KeyExists("fileNameSchema", "Tags"))
                            tagDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(ini.ReadString("fileNameSchema", "Tags").ToLower());
                        else
                            tagDL.fileNameSchema = "%md5%";
                    }
                    else {
                        General.Default.Reload();
                        Tags.Default.Reload();

                        if (General.Default.saveLocation != string.Empty)
                            tagDL.saveTo = General.Default.saveLocation;
                        else
                            tagDL.saveTo = Environment.CurrentDirectory;
                        tagDL.graylist = General.Default.blacklist;
                        tagDL.blacklist = General.Default.zeroToleranceBlacklist;

                        tagDL.saveInfo = General.Default.saveInfo;
                        tagDL.openAfter = false;
                        tagDL.saveBlacklistedFiles = General.Default.saveBlacklisted;
                        tagDL.ignoreFinish = General.Default.ignoreFinish;

                        tagDL.useMinimumScore = Tags.Default.enableScoreMin;
                        if (tagDL.useMinimumScore) {
                            tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                            tagDL.minimumScore = Tags.Default.scoreMin;
                        }

                        if (Tags.Default.imageLimit > 0)
                            tagDL.imageLimit = Tags.Default.imageLimit;

                        if (tagDL.pageLimit > 0)
                            tagDL.pageLimit = Tags.Default.pageLimit;

                        tagDL.separateRatings = Tags.Default.separateRatings;

                        if (Tags.Default.Explicit)
                            ratings += "e ";
                        if (Tags.Default.Questionable)
                            ratings += "q ";
                        if (Tags.Default.Safe)
                            ratings += "s";
                        ratings = ratings.TrimEnd(' ');
                        tagDL.ratings = ratings.Split(' ');

                        tagDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(Tags.Default.fileNameSchema.ToLower());
                    }

                    tagDL.Show();

                    return true;
                }
                catch {
                    return false;
                }
            }
            public static bool downloadPage(string url, bool useIni = false) {
                try {
                    frmTagDownloader tagDL = new frmTagDownloader();
                    tagDL.webHeader = Program.UserAgent;
                    tagDL.fromURL = true;
                    tagDL.downloadUrl = url;
                    string ratings = string.Empty;

                    if (useIni) {
                        tagDL.saveTo = Environment.CurrentDirectory;
                        if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg")))
                            tagDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                        else
                            tagDL.graylist = string.Empty;

                        if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg"))
                            tagDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                        else
                            tagDL.blacklist = string.Empty;


                        if (ini.KeyExists("saveInfo", "Global"))
                            tagDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                        else
                            tagDL.saveInfo = true;

                        if (ini.KeyExists("openAfter", "Global"))
                            tagDL.openAfter = false;
                        else
                            tagDL.openAfter = false;

                        if (ini.KeyExists("saveBlacklisted", "Global"))
                            tagDL.saveBlacklistedFiles = ini.ReadBool("saveBlacklisted", "Global");
                        else
                            tagDL.saveBlacklistedFiles = true;

                        if (ini.KeyExists("ignoreFinish", "Global"))
                            tagDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                        else
                            tagDL.ignoreFinish = false;


                        if (ini.KeyExists("useMinimumScore", "Tags"))
                            tagDL.useMinimumScore = ini.ReadBool("useMinimumScore", "Tags");
                        else
                            tagDL.useMinimumScore = false;

                        if (tagDL.useMinimumScore) {
                            tagDL.scoreAsTag = ini.ReadBool("scoreAsTag", "Tags");
                            tagDL.minimumScore = ini.ReadInt("scoreMin", "Tags");
                        }

                        if (ini.KeyExists("imageLimit", "Tags"))
                            if (ini.ReadInt("imageLimit", "Tags") > 0)
                                tagDL.imageLimit = ini.ReadInt("imageLimit", "Tags");
                            else
                                tagDL.imageLimit = 0;

                        if (ini.KeyExists("pageLimit", "Tags"))
                            if (ini.ReadInt("pageLimit", "Tags") > 0)
                                tagDL.pageLimit = ini.ReadInt("pageLimit", "Tags");

                        if (ini.KeyExists("separateRatings", "Tags"))
                            tagDL.separateRatings = ini.ReadBool("separateRatings", "Tags");
                        else
                            tagDL.separateRatings = true;

                        if (ini.KeyExists("Explicit", "Tags"))
                            if (ini.ReadBool("Explicit", "Tags"))
                                ratings += "e ";
                        if (ini.KeyExists("Questionable", "Tags"))
                            if (ini.ReadBool("Questionable", "Tags"))
                                ratings += "q ";
                        if (ini.KeyExists("Safe", "Tags"))
                            if (ini.ReadBool("Safe", "Tags"))
                                ratings += "s";
                        ratings = ratings.TrimEnd(' ');

                        if (tagDL.separateRatings)
                            tagDL.ratings = ratings.Split(' ');

                        if (ini.KeyExists("fileNameSchema", "Tags"))
                            tagDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(ini.ReadString("fileNameSchema", "Tags").ToLower());
                        else
                            tagDL.fileNameSchema = "%md5%";
                    }
                    else {
                        General.Default.Reload();
                        Tags.Default.Reload();
                        if (General.Default.saveLocation != string.Empty)
                            tagDL.saveTo = General.Default.saveLocation;
                        else
                            tagDL.saveTo = Environment.CurrentDirectory;
                        tagDL.graylist = General.Default.blacklist;
                        tagDL.blacklist = General.Default.zeroToleranceBlacklist;

                        tagDL.saveInfo = General.Default.saveInfo;
                        tagDL.openAfter = false;
                        tagDL.saveBlacklistedFiles = General.Default.saveBlacklisted;
                        tagDL.ignoreFinish = General.Default.ignoreFinish;

                        tagDL.useMinimumScore = Tags.Default.enableScoreMin;
                        if (tagDL.useMinimumScore) {
                            tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                            tagDL.minimumScore = Tags.Default.scoreMin;
                        }

                        if (Tags.Default.imageLimit > 0)
                            tagDL.imageLimit = Tags.Default.imageLimit;

                        if (tagDL.pageLimit > 0)
                            tagDL.pageLimit = Tags.Default.pageLimit;

                        tagDL.separateRatings = Tags.Default.separateRatings;

                        if (Tags.Default.Explicit)
                            ratings += "e ";
                        if (Tags.Default.Questionable)
                            ratings += "q ";
                        if (Tags.Default.Safe)
                            ratings += "s";
                        ratings = ratings.TrimEnd(' ');
                        tagDL.ratings = ratings.Split(' ');

                        tagDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(Tags.Default.fileNameSchema.ToLower());
                    }

                    tagDL.Show();

                    return true;
                }
                catch {
                    return false;
                }
            }
            public static bool downloadImage(string id, bool useIni = false) {
            if (apiTools.IsValidImageLink(id)) {
                id = apiTools.GetPostIdFromUrl(id);
            }

            try {
                bool useForm = false;
                if (useIni)
                    if (ini.KeyExists("useForm", "Images")) {
                        if (ini.ReadBool("useForm", "Images")){
                            useForm = true;
                        }
                    }
                else
                     useForm = Images.Default.useForm;

                if (useForm) {
                    frmImageDownloader imageDL = new frmImageDownloader();
                    imageDL.postID = id;

                    if (useIni) {
                        imageDL.saveTo = Environment.CurrentDirectory;

                        if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg")))
                            imageDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                        else
                            imageDL.graylist = string.Empty;

                        if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg"))
                            imageDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                        else
                            imageDL.blacklist = string.Empty;


                        if (ini.KeyExists("saveInfo", "Global"))
                            imageDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                        else
                            imageDL.saveInfo = true;

                        if (ini.KeyExists("ignoreFinish", "Global"))
                            imageDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                        else
                            imageDL.ignoreFinish = false;


                        if (ini.KeyExists("fileNameSchema", "Images"))
                            imageDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(ini.ReadString("fileNameSchema", "Images").ToLower());
                        else
                            imageDL.fileNameSchema = "%artist%_%md5%";

                        if (ini.KeyExists("separateRatings", "Images"))
                            imageDL.separateRatings = ini.ReadBool("separateRatings", "Images");
                        else
                            imageDL.separateRatings = true;

                        if (ini.KeyExists("separateBlacklisted", "Images"))
                            imageDL.separateBlacklisted = ini.ReadBool("separateBlacklisted", "Images");
                        else
                            imageDL.separateBlacklisted = true;

                        if (ini.KeyExists("separateArtists", "Images"))
                            imageDL.separateArtists = ini.ReadBool("separateArtists", "Images");
                        else
                            imageDL.separateArtists = false;
                    }
                    else {
                        imageDL.saveTo = General.Default.saveLocation;
                        imageDL.graylist = General.Default.blacklist;
                        imageDL.blacklist = General.Default.zeroToleranceBlacklist;

                        imageDL.saveInfo = General.Default.saveInfo;
                        imageDL.ignoreFinish = General.Default.ignoreFinish;

                        imageDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(Images.Default.fileNameSchema);
                        imageDL.separateRatings = Images.Default.separateRatings;
                        imageDL.separateBlacklisted = Images.Default.separateBlacklisted;
                        imageDL.separateArtists = Images.Default.separateArtists;
                    }

                    imageDL.Show();
                }
                else {
                    ImageDownloader imageDL = new ImageDownloader();
                    imageDL.postID = id;

                    if (useIni) {
                        imageDL.saveTo = Environment.CurrentDirectory;

                        if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg")))
                            imageDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                        else
                            imageDL.graylist = string.Empty;

                        if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg"))
                            imageDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                        else
                            imageDL.blacklist = string.Empty;


                        if (ini.KeyExists("saveInfo", "Global"))
                            imageDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                        else
                            imageDL.saveInfo = true;

                        if (ini.KeyExists("ignoreFinish", "Global"))
                            imageDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                        else
                            imageDL.ignoreFinish = false;


                        if (ini.KeyExists("fileNameSchema", "Images"))
                            imageDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(ini.ReadString("fileNameSchema", "Images").ToLower());
                        else
                            imageDL.fileNameSchema = "%artist%_%md5%";

                        if (ini.KeyExists("separateRatings", "Images"))
                            imageDL.separateRatings = ini.ReadBool("separateRatings", "Images");
                        else
                            imageDL.separateRatings = true;

                        if (ini.KeyExists("separateBlacklisted", "Images"))
                            imageDL.separateBlacklisted = ini.ReadBool("separateBlacklisted", "Images");
                        else
                            imageDL.separateBlacklisted = true;

                        if (ini.KeyExists("separateArtists", "Images"))
                            imageDL.separateArtists = ini.ReadBool("separateArtists", "Images");
                        else
                            imageDL.separateArtists = false;
                    }
                    else {
                        if (General.Default.saveLocation != string.Empty)
                            imageDL.saveTo = General.Default.saveLocation;
                        else
                            imageDL.saveTo = Environment.CurrentDirectory;
                        imageDL.graylist = General.Default.blacklist;
                        imageDL.blacklist = General.Default.zeroToleranceBlacklist;

                        imageDL.saveInfo = General.Default.saveInfo;
                        imageDL.ignoreFinish = General.Default.ignoreFinish;

                        imageDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(Images.Default.fileNameSchema);
                        imageDL.separateRatings = Images.Default.separateRatings;
                        imageDL.separateBlacklisted = Images.Default.separateBlacklisted;
                        imageDL.separateArtists = Images.Default.separateArtists;
                    }

                    imageDL.downloadImage();
                }

                return true;
            }
            catch {
                return false;
            }
        }
        }
    }
}
