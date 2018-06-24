using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace aphrodite_min {
    class TagDownloader {
        #region Variables
        public bool debugMode = false;
        public bool minMode = false;

        public string tags = string.Empty;          // The tags that will be downloaded.
        public string staticTags = string.Empty;    // Static string for the tags. Only sets input tags.
        public string downloadUrl = string.Empty;   // The URL that will be downloaded.
        public string webHeader = string.Empty;     // The WebClient header.

        public string graylist = string.Empty;      // The graylist (Will be downloaded into separate folder).
        public string blacklist = string.Empty;     // The blacklist (Will never be downloaded or saved into tags.nfo).

        public bool fromURL = false;                // Determines if the page is being downloaded or not, downloadURL must be set.

        public string saveTo = String.Empty;        // Global setting for the save directory for the downloader.
        public bool saveInfo = false;               // Global etting for saving the info.
        public bool openAfter = false;              // Global setting for opening the folder after download.
        public bool saveBlacklistedFiles = true;    // Global setting for saving graylisted files..
        public bool ignoreFinish = false;           // Global setting for closing after finishing

        public bool useMinimumScore = false;        // Setting for using minimum score. Must have minimumScore set.
        public bool scoreAsTag = false;             // Setting for using the score as a tag. Requires useMinimumScore = true.
        public int minimumScore = 0;                // Setting for the minimum score. Requires useMinimumScore = true.
        public int imageLimit = 0;                  // Setting for image limit. 0 = no limit.
        public bool usePageLimit = false;           // Setting for using page limit. Must have pageLimit set.
        public int pageLimit = 0;                   // Setting for page limit. Requires usePageLimit = true.
        public bool separateRatings = true;         // Setting for separating the ratings.
        public string[] ratings = null;             // Setting for the ratings. Requires separateRatings = true.

        public static readonly string tagJson = "https://e621.net/post/index.json?tags=";   // API url.
        public static readonly string limitJson = "&limit=";                             // Maximum limit suffix
        public static readonly string pageJson = "&page=";                                  // Page suffix.

        public static readonly string[] badFolderChars = new string[] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };                           // The characters forbidden in folder names.
        public static readonly string[] replacementFolderChars = new string[] { "%5C", "%2F", "%3A", "%2A", "%3F", "%22", "%3C", "%3E", "%7C" };   // Replacement for the forbidden names.

        //       \ = %5C
        //       / = %2F
        //       : = %3A
        //       * = %2A
        //       ? = %3F
        //       " = %22
        //       < = %3C
        //       > = %3E
        //       | = %7C
        // (space) = %20

        int cleanTotalCount = 0;            // Will be the count of how many files that are set for download.
        int cleanExplicitCount = 0;         // Will be the count of how many explicit files that are set for download.
        int cleanQuestionableCount = 0;     // Will be the count of how many questionable files that are set for download.
        int cleanSafeCount = 0;             // Will be the count of how many safe files that are set for download.

        int graylistTotalCount = 0;         // Will be the count of how many graylisted files that are set for download.
        int graylistExplicitCount = 0;      // Will be the count of how many explicit graylisted files that are set for download.
        int graylistQuestionableCount = 0;  // Will be the count of how many questionable graylisted files that are set for download.
        int graylistSafeCount = 0;          // Will be the count of how many safe graylisted files that are set for download.

        int blacklistCount = 0;             // Will be the count of how many blacklisted files that will be skipped.
        int totalCount = 0;                 // Will be the count of how many files that were parsed.
        #endregion

        public bool downloadTags() {
            if (minMode)
                Console.Clear();

                writeToConsole("Awaiting API call");

            staticTags = tags;                                              // Static tags, set once.
            string url = string.Empty;                                      // The URL being accessed, changes per API call/File download.
            string tagInfo = string.Empty;                                  // The buffer for the 'tag.nfo' file that will be created.
            string blacklistInfo = string.Empty;                            // The buffer for the 'tag.blacklisted.nfo' file that will be created.

            string xml = string.Empty;                                      // The XML string.

            List<string> GraylistedTags = new List<string>();               // The list of files that will be downloaded into a separate folder (if saveBlacklisted = true).
            List<string> BlacklistedTags = new List<string>();              // The list of files that will be skipped entirely.

            List<string> URLs = new List<string>();                         // The URLs that will be downloaded (if separateRatings = false).
            List<string> GraylistedURLs = new List<string>();              // The Blacklisted URLs that will be downloaded (if separateRatings = false).

            List<string> ExplicitURLs = new List<string>();                 // The list of Explicit files.
            List<string> QuestionableURLs = new List<string>();             // The list of Questionable files.
            List<string> SafeURLs = new List<string>();                     // The list of Safe files.
            List<string> GraylistedExplicitURLs = new List<string>();       // The list of Graylisted Explicit files.
            List<string> GraylistedQuestionableURLs = new List<string>();   // The list of Graylisted Questionable files.
            List<string> GraylistedSafeURLs = new List<string>();           // The list of Graylisted Safe files.

            int tagLength = 0;                                              // Will be the count of tags being downloaded (1-6).
            int itemCount = 0;                                              // Will be the count of the images on a page.
            int pageCount = 1;                                              // Will be the count of pages. 1 = the first page.

            bool morePages = false;                                         // Will determine if there are more than 1 page.

            try {
            // Set the saveTo.
                string newTagName = tags;
                for (int i = 0; i < badFolderChars.Length; i++)                                        // Replace bad characters (if present).
                    newTagName = newTagName.Replace(badFolderChars[i], replacementFolderChars[i]);
                if (useMinimumScore)                                                                    // Add minimum score to folder name.
                    newTagName += " (scores " + (minimumScore) + "+)";
                if (!this.saveTo.EndsWith("\\Tags\\" + newTagName))                                     // Set the output folder.
                    this.saveTo += "\\Tags\\" + newTagName;

            // Start the buffer for the .nfo files.
                if (useMinimumScore) {
                    tagInfo = "TAGS: " + tags + "\nMINIMUM SCORE: " + minimumScore + "\n\n";
                    blacklistInfo = "TAGS: " + tags + "\nBLACKLISTED TAGS: " + graylist + "\nMINIMUM SCORE: " + minimumScore + "\n\n";
                }
                else {
                    tagInfo = "TAGS: " + tags + "\nMINIMUM SCORE: n/a\n\n";
                    blacklistInfo = "TAGS: " + tags + "\nBLACKLISTED TAGS: " + graylist + "\nMINIMUM SCORE: n/a\n\n";
                }

            // Get the tag length.
                tagLength = tags.Split(' ').Length;

            // Add the minimum score to the search tags (if applicable).
                if (tagLength < 6 && scoreAsTag) {
                    tags += " score:>" + (minimumScore - 1);
                }

            // Set the blacklist.. lists...
                if (!string.IsNullOrWhiteSpace(graylist))
                    GraylistedTags = graylist.Split(' ').ToList();
                if (!string.IsNullOrWhiteSpace(blacklist))
                    BlacklistedTags = blacklist.Split(' ').ToList();

            // Set the API limit.
                tags += "&limit=320";

            // Get XML of page.
                writeToConsole("Downloading tag information for page 1...");
                url = tagJson + tags;
                xml = apiTools.getJSON(url, webHeader);
                if (xml == null)
                    return false;

            // Parse the XML file.
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList xmlID = doc.DocumentElement.SelectNodes("/root/item/id");
                XmlNodeList xmlMD5 = doc.DocumentElement.SelectNodes("/root/item/md5");
                XmlNodeList xmlURL = doc.DocumentElement.SelectNodes("/root/item/file_url");
                XmlNodeList xmlArtist = doc.DocumentElement.SelectNodes("/root/item/artist");
                XmlNodeList xmlTags = doc.DocumentElement.SelectNodes("/root/item/tags");
                XmlNodeList xmlScore = doc.DocumentElement.SelectNodes("/root/item/score");
                XmlNodeList xmlRating = doc.DocumentElement.SelectNodes("/root/item/rating");
                XmlNodeList xmlDescription = doc.DocumentElement.SelectNodes("/root/item/description");

            // Determine the pages by counting the posts.
                itemCount = xmlID.Count;
                if (itemCount > 320) {
                    morePages = true;
                    pageCount++;
                }

            // Begin parsing the XML for tag information per item.
                for (int i = 0; i < xmlID.Count; i++) {
                    string artists = string.Empty;                                          // The artists that worked on the file.
                    string rating = xmlRating[i].InnerText;                                 // Get the rating of the current file.
                    bool isGraylisted = false;                                              // Will determine if the file is graylisted.
                    bool isBlacklisted = false;                                             // Will determine if the file is blacklisted.
                    List<string> foundTags = xmlTags[i].InnerText.Split(' ').ToList();      // Get the entire tag list of the file.
                    string foundGraylistedTags = string.Empty;                              // The buffer for the tags that are graylisted.

                // Check the image limit & break if reached.
                    if (imageLimit > 0 && imageLimit == totalCount)
                        break;

                // Set the rating for the .nfo file.
                    if (rating == "e")
                        rating = "Explicit";
                    else if (rating == "q")
                        rating = "Questionable";
                    else if (rating == "s")
                        rating = "Safe";
                    else
                        rating = "Unknown";

                // Gets the artists of the file and then trims the end of garbage.
                    for (int j = 0; j < xmlArtist[i].ChildNodes.Count; j++)
                        artists += xmlArtist[i].ChildNodes[j].InnerText + "\n               ";
                    artists = artists.TrimEnd(' ');
                    artists = artists.TrimEnd('\n');

                // Check for blacklisted and graylisted tags.
                    for (int j = 0; j < foundTags.Count; j++) {
                        if (BlacklistedTags.Count > 0) {
                            for (int k = 0; k < BlacklistedTags.Count; k++) {
                                if (foundTags[j] == BlacklistedTags[k]) {
                                    isBlacklisted = true;
                                    break;
                                }
                            }
                        }

                        if (isBlacklisted)
                            continue;

                        if (GraylistedTags.Count > 0) {
                            for (int k = 0; k < GraylistedTags.Count; k++) {
                                if (foundTags[j] == GraylistedTags[k]) {
                                    isGraylisted = true;
                                    foundGraylistedTags += GraylistedTags[k] + " ";
                                }
                            }
                        }
                    }

                // Add to the counts (and break for blacklisted)
                    if (isBlacklisted) {
                        blacklistCount++;
                    }
                    else if (isGraylisted) {
                        graylistTotalCount++;
                        switch (xmlRating[i].InnerText.ToLower()) {
                            case "e":
                                graylistExplicitCount++;
                                break;
                            case "q":
                                graylistQuestionableCount++;
                                break;
                            case "s":
                                graylistSafeCount++;
                                break;
                        }
                    }
                    else {
                        cleanTotalCount++;
                        switch (xmlRating[i].InnerText.ToLower()) {
                            case "e":
                                cleanExplicitCount++;
                                break;
                            case "q":
                                cleanQuestionableCount++;
                                break;
                            case "s":
                                cleanSafeCount++;
                                break;
                        }
                    }
                    totalCount++;

                // There's a blacklisted tag, so skip it.
                    if (isBlacklisted)
                        continue;

                // Graylist check & options check
                    if (isGraylisted) {
                        if (saveBlacklistedFiles) {
                            if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                                continue;

                            if (!ratings.Any(xmlRating[i].InnerText.Contains))
                                continue;
                        }
                        else {
                            continue;
                        }
                    }
                    else { // Not blacklisted
                        if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                            continue;

                        if (!ratings.Any(xmlRating[i].InnerText.Contains))
                            continue;
                    }

                // Start adding to the nfo buffer and URL lists
                    if (isGraylisted && saveBlacklistedFiles) {
                        if (separateRatings) {
                            if (xmlRating[i].InnerText == "e")
                                GraylistedExplicitURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "q")
                                GraylistedQuestionableURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "s")
                                GraylistedSafeURLs.Add(xmlURL[i].InnerText);
                        }
                        else {
                            GraylistedURLs.Add(xmlURL[i].InnerText);
                        }

                        blacklistInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n    OFFENDING TAGS: " + foundGraylistedTags + "\n\n";
                    }
                    else {
                        if (separateRatings) {
                            if (xmlRating[i].InnerText == "e")
                                ExplicitURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "q")
                                QuestionableURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "s")
                                SafeURLs.Add(xmlURL[i].InnerText);
                        }
                        else {
                            URLs.Add(xmlURL[i].InnerText);
                        }

                        tagInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n\n";
                    }
                }

            // Check for extra pages and then parse them as well.
                if (morePages) {
                    bool deadPage = false;
                    while (!deadPage) {
                        writeToConsole("Downloading tag information for page " + (pageCount) + "...");
                        url = tagJson + tags + limitJson + "320" + pageJson + pageCount;
                        xml = apiTools.getJSON(tagJson + tags + limitJson + "320" + pageJson + pageCount, webHeader);

                        if (usePageLimit && pageCount == pageLimit + 1)
                            break;

                        if (xml == apiTools.emptyXML) {
                            deadPage = true;
                            break;
                        }

                        // Everything below here is basically a copy & paste from above.
                        doc.LoadXml(xml);
                        xmlID = doc.DocumentElement.SelectNodes("/root/item/id");
                        xmlMD5 = doc.DocumentElement.SelectNodes("/root/item/md5");
                        xmlURL = doc.DocumentElement.SelectNodes("/root/item/file_url");
                        xmlArtist = doc.DocumentElement.SelectNodes("/root/item/artist");
                        xmlTags = doc.DocumentElement.SelectNodes("/root/item/tags");
                        xmlScore = doc.DocumentElement.SelectNodes("/root/item/score");
                        xmlRating = doc.DocumentElement.SelectNodes("/root/item/rating");
                        xmlDescription = doc.DocumentElement.SelectNodes("/root/item/description");

                        for (int i = 0; i < xmlID.Count; i++) {
                            string artists = string.Empty;
                            string rating = xmlRating[i].InnerText;
                            bool isGraylisted = false;
                            bool isBlacklisted = false;
                            List<string> foundTags = xmlTags[i].InnerText.Split(' ').ToList();
                            string foundGraylistedTags = string.Empty;
                            if (imageLimit > 0 && imageLimit == totalCount)
                                break;

                            if (rating == "e")
                                rating = "Explicit";
                            else if (rating == "q")
                                rating = "Questionable";
                            else if (rating == "s")
                                rating = "Safe";
                            else
                                rating = "Unknown";

                            for (int j = 0; j < xmlArtist[i].ChildNodes.Count; j++)
                                artists += xmlArtist[i].ChildNodes[j].InnerText + "\n               ";
                            artists = artists.TrimEnd(' ');
                            artists = artists.TrimEnd('\n');

                            for (int j = 0; j < foundTags.Count; j++) {
                                if (BlacklistedTags.Count > 0) {
                                    for (int k = 0; k < BlacklistedTags.Count; k++) {
                                        if (foundTags[j] == BlacklistedTags[k]) {
                                            isBlacklisted = true;
                                            break;
                                        }
                                    }
                                }

                                if (isBlacklisted)
                                    break;

                                if (GraylistedTags.Count > 0) {
                                    for (int k = 0; k < GraylistedTags.Count; k++) {
                                        if (foundTags[j] == GraylistedTags[k]) {
                                            isGraylisted = true;
                                            foundGraylistedTags += GraylistedTags[k] + " ";
                                        }
                                    }
                                }
                            }

                            if (isBlacklisted) {
                                blacklistCount++;
                            }
                            else if (isGraylisted) {
                                graylistTotalCount++;
                                switch (xmlRating[i].InnerText.ToLower()) {
                                    case "e":
                                        graylistExplicitCount++;
                                        break;
                                    case "q":
                                        graylistQuestionableCount++;
                                        break;
                                    case "s":
                                        graylistSafeCount++;
                                        break;
                                }
                            }
                            else {
                                cleanTotalCount++;
                                switch (xmlRating[i].InnerText.ToLower()) {
                                    case "e":
                                        cleanExplicitCount++;
                                        break;
                                    case "q":
                                        cleanQuestionableCount++;
                                        break;
                                    case "s":
                                        cleanSafeCount++;
                                        break;
                                }
                            }
                            totalCount++;

                            if (isBlacklisted)
                                continue;

                            if (isGraylisted) {
                                if (saveBlacklistedFiles) {
                                    if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                                        continue;

                                    if (!ratings.Any(xmlRating[i].InnerText.Contains))
                                        continue;
                                }
                                else {
                                    continue;
                                }
                            }
                            else {
                                if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                                    continue;

                                if (!ratings.Any(xmlRating[i].InnerText.Contains))
                                    continue;
                            }
                            if (isGraylisted && saveBlacklistedFiles) {
                                if (separateRatings) {
                                    if (xmlRating[i].InnerText == "e")
                                        GraylistedExplicitURLs.Add(xmlURL[i].InnerText);
                                    else if (xmlRating[i].InnerText == "q")
                                        GraylistedQuestionableURLs.Add(xmlURL[i].InnerText);
                                    else if (xmlRating[i].InnerText == "s")
                                        GraylistedSafeURLs.Add(xmlURL[i].InnerText);
                                }
                                else {
                                    GraylistedURLs.Add(xmlURL[i].InnerText);
                                }

                                blacklistInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n    OFFENDING TAGS: " + foundGraylistedTags + "\n\n";
                            }
                            else {
                                if (separateRatings) {
                                    if (xmlRating[i].InnerText == "e")
                                        ExplicitURLs.Add(xmlURL[i].InnerText);
                                    else if (xmlRating[i].InnerText == "q")
                                        QuestionableURLs.Add(xmlURL[i].InnerText);
                                    else if (xmlRating[i].InnerText == "s")
                                        SafeURLs.Add(xmlURL[i].InnerText);
                                }
                                else {
                                    URLs.Add(xmlURL[i].InnerText);
                                }

                                tagInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n\n";
                            }
                        }

                        pageCount++;
                    }
                }

            // Set the file count for the console.
                string countBuffer = "";
                if (separateRatings) {
                    object[] counts = new object[] { cleanTotalCount.ToString(), cleanExplicitCount.ToString(), cleanQuestionableCount.ToString(), cleanSafeCount.ToString(), graylistTotalCount.ToString(), graylistExplicitCount.ToString(), graylistQuestionableCount.ToString(), graylistSafeCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                    countBuffer = string.Format("\n{0} files, {1} explicited, {2} questionable, {3} safe\n{4} blacklisted files, {5} explicit, {6} questionable, {7} safe\n{8} skipped (has zero-tolerance tags)\n{9} files in total", counts);
                }
                else {
                    object[] counts = new object[] { cleanTotalCount.ToString(), graylistTotalCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                    countBuffer = string.Format("\n{0} files\n{1} on the graylist\n{2} on the blacklist (zero-tolerance)\n{3} files in total", counts);
                }
                writeToConsole(countBuffer, true);


            // Create output folders
                if (separateRatings) {
                    if (ExplicitURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\explicit"))
                            Directory.CreateDirectory(saveTo + "\\explicit");
                    if (QuestionableURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\questionable"))
                            Directory.CreateDirectory(saveTo + "\\questionable");
                    if (SafeURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\safe"))
                            Directory.CreateDirectory(saveTo + "\\safe");

                    if (saveBlacklistedFiles) {
                        if (GraylistedExplicitURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\explicit\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\explicit\\blacklisted");
                        if (GraylistedQuestionableURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\questionable\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\questionable\\blacklisted");
                        if (GraylistedSafeURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\safe\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\safe\\blacklisted");
                    }
                }
                else {
                    if (!Directory.Exists(saveTo))
                        Directory.CreateDirectory(saveTo);

                    if (saveBlacklistedFiles && GraylistedURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\blacklisted"))
                            Directory.CreateDirectory(saveTo + "\\blacklisted");
                }

            // Save the info files from the buffer
                if (saveInfo) {
                    tagInfo.TrimEnd('\n');
                    File.WriteAllText(saveTo + "\\tags.nfo", tagInfo, Encoding.UTF8);

                    if (saveBlacklistedFiles) {
                        blacklistInfo.TrimEnd('\n');
                        if (separateRatings)
                            File.WriteAllText(saveTo + "\\tags.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        else
                            File.WriteAllText(saveTo + "\\blacklisted\\tags.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                    }
                }


            // Start the download
                string outputBar = "";  // The progress bar on the webclient download.
                int TotalFiles = (ExplicitURLs.Count + QuestionableURLs.Count + SafeURLs.Count + GraylistedExplicitURLs.Count + GraylistedQuestionableURLs.Count + GraylistedSafeURLs.Count + URLs.Count + GraylistedURLs.Count) - 1;
                using (ExWebClient wc = new ExWebClient()) {
                    wc.DownloadProgressChanged += (s, e) => {
                        int prog = e.ProgressPercentage;
                        if (prog < 1)
                            outputBar = "|----------| ";
                        else if (prog > 10 && prog < 20)
                            outputBar = "|#---------| ";
                        else if (prog > 20 && prog < 30)
                            outputBar = "|##--------| ";
                        else if (prog > 30 && prog < 40)
                            outputBar = "|###-------| ";
                        else if (prog > 40 && prog < 50)
                            outputBar = "|####------| ";
                        else if (prog > 50 && prog < 60)
                            outputBar = "|#####-----| ";
                        else if (prog > 60 && prog < 70)
                            outputBar = "|######----| ";
                        else if (prog > 70 && prog < 80)
                            outputBar = "|#######---| ";
                        else if (prog > 80 && prog < 90)
                            outputBar = "|########--| ";
                        else if (prog > 90 && prog <= 100)
                            outputBar = "|#########-| ";
                        Console.Write("\r{0} {1}% completed     ", outputBar, prog);
                    };
                    wc.DownloadFileCompleted += (s, e) => {
                        lock (e.UserState) {
                            Console.Write("\r|##########| Download completed. {0} files left.               ", TotalFiles);
                            Monitor.Pulse(e.UserState);
                        }
                    };

                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add(webHeader);
                    wc.Method = "GET";

                    if (separateRatings) {
                        if (ExplicitURLs.Count > 0) {
                            for (int y = 0; y < ExplicitURLs.Count; y++) {
                                url = ExplicitURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\explicit\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\explicit\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }

                        if (QuestionableURLs.Count > 0) {
                            for (int y = 0; y < QuestionableURLs.Count; y++) {
                                url = QuestionableURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\questionable\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\questionable\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }

                        if (SafeURLs.Count > 0) {
                            for (int y = 0; y < SafeURLs.Count; y++) {
                                url = SafeURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\safe\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\safe\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }

                        if (saveBlacklistedFiles) {
                            if (GraylistedExplicitURLs.Count > 0) {
                                for (int y = 0; y < GraylistedExplicitURLs.Count; y++) {
                                    url = GraylistedExplicitURLs[y];

                                    string filename = url.Split('/')[6];
                                    if (!File.Exists(saveTo + "\\explicit\\blacklisted\\" + filename)) {
                                        var sync = new Object();
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(url), saveTo + "\\explicit\\blacklisted\\" + filename, sync);
                                            Monitor.Wait(sync);
                                        }
                                    }

                                    TotalFiles--;
                                }
                            }
                        }

                        if (GraylistedQuestionableURLs.Count > 0) {
                            for (int y = 0; y < GraylistedQuestionableURLs.Count; y++) {
                                url = GraylistedQuestionableURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\questionable\\blacklisted\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\questionable\\blacklisted\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }

                        if (GraylistedSafeURLs.Count > 0) {
                            for (int y = 0; y < GraylistedSafeURLs.Count; y++) {
                                url = GraylistedSafeURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\safe\\blacklisted\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\safe\\blacklisted\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }
                    }
                    else {
                        if (URLs.Count > 0) {
                            for (int y = 0; y < URLs.Count; y++) {
                                url = URLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }

                        if (saveBlacklistedFiles && GraylistedURLs.Count > 0) {
                            for (int y = 0; y < GraylistedURLs.Count; y++) {
                                url = GraylistedURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\blacklisted\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\blacklisted\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }
                    }
                }

                int count = 0;
                if (separateRatings) {
                    count += (ExplicitURLs.Count + QuestionableURLs.Count + SafeURLs.Count);
                    if (saveBlacklistedFiles)
                        count += (GraylistedExplicitURLs.Count + GraylistedQuestionableURLs.Count + GraylistedSafeURLs.Count);
                }
                else {
                    count += URLs.Count;
                    if (saveBlacklistedFiles)
                        count += GraylistedURLs.Count;
                }
                Console.WriteLine("\n\nTags \"" + staticTags + "\" downloaded succesfully.\nPress any key to return to the menu...");
                Console.ReadKey();
                return true;
            }
            catch (ThreadAbortException thrEx) {
                Debug.Print("Thread was requested to be, and has been, aborted.");
                Debug.Print("========== BEGIN THREADABORTEXCEPTION ==========");
                Debug.Print(thrEx.ToString());
                Debug.Print("========== END THREADABORTEXCEPTION ==========");
                return false;
                throw thrEx;
            }
            catch (WebException WebE) {
                Debug.Print("A WebException has occured.");
                Debug.Print("========== BEGIN WEBEXCEPTION ==========");
                Debug.Print(WebE.ToString());
                Debug.Print("========== END WEBEXCEPTION ==========");
                apiTools.webError(WebE, url);
                return false;
                throw WebE;
            }
            catch (Exception ex) {
                Debug.Print("A gneral exception has occured.");
                Debug.Print("========== BEGIN EXCEPTION ==========");
                Debug.Print(ex.ToString());
                Debug.Print("========== END EXCEPTION ==========");
                return false;
                throw ex;
            }
        }
        public bool downloadPage(string pageURL) {
            string pageNumber = pageURL.Split('/')[5];
            tags = pageURL.Split('/')[6].Split('#')[0].Replace("%20", " ");
            staticTags = tags;

            writeToConsole("Awaiting API call");
            string url = string.Empty;                                      // The URL being accessed, changes per API call/File download.

            string tagInfo = string.Empty;                                  // The buffer for the 'tag.nfo' file that will be created.
            string blacklistInfo = string.Empty;                            // The buffer for the 'tag.blacklisted.nfo' file that will be created.

            string xml = string.Empty;                                      // The XML string.

            List<string> GraylistedTags = new List<string>();               // The list of files that will be downloaded into a separate folder (if saveBlacklisted = true).
            List<string> BlacklistedTags = new List<string>();              // The list of files that will be skipped entirely.

            List<string> URLs = new List<string>();                         // The URLs that will be downloaded (if separateRatings = false).
            List<string> GraylistedURLs = new List<string>();              // The Blacklisted URLs that will be downloaded (if separateRatings = false).

            List<string> ExplicitURLs = new List<string>();                 // The list of Explicit files.
            List<string> QuestionableURLs = new List<string>();             // The list of Questionable files.
            List<string> SafeURLs = new List<string>();                     // The list of Safe files.
            List<string> GraylistedExplicitURLs = new List<string>();       // The list of Graylisted Explicit files.
            List<string> GraylistedQuestionableURLs = new List<string>();   // The list of Graylisted Questionable files.
            List<string> GraylistedSafeURLs = new List<string>();           // The list of Graylisted Safe files.

            int tagLength = 0;                                              // Will be the count of tags being downloaded (1-6).

            try {
                // Set the saveTo.
                string newTagName = tags;
                for (int i = 0; i < badFolderChars.Length; i++)                                             // Replace bad characters (if present).
                    newTagName = newTagName.Replace(badFolderChars[i], replacementFolderChars[i]);
                if (useMinimumScore)                                                                        // Add minimum score to folder name.
                    newTagName += " (scores " + (minimumScore) + "+)";
                if (!this.saveTo.EndsWith("\\Pages\\" + newTagName + " (page " + pageNumber + ")"))   // Set the output folder.
                    this.saveTo += "\\Pages\\" + newTagName + " (page " + pageNumber + ")";

            // Start the buffer for the .nfo files.
                if (useMinimumScore) {
                    tagInfo = "TAGS: " + tags + "\nMINIMUM SCORE: " + minimumScore + "\nDOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n\n";

                    blacklistInfo = "TAGS: " + tags + "\nMINIMUM SCORE: " + minimumScore + "\nDOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\nBLACKLISTED TAGS: " + graylist + "\n\n";
                }
                else {
                    tagInfo = "TAGS: " + tags + "\nMINIMUM SCORE: n/a\nDOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n\n";

                    blacklistInfo = "TAGS: " + tags + "\nMINIMUM SCORE: n/a\nDOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\nBLACKLISTED TAGS: " + graylist + "\n\n";
                }

            // Get the tag length.
                tagLength = tags.Split(' ').Length;

            // Add the minimum score to the search tags (if applicable).
                if (tagLength < 6 && scoreAsTag) {
                    tags += " score:>" + (minimumScore - 1);
                }

            // Set the blacklist.. lists...
                if (!string.IsNullOrWhiteSpace(graylist))
                    GraylistedTags = graylist.Split(' ').ToList();
                if (!string.IsNullOrWhiteSpace(blacklist))
                    BlacklistedTags = blacklist.Split(' ').ToList();

            // Get XML of page.
                writeToConsole("Downloading page information...", true);
                url = tagJson + tags + pageJson + pageNumber;
                xml = apiTools.getJSON(url, webHeader);
                if (xml == null)
                    return false;

            // Parse the XML file.
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList xmlID = doc.DocumentElement.SelectNodes("/root/item/id");
                XmlNodeList xmlMD5 = doc.DocumentElement.SelectNodes("/root/item/md5");
                XmlNodeList xmlURL = doc.DocumentElement.SelectNodes("/root/item/file_url");
                XmlNodeList xmlArtist = doc.DocumentElement.SelectNodes("/root/item/artist");
                XmlNodeList xmlTags = doc.DocumentElement.SelectNodes("/root/item/tags");
                XmlNodeList xmlScore = doc.DocumentElement.SelectNodes("/root/item/score");
                XmlNodeList xmlRating = doc.DocumentElement.SelectNodes("/root/item/rating");
                XmlNodeList xmlDescription = doc.DocumentElement.SelectNodes("/root/item/description");

            // Begin parsing the XML for tag information per item.
                for (int i = 0; i < xmlID.Count; i++) {
                    string artists = string.Empty;                                          // The artists that worked on the file.
                    string rating = xmlRating[i].InnerText;                                 // Get the rating of the current file.
                    bool isGraylisted = false;                                              // Will determine if the file is graylisted.
                    bool isBlacklisted = false;                                             // Will determine if the file is blacklisted.
                    List<string> foundTags = xmlTags[i].InnerText.Split(' ').ToList();      // Get the entire tag list of the file.
                    string foundGraylistedTags = string.Empty;                              // The buffer for the tags that are graylisted.

                // Check the image limit & break if reached.
                    if (imageLimit > 0 && imageLimit == totalCount)
                        break;

                // Set the rating for the .nfo file.
                    if (rating == "e")
                        rating = "Explicit";
                    else if (rating == "q")
                        rating = "Questionable";
                    else if (rating == "s")
                        rating = "Safe";
                    else
                        rating = "Unknown";

                // Gets the artists of the file and then trims the end of garbage.
                    for (int j = 0; j < xmlArtist[i].ChildNodes.Count; j++)
                        artists += xmlArtist[i].ChildNodes[j].InnerText + "\n               ";
                    artists = artists.TrimEnd(' ');
                    artists = artists.TrimEnd('\n');

                // Check for blacklisted and graylisted tags.
                    for (int j = 0; j < foundTags.Count; j++) {
                        if (BlacklistedTags.Count > 0) {
                            for (int k = 0; k < BlacklistedTags.Count; k++) {
                                if (foundTags[j] == BlacklistedTags[k]) {
                                    isBlacklisted = true;
                                    break;
                                }
                            }
                        }

                        if (isBlacklisted)
                            break;

                        if (GraylistedTags.Count > 0) {
                            for (int k = 0; k < GraylistedTags.Count; k++) {
                                if (foundTags[j] == GraylistedTags[k]) {
                                    isGraylisted = true;
                                    foundGraylistedTags += GraylistedTags[k] + " ";
                                }
                            }
                        }
                    }

                // Add to the counts (and break for blacklisted)
                    if (isBlacklisted) {
                        blacklistCount++;
                    }
                    else if (isGraylisted) {
                        graylistTotalCount++;
                        switch (xmlRating[i].InnerText.ToLower()) {
                            case "e":
                                graylistExplicitCount++;
                                break;
                            case "q":
                                graylistQuestionableCount++;
                                break;
                            case "s":
                                graylistSafeCount++;
                                break;
                        }
                    }
                    else {
                        cleanTotalCount++;
                        switch (xmlRating[i].InnerText.ToLower()) {
                            case "e":
                                cleanExplicitCount++;
                                break;
                            case "q":
                                cleanQuestionableCount++;
                                break;
                            case "s":
                                cleanSafeCount++;
                                break;
                        }
                    }
                    totalCount++;

                // There's a blacklisted tag, so skip it.
                    if (isBlacklisted)
                        continue;

                // Graylist check & options check
                    if (isGraylisted) {
                        if (saveBlacklistedFiles) {
                            if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                                continue;

                            if (!ratings.Any(xmlRating[i].InnerText.Contains))
                                continue;
                        }
                        else {
                            continue;
                        }
                    }
                    else { // Not blacklisted
                        if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                            continue;

                        if (!ratings.Any(xmlRating[i].InnerText.Contains))
                            continue;
                    }

                // Start adding to the nfo buffer and URL lists
                    if (isGraylisted && saveBlacklistedFiles) {
                        if (separateRatings) {
                            if (xmlRating[i].InnerText == "e")
                                GraylistedExplicitURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "q")
                                GraylistedQuestionableURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "s")
                                GraylistedSafeURLs.Add(xmlURL[i].InnerText);
                        }
                        else {
                            GraylistedURLs.Add(xmlURL[i].InnerText);
                        }

                        blacklistInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n    OFFENDING TAGS: " + foundGraylistedTags + "\n\n";
                    }
                    else {
                        if (separateRatings) {
                            if (xmlRating[i].InnerText == "e")
                                ExplicitURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "q")
                                QuestionableURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "s")
                                SafeURLs.Add(xmlURL[i].InnerText);
                        }
                        else {
                            URLs.Add(xmlURL[i].InnerText);
                        }

                        tagInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n\n";
                    }
                }

            // Set the file count for the console.
                string countBuffer = "";
                if (separateRatings) {
                    object[] counts = new object[] { cleanTotalCount.ToString(), cleanExplicitCount.ToString(), cleanQuestionableCount.ToString(), cleanSafeCount.ToString(), graylistTotalCount.ToString(), graylistExplicitCount.ToString(), graylistQuestionableCount.ToString(), graylistSafeCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                    countBuffer = string.Format("\n{0} files, {1} explicited, {2} questionable, {3} safe\n{4} blacklisted files, {5} explicit, {6} questionable, {7} safe\n{8} skipped (has zero-tolerance tags)\n{9} files in total", counts);
                }
                else {
                    object[] counts = new object[] { cleanTotalCount.ToString(), graylistTotalCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                    countBuffer = string.Format("\n{0} files\n{1} on the graylist\n{2} on the blacklist (zero-tolerance)\n{3} files in total", counts);
                }
                writeToConsole(countBuffer, true);

            // Create output folders
                if (separateRatings) {
                    if (ExplicitURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\explicit"))
                            Directory.CreateDirectory(saveTo + "\\explicit");
                    if (QuestionableURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\questionable"))
                            Directory.CreateDirectory(saveTo + "\\questionable");
                    if (SafeURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\safe"))
                            Directory.CreateDirectory(saveTo + "\\safe");

                    if (saveBlacklistedFiles) {
                        if (GraylistedExplicitURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\explicit\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\explicit\\blacklisted");
                        if (GraylistedQuestionableURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\questionable\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\questionable\\blacklisted");
                        if (GraylistedSafeURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\safe\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\safe\\blacklisted");
                    }
                }
                else {
                    if (!Directory.Exists(saveTo))
                        Directory.CreateDirectory(saveTo);

                    if (saveBlacklistedFiles && GraylistedURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\blacklisted"))
                            Directory.CreateDirectory(saveTo + "\\blacklisted");
                }

            // Save the info files from the buffer
                if (saveInfo) {
                    tagInfo.TrimEnd('\n');
                    File.WriteAllText(saveTo + "\\tags.nfo", tagInfo, Encoding.UTF8);

                    if (saveBlacklistedFiles && graylistTotalCount > 0) {
                        blacklistInfo.TrimEnd('\n');
                        if (separateRatings)
                            File.WriteAllText(saveTo + "\\tags.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        else
                            File.WriteAllText(saveTo + "\\blacklisted\\tags.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                    }
                }

            // Start the download
                string outputBar = "";  // The progress bar on the webclient download.
                int TotalFiles = (ExplicitURLs.Count + QuestionableURLs.Count + SafeURLs.Count + GraylistedExplicitURLs.Count + GraylistedQuestionableURLs.Count + GraylistedSafeURLs.Count + URLs.Count + GraylistedURLs.Count) - 1;
                using (ExWebClient wc = new ExWebClient()) {
                    wc.DownloadProgressChanged += (s, e) => {
                        int prog = e.ProgressPercentage;
                        if (prog < 1)
                            outputBar = "|----------| ";
                        else if (prog > 10 && prog < 20)
                            outputBar = "|#---------| ";
                        else if (prog > 20 && prog < 30)
                            outputBar = "|##--------| ";
                        else if (prog > 30 && prog < 40)
                            outputBar = "|###-------| ";
                        else if (prog > 40 && prog < 50)
                            outputBar = "|####------| ";
                        else if (prog > 50 && prog < 60)
                            outputBar = "|#####-----| ";
                        else if (prog > 60 && prog < 70)
                            outputBar = "|######----| ";
                        else if (prog > 70 && prog < 80)
                            outputBar = "|#######---| ";
                        else if (prog > 80 && prog < 90)
                            outputBar = "|########--| ";
                        else if (prog > 90 && prog <= 100)
                            outputBar = "|#########-| ";
                        Console.Write("\r{0} {1}% completed     ", outputBar, prog);
                    };
                    wc.DownloadFileCompleted += (s, e) => {
                        lock (e.UserState) {
                            Console.Write("\r|##########| Download completed. {0} files left.               ", TotalFiles);
                            Monitor.Pulse(e.UserState);
                        }
                    };

                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add(webHeader);
                    wc.Method = "GET";

                    if (separateRatings) {
                        if (ExplicitURLs.Count > 0) {
                            for (int y = 0; y < ExplicitURLs.Count; y++) {
                                url = ExplicitURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\explicit\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\explicit\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }

                        if (QuestionableURLs.Count > 0) {
                            for (int y = 0; y < QuestionableURLs.Count; y++) {
                                url = QuestionableURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\questionable\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\questionable\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }

                        if (SafeURLs.Count > 0) {
                            for (int y = 0; y < SafeURLs.Count; y++) {
                                url = SafeURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\safe\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\safe\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }

                        if (saveBlacklistedFiles) {
                            if (GraylistedExplicitURLs.Count > 0) {
                                for (int y = 0; y < GraylistedExplicitURLs.Count; y++) {
                                    url = GraylistedExplicitURLs[y];

                                    string filename = url.Split('/')[6];
                                    if (!File.Exists(saveTo + "\\explicit\\blacklisted\\" + filename)) {
                                        var sync = new Object();
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(url), saveTo + "\\explicit\\blacklisted\\" + filename, sync);
                                            Monitor.Wait(sync);
                                        }
                                    }

                                    TotalFiles--;
                                }
                            }
                        }

                        if (GraylistedQuestionableURLs.Count > 0) {
                            for (int y = 0; y < GraylistedQuestionableURLs.Count; y++) {
                                url = GraylistedQuestionableURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\questionable\\blacklisted\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\questionable\\blacklisted\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }

                        if (GraylistedSafeURLs.Count > 0) {
                            for (int y = 0; y < GraylistedSafeURLs.Count; y++) {
                                url = GraylistedSafeURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\safe\\blacklisted\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\safe\\blacklisted\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }
                    }
                    else {
                        if (URLs.Count > 0) {
                            for (int y = 0; y < URLs.Count; y++) {
                                url = URLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }

                        if (saveBlacklistedFiles && GraylistedURLs.Count > 0) {
                            for (int y = 0; y < GraylistedURLs.Count; y++) {
                                url = GraylistedURLs[y];

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\blacklisted\\" + filename)) {
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\blacklisted\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }

                                TotalFiles--;
                            }
                        }
                    }
                }

                int count = 0;
                if (separateRatings) {
                    count += (ExplicitURLs.Count + QuestionableURLs.Count + SafeURLs.Count);
                    if (saveBlacklistedFiles)
                        count += (GraylistedExplicitURLs.Count + GraylistedQuestionableURLs.Count + GraylistedSafeURLs.Count);
                }
                else {
                    count += URLs.Count;
                    if (saveBlacklistedFiles)
                        count += GraylistedURLs.Count;
                }

                Console.WriteLine("\n\nPage {0} of tags \"{1}\" downloaded succesfully.\nPress any key to return to the menu...", pageNumber, staticTags);
                Console.ReadKey();
                return true;
            }
            catch (ThreadAbortException thrEx) {
                Debug.Print("Thread was requested to be, and has been, aborted.");
                Debug.Print("========== BEGIN THREADABORTEXCEPTION ==========");
                Debug.Print(thrEx.ToString());
                Debug.Print("========== END THREADABORTEXCEPTION ==========");
                return false;
                throw thrEx;
            }
            catch (WebException WebE) {
                Debug.Print("A WebException has occured.");
                Debug.Print("========== BEGIN WEBEXCEPTION ==========");
                Debug.Print(WebE.ToString());
                Debug.Print("========== END WEBEXCEPTION ==========");
                apiTools.webError(WebE, url);
                return false;
                throw WebE;
            }
            catch (Exception ex) {
                Debug.Print("A gneral exception has occured.");
                Debug.Print("========== BEGIN EXCEPTION ==========");
                Debug.Print(ex.ToString());
                Debug.Print("========== END EXCEPTION ==========");
                return false;
                throw ex;
            }
        }

        public void writeToConsole(string message, bool important = false) {
            if (minMode) {
                if (important) {
                    Console.WriteLine(message);
                    return;
                }
                else if (debugMode) {
                    Console.WriteLine(message);
                    return;
                }
                Debug.Print(message);
            }
            //Console.WriteLine(message);
        }
        public void waitInConsole() {
            Console.ReadLine();
        }
    }
}

// i wanna go ooutsiide