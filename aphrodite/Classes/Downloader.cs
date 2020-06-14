using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aphrodite {
    class Downloader {
        static IniFile ini = new IniFile();

        #region e621
        public static bool downloadPool(string ID, bool useIni = false) {
            try {
                frmPoolDownloader poolDL = new frmPoolDownloader();
                poolDL.header = Program.UserAgent;

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
                        poolDL.fileNameSchema = apiTools.replaceIllegalCharacters(ini.ReadString("fileNameSchema", "Pools").ToLower());
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
                    Settings.Default.Reload();
                    Pools.Default.Reload();

                    if (Settings.Default.saveLocation != string.Empty)
                        poolDL.saveTo = Settings.Default.saveLocation;
                    else
                        poolDL.saveTo = Environment.CurrentDirectory;
                    poolDL.graylist = Settings.Default.blacklist;
                    poolDL.blacklist = Settings.Default.zeroToleranceBlacklist;
                    //poolDL.saveMetadata = Settings.Default.saveMetadata;
                    //poolDL.saveArtistMetadata = Settings.Default.saveArtistMetadata;
                    //poolDL.saveTagMetadata = Settings.Default.saveTagMetadata;

                    poolDL.saveInfo = Settings.Default.saveInfo;
                    poolDL.ignoreFinish = Settings.Default.ignoreFinish;
                    poolDL.saveBlacklisted = Settings.Default.saveBlacklisted;

                    poolDL.fileNameSchema = apiTools.replaceIllegalCharacters(Pools.Default.fileNameSchema);
                    poolDL.mergeBlacklisted = Pools.Default.mergeBlacklisted;
                    poolDL.openAfter = Pools.Default.openAfter;
                }

                poolDL.ShowDialog();

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
                        tagDL.fileNameSchema = apiTools.replaceIllegalCharacters(ini.ReadString("fileNameSchema", "Tags").ToLower());
                    else
                        tagDL.fileNameSchema = "%md5%";
                }
                else {
                    Settings.Default.Reload();
                    Tags.Default.Reload();

                    if (Settings.Default.saveLocation != string.Empty)
                        tagDL.saveTo = Settings.Default.saveLocation;
                    else
                        tagDL.saveTo = Environment.CurrentDirectory;
                    tagDL.graylist = Settings.Default.blacklist;
                    tagDL.blacklist = Settings.Default.zeroToleranceBlacklist;

                    tagDL.saveInfo = Settings.Default.saveInfo;
                    tagDL.openAfter = false;
                    tagDL.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
                    tagDL.ignoreFinish = Settings.Default.ignoreFinish;

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

                    tagDL.fileNameSchema = apiTools.replaceIllegalCharacters(Tags.Default.fileNameSchema.ToLower());
                }

                tagDL.ShowDialog();

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
                        tagDL.fileNameSchema = apiTools.replaceIllegalCharacters(ini.ReadString("fileNameSchema", "Tags").ToLower());
                    else
                        tagDL.fileNameSchema = "%md5%";
                }
                else {
                    Settings.Default.Reload();
                    Tags.Default.Reload();
                    if (Settings.Default.saveLocation != string.Empty)
                        tagDL.saveTo = Settings.Default.saveLocation;
                    else
                        tagDL.saveTo = Environment.CurrentDirectory;
                    tagDL.graylist = Settings.Default.blacklist;
                    tagDL.blacklist = Settings.Default.zeroToleranceBlacklist;

                    tagDL.saveInfo = Settings.Default.saveInfo;
                    tagDL.openAfter = false;
                    tagDL.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
                    tagDL.ignoreFinish = Settings.Default.ignoreFinish;

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

                    tagDL.fileNameSchema = apiTools.replaceIllegalCharacters(Tags.Default.fileNameSchema.ToLower());
                }

                tagDL.ShowDialog();

                return true;
            }
            catch {
                return false;
            }
        }
        public static bool downloadImage(string id, bool useIni = false) {
            if (apiTools.isValidImageLink(id)) {
                id = apiTools.getIdFromURL(id);
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
                            imageDL.fileNameSchema = apiTools.replaceIllegalCharacters(ini.ReadString("fileNameSchema", "Images").ToLower());
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
                    }
                    else {
                        imageDL.saveTo = Settings.Default.saveLocation;
                        imageDL.graylist = Settings.Default.blacklist;
                        imageDL.blacklist = Settings.Default.zeroToleranceBlacklist;

                        imageDL.saveInfo = Settings.Default.saveInfo;
                        imageDL.ignoreFinish = Settings.Default.ignoreFinish;

                        imageDL.fileNameSchema = apiTools.replaceIllegalCharacters(Images.Default.fileNameSchema);
                        imageDL.separateRatings = Images.Default.separateRatings;
                        imageDL.separateBlacklisted = Images.Default.separateBlacklisted;
                    }

                    imageDL.ShowDialog();
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
                            imageDL.fileNameSchema = apiTools.replaceIllegalCharacters(ini.ReadString("fileNameSchema", "Images").ToLower());
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
                    }
                    else {
                        if (Settings.Default.saveLocation != string.Empty)
                            imageDL.saveTo = Settings.Default.saveLocation;
                        else
                            imageDL.saveTo = Environment.CurrentDirectory;
                        imageDL.graylist = Settings.Default.blacklist;
                        imageDL.blacklist = Settings.Default.zeroToleranceBlacklist;

                        imageDL.saveInfo = Settings.Default.saveInfo;
                        imageDL.ignoreFinish = Settings.Default.ignoreFinish;

                        imageDL.fileNameSchema = apiTools.replaceIllegalCharacters(Images.Default.fileNameSchema);
                        imageDL.separateRatings = Images.Default.separateRatings;
                        imageDL.separateBlacklisted = Images.Default.separateBlacklisted;
                    }

                    imageDL.downloadImage();
                }

                return true;
            }
            catch {
                return false;
            }
        }
        #endregion

    }
}
