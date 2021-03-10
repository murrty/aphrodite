using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace aphrodite {
    class ImageDownloader {
    #region Variables
        public string url;                      // The URL of the file to be downloaded.
        public string postID;                   // The ID of the post.

        public string header;                   // String for the header.
        public string saveTo;                   // String for the save directory.
        public string staticSaveTo;             // String for the initial saveTo string.
        public string graylist;                 // String for the graylist.
        public string blacklist;                // String for the blacklist.

        public bool separateRatings;            // Setting for separating ratings.
        public bool separateBlacklisted;        // Setting for separating blacklisted files.
        public bool separateNonImages;          // Setting for separating non images (gif, apng, mp4, webm, swf)

        public bool saveInfo;                   // Global setting for saving images.nfo file.
        public bool ignoreFinish;               // Global setting for exiting after finishing.

        public bool separateArtists;            // Setting to separate files by artist.
        public string fileNameSchema;           // The schema used for the file name.
                                                    // %md5%        = the md5 of the file
                                                    // %id%         = the id of the page
                                                    // %rating%     = the rating of the image (eg: safe)
                                                    // %rating2%    = the lettered rating of the image (eg: s)
                                                    // %artist%     = the first artist in the artists array
                                                    // %ext%        = the extension
                                                    // %fav_count%  = the amount of favorites the post has
                                                    // %score%      = the score of the post
                                                    // %author%     = the user who submitted the post to e621
    #endregion

    #region Downloader
        public bool downloadImage() {
            try {
            // Set the saveto to \\Images.
                if (!saveTo.EndsWith("\\Images"))
                    saveTo += "\\Images";

                staticSaveTo = saveTo;

            // Check the start URL to add the extra forward-slash for the split.
                if (!string.IsNullOrEmpty(url)) {
                    if (url.StartsWith("http://")) {
                        url.Replace("http://", "https://");
                    }
                    if (!url.StartsWith("https://")) {
                        url = "https://" + url;
                    }
                }

            // Get postID from the split (if exists).
                if (string.IsNullOrEmpty(postID)) {
                    postID = apiTools.GetPostIdFromUrl(url);
                }

            // New varaibles for the API parse.
                List<string> GraylistedTags = new List<string>(graylist.Split(' '));
                List<string> BlacklistedTags = new List<string>(blacklist.Split(' '));
                string imageInfo = string.Empty;
                string blacklistInfo = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                string postJson = string.Format("https://e621.net/posts/{0}.json", postID);

            // Begin to get the XML
                url = postJson;
                string postXML = apiTools.GetJSON(postJson, header);

            // Check the XML.
                if (postXML == apiTools.EmptyXML || string.IsNullOrWhiteSpace(postXML)) {
                    apiTools.SendDebugMessage("Xml is empty, aborting.");
                    return false;
                }

            // Begin parsing XML.
                xmlDoc.LoadXml(postXML);
                apiTools.SendDebugMessage("Gathering post information from XML");
                XmlNodeList xmlID = xmlDoc.DocumentElement.SelectNodes("/root/post/id");
                XmlNodeList xmlMD5 = xmlDoc.DocumentElement.SelectNodes("/root/post/file/md5");
                XmlNodeList xmlExt = xmlDoc.DocumentElement.SelectNodes("/root/post/file/ext");
                XmlNodeList xmlURL = xmlDoc.DocumentElement.SelectNodes("/root/post/file/url");
                XmlNodeList xmlTagsGeneral = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/general");
                XmlNodeList xmlTagsSpecies = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/species");
                XmlNodeList xmlTagsCharacter = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/character");
                XmlNodeList xmlTagsCopyright = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/copyright");
                XmlNodeList xmlTagsArtist = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/artist");
                XmlNodeList xmlTagsInvalid = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/invalid");
                XmlNodeList xmlTagsLore = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/lore");
                XmlNodeList xmlTagsMeta = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/meta");
                XmlNodeList xmlTagsLocked = xmlDoc.DocumentElement.SelectNodes("/root/post/locked_tags");
                XmlNodeList xmlScoreUp = xmlDoc.DocumentElement.SelectNodes("/root/post/score/up");
                XmlNodeList xmlScoreDown = xmlDoc.DocumentElement.SelectNodes("/root/post/score/down");
                XmlNodeList xmlScoreTotal = xmlDoc.DocumentElement.SelectNodes("/root/post/score/total");
                XmlNodeList xmlRating = xmlDoc.DocumentElement.SelectNodes("/root/post/rating");
                XmlNodeList xmlFavCount = xmlDoc.DocumentElement.SelectNodes("/root/post/fav_count");
                XmlNodeList xmlAuthor = xmlDoc.DocumentElement.SelectNodes("/root/post/uploader_id");
                XmlNodeList xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/post/description");
                XmlNodeList xmlDeleted = xmlDoc.DocumentElement.SelectNodes("/root/post/flags/deleted");

                if (xmlDeleted[0].InnerText.ToLower() == "true") {
                    //return false;
                }

                string DownloadUrl = xmlURL[0].InnerText;
                if (string.IsNullOrEmpty(DownloadUrl)) {
                    DownloadUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[0].InnerText, xmlExt[0].InnerText);
                    if (DownloadUrl == null) {
                        ErrorLog.ReportCustomException("DownloadUrl was still null after attempting to bypass blacklist with MD5", "ImageDownloader.cs");
                        return false;
                    }
                }

                List<string> foundTags = new List<string>();
                string ReadTags = string.Empty;

                ReadTags += "          General: [";
                for (int x = 0; x < xmlTagsGeneral[0].ChildNodes.Count; x++) {
                    foundTags.Add(xmlTagsGeneral[0].ChildNodes[x].InnerText);
                    ReadTags += xmlTagsGeneral[0].ChildNodes[x].InnerText + ", ";
                }
                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                ReadTags += "]\n          Species: [";
                for (int x = 0; x < xmlTagsSpecies[0].ChildNodes.Count; x++) {
                    foundTags.Add(xmlTagsSpecies[0].ChildNodes[x].InnerText);
                    ReadTags += xmlTagsSpecies[0].ChildNodes[x].InnerText + ", ";
                }
                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                ReadTags += "]\n          Character: [";
                for (int x = 0; x < xmlTagsCharacter[0].ChildNodes.Count; x++) {
                    foundTags.Add(xmlTagsCharacter[0].ChildNodes[x].InnerText);
                    ReadTags += xmlTagsCharacter[0].ChildNodes[x].InnerText + ", ";
                }
                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                ReadTags += "]\n          Copyright: [";
                for (int x = 0; x < xmlTagsCopyright[0].ChildNodes.Count; x++) {
                    foundTags.Add(xmlTagsCopyright[0].ChildNodes[x].InnerText);
                    ReadTags += xmlTagsCopyright[0].ChildNodes[x].InnerText + ", ";
                }
                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                ReadTags += "]\n          Artist: [";
                for (int x = 0; x < xmlTagsArtist[0].ChildNodes.Count; x++) {
                    foundTags.Add(xmlTagsArtist[0].ChildNodes[x].InnerText);
                    ReadTags += xmlTagsArtist[0].ChildNodes[x].InnerText + ", ";
                }
                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                ReadTags += "]\n          Invalid: [";
                for (int x = 0; x < xmlTagsInvalid[0].ChildNodes.Count; x++) {
                    foundTags.Add(xmlTagsInvalid[0].ChildNodes[x].InnerText);
                    ReadTags += xmlTagsInvalid[0].ChildNodes[x].InnerText + ", ";
                }
                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                ReadTags += "]\n          Lore: [";
                for (int x = 0; x < xmlTagsLore[0].ChildNodes.Count; x++) {
                    foundTags.Add(xmlTagsLore[0].ChildNodes[x].InnerText);
                    ReadTags += xmlTagsLore[0].ChildNodes[x].InnerText + ", ";
                }
                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                ReadTags += "]\n          Meta: [";
                for (int x = 0; x < xmlTagsMeta[0].ChildNodes.Count; x++) {
                    foundTags.Add(xmlTagsMeta[0].ChildNodes[x].InnerText);
                    ReadTags += xmlTagsMeta[0].ChildNodes[x].InnerText + ", ";
                }
                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                ReadTags += "]\n          Locked tags: [";
                for (int x = 0; x < xmlTagsLocked[0].ChildNodes.Count; x++) {
                    foundTags.Add(xmlTagsLocked[0].ChildNodes[x].InnerText);
                    ReadTags += xmlTagsLocked[0].ChildNodes[x].InnerText + ", ";
                }
                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                ReadTags += "]\n";

            // Get the rating.
                string rating = xmlRating[0].InnerText;
                switch (rating.ToLower()) {
                    case "e": case "explicit":
                        rating = "Explicit";
                        break;
                    case "q": case "questionable":
                        rating = "Questionable";
                        break;
                    case "s": case "safe":
                        rating = "Safe";
                        break;
                }

            // Set the description
                string imageDescription = " No description";
                if (xmlDescription[0].InnerText != "") {
                    imageDescription = "\n                \"" + xmlDescription[0].InnerText + "\"";
                }

            // Get the artists.
                string foundArtists = string.Empty;
                if (xmlTagsArtist[0].ChildNodes.Count > 0) {
                    for (int i = 0; i < xmlTagsArtist[0].ChildNodes.Count; i++) {
                        foundArtists += xmlTagsArtist[0].ChildNodes[i].InnerText + "\n               ";
                    }
                    foundArtists = foundArtists.TrimEnd(' ');
                    foundArtists = foundArtists.TrimEnd('\n');
                }
                else {
                    foundArtists = "(none)";
                }

            // Read blacklists if it's blacklisted.
                bool isGraylisted = false;
                bool isBlacklisted = false;
                string offendingTags = string.Empty;
                for (int i = 0; i < foundTags.Count; i++) {
                    if (GraylistedTags.Count > 0) {
                        for (int j = 0; j < GraylistedTags.Count; j++) {
                            if (foundTags[i] == GraylistedTags[j]) {
                                offendingTags += foundTags[i];
                                isGraylisted = true;
                            }
                        }
                    }
                // Since the image downloader is user specific, the blacklist won't cancel downloading.
                // Image downloading is basically the user consenting to downloading it, therefore they want it despite the blacklist.
                // Worst-case, just delete it after downloading.
                    if (BlacklistedTags.Count > 0) {
                        for (int j = 0; j < BlacklistedTags.Count; j++) {
                            if (foundTags[i] == BlacklistedTags[j]) {
                                offendingTags += foundTags[i];
                                isGraylisted = true;
                                isBlacklisted = true;
                            }
                        }
                    }
                }

            // set the .nfo buffer.
                if (isGraylisted || isBlacklisted) {
                    if (separateBlacklisted) {
                        blacklistInfo += "BLACKLISTED POST " + postID + ":\n" +
                                         "    MD5: " + xmlMD5[0].InnerText + "\n" +
                                         "    URL: https://e621.net/posts/" + postID + "\n" +
                                         "    ARTIST(S): " + foundArtists + "\n" +
                                         "    TAGS:\n" + ReadTags +//xmlTags[0].InnerText + "\n" +
                                         "    SCORE: Up " + xmlScoreUp[0].InnerText + ", Down " + xmlScoreDown[0].InnerText + ", Total" + xmlScoreTotal[0].InnerText + "\n" +
                                         "    RATING: " + rating + "\n" +
                                         "    OFFENDING TAGS: " + offendingTags + "\n" +
                                         "    DESCRIPITON:" + imageDescription +
                                         "\n\n";
                    }
                    else {
                        imageInfo += "BLACKLISTED POST " + postID + ":\n" +
                                     "    MD5: " + xmlMD5[0].InnerText + "\n" +
                                     "    URL: https://e621.net/posts/" + postID + "\n" +
                                     "    ARTIST(S): " + foundArtists + "\n" +
                                     "    TAGS:\n" + ReadTags +//xmlTags[0].InnerText + "\n" +
                                     "    SCORE: Up " + xmlScoreUp[0].InnerText + ", Down " + xmlScoreDown[0].InnerText + ", Total" + xmlScoreTotal[0].InnerText + "\n" +
                                     "    RATING: " + rating + "\n" +
                                     "    DESCRIPITON:" + imageDescription +
                                     "\n\n";
                    }
                }
                else {
                    imageInfo += "POST " + postID + ":\n" +
                                 "    MD5: " + xmlMD5[0].InnerText + "\n" +
                                 "    URL: https://e621.net/posts/" + postID + "\n" +
                                 "    ARTIST(S): " + foundArtists + "\n" +
                                 "    TAGS:\n" + ReadTags + //xmlTags[0].InnerText + "\n" +
                                 "    SCORE: Up " + xmlScoreUp[0].InnerText + ", Down " + xmlScoreDown[0].InnerText + ", Total" + xmlScoreTotal[0].InnerText + "\n" +
                                 "    RATING: " + rating + "\n" +
                                 "    DESCRIPITON:" + imageDescription +
                                 "\n\n";
                }

            // Trim the excess in the buffer.
                imageInfo = imageInfo.TrimEnd('\n');
                blacklistInfo = blacklistInfo.TrimEnd('\n');

            // Work on the filename
                string fileNameArtist = "unknown";
                bool useHardcodedFilter = false;
                if (string.IsNullOrEmpty(General.Default.undesiredTags))
                    useHardcodedFilter = true;

                if (xmlTagsArtist.Count > 0) {
                    if (!string.IsNullOrEmpty(xmlTagsArtist[0].InnerText)) {
                        for (int i = 0; i < xmlTagsArtist.Count; i++) {
                            if (useHardcodedFilter) {
                                if (!UndesiredTags.isUndesiredHardcoded(xmlTagsArtist[i].InnerText)) {
                                    fileNameArtist = xmlTagsArtist[i].InnerText;
                                    break;
                                }
                            }
                            else {
                                if (!UndesiredTags.isUndesired(xmlTagsArtist[i].InnerText)) {
                                    fileNameArtist = xmlTagsArtist[i].InnerText;
                                    break;
                                }
                            }
                        }
                    }
                }

                string fileName = fileNameSchema.Replace("%md5%", xmlMD5[0].InnerText)
                                                .Replace("%id%", xmlID[0].InnerText)
                                                .Replace("%rating%", rating.ToLower())
                                                .Replace("%rating2%", xmlRating[0].InnerText)
                                                .Replace("%artist%", fileNameArtist)
                                                .Replace("%ext%", xmlExt[0].InnerText)
                                                .Replace("%fav_count%", xmlFavCount[0].InnerText)
                                                .Replace("%score%", xmlScoreTotal[0].InnerText)
                                                .Replace("%scoreup%", xmlScoreUp[0].InnerText)
                                                .Replace("%scoredown%", xmlScoreDown[0].InnerText)
                                                .Replace("%author%", xmlAuthor[0].InnerText) + "." + xmlExt[0].InnerText;

            // Start working on the saveTo string.
                if (separateRatings) {
                    switch (xmlRating[0].InnerText.ToLower()) {
                        case "e": case "explicit":
                            saveTo += "\\explicit";
                            break;
                        case "q": case "questionable":
                            saveTo += "\\questionable";
                            break;
                        case "s": case "safe":
                            saveTo += "\\safe";
                            break;
                    }
                }

                if (isGraylisted || isBlacklisted && separateBlacklisted)
                    saveTo += "\\blacklisted";
                if (separateArtists) {
                    saveTo += "\\" + fileNameArtist;
                }

                switch (separateNonImages) {
                    case true:
                        if (fileName.EndsWith("gif")) {
                            saveTo += "\\gif";
                        }
                        else if (fileName.EndsWith("apng")) {
                            saveTo += "\\apng";
                        }
                        else if (fileName.EndsWith("webm")) {
                            saveTo += "\\webm";
                        }
                        else if (fileName.EndsWith("swf")) {
                            saveTo += "\\swf";
                        }
                        break;
                }

            // Create output directory.
                if (!Directory.Exists(saveTo))
                    Directory.CreateDirectory(saveTo);

            // Check file before continuing.
                if (File.Exists(saveTo + "\\" + fileName))
                    return true;

            // Save image.nfo.
                if (saveInfo) {
                    if (isGraylisted && separateBlacklisted) {
                        if (!File.Exists(staticSaveTo + "\\images.blacklisted.nfo")) {
                            File.WriteAllText(staticSaveTo + "\\images.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        }
                        else {
                            blacklistInfo = "\n\n" + blacklistInfo;

                            string readInfo = File.ReadAllText(staticSaveTo + "\\images.blacklisted.nfo");
                            if (!readInfo.Contains("MD5: " + xmlMD5[0].InnerText))
                                File.AppendAllText(staticSaveTo + "\\images.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        }
                    }
                    else {
                        if (!File.Exists(staticSaveTo + "\\images.nfo")) {
                            File.WriteAllText(staticSaveTo + "\\images.nfo", imageInfo, Encoding.UTF8);
                        }
                        else {
                            imageInfo = "\n\n" + imageInfo;

                            string readInfo = File.ReadAllText(staticSaveTo + "\\images.nfo");
                            if (!readInfo.Contains("MD5: " + xmlMD5[0].InnerText))
                                File.AppendAllText(staticSaveTo + "\\images.nfo", imageInfo, Encoding.UTF8);
                        }
                    }
                }

            // Download file.
                url = DownloadUrl;
                using (ExWebClient wc = new ExWebClient()) {
                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Method = "GET";
                    wc.Headers.Add(header);
                    apiTools.SendDebugMessage("Beginning download of file " + DownloadUrl);

                    wc.DownloadFile(DownloadUrl, saveTo + "\\" + fileName);
                }

                return true;
            }
            catch (ThreadAbortException) {
                apiTools.SendDebugMessage("Thread was requested to be, and has been, aborted. (ImageDownloader.cs)");
                return false;
            }
            catch (ObjectDisposedException) {
                apiTools.SendDebugMessage("An ObjectDiposedException occured. (ImageDownloader.cs)");
                return false;
            }
            catch (WebException WebE) {
                apiTools.SendDebugMessage("A WebException has occured. (ImageDownloader.cs)");
                ErrorLog.ReportWebException(WebE, url, "ImageDownloader.cs");
                return false;
            }
            catch (Exception ex) {
                apiTools.SendDebugMessage("A gneral exception has occured. (ImageDownloader.cs)");
                ErrorLog.ReportException(ex, "ImageDownloader.cs");
                return false;
            }
        }
    #endregion
    }
}