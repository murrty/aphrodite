using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace aphrodite_min {
    class Program {

    #region variables
        public static readonly string UserAgent = "User-Agent: aphrodite-min/" + (Properties.Default.currentVersion) + " (Contact: https://github.com/murrty/aphrodite ... open an issue)";                                  // User agent used for the application.

        static readonly bool debugMode = true;      // Debug mode.
        static bool hasArg = false;                 // Determine if there's an argument to perform an action.
        static bool useIni = false;                 // Determine if the INI file should be used.
        static IniFile ini = new IniFile();
    #endregion

        static void Main(string[] args) {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Console.Title = "aphrodite min " + Properties.Default.currentVersion;

            if (Settings.Default.firstTimeSetup && !debugMode) {
                firstTimeSetup();
            }
            //else if (!debugMode) {
            //    if (File.Exists(Environment.CurrentDirectory + "\\aphrodite.ini"))
            //        if (ini.KeyExists("useIni"))
            //            if (ini.ReadBool("useIni"))
            //                useIni = true;
            //}

            if (readArguments(args))
                Environment.Exit(0);

START:
            Console.Clear();

            if (debugMode) {
                readArgumentsDebug(args);
            }

            if (!hasArg) {
                Console.WriteLine("------- aphrodite min -------");
                Console.WriteLine("| Please select your option |");
                if (useIni)
                    Console.WriteLine("|   Portable mode enabled   |");
                addLinedLines(29);
                Console.WriteLine("(1)   Download a pool from ID");
                Console.WriteLine("(2)   Download images from tags");
                Console.WriteLine("(3)   Download an image from post ID");
                if (!useIni)
                    Console.WriteLine("(7)   Pool wishlist");
                Console.WriteLine("(8)   Change settings");
                Console.WriteLine("(9)   About aphrodite");
                Console.WriteLine("(X)   Exit");
                if (debugMode) {
                    addLinedLines(29);
                    Console.WriteLine("Debug options:\n");
                    Console.WriteLine("(TAG)   (Debug) Test tag downloader");
                    Console.WriteLine("(POOL)  (Debug) Test pool downloader");
                    Console.WriteLine("(IMAGE) (Debug) Test image downloader");
                    addLinedLines(29);
                }
                addBlankLines(1);
                Console.Write("Your choice: ");
                string choice = Console.ReadLine();
                switch (choice.ToUpper()) {
                    case "1": case "POOL": case "POOLS": case "DOWNLOADPOOL": case "DOWNLOADPOOLS":
                        downloadPool();
                        goto START;
                    case "2": case "TAG": case "TAGS": case "DOWNLOADTAG": case "DOWNLOADTAGS":
                        downloadTag();
                        goto START;
                    case "3": case "IMAGE": case "IMAGES": case "DOWNLOADIMAGE": case "DOWNLOADIMAGES":
                        downloadImage();
                        goto START;
                    case "7": case "W": case "WISHLIST": case "PW":
                        if (!useIni)
                            loadWishlist();
                        goto START;
                    case "8": case "SETTINGS": case "SETTING": case "CHANGESETTINGS":
                        changeSettings();
                        goto START;
                    case "9": case "ABOUT":
                        showAbout();
                        goto START;
                    case "TESTTAG":
                        testTagDownloader();
                        goto START;
                    case "TESTPOOL":
                        testPoolDownloader();
                        goto START;
                    case "TESTIMAGE":
                        testImageDownloader();
                        goto START;
                    default:
                        break;
                }
            }
        }
        static bool readArguments(string[] args) {
            for (int i = 0; i < args.Length; i++) {
                if (args.Length <= 0)
                    return false;

                if (args[i].StartsWith("pools:configuresettings") || args[i].StartsWith("tags:configuresettings") || args[i].StartsWith("images:configuresettings") || args[i].StartsWith("configuresettings")) {
                    if (args[i].StartsWith("pools")) {
                        loadPoolSettings();
                        return true;
                    }
                    else if (args[i].StartsWith("tags")) {
                        loadTagSettings();
                        return true;
                    }
                    else if (args[i].StartsWith("images")) {
                        loadImageSettings();
                        return true;
                    }
                    else {
                        changeSettings();
                        return true;
                    }
                }
                else if (args[i].StartsWith("pools:")) {
                    if (apiTools.isValidPoolLink(args[i].Replace("pools:", ""))) {
                        downloadPool(args[i].Replace("pools:", ""));
                    }
                    return true;
                }
                else if (args[i].StartsWith("images:")) {
                    if (apiTools.isValidImageLink(args[i].Replace("images:", ""))) {
                        downloadImage(args[i].Replace("images:", "").Split('/')[5]);
                    }
                    return true;
                }
                else if (args[i].StartsWith("tags:") || args[i].StartsWith("tag:")) {
                    if (apiTools.isValidPageLink(args[i].Replace("tags:", ""))) {
                        downloadPage(args[i].Replace("tags:", ""));
                    }
                    else {
                        string tags = getTags(args);
                        if (string.IsNullOrWhiteSpace(tags))
                            return false;

                        Console.WriteLine("Would you like to download the tags \"{0}\"?", tags);
                        if (askYesOrNo()) {
                            downloadTag(args[i].Replace("tags:", ""));
                        }
                    }

                    return true;
                }
                else
                    return false;
            }

            return false;
        }

    #region download methods
        static void downloadTag(string tags = "") {
retry:
            Console.Clear();
            if (string.IsNullOrWhiteSpace(tags)) {
                Console.WriteLine("Please enter the tag(s) you want to download (6 maximum)\n(Enter nothing to return to menu)\n");
                Console.Write("Tag(s): ");
                tags = Console.ReadLine();
            }

            int tagCount = tags.Split(' ').Length;
            if (tagCount > 6) {
                Console.Clear();
                Console.WriteLine("Maximum of 6 tags are allowed.\nRetry?");
                if (askYesOrNo())
                    goto retry;
                else
                    return;
            }

            if (string.IsNullOrWhiteSpace(tags)) {
                Console.Clear();
                return;
            }


            TagDownloader tagDL = new TagDownloader();
            tagDL.debugMode = debugMode;
            tagDL.minMode = true;

            tagDL.tags = tags;
            tagDL.webHeader = UserAgent;
            string rates = "";

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

                if (ini.KeyExists("usePageLimit", "Tags"))
                    tagDL.usePageLimit = ini.ReadBool("usePageLimit", "Tags");
                else
                    tagDL.usePageLimit = false;

                if (tagDL.usePageLimit)
                    tagDL.pageLimit = ini.ReadInt("pageLimit", "Tags");

                if (ini.KeyExists("separateRatings", "Tags"))
                    tagDL.separateRatings = ini.ReadBool("separateRatings", "Tags");
                else
                    tagDL.separateRatings = true;

                if (ini.KeyExists("Explicit", "Tags"))
                    if (ini.ReadBool("Explicit", "Tags"))
                        rates += "e ";
                if (ini.KeyExists("Questionable", "Tags"))
                    if (ini.ReadBool("Questionable", "Tags"))
                        rates += "q ";
                if (ini.KeyExists("Safe", "Tags"))
                    if (ini.ReadBool("Safe", "Tags"))
                        rates += "s";
                rates = rates.TrimEnd(' ');

                if (tagDL.separateRatings)
                    tagDL.ratings = rates.Split(' ');
            }
            else {
                tagDL.graylist = Settings.Default.graylist;
                tagDL.blacklist = Settings.Default.blacklist;

                tagDL.saveTo = Settings.Default.saveLocation;
                tagDL.saveInfo = Settings.Default.saveInfo;
                tagDL.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
                tagDL.ignoreFinish = Settings.Default.exitOnComplete;

                tagDL.useMinimumScore = Tags.Default.useScoreMin;
                if (tagDL.useMinimumScore) {
                    tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                    tagDL.minimumScore = Tags.Default.scoreMin;
                }
                tagDL.imageLimit = Tags.Default.imageLimit;
                tagDL.usePageLimit = Tags.Default.usePageLimit;
                if (tagDL.usePageLimit)
                    tagDL.pageLimit = Tags.Default.pageLimit;
                tagDL.separateRatings = Tags.Default.separateRatings;
                if (tagDL.separateRatings) {
                    if (Tags.Default.Explicit)
                        rates += "e ";
                    if (Tags.Default.Questionable)
                        rates += "q ";
                    if (Tags.Default.Safe)
                        rates += "s";

                    rates = rates.TrimEnd(' ');
                    tagDL.ratings = rates.Split(' ');
                }
            }

            if (!tagDL.downloadTags()) {
                Console.WriteLine("An error occured.");
                Console.ReadKey();
            }

            return;
        }
        static void downloadPage(string url) {
            if (string.IsNullOrWhiteSpace(url))
                return;

            TagDownloader tagDL = new TagDownloader();
            tagDL.debugMode = debugMode;
            tagDL.minMode = true;

            tagDL.downloadUrl = url;
            tagDL.webHeader = UserAgent;
            string rates = "";

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

                if (ini.KeyExists("usePageLimit", "Tags"))
                    tagDL.usePageLimit = ini.ReadBool("usePageLimit", "Tags");
                else
                    tagDL.usePageLimit = false;

                if (tagDL.usePageLimit)
                    tagDL.pageLimit = ini.ReadInt("pageLimit", "Tags");

                if (ini.KeyExists("separateRatings", "Tags"))
                    tagDL.separateRatings = ini.ReadBool("separateRatings", "Tags");
                else
                    tagDL.separateRatings = true;

                if (ini.KeyExists("Explicit", "Tags"))
                    if (ini.ReadBool("Explicit", "Tags"))
                        rates += "e ";
                if (ini.KeyExists("Questionable", "Tags"))
                    if (ini.ReadBool("Questionable", "Tags"))
                        rates += "q ";
                if (ini.KeyExists("Safe", "Tags"))
                    if (ini.ReadBool("Safe", "Tags"))
                        rates += "s";
                rates = rates.TrimEnd(' ');

                if (tagDL.separateRatings)
                    tagDL.ratings = rates.Split(' ');
            }
            else {
                tagDL.graylist = Settings.Default.graylist;
                tagDL.blacklist = Settings.Default.blacklist;

                tagDL.fromURL = true;

                tagDL.saveTo = Settings.Default.saveLocation;
                tagDL.saveInfo = Settings.Default.saveInfo;
                tagDL.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
                tagDL.ignoreFinish = Settings.Default.exitOnComplete;

                tagDL.useMinimumScore = Tags.Default.useScoreMin;
                if (tagDL.useMinimumScore) {
                    tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                    tagDL.minimumScore = Tags.Default.scoreMin;
                }
                tagDL.imageLimit = Tags.Default.imageLimit;
                tagDL.usePageLimit = Tags.Default.usePageLimit;
                if (tagDL.usePageLimit)
                    tagDL.pageLimit = Tags.Default.pageLimit;
                tagDL.separateRatings = Tags.Default.separateRatings;
                if (tagDL.separateRatings) {
                    if (Tags.Default.Explicit)
                        rates += "e ";
                    if (Tags.Default.Questionable)
                        rates += "q ";
                    if (Tags.Default.Safe)
                        rates += "s";

                    rates = rates.TrimEnd(' ');
                    tagDL.ratings = rates.Split(' ');
                }
            }

            if (!tagDL.downloadPage(url)) {
                Console.WriteLine("An error occured.");
                Console.ReadKey();
            }

            return;
        }
        static void downloadPool(string id = "") {
            Console.Clear();
            int tryID = 0;
            Int32.TryParse(id, out tryID);
            if (string.IsNullOrWhiteSpace(id) || tryID <= 0) {
                Console.WriteLine("Enter the Pool ID to download...\n(Enter nothing to return to menu)\n");
                Console.Write("Post ID: ");
                id = Console.ReadLine();
            }

            Int32.TryParse(id, out tryID);

            if (string.IsNullOrWhiteSpace(id) || tryID <= 0) {
                Console.Clear();
                return;
            }

            Console.WriteLine("Downloading pool ID " + id);
            PoolDownloader poolDl = new PoolDownloader();
            poolDl.debugMode = debugMode;
            poolDl.minMode = true;
            
            poolDl.header = UserAgent;

            if (useIni) {
                poolDl.saveTo = Environment.CurrentDirectory;

                if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg")))
                    poolDl.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                else
                    poolDl.graylist = string.Empty;

                if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg"))
                    poolDl.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                else
                    poolDl.blacklist = string.Empty;



                if (ini.KeyExists("saveInfo", "Global"))
                    poolDl.saveInfo = ini.ReadBool("saveInfo", "Global");
                else
                    poolDl.saveInfo = true;

                if (ini.KeyExists("saveBlacklisted", "Global"))
                    poolDl.saveBlacklisted = ini.ReadBool("saveBlacklisted", "Global");
                else
                    poolDl.saveBlacklisted = true;

                if (ini.KeyExists("ignoreFinish", "Global"))
                    poolDl.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                else
                    poolDl.ignoreFinish = false;



                if (ini.KeyExists("usePoolName", "Pools"))
                    poolDl.usePoolName = ini.ReadBool("usePoolName", "Pools");
                else
                    poolDl.usePoolName = true;

                if (ini.KeyExists("mergeBlacklisted", "Pools"))
                    poolDl.mergeBlacklisted = ini.ReadBool("mergeBlacklisted", "Pools");
                else
                    poolDl.mergeBlacklisted = true;

                if (ini.KeyExists("openAfter", "Pools"))
                    poolDl.openAfter = ini.ReadBool("openAfter", "Pools");
                else
                    poolDl.openAfter = false;
            }
            else {
                poolDl.saveTo = Settings.Default.saveLocation;
                poolDl.graylist = Settings.Default.graylist;
                poolDl.blacklist = Settings.Default.blacklist;

                poolDl.saveInfo = Settings.Default.saveInfo;
                poolDl.ignoreFinish = Settings.Default.exitOnComplete;
                poolDl.saveBlacklisted = Settings.Default.saveBlacklisted;

                poolDl.usePoolName = Pools.Default.usePoolName;
                poolDl.mergeBlacklisted = Pools.Default.mergeBlacklisted;
                poolDl.openAfter = Pools.Default.openAfter;
            }

            if (!poolDl.downloadPool(id)) {
                Console.WriteLine("An error occured.");
                Console.ReadKey();
            }

            return;
        }
        static void downloadImage(string id = "") {
            Console.Clear();
            if (string.IsNullOrWhiteSpace(id)) {
                Console.WriteLine("Enter the post ID to download...\n(Enter nothing to return to menu)\n");
                Console.Write("Post ID: ");
                id = Console.ReadLine();
            }

            if (string.IsNullOrWhiteSpace(id)) {
                Console.Clear();
                return;
            }

            Console.WriteLine("Downloading image ID " + id);
            ImageDownloader dlImage = new ImageDownloader();
            dlImage.debugMode = debugMode;
            dlImage.minMode = true;

            dlImage.header = UserAgent;

            if (useIni) {
                dlImage.saveTo = Environment.CurrentDirectory;

                if (ini.KeyExists("graylist"))
                    dlImage.graylist = ini.ReadString("graylist");
                else
                    dlImage.graylist = string.Empty;

                if (ini.KeyExists("blacklist"))
                    dlImage.blacklist = ini.ReadString("blacklist");
                else
                    dlImage.blacklist = string.Empty;



                if (ini.KeyExists("saveInfo", "Global"))
                    dlImage.saveInfo = ini.ReadBool("saveInfo", "Global");
                else
                    dlImage.saveInfo = true;

                if (ini.KeyExists("ignoreFinish", "Global"))
                    dlImage.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                else
                    dlImage.ignoreFinish = false;



                if (ini.KeyExists("fileNameCode", "Images"))
                    dlImage.fileNameCode = ini.ReadInt("fileNameCode", "Images");
                else
                    dlImage.fileNameCode = 1;

                if (ini.KeyExists("separateRatings", "Images"))
                    dlImage.separateRatings = ini.ReadBool("separateRatings", "Images");
                else
                    dlImage.separateRatings = true;

                if (ini.KeyExists("separateBlacklisted", "Images"))
                    dlImage.separateBlacklisted = ini.ReadBool("separateBlacklisted", "Images");
                else
                    dlImage.separateBlacklisted = true;
            }
            else {
                dlImage.saveTo = Settings.Default.saveLocation;

                dlImage.graylist = Settings.Default.graylist;
                dlImage.blacklist = Settings.Default.blacklist;

                dlImage.separateRatings = Images.Default.separateRatings;
                dlImage.separateBlacklisted = Images.Default.separateBlacklisted;

                dlImage.saveInfo = Settings.Default.saveInfo;
                dlImage.ignoreFinish = Settings.Default.exitOnComplete;

                dlImage.fileNameCode = Images.Default.fileNameCode;
            }
            if (!dlImage.downloadImage("https://e621.net/post/show/" + id)) {
                Console.WriteLine("An error occured.");
                Console.ReadKey();
            }

            return;
        }
    #endregion

    #region check methods
        static string getTags(string[] args) {
            string tags = null;
            for (int i =0; i < args.Length; i++) {
                if (args[i].StartsWith("tags:") || args[i].StartsWith("tag:")) {
                    tags += args[i].Replace("tags:", "").Replace("tag:", "") + " ";
                }
            }
            tags = tags.TrimEnd(' ');
            return tags;
        }
    #endregion

    #region settings
        static void firstTimeSetup() {
            Console.Clear();
            addLinedLines(36);
            Console.WriteLine("----- welcome to aphrodite min -----");
            addLinedLines(36);
            Console.WriteLine("Since this is your first time running, you should configure the directory.\nPress any key to continue...");
            Console.ReadKey();
            configureSaveDirectory();
            Console.Clear();
            Console.WriteLine("The save directory has been set. Would you like to configure settings?");
            Console.WriteLine("You can change the settings at any time.\n");
            if (askYesOrNo()) {
                changeSettings();
            }
            Settings.Default.firstTimeSetup = false;
            Settings.Default.Save();
            Tags.Default.Save();
            Pools.Default.Save();
            Images.Default.Save();
            Console.Clear();
        }

        static void changeSettings() {
settingsMain:
            Console.Clear();
            Console.WriteLine("Select what options you wish to alter");
            addLinedLines(37);
            Console.WriteLine("(1)   Change global settings");
            Console.WriteLine("(2)   Change tag settings");
            Console.WriteLine("(3)   Change pool settings");
            Console.WriteLine("(4)   Change image settings");
            Console.WriteLine("(5)   Modify blacklists");
            Console.WriteLine("(7)   Restore default settings (Blacklists excluded)");
            Console.WriteLine("(9)   Save settings.");
            Console.WriteLine("Alternatively, press any key to return to the menu without saving.\n");
            Console.Write("Option: ");
            string answer = Console.ReadLine();
            switch (answer.ToUpper()) {
                case "1": case "GLOBAL":
                    loadGlobalSettings();
                    goto settingsMain;
                case "2": case "TAG": case "TAGS":
                    loadTagSettings();
                    goto settingsMain;
                case "3": case "POOL": case "POOLS":
                    loadPoolSettings();
                    goto settingsMain;
                case "4": case "IMAGE": case "IMAGES":
                    loadImageSettings();
                    goto settingsMain;
                case "5": case "BLACKLIST": case "BLACKLISTS":
                    changeBlacklist();
                    goto settingsMain;
                case "7": case "DEFAULT":
                    goto settingsMain;
                case "9": case "MENU": case "EXIT":
                    if (!useIni) {
                        Settings.Default.Save();
                        Tags.Default.Save();
                        Pools.Default.Save();
                        Images.Default.Save();
                    }
                    break;
                default:
                    break;
            }
        }

        static void loadGlobalSettings() {
settingsMain:
            Console.Clear();
            Console.WriteLine("Editing global settings");
            Console.WriteLine("Select which setting you'd like to toggle/change.");
            addLinedLines(42);
            if (useIni) {
                Console.WriteLine("(1)   Save directory:                    @");
                Console.WriteLine("(2)   Save .nfo files:                   " + ini.ReadBool("saveInfo", "Global"));
                Console.WriteLine("(3)   Save blacklisted images:           " + ini.ReadBool("saveBlacklisted", "Global"));
                Console.WriteLine("(4)   Don't notify finished downloads:   " + ini.ReadBool("ignoreFinish", "Global"));
            }
            else {
                Console.WriteLine("(1)   Save directory:                    " + Settings.Default.saveLocation);
                Console.WriteLine("(2)   Save .nfo files:                   " + Settings.Default.saveInfo);
                Console.WriteLine("(3)   Save blacklisted images:           " + Settings.Default.saveBlacklisted);
                Console.WriteLine("(4)   Don't notify finished downloads:   " + Settings.Default.exitOnComplete);
            }
            Console.WriteLine("(*)   Exit");
            Console.WriteLine("(Notice: blacklisted tags are mutually shared.)\n");
            Console.Write("Option: ");
            string resp = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(resp))
                goto settingsMain;
            int output;
            Int32.TryParse(resp, out output);
            switch (output) {
                case 1:
                    if (!useIni)
                        configureSaveDirectory();
                    goto settingsMain;
                case 2:
                    if (useIni) {
                        bool bOut = ini.ReadBool("saveInfo", "Global");
                        bOut ^= true;
                        ini.WriteBool("saveInfo", bOut, "Global");
                    }
                    else {
                        Settings.Default.saveInfo ^= true;
                    }
                    goto settingsMain;
                case 3:
                    if (useIni) {
                        bool bOut = ini.ReadBool("saveBlacklisted", "Global");
                        bOut ^= true;
                        ini.WriteBool("saveBlacklisted", bOut, "Global");
                    }
                    else {
                        Settings.Default.saveBlacklisted ^= true;
                    }
                    goto settingsMain;
                case 4:
                    if (useIni) {
                        bool bOut = ini.ReadBool("ignoreFinish", "Global");
                        bOut ^= true;
                        ini.WriteBool("ignoreFinish", bOut, "Global");
                    }
                    else {
                        Settings.Default.exitOnComplete ^= true;
                    }
                    goto settingsMain;
                default:
                    break;
            }
        }
        static void loadTagSettings() {
settingsMain:
            Console.Clear();
            Console.WriteLine("Editing tag settings");
            Console.WriteLine("Explicit, Questionable, and Safe options don't matter if separate images is off.");
            Console.WriteLine("Select which setting you'd like to toggle/change.");
            addLinedLines(42);
            if (useIni) {
                Console.WriteLine("(1)   Separate ratings:               " + ini.ReadBool("separateRatings", "Tags"));
                Console.WriteLine("(2)   Download explicit images:       " + ini.ReadBool("Explicit", "Tags"));
                Console.WriteLine("(3)   Download questionable images:   " + ini.ReadBool("Questionable", "Tags"));
                Console.WriteLine("(4)   Download safe images:           " + ini.ReadBool("Safe", "Tags"));
                Console.WriteLine("(5)   Use score limit:                " + ini.ReadBool("useMinimumScore", "Tags"));
                Console.WriteLine("(6)   Score limit:                    " + ini.ReadInt("scoreMin", "Tags"));
                Console.WriteLine("(7)   Use page limit:                 " + ini.ReadBool("usePageLimit", "Tags"));
                Console.WriteLine("(8)   Page limit:                     " + ini.ReadInt("pageLimit", "Tags"));
                Console.WriteLine("(9)   Image limit:                    " + ini.ReadInt("imageLimit", "Tags"));
                //Console.WriteLine("(10)  Score as tag");
            }
            else {
                Console.WriteLine("(1)   Separate ratings:               " + Tags.Default.separateRatings);
                Console.WriteLine("(2)   Download explicit images:       " + Tags.Default.Explicit);
                Console.WriteLine("(3)   Download questionable images:   " + Tags.Default.Questionable);
                Console.WriteLine("(4)   Download safe images:           " + Tags.Default.Safe);
                Console.WriteLine("(5)   Use score limit:                " + Tags.Default.useScoreMin);
                Console.WriteLine("(6)   Score limit:                    " + Tags.Default.scoreMin);
                Console.WriteLine("(7)   Use page limit:                 " + Tags.Default.usePageLimit);
                Console.WriteLine("(8)   Page limit:                     " + Tags.Default.pageLimit);
                Console.WriteLine("(9)   Image limit:                    " + Tags.Default.imageLimit);
            }
            Console.WriteLine("(*)   Exit");
            Console.WriteLine("(Notice: blacklisted tags are mutually shared.)\n");
            Console.Write("Option: ");
            string resp = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(resp))
                goto settingsMain;
            int output;
            Int32.TryParse(resp, out output);
            switch (output) {
                case 1:
                    if (useIni) {
                        bool bOut = ini.ReadBool("separateRatings", "Tags");
                        bOut ^= true;
                        ini.WriteBool("separateRatings", bOut, "Tags");
                    }
                    else {
                        Tags.Default.separateRatings ^= true;
                    }
                    goto settingsMain;
                case 2:
                    if (useIni) {
                        bool bOut = ini.ReadBool("Explicit", "Tags");
                        bOut ^= true;
                        ini.WriteBool("Explicit", bOut, "Tags");
                    }
                    else {
                        Tags.Default.Explicit ^= true;
                    }
                    goto settingsMain;
                case 3:
                    if (useIni) {
                        bool bOut = ini.ReadBool("Questionable", "Tags");
                        bOut ^= true;
                        ini.WriteBool("Questionable", bOut, "Tags");
                    }
                    else {
                        Tags.Default.Questionable ^= true;
                    }
                    goto settingsMain;
                case 4:
                    if (useIni) {
                        bool bOut = ini.ReadBool("Safe", "Tags");
                        bOut ^= true;
                        ini.WriteBool("Safe", bOut, "Tags");
                    }
                    else {
                        Tags.Default.Safe ^= true;
                    }
                    goto settingsMain;
                case 5:
                    if (useIni) {
                        bool bOut = ini.ReadBool("useMinimumScore", "Tags");
                        bOut ^= true;
                        ini.WriteBool("useMinimumScore", bOut, "Tags");
                    }
                    else {
                        Tags.Default.useScoreMin ^= true;
                    }
                    goto settingsMain;
                case 6:
                    if (useIni) {
                        ini.WriteInt("scoreMin", askForNumber(), "Tags");
                    }
                    else {
                        Tags.Default.scoreMin = askForNumber();
                    }
                    goto settingsMain;
                case 7:
                    if (useIni) {
                        bool bOut = ini.ReadBool("usePageLimit", "Tags");
                        bOut ^= true;
                        ini.WriteBool("usePageLimit", bOut, "Tags");
                    }
                    else {
                        Tags.Default.usePageLimit ^= true;
                    }
                    goto settingsMain;
                case 8:
                    if (useIni) {
                        ini.WriteInt("pageLimit", askForNumber(), "Tags");
                    }
                    else {
                        Tags.Default.pageLimit = askForNumber();
                    }
                    goto settingsMain;
                case 9:
                    if (useIni) {
                        ini.WriteInt("imageLimit", askForNumber(), "Tags");
                    }
                    else {
                        Tags.Default.imageLimit = askForNumber();
                    }
                    goto settingsMain;
                default:
                    break;
            }
        }
        static void loadPoolSettings() {
settingsMain:
            Console.Clear();
            Console.WriteLine("Editing pools settings");
            Console.WriteLine("Select which setting you'd like to toggle/change.");
            addLinedLines(42);
            if (useIni) {
                Console.WriteLine("(1)   Save files as \"pool_####\":          " + ini.ReadBool("usePoolName", "Pools"));
                Console.WriteLine("(2)   Merge blacklisted pages:              " + ini.ReadBool("mergeBlacklisted", "Pools"));
                Console.WriteLine("(3)   Open folder after downloading:        " + ini.ReadBool("openAfter", "Pools"));
                //Console.WriteLine("(4)   Add pools to the wishlist silently:   " + Pools.Default.addWishlistSilent);
            }
            else {
                Console.WriteLine("(1)   Save files as \"pool_####\":          " + Pools.Default.usePoolName);
                Console.WriteLine("(2)   Merge blacklisted pages:              " + Pools.Default.mergeBlacklisted);
                Console.WriteLine("(3)   Open folder after downloading:        " + Pools.Default.openAfter);
                Console.WriteLine("(4)   Add pools to the wishlist silently:   " + Pools.Default.addWishlistSilent);
            }
            Console.WriteLine("(*)   Exit");
            Console.WriteLine("(Notice: blacklisted tags are mutually shared.)\n");
            Console.Write("Option: ");
            string resp = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(resp))
                goto settingsMain;
            int output;
            Int32.TryParse(resp, out output);
            switch (output) {
                case 1:
                    if (useIni) {
                        bool bOut = ini.ReadBool("usePoolName", "Pools");
                        bOut ^= true;
                        ini.WriteBool("usePoolName", bOut, "Pools");
                    }
                    else {
                        Pools.Default.usePoolName ^= true;
                    }
                    goto settingsMain;
                case 2:
                    if (useIni) {
                        bool bOut = ini.ReadBool("mergeBlacklisted", "Pools");
                        bOut ^= true;
                        ini.WriteBool("mergeBlacklisted", bOut, "Pools");
                    }
                    else {
                        Pools.Default.mergeBlacklisted ^= true;
                    }
                    goto settingsMain;
                case 3:
                    if (useIni) {
                        bool bOut = ini.ReadBool("openAfter", "Pools");
                        bOut ^= true;
                        ini.WriteBool("openAfter", bOut, "Pools");
                    }
                    else {
                        Pools.Default.openAfter ^= true;
                    }
                    goto settingsMain;
                case 4:
                    if (useIni)
                        break;
                    Pools.Default.addWishlistSilent ^= true;
                    goto settingsMain;
                default:
                    break;
            }
        }
        static void loadImageSettings() {
settingsMain:
            Console.Clear();
            Console.WriteLine("Editing image settings");
            Console.WriteLine("Select which setting you'd like to toggle/change.");
            addLinedLines(42);
            string nameAs = "artist_md5";
            if (useIni) {
                switch (ini.ReadInt("fileNameCode", "Images")) {
                    case 0:
                        nameAs = "md5";
                        break;
                    case 1:
                        nameAs = "artist_md5";
                        break;
                    default:
                        nameAs = "artist_md5";
                        break;
                }
                Console.WriteLine("(1)   Saving images as:              " + nameAs);
                Console.WriteLine("(2)   Separate ratings:              " + ini.ReadBool("separateRatings", "Images"));
                Console.WriteLine("(3)   Separate blacklisted images:   " + ini.ReadBool("separateBlacklisted", "Images"));
            }
            else {
                switch (Images.Default.fileNameCode) {
                    case 0:
                        nameAs = "md5";
                        break;
                    case 1:
                        nameAs = "artist_md5";
                        break;
                    default:
                        nameAs = "artist_md5";
                        break;
                }
                Console.WriteLine("(1)   Saving images as:              " + nameAs);
                Console.WriteLine("(2)   Separate ratings:              " + Images.Default.separateRatings);
                Console.WriteLine("(3)   Separate blacklisted images:   " + Images.Default.separateBlacklisted);
            }
            Console.WriteLine("(*)   Exit");
            Console.WriteLine("(Notice: blacklisted tags are mutually shared.)\n");
            Console.Write("Option: ");
            string resp = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(resp))
                goto settingsMain;
            int output;
            Int32.TryParse(resp, out output);
            switch (output) {
                case 1:
                    Console.WriteLine("\nSelect the file name for your downloaded files.\n0 = \"md5\"\n1 = \"artist_md5\"");
                    if (useIni) {
                        ini.WriteInt("fileNameCode", askForNumber(), "Images");
                    }
                    else {
                        Images.Default.fileNameCode = askForNumber();
                    }
                    goto settingsMain;
                case 2:
                    if (useIni) {
                        bool bOut = ini.ReadBool("separateRatings", "Images");
                        bOut ^= true;
                        ini.WriteBool("separateRatings", bOut, "Images");
                    }
                    else {
                        Images.Default.separateRatings ^= true;
                    }
                    goto settingsMain;
                case 3:
                    if (useIni) {
                        bool bOut = ini.ReadBool("separateBlacklisted", "Images");
                        bOut ^= true;
                        ini.WriteBool("separateBlacklisted", bOut, "Images");
                    }
                    else {
                        Images.Default.separateBlacklisted ^= true;
                    }
                    goto settingsMain;
                default:
                    break;
            }
        }

        static void configureSaveDirectory() {
            int dirTries = 0;
setDirectory:
            Console.Clear();
noDir:
            Console.WriteLine("Please enter the directory you want to save files at\n");
            Console.Write("Directory: ");
            string newDir = Console.ReadLine();
            Console.Clear();
            if (string.IsNullOrWhiteSpace(newDir)) {
                dirTries++;
                string times = "Please enter a valid directory. You tried an empty directory " + dirTries + " time";
                if (dirTries > 1)
                    times += "s";
                times += ".";
                Console.WriteLine(times);
                goto noDir;
            }
            Console.WriteLine("Is this directory correct?\n" + newDir + "\n\n");
            if (askYesOrNo()) {
                Settings.Default.saveLocation = newDir;
            }
            else {
                goto setDirectory;
            }
            Console.Clear();
            if (!Directory.Exists(newDir)) {
                Console.WriteLine("The directory does not exist. Create it?\n\n");
                if (askYesOrNo()) {
                    Directory.CreateDirectory(newDir);
                }
            }
        }

        static void changeBlacklist() {
settingsMain:
            Console.Clear();
            Console.WriteLine("Select which list you'd like to modify");
            Console.WriteLine("(1) Blacklist");
            Console.WriteLine("(2) Zero-Tolerance blacklist");
            Console.WriteLine("Blacklisted tags are mutually shared.\n");
            Console.Write("Option: ");
            string answer = Console.ReadLine();
            int outp;
            Int32.TryParse(answer, out outp);
            switch (outp) {
                case 1:
                    editGraylist();
                    goto settingsMain;
                case 2:
                    editBlacklist();
                    goto settingsMain;
                default:
                    break;
            }
        }

        static void editGraylist() {
settingMain:
            Console.Clear();
            Console.WriteLine("Currenty, the following tags are in your blacklist:");
            addLinedLines(51);
            Console.WriteLine(Settings.Default.graylist);
            addLinedLines(51);
            Console.WriteLine("Would you like to add or remove tags?");
            Console.WriteLine("(1) Add tag(s)");
            Console.WriteLine("(2) Remove tag(s)");
            Console.WriteLine("(*) Return to previous menu\n");
            Console.Write("Option: ");
            string answer = Console.ReadLine();
            int outp;
            Int32.TryParse(answer, out outp);
            switch (outp) {
                case 1:
                    addToGraylist();
                    goto settingMain;
                case 2:
                    removeFromGraylist();
                    goto settingMain;
                default:
                    break;
            }
        }
        static void addToGraylist(){
            Console.Clear();
            Console.WriteLine("Enter the tag(s) you'd like to add to your blacklist.\nSeparate them with spaces, and add underscores when necessary.");
            Console.Write("Tag(s): ");
            string answer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(answer))
                return;
            Settings.Default.graylist += " " + answer;
        }
        static void removeFromGraylist() {
            Console.Clear();
            Console.WriteLine("Enter the tag(s) you'd like to remove from your blacklist..\nSeparate them with spaces, and add underscores when necessary.");
            Console.Write("Tag(s): ");
            string answer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(answer))
                return;

            string graylist = Settings.Default.graylist;

            if (answer.Contains(' ')) {
                string[] tags = answer.Split(' ');
                for (int i = 0; i < tags.Length; i++) {
                    graylist.Replace(tags[i], "");
                }
            }
            else {
                graylist = graylist.Replace(answer, "");
            }
            graylist = graylist.TrimStart(' ');
            graylist = graylist.TrimEnd(' ');
            graylist = graylist.Replace("  ", " ");
            Settings.Default.graylist = graylist;
        }

        static void editBlacklist() {
settingMain:
            Console.Clear();
            Console.WriteLine("Currenty, the following tags are in your zero-tolerance blacklist");
            addLinedLines(50);
            Console.WriteLine(Settings.Default.blacklist);
            addLinedLines(50);
            Console.WriteLine("Would you like to add or remove tags?");
            Console.WriteLine("(1) Add tag(s)");
            Console.WriteLine("(2) Remove tag(s)");
            Console.WriteLine("(*) Return to previous menu\n");
            Console.Write("Option: ");
            string answer = Console.ReadLine();
            int outp;
            Int32.TryParse(answer, out outp);
            switch (outp) {
                case 1:
                    addToBlacklist();
                    goto settingMain;
                case 2:
                    removeFromBlacklist();
                    goto settingMain;
                default:
                    break;
            }
        }
        static void addToBlacklist() {
            Console.Clear();
            Console.WriteLine("Enter the tag(s) you'd like to add to your zero-tolerance blacklist.\nSeparate them with spaces, and add underscores when necessary.");
            Console.Write("Tag(s): ");
            string answer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(answer))
                return;
            Settings.Default.blacklist += " " + answer;
        }
        static void removeFromBlacklist() {
            Console.Clear();
            Console.WriteLine("Enter the tag(s) you'd like to remove from your zero-tolerance blacklist..\nSeparate them with spaces, and add underscores when necessary.");
            Console.Write("Tag(s): ");
            string answer = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(answer))
                return;

            string blacklist = Settings.Default.blacklist;

            if (answer.Contains(' ')) {
                string[] tags = answer.Split(' ');
                for (int i = 0; i < tags.Length; i++) {
                    blacklist.Replace(tags[i], "");
                }
            }
            else {
                blacklist = blacklist.Replace(answer, "");
            }
            blacklist = blacklist.TrimStart(' ');
            blacklist = blacklist.TrimEnd(' ');
            blacklist = blacklist.Replace("  ", " ");
            Settings.Default.blacklist = blacklist;
        }
    #endregion

    #region pool wishlist
        static void addToWishlist() {
            Console.Clear();
            Console.WriteLine("Enter the pool name to add to the wishlist.");
            Console.Write("Pool name: ");
            string poolName = Console.ReadLine().Replace("|", "\\");
            Console.WriteLine("Enter the pool id of the pool");
            Console.Write("Pool id: ");
            string poolID = Console.ReadLine();


        }
        static void loadWishlist() {
RETRY:
            Console.Clear();
            Console.WriteLine("Current pools in the wishlist:");

            string[] wishlistNames = null;
            string[] wishlistIds = null;

            if (Pools.Default.wishlist.Length > 0) {
                wishlistNames = Pools.Default.wishlistNames.Split('|');
                wishlistIds = Pools.Default.wishlist.Split('|');
            }
            if (wishlistIds == null || wishlistIds.Length <= 0) {
                Console.WriteLine("No pools in the wishlist at the moment.");
                Console.WriteLine("Press any key to return to the menu.");
                Console.ReadKey();
                return;
            }
            else {
                for (int i = 0; i < wishlistIds.Length; i++) {
                    Console.WriteLine("(" + (i + 1) + ") " + wishlistIds[i] + " - " + wishlistIds[i]);
                }

                int wishItem;
                Console.Write("Select wishlist item (or nothing to return to the menu): ");
                Int32.TryParse(Console.ReadLine(), out wishItem);
                if (wishItem == 0) {
                    return;
                }
                if (wishItem > wishlistIds.Length)
                    return;
                else
                    readWishlistItem(wishlistNames[wishItem - 1], wishlistIds[wishItem - 1]);
            }
        }

        static void readWishlistItem(string poolName, string poolID) {
            Console.Clear();
            Console.WriteLine("Pool: " + poolName + ", ID: " + poolID);
            Console.WriteLine("(1) - Download pool");
            Console.WriteLine("(2) - Remove from wishlist");
            Console.WriteLine("(*) - Return to previous menu");
            Console.Write("Your choice: ");
            string output = Console.ReadLine();

            switch (output.ToLower()) {
                case "1":
                    downloadPool(poolID);
                    loadWishlist();
                    break;
                case "2":
                    if (askYesOrNo("Delete " + poolName + "?")) {
                        Pools.Default.wishlistNames = Pools.Default.wishlistNames.Replace(poolName, "").Replace("||", "|").TrimEnd('|').TrimStart('|');
                        Pools.Default.wishlist = Pools.Default.wishlist.Replace(poolID, "").Replace("||", "|").TrimEnd('|').TrimStart('|');
                    }
                    loadWishlist();
                    break;
                default:
                    loadWishlist();
                    break;
            }
        }
    #endregion

    #region aesthetic methods
        static void showAbout() {
            Console.Clear();
            Console.WriteLine("---------------------------");
            Console.WriteLine("|    aphrodite min " + Properties.Default.currentVersion + "    |");
            Console.WriteLine("|   developed by murrty   |");
            Console.WriteLine("|(big) thanks to andy for |");
            Console.WriteLine("|     helping me make     |");
            Console.WriteLine("|      this program.      |");
            Console.WriteLine("|           < 3           |");
            Console.WriteLine("---------------------------");
            Console.WriteLine("Source code available at:");
            Console.WriteLine("https://github.com/murrty/aphrodite");
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        static void addBlankLines(int spaces) {
            if (spaces < 1)
                return;

            for (int i = 0; i < spaces; i++) {
                Console.WriteLine("");
            }
        }
        static void addLinedLines(int lineCount) {
            if (lineCount < 1)
                return;

            string bufferString = "";
            for (int i = 0; i < lineCount; i++) {
                bufferString += "-";
            }
            Console.WriteLine(bufferString);
        }
        static bool askYesOrNo(string inputMessage = "") {
retry:
            if (!string.IsNullOrEmpty(inputMessage))
                Console.WriteLine(inputMessage);

            Console.Write("\rY/N: ");
            string answer = Console.ReadLine();
            switch (answer.ToUpper()) {
                case "Y":
                    return true;
                case "N":
                    return false;
                default:
                    goto retry;
            }
        }
        static int askForNumber() {
            Console.Write("\rEnter a new value: ");
            string answer = Console.ReadLine();
            int output;
            Int32.TryParse(answer, out output);
            return output;
        }
    #endregion

    #region debug methods
        static void readArgumentsDebug(string[] args) {
            if (args.Length <= 0)
                return;

            Console.Write("Args:");
            for (int i = 0; i < args.Length; i++) {
                Console.WriteLine("\n" + args[i]);
            }
            Console.ReadKey();
            Console.Clear();
        }
        static void progressTest() {
            addBlankLines(2);
            for (int i = 0; i < 100; i++) {
                Console.Write("\r{0}%   ", i);
                System.Threading.Thread.Sleep(100);
            }
            addBlankLines(2);
        }

        static void testTagDownloader() {
            if (debugMode) {
                TagDownloader dlTag = new TagDownloader();
                dlTag.debugMode = true;
                dlTag.minMode = true;
                dlTag.tags = "zackary911";
                dlTag.webHeader = UserAgent;
                dlTag.graylist = Settings.Default.graylist;
                dlTag.blacklist = Settings.Default.blacklist;
                dlTag.saveTo = Settings.Default.saveLocation;
                dlTag.saveInfo = Settings.Default.saveInfo;
                dlTag.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
                dlTag.ignoreFinish = false;
                dlTag.useMinimumScore = false;
                dlTag.separateRatings = true;
                dlTag.ratings = new string[] { "e", "q", "s" };
                dlTag.downloadTags();
            }
            return;
        }
        static void testPoolDownloader() {
            if (debugMode) {
                PoolDownloader dlPool = new PoolDownloader();
                dlPool.debugMode = true;
                dlPool.minMode = true;

                dlPool.header = UserAgent;
                dlPool.saveTo = Settings.Default.saveLocation;
                dlPool.graylist = Settings.Default.graylist;
                dlPool.blacklist = Settings.Default.blacklist;

                dlPool.saveInfo = true;
                dlPool.ignoreFinish = false;
                dlPool.saveBlacklisted = true;

                dlPool.usePoolName = true;
                dlPool.mergeBlacklisted = true;
                dlPool.downloadPool("8744");
            }
            return;
        }
        static void testImageDownloader() {
            if (debugMode) {
                ImageDownloader dlImage = new ImageDownloader();
                dlImage.debugMode = true;
                dlImage.minMode = true;
                dlImage.header = UserAgent;
                dlImage.saveTo = Settings.Default.saveLocation;
                dlImage.graylist = Settings.Default.graylist;
                dlImage.blacklist = Settings.Default.blacklist;
                dlImage.separateRatings = true;
                dlImage.separateBlacklisted = true;
                dlImage.saveInfo = true;
                dlImage.ignoreFinish = false;
                dlImage.fileNameCode = 1;
                dlImage.downloadImage("https://e621.net/post/show/1551585/2016-absurd_res-anthro-clothing-clouded_leopard-di");
            }
            return;
        }
    #endregion
    }
}
